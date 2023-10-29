//@BaseCode
//MdStart
namespace QuickTemplate.Logic.Entities
{
    using CommonBase.Contracts;
#if ACCOUNT_ON
    using QuickTemplate.Logic.Entities.Account;
#endif
    /// <summary>
    /// Represents an abstract partial class that extends the VersionEntity class and implements the IVersionableExtendedProperties interface.
    /// </summary>
    public abstract partial class VersionExtendedEntity : VersionEntity, IVersionableExtendedProperties
    {
#if GUID_ON
        /// <summary>
        /// Gets or sets the Guid.
        /// </summary>
        public Guid Guid { get; internal set; } = Guid.NewGuid();
#endif
        
#if CREATED_ON
        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        public DateTime CreatedOn { get; internal set; }
#endif
#if ACCOUNT_ON && CREATEDBY_ON
        /// <summary>
        /// Gets or sets the owner (Identity) reference.
        /// </summary>
        [Column("CreatedById")]
        public IdType? IdentityId_CreatedBy { get; internal set; }
#endif
        
#if MODIFIED_ON
        /// <summary>
        /// Gets or sets the last modified time.
        /// </summary>
        public DateTime? ModifiedOn { get; internal set; }
#endif
#if ACCOUNT_ON && MODIFIEDBY_ON
        /// <summary>
        /// Gets or sets the reference of the user (Identity) who made the last change.
        /// </summary>
        [Column("ModifiedById")]
        public IdType? IdentityId_ModifiedBy { get; internal set; }
#endif
        
        #region Navigation properties
#if ACCOUNT_ON && CREATEDBY_ON
        /// <summary>
        /// Gets or sets the <see cref="Identity"/> object representing the user who created the entity.
        /// </summary>
        /// <remarks>
        /// This property is marked with the <see cref="ForeignKeyAttribute"/> indicating that it references the <see cref="IdentityId_CreatedBy"/> property
        /// in the parent table.
        /// </remarks>
        /// <value>
        /// The <see cref="Identity"/> object representing the user who created the entity.
        /// </value>
        [ForeignKey(nameof(IdentityId_CreatedBy))]
        public Identity? CreatedBy { get; internal set; }
#endif
        
#if ACCOUNT_ON && MODIFIEDBY_ON
        /// <summary>
        /// Gets or sets the identity of the user who modified the object.
        /// </summary>
        /// <value>
        /// An <see cref="Identity"/> representing the user who modified the object.
        /// </value>
        [ForeignKey(nameof(IdentityId_ModifiedBy))]
        public Identity? ModifiedBy { get; internal set; }
#endif
        #endregion Navigation properties
    }
}
//MdEnd


