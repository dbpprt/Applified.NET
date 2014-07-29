using System.ComponentModel.DataAnnotations;

namespace Applified.API.Models.Authentication
{
    public class RegisterExternalBindingModel {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string UserName { get; set; }
    }
}