using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace OnlineShoppingStore.Models
{
    public class ProductsbyOrder
    {
        public DataTable Products { get; set; }
        public int Quantity { get; set; }
    }
}