//@BaseCode
//MdStart
#if ACCOUNT_ON
using System.ComponentModel;

namespace QuickTemplate.AspMvc.Models.Account
{
    /// <summary>
    /// Represents an object used for creating a new identity.
    /// </summary>
    public class IdentityCreate : ModelObject
    {
        /// <summary>
        /// Gets or sets the name of the object.
        /// </summary>
        /// <value>
        /// A string representing the name of the object.
        /// </value>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the email for the user.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the time-out value in minutes.
        /// </summary>
        /// <value>
        /// The time-out value in minutes. The default value is 30.
        /// </value>
        public int TimeOutInMinutes { get; set; } = 30;
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the confirmed password.
        /// </summary>
        /// <value>
        /// The confirmed password.
        /// </value>
        /// <remarks>
        /// Use this property to store the confirmed password entered by the user.
        /// </remarks>
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        [Compare(nameof(Password), ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
#endif
//MdEnd

