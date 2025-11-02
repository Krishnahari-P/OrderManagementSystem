using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Entity.ViewModel
{
    public class AddOrRemoveRolesViewModel
    {
        [Required]
        public String RoleId { get; set; }
        [Required]
        public String RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
