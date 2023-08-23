//@CodeCopy
//MdStart
namespace CommonBase.Contracts
{
    partial interface IBaseAccess<T>
    {
#if GUID_ON
        /// <summary>
        /// Returns the element of type T with the identification of id.
        /// </summary>
        /// <param name="id">The identification.</param>
        /// <returns>The element of the type T with the corresponding identification.</returns>
        Task<T?> GetByGuidAsync(Guid id);
#endif
    }
}
//MdEnd
