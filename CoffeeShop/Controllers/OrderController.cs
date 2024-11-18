using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using CoffeeShop.BAL;

namespace CoffeeShop.Controllers
{
    [CheckAccess]
    public class OrderController : Controller
    {
        private IConfiguration configuration;

        public OrderController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public static  List<OrderModel> orders = new List<OrderModel>
{
    new OrderModel { OrderID = 1, OrderDate = DateTime.Now, CustomerName = "Alice", PaymentMode = "Credit Card", TotalAmount = 199.99d, ShippingAddress = "123 Main St", UserID = 101 },
    new OrderModel { OrderID = 2, OrderDate = DateTime.Now.AddDays(-1), CustomerName = "Bob", PaymentMode = "PayPal", TotalAmount = 299.99d, ShippingAddress = "124 Main St", UserID = 101 },
    new OrderModel { OrderID = 3, OrderDate = DateTime.Now.AddDays(-2), CustomerName = "Charlie", PaymentMode = "Cash", TotalAmount = 399.99d, ShippingAddress = "125 Main St", UserID = 101 },
    new OrderModel { OrderID = 4, OrderDate = DateTime.Now.AddDays(-3), CustomerName = "David", PaymentMode = null, TotalAmount = 499.99d, ShippingAddress = "126 Main St", UserID = 101 },
    new OrderModel { OrderID = 5, OrderDate = DateTime.Now.AddDays(-4), CustomerName = "Eva", PaymentMode = "Credit Card", TotalAmount = 599.99d, ShippingAddress = "127 Main St", UserID = 101 },
    new OrderModel { OrderID = 6, OrderDate = DateTime.Now.AddDays(-5), CustomerName = "Frank", PaymentMode = "Debit Card", TotalAmount = 699.99d, ShippingAddress = "128 Main St", UserID = 101 },
    new OrderModel { OrderID = 7, OrderDate = DateTime.Now.AddDays(-6), CustomerName = "Grace", PaymentMode = null, TotalAmount = 799.99d, ShippingAddress = "129 Main St", UserID = 101 },
    new OrderModel { OrderID = 8, OrderDate = DateTime.Now.AddDays(-7), CustomerName = "Hank", PaymentMode = "Credit Card", TotalAmount = 899.99d, ShippingAddress = "130 Main St", UserID = 101 },
    new OrderModel { OrderID = 9, OrderDate = DateTime.Now.AddDays(-8), CustomerName = "Ivy", PaymentMode = "PayPal", TotalAmount = 999.99d, ShippingAddress = "131 Main St", UserID = 101 },
    new OrderModel { OrderID = 10, OrderDate = DateTime.Now.AddDays(-9), CustomerName = "Jack", PaymentMode = "Cash", TotalAmount = 1099.99d, ShippingAddress = "132 Main St", UserID = 101 }
};


        #region OrderList
        public IActionResult OrderList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "SP_SELECT_ALL_ORDER1";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
            //return View(orders);
        }
        #endregion


        #region AddEditOrder
        public IActionResult AddEditOrder()
        {
            PUserDropDown();
            return View();
        }
        #endregion


        #region OrderEdit
        public IActionResult OrderEdit(int OrderID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "SP_SELECT_ORDER_BY_ID";
            command.Parameters.Add("@OrderID", SqlDbType.Int).Value = OrderID;
            SqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            connection.Close();

            OrderModel orderModel = new OrderModel();
            orderModel.OrderID = Convert.ToInt32(dataTable.Rows[0]["OrderID"]);
            orderModel.OrderDate = Convert.ToDateTime(dataTable.Rows[0]["OrderDate"]);
            orderModel.CustomerName = dataTable.Rows[0]["CustomerName"].ToString();
            orderModel.PaymentMode = dataTable.Rows[0]["PaymentMode"].ToString();
            orderModel.TotalAmount = Convert.ToDouble(dataTable.Rows[0]["TotalAmount"]);
            orderModel.ShippingAddress = dataTable.Rows[0]["ShippingAddress"].ToString();
            orderModel.UserID = Convert.ToInt32(dataTable.Rows[0]["UserID"]);

            PUserDropDown();
            return View("AddEditOrder", orderModel);
        }
        #endregion


        #region OrderSave
        public IActionResult OrderSave(OrderModel orderModel)
        {

          
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (orderModel.OrderID == null)
                {
                    command.CommandText = "SP_INSERT_ORDER";
                }
                else
                {
                    command.CommandText = "SP_UPDATE_ORDER";
                    command.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderModel.OrderID;
                }
                command.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = orderModel.OrderDate;
                command.Parameters.Add("@CustomerName", SqlDbType.VarChar).Value = orderModel.CustomerName;
                command.Parameters.Add("@PaymentMode", SqlDbType.VarChar).Value = orderModel.PaymentMode;
                command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = orderModel.TotalAmount;
                command.Parameters.Add("@ShippingAddress", SqlDbType.VarChar).Value = orderModel.ShippingAddress;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = orderModel.UserID;
                command.ExecuteNonQuery();
            TempData["SuccessMessage"] = orderModel.OrderID == null ? "Order added successfully!" : "Order updated successfully!";

            PUserDropDown();
                return RedirectToAction("OrderList");
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
               /* userDropDownModel.UserID = Convert.ToInt32(data["UserID"]);*/
                userDropDownModel.UserName = data["UserName"].ToString();
                UserList.Add(userDropDownModel);
            }
            ViewBag.UserList = UserList;
        }
        #endregion

        #region Delete
        public IActionResult Delete(int OrderID)
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
                        command.CommandText = "SP_DELETE_ORDER";
                        command.Parameters.Add("@OrderID", SqlDbType.Int).Value = OrderID;
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("OrderList");
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
                return RedirectToAction("OrderList");
            }
            catch (Exception ex)
            {
                TempData["SuccessMessage"] = "Order deleted successfully!";
                Console.WriteLine(ex.ToString());
                return RedirectToAction("OrderList");
            }
        }
        #endregion
    }
}
