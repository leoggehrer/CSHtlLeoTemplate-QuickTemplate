//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Facades.Account
{
    using TOutModel = Models.Account.Role;
    public sealed partial class RolesFacade : ControllerFacade<TOutModel>, Contracts.Account.IRolesAccess<TOutModel>
    {
        public RolesFacade()
            : base(new Controllers.Account.RolesController())
        {
        }
        public RolesFacade(FacadeObject facadeObject)
            : base(new Controllers.Account.RolesController(facadeObject.ControllerObject))
        {

        }
    }
}
#endif
//MdEnd
