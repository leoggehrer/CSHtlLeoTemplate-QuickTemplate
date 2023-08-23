//@CodeCopy
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
namespace QuickTemplate.Logic.Facades.Access
{
    using CommonBase.Contracts;
    using TOutModel = Models.Access.AccessRule;

    partial class AccessRulesFacade
    {
        new private Contracts.Access.IAccessRulesAccess Controller => (ControllerObject as Contracts.Access.IAccessRulesAccess)!;

        public Task<bool> CanBeCreatedAsync(Type type, Contracts.Account.IIdentity identity)
        {
            return Controller.CanBeCreatedAsync(type, identity);
        }
        public Task<bool> CanBeReadAsync(IIdentifyable model, Contracts.Account.IIdentity identity)
        {
            return Controller.CanBeReadAsync(model, identity);
        }
        public Task<bool> CanBeChangedAsync(IIdentifyable model, Contracts.Account.IIdentity identity)
        {
            return Controller.CanBeChangedAsync(model, identity);
        }
        public Task<bool> CanBeDeletedAsync(IIdentifyable model, Contracts.Account.IIdentity identity)
        {
            return Controller.CanBeDeletedAsync(model, identity);
        }
    }
}
#endif
//MdEnd
