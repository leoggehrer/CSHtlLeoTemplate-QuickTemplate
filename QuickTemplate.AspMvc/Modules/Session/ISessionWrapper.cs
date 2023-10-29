//@BaseCode
//MdStart
namespace QuickTemplate.AspMvc.Modules.Session
{
    public partial interface ISessionWrapper
    {
        #region General
        /// <summary>
        /// Checks if the specified key has a value.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key has a value, false otherwise.</returns>
        bool HasValue(string key);
        /// <summary>
        /// Removes an item with the specified key from the collection.
        /// </summary>
        /// <param name="key">The key of the item to be removed.</param>
        void Remove(string key);
        #endregion General
        
        #region Type-Access
        /// <summary>
        /// Sets a value associated with the specified key in the data store.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key to associate with the value.</param>
        /// <param name="value">The value to be stored.</param>
        /// <remarks>
        /// If the key already exists in the data store, the existing value will be overwritten.
        /// </remarks>
        void Set<T>(string key, T value);
        /// <summary>
        /// Gets the value associated with the specified <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to retrieve.</typeparam>
        /// <param name="key">The key of the value to retrieve.</param>
        /// <returns>The value associated with the specified <paramref name="key"/>, or null if the key is not found.</returns>
        /// <remarks>
        /// This method retrieves the value associated with the specified <paramref name="key"/> from the data source. The value is then cast to type T.
        /// </remarks>
        T? Get<T>(string key);
        #endregion Type-Access
        
        #region Object-Access
        /// <summary>
        /// Sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to set.</param>
        /// <param name="value">The value to be set.</param>
        /// <remarks>
        /// This method allows you to associate a specific value with a key. The key should be a unique identifier.
        /// If the key already exists, the existing value will be replaced with the new value.
        /// </remarks>
        void SetValue(string key, object value);
        /// <summary>
        /// Retrieves the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key to retrieve the value for.</param>
        /// <returns>The value associated with the specified key, if found; otherwise, null.</returns>
        object? GetValue(string key);
        #endregion Object-Access
        
        #region Int-Access
        /// <summary>
        /// Sets the integer value associated with the given key.
        /// </summary>
        /// <param name="key">The key for the integer value.</param>
        /// <param name="value">The integer value to set.</param>
        /// <remarks>
        /// This method updates the integer value associated with the given key.
        /// If the key already exists, its value will be replaced with the new value.
        /// If the key does not exist, a new key-value pair will be added to the collection.
        /// </remarks>
        void SetIntValue(string key, int value);
        /// <summary>
        /// Retrieves an integer value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to retrieve.</param>
        /// <returns>
        /// The integer value associated with the specified key, if the key is found;
        /// otherwise, null.
        /// </returns>
        int? GetIntValue(string key);
        #endregion Int-Access
        
        #region String-Access
        /// <summary>
        /// Sets the value of a specific key in a string format.
        /// </summary>
        /// <param name="key">The key to set the value for.</param>
        /// <param name="value">The string value to be set.</param>
        void SetStringValue(string key, string value);
        /// <summary>
        /// Retrieves the value associated with the specified key as a string.
        /// </summary>
        /// <param name="key">The key for the value to retrieve.</param>
        /// <returns>
        /// The value associated with the specified key as a string, if the key is found;
        /// otherwise, <see langword="null"/>.
        /// </returns>
        string? GetStringValue(string key);
        /// <summary>
        /// Retrieves the value associated with the specified key from the configuration.
        /// If the key is not found, returns the specified default value.
        /// </summary>
        /// <param name="key">The key to search for.</param>
        /// <param name="defaultValue">The default value to return if the key is not found.</param>
        /// <returns>
        /// The value associated with the specified key if found;
        /// otherwise, the specified default value.
        /// </returns>
        string? GetStringValue(string key, string defaultValue);
        #endregion String-Access
        
        #region Properties
        /// <summary>
        /// Gets or sets the return URL.
        /// </summary>
        /// <value>A nullable string representing the return URL.</value>
        string? ReturnUrl { get; set; }
        /// <summary>
        /// Gets or sets the return controller value.
        /// </summary>
        /// <value>
        /// The return controller value.
        /// </value>
        string? ReturnController { get; set; }
        /// <summary>
        /// Gets or sets the return action for a specific task.
        /// </summary>
        /// <value>
        /// A string value representing the return action.
        /// </value>
        string? ReturnAction { get; set; }
        /// <summary>
        /// Gets or sets the hint for the string property.
        /// </summary>
        /// <value>
        /// The hint for the string property.
        /// </value>
        string? Hint { get; set; }
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        string? Error { get; set; }
        #endregion Properties
        
#if ACCOUNT_ON
        #region Authentication
        /// <summary>
        /// Gets or sets the login session associated with the account.
        /// </summary>
        /// <value>A nullable <c>LoginSession</c> object representing the login session.</value>
        /// <remarks>
        /// This property holds the login session associated with the account.
        /// The login session contains information about the user's login session,
        /// such as the session ID, start time, and end time.
        /// </remarks>
        Models.Account.LoginSession? LoginSession { get; set; }
        /// <summary>
        /// Gets the session token for the logged-in user.
        /// </summary>
        /// <remarks>
        /// This property returns the session token from the <see cref="LoginSession"/> object if it is not null, otherwise it returns an empty string.
        /// </remarks>
        /// <value>The session token for the logged-in user.</value>
        string SessionToken => LoginSession?.SessionToken ?? string.Empty;
        /// <summary>
        /// Gets a value indicating whether the user has been authenticated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the user is authenticated; otherwise, <c>false</c>.
        /// </value>
        bool IsAuthenticated { get; }
        /// <summary>
        /// Gets a value indicating whether the session is alive or not.
        /// </summary>
        /// <value>
        /// True if the session is alive; otherwise, false.
        /// </value>
        bool IsSessionAlive { get; }
        #endregion Authentication
#endif
    }
}
//MdEnd



