using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel;
using TaskThree.BLL.Interfaces;
using TaskThree.BLL.Repositories;
using TaskThree.DA.Models;
using TaskThree.PL.Models;

namespace TaskThree.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepo;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(IDepartmentRepository departmentRepo,IWebHostEnvironment env)
        {
            _departmentRepo = departmentRepo;
            _env = env;
        }
        public IActionResult Index()
        {
            var departments = _departmentRepo.GetAll();
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid) //server side validation
            {
                var count =_departmentRepo.Add(department);
                if (count > 0)
                    RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        [HttpGet]
        public IActionResult Details(int? id , string viewName = "Details")
        {
            if (/*id is null*/ !id.HasValue)
                return BadRequest();
            var department = _departmentRepo.Get(id.Value);
            if (department is null)
                return NotFound();
            return View(viewName,department);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id,"Edit");
            //if (/*id is null*/ !id.HasValue)
            //    return BadRequest();
            //var department = _departmentRepo.Get(id.Value);
            //if (department is null)
            //    return NotFound();
            //return View(department);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id ,Department department)
        {
            if (id != department.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(department);
            }
            try
            {
                _departmentRepo.Update(department);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                //log exception
                //friendly message
                if (_env.IsDevelopment())
                ModelState.AddModelError(string.Empty , ex.Message);
                else
                    ModelState.AddModelError(string.Empty, ("An error has been occured during update the department"));
             return View(department);
            }
        }
		public IActionResult Delete(int? id)
		{
			return Details(id, "Delete");		
		}
        [HttpPost]
        public IActionResult Delete(Department department)
        {
            try
            {
                _departmentRepo.Delete(department);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, ("An error has been occured during update the department"));
                return View(department);    
               //return View("Error", new ErrorViewModel());
            }
        }

		//      //Delete using modal
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public IActionResult Delete(int id)
		//{
		//	var departmentToDelete = _departmentRepo.Get(id);
		//	if (departmentToDelete == null)
		//	{
		//		return NotFound(); 
		//	}

		//	_departmentRepo.Delete(departmentToDelete); 

		//	return RedirectToAction("Index");
		//}

	}
}
