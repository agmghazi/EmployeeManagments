using System.ComponentModel.DataAnnotations;

namespace EmployeeManagments.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
