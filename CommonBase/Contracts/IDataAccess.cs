//@CodeCopy
//MdStart

namespace CommonBase.Contracts
{
    /// <summary>
    /// Generic interface for data access.
    /// </summary>
    /// <typeparam name="T">The generic type.</typeparam>
    public partial interface IDataAccess<T> : IBaseAccess<T>
    {
        /// <summary>
        /// Returns the number of quantity in the collection based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>Number of entities in the collection.</returns>
        Task<int> CountAsync(string predicate, string[] includeItems);

        /// <summary>
        /// Returns the element of type T with the identification of id.
        /// </summary>
        /// <param name="id">The identification.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>The element of the type T with the corresponding identification (with includes).</returns>
        Task<T?> GetByIdAsync(IdType id, string[] includeItems);
        /// <summary>
        /// Returns all interfaces of the elements in the collection.
        /// </summary>
        /// <param name="includeItems">The include items</param>
        /// <returns>All interfaces of the element collection.</returns>
        Task<T[]> GetAllAsync(string[] includeItems);
        /// <summary>
        /// Returns all elements in the collection.
        /// </summary>
        /// <param name="orderBy">Sorts the elements of a sequence according to a sort clause.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>All interfaces of the element collection.</returns>
        Task<T[]> GetAllAsync(string orderBy, string[] includeItems);
        /// <summary>
        /// Gets a subset of items from the repository.
        /// </summary>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        Task<T[]> GetPageListAsync(int pageIndex, int pageSize, string[] includeItems);
        /// <summary>
        /// Gets a subset of items from the repository.
        /// </summary>
        /// <param name="orderBy">Sorts the elements of a sequence according to a sort clause.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        Task<T[]> GetPageListAsync(string orderBy, int pageIndex, int pageSize, string[] includeItems);

        /// <summary>
        /// Filters a sequence of elements based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>The filter result.</returns>
        Task<T[]> QueryAsync(string predicate, string[] includeItems);
        /// <summary>
        /// Filters a subset of elements based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        Task<T[]> QueryAsync(string predicate, int pageIndex, int pageSize, string[] includeItems);
        /// <summary>
        /// Filters a subset of elements based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="orderBy">Sorts the elements of a sequence in order according to a key.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        Task<T[]> QueryAsync(string predicate, string orderBy, int pageIndex, int pageSize, string[] includeItems);

        /// <summary>
        /// The elements are being tracked by the context but does not yet exist in the repository. 
        /// </summary>
        /// <param name="elements">The elements which are to be inserted.</param>
        /// <returns>The inserted elements.</returns>
        Task<IEnumerable<T>> InsertAsync(IEnumerable<T> elements);

        /// <summary>
        /// The elements are being tracked by the context and exists in the repository, and some or all of its property values have been modified.
        /// </summary>
        /// <param name="elements">The elements which are to be updated.</param>
        /// <returns>The updated elements.</returns>
        Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> elements);
    }
}
//MdEnd
