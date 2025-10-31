using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Entity.Models
{
    [Table("Payment")]
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public int? OrderId { get; set; }
        public int? PurchaseId { get; set; }
        public DateTime PaymentDate { get; set; }
        [Required]
        public decimal AmountPaid { get; set; }

        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [StringLength(100)]
        public string TransactionReference { get; set; }
        public Order? OrderSet { get; set; }
        public Purchase? PurchaseSet { get; set; }
    }
}

