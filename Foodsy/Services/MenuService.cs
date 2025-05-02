   using System.Collections.Generic;
   using System.Linq;
   using System.Threading.Tasks;
   using Foodsy.Data;
   using Foodsy.Models;
   using Microsoft.EntityFrameworkCore;

   public class MenuService : IMenuService
   {
       private readonly AppDbContext _context;

       public MenuService(AppDbContext context)
       {
           _context = context;
       }

       public async Task<List<MenuItem>> GetMenuItemsAsync()
       {
           return await _context.MenuItems.ToListAsync();
       }
   }
   