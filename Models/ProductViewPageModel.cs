using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace OnlineShoppingStore.Models
{
    public class ProductViewPageModel
    {
      public  DataTable ProductDatatable { get; set; }
      public  DataTable RelatedProductTable { get; set; }

    }
}