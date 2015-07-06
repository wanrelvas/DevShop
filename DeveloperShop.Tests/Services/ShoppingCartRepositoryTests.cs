using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeveloperShop.Services;
using System.Web;
using DeveloperShop.Models;
using System.Collections.Generic;

namespace DeveloperShop.Tests.Services
{
    [TestClass]
    public class ShoppingCartRepositoryTests
    {
        private static string CacheKey = "ShoppingCart";

        [TestMethod]
        public void GetShoppingCart()
        {
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            ShoppingCartRepository repository = new ShoppingCartRepository();

            var shoppingCart= repository.GetCurrentShoppingCart();

            Assert.IsNotNull(shoppingCart);
            Assert.IsNotNull(shoppingCart.ShoppingCartDevelopers);
            Assert.IsTrue(shoppingCart.ShoppingCartDevelopers.Count > 0);
        }

        [TestMethod]
        public void AddDeveloperToShoppingCart()
        {
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            var newDeveloper = new ShoppingCartDeveloper()
            {
                Username="wanelvas",
                Price = 200,
                Hours = 40
            };

            ShoppingCartRepository repository = new ShoppingCartRepository();

            bool devloperAdded = repository.AddDeveloperToShoppingCart(newDeveloper);

            var contextModified = (ShoppingCart)HttpContext.Current.Cache[CacheKey];

            Assert.IsTrue(devloperAdded);
            Assert.AreEqual(newDeveloper.Price,contextModified.ShoppingCartDevelopers.Find(x=>x.Username.Equals(newDeveloper.Username)).Price);

        }       

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddDeveloperToShoppingCartErrorByNullDeveloperParameter()
        {
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            ShoppingCartRepository repository = new ShoppingCartRepository();

            repository.AddDeveloperToShoppingCart(null);


        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddDeveloperToShoppingCartErrorByEmptyDeveloperValuesParameter()
        {
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            ShoppingCartRepository repository = new ShoppingCartRepository();

            var newDeveloper = new ShoppingCartDeveloper();

            newDeveloper.Price = default(double);
            newDeveloper.Username = String.Empty;
            newDeveloper.Hours = default(int);

           repository.AddDeveloperToShoppingCart(newDeveloper);

        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateWaitObjectException))]
        public void AddDeveloperToShoppingCartErrorByDuplicatingDevelopers()
        {
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            var newDeveloper = new ShoppingCartDeveloper()
            {
                Username = "wrelvas",
                Price = 200
            };

            var duplicatedDeveloper = new ShoppingCartDeveloper()
            {
                Username = "wrelvas",
                Price= 200
            };

            ShoppingCartRepository repository = new ShoppingCartRepository();

            bool firsDevloperAdded = repository.AddDeveloperToShoppingCart(newDeveloper);
            bool secondDeveloperAdded = repository.AddDeveloperToShoppingCart(duplicatedDeveloper);

            Assert.IsTrue(firsDevloperAdded);
            Assert.IsFalse(secondDeveloperAdded);
            
        }

        [TestMethod]
        public void RemoveDeveloperFromShoppingCart()
        {
            //ARRANGE
            var developerUsernameToRemove = "wanrelvas";

            var fakeDeveloper = new ShoppingCartDeveloper()
            {
                Username = developerUsernameToRemove,
                Price = 200
            };

            var shopppingCart = new ShoppingCart() { ShoppingCartDevelopers = new List<ShoppingCartDeveloper>() };

            shopppingCart.ShoppingCartDevelopers.Add(new ShoppingCartDeveloper()
            {
                Username = "wan",
                Price= 100
            });
            shopppingCart.ShoppingCartDevelopers.Add(fakeDeveloper);
            shopppingCart.ShoppingCartDevelopers.Add(new ShoppingCartDeveloper()
            {
                Username="alfa",
                Price = 300
            });

            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            HttpContext.Current.Cache[CacheKey] = shopppingCart;

            ShoppingCartRepository repository = new ShoppingCartRepository();

            //ACT
            bool developerRemoved = repository.RemoveDeveloperFromShoppingCart(developerUsernameToRemove);

            //ASSERT   
            Assert.IsTrue(developerRemoved);
            var contextModified = (ShoppingCart)HttpContext.Current.Cache[CacheKey];
            Assert.AreEqual(2, contextModified.ShoppingCartDevelopers.Count);

        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void RemoveDeveloperFromShoppingCartErrorByRemovingNonExistentDeveloper()
        {
            //ARRANGE
            var developerUsernameToRemove = "wanessa";

            var fakeDeveloper = new ShoppingCartDeveloper()
            {
                Username = "wanrelvas",
                Price = 200
            };

            var shopppingCart = new ShoppingCart() { ShoppingCartDevelopers = new List<ShoppingCartDeveloper>() };
            shopppingCart.ShoppingCartDevelopers.Add(fakeDeveloper);
           
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            HttpContext.Current.Cache[CacheKey] = shopppingCart;

            ShoppingCartRepository repository = new ShoppingCartRepository();

            //ACT
            bool developerRemoved = repository.RemoveDeveloperFromShoppingCart(developerUsernameToRemove);

            //ASSERT   
            Assert.IsFalse(developerRemoved);
            var contextModified = (ShoppingCart)HttpContext.Current.Cache[CacheKey];
            Assert.AreEqual(1, contextModified.ShoppingCartDevelopers.Count);
        }

        [TestMethod]
        public void CleanShoppingCartWithSuccess()
        {
            var shopppingCart = new ShoppingCart() { ShoppingCartDevelopers = new List<ShoppingCartDeveloper>() };

            shopppingCart.ShoppingCartDevelopers.Add(new ShoppingCartDeveloper()
            {
                Username = "wan",
                Price = 100
            });
            shopppingCart.ShoppingCartDevelopers.Add(new ShoppingCartDeveloper()
            {
                Username = "alfa",
                Price = 300
            });

            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            HttpContext.Current.Cache[CacheKey] = shopppingCart;

            ShoppingCartRepository repository = new ShoppingCartRepository();

            bool cartCleaned= repository.CleanShoppingCart();

            var contextModified = (ShoppingCart)HttpContext.Current.Cache[CacheKey];

            Assert.IsTrue(cartCleaned);
            Assert.AreEqual(0, contextModified.ShoppingCartDevelopers.Count);

        }

        [TestMethod]
        public void CleanEmptyShoppingCartWithoutError()
        {
            var shopppingCart = new ShoppingCart() { ShoppingCartDevelopers = new List<ShoppingCartDeveloper>() };

            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            HttpContext.Current.Cache[CacheKey] = shopppingCart;

            ShoppingCartRepository repository = new ShoppingCartRepository();

            bool cartCleaned = repository.CleanShoppingCart();

            var contextModified = (ShoppingCart)HttpContext.Current.Cache[CacheKey];

            Assert.IsTrue(cartCleaned);
            Assert.AreEqual(0, contextModified.ShoppingCartDevelopers.Count);

        }
    }
}
