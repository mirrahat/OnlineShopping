using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShoppingStore.Models
{
    public class Adminproductlist
    {
        public int ProductId { get; set; }
        [Required]
        [Range(1, 50)]
        public Nullable<int> CategoryId { get; set; }
        public string SubCategoryName { get; set; }
        [Required(ErrorMessage = "Product Name is Required")]
        [StringLength(100, ErrorMessage = "Minimum 3 and Maximum 100", MinimumLength = 3)]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Description Is required")]
        public string Descriptions { get; set; }
        [Range(typeof(decimal), "1", "2000000000", ErrorMessage = "Invalid Price")]
        public int ProductPrice { get; set; }
        public int OldPrice { get; set; }
        public string ProductImage { get; set; }
        [Range(typeof(int), "1", "500", ErrorMessage = "Invalid Quantity")]
        public Nullable<int> ProductQuantity { get; set; }

        public SelectList Categories { get; set; }

        [Required(ErrorMessage = "ProductSize Is required")]
        public string ProductSize { get; set; }
        [Required(ErrorMessage = "ProductColor Is required")]

        public string ProductColor { get; set; }
        

        [Required(ErrorMessage = "Tag Is required")]
        public string Taglist { get; set;  }
        public string avaiable { get; set; }

        public string discount { get; set; }
        public String BrandName { get; set; }
    }
}