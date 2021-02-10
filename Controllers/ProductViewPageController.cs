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
    public class ProductViewPageController : Controller
    {
        string connectionString = @"Data Source = DESKTOP-2V8523P\SQLEXPRESS;  Initial Catalog =OnlineShoppingStore;  Integrated Security  = true";
        public ActionResult ViewDetailsProduct(int id)
        {
            DataTable dtblProduct = new DataTable();
            DataTable dtblRelatedProductTable = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Product  where ProductId= @ProductId", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@ProductId", id);
                sqlDa.Fill(dtblProduct);
            }

            Session["Productidforcotact"] = Convert.ToInt32(dtblProduct.Rows[0][0].ToString());
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Product  where SubCategoryName= @SubCategoryName", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@SubCategoryName", dtblProduct.Rows[0][2].ToString());
                sqlDa.Fill(dtblRelatedProductTable);
            }
            ProductViewPageModel ProductView = new ProductViewPageModel();
            ProductView.ProductDatatable = dtblProduct;
            ProductView.RelatedProductTable = dtblRelatedProductTable;
            return View(ProductView);
        }

        public ActionResult productContact(string name, string mail, string message)
        {

            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Insert into ProductViewByContact  Values(@name,@mail,@message) ";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@name", name);
                sqlCmd.Parameters.AddWithValue("@mail", mail);
                sqlCmd.Parameters.AddWithValue("@message", message);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("../Home/Index");
        }
        public ActionResult Categoryview(int id)
        {
            DataTable dtblRelatedProductTable = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Product  where CategoryId= @CategoryId", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@CategoryId", id);
                sqlDa.Fill(dtblRelatedProductTable);
            }
            return View(dtblRelatedProductTable);
        }
        public ActionResult VirtualTrail(int id)
        {
            DataTable dtblProduct = new DataTable();
            DataTable dtblRelatedProductTable = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Product  where ProductId= @ProductId", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@ProductId", id);
                sqlDa.Fill(dtblProduct);
            }
            return View(dtblProduct);
        }
        public ActionResult HotDeal()
        {
            string discount = "50%";
            DataTable dtblProduct = new DataTable();
            DataTable dtblRelatedProductTable = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Product  where Discount= @discount", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@discount", discount);
                sqlDa.Fill(dtblProduct);
            }
            return View(dtblProduct);

        }

        

        public ActionResult NewAccessories()
        {
            DataTable dtblProduct = new DataTable();
            string Accessories = "Accessories";
            string tag = "New";
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Product  where SubCategoryName= @SubCategoryName and Tag=@tag", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@SubCategoryName", Accessories);
                sqlDa.SelectCommand.Parameters.AddWithValue("@tag", tag);
                sqlDa.Fill(dtblProduct);
            }
            return View(dtblProduct);
        }
    }
}