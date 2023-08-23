//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Controllers.Account
{
    using TEntity = Entities.Account.User;
    using TOutModel = Models.Account.User;

    [Modules.Security.Authorize("SysAdmin", "AppAdmin")]
    internal sealed partial class UsersController : EntitiesController<TEntity, TOutModel>, Contracts.Account.IUsersAccess
    {
        public UsersController()
        {
        }

        public UsersController(ControllerObject other) : base(other)
        {
        }
    }
}
#endif
//MdEnd
