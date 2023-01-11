//@Ignore
namespace QuickTemplate.Logic.Entities.TestInternal
{
#if SQLITE_ON
    [Table("Companies")]
#else
    [Table("Companies", Schema = "testinternal")]
#endif
    [Index(nameof(Name), IsUnique = true)]
    internal class Company : VersionExtendedEntity
    {
        [MaxLength(128)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(256)]
        public string Address { get; set; } = string.Empty;

        #region Navigation properties
        [ForeignKey("CompanyId")]
        public List<CompanyXCustomer> CompanyXCustomers { get; set; } = new();
        #endregion Navigation properties
    }
}
