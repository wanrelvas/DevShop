using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeveloperShop.Controllers;
using DeveloperShop.Models;
using Moq;
using DeveloperShop.Services;
using System.Web.Http.Routing;
using System.Web.Http.Hosting;
using System.Net.Http;
using System.Collections.Generic;

namespace DeveloperShop.Tests.Controllers
{
    [TestClass]
    public class ShoppingCartControllerTest
    {
        private static ShoppingCartController CreateController(Mock<IShoppingCartRepository> shoppingCartRepository)
        {
            ShoppingCartController controller = new ShoppingCartController(shoppingCartRepository.Object);
            controller.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/ShoppingCart")
            };

            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new System.Web.Http.HttpConfiguration());
            return controller;
        }


        [TestMethod]
        public void GetTest()
        {
            var shoppingCartRepository = new Mock<IShoppingCartRepository>();
            shoppingCartRepository.Setup(s => s.GetCurrentShoppingCart()).Returns(new ShoppingCart() { ShoppingCartDevelopers = new System.Collections.Generic.List<ShoppingCartDeveloper>()});

            ShoppingCartController controller =CreateController(shoppingCartRepository);

            var restult = controller.Get();

            Assert.IsNotNull(restult);
            Assert.IsInstanceOfType(restult, typeof(ShoppingCart));
            
        }

        [TestMethod]
        public void PostWithSuccessResponseTest()
        {
            var developer = new ShoppingCartDeveloper()
            {
                Username= "wanrelvas",
                Price = 200
            };

            var shoppingCartRepository = new Mock<IShoppingCartRepository>();
            shoppingCartRepository.Setup(s => s.AddDeveloperToShoppingCart(developer)).Returns(true);

            ShoppingCartController controller = CreateController(shoppingCartRepository);

            var result = controller.Post(developer);

            Assert.AreEqual(System.Net.HttpStatusCode.Created, result.StatusCode);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void PostWithErrorResponseTest()
        {
            var developer = new ShoppingCartDeveloper();

            var shoppingCartRepository = new Mock<IShoppingCartRepository>();
            shoppingCartRepository.Setup(s => s.AddDeveloperToShoppingCart(developer)).Throws(new DuplicateWaitObjectException());

            ShoppingCartController controller = CreateController(shoppingCartRepository);

            var result = controller.Post(developer);

            Assert.AreEqual(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.IsNotNull(result.Content);
        }

      
        [TestMethod]
        public void DeleteWithSucccessResponseTest()
        {
            var developerUsername = "wrelvas";

            var shoppingCartRepository = new Mock<IShoppingCartRepository>();
            shoppingCartRepository.Setup(x => x.RemoveDeveloperFromShoppingCart(developerUsername)).Returns(true);

            ShoppingCartController controller = CreateController(shoppingCartRepository);

            HttpResponseMessage result = controller.Delete(developerUsername);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, result.StatusCode);
            Assert.IsNull(result.Content);

        }

        [TestMethod]
        public void DeleteWithErrorResponseTest()
        {
            var developerUsername = "wrelvas";

            var shoppingCartRepository = new Mock<IShoppingCartRepository>();
            shoppingCartRepository.Setup(x => x.RemoveDeveloperFromShoppingCart(developerUsername)).Throws(new KeyNotFoundException());

            ShoppingCartController controller = CreateController(shoppingCartRepository);

            HttpResponseMessage result = controller.Delete(developerUsername);

            Assert.AreEqual(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.IsNotNull(result.Content);

        }


        [TestMethod]
        public void CheckOutWithSucccess()
        {
           
            var shoppingCartRepository = new Mock<IShoppingCartRepository>();
            shoppingCartRepository.Setup(x => x.CleanShoppingCart()).Returns(true);

            ShoppingCartController controller = CreateController(shoppingCartRepository);

            HttpResponseMessage result = controller.CheckOut();

            Assert.AreEqual(System.Net.HttpStatusCode.OK, result.StatusCode);
            Assert.IsNull(result.Content);

        }

        [TestMethod]
        public void CheckOutWithError()
        {

            var shoppingCartRepository = new Mock<IShoppingCartRepository>();
            shoppingCartRepository.Setup(x => x.CleanShoppingCart()).Throws(new ContextMarshalException());

            ShoppingCartController controller = CreateController(shoppingCartRepository);

            HttpResponseMessage result = controller.CheckOut();

            Assert.AreEqual(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.IsNotNull(result.Content);

        }
    }
}
