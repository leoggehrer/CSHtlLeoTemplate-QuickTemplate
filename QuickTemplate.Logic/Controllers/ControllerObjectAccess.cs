//@BaseCode
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
using CommonBase.Contracts;
using Error = CommonBase.Modules.Exceptions.ErrorType;

namespace QuickTemplate.Logic.Controllers
{
    /// <summary>
    /// Partial class representing a ControllerObject.
    /// </summary>
    partial class ControllerObject
    {
        #region Fields
        private Access.AccessRulesController? _accessRulesController = null;
        #endregion Fields
        
        #region Properties
        /// <summary>
        /// Gets the access rules controller for managing access rules.
        /// </summary>
        /// <value>
        /// An instance of <see cref="Access.AccessRulesController"/>.
        /// </value>
        internal Access.AccessRulesController AccessRulesController
        {
            get
            {
                return _accessRulesController ??= new Access.AccessRulesController(this);
            }
        }
        #endregion Properties
        
        #region Methodes
        /// <summary>
        /// Asynchronously checks if the specified subject type can be created.
        /// </summary>
        /// <param name="subjectType">The type of the subject.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="Modules.Exceptions.AuthorizationException">Thrown when the session token is invalid.</exception>
        /// <exception cref="Modules.Exceptions.AccessRuleException">Thrown when there is a violation of the access rules and the subject type cannot be created.</exception>
        protected virtual async Task CheckCanBeCreatedAsync(Type subjectType)
        {
            var curSession = await Modules.Account.AccountManager.QueryAliveSessionAsync(SessionToken).ConfigureAwait(false);
            
            if (curSession == null)
            throw new Modules.Exceptions.AuthorizationException(Error.InvalidSessionToken);
            
            var canBeCreated = await AccessRulesController.CanBeCreatedAsync(subjectType, curSession.Identity!).ConfigureAwait(false);
            
            if (canBeCreated == false)
            throw new Modules.Exceptions.AccessRuleException(Error.AccessRuleViolationCanNotCreated);
        }
        /// <summary>
        /// Checks if the specified entity can be read asynchronously.
        /// </summary>
        /// <param name="entity">The entity to check for readability.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="Modules.Exceptions.AuthorizationException">Thrown when the current session token is invalid.</exception>
        /// <exception cref="Modules.Exceptions.AccessRuleException">Thrown when the access rules do not allow reading the entity.</exception>
        protected virtual async Task CheckCanBeReadAsync(IIdentifyable entity)
        {
            var curSession = await Modules.Account.AccountManager.QueryAliveSessionAsync(SessionToken).ConfigureAwait(false);
            
            if (curSession == null)
            throw new Modules.Exceptions.AuthorizationException(Error.InvalidSessionToken);
            
            var canBeCreated = await AccessRulesController.CanBeReadAsync(entity, curSession.Identity!).ConfigureAwait(false);
            
            if (canBeCreated == false)
            throw new Modules.Exceptions.AccessRuleException(Error.AccessRuleViolationCanNotRead);
        }
        /// <summary>
        /// Checks if the specified entity can be changed.
        /// </summary>
        /// <remarks>
        /// This method performs the following checks:
        /// - Retrieves the current session using the provided session token.
        /// - Validates the session and throws an authorization exception if it is invalid.
        /// - Calls the CanBeChangedAsync method of the AccessRulesController to determine if the entity can be changed.
        /// - Throws an access rule exception if the entity cannot be changed.
        /// </remarks>
        /// <param name="entity">The entity to check.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="Modules.Exceptions.AuthorizationException">Thrown if the session token is invalid.</exception>
        /// <exception cref="Modules.Exceptions.AccessRuleException">Thrown if the entity cannot be changed.</exception>
        protected virtual async Task CheckCanBeChangedAsync(IIdentifyable entity)
        {
            var curSession = await Modules.Account.AccountManager.QueryAliveSessionAsync(SessionToken).ConfigureAwait(false);
            
            if (curSession == null)
            throw new Modules.Exceptions.AuthorizationException(Error.InvalidSessionToken);
            
            var canBeCreated = await AccessRulesController.CanBeChangedAsync(entity, curSession.Identity!).ConfigureAwait(false);
            
            if (canBeCreated == false)
            throw new Modules.Exceptions.AccessRuleException(Error.AccessRuleViolationCanNotChanged);
        }
        /// <summary>
        /// Checks if the specified entity can be deleted.
        /// </summary>
        /// <param name="entity">The entity to be checked if it can be deleted.</param>
        /// <returns>A task representing the asynchronous operation of checking if the entity can be deleted.</returns>
        /// <exception cref="Modules.Exceptions.AuthorizationException">Thrown if the session token is invalid.</exception>
        /// <exception cref="Modules.Exceptions.AccessRuleException">Thrown if the access rule violation prevents the entity from being deleted.</exception>
        /// <remarks>
        /// This method verifies if the current session is alive using the specified session token.
        /// If the session token is invalid, an AuthorizationException is thrown.
        /// It then checks if the specified entity can be deleted using the AccessRulesController.
        /// If the entity cannot be deleted, an AccessRuleException is thrown.
        /// </remarks>
        protected virtual async Task CheckCanBeDeletedAsync(IIdentifyable entity)
        {
            var curSession = await Modules.Account.AccountManager.QueryAliveSessionAsync(SessionToken).ConfigureAwait(false);
            
            if (curSession == null)
            throw new Modules.Exceptions.AuthorizationException(Error.InvalidSessionToken);
            
            var canBeCreated = await AccessRulesController.CanBeDeletedAsync(entity, curSession.Identity!).ConfigureAwait(false);
            
            if (canBeCreated == false)
            throw new Modules.Exceptions.AccessRuleException(Error.AccessRuleViolationCanNotDeleted);
        }
        
        /// <summary>
        /// Disposes the access part of the object.
        /// </summary>
        partial void DisposeAccessPart()
        {
            _accessRulesController?.Dispose();
            _accessRulesController = null;
        }
        #endregion Methodes
    }
}
#endif
//MdEnd

