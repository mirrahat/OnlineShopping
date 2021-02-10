using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using OnlineShoppingStore.Models;

namespace OnlineShoppingStore.Controllers
{
    public class MyAdminController : Controller
    {    string connectionString = @"Data Source = DESKTOP-2V8523P\SQLEXPRESS;  Initial Catalog =OnlineShoppingStore;  Integrated Security  = true";
        // GET: MyAdmin
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("adminlogin");
            }
            return View();
        }
        public ActionResult adminlog(string adminusername, string adminpassword)
        {

            if (adminusername == "myadmin" && adminpassword == "12345")
            {
                Session["admin"] = "Login";
                return RedirectToAction("MyDashbord");
            }
            else
            {
                TempData["Admin"] = "Incorrect Username Or Password";
                return RedirectToAction("adminlogin");
            }


        }
        public List<SelectListItem> GetCategory() {
          
            List<SelectListItem> list = new List<SelectListItem>();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Category", sqlcon);
                sqlDa.Fill(dtblProduct);
                foreach (DataRow item in dtblProduct.Rows)
                {
                    list.Add(new SelectListItem { Value = item["CategoryId"].ToString(), Text = item["CategoryName"].ToString() });
                }
            }
            return list;
        }
        public ActionResult MyDashbord()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("adminlogin");
            }
            return View();
        }
        // GET: MyAdmin
        [HttpGet]
        public ActionResult MyProducts()
        { DataTable dtblProduct = new DataTable();
          using (SqlConnection sqlcon = new SqlConnection(connectionString)) {
          sqlcon.Open();
          SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Product", sqlcon);
          sqlDa.Fill(dtblProduct);
        }
        return View(dtblProduct);
        }
        [HttpGet]
        public ActionResult Create() {
            ViewBag.CategoryList = GetCategory();

            return View(new Adminproductlist());
        }
        [HttpPost]
        public ActionResult Create(Adminproductlist productlist, HttpPostedFileBase file)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("adminlogin");
            }
            string pic = null;
            if (file != null)
            {   pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), pic);
                file.SaveAs(path);
            }
             using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   sqlcon.Open();
                string query = "Insert into Product  Values(@CategoryId,@SubCategoryName,@ProductName,@ProductDescription," +
                    "@ProductQuantity,@ProductPrice,@OldPrice,@ProductImage,@Avilable,@ProductColor,@Discount,@Tag,@ProductSize,@BrandName) ";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@CategoryId", productlist.CategoryId);
                sqlCmd.Parameters.AddWithValue("@SubCategoryName", productlist.SubCategoryName);
                sqlCmd.Parameters.AddWithValue("@ProductName", productlist.ProductName);
                sqlCmd.Parameters.AddWithValue("@ProductDescription", productlist.Descriptions);
                sqlCmd.Parameters.AddWithValue("@ProductQuantity", productlist.ProductQuantity);
                sqlCmd.Parameters.AddWithValue("@ProductPrice", productlist.ProductPrice);
                sqlCmd.Parameters.AddWithValue("@OldPrice", productlist.OldPrice);
                sqlCmd.Parameters.AddWithValue("@ProductImage", pic);
                sqlCmd.Parameters.AddWithValue("@Avilable", productlist.avaiable);
                sqlCmd.Parameters.AddWithValue("@ProductColor", productlist.ProductColor);
                sqlCmd.Parameters.AddWithValue("@Discount", productlist.discount);
                sqlCmd.Parameters.AddWithValue("@Tag", productlist.Taglist);
                sqlCmd.Parameters.AddWithValue("@ProductSize", productlist.ProductSize);
                sqlCmd.Parameters.AddWithValue("@BrandName", productlist.BrandName);
                sqlCmd.ExecuteNonQuery();
            }
               return RedirectToAction("MyProducts");
        }

        [HttpGet]
        [Route("MyAdmin/Edit/{ProductId}")]
        public ActionResult Edit(int ProductId)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("adminlogin");
            }
            ViewBag.CategoryList = GetCategory();
            Adminproductlist productlist = new Adminproductlist();
                  DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {

                sqlcon.Open();
                string query = $"Select  * from Product  where ProductId = @ProductId";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query,sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@ProductId", ProductId);
                sqlDa.Fill(dtblProduct);
            }
            if (dtblProduct.Rows.Count == 1)
            {   productlist.ProductId = Convert.ToInt32(dtblProduct.Rows[0][0].ToString());
                productlist.CategoryId = Convert.ToInt32(dtblProduct.Rows[0][1].ToString());
                productlist.SubCategoryName = dtblProduct.Rows[0][2].ToString();
                productlist.ProductName = dtblProduct.Rows[0][3].ToString();
                productlist.Descriptions = dtblProduct.Rows[0][4].ToString();
                productlist.ProductQuantity = Convert.ToInt32(dtblProduct.Rows[0][5].ToString());
                productlist.ProductPrice = Convert.ToInt32(dtblProduct.Rows[0][6].ToString());
                productlist.OldPrice = Convert.ToInt32(dtblProduct.Rows[0][7].ToString());
                productlist.ProductImage = dtblProduct.Rows[0][8].ToString();
                productlist.avaiable =(dtblProduct.Rows[0][9]).ToString();
                productlist.ProductColor = dtblProduct.Rows[0][10].ToString();
                productlist.discount = dtblProduct.Rows[0][11].ToString();
                productlist.Taglist = dtblProduct.Rows[0][12].ToString();
                productlist.BrandName = dtblProduct.Rows[0][13].ToString();
                return View(productlist);
            }
            else
            {
                return RedirectToAction("MyProducts");
            }

        }

        [HttpPost]

        public ActionResult EditA(Adminproductlist productlist, HttpPostedFileBase  file) {
            if (Session["admin"] == null)
            {
                return RedirectToAction("adminlogin");
            }
            string pic=null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), pic);
                file.SaveAs(path);
                productlist.ProductImage = pic;
            }
            else {
                pic = productlist.ProductImage;
            }
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "UPDATE Product SET CategoryId=@CategoryId,SubCategoryName=@SubCategoryName,ProductName=@ProductName,ProductDescription=@ProductDescription," +
                    "ProductQuantity=@ProductQuantity,ProductPrice=@ProductPrice,OldPrice=@OldPrice,ProductImage=@ProductImage,Avilable=@Avilable,ProductColor=@ProductColor,Discount=@Discount," +
                    "Tag=@Tag,ProductSize=@ProductSize,BrandName=@BrandName where ProductId = @ProductId ";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@ProductId", productlist.ProductId.ToString());
                sqlCmd.Parameters.AddWithValue("@CategoryId", productlist.CategoryId.ToString());
                sqlCmd.Parameters.AddWithValue("@SubCategoryName", productlist.SubCategoryName.ToString());
                sqlCmd.Parameters.AddWithValue("@ProductName", productlist.ProductName.ToString());
                sqlCmd.Parameters.AddWithValue("@ProductDescription", productlist.Descriptions.ToString());
                sqlCmd.Parameters.AddWithValue("@ProductQuantity", productlist.ProductQuantity.ToString());
                sqlCmd.Parameters.AddWithValue("@ProductPrice", productlist.ProductPrice.ToString());
                sqlCmd.Parameters.AddWithValue("@OldPrice", productlist.OldPrice.ToString());
                sqlCmd.Parameters.AddWithValue("@ProductImage", pic);
                sqlCmd.Parameters.AddWithValue("@Avilable", productlist.avaiable.ToString());
                sqlCmd.Parameters.AddWithValue("@ProductColor", productlist.ProductColor.ToString());
                sqlCmd.Parameters.AddWithValue("@Discount", productlist.discount.ToString());
                sqlCmd.Parameters.AddWithValue("@Tag", productlist.Taglist.ToString());
                sqlCmd.Parameters.AddWithValue("@ProductSize", productlist.ProductSize.ToString());
                sqlCmd.Parameters.AddWithValue("@BrandName", productlist.BrandName.ToString());
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("MyProducts");
        }

        [HttpGet]
        [Route("MyAdmin/OrderviewDetails/{OrderId}")]
        public ActionResult OrderviewDetails(int OrderId) {
            if (Session["admin"] == null)
            {
                return RedirectToAction("adminlogin");
            }
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   sqlcon.Open();
                string query = $"Select  * from Orders  where OrderId = @OrderId";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@OrderId", OrderId);
                sqlDa.Fill(dtblProduct);
            }
                 return View(dtblProduct);
        }
        public ActionResult Orderview()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("adminlogin");
            }
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Orders", sqlcon);
                sqlDa.Fill(dtblProduct);
            }
            return View(dtblProduct);
        }

        int cid;
        public ActionResult ConfirmOrder(int id)
        {   cid = id;
            Session["Orderid"] = id;
            return View();
        }
        public ActionResult logout() {
            Session["admin"] = null;
          return   RedirectToAction("adminlogin");
        }
        public ActionResult InsertConfirm(string OrderStatus, String date)
        {       cid = Convert.ToInt32(Session["Orderid"].ToString());
                Session["Orderid"] = null;
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   sqlcon.Open();
                string query = "UPDATE Orders SET  status=@OrderStatus,deliveryDate=@date where OrderId=@OrderId  ";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@OrderId", cid);
                sqlCmd.Parameters.AddWithValue("@date", date);
                sqlCmd.Parameters.AddWithValue("@OrderStatus", OrderStatus);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("MyProducts");
        }


        public ActionResult OrderProducts(int id)
        {      DataTable dtblOrderProducts = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   sqlcon.Open();
                SqlDataAdapter sqlDaOrder = new SqlDataAdapter("Select * from Cart where CartNumber=@id", sqlcon);
                sqlDaOrder.SelectCommand.Parameters.AddWithValue("@id", id);
                sqlDaOrder.Fill(dtblOrderProducts);
            }
            return View();
        }

        public ActionResult OrderProductDetails(int id)
        {    DataTable dtblOrderProducts = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   sqlcon.Open();
                SqlDataAdapter sqlDaOrder = new SqlDataAdapter("Select * from Orders where OrderId=@id", sqlcon);
                sqlDaOrder.SelectCommand.Parameters.AddWithValue("@id", id);
                sqlDaOrder.Fill(dtblOrderProducts);
            }
            DataTable dtblOrderscart = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                
                sqlcon.Open();
                SqlDataAdapter sqlDaOrder = new SqlDataAdapter("Select * from Cart where CartNumber=@CartNumber", sqlcon);
                sqlDaOrder.SelectCommand.Parameters.AddWithValue("@CartNumber", dtblOrderProducts.Rows[0][11]);
                sqlDaOrder.Fill(dtblOrderscart);
            }
            DataTable ProductTable = new DataTable();
            List<ProductsbyOrder> myproductbyorder = new List<ProductsbyOrder>();
            
            foreach (DataRow dtrow in dtblOrderscart.Rows) {
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {   sqlcon.Open();
                    SqlDataAdapter sqlDaOrder = new SqlDataAdapter("Select * from Product where ProductId=@ProductId", sqlcon);
                    sqlDaOrder.SelectCommand.Parameters.AddWithValue("@ProductId", dtrow["ProductId"]);
                    sqlDaOrder.Fill(ProductTable);
                }
                myproductbyorder.Add(new ProductsbyOrder()
                {
                 Quantity =Convert.ToInt32(dtblOrderscart.Rows[0][4].ToString()),
                 Products = ProductTable,
                    
                });
                ViewBag.products = myproductbyorder;
            }
            return View();
        }
        public ActionResult Delete(int id)
        {   using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   sqlcon.Open();
                string query = $"Delete  from Product where ProductId=@ProductId";
                SqlCommand sqldlt = new SqlCommand(query, sqlcon);
                sqldlt.Parameters.AddWithValue("@ProductId", id);
                
                sqldlt.ExecuteNonQuery();
            }
            return RedirectToAction("MyProducts");
        }
        public ActionResult AdminCustomOrder()
        {   DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from CustomOrder", sqlcon);
                sqlDa.Fill(dtblProduct);
            }
            return View(dtblProduct);
        }


        public ActionResult CustomOrderConfirm(int id)
        {
           Session["CustomOrderId"] = id;
            return View();
        }


        public ActionResult InsertCustomorderConfirm(string date, string bill,string OrderStatus) {
            cid = Convert.ToInt32(Session["CustomOrderId"].ToString());
            Session["CustomOrderId"] = null;
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   sqlcon.Open();
                string query = "UPDATE CustomOrder SET  status=@status,Bill=@Bill,deliveryDate=@deliveryDate where CO_ID=@CO_ID  ";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@CO_ID", cid);
                sqlCmd.Parameters.AddWithValue("@status", OrderStatus);
                sqlCmd.Parameters.AddWithValue("@Bill", bill);
                sqlCmd.Parameters.AddWithValue("@deliveryDate", date);
                sqlCmd.ExecuteNonQuery();
            }
            return    RedirectToAction("MyProducts");
        }

        public ActionResult adminlogin() {
            return View();
        }

    }


    }
