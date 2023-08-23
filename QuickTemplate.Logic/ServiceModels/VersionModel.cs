//@CodeCopy
//MdStart

using CommonBase.Contracts;

namespace QuickTemplate.Logic.ServiceModels
{
    public abstract partial class VersionModel : ServiceModel, IVersionable
    {
        /// <summary>
        /// Row version of the entity.
        /// </summary>
        public byte[]? RowVersion { get; set; }
    }
}
//MdEnd
