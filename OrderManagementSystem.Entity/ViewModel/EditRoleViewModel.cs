using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Entity.ViewModel
{
    public class EditRoleViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public String RoleName { get; set; }
    }
}
