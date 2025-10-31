using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Entity.Models
{
    [Table("Supplier")]
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(150)]
        public string SupplierName { get; set; }

        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        public DateTime CreatedDate { get; set; }
        public ICollection<Purchase>? Purchases { get; set; } = new List<Purchase>();

    }
}
