using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Entity.ViewModel
{
    public class CreateRoleViewModel
    {
        [Required]
        public String RoleName { get; set; }
    }
}
