//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Contracts.Account
{
    using CommonBase.Contracts;
    using TOutModel = Models.Account.User;
    
    /// <summary>
    /// Represents an interface for accessing users data.
    /// </summary>
    /// <typeparam name="TOutModel">The type of the output model.</typeparam>
    public partial interface IUsersAccess : IDataAccess<TOutModel>
    {
    }
}
#endif
//MdEnd
