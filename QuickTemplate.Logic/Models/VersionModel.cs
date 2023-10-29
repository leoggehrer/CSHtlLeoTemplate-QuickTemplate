//@BaseCode
//MdStart
namespace QuickTemplate.Logic.Models
{
    /// <summary>
    /// Represents an abstract class that serves as the base class for versionable models.
    /// </summary>
    public abstract partial class VersionModel : ModelObject, BaseContracts.IVersionable
    {
        ///<summary>
        ///Gets or sets the source entity for the current version entity.
        ///</summary>
        ///<value>
        ///The source entity of type <see cref="Entities.VersionEntity"/>.
        ///</value>
        ///<remarks>
        ///This property represents the source entity associated with the current version entity.
        ///</remarks>
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
        
        /// <summary>
        /// Overrides the GetHashCode method in the base class.
        /// Calculates the hash code of the specified list of objects.
        /// </summary>
        /// <param name="values">The list of objects to calculate the hash code from.</param>
        /// <returns>The hash code calculated from the specified list of objects.</returns>
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
