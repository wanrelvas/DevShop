using DeveloperShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeveloperShop.Services
{
    public interface IShoppingCartRepository
    {
        ShoppingCart GetCurrentShoppingCart();
        bool AddDeveloperToShoppingCart(ShoppingCartDeveloper newDeveloper);
        bool RemoveDeveloperFromShoppingCart(string developerUsernameToRemove);
        bool CleanShoppingCart();
    }
}
