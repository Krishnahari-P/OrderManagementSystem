using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Entity.Models
{
    [Table("Category")]
    public class Category
    {
        [Key] 
        public int CategoryId { get; set; }
        [StringLength(100)]
        [Required]
        public String CategoryName { get; set; }
        [StringLength(250)]
        public String Description { get; set; }
        public ICollection<Product>? Products { get; set; }=new List<Product>();
    }
}
