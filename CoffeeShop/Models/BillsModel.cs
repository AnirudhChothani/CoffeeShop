using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class BillsModel
    {
        public int? BillID { get; set; }
        [Required]
        public string BillNumber { get; set; }

        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "Please enter a valid date.")]
        [PastOrCurrentDate(ErrorMessage = "The date cannot be in the future.")]
        public DateTime BillDate { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Total Amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Total Amount must be a positive value.")]
        public double TotalAmount { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Please enter a discount percentage between 0 and 100.")]
        public double Discount { get; set; }
        [Required]

        [Range(0, double.MaxValue, ErrorMessage = "Net Amount must be a positive value.")]
        public double NetAmount { get; set; }
            [Required]
        public int UserID { get; set; } = 101;
    }
    public class PastOrCurrentDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateValue)
            {
                if (dateValue > DateTime.Now)
                {
                    return new ValidationResult(ErrorMessage ?? "The date cannot be in the future.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
