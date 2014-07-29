using System.ComponentModel.DataAnnotations;

namespace Applified.API.Models.Authentication
{
    public class ForgotPasswordViewModel {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}