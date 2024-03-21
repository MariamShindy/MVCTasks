using Microsoft.AspNetCore.Mvc;
using TaskThree.BLL.Interfaces;
using TaskThree.BLL.Repositories;

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
            return View();
        }
    }
}
