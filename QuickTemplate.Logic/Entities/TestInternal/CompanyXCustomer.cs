//@Ignore
namespace QuickTemplate.Logic.Entities.TestInternal
{
#if SQLITE_ON
    [Table("CompanyXCustomer")]
#else
    [Table("CompanyXCustomer", Schema = "testinternal")]
#endif
    [Index(nameof(CompanyId), nameof(CustomerId), IsUnique = true)]
    internal partial class CompanyXCustomer : VersionExtendedEntity
    {
        public IdType CompanyId { get; set; }
        public IdType CustomerId { get; set; }

        #region Navigation properties
        public Company? Company { get; set; }
        public Customer? Customer { get; set; }
        #endregion Navigation properties
    }
}
