//@BaseCode
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
namespace QuickTemplate.Logic.Entities.Access
{
    using QuickTemplate.Logic.Modules.Access;
    /// <summary>
    /// Represents an access rule in the account system.
    /// </summary>
#if SQLITE_ON
    [System.ComponentModel.DataAnnotations.Schema.Table("AccessRules")]
#else
    [System.ComponentModel.DataAnnotations.Schema.Table("AccessRules", Schema = "access")]
#endif
    [Microsoft.EntityFrameworkCore.Index(nameof(EntityType), IsUnique = false)]
    internal partial class AccessRule : VersionExtendedEntity
    {
        ///<summary>
        /// Gets or sets the type of the rule.
        ///</summary>
        ///<value>The type of the rule.</value>
        public RuleType Type { get; set; }
        /// <summary>
        /// Gets or sets the EntityType.
        /// </summary>
        /// <remarks>
        /// The maximum length of the EntityType is 128 characters.
        /// </remarks>
        [MaxLength(128)]
        public string EntityType { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the RelationshipEntityType.
        /// </summary>
        /// <remarks>
        /// Maximum length of 128 characters.
        /// </remarks>
        [MaxLength(128)]
        public string? RelationshipEntityType { get; set; }
        /// <summary>
        /// Gets or sets the PropertyName.
        /// </summary>
        /// <remarks>
        /// The maximum length for the PropertyName is 128 characters.
        /// </remarks>
        /// <value>
        /// The PropertyName.
        /// </value>
        [MaxLength(128)]
        public string? PropertyName { get; set; }
        /// <summary>
        /// Gets or sets the entity value.
        /// </summary>
        /// <remarks>
        /// The maximum length for the entity value is 36.
        /// </remarks>
        [MaxLength(36)]
        public string? EntityValue { get; set; }
        /// <summary>
        /// Gets or sets the access type.
        /// </summary>
        /// <value>
        /// The access type.
        /// </value>
        public AccessType AccessType { get; set; }
        /// <summary>
        /// Gets or sets the access role type.
        /// </summary>
        /// <remarks>
        /// The maximum length for the access role type is 128 characters.
        /// </remarks>
        /// <value>
        /// The access role type.
        /// </value>
        [MaxLength(128)]
        public string? AccessRoleType { get; set; }
        /// <summary>
        /// Gets or sets the access value.
        /// </summary>
        /// <value>The access value.</value>
        /// <remarks>This property is limited to a maximum length of 36 characters.</remarks>
        [MaxLength(36)]
        public string? AccessValue { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this object is creatable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the object is creatable; otherwise, <c>false</c>.
        /// </value>
        public bool Creatable { get; set; } = true;
        /// <summary>
        /// Gets or sets a value indicating whether the property is readable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the property is readable; otherwise, <c>false</c>.
        /// </value>
        public bool Readable { get; set; } = true;
        /// <summary>
        /// Gets or sets a value indicating whether the object is updatable.
        /// </summary>
        /// <value>
        /// true if the object is updatable; otherwise, false.
        /// </value>
        public bool Updatable { get; set; } = true;
        /// <summary>
        /// Gets or sets a value indicating whether the object is deletable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the object is deletable; otherwise, <c>false</c>.
        /// </value>
        public bool Deletable { get; set; } = true;
        /// <summary>
        /// Gets or sets a value indicating whether the object is viewable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the object is viewable; otherwise, <c>false</c>.
        /// </value>
        public bool Viewable { get; set; } = true;
    }
}
#endif
//MdEnd


