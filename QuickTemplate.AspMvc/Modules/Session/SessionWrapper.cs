//@BaseCode
//MdStart
using QuickTemplate.AspMvc.Extensions;

namespace QuickTemplate.AspMvc.Modules.Session
{
    public partial class SessionWrapper : ISessionWrapper
    {
        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value>The session.</value>
        private ISession Session { get; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionWrapper"/> class.
        /// </summary>
        /// <param name="session">The session object to be wrapped.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided session object is null.</exception>
        public SessionWrapper(ISession session)
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
        }
        
        #region General
        /// <summary>
        /// Checks if the Session contains a value associated with a specified key.
        /// </summary>
        /// <param name="key">The key to check for in the Session.</param>
        /// <returns>true if the Session contains a value associated with the specified key; otherwise, false.</returns>
        public bool HasValue(string key)
        {
            return Session.TryGetValue(key, out _);
        }
        /// <summary>
        /// Removes the specified key from the session.
        /// </summary>
        /// <param name="key">The key of the item to remove from the session.</param>
        /// <remarks>
        /// This method removes an item from the session using the specified key. If the
        /// key is not found in the session, no action is taken.
        /// </remarks>
        public void Remove(string key)
        {
            Session.Remove(key);
        }
        #endregion General
        
        #region Type-Access
        /// <summary>
        /// Sets the value of a specified key in the session.
        /// </summary>
        /// <typeparam name="T">The type of the value to be set.</typeparam>
        /// <param name="key">The key of the value to be set.</param>
        /// <param name="value">The value to be set.</param>
        public void Set<T>(string key, T value)
        {
            Session.Set<T>(key, value);
        }
        /// <summary>
        /// Retrieves the value associated with the specified key from the session.
        /// </summary>
        /// <typeparam name="T">The type of the value to retrieve.</typeparam>
        /// <param name="key">The key of the value to retrieve.</param>
        /// <returns>The value associated with the specified key, or null if the key does not exist in the session.</returns>
        public T? Get<T>(string key)
        {
            return Session.Get<T>(key);
        }
        #endregion Type-Access
        
        #region Object-Access
        /// <summary>
        /// Sets the specified value in the Session object using the provided key.
        /// </summary>
        /// <param name="key">The key associated with the value.</param>
        /// <param name="value">The value to be stored in the Session object.</param>
        public void SetValue(string key, object value)
        {
            Session.Set(key, value);
        }
        /// <summary>
        /// Retrieves the value from the session using the specified key.
        /// </summary>
        /// <param name="key">The key used to retrieve the value from the session.</param>
        /// <returns>
        /// The value associated with the specified key, or null if the key is not found.
        /// </returns>
        public object? GetValue(string key)
        {
            return Session.Get<object>(key);
        }
        #endregion Object-Access
        
        #region Int-Access
        /// <summary>
        /// Sets the integer value in the session using the specified key.
        /// </summary>
        /// <param name="key">The key used to identify the integer value in the session.</param>
        /// <param name="value">The integer value to be set in the session.</param>
        public void SetIntValue(string key, int value)
        {
            Session.SetInt32(key, value);
        }
        /// <summary>
        /// Retrieves an integer value from the session using the specified key.
        /// </summary>
        /// <param name="key">The key used to identify the integer value in the session.</param>
        /// <returns>
        /// The integer value associated with the specified key if found; otherwise, null.
        /// </returns>
        public int? GetIntValue(string key)
        {
            return Session.GetInt32(key);
        }
        #endregion Int-Access
        
        #region String-Access
        /// <summary>
        /// Sets the value of a specified key in the session state.
        /// </summary>
        /// <param name="key">The key of the value to set.</param>
        /// <param name="value">The value to set for the specified key.</param>
        public void SetStringValue(string key, string value)
        {
            Session.SetString(key, value);
        }
        /// <summary>
        /// Retrieves a string value associated with the specified key from the session.
        /// </summary>
        /// <param name="key">The key of the string value to retrieve.</param>
        /// <returns>Returns the string value associated with the specified key, or null if the key is not found in the session.</returns>
        public string? GetStringValue(string key)
        {
            return Session.GetString(key);
        }
        /// <summary>
        /// Gets the string value associated with the specified key from the session.
        /// If the value is not found or is empty, returns the default value instead.
        /// </summary>
        /// <param name="key">The key used to retrieve the string value from the session.</param>
        /// <param name="defaultValue">The default value to be returned if the specified key is not found or is empty.</param>
        /// <returns>
        /// The string value associated with the specified key if found in the session;
        /// otherwise, the default value provided.
        /// </returns>
        public string GetStringValue(string key, string defaultValue)
        {
            var result = Session.GetString(key);
            
            return string.IsNullOrEmpty(result) ? defaultValue : result;
        }
        #endregion String-Access
        
        #region Properties
        /// <summary>
        /// Gets or sets the Return Url.
        /// </summary>
        /// <value>
        /// The Return Url.
        /// </value>
        public string? ReturnUrl
        {
            get
            {
                return GetStringValue(nameof(ReturnUrl));
            }
            set
            {
                SetStringValue(nameof(ReturnUrl), value ?? string.Empty);
            }
        }
        /// <summary>
        /// Gets or sets the return controller for the property.
        /// </summary>
        /// <value>
        /// The return controller as a string.
        /// </value>
        public string? ReturnController
        {
            get
            {
                return GetStringValue(nameof(ReturnController));
            }
            set
            {
                SetStringValue(nameof(ReturnController), value ?? string.Empty);
            }
        }
        /// <summary>
        /// Gets or sets the return action.
        /// </summary>
        /// <value>
        /// The return action.
        /// </value>
        public string? ReturnAction
        {
            get
            {
                return GetStringValue(nameof(ReturnAction));
            }
            set
            {
                SetStringValue(nameof(ReturnAction), value ?? string.Empty);
            }
        }
        /// <summary>
        /// Gets or sets the hint for the property.
        /// </summary>
        /// <value>
        /// The hint for the property.
        /// </value>
        public string? Hint
        {
            get
            {
                return GetStringValue(nameof(Hint));
            }
            set
            {
                SetStringValue(nameof(Hint), value ?? string.Empty);
            }
        }
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string? Error
        {
            get
            {
                return GetStringValue(nameof(Error));
            }
            set
            {
                SetStringValue(nameof(Error), value ?? string.Empty);
            }
        }
        #endregion Properties
        
#if ACCOUNT_ON
        #region Authentication
        /// <summary>
        /// Gets or sets the login session of the account.
        /// </summary>
        public Models.Account.LoginSession? LoginSession
        {
            get => Session.Get<Models.Account.LoginSession>(nameof(LoginSession));
            set => Session.Set(nameof(LoginSession), value);
        }
        /// <summary>
        /// Gets the session token.
        /// </summary>
        /// <returns>The session token if there is an active session; otherwise, an empty string.</returns>
        public string SessionToken
        {
            get
            {
                var loginSession = LoginSession;
                
                return loginSession != null ? loginSession.SessionToken : string.Empty;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the user is authenticated.
        /// </summary>
        /// <value>true if the user is authenticated; otherwise, false.</value>
        public bool IsAuthenticated
        {
            get
            {
                return LoginSession != null;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the session is alive.
        /// </summary>
        /// <remarks>
        /// This property returns true if the session is alive and active, and false otherwise.
        /// </remarks>
        /// <returns>
        /// True if the session is alive; otherwise, false.
        /// </returns>
        public bool IsSessionAlive
        {
            get
            {
                var result = false;
                var loginSession = LoginSession;
                
                if (IsAuthenticated)
                {
                    result = Task.Run(async () => await Logic.AccountAccess.IsSessionAliveAsync(SessionToken).ConfigureAwait(false)).Result;
                }
                return result;
            }
        }
        /// <summary>
        /// Checks if the current user has a specific role or multiple roles.
        /// </summary>
        /// <param name="role">The role to check.</param>
        /// <param name="further">An optional array of additional roles to check.</param>
        /// <returns>
        ///   <c>true</c> if the user has the specified role or any of the additional roles;
        ///   otherwise, <c>false</c>.
        /// </returns>
        public bool HasRole(string role, params string[] further)
        {
            var result = false;
            var loginSession = LoginSession;
            
            if (loginSession != null)
            {
                result = Task.Run(async () => await Logic.AccountAccess.HasRoleAsync(loginSession.SessionToken, role).ConfigureAwait(false)).Result;
                for (int i = 0; result == false && i < further.Length; i++)
                {
                    result = Task.Run(async () => await Logic.AccountAccess.HasRoleAsync(loginSession.SessionToken, further[i]).ConfigureAwait(false)).Result;
                }
            }
            return result;
        }
        #endregion Authentication
#endif
    }
}
//MdEnd


