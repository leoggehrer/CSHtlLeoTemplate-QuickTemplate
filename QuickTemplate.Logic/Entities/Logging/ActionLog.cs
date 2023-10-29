//@BaseCode
//MdStart
#if ACCOUNT_ON && LOGGING_ON
namespace QuickTemplate.Logic.Entities.Logging
{
    /// <summary>
    /// Represents an action log in the account system.
    /// </summary>
#if SQLITE_ON
    [System.ComponentModel.DataAnnotations.Schema.Table("ActionLogs")]
#else
    [System.ComponentModel.DataAnnotations.Schema.Table("ActionLogs", Schema = "logging")]
#endif
    internal partial class ActionLog : EntityObject
    {
        /// <summary>
        /// Gets or sets the identity ID.
        /// </summary>
        public IdType IdentityId { get; internal set; }
        /// <summary>
        /// Gets or sets the time value.
        /// </summary>
        public DateTime Time { get; internal set; }
        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Subject { get; internal set; } = string.Empty;
        /// <summary>
        /// Gets or sets the Action.
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string Action { get; internal set; } = string.Empty;
        /// <summary>
        /// Gets or sets the information.
        /// </summary>
        [Required]
        public string Info { get; internal set; } = string.Empty;
    }
}
#endif
//MdEnd
