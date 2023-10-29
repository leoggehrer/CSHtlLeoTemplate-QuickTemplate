//@BaseCode
//MdStart
#if ACCOUNT_ON
using QuickTemplate.Logic.Modules.Account;
using QuickTemplate.Logic.Modules.Exceptions;
using System.Reflection;
using Error = CommonBase.Modules.Exceptions.ErrorType;

namespace QuickTemplate.Logic.Modules.Security
{
    internal static partial class Authorization
    {
        /// <summary>
        /// Initializes the <see cref="Authorization"/> class.
        /// </summary>
        static Authorization()
        {
            ClassConstructing();
            if (string.IsNullOrEmpty(SystemAuthorizationToken))
            {
                SystemAuthorizationToken = Guid.NewGuid().ToString();
            }
            ClassConstructed();
        }
        /// <summary>
        /// This method is called when a class is being constructed.
        /// </summary>
        /// <remarks>
        /// This method is invoked before the constructor is executed.
        /// </remarks>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when the class is constructed.
        /// </summary>
        static partial void ClassConstructed();
        
        /// <summary>
        /// Gets or sets the default timeout value in minutes for a specific operation.
        /// </summary>
        /// <value>
        /// The default timeout value in minutes.
        /// </value>
        /// <remarks>
        /// The default timeout value is used when no specific timeout is specified for an operation.
        /// </remarks>
        internal static int DefaultTimeOutInMinutes { get; private set; } = 90;
        /// <summary>
        /// Gets or sets the default time-out value in seconds.
        /// </summary>
        /// <value>
        /// The default time-out value in seconds.
        /// </value>
        internal static int DefaultTimeOutInSeconds => DefaultTimeOutInMinutes * 60;
        /// <summary>
        /// Gets or sets the system authorization token.
        /// </summary>
        /// <value>
        /// The system authorization token.
        /// </value>
        internal static string SystemAuthorizationToken { get; set; }
        
        /// <summary>
        /// Checks authorization asynchronously.
        /// </summary>
        /// <param name="sessionToken">The session token used for authorization.</param>
        /// <param name="subjectType">The type of the subject being authorized.</param>
        /// <param name="action">The action being authorized.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        internal static Task CheckAuthorizationAsync(string? sessionToken, Type subjectType, string action)
        {
            return CheckAuthorizationAsync(sessionToken, subjectType, action, string.Empty);
        }
        /// <summary>
        /// Checks the authorization for a given session token, subject type, action, and info data.
        /// </summary>
        /// <param name="sessionToken">The session token representing the user session.</param>
        /// <param name="subjectType">The type of the subject on which the action is being performed.</param>
        /// <param name="action">The action being performed.</param>
        /// <param name="infoData">Additional information/data related to the action.</param>
        /// <returns>A task representing the asynchronous operation of checking authorization.</returns>
        /// <remarks>
        /// This method checks authorization by invoking the <see cref="BeforeCheckAuthorization"/> method to allow any pre-check handling.
        /// If the authorization check is not handled by the pre-check handling, it then calls the
        /// <see cref="CheckAuthorizationInternalAsync"/> method asynchronously to perform the authorization check.
        /// Finally, it invokes the <see cref="AfterCheckAuthorization"/> method to perform any post-check handling.
        /// </remarks>
        internal static async Task CheckAuthorizationAsync(string? sessionToken, Type subjectType, string action, string infoData)
        {
            bool handled = false;
            
            BeforeCheckAuthorization(sessionToken, subjectType, action, ref handled);
            if (handled == false)
            {
                await CheckAuthorizationInternalAsync(sessionToken, subjectType, action, infoData).ConfigureAwait(false);
            }
            AfterCheckAuthorization(sessionToken, subjectType, action);
        }
        /// <summary>
        /// Performs tasks before checking authorization for a given session token, subject type, and action.
        /// </summary>
        /// <param name="sessionToken">The session token for the current user.</param>
        /// <param name="subjectType">The type of subject that requires authorization.</param>
        /// <param name="action">The action that requires authorization.</param>
        /// <param name="handled">A reference to a boolean indicating whether the authorization check has been handled.</param>
        /// <remarks>
        /// This method allows performing any necessary tasks before checking authorization for a given session token, subject type, and action. It can be used to modify the incoming parameters or perform additional operations related to authorization.
        /// </remarks>
        static partial void BeforeCheckAuthorization(string? sessionToken, Type subjectType, string action, ref bool handled);
        /// <summary>
        /// Performs additional actions after checking the authorization of a session token for a specified subject type and action.
        /// </summary>
        /// <param name="sessionToken">The session token to be checked for authorization.</param>
        /// <param name="subjectType">The type of the subject being authorized.</param>
        /// <param name="action">The action to be authorized.</param>
        /// <remarks>
        /// This method is called after the authorization check is performed on a session token for a specified subject type and action.
        /// It allows for additional actions or logging to be performed based on the result of the authorization check.
        /// </remarks>
        static partial void AfterCheckAuthorization(string? sessionToken, Type subjectType, string action);
        
        /// <summary>
        /// Checks authorization asynchronously.
        /// </summary>
        /// <param name="sessionToken">The session token for authentication.</param>
        /// <param name="subjectType">The type of the subject.</param>
        /// <param name="action">The action to be performed.</param>
        /// <param name="roles">The roles required for authorization.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        internal static Task CheckAuthorizationAsync(string? sessionToken, Type subjectType, string action, params string[] roles)
        {
            return CheckAuthorizationAsync(sessionToken, subjectType, action, string.Empty, roles);
        }
        /// <summary>
        /// Checks authorization for a given session token, subject type, action, informational data, and roles.
        /// </summary>
        /// <param name="sessionToken">The session token.</param>
        /// <param name="subjectType">The type of the subject.</param>
        /// <param name="action">The action to be authorized.</param>
        /// <param name="infoData">The informational data.</param>
        /// <param name="roles">The roles.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method checks authorization for a given session token, subject type, action, informational data, and roles.
        /// It first calls the <see cref="BeforeCheckAuthorization"/> method to perform any pre-authorization checks and handle them accordingly.
        /// If the <paramref name="handled"/> flag is false after calling <see cref="BeforeCheckAuthorization"/>,
        /// the internal authorization validation process will be executed by calling the <see cref="CheckAuthorizationInternalAsync"/> method.
        /// After checking authorization, the <see cref="AfterCheckAuthorization"/> method is called to perform any post-authorization tasks.
        /// </remarks>
        internal static async Task CheckAuthorizationAsync(string? sessionToken, Type subjectType, string action, string infoData, params string[] roles)
        {
            bool handled = false;
            
            BeforeCheckAuthorization(sessionToken, subjectType, action, roles, ref handled);
            if (handled == false)
            {
                await CheckAuthorizationInternalAsync(sessionToken, subjectType, action, infoData, roles).ConfigureAwait(false);
            }
            AfterCheckAuthorization(sessionToken, subjectType, action, roles);
        }
        /// <summary>
        /// This method is called before checking the authorization of a session token.
        /// </summary>
        /// <param name="sessionToken">The session token to be checked for authorization.</param>
        /// <param name="subjectType">The type of the subject to be authorized.</param>
        /// <param name="action">The action to be authorized.</param>
        /// <param name="roles">The roles assigned to the subject.</param>
        /// <param name="handled">A reference to a boolean indicating if the authorization has been handled.</param>
        static partial void BeforeCheckAuthorization(string? sessionToken, Type subjectType, string action, string[] roles, ref bool handled);
        /// <summary>
        /// Executes after the authorization check is performed.
        /// </summary>
        /// <param name="sessionToken">The token representing the current session (nullable).</param>
        /// <param name="subjectType">The type of subject being authorized.</param>
        /// <param name="action">The action being authorized.</param>
        /// <param name="roles">The roles associated with the authorized subject.</param>
        /// <remarks>
        /// This method can be used to perform additional tasks after the authorization check is made,
        /// such as logging, auditing, or applying additional authorization logic.
        /// </remarks>
        static partial void AfterCheckAuthorization(string? sessionToken, Type subjectType, string action, string[] roles);
        
        private static async Task CheckAuthorizationInternalAsync(string? sessionToken, Type subjectType, string action, string infoData)
        {
            static AuthorizeAttribute? GetClassAuthorization(Type classType)
            {
                var runType = classType;
                var result = default(AuthorizeAttribute);
                
                do
                {
                    result = runType.GetCustomAttributes<AuthorizeAttribute>().FirstOrDefault();
                    runType = runType.BaseType;
                } while (result == null && runType != null);
                return result;
            }
            
            if (string.IsNullOrEmpty(sessionToken))
            {
                var authorization = GetClassAuthorization(subjectType);
                var isRequired = authorization?.Required ?? false;
                
                if (isRequired)
                {
                    throw new AuthorizationException(Error.NotLogedIn);
                }
            }
            else if (sessionToken.Equals(SystemAuthorizationToken) == false)
            {
                var authorization = GetClassAuthorization(subjectType);
                
                if (authorization != null && authorization.Required)
                {
                    var curSession = await AccountManager.QueryAliveSessionAsync(sessionToken).ConfigureAwait(false);
                    
                    if (curSession == null)
                    throw new AuthorizationException(Error.InvalidSessionToken);
                    
                    if (curSession.IsTimeout)
                    throw new AuthorizationException(Error.AuthorizationTimeOut);
                    
                    bool isAuthorized = authorization.Roles.Any() == false
                    || curSession.Roles.Any(lr => authorization.Roles.Contains(lr.Designation));
                    
                    if (isAuthorized == false)
                    throw new AuthorizationException(Error.NotAuthorized);
                    
                    curSession.LastAccess = DateTime.UtcNow;
#if LOGGING_ON
                    Logging(curSession.IdentityId, subjectType.Name, action, infoData);
#endif
                }
            }
        }
        private static async Task CheckAuthorizationInternalAsync(string? sessionToken, Type subjectType, string action, string infoData, params string[] roles)
        {
            if (string.IsNullOrEmpty(sessionToken))
            {
                throw new AuthorizationException(Error.NotLogedIn);
            }
            else if (sessionToken.Equals(SystemAuthorizationToken) == false)
            {
                var curSession = await AccountManager.QueryAliveSessionAsync(sessionToken).ConfigureAwait(false);
                
                if (curSession == null)
                throw new AuthorizationException(Error.InvalidSessionToken);
                
                if (curSession.IsTimeout)
                throw new AuthorizationException(Error.AuthorizationTimeOut);
                
                bool isAuthorized = curSession.Roles.Any(lr => roles.Contains(lr.Designation));
                
                if (isAuthorized == false)
                throw new AuthorizationException(Error.NotAuthorized);
                
                curSession.LastAccess = DateTime.UtcNow;
#if LOGGING_ON
                Logging(curSession.IdentityId, subjectType.Name, action, infoData);
#endif
            }
        }
        
        /// <summary>
        /// Checks the authorization for the given session token, method information, action and info data.
        /// </summary>
        /// <param name="sessionToken">The session token to check authorization for.</param>
        /// <param name="methodInfo">The method information of the calling method.</param>
        /// <param name="action">The action to check authorization for.</param>
        /// <param name="infoData">The information data related to the authorization check.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        internal static async Task CheckAuthorizationAsync(string? sessionToken, MethodInfo methodInfo, string action, string infoData)
        {
            bool handled = false;
            
            BeforeCheckAuthorization(sessionToken, methodInfo, action, ref handled);
            if (handled == false)
            {
                await CheckAuthorizationInternalAsync(sessionToken, methodInfo, action, infoData).ConfigureAwait(false);
            }
            AfterCheckAuthorization(sessionToken, methodInfo, action);
        }
        /// <summary>
        /// Performs an action before checking the authorization for a specified method.
        /// </summary>
        /// <param name="sessionToken">Session token used for authentication. Can be null.</param>
        /// <param name="methodInfo">MethodInfo object representing the method being authorized.</param>
        /// <param name="action">Action string indicating the type of authorization being performed.</param>
        /// <param name="handled">Output parameter indicating whether the authorization check has been handled.</param>
        static partial void BeforeCheckAuthorization(string? sessionToken, MethodInfo methodInfo, string action, ref bool handled);
        /// <summary>
        /// Performs additional operations after checking authorization for a specific action.
        /// </summary>
        /// <param name="sessionToken">The token that identifies the current session. Can be null.</param>
        /// <param name="methodInfo">Information about the method that was authorized.</param>
        /// <param name="action">The action that was authorized.</param>
        /// <remarks>
        /// This method is called after the authorization check has been performed for the given action.
        /// Use this method to perform any additional operations or validations related to authorization.
        /// </remarks>
        static partial void AfterCheckAuthorization(string? sessionToken, MethodInfo methodInfo, string action);
        
        private static async Task CheckAuthorizationInternalAsync(string? sessionToken, MethodBase methodBase, string action, string infoData)
        {
            static AuthorizeAttribute? GetMethodAuthorization(MethodBase methodBase)
            {
                return methodBase.GetCustomAttributes<AuthorizeAttribute>().FirstOrDefault();
            }
            
            if (string.IsNullOrEmpty(sessionToken))
            {
                var authorization = GetMethodAuthorization(methodBase);
                var isRequired = authorization?.Required ?? false;
                
                if (isRequired)
                {
                    throw new AuthorizationException(Error.NotLogedIn);
                }
            }
            else if (sessionToken.Equals(SystemAuthorizationToken) == false)
            {
                var authorization = GetMethodAuthorization(methodBase);
                
                if (authorization != null && authorization.Required)
                {
                    var curSession = await AccountManager.QueryAliveSessionAsync(sessionToken).ConfigureAwait(false);
                    
                    if (curSession == null)
                    throw new AuthorizationException(Error.InvalidSessionToken);
                    
                    if (curSession.IsTimeout)
                    throw new AuthorizationException(Error.AuthorizationTimeOut);
                    
                    bool isAuthorized = authorization.Roles.Any() == false
                    || curSession.Roles.Any(lr => authorization.Roles.Contains(lr.Designation));
                    
                    if (isAuthorized == false)
                    throw new AuthorizationException(Error.NotAuthorized);
                    
                    curSession.LastAccess = DateTime.UtcNow;
#if LOGGING_ON
                    Logging(curSession.IdentityId, methodBase.Name, action, infoData);
#endif
                }
            }
        }
        
#if LOGGING_ON
        /// <summary>
        /// Logs an action performed by a user to the action log.
        /// </summary>
        /// <param name="identityId">The identity ID of the user performing the action.</param>
        /// <param name="subject">The subject of the action.</param>
        /// <param name="action">The action performed.</param>
        /// <param name="info">Additional information about the action.</param>
        /// <remarks>
        /// This method logs the action to the action log database asynchronously.
        /// It first checks if the action is already handled by invoking the <see cref="BeforeLogging"/> method.
        /// If the action is not handled, it creates a new instance of the <see cref="Controllers.Logging.ActionLogsController"/> class
        /// and inserts a new record in the action log table with the provided identity ID, current timestamp, subject, action, and info.
        /// Finally, it invokes the <see cref="AfterLogging"/> method.
        /// </remarks>
        private static void Logging(IdType identityId, string subject, string action, string info)
        {
            Task.Run(async () =>
            {
                bool handled = false;
                
                BeforeLogging(subject, action, ref handled);
                if (handled == false)
                {
                    using var actionLogCtrl = new Controllers.Logging.ActionLogsController()
                    {
                        SessionToken = SystemAuthorizationToken
                    };
                    var entity = new Entities.Logging.ActionLog
                    {
                        IdentityId = identityId,
                        Time = DateTime.Now,
                        Subject = subject,
                        Action = action,
                        Info = info
                    };
                    await actionLogCtrl.InsertAsync(entity).ConfigureAwait(false);
                    await actionLogCtrl.SaveChangesAsync().ConfigureAwait(false);
                }
                AfterLogging(subject, action);
            });
        }
        /// <summary>
        /// Method called before logging an action.
        /// </summary>
        /// <param name="subject">The subject of the action.</param>
        /// <param name="action">The action being performed.</param>
        /// <param name="handled">Flag indicating whether the action has been handled.</param>
        /// <remarks>
        /// This method is invoked before the actual logging of an action. It allows for additional handling or modification
        /// of the action or subject. By setting the <paramref name="handled"/> parameter, the method caller can indicate
        /// whether the action has been fully handled and no further logging is required.
        /// </remarks>
        static partial void BeforeLogging(string subject, string action, ref bool handled);
        /// <summary>
        /// This method is called after logging a subject and an action.
        /// </summary>
        /// <param name="subject">The subject of the log.</param>
        /// <param name="action">The action performed.</param>
        /// <remarks>
        /// This method can be implemented by partial classes to perform any actions
        /// that need to be executed after logging the subject and action.
        /// </remarks>
        static partial void AfterLogging(string subject, string action);
#endif
    }
}
#endif
//MdEnd

