using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeveloperShop.Models;

namespace DeveloperShop.Services
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private const string CacheKey = "ShoppingCart";

        private HttpContext Context;

        public ShoppingCartRepository()
        {
            if (Context == null)
            {
                Context = HttpContext.Current;
            }

            if (Context != null)
            {
                if (Context.Cache[CacheKey] == null)
                {
                    Context.Cache[CacheKey] = MockDB();
                }
            }
        }


        public ShoppingCart GetCurrentShoppingCart()
        {
            return (ShoppingCart)Context.Cache[CacheKey];
        }



        private static ShoppingCart MockDB()
        {
            return new ShoppingCart
            {

                ShoppingCartDevelopers = new List<ShoppingCartDeveloper>(){

                    new ShoppingCartDeveloper(){
                       
                                                    Username = "brenoc",
                                                    Price = 224,
                                                Hours = 2

                    },
                    new ShoppingCartDeveloper(){
                        
                                                    Username = "firstdoit",
                                                    Price = 416,
                                                    Hours = 4
                    }
                }

            };
        }

        public bool AddDeveloperToShoppingCart(ShoppingCartDeveloper newDeveloper)
        {
            
            try
            {
                if (newDeveloper == default(ShoppingCartDeveloper) || 
                    String.IsNullOrEmpty(newDeveloper.Username) || 
                    newDeveloper.Price ==default(double))
                {
                    throw new ArgumentNullException();
                }


                ShoppingCart shoppingCart = (ShoppingCart)Context.Cache[CacheKey];

                if (shoppingCart != null)
                {
                    if (shoppingCart.ShoppingCartDevelopers.FirstOrDefault(x => x.Username.Equals(newDeveloper.Username)) 
                                                                == default(ShoppingCartDeveloper))
                    {
                        shoppingCart.ShoppingCartDevelopers.Add(newDeveloper);
                    }
                    else
                    {
                        throw new DuplicateWaitObjectException();
                    }
                }
                else
                {
                    shoppingCart = new ShoppingCart()
                    {
                        ShoppingCartDevelopers = new List<ShoppingCartDeveloper>()
                    };

                    shoppingCart.ShoppingCartDevelopers.Add(newDeveloper);
                }

                Context.Cache[CacheKey] = shoppingCart;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public bool RemoveDeveloperFromShoppingCart(string developerUsernameToRemove)
        {
            bool removed = false;

            try
            {
                ShoppingCart shoppingCart = (ShoppingCart)Context.Cache[CacheKey];

                if (shoppingCart != null)
                {
                    var developerToRemove = shoppingCart.ShoppingCartDevelopers.Find(x => x.Username.Equals(developerUsernameToRemove));
                    removed = shoppingCart.ShoppingCartDevelopers.Remove(developerToRemove);

                    if (!removed)
                    {
                        throw new KeyNotFoundException();
                    }

                    Context.Cache[CacheKey] = shoppingCart;
                    return removed;
                }
                else
                {
                    throw new ContextMarshalException();
                }

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

        }

        public bool CleanShoppingCart()
        {
            try
            {
                ShoppingCart shoppingCart = (ShoppingCart)Context.Cache[CacheKey];

                if (shoppingCart != null)
                {
                    shoppingCart.ShoppingCartDevelopers.Clear();
                    Context.Cache[CacheKey] = shoppingCart;
                    return true;
                }
                else
                {
                    throw new ContextMarshalException();
                }

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}