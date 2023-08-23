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
        static Authorization()
        {
            ClassConstructing();
            if (string.IsNullOrEmpty(SystemAuthorizationToken))
            {
                SystemAuthorizationToken = Guid.NewGuid().ToString();
            }
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        internal static int DefaultTimeOutInMinutes { get; private set; } = 90;
        internal static int DefaultTimeOutInSeconds => DefaultTimeOutInMinutes * 60;
        internal static string SystemAuthorizationToken { get; set; }

        internal static Task CheckAuthorizationAsync(string? sessionToken, Type subjectType, string action)
        {
            return CheckAuthorizationAsync(sessionToken, subjectType, action, string.Empty);
        }
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
        static partial void BeforeCheckAuthorization(string? sessionToken, Type subjectType, string action, ref bool handled);
        static partial void AfterCheckAuthorization(string? sessionToken, Type subjectType, string action);

        internal static Task CheckAuthorizationAsync(string? sessionToken, Type subjectType, string action, params string[] roles)
        {
            return CheckAuthorizationAsync(sessionToken, subjectType, action, string.Empty, roles);
        }
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
        static partial void BeforeCheckAuthorization(string? sessionToken, Type subjectType, string action, string[] roles, ref bool handled);
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

#if LOGGING_ON
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
        static partial void BeforeLogging(string subject, string action, ref bool handled);
        static partial void AfterLogging(string subject, string action);
#endif
    }
}
#endif
//MdEnd
