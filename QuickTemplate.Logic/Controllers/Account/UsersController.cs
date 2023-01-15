//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Controllers.Account
{
    using EntityUser = Entities.Account.User;
    using OutModelUser = Models.Account.User;

    [Modules.Security.Authorize("SysAdmin", "AppAdmin")]
    internal sealed partial class UsersController : EntitiesController<EntityUser, OutModelUser>, Contracts.Account.IUsersAccess<OutModelUser>
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
