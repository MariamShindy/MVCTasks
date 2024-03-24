using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using TaskThree.BLL.Interfaces;
using TaskThree.DA.Data;
using TaskThree.DA.Models;

namespace TaskThree.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IEmployeeRepository employeeRepository;
        public EmployeeController(IWebHostEnvironment _env , IEmployeeRepository employeeRepository)
        {
            this._env = _env;
            this.employeeRepository = employeeRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var count = employeeRepository.Add(employee);
                if (count > 0)
                {
                    RedirectToAction(nameof(Index));
                }
            }
            return View(employee);
        }
        [HttpGet]
        public IActionResult Details(int ?id , string ViewName ="Details")
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            var emp = employeeRepository.Get(id.Value);
            if (emp is null)
            {
                return NotFound();
            }
            return View(ViewName,emp);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id ,Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
                return View(employee);
            try
            {
                employeeRepository.Update(employee);
                return RedirectToAction(nameof(Index));
            }
            catch(System.Exception ex)
            {
                if(_env.IsDevelopment())
                ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, ("An error has been occured during update the employee"));
                return View(employee);
            }
        }
        [HttpGet]
        public IActionResult Delete(int ?id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete( Employee employee)
        {
            try
            {
                employeeRepository.Delete(employee);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, ("An error has been occured during update the employee"));
                return View(employee);
            }
        }

    }
}
