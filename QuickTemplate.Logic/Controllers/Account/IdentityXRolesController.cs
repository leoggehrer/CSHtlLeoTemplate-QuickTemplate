//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Controllers.Account
{
    using TEntity = Entities.Account.IdentityXRole;
    using TOutModel = Models.Account.IdentityXRole;

    [Modules.Security.Authorize("SysAdmin", "AppAdmin")]
    internal sealed partial class IdentityXRolesController : EntitiesController<TEntity, TOutModel>, Contracts.Account.IRolesAccess<TOutModel>
    {
        public IdentityXRolesController()
        {
        }

        public IdentityXRolesController(ControllerObject other) : base(other)
        {
        }

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
