using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeveloperShop.Models
{
    public class ShoppingCartDeveloper : Developer
    {
        public int Hours { get; set; }
        public double TotalPrice
        {
            get
            {
                return this.Price * this.Hours;
            }
            set
            {
                this.TotalPrice = value;
            }
        }
    }
}