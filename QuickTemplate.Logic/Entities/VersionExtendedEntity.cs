//@BaseCode
//MdStart
namespace QuickTemplate.Logic.Entities
{
    using CommonBase.Contracts;
#if ACCOUNT_ON
    using QuickTemplate.Logic.Entities.Account;
#endif
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
        [ForeignKey(nameof(IdentityId_CreatedBy))]
        public Identity? CreatedBy { get; internal set; }
#endif

#if ACCOUNT_ON && MODIFIEDBY_ON
        [ForeignKey(nameof(IdentityId_ModifiedBy))]
        public Identity? ModifiedBy { get; internal set; }
#endif
        #endregion Navigation properties
    }
}
//MdEnd
