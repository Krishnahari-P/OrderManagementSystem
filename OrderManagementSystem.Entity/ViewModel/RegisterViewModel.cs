using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Entity.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public String UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Confirmation failed due to mismatch")]
        public String ConfirmPassword { get; set; }

    }
}
