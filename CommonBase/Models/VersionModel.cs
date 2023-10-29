//@CodeCopy
//MdStart
using CommonBase.Contracts;

namespace CommonBase.Models
{
    /// <summary>
    /// Represents an abstract partial class for version management.
    /// </summary>
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

        /// <summary>
        /// Computes the hash code for the specified list of objects.
        /// </summary>
        /// <param name="values">A list of objects.</param>
        /// <returns>The computed hash code.</returns>
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


