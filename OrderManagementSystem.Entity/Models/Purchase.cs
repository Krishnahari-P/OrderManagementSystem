using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Entity.Models
{
    [Table("Purchase")]
    public class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        public DateTime PurchaseDate { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [StringLength(50)]
        public string Status { get; set; }
        public Supplier? SupplierSet { get; set; }
        public ICollection<PurchaseItem>? PurchaseItems { get; set; } = new List<PurchaseItem>();
        public Payment? PaymentSet { get; set; }


    }
}
