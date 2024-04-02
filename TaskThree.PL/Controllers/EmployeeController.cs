using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskThree.BLL.Interfaces;
using TaskThree.BLL.Repositories;
using TaskThree.DA.Data;
using TaskThree.DA.Models;
using TaskThree.PL.Helpers;
using TaskThree.PL.ViewModels;

namespace TaskThree.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IUnitOfWork unitOfWork;
       // private readonly IEmployeeRepository employeeRepository;
        private readonly IMapper mapper;

        //private readonly IDepartmentRepository departmentRepository;

        public EmployeeController(IWebHostEnvironment _env,IUnitOfWork unitOfWork, /*IEmployeeRepository employeeRepository,*/ IMapper mapper /*,IDepartmentRepository departmentRepository*/)
        {
            this._env = _env;
            this.unitOfWork = unitOfWork;
            //this.employeeRepository = employeeRepository;
            this.mapper = mapper;
            //this.departmentRepository = departmentRepository;
        }


        public IActionResult Index(string SearchInp)
        {
            var employees = Enumerable.Empty<Employee>();
            var employeeRepo = unitOfWork.Repository<Employee>() as EmployeeRepository;
            if (string.IsNullOrEmpty(SearchInp))
            {
                //employees = await employeeRepo.GetAllAsync();
                employees = employeeRepo.GetAllAsync();
            }
            //ViewData["message"] = "Hello ViewData";
            //ViewBag.message = "Hello ViewBag";
            else
            {
                employees = employeeRepo.SearchByName(SearchInp.ToLower());
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
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            employeeVM.ImageName = await DocumentSettings.UploadFile(employeeVM.Image, "images");
            //Employee mappedEmployee = (Employee)employee;
            var mappedEmp = mapper.Map<EmployeeViewModel, Employee>(employeeVM);
            if (ModelState.IsValid)
            {
                unitOfWork.Repository<Employee>().Add(mappedEmp);
                var count = await unitOfWork.Complete();
                if (count > 0)
                {
                    TempData["message"] = "Department is created successfully";
                }
                else
                    TempData["message"] = "An error has occurud while creating department";
                return RedirectToAction(nameof(Index));
            }
            return View(mappedEmp);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            var emp = await unitOfWork.Repository<Employee>().GetAsync(id.Value);
            var mappedEmp = mapper.Map<Employee, EmployeeViewModel>(emp);
            if (emp is null)
            {
                return NotFound();
            }
            if (ViewName.Equals(nameof(Delete) , StringComparison.OrdinalIgnoreCase))
            {
                TempData["ImageName"] = emp.ImageName;
            }

			return View(ViewName, mappedEmp);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //ViewBag.Departments = departmentRepository.GetAll();
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
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
                unitOfWork.Repository<Employee>().Update(mappedEmp);
                await unitOfWork.Complete();
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
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM)
        {
            try
            {
                employeeVM.ImageName = TempData["ImageName"] as string;
                var mappedEmp = mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                unitOfWork.Repository<Employee>().Delete(mappedEmp);
                var count = await unitOfWork.Complete();
                if (count > 0)
                {
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "images");
                    return RedirectToAction(nameof(Index));
                }
                return View(employeeVM);
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
