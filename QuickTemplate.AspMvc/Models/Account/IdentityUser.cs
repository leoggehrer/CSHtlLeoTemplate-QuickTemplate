//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.AspMvc.Models.Account
{
    /// <summary>
    /// Represents a user with identity information and properties.
    /// </summary>
    public partial class IdentityUser : VersionModel
    {
        /// <summary>
        /// Gets or sets an array of Identity objects.
        /// </summary>
        /// <value>
        /// An array of Identity objects.
        /// </value>
        public Identity[]? IdentityList { get; set; }
        
        ///<summary>
        /// Gets or sets the identity email.
        ///</summary>
        /// <value>
        /// The identity email.
        ///</value>
        public string IdentityEmail { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the identity id.
        /// </summary>
        public IdType IdentityId { get; set; }
        /// <summary>
        /// Gets and sets the user first name.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;
        /// <summary>
        /// Gets and sets the user last name.
        /// </summary>
        public string LastName { get; set; } = string.Empty;
        
        /// <summary>
        /// Creates an instance of Identity and copies the properties of the same name from the object parameter.
        /// </summary>
        /// <param name="source">The object to copy.</param>
        /// <returns></returns>
        public static IdentityUser Create(object source)
        {
            var result = new IdentityUser();
            
            result.CopyFrom(source);
            return result;
        }
    }
}
#endif
//MdEnd
