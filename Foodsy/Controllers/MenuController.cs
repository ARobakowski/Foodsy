// File: Controllers/MenuController.cs
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Foodsy.Models;

public class MenuController : Controller
{
    private readonly IMenuService _menuService;

    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    public async Task<IActionResult> Menu()
    {
        var menuItems = await _menuService.GetMenuItemsAsync();
        return View(menuItems);
    }
}
