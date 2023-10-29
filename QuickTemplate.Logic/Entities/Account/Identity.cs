//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Entities.Account
{
    using QuickTemplate.Logic.Contracts.Account;
    using QuickTemplate.Logic.Modules.Common;
    
    /// <summary>
    /// Represents an identity in the account system.
    /// </summary>
#if SQLITE_ON
    [System.ComponentModel.DataAnnotations.Schema.Table("Identities")]
#else
    [System.ComponentModel.DataAnnotations.Schema.Table("Identities", Schema = "account")]
#endif
    [Microsoft.EntityFrameworkCore.Index(nameof(Email), IsUnique = true)]
    public abstract partial class Identity : VersionExtendedEntity, IIdentity
    {
#if GUID_OFF
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public Guid Guid { get; internal set; }
#endif
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [MaxLength(128)]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [MaxLength(128)]
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the time-out value in minutes.
        /// </summary>
        public int TimeOutInMinutes { get; set; } = 30;
        /// <summary>
        /// Gets or sets a value indicating whether JWT authentication is enabled.
        /// </summary>
        public bool EnableJwtAuth { get; set; }
        /// <summary>
        /// Gets or sets the number of failed access attempts for the user.
        /// </summary>
        public int AccessFailedCount { get; set; }
        ///<summary>
        ///Gets or sets the State of the object.
        ///</summary>
        public State State { get; set; } = State.Active;
        /// <summary>
        /// Gets an array of roles associated with the user.
        /// </summary>
        public IRole[] Roles => IdentityXRoles.Where(iXr => iXr.Role != null)
                                              .Select(iXr => iXr.Role!)
                                              .ToArray();
        // Navigation properties
        /// <summary>
        /// Gets or sets the list of IdentityXRole objects associated with this entity.
        /// </summary>
        public List<IdentityXRole> IdentityXRoles { get; internal set; } = new();
        
        /// <summary>
        /// Checks if the user has a role with the specified GUID.
        /// </summary>
        /// <param name="guid">The GUID of the role to check.</param>
        /// <returns>True if the user has the role, otherwise false.</returns>
        public bool HasRole(Guid guid)
        {
            return IdentityXRoles.Any(iXr => iXr.Role != null && iXr.Role.Guid == guid);
        }
    }
}
#endif
//MdEnd

