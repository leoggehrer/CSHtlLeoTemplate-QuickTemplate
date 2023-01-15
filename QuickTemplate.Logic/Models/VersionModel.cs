//@BaseCode
//MdStart
namespace QuickTemplate.Logic.Models
{
    using Logic.Contracts;
    public abstract partial class VersionModel : ModelObject, IVersionable
    {
        new internal virtual Entities.VersionEntity Source
        {
            get => (Entities.VersionEntity)_source!;
            set => _source = value;
        }
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
#if REVISION_ON
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
