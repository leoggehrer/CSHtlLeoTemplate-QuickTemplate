//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Models.Account
{
    using TEntity = Entities.Account.LoginSession;
    using TModel = Models.Account.LoginSession;
    /// <summary>
    /// Represents a login session model.
    /// </summary>
    public partial class LoginSession : VersionModel
    {
        ///<summary>
        /// Gets or sets the identity ID.
        ///</summary>
        ///<value>
        /// An instance of the <see cref="IdType"/> enum representing the identity ID.
        ///</value>
        public IdType IdentityId { get; set; }
        /// <summary>
        /// Gets or sets the session token.
        /// </summary>
        /// <value>The session token.</value>
        public string SessionToken { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the login time of the user.
        /// </summary>
        /// <value>The login time.</value>
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// Gets or sets the logout time of the user.
        /// </summary>
        /// <value>
        /// The logout time, or null if user is currently logged in.
        /// </value>
        /// <remarks>
        /// This property represents the time when the user logged out.
        /// If the user is currently logged in, the LogoutTime will be null.
        /// </remarks>
        public DateTime? LogoutTime { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the email address value.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the optional information.
        /// </summary>
        /// <value>
        /// The optional information.
        /// </value>
        public string? OptionalInfo { get; set; }
        /// <summary>
        /// Gets or sets the array of roles.
        /// </summary>
        /// <value>
        /// The array of roles.
        /// </value>
        public Role[] Roles { get; set; } = Array.Empty<Role>();
        
        /// <summary>
        /// Creates a new instance of LoginSession based on the provided TEntity object.
        /// </summary>
        /// <typeparam name="TEntity">The type of object to create a LoginSession from.</typeparam>
        /// <param name="other">The TEntity object to create a LoginSession from.</param>
        /// <returns>A new instance of LoginSession.</returns>
        internal static LoginSession Create(TEntity other)
        {
            BeforeCreate(other);
            var result = new LoginSession();
            
            result.CopyFrom(other);
            result.Roles = other.Roles.Select(r => Role.Create(r)).ToArray();
            AfterCreate(result, other);
            return result;
        }
        /// <summary>
        /// This method is invoked before creating an entity of type TEntity.
        /// </summary>
        /// <param name="other">The other entity.</param>
        static partial void BeforeCreate(TEntity other);
        /// <summary>
        /// Represents a partial method that is automatically called after creating an instance of TModel.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="instance">The created instance of TModel.</param>
        /// <param name="other">The associated TEntity instance.</param>
        /// <remarks>
        /// This method is automatically called after a new instance of TModel is created.
        /// Use this method to perform any additional logic or operations after the creation of TModel.
        /// </remarks>
        static partial void AfterCreate(TModel instance, TEntity other);
    }
}
#endif
//MdEnd

