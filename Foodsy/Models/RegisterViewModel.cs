using System.ComponentModel.DataAnnotations;

namespace Foodsy.Models
{
    public class RegisterViewModel
    {
        
        public required string FullName { get; set; }

        
        [EmailAddress]
        public required string Email { get; set; }

        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }
    }
}
