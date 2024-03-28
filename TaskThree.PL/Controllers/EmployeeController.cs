using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using TaskThree.BLL.Interfaces;
using TaskThree.DA.Data;
using TaskThree.DA.Models;

namespace TaskThree.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IEmployeeRepository employeeRepository;
		//private readonly IDepartmentRepository departmentRepository;

		public EmployeeController(IWebHostEnvironment _env , IEmployeeRepository employeeRepository /*,IDepartmentRepository departmentRepository*/)
        {
            this._env = _env;
            this.employeeRepository = employeeRepository;
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
                 employees =employeeRepository.SearchByName(SearchInp.ToLower());
            }
			return View(employees);
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
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var count = employeeRepository.Add(employee);
                if (count > 0)
                    TempData["message"] = "Department is created successfully";
                else
                    TempData["message"] = "An error has occurud while creating department";
             return RedirectToAction(nameof(Index));
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
            //ViewBag.Departments = departmentRepository.GetAll();
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
