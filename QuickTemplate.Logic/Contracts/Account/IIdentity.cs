//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Contracts.Account
{
    using QuickTemplate.Logic.Modules.Common;
    
    /// <summary>
    /// Represents an identity with basic properties.
    /// </summary>
    public partial interface IIdentity
    {
        /// <summary>
        /// Gets the Id value of the object.
        /// </summary>
        /// <remarks>
        /// The Id is a unique identifier for the object.
        /// </remarks>
        /// <value>
        /// The Id value of the object.
        /// </value>
        IdType Id { get; }
        /// <summary>
        /// Gets the globally unique identifier (GUID) associated with this instance.
        /// </summary>
        Guid Guid { get; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        string Email { get; set; }
        /// <summary>
        /// Gets or sets the time-out period in minutes.
        /// </summary>
        /// <remarks>
        /// This property represents the time in minutes after which a timeout occurs.
        /// </remarks>
        int TimeOutInMinutes { get; set; }
        /// <summary>
        /// Gets or sets the number of access failed attempts for the user.
        /// </summary>
        int AccessFailedCount { get; set; }
        /// <summary>
        /// Gets or sets the state property.
        /// </summary>
        State State { get; set; }
        /// <summary>
        /// Gets the array of roles associated with the user.
        /// </summary>
        IRole[] Roles { get; }
        
        /// <summary>
        /// Checks if the user has a specific role identified by the provided GUID.
        /// </summary>
        /// <param name="guid">The GUID of the role to check for.</param>
        /// <returns>true if the user has the specified role, false otherwise.</returns>
        bool HasRole(Guid guid);
    }
}
#endif
//MdEnd


