namespace Foodsy.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LoginViewModel
    {
        public LoginViewModel()
        {
            Email = string.Empty; 
            Password = string.Empty; 
            RememberMe = false; 
        }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }


}
