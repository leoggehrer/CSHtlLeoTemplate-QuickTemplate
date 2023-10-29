//@BaseCode
//MdStart
using CommonBase.Contracts;

namespace QuickTemplate.Logic.Entities
{
    /// <summary>
    /// Represents an abstract base class for entities that can be versioned.
    /// </summary>
    /// <summary>
    /// Gets or sets the row version of the entity.
    /// </summary>
    public abstract partial class VersionEntity : EntityObject, IVersionable
    {
#if ROWVERSION_ON
        /// <summary>
        /// Row version of the entity.
        /// </summary>
        [Timestamp]
        public byte[]? RowVersion { get; internal set; }
#endif
    }
}
//MdEnd
