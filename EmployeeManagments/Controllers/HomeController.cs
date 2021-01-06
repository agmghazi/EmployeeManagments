using EmployeeManagments.Models;
using EmployeeManagments.Security;
using EmployeeManagments.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

namespace EmployeeManagments.Controllers
{
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeReposiory;
        // It is through IDataProtector interface Protect and Unprotect methods,
        // we encrypt and decrypt respectively
        private readonly IDataProtector protector;

        private readonly IHostingEnvironment _hostingEnvironment;

        // It is the CreateProtector() method of IDataProtectionProvider interface
        // that creates an instance of IDataProtector. CreateProtector() requires
        // a purpose string. So both IDataProtectionProvider and the class that
        // contains our purpose strings are injected using the contructor
        public HomeController(IEmployeeRepository employeeRepository,
                IHostingEnvironment hostingEnvironment,
                IDataProtectionProvider dataProtectionProvider,
                DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _employeeReposiory = employeeRepository;
            _hostingEnvironment = hostingEnvironment;
            // Pass the purpose string as a parameter
            this.protector = dataProtectionProvider.CreateProtector(
                dataProtectionPurposeStrings.EmployeeIdRouteValue);
        }
        [AllowAnonymous]
        public ViewResult Index()
        {
            var model = _employeeReposiory.GetAllEmployees()
                .Select(e =>
                {
                    // Encrypt the ID value and store in EncryptedId property
                    e.EncryptedId = protector.Protect(e.Id.ToString());
                    return e;
                });
            return View(model);
        }

        [AllowAnonymous]
        public ViewResult Details(string id)
        {
            int employeeId = Convert.ToInt32(protector.Unprotect(id));

            Employee employee = _employeeReposiory.GetEmployee(employeeId);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", employeeId);
            }
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = employee,
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
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };
                _employeeReposiory.Add(newEmployee);

                return RedirectToAction("Details", new { id = newEmployee.Id });
            }

            return View();

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Employee employee = _employeeReposiory.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeReposiory.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;

                if (model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessUploadedFile(model);
                }

                _employeeReposiory.Update(employee);

                return RedirectToAction("Index");
            }

            return View(model);

        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileSream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileSream);
                }
            }

            return uniqueFileName;
        }



    }
}
