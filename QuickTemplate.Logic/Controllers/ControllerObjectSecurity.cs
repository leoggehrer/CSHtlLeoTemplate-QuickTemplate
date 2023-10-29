//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Controllers
{
    using QuickTemplate.Logic.DataContext;
    /// <summary>
    /// Represents a controller object.
    /// </summary>
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
        
        /// <summary>
        /// Handles the event of a changed session token.
        /// </summary>
        /// <param name="source">The source object that raised the event.</param>
        /// <param name="e">The EventArgs associated with the event.</param>
        private void HandleChangedSessionToken(object source, EventArgs e)
        {
            var handled = false;
            
            BeforeHandleManagedMembers(ref handled);
            
            if (handled == false)
            {
            }
            AfterHandleManagedMembers();
        }
        /// <summary>
        /// This method is called before handling managed members.
        /// </summary>
        /// <param name="handled">A reference to a boolean indicating if the members have been handled.</param>
        /// <remarks>
        /// The BeforeHandleManagedMembers method is called before starting the process of handling managed members.
        /// It allows for any necessary pre-processing or checks to be performed before proceeding with the handling of managed members.
        /// </remarks>
        partial void BeforeHandleManagedMembers(ref bool handled);
        /// <summary>
        /// An optional method that can be implemented by a class that handles managed members.
        /// This method is called after the class has finished handling the managed members.
        /// </summary>
        /// <remarks>
        /// This method is intended to provide a hook for performing any necessary actions or clean-up
        /// after the managed members have been handled by the class.
        /// </remarks>
        partial void AfterHandleManagedMembers();
        #endregion SessionToken
        
        /// <summary>
        /// Constructs the security part of the project database context.
        /// </summary>
        /// <param name="context">The ProjectDbContext object.</param>
        /// <remarks>
        /// This partial method is automatically called during the construction of the ProjectDbContext class.
        /// It handles the event of changing the session token by subscribing to the ChangedSessionToken event.
        /// </remarks>
        partial void ConstructingSecurityPart(ProjectDbContext context)
        {
            ChangedSessionToken += HandleChangedSessionToken!;
        }
        /// <summary>
        /// Copies the session token from the specified ControllerObject and subscribes to the ChangedSessionToken event.
        /// </summary>
        /// <param name="other">The ControllerObject from which to copy the session token.</param>
        /// <remarks>
        /// This method is called during the construction of a ControllerObject and is intended for internal use only.
        /// </remarks>
        /// <seealso cref="HandleChangedSessionToken"/>
        partial void ConstructingSecurityPart(ControllerObject other)
        {
            SessionToken = other.SessionToken;
            ChangedSessionToken += HandleChangedSessionToken!;
        }
        
        /// <summary>
        /// Checks the authorization for a specified subject type and action asynchronously.
        /// </summary>
        /// <param name="subjectType">The type of the subject.</param>
        /// <param name="action">The action to be performed on the subject.</param>
        /// <returns>A Task representing the asynchronous operation of checking authorization.</returns>
        protected virtual Task CheckAuthorizationAsync(Type subjectType, string action)
        {
            return Modules.Security.Authorization.CheckAuthorizationAsync(SessionToken, subjectType, action, string.Empty);
        }
        /// <summary>
        /// Checks the authorization for the specified subject type, action, and info data asynchronously.
        /// </summary>
        /// <param name="subjectType">The type of the subject.</param>
        /// <param name="action">The action to perform.</param>
        /// <param name="infoData">Additional info data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected virtual Task CheckAuthorizationAsync(Type subjectType, string action, string infoData)
        {
            return Modules.Security.Authorization.CheckAuthorizationAsync(SessionToken, subjectType, action, infoData);
        }
        /// <summary>
        /// Checks authorization asynchronously for a given subject type and action.
        /// </summary>
        /// <param name="subjectType">The type of the subject for which authorization is being checked.</param>
        /// <param name="action">The action being performed on the subject.</param>
        /// <param name="roles">The roles that are required for authorization.</param>
        /// <returns>A Task representing the asynchronous operation of checking authorization.</returns>
        protected virtual Task CheckAuthorizationAsync(Type subjectType, string action, params string[] roles)
        {
            return Modules.Security.Authorization.CheckAuthorizationAsync(SessionToken, subjectType, action, string.Empty, roles);
        }
        /// <summary>
        /// Checks the authorization for the specified subject type, action, info data and roles asynchronously.
        /// </summary>
        /// <param name="subjectType">The type of the subject.</param>
        /// <param name="action">The action to be performed.</param>
        /// <param name="infoData">The additional information data.</param>
        /// <param name="roles">The roles allowed for authorization.</param>
        /// <returns>A task representing the asynchronous operation, which will return the result of the authorization check.</returns>
        protected virtual Task CheckAuthorizationAsync(Type subjectType, string action, string infoData, params string[] roles)
        {
            return Modules.Security.Authorization.CheckAuthorizationAsync(SessionToken, subjectType, action, infoData, roles);
        }
    }
}
#endif
//MdEnd

