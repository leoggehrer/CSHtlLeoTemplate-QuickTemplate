//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic
{
    using QuickTemplate.Logic.Models.Account;
    using QuickTemplate.Logic.Modules.Account;
    /// <summary>
    /// Provides access to various account-related operations.
    /// </summary>
    public static partial class AccountAccess
    {
        /// <summary>
        /// Checks if the session associated with the given session token is alive.
        /// </summary>
        /// <param name="sessionToken">The session token used for authentication.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a boolean indicating whether the session is alive or not.</returns>
        public static Task<bool> IsSessionAliveAsync(string sessionToken)
        {
            return AccountManager.IsSessionAliveAsync(sessionToken);
        }
        
        /// <summary>
        /// Initializes application access for a user by calling the AccountManager's InitAppAccessAsync method.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="enableJwtAuth">A flag indicating whether to enable JWT authentication.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public static Task InitAppAccessAsync(string name, string email, string password, bool enableJwtAuth)
        {
            return AccountManager.InitAppAccessAsync(name, email, password, enableJwtAuth);
        }
        /// <summary>
        /// Asynchronously adds application access for a user.
        /// </summary>
        /// <param name="sessionToken">The session token of the user.</param>
        /// <param name="name">The name of the user.</param>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="timeOutInMinutes">The timeout duration for the access in minutes.</param>
        /// <param name="enableJwtAuth">True if JWT authentication is enabled, false otherwise.</param>
        /// <param name="roles">An optional array of roles to assign to the user.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method adds application access for a user by calling the AddAppAccessAsync method in the AccountManager class.
        /// </remarks>
        public static Task AddAppAccessAsync(string sessionToken, string name, string email, string password, int timeOutInMinutes, bool enableJwtAuth, params string[] roles)
        {
            return AccountManager.AddAppAccessAsync(sessionToken, name, email, password, timeOutInMinutes, enableJwtAuth, roles);
        }
        
        /// <summary>
        /// Asynchronously logs in using a JSON web token.
        /// </summary>
        /// <param name="jsonWebToken">The JSON web token to be used for logging in.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a LoginSession object.</returns>
        public static async Task<LoginSession> LogonAsync(string jsonWebToken)
        {
            var result = await AccountManager.LogonAsync(jsonWebToken).ConfigureAwait(false);
            
            return LoginSession.Create(result);
        }
        /// <summary>
        /// Logs a user on asynchronously using the provided email and password.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>A task that represents the asynchronous logon operation. The task result contains a <see cref="LoginSession"/> object.</returns>
        public static async Task<LoginSession> LogonAsync(string email, string password)
        {
            var result = await AccountManager.LogonAsync(email, password).ConfigureAwait(false);
            
            return LoginSession.Create(result);
        }
        /// <summary>
        /// Asynchronously logs on a user using the specified email, password, and optional information.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="optionalInfo">Optional information related to the user.</param>
        /// <returns>The login session object.</returns>
        public static async Task<LoginSession> LogonAsync(string email, string password, string optionalInfo)
        {
            var result = await AccountManager.LogonAsync(email, password, optionalInfo).ConfigureAwait(false);
            
            return LoginSession.Create(result);
        }
        /// <summary>
        /// Queries the login using the provided session token asynchronously.
        /// </summary>
        /// <param name="sessionToken">The session token to be used for querying login.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result is a nullable LoginSession.
        /// If the login is successfully queried, a new LoginSession instance is created and returned.
        /// If the login query returns null, null is returned.
        /// </returns>
        public static async Task<LoginSession?> QueryLoginAsync(string sessionToken)
        {
            var result = await AccountManager.QueryLoginAsync(sessionToken).ConfigureAwait(false);
            
            return result != null ? LoginSession.Create(result) : null;
        }
        
        /// <summary>
        /// Checks if a user, identified by the sessionToken, has a specific role.
        /// </summary>
        /// <param name="sessionToken">The session token of the user.</param>
        /// <param name="role">The role to check.</param>
        /// <returns>
        /// A Task representing the asynchronous operation that yields a boolean value indicating
        /// if the user has the specified role. True if the user has the role, false otherwise.
        /// </returns>
        public static Task<bool> HasRoleAsync(string sessionToken, string role)
        {
            return AccountManager.HasRoleAsync(sessionToken, role);
        }
        /// <summary>
        /// Asynchronously queries the roles associated with a session token.
        /// </summary>
        /// <param name="sessionToken">The session token used to authenticate the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the enumerable collection of role names.</returns>
        public static Task<IEnumerable<string>> QueryRolesAsync(string sessionToken)
        {
            return AccountManager.QueryRolesAsync(sessionToken);
        }
        
        /// <summary>
        /// Changes the password for a user asynchronously.
        /// </summary>
        /// <param name="sessionToken">The session token for the user.</param>
        /// <param name="oldPassword">The old password for the user.</param>
        /// <param name="newPassword">The new password for the user.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task ChangePasswordAsync(string sessionToken, string oldPassword, string newPassword)
        {
            return AccountManager.ChangePasswordAsync(sessionToken, oldPassword, newPassword);
        }
        /// <summary>
        /// Changes the password for the specified email address asynchronously.
        /// </summary>
        /// <param name="sessionToken">The session token associated with the account.</param>
        /// <param name="email">The email address of the account.</param>
        /// <param name="newPassword">The new password to set.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task ChangePasswordForAsync(string sessionToken, string email, string newPassword)
        {
            return AccountManager.ChangePasswordAsync(sessionToken, email, newPassword);
        }
        /// <summary>
        /// Resets the failed login count for a user asynchronously.
        /// </summary>
        /// <param name="sessionToken">The session token of the authenticated user.</param>
        /// <param name="email">The email address of the user.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static Task ResetFailedCountForAsync(string sessionToken, string email)
        {
            return AccountManager.ResetFailedCountForAsync(sessionToken, email);
        }
        
        /// <summary>
        /// Logs out the specified user session asynchronously.
        /// </summary>
        /// <param name="sessionToken">The session token of the user.</param>
        /// <returns>A task representing the asynchronous logout operation.</returns>
        public static Task LogoutAsync(string sessionToken)
        {
            return AccountManager.LogoutAsync(sessionToken);
        }
        
        /// <summary>
        /// Retrieves an Identity asynchronously by session token and ID.
        /// </summary>
        /// <param name="sessionToken">The session token to use for authentication.</param>
        /// <param name="id">The ID of the Identity to retrieve.</param>
        /// <returns>An asynchronous task that represents the operation. The task result contains the retrieved Identity.</returns>
        /// <exception cref="Modules.Exceptions.LogicException">Thrown when the provided ID is invalid.</exception>
        public static async Task<Identity> GetIdentityByAsync(string sessionToken, IdType id)
        {
            using var ctrl = new Controllers.Account.IdentitiesController() { SessionToken = sessionToken };
            var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);
            
            return entity != null ? Identity.Create(entity) : throw new Modules.Exceptions.LogicException(CommonBase.Modules.Exceptions.ErrorType.InvalidId);
        }
        /// <summary>
        /// Retrieves an array of identities asynchronously using the specified session token.
        /// </summary>
        /// <param name="sessionToken">The session token used for authentication.</param>
        /// <returns>An array of Identity objects.</returns>
        /// <remarks>
        /// This method retrieves a collection of identities from the IdentitiesController by initializing the controller with the provided session token.
        /// It then asynchronously calls the GetAllAsync method to retrieve all entities from the controller.
        /// The retrieved entities are then used to create Identity objects which are stored in an array before being returned.
        /// </remarks>
        public static async Task<Identity[]> GetIdentitiesAsync(string sessionToken)
        {
            using var ctrl = new Controllers.Account.IdentitiesController() { SessionToken = sessionToken };
            var entities = await ctrl.GetAllAsync().ConfigureAwait(false);
            
            return entities.Select(e => Identity.Create(e)).ToArray();
        }
        /// <summary>
        /// Updates an identity asynchronously.
        /// </summary>
        /// <param name="sessionToken">The session token used for authorization.</param>
        /// <param name="id">The ID of the identity to update.</param>
        /// <param name="identity">The new identity data to update.</param>
        /// <returns>The updated identity.</returns>
        /// <exception cref="Modules.Exceptions.LogicException">Thrown when the provided ID is invalid.</exception>
        public static async Task<Identity> UpdateIdentityAsync(string sessionToken, IdType id, Identity identity)
        {
            using var ctrl = new Controllers.Account.IdentitiesController() { SessionToken = sessionToken };
            var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);
            
            if (entity == null)
            {
                throw new Modules.Exceptions.LogicException(CommonBase.Modules.Exceptions.ErrorType.InvalidId);
            }
            
            entity.CopyFrom(identity, n => n.Equals("Guid", StringComparison.InvariantCultureIgnoreCase) == false);
            entity = await ctrl.UpdateAsync(entity).ConfigureAwait(false);
            await ctrl.SaveChangesAsync().ConfigureAwait(false);
            
            identity.CopyFrom(entity);
            return identity;
        }
        /// <summary>
        /// Deletes an identity asynchronously.
        /// </summary>
        /// <param name="sessionToken">The session token for authentication.</param>
        /// <param name="id">The ID of the identity to be deleted.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        /// <exception cref="Modules.Exceptions.LogicException">Thrown when the provided ID is invalid.</exception>
        public static async Task DeleteIdentityAsync(string sessionToken, IdType id)
        {
            using var ctrl = new Controllers.Account.IdentitiesController() { SessionToken = sessionToken };
            var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);
            
            if (entity == null)
            {
                throw new Modules.Exceptions.LogicException(CommonBase.Modules.Exceptions.ErrorType.InvalidId);
            }
            
            await ctrl.DeleteAsync(id).ConfigureAwait(false);
            await ctrl.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
#endif
//MdEnd

