using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;

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

        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ViewBag.ExceptionPath = exceptionHandlerPathFeature.Path;
            ViewBag.ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
            ViewBag.StackTrace = exceptionHandlerPathFeature.Error.StackTrace;
            return View("Error");
        }
    }
}
