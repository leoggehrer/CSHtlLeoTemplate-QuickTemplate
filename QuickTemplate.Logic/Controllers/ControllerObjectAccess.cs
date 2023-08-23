//@BaseCode
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
using CommonBase.Contracts;
using Error = CommonBase.Modules.Exceptions.ErrorType;

namespace QuickTemplate.Logic.Controllers
{
    partial class ControllerObject
    {
        #region Fields
        private Access.AccessRulesController? _accessRulesController = null;
        #endregion Fields

        #region Properties
        internal Access.AccessRulesController AccessRulesController
        {
            get
            {
                return _accessRulesController ??= new Access.AccessRulesController(this);
            }
        }
        #endregion Properties

        #region Methodes
        protected virtual async Task CheckCanBeCreatedAsync(Type subjectType)
        {
            var curSession = await Modules.Account.AccountManager.QueryAliveSessionAsync(SessionToken).ConfigureAwait(false);

            if (curSession == null)
                throw new Modules.Exceptions.AuthorizationException(Error.InvalidSessionToken);

            var canBeCreated = await AccessRulesController.CanBeCreatedAsync(subjectType, curSession.Identity!).ConfigureAwait(false);

            if (canBeCreated == false)
                throw new Modules.Exceptions.AccessRuleException(Error.AccessRuleViolationCanNotCreated);
        }
        protected virtual async Task CheckCanBeReadAsync(IIdentifyable entity)
        {
            var curSession = await Modules.Account.AccountManager.QueryAliveSessionAsync(SessionToken).ConfigureAwait(false);

            if (curSession == null)
                throw new Modules.Exceptions.AuthorizationException(Error.InvalidSessionToken);

            var canBeCreated = await AccessRulesController.CanBeReadAsync(entity, curSession.Identity!).ConfigureAwait(false);

            if (canBeCreated == false)
                throw new Modules.Exceptions.AccessRuleException(Error.AccessRuleViolationCanNotRead);
        }
        protected virtual async Task CheckCanBeChangedAsync(IIdentifyable entity)
        {
            var curSession = await Modules.Account.AccountManager.QueryAliveSessionAsync(SessionToken).ConfigureAwait(false);

            if (curSession == null)
                throw new Modules.Exceptions.AuthorizationException(Error.InvalidSessionToken);

            var canBeCreated = await AccessRulesController.CanBeChangedAsync(entity, curSession.Identity!).ConfigureAwait(false);

            if (canBeCreated == false)
                throw new Modules.Exceptions.AccessRuleException(Error.AccessRuleViolationCanNotChanged);
        }
        protected virtual async Task CheckCanBeDeletedAsync(IIdentifyable entity)
        {
            var curSession = await Modules.Account.AccountManager.QueryAliveSessionAsync(SessionToken).ConfigureAwait(false);

            if (curSession == null)
                throw new Modules.Exceptions.AuthorizationException(Error.InvalidSessionToken);

            var canBeCreated = await AccessRulesController.CanBeDeletedAsync(entity, curSession.Identity!).ConfigureAwait(false);

            if (canBeCreated == false)
                throw new Modules.Exceptions.AccessRuleException(Error.AccessRuleViolationCanNotDeleted);
        }

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
