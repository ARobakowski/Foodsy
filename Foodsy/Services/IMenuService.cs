   // File: Services/IMenuService.cs
   using System.Collections.Generic;
   using System.Threading.Tasks;
   using Foodsy.Models;

   public interface IMenuService
   {
       Task<List<MenuItem>> GetMenuItemsAsync();
   }
   