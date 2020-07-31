using System.ComponentModel.DataAnnotations;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Models
{
    public class LoginUserModel
    {
        [Required(ErrorMessage = "Have to supply an e-mail address")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Have to supply a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
