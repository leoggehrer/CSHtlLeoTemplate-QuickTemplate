//@Ignore
namespace QuickTemplate.Logic.Entities.TestInternal
{
#if SQLITE_ON
    [Table("Customers")]
#else
    [Table("Customers", Schema = "testinternal")]
#endif
    internal class Customer : VersionExtendedEntity
    {
        [MaxLength(128)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(256)]
        public string LastName { get; set; } = string.Empty;

        #region Navigation properties
        [ForeignKey("CustomerId")]
        public List<CompanyXCustomer> CompanyXCustomers { get; set; } = new();
        #endregion Navigation properties
    }
}
