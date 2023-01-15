//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.AspMvc.Models.Account
{
    public partial class IdentityRole : VersionModel
    {
#if !GUID_ON
        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        public Guid Guid { get; set; }
#endif
        public string Designation { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;

        public static IdentityRole Create(object source)
        {
            var result = new IdentityRole();

            result.CopyFrom(source);
            return result;
        }
    }
}
#endif
//MdEnd
