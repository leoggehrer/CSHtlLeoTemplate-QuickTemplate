//@BaseCode
//MdStart
using CommonBase.Contracts;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace QuickTemplate.Logic.Controllers
{
    /// <summary>
    /// This class provides the CRUD operations for an entity type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type for which the operations are available.</typeparam>
    /// <typeparam name="TOutModel">The type of the output model.</typeparam>
#if ACCOUNT_ON
    [Modules.Security.Authorize]
#endif
    public abstract partial class EntitiesController<TEntity, TOutModel> : ControllerObject, IDataAccess<TOutModel>
    where TEntity : Entities.EntityObject, new()
    where TOutModel : Models.ModelObject, new()
    {
        protected enum ActionType
        {
            Insert = 1,
            Update = 2 * Insert,
            Delete = 2 * Update,
            Save = 2 * Delete,
        }
        /// <summary>
        /// Initializes the static instance of the EntitiesController class.
        /// </summary>
        /// <remarks>
        /// This method is called before any instances of the EntitiesController class are created.
        /// It performs necessary initialization tasks.
        /// </remarks>
        static EntitiesController()
        {
            BeforeClassInitialize();
            
            AfterClassInitialize();
        }
        /// <summary>
        /// Executes before the initialization of the class.
        /// </summary>
        /// <remarks>
        /// This method is intended to be implemented partially.
        /// This method is called before the class is initialized.
        /// </remarks>
        static partial void BeforeClassInitialize();
        /// <summary>
        /// This method is called after the initialization of the class is complete.
        /// </summary>
        static partial void AfterClassInitialize();
        
        private DbSet<TEntity>? dbSet = null;
        
        /// <summary>
        /// Gets the internal entity set.
        /// </summary>
        internal DbSet<TEntity> EntitySet
        {
            get
            {
                if (dbSet == null)
                {
                    if (Context != null)
                    {
                        dbSet = Context.GetDbSet<TEntity>();
                    }
                    else
                    {
                        using var ctx = new DataContext.ProjectDbContext();
                        
                        dbSet = ctx.GetDbSet<TEntity>();
                        
                    }
                }
                return dbSet;
            }
        }
        /// <summary>
        /// Gets the internal entity set as queryable.
        /// </summary>
        internal IQueryable<TEntity> QuerySet => EntitySet.AsQueryable();
        /// <summary>
        /// Gets the internal includes.
        /// </summary>
        /// <returns></returns>
        internal virtual IEnumerable<string> Includes => Array.Empty<string>();
        
        /// <summary>
        /// Creates an instance.
        /// </summary>
        protected EntitiesController()
        : base(new DataContext.ProjectDbContext())
        {
        }
        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="other">A reference to an other controller</param>
        protected EntitiesController(ControllerObject other)
        : base(other)
        {
        }
        
        #region MaxPageSize and Count
        /// <summary>
        /// Gets the maximum page size.
        /// </summary>
        public virtual int MaxPageSize => StaticLiterals.MaxPageSize;
        
        /// <summary>
        /// Creates a new element of type TModel.
        /// </summary>
        /// <returns>The new model.</returns>
        public virtual TOutModel Create()
        {
            return ToModel(new TEntity());
        }
        
        /// <summary>
        /// Gets the number of quantity in the collection.
        /// </summary>
        /// <returns>Number of entities in the collection.</returns>
        public virtual async Task<int> CountAsync()
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(CountAsync)).ConfigureAwait(false);
#endif
            return await ExecuteCountAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// Gets the number of quantity in the collection (without authorization).
        /// </summary>
        /// <returns>Number of entities in the collection.</returns>
        internal virtual Task<int> ExecuteCountAsync()
        {
            return EntitySet.CountAsync();
        }
        /// <summary>
        /// Returns the number of quantity in the collection based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <returns>Number of entities in the collection.</returns>
        public virtual async Task<int> CountAsync(string predicate)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(CountAsync), predicate).ConfigureAwait(false);
#endif
            return await ExecuteCountAsync(predicate, Array.Empty<string>()).ConfigureAwait(false);
        }
        /// <summary>
        /// Returns the number of quantity in the collection based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>Number of entities in the collection.</returns>
        public virtual async Task<int> CountAsync(string predicate, params string[] includeItems)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(CountAsync), predicate).ConfigureAwait(false);
#endif
            return await ExecuteCountAsync(predicate, includeItems).ConfigureAwait(false);
        }
        /// <summary>
        /// Returns the number of quantity in the collection based on a predicate (without authorization).
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>Number of entities in the collection.</returns>
        internal virtual Task<int> ExecuteCountAsync(string predicate, params string[] includeItems)
        {
            var query = EntitySet.AsQueryable();
            
            foreach (var includeItem in Includes.Union(includeItems).Distinct())
            {
                query = query.Include(includeItem);
            }
            return query.Where(predicate).CountAsync();
        }
        /// <summary>
        /// Returns the number of quantity in the collection based on a predicate (without authorization).
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>Number of entities in the collection.</returns>
        internal virtual Task<int> ExecuteCountAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeItems)
        {
            var query = EntitySet.AsQueryable();
            
            foreach (var includeItem in Includes.Union(includeItems).Distinct())
            {
                query = query.Include(includeItem);
            }
            return query.Where(predicate).CountAsync();
        }
        #endregion  MaxPageSize and Count
        
        #region Converts
        /// <summary>
        /// Converts the entity type to the facade type.
        /// </summary>
        /// <param name="entity">Entity type</param>
        /// <returns>The facade type</returns>
        internal virtual TOutModel ToModel(TEntity entity)
        {
            var result = new TOutModel
            {
                Source = entity
            };
            return result;
        }
        /// <summary>
        /// Converts the model type to the entity type.
        /// </summary>
        /// <param name="model">Model type</param>
        /// <returns>The entity type</returns>
        internal virtual TEntity ToEntity(TOutModel model)
        {
            var result = model.Source as TEntity;
            
            return result!;
        }
        #endregion Converts
        
        #region Before-Return
        /// <summary>
        /// Transforms the given entity into the corresponding model before returning it.
        /// </summary>
        /// <param name="entity">The entity to transform.</param>
        /// <returns>The corresponding model of the entity.</returns>
        /// <typeparam name="TOutModel">The type of the model.</typeparam>
        /// <param name="entity">The entity to transform.</param>
        /// <returns>The corresponding model of the entity.</returns>
        internal virtual TOutModel BeforeReturn(TEntity entity) => ToModel(entity);
        /// <summary>
        /// Performs a transformation of the given entities to a collection of output models before returning.
        /// </summary>
        /// <typeparam name="TOutModel">The type of output model.</typeparam>
        /// <param name="entities">The collection of entities to transform.</param>
        /// <returns>A collection of output models.</returns>
        internal virtual IEnumerable<TOutModel> BeforeReturn(IEnumerable<TEntity> entities) => entities.Select(e => ToModel(e));
        #endregion Before-Return
        
        #region Get
        /// <summary>
        /// Returns the element of type T with the identification of id.
        /// </summary>
        /// <param name="id">The identification.</param>
        /// <returns>The element of the type T with the corresponding identification.</returns>
        public virtual async Task<TOutModel?> GetByIdAsync(IdType id)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(GetByIdAsync), id.ToString()).ConfigureAwait(false);
#endif
            var result = await ExecuteGetByIdAsync(id).ConfigureAwait(false);
            
            return result != null ? BeforeReturn(result) : null;
        }
        /// <summary>
        /// Returns the element of type T with the identification of id.
        /// </summary>
        /// <param name="id">The identification.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>The element of the type T with the corresponding identification (with includes).</returns>
        public virtual async Task<TOutModel?> GetByIdAsync(IdType id, params string[] includeItems)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(GetByIdAsync), id.ToString()).ConfigureAwait(false);
#endif
            var result = await ExecuteGetByIdAsync(id, includeItems).ConfigureAwait(false);
            
            return result != null ? BeforeReturn(result) : null;
        }
        
        /// <summary>
        /// Gets all items from the repository.
        /// </summary>
        /// <param name="includeItems">The include items</param>
        /// <returns>All items in accordance with the parameters.</returns>
        public virtual async Task<TOutModel[]> GetAllAsync()
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(GetAllAsync)).ConfigureAwait(false);
#endif
            var result = await ExecuteGetAllAsync().ConfigureAwait(false);
            
            return result != null ? BeforeReturn(result).ToArray() : Array.Empty<TOutModel>();
        }
        /// <summary>
        /// Gets all items from the repository.
        /// </summary>
        /// <param name="includeItems">The include items</param>
        /// <returns>All items in accordance with the parameters.</returns>
        public virtual async Task<TOutModel[]> GetAllAsync(params string[] includeItems)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(GetAllAsync)).ConfigureAwait(false);
#endif
            var result = await ExecuteGetAllAsync(includeItems).ConfigureAwait(false);
            
            return result != null ? BeforeReturn(result).ToArray() : Array.Empty<TOutModel>();
        }
        /// <summary>
        /// Gets all items from the repository.
        /// </summary>
        /// <param name="orderBy">Sorts the elements of a sequence according to a sort clause.</param>
        /// <returns>All items in accordance with the parameters.</returns>
        public virtual async Task<TOutModel[]> GetAllAsync(string orderBy)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(GetAllAsync)).ConfigureAwait(false);
#endif
            var result = await ExecuteGetAllAsync(orderBy).ConfigureAwait(false);
            
            return result != null ? BeforeReturn(result).ToArray() : Array.Empty<TOutModel>();
        }
        /// <summary>
        /// Gets all items from the repository.
        /// </summary>
        /// <param name="orderBy">Sorts the elements of a sequence according to a sort clause.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>All items in accordance with the parameters.</returns>
        public virtual async Task<TOutModel[]> GetAllAsync(string orderBy, params string[] includeItems)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(GetAllAsync)).ConfigureAwait(false);
#endif
            var result = await ExecuteGetAllAsync(orderBy, includeItems).ConfigureAwait(false);
            
            return result != null ? BeforeReturn(result).ToArray() : Array.Empty<TOutModel>();
        }
        
        /// <summary>
        /// Gets a subset of items from the repository.
        /// </summary>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        public virtual async Task<TOutModel[]> GetPageListAsync(int pageIndex, int pageSize)
        {
            CheckPageParams(pageIndex, pageSize);
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(GetPageListAsync)).ConfigureAwait(false);
#endif
            var result = await ExecuteGetPageListAsync(pageIndex, pageSize).ConfigureAwait(false);
            
            return result != null ? BeforeReturn(result).ToArray() : Array.Empty<TOutModel>();
        }
        /// <summary>
        /// Gets a subset of items from the repository.
        /// </summary>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        public virtual async Task<TOutModel[]> GetPageListAsync(int pageIndex, int pageSize, params string[] includeItems)
        {
            CheckPageParams(pageIndex, pageSize);
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(GetPageListAsync)).ConfigureAwait(false);
#endif
            var result = await ExecuteGetPageListAsync(pageIndex, pageSize, includeItems).ConfigureAwait(false);
            
            return result != null ? BeforeReturn(result).ToArray() : Array.Empty<TOutModel>();
        }
        /// <summary>
        /// Gets a subset of items from the repository.
        /// </summary>
        /// <param name="orderBy">Sorts the elements of a sequence in order according to a key.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        public virtual async Task<TOutModel[]> GetPageListAsync(string orderBy, int pageIndex, int pageSize)
        {
            CheckPageParams(pageIndex, pageSize);
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(GetPageListAsync)).ConfigureAwait(false);
#endif
            var result = await ExecuteGetPageListAsync(orderBy, pageIndex, pageSize).ConfigureAwait(false);
            
            return result != null ? BeforeReturn(result).ToArray() : Array.Empty<TOutModel>();
        }
        /// <summary>
        /// Gets a subset of items from the repository.
        /// </summary>
        /// <param name="orderBy">Sorts the elements of a sequence in order according to a key.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        public virtual async Task<TOutModel[]> GetPageListAsync(string orderBy, int pageIndex, int pageSize, params string[] includeItems)
        {
            CheckPageParams(pageIndex, pageSize);
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(GetPageListAsync)).ConfigureAwait(false);
#endif
            var result = await ExecuteGetPageListAsync(orderBy, pageIndex, pageSize, includeItems).ConfigureAwait(false);
            
            return result != null ? BeforeReturn(result).ToArray() : Array.Empty<TOutModel>();
        }
        
        /// <summary>
        /// Returns the element of type T with the identification of id (without authorization).
        /// </summary>
        /// <param name="id">The identification.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>The element of the type T with the corresponding identification (with includes).</returns>
        internal virtual Task<TEntity?> ExecuteGetByIdAsync(IdType id, params string[] includeItems)
        {
            var query = EntitySet.AsQueryable();
            
            foreach (var includeItem in Includes.Union(includeItems).Distinct())
            {
                query = query.Include(includeItem);
            }
            return query.FirstOrDefaultAsync(e => e.Id == id);
        }
        
        /// <summary>
        /// Returns all entities in the collection (without authorization).
        /// </summary>
        /// <param name="includeItems">The include items</param>
        /// <returns>All entity collection (with include).</returns>
        internal virtual async Task<TEntity[]> ExecuteGetAllAsync(params string[] includeItems)
        {
            int idx = 0, qryCount;
            var result = new List<TEntity>();
            var query = EntitySet.AsQueryable();
            
            foreach (var includeItem in Includes.Union(includeItems).Distinct())
            {
                query = query.Include(includeItem);
            }
            
            do
            {
                var qry = await query.Skip(idx++ * MaxPageSize)
                                     .Take(MaxPageSize)
                                     .AsNoTracking()
                                     .ToArrayAsync()
                                     .ConfigureAwait(false);
                
                qryCount = result.AddRangeAndCount(qry);
            } while (qryCount == MaxPageSize);
            return result.ToArray();
        }
        /// <summary>
        /// Returns all entities in the collection (without authorization).
        /// </summary>
        /// <param name="orderBy">Sorts the elements of a sequence according to a sort clause.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>All entity collection (with include).</returns>
        internal virtual async Task<TEntity[]> ExecuteGetAllAsync(string orderBy, params string[] includeItems)
        {
            int idx = 0, qryCount;
            var result = new List<TEntity>();
            var query = EntitySet.AsQueryable();
            
            foreach (var includeItem in Includes.Union(includeItems).Distinct())
            {
                query = query.Include(includeItem);
            }
            
            do
            {
                var qry = await query.OrderBy(orderBy)
                                     .Skip(idx++ * MaxPageSize)
                                     .Take(MaxPageSize)
                                     .AsNoTracking()
                                     .ToArrayAsync()
                                     .ConfigureAwait(false);
                
                qryCount = result.AddRangeAndCount(qry);
            } while (qryCount == MaxPageSize);
            return result.ToArray();
        }
        
        /// <summary>
        /// Returns a subset of items from the repository.
        /// </summary>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        internal virtual Task<TEntity[]> ExecuteGetPageListAsync(int pageIndex, int pageSize, params string[] includeItems)
        {
            var query = EntitySet.AsQueryable();
            
            foreach (var includeItem in Includes.Union(includeItems).Distinct())
            {
                query = query.Include(includeItem);
            }
            return query.Skip(pageIndex * pageSize)
                        .Take(pageSize)
                        .AsNoTracking()
                        .ToArrayAsync();
        }
        /// <summary>
        /// Returns a subset of items from the repository.
        /// </summary>
        /// <param name="orderBy">Sorts the elements of a sequence in order according to a key.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>Subset in accordance with the parameters.</returns>
        internal virtual Task<TEntity[]> ExecuteGetPageListAsync(string orderBy, int pageIndex, int pageSize, params string[] includeItems)
        {
            var query = EntitySet.AsQueryable();
            
            foreach (var includeItem in Includes.Union(includeItems).Distinct())
            {
                query = query.Include(includeItem);
            }
            return query.OrderBy(orderBy)
                        .Skip(pageIndex * pageSize)
                        .Take(pageSize)
                        .AsNoTracking()
                        .ToArrayAsync();
        }
        #endregion Get
        
        #region Query
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <returns>The filter result.</returns>
        public virtual async Task<TOutModel[]> QueryAsync(string predicate)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(QueryAsync), predicate).ConfigureAwait(false);
#endif
            int idx = 0, qryCount;
            var result = new List<TEntity>();
            do
            {
                var qry = await ExecuteQueryAsync(predicate, idx, MaxPageSize).ConfigureAwait(false);
                
                qryCount = result.AddRangeAndCount(qry);
            } while (qryCount == MaxPageSize);
            return BeforeReturn(result).ToArray();
        }
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>The filter result.</returns>
        public virtual async Task<TOutModel[]> QueryAsync(string predicate, params string[] includeItems)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(QueryAsync), predicate).ConfigureAwait(false);
#endif
            int idx = 0, qryCount;
            var result = new List<TEntity>();
            do
            {
                var qry = await ExecuteQueryAsync(predicate, idx, MaxPageSize, includeItems).ConfigureAwait(false);
                
                qryCount = result.AddRangeAndCount(qry);
            } while (qryCount == MaxPageSize);
            return BeforeReturn(result).ToArray();
        }
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="orderBy">Sorts the elements of a sequence according to a sort clause.</param>
        /// <returns>The filter result.</returns>
        public virtual async Task<TOutModel[]> QueryAsync(string predicate, string orderBy)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(QueryAsync), predicate).ConfigureAwait(false);
#endif
            int idx = 0, qryCount;
            var result = new List<TEntity>();
            do
            {
                var qry = await ExecuteQueryAsync(predicate, orderBy, idx, MaxPageSize).ConfigureAwait(false);
                
                qryCount = result.AddRangeAndCount(qry);
            } while (qryCount == MaxPageSize);
            return BeforeReturn(result).ToArray();
        }
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="orderBy">Sorts the elements of a sequence according to a sort clause.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>The filter result.</returns>
        public virtual async Task<TOutModel[]> QueryAsync(string predicate, string orderBy, params string[] includeItems)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(QueryAsync), predicate).ConfigureAwait(false);
#endif
            int idx = 0, qryCount;
            var result = new List<TEntity>();
            do
            {
                var qry = await ExecuteQueryAsync(predicate, orderBy, idx, MaxPageSize, includeItems).ConfigureAwait(false);
                
                qryCount = result.AddRangeAndCount(qry);
            } while (qryCount == MaxPageSize);
            return BeforeReturn(result).ToArray();
        }
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <returns>The filter result.</returns>
        public virtual async Task<TOutModel[]> QueryAsync(string predicate, int pageIndex, int pageSize)
        {
            CheckPageParams(pageIndex, pageSize);
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(QueryAsync), predicate).ConfigureAwait(false);
#endif
            var result = await ExecuteQueryAsync(predicate, pageIndex, pageSize).ConfigureAwait(false);
            
            return result != null ? BeforeReturn(result).ToArray() : Array.Empty<TOutModel>();
        }
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>The filter result.</returns>
        public virtual async Task<TOutModel[]> QueryAsync(string predicate, int pageIndex, int pageSize, params string[] includeItems)
        {
            CheckPageParams(pageIndex, pageSize);
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(QueryAsync), predicate).ConfigureAwait(false);
#endif
            var result = await ExecuteQueryAsync(predicate, pageIndex, pageSize, includeItems).ConfigureAwait(false);
            
            return result != null ? BeforeReturn(result).ToArray() : Array.Empty<TOutModel>();
        }
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="orderBy">Sorts the elements of a sequence according to a sort clause.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <returns>The filter result.</returns>
        public virtual async Task<TOutModel[]> QueryAsync(string predicate, string orderBy, int pageIndex, int pageSize)
        {
            CheckPageParams(pageIndex, pageSize);
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(QueryAsync), predicate).ConfigureAwait(false);
#endif
            var result = await ExecuteQueryAsync(predicate, orderBy, pageIndex, pageSize).ConfigureAwait(false);
            
            return result != null ? BeforeReturn(result).ToArray() : Array.Empty<TOutModel>();
        }
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="orderBy">Sorts the elements of a sequence according to a sort clause.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>The filter result.</returns>
        public virtual async Task<TOutModel[]> QueryAsync(string predicate, string orderBy, int pageIndex, int pageSize, params string[] includeItems)
        {
            CheckPageParams(pageIndex, pageSize);
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(QueryAsync), predicate).ConfigureAwait(false);
#endif
            var result = await ExecuteQueryAsync(predicate, orderBy, pageIndex, pageSize, includeItems).ConfigureAwait(false);
            
            return result != null ? BeforeReturn(result).ToArray() : Array.Empty<TOutModel>();
        }
        
        /// <summary>
        /// Filters a sequence of values based on a predicate (without authorization).
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>The filter result.</returns>
        internal virtual Task<TOutModel[]> ExecuteQueryAsync(string predicate, params string[] includeItems)
        {
            var query = EntitySet.AsQueryable();
            
            foreach (var includeItem in Includes.Union(includeItems).Distinct())
            {
                query = query.Include(includeItem);
            }
            return query.Where(predicate).AsNoTracking().Select(e => ToModel(e)).ToArrayAsync();
        }
        /// <summary>
        /// Filters a sequence of values based on a predicate (without authorization).
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>The filter result.</returns>
        internal virtual Task<TEntity[]> ExecuteQueryAsync(string predicate, int pageIndex, int pageSize, params string[] includeItems)
        {
            var query = EntitySet.AsQueryable();
            
            foreach (var includeItem in Includes.Union(includeItems).Distinct())
            {
                query = query.Include(includeItem);
            }
            return query.Where(predicate)
                        .Skip(pageIndex * pageSize)
                        .Take(pageSize)
                        .AsNoTracking()
                        .ToArrayAsync();
        }
        /// <summary>
        /// Filters a sequence of values based on a predicate (without authorization).
        /// </summary>
        /// <param name="predicate">A string to test each element for a condition.</param>
        /// <param name="orderBy">Sorts the elements of a sequence according to a sort clause.</param>
        /// <param name="pageIndex">0 based page index.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>The filter result.</returns>
        internal virtual Task<TEntity[]> ExecuteQueryAsync(string predicate, string orderBy, int pageIndex, int pageSize, params string[] includeItems)
        {
            var query = EntitySet.AsQueryable();
            
            foreach (var includeItem in Includes.Union(includeItems).Distinct())
            {
                query = query.Include(includeItem);
            }
            return query.Where(predicate)
                        .OrderBy(orderBy)
                        .Skip(pageIndex * pageSize)
                        .Take(pageSize)
                        .AsNoTracking()
                        .ToArrayAsync();
        }
        /// <summary>
        /// Filters a sequence of values based on a predicate (without authorization).
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>The filter result.</returns>
        internal virtual Task<TEntity[]> ExecuteQueryAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeItems)
        {
            var query = EntitySet.AsQueryable();
            
            foreach (var includeItem in Includes.Union(includeItems).Distinct())
            {
                query = query.Include(includeItem);
            }
            return query.Where(predicate).AsNoTracking().ToArrayAsync();
        }
        #endregion Query
        
        #region Action
        /// <summary>
        /// This method is called before an action is performed.
        /// </summary>
        /// <param name="actionType">Action types such as insert, edit, etc.</param>
        /// <param name="entity">The entity that the action affects.</param>
        protected virtual void ValidateEntity(ActionType actionType, TEntity entity)
        {
            
        }
        /// <summary>
        /// This method is called before an action is performed.
        /// </summary>
        /// <param name="actionType">Action types such as save, etc.</param>
        protected virtual void BeforeActionExecute(ActionType actionType)
        {
            
        }
        /// <summary>
        /// This method is called before an action is performed.
        /// </summary>
        /// <param name="actionType">Action types such as insert, edit, etc.</param>
        /// <param name="entity">The entity that the action affects.</param>
        protected virtual void BeforeActionExecute(ActionType actionType, TEntity entity)
        {
            
        }
        /// <summary>
        /// This method is called after an action is performed.
        /// </summary>
        /// <param name="actionType">Action types such as insert, edit, etc.</param>
        protected virtual void AfterActionExecute(ActionType actionType)
        {
            
        }
        /// <summary>
        /// This method is called after an action is performed.
        /// </summary>
        /// <param name="actionType">Action types such as insert, edit, etc.</param>
        /// <param name="entity">The entity that the action affects.</param>
        protected virtual void AfterActionExecute(ActionType actionType, TEntity entity)
        {
            
        }
        #endregion Action
        
        #region Insert
        /// <summary>
        /// The entity is being tracked by the context but does not yet exist in the repository.
        /// </summary>
        /// <param name="model">The model which is to be inserted.</param>
        /// <returns>The inserted model.</returns>
        public virtual async Task<TOutModel> InsertAsync(TOutModel model)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(InsertAsync)).ConfigureAwait(false);
#endif
            var result = await ExecuteInsertAsync(ToEntity(model)).ConfigureAwait(false);
            
            return BeforeReturn(result);
        }
        /// <summary>
        /// The entity is being tracked by the context but does not yet exist in the repository.
        /// </summary>
        /// <param name="entity">The entity which is to be inserted.</param>
        /// <returns>The inserted entity.</returns>
        internal virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(InsertAsync)).ConfigureAwait(false);
#endif
            return await ExecuteInsertAsync(entity).ConfigureAwait(false);
        }
        /// <summary>
        /// The entity is being tracked by the context but does not yet exist in the repository (without authorization).
        /// </summary>
        /// <param name="entity">The entity which is to be inserted.</param>
        /// <returns>The inserted entity.</returns>
        internal virtual async Task<TEntity> ExecuteInsertAsync(TEntity entity)
        {
            ValidateEntity(ActionType.Insert, entity);
            BeforeActionExecute(ActionType.Insert, entity);
            BeforeExecuteInsert(entity);
            HandleInsertExtendedProperties(entity);
            await EntitySet.AddAsync(entity).ConfigureAwait(false);
            AfterExecuteInsert(entity);
            AfterActionExecute(ActionType.Insert, entity);
            return entity;
        }
        /// <summary>
        /// The entities are being tracked by the context but does not yet exist in the repository.
        /// </summary>
        /// <param name="models">The models which are to be inserted.</param>
        /// <returns>The inserted models.</returns>
        public virtual async Task<IEnumerable<TOutModel>> InsertAsync(IEnumerable<TOutModel> models)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(InsertAsync), "Array").ConfigureAwait(false);
#endif
            var entities = models.Select(m => ToEntity(m));
            var result = await ExecuteInsertAsync(entities).ConfigureAwait(false);
            
            return BeforeReturn(result);
        }
        /// <summary>
        /// The entities are being tracked by the context but does not yet exist in the repository (without authorization).
        /// </summary>
        /// <param name="entities">The entities which are to be inserted.</param>
        /// <returns>The inserted entities.</returns>
        internal virtual async Task<IEnumerable<TEntity>> ExecuteInsertAsync(IEnumerable<TEntity> entities)
        {
            var entityList = new List<TEntity>();
            
            foreach (var entity in entities)
            {
                ValidateEntity(ActionType.Insert, entity);
                BeforeActionExecute(ActionType.Insert, entity);
                BeforeExecuteInsert(entity);
                HandleInsertExtendedProperties(entity);
                entityList.Add(entity);
            }
            await EntitySet.AddRangeAsync(entityList).ConfigureAwait(false);
            foreach (var entity in entityList)
            {
                AfterExecuteInsert(entity);
                AfterActionExecute(ActionType.Insert, entity);
            }
            return entityList;
        }
        /// <summary>
        /// Called before executing the insert operation for the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity being inserted.</typeparam>
        /// <param name="entity">The entity being inserted.</param>
        /// <remarks>
        /// This method can be used to perform any necessary operations or validations before the insert operation is executed.
        /// </remarks>
        partial void BeforeExecuteInsert(TEntity entity);
        /// <summary>
        /// This method is called after an entity has been successfully inserted into the database.
        /// </summary>
        /// <param name="entity">The entity that has been inserted.</param>
        partial void AfterExecuteInsert(TEntity entity);
        #endregion Insert
        
        #region Update
        /// <summary>
        /// The model is being tracked by the context and exists in the repository, and some or all of its property values have been modified.
        /// </summary>
        /// <param name="model">The model which is to be updated.</param>
        /// <returns>The the modified model.</returns>
        public virtual async Task<TOutModel> UpdateAsync(TOutModel model)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(UpdateAsync)).ConfigureAwait(false);
            var result = ExecuteUpdate(ToEntity(model));
            
            return BeforeReturn(result);
#else
            var result = ExecuteUpdate(ToEntity(model));
            
            return await Task.Run(() => BeforeReturn(result)).ConfigureAwait(false);
#endif
        }
        /// <summary>
        /// The entity is being tracked by the context and exists in the repository, and some or all of its property values have been modified.
        /// </summary>
        /// <param name="model">The model which is to be updated.</param>
        /// <returns>The the modified model.</returns>
        internal virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(UpdateAsync)).ConfigureAwait(false);
            
            return ExecuteUpdate(entity);
#else
            return await Task.Run(() => ExecuteUpdate(entity)).ConfigureAwait(false);
#endif
        }
        /// <summary>
        /// The entity is being tracked by the context and exists in the repository, and some or all of its property values have been modified (without authorization).
        /// </summary>
        /// <param name="entity">The entity which is to be updated.</param>
        /// <returns>The the modified entity.</returns>
        internal virtual TEntity ExecuteUpdate(TEntity entity)
        {
            ValidateEntity(ActionType.Update, entity);
            BeforeActionExecute(ActionType.Update, entity);
            BeforeExecuteUpdate(entity);
            HandleUpdateExtendedProperties(entity);
            EntitySet.Update(entity);
            AfterExecuteUpdate(entity);
            AfterActionExecute(ActionType.Update, entity);
            return entity;
        }
        /// <summary>
        /// The entities are being tracked by the context and exists in the repository, and some or all of its property values have been modified.
        /// </summary>
        /// <param name="models">The models which are to be updated.</param>
        /// <returns>The updated models.</returns>
        public virtual async Task<IEnumerable<TOutModel>> UpdateAsync(IEnumerable<TOutModel> models)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(UpdateAsync), "Array").ConfigureAwait(false);
            var result = ExecuteUpdate(models.Select(m => ToEntity(m)));
            
            return BeforeReturn(result);
#else
            var result = ExecuteUpdate(models.Select(m => ToEntity(m)));
            
            return await Task.Run(() => BeforeReturn(result)).ConfigureAwait(false);
#endif
        }
        /// <summary>
        /// The entities are being tracked by the context and exists in the repository, and some or all of its property values have been modified.
        /// </summary>
        /// <param name="entities">The entities which are to be updated.</param>
        /// <returns>The updated entities.</returns>
        internal virtual IEnumerable<TEntity> ExecuteUpdate(IEnumerable<TEntity> entities)
        {
            var entityList = new List<TEntity>();
            
            foreach (var entity in entities)
            {
                ValidateEntity(ActionType.Update, entity);
                BeforeActionExecute(ActionType.Update, entity);
                BeforeExecuteUpdate(entity);
                HandleUpdateExtendedProperties(entity);
                entityList.Add(entity);
            }
            EntitySet.UpdateRange(entityList);
            foreach (var entity in entityList)
            {
                AfterExecuteUpdate(entity);
                AfterActionExecute(ActionType.Update, entity);
            }
            return entityList;
        }
        /// <summary>
        /// This method is called before executing an update operation on an entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity that is being updated.</param>
        partial void BeforeExecuteUpdate(TEntity entity);
        /// <summary>
        /// This method is called after executing an update operation on the specified entity.
        /// </summary>
        /// <param name="entity">The entity that was updated.</param>
        /// <remarks>
        /// Use this method to perform any additional actions or logic after updating the entity in the database.
        /// </remarks>
        partial void AfterExecuteUpdate(TEntity entity);
        #endregion Update
        
        #region Delete
        /// <summary>
        /// Removes the entity from the repository with the appropriate identity.
        /// </summary>
        /// <param name="id">The identification.</param>
        public virtual async Task DeleteAsync(IdType id)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(DeleteAsync), id.ToString()).ConfigureAwait(false);
#endif
            TEntity? entity = await ExecuteGetByIdAsync(id).ConfigureAwait(false);
            
            if (entity != null)
            {
                ExecuteDelete(entity);
            }
        }
        /// <summary>
        /// Removes the entity from the repository with the appropriate identity (without authorization).
        /// </summary>
        /// <param name="id">The identification.</param>
        internal virtual void ExecuteDelete(TEntity entity)
        {
            ValidateEntity(ActionType.Delete, entity);
            BeforeActionExecute(ActionType.Delete, entity);
            BeforeExecuteDelete(entity);
            EntitySet.Remove(entity);
            AfterExecuteDelete(entity);
            AfterActionExecute(ActionType.Delete, entity);
        }
        /// <summary>
        /// This method is called before executing the delete operation on an entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity</typeparam>
        /// <param name="entity">The entity</param>
        partial void BeforeExecuteDelete(TEntity entity);
        /// <summary>
        /// Performs any necessary actions after executing a delete operation on a TEntity entity.
        /// </summary>
        /// <param name="entity">The TEntity entity that was deleted.</param>
        partial void AfterExecuteDelete(TEntity entity);
        #endregion Delete
        
        #region SaveChanges
        /// <summary>
        /// Saves any changes in the underlying persistence.
        /// </summary>
        /// <returns>The number of state entries written to the underlying database.</returns>
        public async Task<int> SaveChangesAsync()
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(SaveChangesAsync)).ConfigureAwait(false);
#endif
            return await ExecuteSaveChangesAsync().ConfigureAwait(false);
        }
        /// <summary>
        /// Saves any changes in the underlying persistence (without authorization).
        /// </summary>
        /// <returns>The number of state entries written to the underlying database.</returns>
        internal virtual async Task<int> ExecuteSaveChangesAsync()
        {
            var result = 0;
            
            if (Context != null)
            {
                BeforeActionExecute(ActionType.Save);
                BeforeExecuteSaveChanges();
                result = await Context.SaveChangesAsync().ConfigureAwait(false);
                AfterExecuteSaveChanges();
                AfterActionExecute(ActionType.Save);
            }
            return result;
        }
        /// <summary>
        /// This method is called before the execution of the SaveChanges() method.
        /// </summary>
        partial void BeforeExecuteSaveChanges();
        /// <summary>
        /// This method is called after the <see cref="SaveChanges"/> method is executed.
        /// </summary>
        /// <remarks>
        /// This method allows you to perform additional actions or tasks after the changes have been saved to the database.
        /// Only a part of this method can be implemented in a partial class.
        /// </remarks>
        partial void AfterExecuteSaveChanges();
        #endregion SaveChanges
        
        #region Helpers
        /// <summary>
        /// Checks the validity of the page parameters.
        /// </summary>
        /// <param name="pageIndex">The index of the page to check.</param>
        /// <param name="pageSize">The size of the page to check.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the pageIndex is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the pageSize is less than or equal to 0, or greater than MaxPageSize.</exception>
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
