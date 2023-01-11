//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Models.Account
{
    using TEntity = Entities.Account.LoginSession;
    using TModel = Models.Account.LoginSession;
    public partial class LoginSession : VersionModel
    {
        public IdType IdentityId { get; set; }
        public string SessionToken { get; set; } = string.Empty;
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? OptionalInfo { get; set; }
        public Role[] Roles { get; set; } = Array.Empty<Role>();

        internal static LoginSession Create(TEntity other)
        {
            BeforeCreate(other);
            var result = new LoginSession();

            result.CopyFrom(other);
            result.Roles = other.Roles.Select(r => Role.Create(r)).ToArray();
            AfterCreate(result, other);
            return result;
        }
        static partial void BeforeCreate(TEntity other);
        static partial void AfterCreate(TModel instance, TEntity other);
    }
}
#endif
//MdEnd
