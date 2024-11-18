using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class OrderDetailsModel
    {
        public int? OrderDetailID { get; set; }
        [Required]
        public int OrderID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative number.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Total Amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Total Amount must be a positive value.")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "Total Amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Total Amount must be a positive value.")]
        public double TotalAmount { get; set; }

        [Required]
        public int UserID { get; set; } = 101;
    }
    public class OrderDropDownModel
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
    }
    public class ProductDropDownModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    }
}
