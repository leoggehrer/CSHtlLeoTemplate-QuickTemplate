//@Ignore
namespace QuickTemplate.Logic.Entities.TestPublic
{
#if SQLITE_ON
    [Table("Members")]
#else
    [Table("Members", Schema = "testpublic")]
#endif
    public class Member : VersionExtendedEntity
    {
        [MaxLength(128)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(256)]
        public string LastName { get; set; } = string.Empty;

        #region Navigation properties
        public List<Project> Projects { get; set; } = new();
        #endregion Navigation properties
    }
}
