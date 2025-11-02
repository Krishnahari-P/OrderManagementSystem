using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Entity.Models
{
    [Table("Menu")]
    public class Menu
    {
        [Key]
        public int MenuId { get; set; }
        public String DisplayName { get; set; }
        public String Controller { get; set; }
        public String Action { get; set; }
        public bool IsActive { get; set; }

    }
}
