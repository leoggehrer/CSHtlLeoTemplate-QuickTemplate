//@BaseCode
//MdStart
#if ACCOUNT_ON
using System.ComponentModel;

namespace QuickTemplate.AspMvc.Models.Account
{
    public partial class IdentityCreate : ModelObject
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TimeOutInMinutes { get; set; } = 30;
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        [Compare(nameof(Password), ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
#endif
//MdEnd
