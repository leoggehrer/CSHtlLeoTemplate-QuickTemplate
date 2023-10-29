//@BaseCode
//MdStart

namespace CommonBase.Contracts
{
    /// <summary>
    /// An interface representing identifyable entities.
    /// </summary>
    public partial interface IIdentifyable
    {
        /// <summary>
        /// Gets the ID of the object.
        /// </summary>
        /// <returns>The ID.</returns>
        /// <remarks>
        /// This property gets the unique identifier of the object.
        /// </remarks>
        IdType Id { get; }
    }
}
//MdEnd


