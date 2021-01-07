using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagments.Models
{
    public class EmailModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "please, enter valid mail")]
        public string To { get; set; }

        [EmailAddress(ErrorMessage = "please, enter valid mail")]
        public string CC { get; set; }
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        public IFormFile Attachment { get; set; }

    }
}
