using Microsoft.AspNetCore.Mvc;
using TaskThree.BLL.Interfaces;
using TaskThree.BLL.Repositories;
using TaskThree.DA.Models;

namespace TaskThree.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepo;
        public DepartmentController(IDepartmentRepository departmentRepo)
        {
            _departmentRepo = departmentRepo;
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
        public IActionResult Details(int? id)
        {
            if (/*id is null*/ !id.HasValue)
                return BadRequest();
            var department = _departmentRepo.Get(id.Value);
            if (department is null)
                return NotFound();
            return View(department);
        }

    }
}
