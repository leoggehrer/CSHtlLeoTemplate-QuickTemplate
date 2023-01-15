//@BaseCode
//MdStart
using System.Linq.Dynamic.Core;

namespace QuickTemplate.Logic.Controllers
{
#if ACCOUNT_ON
    using QuickTemplate.Logic.Modules.Account;
    using System.Linq.Dynamic.Core;
    using System.Linq.Expressions;
#endif
    partial class EntitiesController<TEntity, TOutModel>
    {
        #region Get
#if GUID_ON
        /// <summary>
        /// Returns the element of type T with the identification of id.
        /// </summary>
        /// <param name="id">The identification.</param>
        /// <returns>The element of the type T with the corresponding identification.</returns>
        public virtual async Task<TOutModel?> GetByGuidAsync(Guid id)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(GetByIdAsync), id.ToString()).ConfigureAwait(false);
#endif
            var result = await ExecuteGetByGuidAsync(id).ConfigureAwait(false);

            return result != null ? BeforeReturn(result) : null;
        }
        /// <summary>
        /// Returns the element of type T with the identification of id.
        /// </summary>
        /// <param name="id">The identification.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>The element of the type T with the corresponding identification (with includes).</returns>
        public virtual async Task<TOutModel?> GetByGuidAsync(Guid id, params string[] includeItems)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), nameof(GetByIdAsync), id.ToString()).ConfigureAwait(false);
#endif
            var result = await ExecuteGetByGuidAsync(id, includeItems).ConfigureAwait(false);

            return result != null ? BeforeReturn(result) : null;
        }
        /// <summary>
        /// Returns the element of type T with the identification of id (without authorization).
        /// </summary>
        /// <param name="id">The identification.</param>
        /// <param name="includeItems">The include items</param>
        /// <returns>The element of the type T with the corresponding identification.</returns>
        internal virtual Task<TEntity?> ExecuteGetByGuidAsync(Guid id, params string[] includeItems)
        {
            var query = EntitySet.AsQueryable();

            foreach (var includeItem in Includes.Union(includeItems).Distinct())
            {
                query = query.Include(includeItem);
            }
            return query.Where("Guid.Equals(@0)", new object[] { id }).FirstOrDefaultAsync();
        }
#endif
        #endregion Get

        #region Action
        protected virtual void HandleInsertExtendedProperties(TEntity entity)
        {
            if (entity is Entities.VersionExtendedEntity instance)
            {
#if GUID_ON
                if (instance.Guid == Guid.Empty)
                {
                    instance.Guid = Guid.NewGuid();
                }
#endif

#if CREATED_ON
                instance.CreatedOn = DateTime.UtcNow;
#endif

#if MODIFIED_ON
                instance.ModifiedOn = null;
#endif

#if ACCOUNT_ON && CREATEDBY_ON
                var curSession = AccountManager.QueryLoginSession(SessionToken);

                instance.IdentityId_CreatedBy = curSession?.IdentityId;
#endif

#if ACCOUNT_ON && MODIFIEDBY_ON
                instance.IdentityId_ModifiedBy = null;
#endif
            }
        }
        protected virtual void HandleUpdateExtendedProperties(TEntity entity)
        {
            if (entity is Entities.VersionExtendedEntity instance)
            {
#if MODIFIED_ON
                instance.ModifiedOn = DateTime.UtcNow;
#endif

#if ACCOUNT_ON && MODIFIEDBY_ON
                var curSession = AccountManager.QueryLoginSession(SessionToken);

                instance.IdentityId_ModifiedBy = curSession?.IdentityId;
#endif
            }
        }
        #endregion Action
    }
}
//MdEnd
