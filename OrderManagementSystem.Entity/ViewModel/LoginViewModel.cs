using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Entity.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public required String UserName { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public String Password { get; set; }
    }
}
