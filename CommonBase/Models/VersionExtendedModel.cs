//@CodeCopy
//MdStart
using CommonBase.Contracts;

namespace CommonBase.Models
{
    /// <summary>
    /// Represents an abstract partial class that extends the VersionModel class and implements the IVersionableExtendedProperties interface.
    /// </summary>
    public abstract partial class VersionExtendedModel : VersionModel, IVersionableExtendedProperties
    {
#if GUID_ON
        /// <summary>
        /// Gets or sets the Guid value of the model.
        /// </summary>
        public Guid Guid { get; set; }
#endif
#if CREATED_ON
        /// <summary>
        /// Gets or sets the CreatedOn value of the model.
        /// </summary>
        public System.DateTime CreatedOn
        {
            get => Source != null ? Source.CreatedOn : DateTime.UtcNow;
            internal set
            {
                if (Source != null)
                {
                    Source.CreatedOn = value;
                }
            }
        }
#endif
#if ACCOUNT_ON && CREATEDBY_ON
        /// <summary>
        /// Gets or sets the IdentityId_CreatedBy value of the model.
        /// </summary>
        public IdType? IdentityId_CreatedBy
        {
            get => Source?.IdentityId_CreatedBy;
            internal set
            {
                if (Source != null)
                {
                    Source.IdentityId_CreatedBy = value;
                }
            }
        }
#endif

#if MODIFIED_ON
        /// <summary>
        /// Gets or sets the ModifiedOn value of the model.
        /// </summary>
        public System.DateTime? ModifiedOn
        {
            get => Source?.ModifiedOn;
            internal set
            {
                if (Source != null)
                {
                    Source.ModifiedOn = value;
                }
            }
        }
#endif

#if ACCOUNT_ON && MODIFIEDBY_ON
        /// <summary>
        /// Gets or sets the IdentityId_ModifiedBy value of the model.
        /// </summary>
        public IdType? IdentityId_ModifiedBy
        {
            get => Source?.IdentityId_ModifiedBy;
            internal set
            {
                if (Source != null)
                {
                    Source.IdentityId_ModifiedBy = value;
                }
            }
        }
#endif
        /// <summary>
        /// Copies extended properties from the specified source to the current instance.
        /// </summary>
        /// <param name="source">The source object from which to copy the extended properties.</param>
        /// <remarks>
        /// The extended properties that are copied include:
        /// - CreatedOn: Only if the 'CREATED_ON' symbol is defined.
        /// - IdentityId_CreatedBy: Only if the 'ACCOUNT_ON' and 'CREATEDBY_ON' symbols are defined.
        /// - ModifiedOn: Only if the 'MODIFIED_ON' symbol is defined.
        /// - IdentityId_ModifiedBy: Only if the 'ACCOUNT_ON' and 'MODIFIEDBY_ON' symbols are defined.
        /// </remarks>
        protected void CopyExendedProperties(IVersionableExtendedProperties source)
        {
#if CREATED_ON
            CreatedOn = source.CreatedOn;
#endif
#if ACCOUNT_ON && CREATEDBY_ON
            IdentityId_CreatedBy = source.IdentityId_CreatedBy;
#endif

#if MODIFIED_ON
            ModifiedOn = source.ModifiedOn;
#endif
#if ACCOUNT_ON && MODIFIEDBY_ON
            IdentityId_ModifiedBy = source.IdentityId_ModifiedBy;
#endif
        }

        /// <summary>
        /// Computes and returns the hash code for the specified objects.
        /// </summary>
        /// <param name="items">An array of objects to compute the hash code for.</param>
        /// <returns>The computed hash code.</returns>
        protected int GetHashCode(params object?[] items)
        {
            return GetHashCode(items.ToList());
        }
        /// <summary>
        /// Computes the hash code for a list of object values.
        /// </summary>
        /// <param name="values">The list of object values.</param>
        /// <returns>
        /// A hash code value for the list of object values.
        /// </returns>
        protected override int GetHashCode(List<object?> values)
        {
#if GUID_ON
            values.Add(Guid);
#endif

#if CREATED_ON
            values.Add(CreatedOn);
#endif
#if ACCOUNT_ON && CREATEDBY_ON
            if (IdentityId_CreatedBy != null)
            values.Add(IdentityId_CreatedBy);
#endif

#if MODIFIED_ON
            if (ModifiedOn != null)
            values.Add(ModifiedOn);
#endif
#if ACCOUNT_ON && MODIFIEDBY_ON
            if (IdentityId_ModifiedBy != null)
            values.Add(IdentityId_ModifiedBy);
#endif
            return base.GetHashCode(values);
        }
    }
}
//MdEnd


