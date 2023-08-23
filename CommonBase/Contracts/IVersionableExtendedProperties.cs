//@CodeCopy
//MdStart

namespace CommonBase.Contracts
{
    public partial interface IVersionableExtendedProperties : IVersionable
    {
#if GUID_ON
        /// <summary>
        /// Gets the Guid.
        /// </summary>
        Guid Guid { get; }
#endif

#if CREATED_ON
        /// <summary>
        /// Gets the creation time.
        /// </summary>
        System.DateTime CreatedOn { get; }
#endif
#if ACCOUNT_ON && MODIFIEDBY_ON
        /// <summary>
        /// Gets the owner (Identity) reference.
        /// </summary>
        IdType? IdentityId_CreatedBy { get; }
#endif

#if MODIFIED_ON
        /// <summary>
        /// Gets the last modified time.
        /// </summary>
        System.DateTime? ModifiedOn { get; }
#endif
#if ACCOUNT_ON && MODIFIEDBY_ON
        /// <summary>
        /// Gets the reference of the user (Identity) who made the last change.
        /// </summary>
        IdType? IdentityId_ModifiedBy { get; }
#endif
    }
}
//MdEnd
