using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;

namespace DeveloperShop.Services.API
{
    public class GitHubAPIService
    {
        protected RestClient GitClient;

        public GitHubAPIService()
        {
            GitClient = new RestClient("https://api.github.com/");
        }
    }
}