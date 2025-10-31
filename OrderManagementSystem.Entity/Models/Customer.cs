using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Entity.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        [StringLength(100)]
        public String FirstName { get; set; }
        [Required]
        [StringLength(100)]
        public String LastName { get; set; }
        [EmailAddress]
        [Required]
        [StringLength(200)]
        public String Email { get; set; }
        [StringLength(20)]
        public String Phone { get; set; }
        [StringLength(250)]
        public String Address{ get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<Order>? Orders { get; set; } = new List<Order>();



    }
}
