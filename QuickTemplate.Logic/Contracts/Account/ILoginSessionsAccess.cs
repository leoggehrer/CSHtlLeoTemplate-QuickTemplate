//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Contracts.Account
{
    using CommonBase.Contracts;
    using TOutModel = Models.Account.LoginSession;

    public partial interface ILoginSessionsAccess : IDataAccess<TOutModel>
    {
    }
}
#endif
//MdEnd
