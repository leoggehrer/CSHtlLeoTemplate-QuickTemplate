//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Entities.Account
{
    using QuickTemplate.Logic.Contracts.Account;
#if SQLITE_ON
    [System.ComponentModel.DataAnnotations.Schema.Table("Roles")]
#else
    [System.ComponentModel.DataAnnotations.Schema.Table("Roles", Schema = "account")]
#endif
    [Microsoft.EntityFrameworkCore.Index(nameof(Designation), IsUnique = true)]
    public partial class Role : VersionExtendedEntity, IRole
    {
#if GUID_OFF
        public Guid Guid { get; internal set; }
#endif
        [MaxLength(64)]
        public string Designation { get; set; } = string.Empty;
        [MaxLength(256)]
        public string? Description { get; set; }
    }
}
#endif
//MdEnd
