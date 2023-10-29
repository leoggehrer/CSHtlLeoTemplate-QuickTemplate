//@BaseCode
//MdStart
namespace QuickTemplate.Logic.Models
{
    /// <summary>
    /// Represents an abstract partial class that extends the VersionModel class and implements the IVersionableExtendedProperties interface.
    /// </summary>
    public abstract partial class VersionExtendedModel : VersionModel, BaseContracts.IVersionableExtendedProperties
    {
        /// <summary>
        /// Gets or sets the Source of this object.
        /// </summary>
        /// <value>The Source object of type VersionExtendedEntity.</value>
        new internal virtual Entities.VersionExtendedEntity Source
        {
            get => (Entities.VersionExtendedEntity)_source!;
            set => _source = value;
        }
#if GUID_ON
        /// <summary>
        /// Gets or sets the unique identifier of the object.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        public Guid Guid { get; set; }
#endif
#if CREATED_ON
        /// <summary>
        /// Gets or sets the date and time when the object was created.
        /// </summary>
        /// <remarks>
        /// If the source object is not null, returns the value of the created date and time from the source object.
        /// If the source object is null, returns the current date and time in Coordinated Universal Time (UTC).
        /// The set method updates the created date and time in the source object if it is not null.
        /// </remarks>
        /// <value>
        /// The date and time when the object was created.
        /// </value>
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
        /// Gets or sets the identity id created by.
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
        /// Gets or sets the modified date and time.
        /// </summary>
        /// <summary>
        /// Gets or sets the modified date and time.
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
        /// Gets or sets the modified-by identity identifier.
        /// </summary>
        /// <value>The modified-by identity identifier.</value>
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
        /// Copies extended properties from the source object to the current object.
        /// </summary>
        /// <param name="source">The source object that contains the extended properties.</param>
        protected static void CopyExendedProperties(BaseContracts.IVersionableExtendedProperties source)
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
        /// Retrieves the hash code for the specified list of objects.
        /// </summary>
        /// <param name="items">The list of objects to calculate the hash code for.</param>
        /// <returns>The hash code calculated based on the list of objects.</returns>
        protected int GetHashCode(params object?[] items)
        {
            return GetHashCode(items.ToList());
        }
        /// <summary>
        /// Overrides the GetHashCode method to calculate the hash code for the specified list of objects.
        /// </summary>
        /// <param name="values">The list of objects for which to calculate the hash code.</param>
        /// <returns>The hash code for the specified list of objects.</returns>
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

