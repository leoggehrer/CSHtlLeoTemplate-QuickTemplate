//@CodeCopy
//MdStart
#if ACCOUNT_ON && REVISION_ON
namespace QuickTemplate.Logic.Entities.Revision
{
    /// <summary>
    /// Represents a history in the account system.
    /// </summary>
#if SQLITE_ON
    [System.ComponentModel.DataAnnotations.Schema.Table("Histories")]
#else
    [System.ComponentModel.DataAnnotations.Schema.Table("Histories", Schema = "revision")]
#endif
    internal partial class History : EntityObject
    {
        /// <summary>
        /// Gets or sets the identity ID of an object.
        /// </summary>
        public IdType IdentityId { get; set; }
        /// <summary>
        /// Gets or sets the type of action.
        /// </summary>
        /// <remarks>
        /// This property indicates the type of action being performed.
        /// It can be used to specify the action type in various scenarios.
        /// The default value is an empty string.
        /// </remarks>
        public string ActionType { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the action time.
        /// </summary>
        public DateTime ActionTime { get; set; }
        /// <summary>
        /// Gets or sets the subject name.
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string SubjectName { get; set; } = String.Empty;
        /// <summary>
        /// Gets or sets the subject ID.
        /// </summary>
        /// <value>The subject ID.</value>
        public IdType SubjectId { get; set; }
        /// <summary>
        /// Gets or sets the JSON data.
        /// </summary>
        public string JsonData { get; set; } = String.Empty;
    }
}
#endif
//MdEnd

