using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using TaskThree.BLL.Interfaces;
using TaskThree.BLL.Repositories;
using TaskThree.DA.Models;
using TaskThree.PL.Models;
using TaskThree.PL.ViewModels;

namespace TaskThree.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        // private readonly IDepartmentRepository _departmentRepo;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper mapper;

        public DepartmentController(/*IDepartmentRepository departmentRepo*/ IUnitOfWork unitOfWork, IWebHostEnvironment env, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            // _departmentRepo = departmentRepo;
            _env = env;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            var departments = unitOfWork.Repository<Department>().GetAll();
            var mappedDepartments = mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return View(mappedDepartments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(DepartmentViewModel departmentVM)
        {
            var mappedDep = mapper.Map<DepartmentViewModel, Department>(departmentVM);
            if (ModelState.IsValid) //server side validation
            {
                unitOfWork.Repository<Department>().Add(mappedDep);
                var count = unitOfWork.Complete();
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(mappedDep);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (/*id is null*/ !id.HasValue)
                return BadRequest();
            var department = unitOfWork.Repository<Department>().Get(id.Value);
            if (department is null)
                return NotFound();
            var mappedDep = mapper.Map<Department, DepartmentViewModel>(department);
            return View(viewName, mappedDep);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
            //if (/*id is null*/ !id.HasValue)
            //    return BadRequest();
            //var department = _departmentRepo.Get(id.Value);
            //if (department is null)
            //    return NotFound();
            //return View(department);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, DepartmentViewModel departmentVM)
        {
            var mappedDep = mapper.Map<DepartmentViewModel, Department>(departmentVM);
            if (id != departmentVM.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(departmentVM);
            }
            try
            {
                unitOfWork.Repository<Department>().Update(mappedDep);
                unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                //log exception
                //friendly message
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, ("An error has been occured during update the department"));
                return View(departmentVM);
            }
        }
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        public IActionResult Delete(DepartmentViewModel departmentVM)
        {
            try
            {
                var mappedDep = mapper.Map<DepartmentViewModel, Department>(departmentVM);
                unitOfWork.Repository<Department>().Delete(mappedDep);
                unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, ("An error has been occured during update the department"));
                return View(departmentVM);
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
