//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.AspMvc.Models.Account
{
    /// <summary>
    /// Represents the view model for logging in a user.
    /// </summary>
    public partial class LogonViewModel
    {
        /// <summary>
        /// Gets or sets the return URL.
        /// </summary>
        /// <value>
        /// The return URL.
        /// </value>
        public string? ReturnUrl { get; set; }
        /// <summary>
        /// Gets or sets the Identity URL.
        /// </summary>
        /// <value>
        /// The Identity URL as a nullable string.
        /// </value>
        public string? IdentityUrl { get; set; }
        
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email address provided by the user.</value>
        /// <remarks>
        /// This property is required and should contain a valid email address.
        /// </remarks>
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        ///<summary>
        /// Gets or sets the password.
        ///</summary>
        ///<remarks>
        /// This property is required to have a value.
        /// The data type of this property is password.
        /// This property is displayed with the name "Password".
        ///</remarks>
        ///<value>
        /// The password as a string. The default value is an empty string.
        ///</value>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;
    }
}
#endif
//MdEnd

