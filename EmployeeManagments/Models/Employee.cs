﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagments.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }


        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 character")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "office Name")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "InValid Email Format")]
        public string Email { get; set; }
        [Required]
        public Dept? Department { get; set; }

        public string PhotoPath { get; set; }
    }
}
