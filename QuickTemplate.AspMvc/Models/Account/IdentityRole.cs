//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.AspMvc.Models.Account
{
    public partial class IdentityRole : VersionModel
    {
#if !GUID_ON
        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        public Guid Guid { get; set; }
#endif
        /// <summary>
        /// Gets or sets the designation.
        /// </summary>
        /// <value>
        /// The designation.
        /// </value>
        public string Designation { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string? Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Creates a new instance of the IdentityRole class and initializes it by copying properties from the provided source object.
        /// </summary>
        /// <param name="source">The object from which the properties are copied.</param>
        /// <returns>A new instance of IdentityRole with properties copied from the source object.</returns>
        public static IdentityRole Create(object source)
        {
            var result = new IdentityRole();
            
            result.CopyFrom(source);
            return result;
        }
    }
}
#endif
//MdEnd
