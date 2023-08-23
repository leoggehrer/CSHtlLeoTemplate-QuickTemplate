//@CodeCopy
//MdStart

namespace CommonBase.Models.Accounts
{
    public partial class LogonSession
    {
        public IdType IdentityId { get; set; }
        public string SessionToken { get; set; } = string.Empty;
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? OptionalInfo { get; set; }
        public Role[] Roles { get; set; } = Array.Empty<Role>();
    }
}
//MdEnd
