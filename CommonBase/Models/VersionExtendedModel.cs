//@CodeCopy
//MdStart
using CommonBase.Contracts;

namespace CommonBase.Models
{
    public abstract partial class VersionExtendedModel : VersionModel, IVersionableExtendedProperties
    {
#if GUID_ON
        public Guid Guid { get; set; }
#endif
#if CREATED_ON
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

        protected int GetHashCode(params object?[] items)
        {
            return GetHashCode(items.ToList());
        }
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
