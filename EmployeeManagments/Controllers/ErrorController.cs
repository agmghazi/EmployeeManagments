using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagments.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statuseCode}")]
        public IActionResult HttpStatuseCodeHandler(int statuseCode)
        {
            switch (statuseCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "sorry, the resource you request not found";
                    break;
            }
            return View("NotFound");
        }
    }
}
