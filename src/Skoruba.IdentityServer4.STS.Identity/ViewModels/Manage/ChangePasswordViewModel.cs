using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Skoruba.IdentityServer4.STS.Identity.ViewModels.Manage
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "OldPassword", Prompt = "OldPasswordPlaceholder")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", Prompt = "NewPasswordPlaceholder")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        [Display(Name = "ConfirmPassword", Prompt = "ConfirmPasswordPlaceholder")]
        public string ConfirmPassword { get; set; }

        public string StatusMessage { get; set; }

        public string ReturnUrl { get; set; }
    }
}
