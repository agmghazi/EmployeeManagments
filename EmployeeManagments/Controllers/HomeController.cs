using EmployeeManagments.Models;
using EmployeeManagments.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagments.Controllers
{
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeReposiory;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeReposiory = employeeRepository;
        }
        public ViewResult Index()
        {
            var getallEmployee = _employeeReposiory.GetAllEmployees();
            return View(getallEmployee);
        }

        public ViewResult Details(int? id)
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeReposiory.GetEmployee(id ?? 1),
                PageTitle = "Employee Details"
            };

            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                Employee newEmployee = _employeeReposiory.Add(employee);

                //return RedirectToAction("Details", new { id = newEmployee.Id });
            }

            return View();

        }
    }
}
