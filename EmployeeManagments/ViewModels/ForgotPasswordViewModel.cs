using System.ComponentModel.DataAnnotations;

namespace EmployeeManagments.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
