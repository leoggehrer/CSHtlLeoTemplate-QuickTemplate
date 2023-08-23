//@CodeCopy
//MdStart
using CommonBase.Contracts;

namespace CommonBase.Models
{
    public abstract partial class VersionModel : ModelObject, IVersionable
    {
#if ROWVERSION_ON
        /// <summary>
        /// Row version of the entity.
        /// </summary>
        public virtual byte[]? RowVersion
        {
            get => Source?.RowVersion ?? Array.Empty<byte>();
            set
            {
                if (Source != null)
                {
                    Source.RowVersion = value;
                }
            }
        }
#endif

        protected override int GetHashCode(List<object?> values)
        {
#if ROWVERSION_ON
            if (RowVersion != null)
            {
                values.Add(RowVersion);
            }
#endif
            return base.GetHashCode(values);
        }
    }
}
//MdEnd
