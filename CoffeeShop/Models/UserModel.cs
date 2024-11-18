using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class UserModel
    {
        [Required]
        public int? UserID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Your Email is not valid.")]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Mobile number is required.")]
        [MaxLength(10, ErrorMessage = "Mobile number cannot exceed 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public string MobileNo { get; set; }

        [Required]
        [StringLength(50)]
        public string Address { get; set; }
        [Required(ErrorMessage ="Please Choose Any one")]
        public Boolean IsActive { get; set; }
    }

    public class UserLoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }

}
