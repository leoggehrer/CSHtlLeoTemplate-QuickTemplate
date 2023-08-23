//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Contracts.Account
{
    using CommonBase.Contracts;
    using TOutModel = Models.Account.User;

    public partial interface IUsersAccess : IDataAccess<TOutModel>
    {
    }
}
#endif
//MdEnd
