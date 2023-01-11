//@BaseCode
//MdStart

using QuickTemplate.Logic.Contracts;
using QuickTemplate.Logic.Modules.RestApi;

namespace QuickTemplate.Logic.Services
{
    using System;
    using QuickTemplate.Logic.ServiceModels;

    /// <summary>
    /// This class provides the CRUD operations for a service type.
    /// </summary>
    /// <typeparam name="TService">The service type for which the operations are available.</typeparam>
    public abstract partial class GenericService<TService> : ServiceObject, IBaseAccess<TService>
        where TService : IdentityService, new()
    {
        static GenericService()
        {
            BeforeClassInitialize();

            AfterClassInitialize();
        }
        static partial void BeforeClassInitialize();
        static partial void AfterClassInitialize();

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="baseAddress">The base address like 'https://localhost:7085/api'</param>
        /// <param name="requestUri">The request uri like 'Members'</param>
        protected GenericService(string baseAddress, string requestUri) : base(baseAddress, requestUri)
        {
        }
        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="sessionToken">The session token.</param>
        /// <param name="baseAddress">The base address like 'https://localhost:7085/api'</param>
        /// <param name="requestUri">The request uri like 'Members'</param>
        protected GenericService(string sessionToken, string baseAddress, string requestUri) : base(sessionToken, baseAddress, requestUri)
        {
        }

        #region Create
        /// <summary>
        /// Creates a new element of type TService.
        /// </summary>
        /// <returns>The new element.</returns>
        public TService Create()
        {
            return new TService();
        }
        #endregion Create

        #region MaxPageSize and Count
        /// <summary>
        /// Gets the maximum page size.
        /// </summary>
        public virtual int MaxPageSize => StaticLiterals.MaxPageSize;

        /// <summary>
        /// Gets the number of quantity in the collection.
        /// </summary>
        /// <returns>Number of entities in the collection.</returns>
        public virtual Task<int> CountAsync()
        {
            var clientAccess = new ClientAccess(BaseAddress, SessionToken);

            return clientAccess.GetCountAsync(RequestUri);
        }
        /// <summary>
        /// Returns the number of quantity in the collection based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <returns>Number of entities in the collection.</returns>
        public virtual Task<int> CountAsync(string predicate)
        {
            var clientAccess = new ClientAccess(BaseAddress, SessionToken);

            return clientAccess.GetCountAsync(RequestUri, predicate);
        }
        #endregion  MaxPageSize and Count

        #region Get
#if GUID_ON
        /// <summary>
        /// Returns the element of type T with the identification of id.
        /// </summary>
        /// <param name="id">The identification.</param>
        /// <returns>The element of the type T with the corresponding identification.</returns>
        public Task<TService?> GetByGuidAsync(Guid id)
        {
            var clientAccess = new ClientAccess(BaseAddress, SessionToken);

            return clientAccess.GetByGuidAsync<TService>(RequestUri, id);
        }
#endif

        /// <summary>
        /// Returns the element of type T with the identification of id.
        /// </summary>
        /// <param name="id">The identification.</param>
        /// <returns>The element of the type T with the corresponding identification.</returns>
        public virtual Task<TService?> GetByIdAsync(IdType id)
        {
            var clientAccess = new ClientAccess(BaseAddress, SessionToken);

            return clientAccess.GetByIdAsync<TService>(RequestUri, id);
        }

        /// <summary>
        /// Gets all items from the repository.
        /// </summary>
        /// <returns>All items in accordance with the parameters.</returns>
        public virtual Task<TService[]> GetAllAsync()
        {
            var clientAccess = new ClientAccess(BaseAddress, SessionToken);

            return clientAccess.GetAsync<TService>(RequestUri);
        }
        /// <summary>
        /// Gets all items from the repository.
        /// </summary>
        /// <param name="orderBy">Sorts the elements of a sequence according to a sort clause.</param>
        /// <returns>All items in accordance with the parameters.</returns>
        public virtual Task<TService[]> GetAllAsync(string orderBy)
        {
            var clientAccess = new ClientAccess(BaseAddress, SessionToken);

            return clientAccess.GetAsync<TService>(RequestUri, orderBy);
        }

        /// <summary>
        /// Gets a subset of items from the repository.
        /// </summary>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        public virtual Task<TService[]> GetPageListAsync(int pageIndex, int pageSize)
        {
            CheckPageParams(pageIndex, pageSize);

            var clientAccess = new ClientAccess(BaseAddress, SessionToken);

            return clientAccess.GetPageListAsync<TService>(RequestUri, pageIndex, pageSize);
        }
        /// <summary>
        /// Gets a subset of items from the repository.
        /// </summary>
        /// <param name="orderBy">Sorts the elements of a sequence in order according to a key.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        public virtual Task<TService[]> GetPageListAsync(string orderBy, int pageIndex, int pageSize)
        {
            CheckPageParams(pageIndex, pageSize);

            var clientAccess = new ClientAccess(BaseAddress, SessionToken);

            return clientAccess.GetPageListAsync<TService>(RequestUri, orderBy, pageIndex, pageSize);
        }
        #endregion Get

        #region Query
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <returns>The filter result.</returns>
        public virtual Task<TService[]> QueryAsync(string predicate)
        {
            var clientAccess = new ClientAccess(BaseAddress, SessionToken);

            return clientAccess.QueryAllAsync<TService>(RequestUri, predicate);
        }
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="orderBy">Sorts the elements of a sequence according to a sort clause.</param>
        /// <returns>The filter result.</returns>
        public virtual Task<TService[]> QueryAsync(string predicate, string orderBy)
        {
            var clientAccess = new ClientAccess(BaseAddress, SessionToken);

            return clientAccess.QueryAllAsync<TService>(RequestUri, predicate, orderBy);
        }
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <returns>The filter result.</returns>
        public virtual Task<TService[]> QueryAsync(string predicate, int pageIndex, int pageSize)
        {
            CheckPageParams(pageIndex, pageSize);

            var clientAccess = new ClientAccess(BaseAddress, SessionToken);

            return clientAccess.QueryAllAsync<TService>(RequestUri, predicate, pageIndex, pageSize);
        }
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="orderBy">Sorts the elements of a sequence according to a sort clause.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <returns>The filter result.</returns>
        public virtual Task<TService[]> QueryAsync(string predicate, string orderBy, int pageIndex, int pageSize)
        {
            CheckPageParams(pageIndex, pageSize);

            var clientAccess = new ClientAccess(BaseAddress, SessionToken);

            return clientAccess.QueryAllAsync<TService>(RequestUri, predicate, orderBy, pageIndex, pageSize);
        }
        #endregion Query

        #region Insert
        /// <summary>
        /// The service is being tracked by the context but does not yet exist in the repository. 
        /// </summary>
        /// <param name="service">The service which is to be inserted.</param>
        /// <returns>The inserted service.</returns>
        public virtual Task<TService> InsertAsync(TService service)
        {
            var clientAccess = new ClientAccess(BaseAddress, SessionToken);

            return clientAccess.PostAsync(RequestUri, service);
        }
        #endregion Insert

        #region Update
        /// <summary>
        /// The service is being tracked by the context and exists in the repository, and some or all of its property values have been modified.
        /// </summary>
        /// <param name="service">The service which is to be updated.</param>
        /// <returns>The the modified service.</returns>
        public virtual Task<TService> UpdateAsync(TService service)
        {
            var clientAccess = new ClientAccess(BaseAddress, SessionToken);

            return clientAccess.PutAsync(RequestUri, service.Id, service);
        }
        #endregion Update

        #region Delete
        /// <summary>
        /// Removes the service from the repository with the appropriate idservice.
        /// </summary>
        /// <param name="id">The identification.</param>
        public virtual Task DeleteAsync(IdType id)
        {
            var clientAccess = new ClientAccess(BaseAddress, SessionToken);

            return clientAccess.DeleteAsync(RequestUri, id);
        }
        #endregion Delete

        #region SaveChanges
        /// <summary>
        /// Saves any changes in the underlying persistence.
        /// </summary>
        /// <returns>The number of state entries written to the underlying database.</returns>
        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult(0);
        }
        #endregion SaveChanges

        #region Helpers
        internal void CheckPageParams(int pageIndex, int pageSize)
        {
            if (pageIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex));
            }
            if (pageSize <= 0 || pageSize > MaxPageSize)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }
        }
        #endregion Helpers
    }
}
//MdEnd