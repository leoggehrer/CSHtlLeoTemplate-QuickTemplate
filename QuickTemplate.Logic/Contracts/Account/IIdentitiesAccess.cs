//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Contracts.Account
{
    using TOutModel = Models.Account.Identity;

    public partial interface IIdentitiesAccess : IDataAccess<TOutModel>
    {
        public Task AddRoleAsync(IdType id, IdType roleId);
        public Task RemoveRoleAsync(IdType id, IdType roleId);
    }
}
#endif
//MdEnd