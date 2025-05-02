using Foodsy.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodsy.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }

        public required string CustomerId { get; set; }  
        public required ApplicationUser Customer { get; set; }  

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

}

