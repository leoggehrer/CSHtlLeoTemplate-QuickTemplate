//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.AspMvc.Models.Account
{
    using Logic.Modules.Common;
    
    /// <summary>
    /// Represents an Identity with properties such as Guid, Name, Email, TimeOutInMinutes, AccessFailedCount, State, and IdentityRoles.
    /// </summary>
    public partial class Identity : VersionModel
    {
        /// <summary>
        /// Gets or sets the list of Identity roles.
        /// </summary>
        /// <value>
        /// An array of IdentityRole objects representing the list of identity roles.
        /// </value>
        public IdentityRole[] IdentityRoleList { get; set; } = Array.Empty<IdentityRole>();
        /// <summary>
        /// Gets an array of IdentityRole objects that are to be added to the list.
        /// </summary>
        /// <value>
        /// An array of IdentityRole objects that are to be added to the list.
        /// </value>
        public IdentityRole[] AddIdentityRoleList
        {
            get
            {
                IdentityRole[] result;
                
                if (IdentityRoleList != null)
                {
                    result = IdentityRoleList.Where(e => IdentityRoles.Any(m => m.Id == e.Id) == false).ToArray();
                }
                else
                {
                    result = Array.Empty<IdentityRole>();
                }
                return result;
            }
        }
        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the time-out value, in minutes, for the property. The default value is 30 minutes.
        /// </summary>
        /// <value>
        /// The time-out value, in minutes.
        /// </value>
        public int TimeOutInMinutes { get; set; } = 30;
        /// <summary>
        /// Gets or sets the number of times the user's access has failed.
        /// </summary>
        /// <value>The number of failed access attempts.</value>
        public int AccessFailedCount { get; set; }
        /// <summary>
        /// Gets or sets the state of the object.
        /// </summary>
        /// <value>
        /// The state of the object.
        /// </value>
        public State State { get; set; } = State.Active;
        
        /// <summary>
        /// Gets or sets the identity roles associated with the user.
        /// </summary>
        /// <value>An array of <see cref="IdentityRole"/> objects.</value>
        public IdentityRole[] IdentityRoles { get; private set; } = Array.Empty<IdentityRole>();
        /// <summary>
        /// Creates a new <see cref="Identity"/> object based on the specified source object.
        /// </summary>
        /// <param name="source">The source object to create the <see cref="Identity"/> object from.</param>
        /// <returns>A new instance of <see cref="Identity"/>.</returns>
        public static Identity Create(object source)
        {
            var result = new Identity();
            
            result.CopyFrom(source);
            if (source is Logic.Models.Account.Identity identity)
            {
                result.IdentityRoles = identity.Roles.Select(e => IdentityRole.Create(e)).ToArray();
            }
            return result;
        }
        /// <summary>
        /// Create a new Identity object based on the given Logic.Models.Account.Identity object.
        /// </summary>
        /// <param name="source">The source Logic.Models.Account.Identity object to be used as the base for the new Identity object.</param>
        /// <returns>A new Identity object.</returns>
        public static Identity Create(Logic.Models.Account.Identity source)
        {
            var result = new Identity();
            
            result.CopyFrom(source);
            result.IdentityRoles = source.Roles.Select(e => IdentityRole.Create(e)).ToArray();
            return result;
        }
    }
}
#endif
//MdEnd

