//@BaseCode
//MdStart

using CommonBase.Contracts;

namespace QuickTemplate.Logic.ServiceModels
{
    /// <summary>
    /// Represents a version model that is used for versionable entities.
    /// </summary>
    public abstract partial class VersionModel : ServiceModel, IVersionable
    {
        /// <summary>
        /// Row version of the entity.
        /// </summary>
        public byte[]? RowVersion { get; set; }
    }
}
//MdEnd
