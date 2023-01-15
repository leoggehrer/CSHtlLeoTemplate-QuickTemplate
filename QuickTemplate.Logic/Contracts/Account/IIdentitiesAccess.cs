//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Contracts.Account
{
    public partial interface IIdentitiesAccess<T> : IDataAccess<T>
    {
        public Task AddRoleAsync(IdType id, IdType roleId);
        public Task RemoveRoleAsync(IdType id, IdType roleId);
    }
}
#endif
//MdEnd
