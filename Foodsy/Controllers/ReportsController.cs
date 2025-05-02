using Foodsy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class ReportsController : Controller
{
    private readonly AppDbContext _context;

    public ReportsController(AppDbContext context)
    {
        _context = context;
    }

  
    public async Task<IActionResult> MonthlySales()
    {
        var today = DateTime.Today;
        var startOfMonth = new DateTime(today.Year, today.Month, 1);

        var orders = await _context.Orders
            .Where(o => o.OrderDate >= startOfMonth)
            .ToListAsync();

        var totalSales = orders.Sum(o => o.TotalAmount);
        var totalOrders = orders.Count;

        ViewData["TotalSales"] = totalSales;
        ViewData["TotalOrders"] = totalOrders;
        ViewData["Month"] = today.ToString("MMMM yyyy");

        return View();
    }

  
    public IActionResult CustomerActivity()
    {
        var customerActivity = _context.Orders
            .GroupBy(o => o.CustomerId)
            .Select(g => new
            {
                CustomerId = g.Key,
                FullName = _context.Users.FirstOrDefault(u => u.Id == g.Key).FullName, 
                OrderCount = g.Count(),
                TotalSpent = g.Sum(o => o.TotalAmount)
            })
            .ToList<dynamic>();

        return View(customerActivity);
    }
}
