using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class OrderModel
    {
        [Required]
        
        public int? OrderID { get; set; }
        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "Please enter a valid date.")]
        [PastOrCurrentDate(ErrorMessage = "The date cannot be in the future.")]
        public DateTime OrderDate  { get; set; }
        [Required]
        [StringLength(50)]
        public string CustomerName { get; set; }
        [Required]
        public string PaymentMode { get; set; }

        [Required(ErrorMessage = "Total Amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Total Amount must be a positive value.")]
        public double TotalAmount { get; set; }
        [Required]
        [StringLength (50)]
        public string ShippingAddress { get; set; }
        [Required]
        public int UserID { get; set; } = 101;
    }
    
}
