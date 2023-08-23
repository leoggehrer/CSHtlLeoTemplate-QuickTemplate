//@CodeCopy
//MdStart
#if ACCOUNT_ON && REVISION_ON
namespace QuickTemplate.Logic.Entities.Revision
{
#if SQLITE_ON
    [System.ComponentModel.DataAnnotations.Schema.Table("Histories")]
#else
    [System.ComponentModel.DataAnnotations.Schema.Table("Histories", Schema = "revision")]
#endif
    internal partial class History : EntityObject
    {
        public IdType IdentityId { get; set; }
        public string ActionType { get; set; } = string.Empty;
        public DateTime ActionTime { get; set; }
        [Required]
        [MaxLength(128)]
        public string SubjectName { get; set; } = String.Empty;
        public IdType SubjectId { get; set; }
        public string JsonData { get; set; } = String.Empty;
    }
}
#endif
//MdEnd
