using Foodsy.Data;
using Foodsy.Extensions;
using Foodsy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

[Authorize] 
public class OrderController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderController(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> ManageOrders()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ToListAsync();
        return View("~/Views/Admin/ManageOrders.cshtml", orders);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        _context.OrderItems.RemoveRange(order.OrderItems); 
        _context.Orders.Remove(order); 
        await _context.SaveChangesAsync();

        return RedirectToAction("ManageOrders");
    }

    [HttpPost]
    public IActionResult AddToCart(int menuItemId)
    {
        var menuItem = _context.MenuItems.FirstOrDefault(m => m.Id == menuItemId);
        if (menuItem == null) return NotFound();

        var cartItems = HttpContext.Session.GetObjectFromJson<List<MenuItem>>("Cart") ?? new List<MenuItem>();
        cartItems.Add(menuItem);
        HttpContext.Session.SetObjectAsJson("Cart", cartItems);
        TempData["Message"] = $"{menuItem.Name} has been added to your cart.";
        return RedirectToAction("Menu", "Menu");
    }

    public IActionResult ViewCart()
    {
        var cartItems = HttpContext.Session.GetObjectFromJson<List<MenuItem>>("Cart") ?? new List<MenuItem>();
        return View(cartItems);
    }

    [HttpPost]
    public IActionResult RemoveFromCart(int menuItemId)
    {
        var cartItems = HttpContext.Session.GetObjectFromJson<List<MenuItem>>("Cart") ?? new List<MenuItem>();
        var itemToRemove = cartItems.FirstOrDefault(i => i.Id == menuItemId);

        if (itemToRemove != null)
        {
            cartItems.Remove(itemToRemove);
            HttpContext.Session.SetObjectAsJson("Cart", cartItems);
        }

        return RedirectToAction("ViewCart");
    }

    private decimal CalculateOrderTotal()
    {
        var cartItems = HttpContext.Session.GetObjectFromJson<List<MenuItem>>("Cart");
        return cartItems?.Sum(item => item.Price) ?? 0;
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder()
    {
        var cartItems = HttpContext.Session.GetObjectFromJson<List<MenuItem>>("Cart");
        if (cartItems == null || !cartItems.Any()) return RedirectToAction("ViewCart");

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized(); 

        var newOrder = new Order
        {
            CustomerId = user.Id, 
            Customer = user, 
            TotalAmount = cartItems.Sum(item => item.Price),
            OrderDate = DateTime.Now
        };

        foreach (var cartItem in cartItems)
        {
            newOrder.OrderItems.Add(new OrderItem
            {
                MenuItemId = cartItem.Id,
                MenuItemName = cartItem.Name,
                Price = cartItem.Price,
                Quantity = 1
            });
        }

        _context.Orders.Add(newOrder);
        await _context.SaveChangesAsync(); 
        HttpContext.Session.Remove("Cart");

        return RedirectToAction("OrderConfirmation", new { orderId = newOrder.Id });
    }

    public async Task<IActionResult> OrderConfirmation(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null) return NotFound();

        return View(order);
    }
}
