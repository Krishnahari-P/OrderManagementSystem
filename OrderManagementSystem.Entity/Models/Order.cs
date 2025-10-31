using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Entity.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public Customer? CustomerSet { get; set; }
        public DateTime OrderDate { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
        public Payment? PaymentSet { get; set; }


    }
}
