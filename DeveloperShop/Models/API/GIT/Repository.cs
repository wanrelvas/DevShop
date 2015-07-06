using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeveloperShop.Models.API.GIT
{
    public class Repository
    {
        public int id { get; set; }
        public string name { get; set; }
        public string full_name { get; set; }
        public User owner { get; set; }
        public bool Private { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime pushed_at { get; set; }
        public int size { get; set; }
        public int stargazers_count { get; set; }
        public int watchers_count { get; set; }
        public bool has_issues { get; set; }
        public bool has_downloads { get; set; }
        public bool has_wiki { get; set; }
        public int forks_count { get; set; }

    }
}
