﻿using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TaskThree.BLL.Interfaces;
using TaskThree.DA.Data;
using TaskThree.DA.Models;
using TaskThree.PL.ViewModels;

namespace TaskThree.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IMapper mapper;

        //private readonly IDepartmentRepository departmentRepository;

        public EmployeeController(IWebHostEnvironment _env, IEmployeeRepository employeeRepository, IMapper mapper /*,IDepartmentRepository departmentRepository*/)
        {
            this._env = _env;
            this.employeeRepository = employeeRepository;
            this.mapper = mapper;
            //this.departmentRepository = departmentRepository;
        }

        public IActionResult Index(string SearchInp)
        {
            var employees = Enumerable.Empty<Employee>();

            if (string.IsNullOrEmpty(SearchInp))
            {
                employees = employeeRepository.GetAll();
            }
            //ViewData["message"] = "Hello ViewData";
            //ViewBag.message = "Hello ViewBag";
            else
            {
                employees = employeeRepository.SearchByName(SearchInp.ToLower());
            }
            var mappedEmployees = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmployees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            //ViewData["Departments"] = departmentRepository.GetAll();
            //ViewBag.Departments = departmentRepository.GetAll(); 
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            //Employee mappedEmployee = (Employee)employee;
            var mappedEmp = mapper.Map<EmployeeViewModel, Employee>(employeeVM);
            if (ModelState.IsValid)
            {
                var count = employeeRepository.Add(mappedEmp);
                if (count > 0)
                    TempData["message"] = "Department is created successfully";
                else
                    TempData["message"] = "An error has occurud while creating department";
                return RedirectToAction(nameof(Index));
            }
            return View(mappedEmp);
        }
        [HttpGet]
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            var emp = employeeRepository.Get(id.Value);
            var mappedEmp = mapper.Map<Employee, EmployeeViewModel>(emp);
            if (emp is null)
            {
                return NotFound();
            }
            return View(ViewName, mappedEmp);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //ViewBag.Departments = departmentRepository.GetAll();
            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
                return View(employeeVM);
            try
            {
                var mappedEmp = mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                employeeRepository.Update(mappedEmp);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, ("An error has been occured during update the employee"));
                return View(employeeVM);
            }
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(EmployeeViewModel employeeVM)
        {
            try
            {
                var mappedEmp = mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                employeeRepository.Delete(mappedEmp);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, ("An error has been occured during update the employee"));
                return View(employeeVM);
            }
        }

    }
}
