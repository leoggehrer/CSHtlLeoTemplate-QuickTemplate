//@BaseCode
//MdStart
namespace QuickTemplate.AspMvc.Models.View
{
    /// <summary>
    /// Represents a filter model.
    /// </summary>
    public partial interface IFilterModel
    {
        ///<summary>
        /// Gets or sets a value indicating whether the item should be displayed.
        ///</summary>
        /// <remarks>
        /// This property determines if the item should be shown or hidden. If the value is true, the item will be
        /// displayed. If the value is false, the item will be hidden.
        ///</remarks>
        /// <returns>
        /// True if the item should be displayed; otherwise, false.
        /// </returns>
        bool Show { get; }
        /// <summary>
        /// Gets a value indicating whether the entity has a value.
        /// </summary>
        /// <returns>True if the entity has a value; otherwise, false.</returns>
        bool HasEntityValue { get; }
        /// <summary>
        /// Creates an entity predicate.
        /// </summary>
        /// <returns>The entity predicate as a string.</returns>
        string CreateEntityPredicate();
    }
}
//MdEnd
