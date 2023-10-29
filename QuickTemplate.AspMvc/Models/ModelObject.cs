//@BaseCode
//MdStart
namespace QuickTemplate.AspMvc.Models
{
    /// <summary>
    /// Represents an abstract base class for model objects.
    /// </summary>
    /// <remarks>
    /// This class inherits from the <see cref="BaseModels.ModelObject"/> class.
    /// </remarks>
    public abstract partial class ModelObject : BaseModels.ModelObject
    {
        /// <summary>
        /// Indicates whether the id has a default value.
        /// </summary>
        [ScaffoldColumn(false)]
        public bool IsIdDefault => Id == default;
    }
}
//MdEnd
