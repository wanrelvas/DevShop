using DeveloperShop.Models.API.GIT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;

namespace DeveloperShop.Services.API
{
    public class UserAPIService: GitHubAPIService
    {
        public IList<User> ListUsers()
        {
            try
            {
                var request = new RestRequest("/users", Method.GET);

                IRestResponse<List<User>> apiUsersResponse = GitClient.Execute<List<User>>(request);

                return apiUsersResponse.Data;
            }
            catch (Exception ex)
            {                
                throw ex;
            }

        }

        public User GetUser(string userLogin)
        {
            try
            {
                var request = new RestRequest("/users/{login}", Method.GET);
                request.AddUrlSegment("login", userLogin);

                IRestResponse<User> apiUsersResponse = GitClient.Execute<User>(request);

                return apiUsersResponse.Data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User GetUserDetails(string userLogin)
        {
            User gitUser = GetUser(userLogin);

            gitUser.Repositories = GetUserRepositories(userLogin);
            gitUser.Starred = GetUserStarred(userLogin);

            return gitUser;
        }

        private List<Repository> GetUserStarred(string userLogin)
        {
            try
            {
                var request = new RestRequest("/users/{login}/starred", Method.GET);
                request.AddUrlSegment("login", userLogin);

                IRestResponse<List<Repository>> apiUsersResponse = GitClient.Execute<List<Repository>>(request);

                return apiUsersResponse.Data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<Repository> GetUserRepositories(string userLogin)
        {     
            try
            {
                var request = new RestRequest("/users/{login}/repos", Method.GET);
                request.AddUrlSegment("login", userLogin);

                IRestResponse<List<Repository>> apiUsersResponse = GitClient.Execute<List<Repository>>(request);

                return apiUsersResponse.Data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}