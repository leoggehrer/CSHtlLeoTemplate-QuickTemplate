//@Ignore
using QuickTemplate.Logic.Modules.Common;

namespace QuickTemplate.Logic.Entities.TestPublic
{
#if SQLITE_ON
    [Table("Projects")]
#else
    [Table("Projects", Schema = "testpublic")]
#endif
    [Index(nameof(Name), IsUnique = true)]
    public class Project : VersionExtendedEntity
    {
        [MaxLength(128)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(256)]
        public string? Description { get; set; }

        public State State { get; set; }

        #region Navigation properties
        public List<Member> Members { get; set; } = new();
        #endregion Navigation properties
    }
}
