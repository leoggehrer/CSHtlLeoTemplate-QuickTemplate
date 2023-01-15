//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Contracts.Account
{
    using QuickTemplate.Logic.Entities.Account;
    public partial interface IIdentityXRole
    {
        IdType IdentityId { get; set; }
        IdType RoleId { get; set; }
        Role? Role { get; set; }
    }
}
#endif
//MdEnd
