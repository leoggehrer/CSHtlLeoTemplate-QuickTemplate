//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Facades.Account
{
    using TOutModel = Models.Account.User;

    public sealed partial class UsersFacade : ControllerFacade<TOutModel>, Contracts.Account.IUsersAccess
    {
        public UsersFacade()
            : base(new Controllers.Account.UsersController())
        {
        }
        public UsersFacade(FacadeObject facadeObject)
            : base(new Controllers.Account.UsersController(facadeObject.ControllerObject))
        {

        }
    }
}
#endif
//MdEnd
