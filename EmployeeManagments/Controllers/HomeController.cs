using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagments.Models;

namespace EmployeeManagments.Controllers
{
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeReposiory;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeReposiory = employeeRepository;
        }
        public string Index()
        {
            return _employeeReposiory.GetEmployee(1).Name;
        }

        public JsonResult Details()
        {
            Employee employee = _employeeReposiory.GetEmployee(1);
            return Json(employee);
        }
    }
}
