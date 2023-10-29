//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Controllers.Account
{
    using TEntity = Entities.Account.IdentityXRole;
    using TOutModel = Models.Account.IdentityXRole;

    /// <summary>
    /// Represents a controller for handling identity and roles.
    /// </summary>
    [Modules.Security.Authorize("SysAdmin", "AppAdmin")]
    internal sealed partial class IdentityXRolesController : EntitiesController<TEntity, TOutModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityXRolesController"/> class.
        /// </summary>
        public IdentityXRolesController()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityXRolesController"/> class.
        /// </summary>
        /// <param name="other">An other controller object.</param>
        public IdentityXRolesController(ControllerObject other) : base(other)
        {
        }
        
        /// <summary>
        /// Queries and retrieves an array of entities of type TEntity asynchronously based on the specified identity ID.
        /// </summary>
        /// <param name="identityId">The ID used to query the entities.</param>
        /// <returns>A task representing the asynchronous operation. The task result is an array of entities of type TEntity.</returns>
        internal Task<TEntity[]> QueryByIdentityAsync(IdType identityId)
        {
            var query = EntitySet.AsQueryable();
            
            query = query.Include(e => e.Role);
            query = query.Where(e => e.IdentityId == identityId);
            
            return query.AsNoTracking().ToArrayAsync();
        }
    }
}
#endif
//MdEnd
