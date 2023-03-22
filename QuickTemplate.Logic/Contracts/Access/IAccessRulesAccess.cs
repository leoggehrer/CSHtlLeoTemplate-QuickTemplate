//@BaseCode
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
namespace QuickTemplate.Logic.Contracts.Access
{
    using TOutModel = Models.Access.AccessRule;

    public partial interface IAccessRulesAccess : Contracts.IDataAccess<TOutModel>
    {
    }
}
#endif
//MdEnd