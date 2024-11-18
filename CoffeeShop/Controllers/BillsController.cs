using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using CoffeeShop.BAL;

namespace CoffeeShop.Controllers
{
    [CheckAccess]


    public class BillsController : Controller
    {

        private IConfiguration configuration;

        public BillsController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public static  List<BillsModel> bills = new List<BillsModel>
        {
    new BillsModel { BillID = 1, BillNumber = "B1001", BillDate = DateTime.Now, OrderID = 1, TotalAmount = 399.98d, Discount = 20.00d, NetAmount = 379.98d, UserID = 101 },
    new BillsModel { BillID = 2, BillNumber = "B1002", BillDate = DateTime.Now.AddDays(-1), OrderID = 2, TotalAmount = 29.99d, Discount = 0, NetAmount = 29.99d, UserID = 101 },
    new BillsModel { BillID = 3, BillNumber = "B1003", BillDate = DateTime.Now.AddDays(-2), OrderID = 3, TotalAmount = 119.97d, Discount = 10.00d, NetAmount = 109.97d, UserID = 101 },
    new BillsModel { BillID = 4, BillNumber = "B1004", BillDate = DateTime.Now.AddDays(-3), OrderID = 4, TotalAmount = 199.96d, Discount = 15.00d, NetAmount = 184.96d, UserID = 101 },
    new BillsModel { BillID = 5, BillNumber = "B1005", BillDate = DateTime.Now.AddDays(-4), OrderID = 5, TotalAmount = 119.98d, Discount = 0, NetAmount = 119.98d, UserID = 101 },
    new BillsModel { BillID = 6, BillNumber = "B1006", BillDate = DateTime.Now.AddDays(-5), OrderID = 6, TotalAmount = 69.99d, Discount = 5.00d, NetAmount = 64.99d, UserID = 101 },
    new BillsModel { BillID = 7, BillNumber = "B1007", BillDate = DateTime.Now.AddDays(-6), OrderID = 7, TotalAmount = 239.97d, Discount = 30.00d, NetAmount = 209.97d, UserID = 101 },
    new BillsModel { BillID = 8, BillNumber = "B1008", BillDate = DateTime.Now.AddDays(-7), OrderID = 8, TotalAmount = 179.98d, Discount = 0, NetAmount = 179.98d, UserID = 101 },
    new BillsModel { BillID = 9, BillNumber = "B1009", BillDate = DateTime.Now.AddDays(-8), OrderID = 9, TotalAmount = 99.99d, Discount = 8.00d, NetAmount = 91.99d, UserID = 101 },
    new BillsModel { BillID = 10, BillNumber = "B1010", BillDate = DateTime.Now.AddDays(-9), OrderID = 10, TotalAmount = 439.96d, Discount = 50.00d, NetAmount = 389.96d, UserID = 101 }
};




        #region BillList
        public IActionResult BillsList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "SP_SELECT_ALL_BILLS ";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
            //return View(bills);
        }
        #endregion

        #region AddEditBills
        public IActionResult AddEditBills()
        {
            PUserDropDown();
            OrderDropDown();
            return View();
        }
        #endregion

        #region BillsSave
        public IActionResult BillsSave(BillsModel billsModel)
        {
            
            
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (billsModel.BillID == null)
                {
                    command.CommandText = "SP_INSERT_BILLS";
                }
                else
                {
                    command.CommandText = "SP_UPDATE_BILLS";
                    command.Parameters.Add("@BillID", SqlDbType.Int).Value = billsModel.BillID;
                }
                command.Parameters.Add("@BillNumber", SqlDbType.Int).Value = billsModel.BillNumber;
                command.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = billsModel.BillDate;
                command.Parameters.Add("@OrderID", SqlDbType.Int).Value = billsModel.OrderID;
                command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = billsModel.TotalAmount;
                command.Parameters.Add("@Discount", SqlDbType.Decimal).Value = billsModel.Discount;
                command.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = billsModel.NetAmount;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = billsModel.UserID;
                command.ExecuteNonQuery();
                TempData["SuccessMessage"] = billsModel.BillID == null ? "Bill added successfully!" : "Bill updated successfully!";
               OrderDropDown();
              PUserDropDown();   
            return RedirectToAction("BillsList");
                      
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

        #region BillsEdit
        public IActionResult BillsEdit(int BillID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "SP_SELECT_BILLS_BY_ID";
            command.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID;
            SqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            connection.Close();

            BillsModel billsModel = new BillsModel();
            billsModel.BillNumber = dataTable.Rows[0]["BillNumber"].ToString();
            billsModel.BillDate = Convert.ToDateTime(dataTable.Rows[0]["BillDate"]);
            billsModel.OrderID = Convert.ToInt32(dataTable.Rows[0]["OrderID"]);
            billsModel.TotalAmount = Convert.ToDouble(dataTable.Rows[0]["TotalAmount"]);
            billsModel.Discount = Convert.ToDouble(dataTable.Rows[0]["Discount"]);
            billsModel.NetAmount = Convert.ToDouble(dataTable.Rows[0]["NetAmount"]);
            billsModel.UserID = Convert.ToInt32(dataTable.Rows[0]["UserID"]);

            PUserDropDown();
            OrderDropDown();
            return View("AddEditBills", billsModel);
        }
        #endregion


        #region Delete
        public IActionResult Delete(int BillID)
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
                        command.CommandText = "SP_DELETE_BILLS";
                        command.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID;
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("BillsList");
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
                return RedirectToAction("BillsList");
            }
            catch (Exception ex)
            {
                TempData["SuccessMessage"] = "Bill deleted successfully!";
                Console.WriteLine(ex.ToString());
                return RedirectToAction("BillsList");
            }
        }
        #endregion
    }
}
