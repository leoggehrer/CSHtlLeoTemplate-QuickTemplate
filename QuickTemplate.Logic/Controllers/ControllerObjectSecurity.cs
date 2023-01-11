//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Controllers
{
    using QuickTemplate.Logic.DataContext;
    partial class ControllerObject
    {
        #region SessionToken
        private string? sessionToken;
        protected event EventHandler? ChangedSessionToken;

        /// <summary>
        /// Sets the session token.
        /// </summary>
        public string SessionToken
        {
            internal get => sessionToken ?? string.Empty;
            set
            {
                sessionToken = value;
                ChangedSessionToken?.Invoke(this, EventArgs.Empty);
            }
        }

        private void HandleChangedSessionToken(object source, EventArgs e)
        {
            var handled = false;

            BeforeHandleManagedMembers(ref handled);

            if (handled == false)
            {
            }
            AfterHandleManagedMembers();
        }
        partial void BeforeHandleManagedMembers(ref bool handled);
        partial void AfterHandleManagedMembers();
        #endregion SessionToken

        partial void ConstructingSecurityPart(ProjectDbContext context)
        {
            ChangedSessionToken += HandleChangedSessionToken!;
        }
        partial void ConstructingSecurityPart(ControllerObject other)
        {
            SessionToken = other.SessionToken;
            ChangedSessionToken += HandleChangedSessionToken!;
        }

        protected virtual Task CheckAuthorizationAsync(Type subjectType, string action)
        {
            return Modules.Security.Authorization.CheckAuthorizationAsync(SessionToken, subjectType, action, string.Empty);
        }
        protected virtual Task CheckAuthorizationAsync(Type subjectType, string action, string infoData)
        {
            return Modules.Security.Authorization.CheckAuthorizationAsync(SessionToken, subjectType, action, infoData);
        }

        protected virtual Task CheckAuthorizationAsync(Type subjectType, string action, params string[] roles)
        {
            return Modules.Security.Authorization.CheckAuthorizationAsync(SessionToken, subjectType, action, string.Empty, roles);
        }
        protected virtual Task CheckAuthorizationAsync(Type subjectType, string action, string infoData, params string[] roles)
        {
            return Modules.Security.Authorization.CheckAuthorizationAsync(SessionToken, subjectType, action, infoData, roles);
        }
    }
}
#endif
//MdEnd