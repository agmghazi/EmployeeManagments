using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagments.Models
{
    public class EmployeeCreateViewModel
    {
        public string Name { get; set; }
        [Required]
        [Display(Name = "office Name")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "InValid Email Format")]
        public string Email { get; set; }
        [Required]
        public Dept? Department { get; set; }

        [Required]
        public IFormFile Photo { get; set; }
    }
}
