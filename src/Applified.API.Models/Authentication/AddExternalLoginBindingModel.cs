using System.ComponentModel.DataAnnotations;

namespace Applified.API.Models.Authentication
{
    public class AddExternalLoginBindingModel {
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }
}