using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.EMMA;
using CoffeeShop.BAL;


namespace CoffeeShop.Controllers
{
    [CheckAccess]
    public class ProductController : Controller
    {
        private readonly IConfiguration configuration;

        public ProductController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        #region ProductList
        public IActionResult ProductList()
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");
            DataTable table = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_SELECT_ALL_PRODUCT";
                    SqlDataReader reader = command.ExecuteReader();
                    table.Load(reader);
                }
            }

            return View(table);
        }
        #endregion


        #region AddEditProduct
        public IActionResult AddEditProduct()
        {
           
            PUserDropDown();
            return View();
           
        }
        #endregion


        #region ProductSave
        public IActionResult ProductSave(ProductModel productModel)
        {

                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (productModel.ProductID == null)
                {
                    command.CommandText = "SP_INSERT_PRODUCT";
                }
                else
                {
                    command.CommandText = "SP_UPDATE_PRODUCT";
                    command.Parameters.Add("@ProductID", SqlDbType.Int).Value = productModel.ProductID;
                }
                command.Parameters.Add("@ProductName", SqlDbType.VarChar).Value = productModel.ProductName;
                command.Parameters.Add("@ProductCode", SqlDbType.VarChar).Value = productModel.ProductCode;
                command.Parameters.Add("@ProductPrice", SqlDbType.Decimal).Value = productModel.ProductPrice;
                command.Parameters.Add("@Description", SqlDbType.VarChar).Value = productModel.Description;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = productModel.UserID;
                command.ExecuteNonQuery();
             TempData["SuccessMessage"] = productModel.ProductID == null ? "Product added successfully!" : "Product updated successfully!";
             PUserDropDown();
                return RedirectToAction("ProductList");  
        }

        #endregion

        #region UserDropdown

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


        #region ProductEdit
        public IActionResult ProductEdit(int ProductID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "SP_SELECT_PRODUCT_BY_ID";
            command.Parameters.Add("@ProductID", SqlDbType.Int).Value = ProductID;
            SqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            connection.Close();

            ProductModel productModel = new ProductModel();
            productModel.ProductID = Convert.ToInt32(dataTable.Rows[0]["ProductID"]);
            productModel.ProductName = dataTable.Rows[0]["ProductName"].ToString();
            productModel.ProductPrice = Convert.ToDouble(dataTable.Rows[0]["ProductPrice"]);
            productModel.ProductCode = dataTable.Rows[0]["ProductCode"].ToString();
            productModel.Description = dataTable.Rows[0]["Description"].ToString();
            productModel.UserID = Convert.ToInt32(dataTable.Rows[0]["UserID"]);

            PUserDropDown();

            return View("AddEditProduct", productModel);
        }
        #endregion


        #region Delete
        public IActionResult Delete(int ProductID)
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
                        command.CommandText = "SP_DELETE_PRODUCT";
                        command.Parameters.Add("@ProductID", SqlDbType.Int).Value = ProductID;
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("ProductList");
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
                return RedirectToAction("ProductList");
            }
            catch (Exception ex)
            {
                TempData["SuccessMessage"] = "Product deleted successfully!";
                Console.WriteLine(ex.ToString());
                return RedirectToAction("ProductList");
            }
        }
        #endregion


        #region ExportToExcel
        public ActionResult ExportToExcel()
        {
            DataTable dt = GetProductData();

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Products");

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProductList.xlsx");
                }
            }
        }
        #endregion

        #region GetProductData
        private DataTable GetProductData()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_SELECT_ALL_PRODUCT";
                SqlDataReader reader = command.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);
                return dt;
            }
        }
        #endregion

        #region ExportToPDF
        public ActionResult ExportToPDF()
        {
            // Create a new PDF document
            Document document = new Document();
            MemoryStream stream = new MemoryStream();
            PdfWriter.GetInstance(document, stream).CloseStream = false;
            document.Open();

            // Add title to the PDF
            document.Add(new Paragraph("Product List"));
            document.Add(new Paragraph(" ")); // Add some space

            // Create a table with the same number of columns as your HTML table
            PdfPTable table = new PdfPTable(6);

            // Add the headers
            table.AddCell("ProductID");
            table.AddCell("ProductName");
            table.AddCell("ProductPrice");
            table.AddCell("ProductCode");
            table.AddCell("Description");
            table.AddCell("UserName");

            // Fetch data from the database or the Model
            DataTable products = GetProductData(); // Replace this with your method to get the data

            // Add the data rows to the table
            foreach (DataRow row in products.Rows)
            {
                table.AddCell(row["ProductID"].ToString());
                table.AddCell(row["ProductName"].ToString());
                table.AddCell(row["ProductPrice"].ToString());
                table.AddCell(row["ProductCode"].ToString());
                table.AddCell(row["Description"].ToString());
                table.AddCell(row["UserID"].ToString());
            }

            // Add the table to the document
            document.Add(table);

            // Close the document
            document.Close();

            // Return the PDF as a file download
            byte[] byteArray = stream.ToArray();
            stream.Write(byteArray, 0, byteArray.Length);
            stream.Position = 0;
            return File(stream, "application/pdf", "ProductList.pdf");
        }
        #endregion

        // Dummy method to simulate data retrieval, replace it with your actual method

        #region GetProductDataPdf
        private DataTable GetProductDataPdf()
        {
            // Simulate data retrieval from your database
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductID");
            dt.Columns.Add("ProductName");
            dt.Columns.Add("ProductPrice");
            dt.Columns.Add("ProductCode");
            dt.Columns.Add("Description");
            dt.Columns.Add("UserID");



            return dt;
        }

        #endregion
    }

}
