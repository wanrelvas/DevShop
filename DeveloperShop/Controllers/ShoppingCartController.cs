using DeveloperShop.Models;
using DeveloperShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DeveloperShop.Controllers
{
    public class ShoppingCartController : ApiController
    {
        IShoppingCartRepository ShoppingCartRepository;

        public ShoppingCartController()
        {
            ShoppingCartRepository = new ShoppingCartRepository();
        }

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository)
        {
            ShoppingCartRepository = shoppingCartRepository;
        }

        public ShoppingCart Get()
        {
            return ShoppingCartRepository.GetCurrentShoppingCart();
        }

        public HttpResponseMessage Post(ShoppingCartDeveloper developer)
        {
            try
            {
                ShoppingCartRepository.AddDeveloperToShoppingCart(developer);
            }
            catch (Exception ex)
            {                
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }

            return Request.CreateResponse<ShoppingCartDeveloper>(System.Net.HttpStatusCode.Created, developer);
           
          
        }

        [HttpDelete]
        public HttpResponseMessage Delete(string id)
        {
            try
            {
                ShoppingCartRepository.RemoveDeveloperFromShoppingCart(id);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }


        [HttpDelete]
        public HttpResponseMessage CheckOut()
        {
            try
            {
                ShoppingCartRepository.CleanShoppingCart();
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);                
                
            }

            return Request.CreateResponse(HttpStatusCode.OK);

        }

    }
}
