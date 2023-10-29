//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.AspMvc.Models.Account
{
    /// <summary>
    /// Represents a view model for changing a user's password.
    /// </summary>
    public partial class ChangePasswordViewModel
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        /// <value>The user name.</value>
        [ScaffoldColumn(false)]
        public string UserName { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>The email address.</value>
        /// <remarks>
        /// This property is decorated with the DataType attribute with the value of EmailAddress.
        /// </remarks>
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the current password of the user.
        /// </summary>
        /// <remarks>
        /// This property is required and is used to validate the current password entered by the user.
        /// </remarks>
        /// <value>
        /// The current password of the user.
        /// </value>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        /// <value>
        /// The new password.
        /// </value>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the confirmed password entered by the user.
        /// </summary>
        /// <remarks>
        /// This property is used when the user is required to confirm a new password.
        /// It should be compared with the "NewPassword" property to ensure both passwords match.
        /// </remarks>
        /// <value>
        /// A <see cref="System.String"/> representing the confirmed password.
        /// </value>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
#endif
//MdEnd

