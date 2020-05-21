using EmarketTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmarketTask.ViewModels
{
    public class ProductCart
    {
        public IEnumerable<Cart> cart { get; set; }
        public IEnumerable<product> myproducts { get; set; }
        //public IEnumerable<category> category { get; set; }
    }
}