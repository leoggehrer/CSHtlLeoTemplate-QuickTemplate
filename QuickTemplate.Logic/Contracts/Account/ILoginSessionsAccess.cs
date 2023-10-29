//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Contracts.Account
{
    using CommonBase.Contracts;
    using TOutModel = Models.Account.LoginSession;
    
    /// <summary>
    /// Represents an interface for accessing login sessions.
    /// </summary>
    /// <typeparam name="TOutModel">The type of the output model.</typeparam>
    public partial interface ILoginSessionsAccess : IDataAccess<TOutModel>
    {
    }
}
#endif
//MdEnd
