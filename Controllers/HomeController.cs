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
    public class HomeController : Controller
    {  string connectionString = @"Data Source = DESKTOP-2V8523P\SQLEXPRESS;  Initial Catalog =OnlineShoppingStore;  Integrated Security  = true";
       [HttpGet]
        public ActionResult IndexSearch(string search)
        {  DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {sqlcon.Open();
                //SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Product where upper(ProductName) =@search or lower(ProductName)=@search ", sqlcon);
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Product where upper(ProductName) =@search or lower(ProductName)=@search or upper(SubcategoryName) =@search or lower(SubcategoryName)= @search or upper(ProductColor) = @search or lower(ProductColor) =@search or upper(Tag) = @search or lower(Tag)=@search or upper(ProductSize) =@search  or lower(ProductSize)= @search or upper(BrandName)= @search or lower(BrandName)= @search", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@search", search);
             sqlDa.Fill(dtblProduct);
            }
          return View(dtblProduct);
        }

        [HttpGet]
        public ActionResult Index()
        {   List<combine> multipleProducts = new List<combine>();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                string eid = "Eid Special";
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("  select * from Product WHere not Tag =@Tag ", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Tag", eid);
                sqlDa.Fill(dtblProduct);
            }
            int m = dtblProduct.Rows.Count;
               combine comb = new combine();
               comb.ProductTable = dtblProduct;        
                DataTable dtblProductAccessories = new DataTable();
                string Accessories = "Accessories";
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    sqlcon.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Product  where SubCategoryName= @SubCategoryName", sqlcon);
                    sqlDa.SelectCommand.Parameters.AddWithValue("@SubCategoryName", Accessories);
                    sqlDa.Fill(dtblProductAccessories);
                }
            DataTable dtblProductEid = new DataTable();
            
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   string eid= "Eid Special";
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("  select * from Product WHere Tag =@Tag ", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Tag", eid);
                sqlDa.Fill(dtblProductEid);
            }

            multipleProducts.Add(new combine()
            {
                ProductTable = dtblProduct,
                AccessoriesTable = dtblProductAccessories,
                EidProducts = dtblProductEid
            });

            

            return View(multipleProducts);
        }
        public ActionResult ViewAll() {

              DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                string eid = "Eid Special";
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("  select * from Product  ", sqlcon);
               
                sqlDa.Fill(dtblProduct);
            }
            return View(dtblProduct);
        }

       
        public int IncCartNumber()
        {
            int carttracid = 1;
            int cartnumber = 0;
            int prevQuantity = 1;
            //     int ProductId = Convert.ToInt32(Request.QueryString["id"]);
            DataTable dtblProduct = new DataTable();
            DataTable dtblProductcart = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   sqlcon.Open();
                // SqlCommand sqlCmdgetcart = new SqlCommand(query, sqlcon);
                string query = $"Select  * from Carttrac  where carttracid = @carttracid";
                SqlDataAdapter sqlDacart = new SqlDataAdapter(query, sqlcon);
                sqlDacart.SelectCommand.Parameters.AddWithValue("@carttracid", carttracid);
                sqlDacart.Fill(dtblProductcart);
                if (dtblProductcart.Rows.Count == 1)
                {  cartnumber = Convert.ToInt32(dtblProductcart.Rows[0][1]);
                  //  cartnumber = cartnumber + 1;
                }
                string cartupdatequery = "UPDATE Carttrac SET  cartnumber= @cartnumber where carttracid = @carttracid ";
                SqlCommand sqlCmdupdatecart = new SqlCommand(cartupdatequery, sqlcon);
                sqlCmdupdatecart.Parameters.AddWithValue("@carttracid", carttracid);
                sqlCmdupdatecart.Parameters.AddWithValue("@cartnumber", cartnumber);
                sqlCmdupdatecart.ExecuteNonQuery();
             }
          
            return cartnumber;

        }

        public void UpdateCartnum(int cartnumber)
        {  cartnumber = cartnumber + 1;
            int carttracid = 1;
            DataTable dtblProduct = new DataTable();
            DataTable dtblProductcart = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   sqlcon.Open();
                //SqlCommand sqlCmdupdatecart= new SqlCommand(query, sqlcon);
                string cartupdatequery = "UPDATE Carttrac SET  cartnumber= @cartnumber where carttracid = @carttracid ";
                SqlCommand sqlCmdupdatecart = new SqlCommand(cartupdatequery, sqlcon);
                sqlCmdupdatecart.Parameters.AddWithValue("@carttracid", carttracid);
                sqlCmdupdatecart.Parameters.AddWithValue("@cartnumber", cartnumber);
                sqlCmdupdatecart.ExecuteNonQuery();

            }

        }
        public void InsertIntoCart(int Quantity, DataTable dtblProduct,int num) {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                string query = "Insert into Cart  Values(@CartNumber,@ProductName,@ProductId,@Quantity) ";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@CartNumber",num );
                sqlCmd.Parameters.AddWithValue("@ProductName",dtblProduct.Rows[0][2]);
                sqlCmd.Parameters.AddWithValue("@ProductId", dtblProduct.Rows[0][0]);
                sqlCmd.Parameters.AddWithValue("@Quantity", Quantity);
                sqlCmd.ExecuteNonQuery();
            }


        }

        public void UpdateCart(int Quantity, DataTable dtblProduct, int num) {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   sqlcon.Open();
                string cartupdatequery = "UPDATE Cart SET  Quantity= @Quantity where CartNumber=CartNumber and ProductId = @ProductId  ";
                SqlCommand sqlCmdupdatecart = new SqlCommand(cartupdatequery, sqlcon);
                sqlCmdupdatecart.Parameters.AddWithValue("@ProductId", dtblProduct.Rows[0][0]);
                sqlCmdupdatecart.Parameters.AddWithValue("@Quantity", Quantity);
                sqlCmdupdatecart.Parameters.AddWithValue("@CartNumber", num);
                sqlCmdupdatecart.ExecuteNonQuery();
             }

        }
     //   int totalcart = 1;
         
        public ActionResult AddtoCart(int id) {
            int carttracid = 1;
            int cartnumber = 1;
            bool flag = true;
            int prevQuantity = 1;
            DataTable dtblProduct = new DataTable();
            DataTable dtblProductcart = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Product where ProductId = @ProductId", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@ProductId", id);
                sqlDa.Fill(dtblProduct);
            }
            if (Session["cart"] == null)
            {    cartnumber = IncCartNumber();
                 MyCartnumber nm = new MyCartnumber();
                 Session["cartNumber"]  = cartnumber.ToString();
                 UpdateCartnum(cartnumber);
                 List<item> cart = new List<item>();
                cart.Add(new item()
                {   Quantity = 1,
                    ProductCartTable = dtblProduct,
                    carttracnumber = cartnumber,
                    totalcart = 1
                });
                InsertIntoCart(1, dtblProduct, cartnumber);
                Session["cart"] = cart;
             }
            else {
                int sendCardnumber = IncCartNumber() - 1;
                List<item> cart = (List<item>)Session["cart"];
                DataRow dtrow = dtblProduct.Rows[0];
                foreach (var item in cart.ToList())
                {   int Pid = Convert.ToInt32(item.ProductCartTable.Rows[0][0]);
                    if (Pid == id)
                    { flag = false;
                     prevQuantity = item.Quantity +1;
                     cart.Remove(item);
                     UpdateCart(prevQuantity, dtblProduct, sendCardnumber);
                     break;
                    }
                }
                if (flag) {
                    InsertIntoCart(prevQuantity, dtblProduct, sendCardnumber);
                }
                cart.Add(new item()
                {   Quantity = prevQuantity,
                    ProductCartTable = dtblProduct,
                    carttracnumber = IncCartNumber() - 1,
                    //totalcart = totalcart + 1
                });

                  Session["cart"] = cart;

            }
            return Redirect("Index");

        }

        public ActionResult AddtoCartview() {

            return View();
        }

        public ActionResult About()
        {
        ViewBag.Message = "Your application description page.";
         return View();
        }

        public ActionResult Contact()
        {ViewBag.Message = "Your contact page.";
         return View();
        }

        public ActionResult userlogin()
        {
            return View();
        }
        public ActionResult Registration()
        {
            return View();
        }

        public ActionResult regform(string firstname,string lastname,string username,string password, string gender, string email, string phone)
        {
            DataTable finduser = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {sqlcon.Open();
             SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Users where username = @username", sqlcon);
             sqlDa.SelectCommand.Parameters.AddWithValue("@username", username);
             sqlDa.SelectCommand.Parameters.AddWithValue("@userpassword", password);
             sqlDa.Fill(finduser);
            }
            if (finduser.Rows.Count==1)
            {TempData["usernameexist"] = "Alreday username exist";
             return RedirectToAction("Registration");
            }
            DataTable findusermail = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {sqlcon.Open();
             SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Users where usermail = @email ", sqlcon);
             sqlDa.SelectCommand.Parameters.AddWithValue("@email", email);
             sqlDa.Fill(findusermail);
            }
            if (findusermail.Rows.Count == 1)
            {TempData["usernameexist"] = "Alreday mail exist";
             return RedirectToAction("Registration");
            }
            int countpasswordlength = password.Length;
            if (countpasswordlength<8) {
                TempData["lowerpassword"] = "Password Must be at least 8 Character";
             return RedirectToAction("Registration");
            }
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {sqlcon.Open();
             string query = "Insert into Users  Values(@userfirstname,@userlastname,@username,@userpassword,@usermail,@usercontact,@usergender) ";
             SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
             sqlCmd.Parameters.AddWithValue("@userfirstname", firstname);
             sqlCmd.Parameters.AddWithValue("@userlastname", lastname);
             sqlCmd.Parameters.AddWithValue("@username", username);
             sqlCmd.Parameters.AddWithValue("@userpassword", password);
             sqlCmd.Parameters.AddWithValue("@usermail", email);
             sqlCmd.Parameters.AddWithValue("@usercontact", phone);
             sqlCmd.Parameters.AddWithValue("@usergender", gender);
             sqlCmd.ExecuteNonQuery();
            }
             return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int id) {
            int sendCardnumber = IncCartNumber() - 1;

            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {sqlcon.Open();
            string cartupdatequery = "Delete From Cart where CartNumber=@CartNumber and  ProductId=@ProductId ";
             SqlCommand sqlCmdupdatecart = new SqlCommand(cartupdatequery, sqlcon);
             sqlCmdupdatecart.Parameters.AddWithValue("@CartNumber", sendCardnumber);
             sqlCmdupdatecart.Parameters.AddWithValue("@ProductId", id);
             sqlCmdupdatecart.ExecuteNonQuery();
                
            }

            List<item> cart = (List<item>)Session["cart"];
          foreach (var item in cart) {
           int Pid=Convert.ToInt32(item.ProductCartTable.Rows[0][0]);
           if (Pid == id) {
            cart.Remove(item);
            break;
           }
          }
            Session["cart"] = cart;
            return RedirectToAction("Index");
    }
        public ActionResult Addtocartedit()
        {
            return View();
        }
        public ActionResult Login()
        {
           return View();

        }
        public ActionResult AddToCartinc(int id) {
            int carttracid = 1;
            int cartnumber = 1;
            bool flag = true;
            int prevQuantity = 1;
            DataTable dtblProduct = new DataTable();
            DataTable dtblProductcart = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Product where ProductId = @ProductId", sqlcon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@ProductId", id);
                sqlDa.Fill(dtblProduct);
            }
            if (Session["cart"] == null)
            {
                cartnumber = IncCartNumber();
                MyCartnumber nm = new MyCartnumber();
                Session["cartNumber"] = cartnumber.ToString();
                UpdateCartnum(cartnumber);
                List<item> cart = new List<item>();
                cart.Add(new item()
                {
                    Quantity = 1,
                    ProductCartTable = dtblProduct,
                    carttracnumber = cartnumber,
                    totalcart = 1
                });
                InsertIntoCart(1, dtblProduct, cartnumber);
                Session["cart"] = cart;

            }
            else
            {
                int sendCardnumber = IncCartNumber() - 1;


                List<item> cart = (List<item>)Session["cart"];
                DataRow dtrow = dtblProduct.Rows[0];
                foreach (var item in cart.ToList())
                {
                    int Pid = Convert.ToInt32(item.ProductCartTable.Rows[0][0]);
                    if (Pid == id)
                    {  flag = false;
                        prevQuantity = item.Quantity + 1;
                        cart.Remove(item);
                        UpdateCart(prevQuantity, dtblProduct, sendCardnumber);
                        break;
                    }
                }
                if (flag)
                {
                    InsertIntoCart(prevQuantity, dtblProduct, sendCardnumber);
                }
                cart.Add(new item()
                {
                    Quantity = prevQuantity,
                    ProductCartTable = dtblProduct,
                    carttracnumber = IncCartNumber() - 1,
                    //totalcart = totalcart + 1
                });
               Session["cart"] = cart;
            }
            return Redirect("Addtocartedit");
        }


        public ActionResult DecreaseQty(int id) 
        {   int ProductId = Convert.ToInt32(Request.QueryString["id"]);
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {sqlcon.Open();
             SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Product where ProductId = @search", sqlcon);
             sqlDa.SelectCommand.Parameters.AddWithValue("@search", id);
             sqlDa.Fill(dtblProduct);
            }
            if (Session["cart"] != null)
            {   List<item> cart = (List<item>)Session["cart"];
                foreach (var item in cart)
                {   int pid = Convert.ToInt32(item.ProductCartTable.Rows[0][0]);
                    if (pid== id)
                    {   int prevQty = item.Quantity;
                        int sendCardnumber = IncCartNumber() - 1;
                        UpdateCart(prevQty-1, dtblProduct, sendCardnumber);
                        if (prevQty > 0)
                        {   cart.Remove(item);
                            cart.Add(new item()
                            {ProductCartTable = dtblProduct,
                             Quantity = prevQty - 1
                            });
                        }
                        break;
                    }
                }
                Session["cart"] = cart;
            }
         return Redirect("Addtocartedit");
        }


        [HttpGet]
        public ActionResult log(string username, string password) {
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {sqlcon.Open();
             SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Users where username = @username  and userpassword=@userpassword", sqlcon);
             sqlDa.SelectCommand.Parameters.AddWithValue("@username", username);
             sqlDa.SelectCommand.Parameters.AddWithValue("@userpassword", password);
             sqlDa.Fill(dtblProduct);
            }
            foreach (DataRow item in dtblProduct.Rows)
            {
                if (item["username"].ToString()==username && item["userpassword"].ToString()==password)
                {Session["username"] = username;
                 Session["firstname"] = item["userfirstname"];
                 break;
                }
            }
              if (Session["username"] == null)
              {
               TempData["errormessage"] = "Username or Password Is Incorrect";
               return RedirectToAction("Login");
              }
              else {
               return Redirect("~/UserInfo/UserProfile");
              } 
        }

        public ActionResult OrderDetails()
        {
            return View();
        }
        public ActionResult logout() {
            Session["username"] = null;
            Session["cart"] = null;
            return RedirectToAction("Login");
        }

        public ActionResult MyOrder(string FirsrName, string LastName, string Mail, string OrderAddress, string City, string Country, int ZipCode, string PhoneNumber, string PaymentMethod, string ShippingType)
        {   DataTable dtblProduct = new DataTable();
            string currentusername;
            int lineTotal = 0;
            DataTable dtblProductusers = new DataTable();
            if (Session["username"] == null)
            {
                return RedirectToAction("../Home/Login");
            }
            else
            {   currentusername = Session["username"].ToString();
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                { sqlcon.Open();
                  SqlDataAdapter sqlDauser = new SqlDataAdapter("Select * from Users where username = @username", sqlcon);
                  sqlDauser.SelectCommand.Parameters.AddWithValue("@username", currentusername);
                  sqlDauser.Fill(dtblProductusers);
                }
                foreach (item item in (List<item>)Session["cart"])
                {  int productPrice = Convert.ToInt32(item.ProductCartTable.Rows[0][6]);
                  lineTotal = lineTotal+Convert.ToInt32(item.Quantity * productPrice);
                }
               using (SqlConnection sqlcon = new SqlConnection(connectionString))
               {string status = "Not Set Yet By Admin";
                string Bill = "Not Set Yet By Admin";
                string deliveryDate = "Not Se Yet By Admin";
                sqlcon.Open();
                string query = "Insert into Orders  Values(@FirsrName,@LastName,@Mail,@OrderAddress,@City,@Country,@ZipCode," +
                "@PhoneNumber,@PaymentMethod,@ShippingType,@cartnumber,@userid,@Totalbill,@status,@deliveryDate) ";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@FirsrName", FirsrName);
                sqlCmd.Parameters.AddWithValue("@LastName", LastName);
                sqlCmd.Parameters.AddWithValue("@Mail", Mail);
                sqlCmd.Parameters.AddWithValue("@OrderAddress", OrderAddress);
                sqlCmd.Parameters.AddWithValue("@City", City);
                sqlCmd.Parameters.AddWithValue("@Country", Country);
                sqlCmd.Parameters.AddWithValue("@ZipCode", ZipCode);
                sqlCmd.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                sqlCmd.Parameters.AddWithValue("@PaymentMethod", PaymentMethod);
                sqlCmd.Parameters.AddWithValue("@ShippingType", ShippingType);
                sqlCmd.Parameters.AddWithValue("@cartnumber", Convert.ToInt32(Convert.ToInt32(Session["cartNumber"].ToString())));
                sqlCmd.Parameters.AddWithValue("@userid", Convert.ToInt32(dtblProductusers.Rows[0][0]));
                sqlCmd.Parameters.AddWithValue("@Totalbill", lineTotal);
                sqlCmd.Parameters.AddWithValue("@status", status);
                sqlCmd.Parameters.AddWithValue("@deliveryDate", deliveryDate);
                sqlCmd.ExecuteNonQuery();
               }
                Session["cartNumber"] = null;
                Session["cart"] = null;
                return Redirect("~/UserInfo/UserProfile");
            }
        }

         

        public ActionResult CustomOrder()
        {
            return View();
        }

        public ActionResult InsertCustomOrder(string cottonname,string usergender, string Chest, string Neck, string Waist, string Seat, string productslength, string Shoulder, string armlength)
        {   DataTable dtblProductusers = new DataTable();
            string currentusername;
            if (Session["username"] == null)
            {
                return RedirectToAction("~/Home/Login");
            }
            else
            {   currentusername = Session["username"].ToString();
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {sqlcon.Open();
                 SqlDataAdapter sqlDauser = new SqlDataAdapter("Select * from Users where username = @username", sqlcon);
                 sqlDauser.SelectCommand.Parameters.AddWithValue("@username", currentusername);
                 sqlDauser.Fill(dtblProductusers);
                }

            }
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {   string status = "Not Set Yet By Admin";
                string Bill = "Not Set Yet By Admin";
                string deliveryDate = "Not Se Yet By Admin";
                sqlcon.Open();
                string query = "Insert into CustomOrder  Values(@userid,@usergender,@cottonname,@Chest,@Neck,@Waist,@Seat,@productslength,@Shoulder,@armlength,@status,@Bill,@deliveryDate)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                sqlCmd.Parameters.AddWithValue("@userid", dtblProductusers.Rows[0][0]);
                sqlCmd.Parameters.AddWithValue("@usergender", usergender);
                sqlCmd.Parameters.AddWithValue("@cottonname", cottonname);
                sqlCmd.Parameters.AddWithValue("@Chest", Chest);
                sqlCmd.Parameters.AddWithValue("@Neck", Neck);
                sqlCmd.Parameters.AddWithValue("@Waist", Waist);
                sqlCmd.Parameters.AddWithValue("@Seat", Seat);
                sqlCmd.Parameters.AddWithValue("@productslength", productslength);
                sqlCmd.Parameters.AddWithValue("@Shoulder", Shoulder);
                sqlCmd.Parameters.AddWithValue("@armlength", armlength);
                sqlCmd.Parameters.AddWithValue("@status", status);
                sqlCmd.Parameters.AddWithValue("@Bill", Bill);
                sqlCmd.Parameters.AddWithValue("@deliveryDate", deliveryDate);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
        public ActionResult ProductView() {
            return View();
        }
        public ActionResult Forgot() {
            return View();
        }
        public ActionResult SetPassword(string username,string newpassword) {
            DataTable finduser = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {sqlcon.Open();
             SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Users where username = @username", sqlcon);
             sqlDa.SelectCommand.Parameters.AddWithValue("@username", username);
             sqlDa.Fill(finduser);
            }
            if (finduser.Rows.Count == 0)
            {
                TempData["usernameexist"] = "Please Insert A Currect Username";
                return RedirectToAction("Forgot");
            }
            int countpasswordlength = newpassword.Length;
            if (countpasswordlength < 8)
            {TempData["usernameexist"] = "Password Must be at least 8 Character";
             return RedirectToAction("Forgot");
            }
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {sqlcon.Open();
             string cartupdatequery = "UPDATE Users SET  userpassword= @userpassword where username = @username ";
             SqlCommand sqlCmdupdatecart = new SqlCommand(cartupdatequery, sqlcon);
             sqlCmdupdatecart.Parameters.AddWithValue("@username", username);
             sqlCmdupdatecart.Parameters.AddWithValue("@userpassword", newpassword);
             sqlCmdupdatecart.ExecuteNonQuery();
             return RedirectToAction("Index");
            }
        }

        public ActionResult CategoryByView(int id) {
            DataTable dtblRelatedProductTable = new DataTable();
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {sqlcon.Open();
             SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Product  where CategoryId= @CategoryId", sqlcon);
             sqlDa.SelectCommand.Parameters.AddWithValue("@CategoryId", id);
             sqlDa.Fill(dtblRelatedProductTable);
            }
            return View(dtblRelatedProductTable);
        }
       
    }
}