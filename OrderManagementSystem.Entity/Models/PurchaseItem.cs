using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Entity.Models
{
    [Table("PurchaseItem")]
    public class PurchaseItem
    {
        [Key]
        public int PurchaseItemId { get; set; }

        [Required]
        public int PurchaseId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "DECIMAL(10,2)")]
        public decimal UnitCost { get; set; }
        public Purchase? PurchaseSet { get; set; }
        public Product? ProductSet { get; set; }
    }
}
