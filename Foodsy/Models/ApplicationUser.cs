using Foodsy.Models;
using Microsoft.AspNetCore.Identity;

namespace Foodsy.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string FullName { get; set; } 
        public ICollection<Order> Orders { get; set; } = new List<Order>(); 
    }
}