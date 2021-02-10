using OnlineShoppingStore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShoppingStore.Controllers
{
    public class UserInfoController : Controller
    {   string connectionString = @"Data Source = DESKTOP-2V8523P\SQLEXPRESS;  Initial Catalog =OnlineShoppingStore;  Integrated Security  = true";
        // GET: UserInfo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult A()
        {
            return Content(5.ToString());
        }
        public ActionResult UserProfile()
        {
            DataTable dtblProduct = new DataTable();
            string currentusername;
            DataTable dtblProductusers = new DataTable();
            if (Session["username"] == null)
            {
                return RedirectToAction("../Home/Login");
            }
            else
            {   currentusername = Session["username"].ToString();
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    sqlcon.Open();
                    SqlDataAdapter sqlDauser = new SqlDataAdapter("Select * from Users where username = @username", sqlcon);
                    sqlDauser.SelectCommand.Parameters.AddWithValue("@username", currentusername);
                    sqlDauser.Fill(dtblProductusers);
                }
            }
            DataTable dtblOrder = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   sqlcon.Open();
                SqlDataAdapter sqlDaOrder = new SqlDataAdapter("Select * from Orders where userid=@userid", sqlcon);
                sqlDaOrder.SelectCommand.Parameters.AddWithValue("@userid", dtblProductusers.Rows[0][0]);
                sqlDaOrder.Fill(dtblOrder);
            }
            List<multipleOrders> multipleOrders = new List<multipleOrders>();
            foreach (DataRow dtrow in dtblOrder.Rows)
            {   multipleOrders.Add(new multipleOrders()
                {
                    MultipleOrder = dtblOrder,
                });

            }
            DataTable CustomdtblOrder = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDaOrder = new SqlDataAdapter("Select * from CustomOrder where userid=@userid", sqlcon);
                sqlDaOrder.SelectCommand.Parameters.AddWithValue("@userid", dtblProductusers.Rows[0][0]);
                sqlDaOrder.Fill(CustomdtblOrder);
            }

            foreach (DataRow dtrow in CustomdtblOrder.Rows)
            {   multipleOrders.Add(new multipleOrders()
                {
                    CustomOrder = CustomdtblOrder,
                });

            }
            return View(multipleOrders);
        }
        
      
        public ActionResult GetUserProductsbyOrder() {
            string currentusername;
            int lineTotal = 0;
            DataTable dtblProductusers = new DataTable();
            if (Session["username"] == null)
            {
                return RedirectToAction("../Home/Login");
            }
          else { 
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {currentusername = Session["username"].ToString();
                sqlcon.Open();
                SqlDataAdapter sqlDauser = new SqlDataAdapter("Select * from Users where username = @username", sqlcon);
                sqlDauser.SelectCommand.Parameters.AddWithValue("@username", currentusername);
                sqlDauser.Fill(dtblProductusers);
            }
            DataTable dtblOrderProducts = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDaOrder = new SqlDataAdapter("Select * from Orders where userid=@userid", sqlcon);
                sqlDaOrder.SelectCommand.Parameters.AddWithValue("@userid", dtblProductusers.Rows[0][0]);
                sqlDaOrder.Fill(dtblOrderProducts);
            }
                List<multipleOrders> multipleOrders = new List<multipleOrders>();

                foreach (DataRow dtrow in dtblOrderProducts.Rows)
                {
                  

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

            foreach (DataRow dtrow in dtblOrderscart.Rows)
            {
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    sqlcon.Open();
                    SqlDataAdapter sqlDaOrder = new SqlDataAdapter("Select * from Product where ProductId=@ProductId", sqlcon);
                    sqlDaOrder.SelectCommand.Parameters.AddWithValue("@ProductId", dtrow["ProductId"]);
                    sqlDaOrder.Fill(ProductTable);
                }
                myproductbyorder.Add(new ProductsbyOrder()
                {
                    Quantity = Convert.ToInt32(dtblOrderscart.Rows[0][4].ToString()),
                    Products = ProductTable,

                });
                ViewBag.products = myproductbyorder;
            }
           }
            return View();

        }
        public ActionResult OrderProductDetails(int id)
        {
            DataTable dtblOrderProducts = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
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

            foreach (DataRow dtrow in dtblOrderscart.Rows)
            {
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    sqlcon.Open();
                    SqlDataAdapter sqlDaOrder = new SqlDataAdapter("Select * from Product where ProductId=@ProductId", sqlcon);
                    sqlDaOrder.SelectCommand.Parameters.AddWithValue("@ProductId", dtrow["ProductId"]);
                    sqlDaOrder.Fill(ProductTable);
                }
                myproductbyorder.Add(new ProductsbyOrder()
                {
                    Quantity = Convert.ToInt32(dtblOrderscart.Rows[0][4].ToString()),
                    Products = ProductTable,

                });
                ViewBag.products = myproductbyorder;
            }
            return View();
        }
    }
}