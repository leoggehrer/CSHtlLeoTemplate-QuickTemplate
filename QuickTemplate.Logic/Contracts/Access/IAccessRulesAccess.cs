//@BaseCode
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
namespace QuickTemplate.Logic.Contracts.Access
{
    using CommonBase.Contracts;
    using TOutModel = Models.Access.AccessRule;
    
    /// <summary>
    /// Represents an interface for accessing access rules.
    /// </summary>
    /// <typeparam name="TOutModel">The output model type.</typeparam>
    public partial interface IAccessRulesAccess : IDataAccess<TOutModel>
    {
    }
}
#endif
//MdEnd
