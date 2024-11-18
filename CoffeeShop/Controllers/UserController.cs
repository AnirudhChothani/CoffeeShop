using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using CoffeeShop.BAL;

namespace CoffeeShop.Controllers
{
    [CheckAccess]
    public class UserController : Controller
    {
        private IConfiguration configuration;

        public UserController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public static  List<UserModel> users = new List<UserModel>
{
    new UserModel { UserID = 1, UserName = "Alice", Email = "alice@example.com", Password = "password1", MobileNo = "1234567890", Address = "123 Main St", IsActive = true },
    new UserModel { UserID = 2, UserName = "Bob", Email = "bob@example.com", Password = "password2", MobileNo = "1234567891", Address = "124 Main St", IsActive = true },
    new UserModel { UserID = 3, UserName = "Charlie", Email = "charlie@example.com", Password = "password3", MobileNo = "1234567892", Address = "125 Main St", IsActive = true },
    new UserModel { UserID = 4, UserName = "David", Email = "david@example.com", Password = "password4", MobileNo = "1234567893", Address = "126 Main St", IsActive = true },
    new UserModel { UserID = 5, UserName = "Eva", Email = "eva@example.com", Password = "password5", MobileNo = "1234567894", Address = "127 Main St", IsActive = true },
    new UserModel { UserID = 6, UserName = "Frank", Email = "frank@example.com", Password = "password6", MobileNo = "1234567895", Address = "128 Main St", IsActive = true },
    new UserModel { UserID = 7, UserName = "Grace", Email = "grace@example.com", Password = "password7", MobileNo = "1234567896", Address = "129 Main St", IsActive = true },
    new UserModel { UserID = 8, UserName = "Hank", Email = "hank@example.com", Password = "password8", MobileNo = "1234567897", Address = "130 Main St", IsActive = true },
    new UserModel { UserID = 9, UserName = "Ivy", Email = "ivy@example.com", Password = "password9", MobileNo = "1234567898", Address = "131 Main St", IsActive = true },
    new UserModel { UserID = 10, UserName = "Jack", Email = "jack@example.com", Password = "password10", MobileNo = "1234567899", Address = "132 Main St", IsActive = true }
};

        #region UserList
        public IActionResult UserList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "SP_SELECT_ALL_User ";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
            //return View(users);
        }
        #endregion

        #region AddEditUser
        public IActionResult AddEditUser()
        {
            return View();
        }
        #endregion


        #region UserSave
        [HttpPost]
        public IActionResult UserSave(UserModel user)
        {
            
                    string connectionString = this.configuration.GetConnectionString("ConnectionString");
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            if (user.UserID == null)
                            {
                                command.CommandText = "SP_INSERT_USER";
                            }
                            else
                            {
                                command.CommandText = "SP_UPDATE_USER";
                                command.Parameters.Add("@UserID", SqlDbType.Int).Value = user.UserID;
                            }

                            command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = user.UserName;
                            command.Parameters.Add("@Email", SqlDbType.VarChar).Value = user.Email;
                            command.Parameters.Add("@Password", SqlDbType.VarChar).Value = user.Password;
                            command.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = user.MobileNo;
                            command.Parameters.Add("@Address", SqlDbType.VarChar).Value = user.Address;
                            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = user.IsActive;
                            command.ExecuteNonQuery();
                        }
                    }
            TempData["SuccessMessage"] = user.UserID == null ? "User added successfully!" : "User updated successfully!";
            return RedirectToAction("UserList");
           
        }
        #endregion


        #region UserEdit
        public IActionResult UserEdit(int UserID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "SP_SELECT_USER_BY_ID";
            command.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
            SqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            connection.Close();

            UserModel UserModel = new UserModel();
            UserModel.UserID = Convert.ToInt32(dataTable.Rows[0]["UserID"]);
            UserModel.UserName = dataTable.Rows[0]["UserName"].ToString();
            UserModel.Email = dataTable.Rows[0]["Email"].ToString();
            UserModel.Password = dataTable.Rows[0]["Password"].ToString();
            UserModel.MobileNo = dataTable.Rows[0]["MobileNo"].ToString();
            UserModel.Address = dataTable.Rows[0]["Address"].ToString();
            UserModel.IsActive = Convert.ToBoolean(dataTable.Rows[0]["IsActive"]);

            return View("AddEditUser", UserModel);
        }
        #endregion


        #region Delete
        public IActionResult Delete(int UserID)
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
                        command.CommandText = "SP_DELETE_USER";
                        command.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("UserList");
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
                return RedirectToAction("UserList");
            }
            catch (Exception ex)
            {
                TempData["SuccessMessage"] = "User deleted successfully!";
                Console.WriteLine(ex.ToString());
                return RedirectToAction("UserList");
            }
        }

        #endregion
    }
}
