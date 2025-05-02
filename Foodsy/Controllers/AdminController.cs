namespace Foodsy.Controllers
{
    using Foodsy.Data;
    using Foodsy.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize(Roles = "Admin")] 
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        
        public IActionResult AdminDashboard()
        {
            
            return View();
        }

       
        public async Task<IActionResult> ManageUsers()
        {
            var users = _userManager.Users.ToList(); 
            return View(users);
        }

        
        public IActionResult ManageOrders()
        {
            var orders = _context.Orders
             .Include(o => o.OrderItems) 
             .ToList();

            return View(orders);
        }

     
        public IActionResult ViewReports()
        {
            
            return View();
        }
    }
}
