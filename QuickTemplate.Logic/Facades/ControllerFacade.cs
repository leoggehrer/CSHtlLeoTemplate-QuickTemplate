//@BaseCode
//MdStart

using QuickTemplate.Logic.Contracts;
using QuickTemplate.Logic.Controllers;

namespace QuickTemplate.Logic.Facades
{
    /// <summary>
    /// Generic facade for mapping entity types to model types.
    /// </summary>
    /// <typeparam name="TModel">The model type as public type</typeparam>
    public abstract partial class ControllerFacade<TModel> : FacadeObject, IDataAccess<TModel>
        where TModel : Models.ModelObject, new()
    {
        protected IDataAccess<TModel> Controller { get; init; }
        protected ControllerFacade(IDataAccess<TModel> controller)
            : base((controller as ControllerObject)!)
        {
            Controller = controller;
        }

#if ACCOUNT_ON
        #region SessionToken
        /// <summary>
        /// Sets the authorization token.
        /// </summary>
        public string SessionToken
        {
            set
            {
                Controller.SessionToken = value;
            }
        }
        #endregion SessionToken
#endif

        #region Create
        /// <summary>
        /// Creates a new element of type TModel.
        /// </summary>
        /// <returns>The new element.</returns>
        public TModel Create()
        {
            return Controller.Create();
        }
        #endregion Create

        #region MaxPageSize and Count
        /// <summary>
        /// Gets the maximum page size.
        /// </summary>
        public virtual int MaxPageSize => Controller.MaxPageSize;
        /// <summary>
        /// Gets the number of quantity in the collection.
        /// </summary>
        /// <returns>Number of elements in the collection.</returns>
        public virtual Task<int> CountAsync()
        {
            return Controller.CountAsync();
        }
        /// <summary>
        /// Returns the number of quantity in the collection based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <returns>Number of entities in the collection.</returns>
        public virtual Task<int> CountAsync(string predicate)
        {
            return Controller.CountAsync(predicate);
        }
        /// <summary>
        /// Returns the number of quantity in the collection based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>Number of entities in the collection.</returns>
        public virtual Task<int> CountAsync(string predicate, params string[] includeItems)
        {
            return Controller.CountAsync(predicate, includeItems);
        }
        #endregion MaxPageSize and Count

        #region Queries
#if GUID_ON
        /// <summary>
        /// Returns the element of type T with the identification of id.
        /// </summary>
        /// <param name="id">The identification.</param>
        /// <returns>The element of the type T with the corresponding identification.</returns>
        public virtual Task<TModel?> GetByGuidAsync(Guid id)
        {
            return Controller.GetByGuidAsync(id);
        }
#endif
        /// <summary>
        /// Returns the element of type T with the identification of id.
        /// </summary>
        /// <param name="id">The identification.</param>
        /// <returns>The element of the type T with the corresponding identification.</returns>
        public virtual Task<TModel?> GetByIdAsync(IdType id)
        {
            return Controller.GetByIdAsync(id);
        }
        /// <summary>
        /// Returns the element of type T with the identification of id.
        /// </summary>
        /// <param name="id">The identification.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>The element of the type T with the corresponding identification (with includes).</returns>
        public virtual Task<TModel?> GetByIdAsync(IdType id, params string[] includeItems)
        {
            return Controller.GetByIdAsync(id, includeItems);
        }

        /// <summary>
        /// Returns all objects of the elements in the collection.
        /// </summary>
        /// <returns>All objects of the element collection.</returns>
        public virtual Task<TModel[]> GetAllAsync()
        {
            return Controller.GetAllAsync();
        }
        /// <summary>
        /// Returns all objects of the elements in the collection.
        /// </summary>
        /// <param name="includeItems">The include items</param>
        /// <returns>All objects of the element collection.</returns>
        public virtual Task<TModel[]> GetAllAsync(params string[] includeItems)
        {
            return Controller.GetAllAsync(includeItems);
        }
        /// <summary>
        /// Returns all elements in the collection.
        /// </summary>
        /// <param name="orderBy">Sorts the elements of a sequence according to a sort clause.</param>
        /// <returns>All interfaces of the element collection.</returns>
        public virtual Task<TModel[]> GetAllAsync(string orderBy)
        {
            return Controller.GetAllAsync(orderBy);
        }
        /// <summary>
        /// Returns all interfaces of the elements in the collection.
        /// </summary>
        /// <param name="includeItems">The include items</param>
        /// <returns>All interfaces of the element collection.</returns>
        public virtual Task<TModel[]> GetAllAsync(string orderBy, params string[] includeItems)
        {
            return Controller.GetAllAsync(orderBy, includeItems);
        }

        /// <summary>
        /// Gets a subset of items from the repository.
        /// </summary>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        public virtual Task<TModel[]> GetPageListAsync(int pageIndex, int pageSize)
        {
            return Controller.GetPageListAsync(pageIndex, pageSize);
        }
        /// <summary>
        /// Gets a subset of items from the repository.
        /// </summary>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        public virtual Task<TModel[]> GetPageListAsync(int pageIndex, int pageSize, params string[] includeItems)
        {
            return Controller.GetPageListAsync(pageIndex, pageSize, includeItems);
        }
        /// <summary>
        /// Gets a subset of items from the repository.
        /// </summary>
        /// <param name="orderBy">Sorts the elements of a sequence in order according to a key.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        public virtual Task<TModel[]> GetPageListAsync(string orderBy, int pageIndex, int pageSize)
        {
            return Controller.GetPageListAsync(orderBy, pageIndex, pageSize);
        }
        /// <summary>
        /// Gets a subset of items from the repository.
        /// </summary>
        /// <param name="orderBy">Sorts the elements of a sequence in order according to a key.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        public virtual Task<TModel[]> GetPageListAsync(string orderBy, int pageIndex, int pageSize, params string[] includeItems)
        {
            return Controller.GetPageListAsync(orderBy, pageIndex, pageSize, includeItems);
        }

        /// <summary>
        /// Filters a sequence of elements based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <returns>The filter result.</returns>
        public virtual Task<TModel[]> QueryAsync(string predicate)
        {
            return Controller.QueryAsync(predicate);
        }
        /// <summary>
        /// Gets a subset of items from the repository.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        public virtual Task<TModel[]> QueryAsync(string predicate, params string[] includeItems)
        {
            return Controller.QueryAsync(predicate, includeItems);
        }
        /// <summary>
        /// Filters a sequence of elements based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="orderBy">Sorts the elements of a sequence in order according to a key.</param>
        /// <returns>The filter result.</returns>
        public virtual Task<TModel[]> QueryAsync(string predicate, string orderBy)
        {
            return Controller.QueryAsync(predicate, orderBy);
        }

        /// <summary>
        /// Filters a subset of elements based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        public virtual Task<TModel[]> QueryAsync(string predicate, int pageIndex, int pageSize)
        {
            return Controller.QueryAsync(predicate, pageIndex, pageSize);
        }
        /// <summary>
        /// Filters a subset of elements based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        public virtual Task<TModel[]> QueryAsync(string predicate, int pageIndex, int pageSize, params string[] includeItems)
        {
            return Controller.QueryAsync(predicate, pageIndex, pageSize, includeItems);
        }
        /// <summary>
        /// Filters a subset of elements based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="orderBy">Sorts the elements of a sequence in order according to a key.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        public virtual Task<TModel[]> QueryAsync(string predicate, string orderBy, int pageIndex, int pageSize)
        {
            return Controller.QueryAsync(predicate, orderBy, pageIndex, pageSize);
        }
        /// <summary>
        /// Filters a subset of elements based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="orderBy">Sorts the elements of a sequence in order according to a key.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        public virtual Task<TModel[]> QueryAsync(string predicate, string orderBy, int pageIndex, int pageSize, params string[] includeItems)
        {
            return Controller.QueryAsync(predicate, orderBy, pageIndex, pageSize, includeItems);
        }
        #endregion Queries

        #region Insert
        /// <summary>
        /// The element is being tracked by the context but does not yet exist in the repository. 
        /// </summary>
        /// <param name="element">The element which is to be inserted.</param>
        /// <returns>The inserted element.</returns>
        public virtual Task<TModel> InsertAsync(TModel model)
        {
            return Controller.InsertAsync(model);
        }
        /// <summary>
        /// The elements are being tracked by the context but does not yet exist in the repository. 
        /// </summary>
        /// <param name="elements">The elements which are to be inserted.</param>
        /// <returns>The inserted elements.</returns>
        public virtual Task<IEnumerable<TModel>> InsertAsync(IEnumerable<TModel> models)
        {
            return Controller.InsertAsync(models);
        }
        #endregion Insert

        #region Update
        /// <summary>
        /// The element is being tracked by the context and exists in the repository, and some or all of its property values have been modified.
        /// </summary>
        /// <param name="element">The element which is to be updated.</param>
        /// <returns>The the modified element.</returns>
        public virtual async Task<TModel> UpdateAsync(TModel model)
        {
            var updModel = await Controller.GetByIdAsync(model.Id).ConfigureAwait(false);

            _ = updModel ?? throw new Exception("Entity not found.");

            updModel.CopyFrom(model);
            return await Controller.UpdateAsync(updModel).ConfigureAwait(false);
        }
        /// <summary>
        /// The elements are being tracked by the context and exists in the repository, and some or all of its property values have been modified.
        /// </summary>
        /// <param name="elements">The elements which are to be updated.</param>
        /// <returns>The updated elements.</returns>
        public virtual async Task<IEnumerable<TModel>> UpdateAsync(IEnumerable<TModel> models)
        {
            var result = new List<TModel>();

            foreach (var model in models)
            {
                result.Add(await UpdateAsync(model).ConfigureAwait(false));
            }
            return result;
        }
        #endregion Update

        #region Delete
        /// <summary>
        /// Removes the element from the repository with the appropriate idelement.
        /// </summary>
        /// <param name="id">The identification.</param>
        public virtual Task DeleteAsync(IdType id)
        {
            return Controller.DeleteAsync(id);
        }
        #endregion Delete

        #region SaveChanges
        /// <summary>
        /// Saves any changes in the underlying persistence.
        /// </summary>
        /// <returns>The number of state entries written to the underlying database.</returns>
        public Task<int> SaveChangesAsync()
        {
            return Controller.SaveChangesAsync();
        }
        #endregion SaveChanges
    }
}
//MdEnd
