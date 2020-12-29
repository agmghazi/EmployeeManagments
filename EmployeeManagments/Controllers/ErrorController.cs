using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace EmployeeManagments.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }
        [Route("Error/{statuseCode}")]
        public IActionResult HttpStatuseCodeHandler(int statuseCode)
        {
            var statuseResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statuseCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "sorry, the resource you request not found";
                   _logger.LogWarning($"404 Error Occurred. path = {statuseResult.OriginalPath}" +
                                      $"and QueryString = {statuseResult.OriginalQueryString}");
                    break;
            }
            return View("NotFound");
        }

        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

           _logger.LogError($"The path {exceptionHandlerPathFeature.Path} threw an exception {exceptionHandlerPathFeature.Error}");
            return View("Error");
        }
    }
}
