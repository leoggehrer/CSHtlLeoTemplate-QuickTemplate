//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Entities.Account
{
#if SQLITE_ON
    [System.ComponentModel.DataAnnotations.Schema.Table("SecureIdentities")]
#else
    [System.ComponentModel.DataAnnotations.Schema.Table("SecureIdentities", Schema = "account")]
#endif
    [Microsoft.EntityFrameworkCore.Index(nameof(Email), IsUnique = true)]
    internal partial class SecureIdentity : Identity
    {
        [Required]
        [MaxLength(512)]
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        [Required]
        [MaxLength(512)]
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

#region Transient properties
        [NotMapped]
        public string Password { get; set; } = string.Empty;
#endregion Transient properties
    }
}
#endif
//MdEnd
