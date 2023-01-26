//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Entities.Account
{
    using QuickTemplate.Logic.Contracts.Account;
    using QuickTemplate.Logic.Modules.Common;

#if SQLITE_ON
    [Table("Identities")]
#else
    [Table("Identities", Schema = "account")]
#endif
    [Index(nameof(Email), IsUnique = true)]
    public abstract partial class Identity : VersionExtendedEntity, IIdentity
    {
        public Guid Guid { get; internal set; }
        [MaxLength(128)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(128)]
        public string Email { get; set; } = string.Empty;
        public int TimeOutInMinutes { get; set; } = 30;
        public bool EnableJwtAuth { get; set; }
        public int AccessFailedCount { get; set; }
        public State State { get; set; } = State.Active;
        public IRole[] Roles => IdentityXRoles.Where(iXr => iXr.Role != null)
                                              .Select(iXr => iXr.Role!)
                                              .ToArray();
        // Navigation properties
        public List<IdentityXRole> IdentityXRoles { get; internal set; } = new();

        public bool HasRole(Guid guid)
        {
            return IdentityXRoles.Any(iXr => iXr.Role != null && iXr.Role.Guid == guid);
        }
    }
}
#endif
//MdEnd
