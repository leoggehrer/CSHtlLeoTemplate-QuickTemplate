﻿//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Entities.Account
{
    /// <summary>
    /// Represents a login session in the account system.
    /// </summary>
    
#if SQLITE_ON
    [System.ComponentModel.DataAnnotations.Schema.Table("LoginSessions")]
#else
    [System.ComponentModel.DataAnnotations.Schema.Table("LoginSessions", Schema = "account")]
#endif
    internal partial class LoginSession : VersionEntity
    {
        private DateTime? _logoutTime;
        private SecureIdentity? identity;
        
        /// <summary>
        /// Gets or sets the identity ID.
        /// </summary>
        public IdType IdentityId { get; internal set; }
        /// <summary>
        /// Gets or sets the timeout value in minutes.
        /// </summary>
        public int TimeOutInMinutes { get; internal set; }
        /// <summary>
        /// Gets or sets the session token.
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string SessionToken { get; internal set; } = string.Empty;
        /// <summary>
        /// Gets or sets the time when the user logs in, in Coordinated Universal Time (UTC).
        /// </summary>
        public DateTime LoginTime { get; internal set; } = DateTime.UtcNow;
        /// <summary>
        /// Gets or sets the last access time for the property.
        /// </summary>
        public DateTime LastAccess { get; internal set; } = DateTime.UtcNow;
        /// <summary>
        /// Gets or sets the logout time.
        /// </summary>
        /// <returns>
        /// A nullable DateTime representing the logout time.
        /// </returns>
        /// <remarks>
        /// If the logout time is being read, the OnLogoutTimeReading event will be triggered.
        /// If the logout time is being changed, the OnLogoutTimeChanging event will be triggered.
        /// If the OnLogoutTimeChanging event is not handled by any subscribers, the value will be set directly.
        /// After the logout time is changed, the OnLogoutTimeChanged event will be triggered.
        /// </remarks>
        public DateTime? LogoutTime
        {
            get
            {
                OnLogoutTimeReading();
                return _logoutTime;
            }
            internal set
            {
                bool handled = false;
                OnLogoutTimeChanging(ref handled, value, ref _logoutTime);
                if (handled == false)
                {
                    _logoutTime = value;
                }
                OnLogoutTimeChanged();
            }
        }
        /// <summary>
        /// This method is called when the logout time reading occurs.
        /// </summary>
        /// <remarks>
        /// Implement this method to perform any additional actions when the logout time is being read.
        /// This method is declared as "partial" so it can be implemented in different parts of the partial class.
        /// </remarks>
        partial void OnLogoutTimeReading();
        /// <summary>
        /// Event raised when the logout time is changing.
        /// </summary>
        /// <param name="handled">Reference to a boolean value indicating whether the event has been handled.</param>
        /// <param name="value">The new value of the logout time.</param>
        /// <param name="_logoutTime">Reference to the current value of the logout time.</param>
        /// <remarks>
        /// This event can be used to perform additional logic before the logout time is updated.
        /// By setting the <paramref name="handled"/> parameter to <c>true</c>, the event is considered handled and
        /// the default logic to update the logout time will be overridden.
        /// </remarks>
        partial void OnLogoutTimeChanging(ref bool handled, DateTime? value, ref DateTime? _logoutTime);
        /// <summary>
        /// This method is called when the logout time is changed.
        /// </summary>
        partial void OnLogoutTimeChanged();
        /// <summary>
        /// Gets or sets the optional information.
        /// </summary>
        /// <value>
        /// The optional information.
        /// </value>
        /// <remarks>
        /// The maximum length of the optional information is 4096 characters.
        /// </remarks>
        [MaxLength(4096)]
        public string? OptionalInfo { get; internal set; }
        
        #region Transient properties
        /// <summary>
        /// Gets or sets the password hash.
        /// </summary>
        /// <value>
        /// The password hash as a byte array.
        /// </value>
        [NotMapped]
        internal byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        /// <summary>
        /// Gets or sets the salt value used for password hashing.
        /// </summary>
        /// <remarks>
        /// This property is not mapped to the database.
        /// </remarks>
        /// <value>
        /// The salt value used for password hashing.
        /// </value>
        [NotMapped]
        internal byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        /// <summary>
        /// Gets or sets a value indicating whether the authentication is remote.
        /// </summary>
        /// <remarks>
        /// Use this property to determine if the authentication method is remote or not.
        /// </remarks>
        /// <value>
        ///   <c>true</c> if the authentication is remote; otherwise, <c>false</c>.
        /// </value>
        [NotMapped]
        public bool IsRemoteAuth { get; internal set; }
        /// <summary>
        /// Gets or sets the origin of the property.
        /// </summary>
        /// <remarks>
        /// This property is not mapped to the database.
        /// </remarks>
        /// <value>
        /// The origin of the property.
        /// </value>
        [NotMapped]
        public string Origin { get; internal set; } = string.Empty;
        /// <summary>
        /// Gets or sets the name of the object.
        /// </summary>
        /// <remarks>
        /// This property is not mapped to a database column.
        /// </remarks>
        /// <value>
        /// The name of the object.
        /// </value>
        [NotMapped]
        public string Name { get; internal set; } = string.Empty;
        /// <summary>
        /// Gets or sets the email address associated with the object.
        /// </summary>
        /// <remarks>
        /// This property is not mapped to a database column.
        /// </remarks>
        /// <value>
        /// A <see cref="System.String"/> representing the email address.
        /// </value>
        /// <seealso cref="NotMappedAttribute"/>
        [NotMapped]
        public string Email { get; internal set; } = string.Empty;
        /// <summary>
        /// Gets or sets the JSON Web Token.
        /// </summary>
        /// <remarks>
        /// This property is not mapped to a database column.
        /// </remarks>
        /// <value>The JSON Web Token.</value>
        [NotMapped]
        public string JsonWebToken { get; internal set; } = string.Empty;
        
        /// <summary>
        /// Gets a value indicating whether the object is active or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the object is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive => IsTimeout == false;
        /// <summary>
        /// Gets a value indicating whether the session timeout has occurred.
        /// </summary>
        /// <remarks>
        /// The timeout is determined by comparing the last access time of the session to the current time.
        /// If the "LogoutTime" property has a value or if the elapsed time in seconds exceeds the specified timeout period,
        /// this property will return true. Otherwise, it will return false.
        /// </remarks>
        [NotMapped]
        public bool IsTimeout
        {
            get
            {
                TimeSpan ts = DateTime.UtcNow - LastAccess;
                
                return LogoutTime.HasValue || ts.TotalSeconds > TimeOutInMinutes * 60;
            }
        }
        /// <summary>
        /// Gets the list of roles for this object.
        /// </summary>
        [NotMapped]
        public List<Role> Roles { get; } = new();
        #endregion Transient properties
        
        #region Navigation properties
        /// <summary>
        /// Gets or sets the secure identity.
        /// </summary>
        public SecureIdentity? Identity
        {
            get => identity;
            set
            {
                identity = value;
                TimeOutInMinutes = identity != null ? identity.TimeOutInMinutes : 0;
                PasswordHash = identity != null ? identity.PasswordHash : Array.Empty<byte>();
                PasswordSalt = identity != null ? identity.PasswordSalt : Array.Empty<byte>();
            }
        }
        #endregion Navigation properties
        
        /// <summary>
        /// Clones the LoginSession object, including its roles, and returns the cloned object.
        /// </summary>
        /// <returns>A cloned LoginSession object.</returns>
        public LoginSession Clone()
        {
            var result = new LoginSession();
            
            result.CopyFrom(this);
            foreach (var role in Roles)
            {
                var cRole = new Role();
                
                cRole.CopyFrom(role);
                result.Roles.Add(cRole);
            }
            return result;
        }
    }
}
#endif
//MdEnd


