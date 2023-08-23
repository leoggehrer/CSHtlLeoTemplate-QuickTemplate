//@CodeCopy
//MdStart
namespace CommonBase.Contracts
{
    /// <summary>
    /// Generic interface for data access.
    /// </summary>
    /// <typeparam name="T">The generic type.</typeparam>
    public partial interface IBaseAccess<T> : IDisposable
    {
        /// <summary>
        /// Gets the maximum page size.
        /// </summary>
        int MaxPageSize { get; }

        /// <summary>
        /// Creates a new element of type T.
        /// </summary>
        /// <returns>The new element.</returns>
        T Create();

        /// <summary>
        /// Gets the number of quantity in the collection.
        /// </summary>
        /// <returns>Number of elements in the collection.</returns>
        Task<int> CountAsync();
        /// <summary>
        /// Returns the number of quantity in the collection based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <returns>Number of entities in the collection.</returns>
        Task<int> CountAsync(string predicate);

        /// <summary>
        /// Returns the element of type T with the identification of id.
        /// </summary>
        /// <param name="id">The identification.</param>
        /// <returns>The element of the type T with the corresponding identification.</returns>
        Task<T?> GetByIdAsync(IdType id);
        /// <summary>
        /// Returns all objects of the elements in the collection.
        /// </summary>
        /// <returns>All objects of the element collection.</returns>
        Task<T[]> GetAllAsync();
        /// <summary>
        /// Returns all elements in the collection.
        /// </summary>
        /// <param name="orderBy">Sorts the elements of a sequence according to a sort clause.</param>
        /// <returns>All interfaces of the element collection.</returns>
        Task<T[]> GetAllAsync(string orderBy);
        /// <summary>
        /// Gets a subset of items from the repository.
        /// </summary>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        Task<T[]> GetPageListAsync(int pageIndex, int pageSize);
        /// <summary>
        /// Gets a subset of items from the repository.
        /// </summary>
        /// <param name="orderBy">Sorts the elements of a sequence in order according to a key.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        Task<T[]> GetPageListAsync(string orderBy, int pageIndex, int pageSize);

        /// <summary>
        /// Filters a sequence of elements based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <returns>The filter result.</returns>
        Task<T[]> QueryAsync(string predicate);
        /// <summary>
        /// Filters a sequence of elements based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="orderBy">Sorts the elements of a sequence in order according to a key.</param>
        /// <returns>The filter result.</returns>
        Task<T[]> QueryAsync(string predicate, string orderBy);
        /// <summary>
        /// Filters a subset of elements based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        Task<T[]> QueryAsync(string predicate, int pageIndex, int pageSize);
        /// <summary>
        /// Filters a subset of elements based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="orderBy">Sorts the elements of a sequence in order according to a key.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        Task<T[]> QueryAsync(string predicate, string orderBy, int pageIndex, int pageSize);

        /// <summary>
        /// The element is being tracked by the context but does not yet exist in the repository. 
        /// </summary>
        /// <param name="element">The element which is to be inserted.</param>
        /// <returns>The inserted element.</returns>
        Task<T> InsertAsync(T element);

        /// <summary>
        /// The element is being tracked by the context and exists in the repository, and some or all of its property values have been modified.
        /// </summary>
        /// <param name="element">The element which is to be updated.</param>
        /// <returns>The the modified element.</returns>
        Task<T> UpdateAsync(T element);

        /// <summary>
        /// Removes the element from the repository with the appropriate idelement.
        /// </summary>
        /// <param name="id">The identification.</param>
        Task DeleteAsync(IdType id);

        /// <summary>
        /// Saves any changes in the underlying persistence.
        /// </summary>
        /// <returns>The number of state entries written to the underlying database.</returns>
        Task<int> SaveChangesAsync();
    }
}
//MdEnd
