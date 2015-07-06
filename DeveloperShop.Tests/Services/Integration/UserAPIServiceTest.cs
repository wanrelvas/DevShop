using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeveloperShop.Models.API.GIT;
using DeveloperShop.Services.API;
using System.Collections.Generic;
using System.Linq;

namespace DeveloperShop.Tests.Services.API
{
    [TestClass]
    public class UserAPIServiceTest
    {
        [TestMethod]
        public void ListGitUsersWithSucccess()
        {
            UserAPIService userAPIService = new UserAPIService();

            IList<User> gitUsers = userAPIService.ListUsers();

            Assert.IsNotNull(gitUsers);
            Assert.IsTrue(gitUsers.Count > 0);
        }


        [TestMethod]
        public void GetGitUserOnAValidModelWithSucccess()
        {
            var userLogin = "octocat";

            UserAPIService userAPIService = new UserAPIService();

            User gitUser = userAPIService.GetUser(userLogin);

            Assert.IsNotNull(gitUser);
        
            Assert.IsFalse(String.IsNullOrEmpty(gitUser.login));
            Assert.IsFalse(String.IsNullOrEmpty(gitUser.url));
            Assert.IsNotNull(gitUser.created_at);

        }

        [TestMethod]
        public void GetGitUserDetailsWithSucccess()
        {
            var userLogin = "octocat";

            UserAPIService userAPIService = new UserAPIService();

            User gitUser = userAPIService.GetUserDetails(userLogin);

            Assert.IsNotNull(gitUser);
            Assert.IsNotNull(gitUser.Repositories);
            Assert.IsNotNull(gitUser.Starred);
            Assert.IsTrue(gitUser.Repositories.Count > 0);
            Assert.IsNotNull(gitUser.Starred.Count >0);

        }
    }
}
