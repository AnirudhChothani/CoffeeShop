using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using CoffeeShop.BAL;

namespace CoffeeShop.Controllers
{
    [CheckAccess]
    public class OrderDetailsController : Controller
    {

        private IConfiguration configuration;

        public OrderDetailsController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public static  List<OrderDetailsModel> orderDetails = new List<OrderDetailsModel>
{
    new OrderDetailsModel { OrderDetailID = 1, OrderID = 1, ProductID = 1, Quantity = 2, Amount = 19.99d, TotalAmount = 39.98d, UserID = 101 },
    new OrderDetailsModel { OrderDetailID = 2, OrderID = 2, ProductID = 2, Quantity = 1, Amount = 29.99d, TotalAmount = 29.99d, UserID = 101 },
    new OrderDetailsModel { OrderDetailID = 3, OrderID = 3, ProductID = 3, Quantity = 3, Amount = 39.99d, TotalAmount = 119.9d, UserID = 101 },
    new OrderDetailsModel { OrderDetailID = 4, OrderID = 4, ProductID = 4, Quantity = 4, Amount = 49.99d, TotalAmount = 199.96d, UserID = 101 },
    new OrderDetailsModel { OrderDetailID = 5, OrderID = 5, ProductID = 5, Quantity = 2, Amount = 59.99d, TotalAmount = 119.98d, UserID = 101 },
    new OrderDetailsModel { OrderDetailID = 6, OrderID = 6, ProductID = 6, Quantity = 1, Amount = 69.99d, TotalAmount = 69.99d, UserID = 101 },
    new OrderDetailsModel { OrderDetailID = 7, OrderID = 7, ProductID = 7, Quantity = 3, Amount = 79.99d, TotalAmount = 239.97d, UserID = 101 },
    new OrderDetailsModel { OrderDetailID = 8, OrderID = 8, ProductID = 8, Quantity = 2, Amount = 89.99d, TotalAmount = 179.98d, UserID = 101 },
    new OrderDetailsModel { OrderDetailID = 9, OrderID = 9, ProductID = 9, Quantity = 1, Amount = 99.99d, TotalAmount = 99.99d, UserID = 101 },
    new OrderDetailsModel { OrderDetailID = 10, OrderID = 10, ProductID = 10, Quantity = 4, Amount = 109.99d, TotalAmount = 439.96d, UserID = 101 }
};


        #region OrderDetailList
        public IActionResult OrderDetailList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "SP_SELECT_ALL_ORDERDETAILS ";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
            //return View(orderDetails);
        }
        #endregion

        #region AddEditOrderDetails
        public IActionResult AddEditOrderDetails()
        {
            ProductDropDown();
            PUserDropDown();
            OrderDropDown();
            return View();
        }
        #endregion

        #region OrderDetailsEdit

        public IActionResult OrderDetailsEdit(int OrderDetailID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "SP_SELECT_ORDERDETAILS_BY_ID";
            command.Parameters.Add("@OrderDetailID", SqlDbType.Int).Value = OrderDetailID;
            SqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            connection.Close();

            OrderDetailsModel orderDetailsModel = new OrderDetailsModel();
            orderDetailsModel.OrderDetailID = Convert.ToInt32(dataTable.Rows[0]["OrderDetailID"]);
            orderDetailsModel.OrderID = Convert.ToInt32(dataTable.Rows[0]["OrderID"]);
            orderDetailsModel.ProductID = Convert.ToInt32(dataTable.Rows[0]["ProductID"]);
            orderDetailsModel.Quantity = Convert.ToInt32(dataTable.Rows[0]["Quantity"]);
            orderDetailsModel.Amount = Convert.ToDouble(dataTable.Rows[0]["Amount"]);
            orderDetailsModel.TotalAmount = Convert.ToDouble(dataTable.Rows[0]["TotalAmount"]);
            orderDetailsModel.UserID = Convert.ToInt32(dataTable.Rows[0]["UserID"]);
            ProductDropDown();
            PUserDropDown();
            OrderDropDown();
            return View("AddEditOrderDetails", orderDetailsModel);
        }
        #endregion

        #region PUserDropDown
        public void PUserDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "SP_USER_DROPDOWN";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            List<UserDropDownModel> UserList = new List<UserDropDownModel>();
            foreach (DataRow data in dataTable1.Rows)
            {
                UserDropDownModel userDropDownModel = new UserDropDownModel();
                userDropDownModel.UserID = Convert.ToInt32(data["UserID"]);
                userDropDownModel.UserName = data["UserName"].ToString();
                UserList.Add(userDropDownModel);
            }
            ViewBag.UserList = UserList;
        }
        #endregion

        #region OrderDropDown
        public void OrderDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "SP_ORDER_DROPDOWN";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            List<OrderDropDownModel> OrderList = new List<OrderDropDownModel>();
            foreach (DataRow data in dataTable1.Rows)
            {
                OrderDropDownModel orderDropDownModel = new OrderDropDownModel();
                orderDropDownModel.OrderID = Convert.ToInt32(data["OrderID"]);
                orderDropDownModel.CustomerName = data["CustomerName"].ToString();
                OrderList.Add(orderDropDownModel);
            }
            ViewBag.OrderList = OrderList;
        }
        #endregion


        #region ProductDropDown
        public void ProductDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "SP_PRODUCT_DROPDOWN";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            List<ProductDropDownModel> ProductList = new List<ProductDropDownModel>();
            foreach (DataRow data in dataTable1.Rows)
            {
                ProductDropDownModel productDropDownModel = new ProductDropDownModel();
                productDropDownModel.ProductID = Convert.ToInt32(data["ProductID"]);
                productDropDownModel.ProductName = data["ProductName"].ToString();
                ProductList.Add(productDropDownModel);
            }
            ViewBag.ProductList = ProductList;
        }
        #endregion


        #region Save
        public IActionResult Save(OrderDetailsModel orderDetailsModel)
        {
           
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (orderDetailsModel.OrderDetailID == null)
                {
                    command.CommandText = "SP_INSERT_ORDERDETAILS";
                }
                else
                {
                    command.CommandText = "SP_UPDATE_ORDERDETAILS";
                    command.Parameters.Add("@OrderDetailID", SqlDbType.Int).Value = orderDetailsModel.OrderDetailID;
                }
                command.Parameters.Add("@OrderID", SqlDbType.VarChar).Value = orderDetailsModel.OrderID;
                command.Parameters.Add("@ProductID", SqlDbType.VarChar).Value = orderDetailsModel.ProductID;
                command.Parameters.Add("@Quantity", SqlDbType.VarChar).Value = orderDetailsModel.Quantity;
                command.Parameters.Add("@Amount", SqlDbType.VarChar).Value = orderDetailsModel.Amount;
                command.Parameters.Add("@TotalAmount", SqlDbType.VarChar).Value = orderDetailsModel.TotalAmount;
                command.Parameters.Add("@UserID", SqlDbType.VarChar).Value = orderDetailsModel.UserID;
                command.ExecuteNonQuery();
                TempData["SuccessMessage"] = orderDetailsModel.OrderDetailID == null ? "OrderDetail added successfully!" : "OrderDetail updated successfully!";

                ProductDropDown();
                PUserDropDown();
                 OrderDropDown();
                return RedirectToAction("OrderDetailList");
            

        }
        #endregion

        #region Delete

        public IActionResult Delete(int OrderDetailID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_DELETE_ORDERDETAILS";
                        command.Parameters.Add("@OrderDetailID", SqlDbType.Int).Value = OrderDetailID;
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("OrderDetailList");
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547) // SQL Server error code for foreign key constraint violation
                {
                    TempData["ErrorMessage"] = "Unable to delete the product because it is referenced in another record.";
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred while deleting the product. Please try again.";
                }
                Console.WriteLine(ex.ToString());
                return RedirectToAction("OrderDetailList");
            }
            catch (Exception ex)
            {
                TempData["SuccessMessage"] = "OrderDetail deleted successfully!";
                Console.WriteLine(ex.ToString());
                return RedirectToAction("OrderDetailList");
            }
        }
        #endregion
    }
}
