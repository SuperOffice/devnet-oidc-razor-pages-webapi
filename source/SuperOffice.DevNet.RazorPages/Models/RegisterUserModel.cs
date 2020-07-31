using System.ComponentModel.DataAnnotations;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Models
{
    public class RegisterUserModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
        [Compare("Password", ErrorMessage = "The password and repeat password do not match.")]
        public string RepeatPassword { get; set; }
    }
}
