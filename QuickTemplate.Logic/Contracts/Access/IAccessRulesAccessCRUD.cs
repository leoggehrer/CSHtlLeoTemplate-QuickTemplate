//@CodeCopy
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
using CommonBase.Contracts;

namespace QuickTemplate.Logic.Contracts.Access
{
    public partial interface IAccessRulesAccess
    {
        Task<bool> CanBeCreatedAsync(Type type, Account.IIdentity identity);
        Task<bool> CanBeReadAsync(IIdentifyable item, Account.IIdentity identity);
        Task<bool> CanBeChangedAsync(IIdentifyable item, Account.IIdentity identity);
        Task<bool> CanBeDeletedAsync(IIdentifyable item, Account.IIdentity identity);
    }
}
#endif
//MdEnd
