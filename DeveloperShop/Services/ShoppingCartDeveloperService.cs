using DeveloperShop.Models;
using DeveloperShop.Models.API.GIT;
using DeveloperShop.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeveloperShop.Services
{
    public class ShoppingCartDeveloperService
    {
        private const double MINIMUM_DEVELOPER_PRICE = 40.0;
        UserAPIService GitUserAPI;

        public ShoppingCartDeveloperService()
        {
            GitUserAPI = new UserAPIService();
        }

        public ShoppingCartDeveloper GeDeveloperDetails(string username)
        {
            try 
	        {	        
		        var developerOnGit = GitUserAPI.GetUserDetails(username);

                var shoppingCartDeveloper = new ShoppingCartDeveloper(){
                    Username = developerOnGit.login,
                    Price = DefineDeveloperPrice(developerOnGit)
                };

                return shoppingCartDeveloper;
	        }
	        catch (Exception ex)
	        {
		
		        throw ex;
	        }
            
        }

        private double DefineDeveloperPrice(User developerOnGit)
        {
            var price = MINIMUM_DEVELOPER_PRICE;

            var stars = developerOnGit.Starred.Count;

            var repos = developerOnGit.Repositories.Count;

            var followers = developerOnGit.followers;

            var totalPoint = stars + repos + followers +1;

            price = price * (1.0 + (totalPoint / 500));

            return price;
        }

        
    }
}