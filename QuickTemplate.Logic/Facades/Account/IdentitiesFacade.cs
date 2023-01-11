//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Facades.Account
{
    using TOutModel = Models.Account.Identity;
    public sealed partial class IdentitiesFacade : ControllerFacade<TOutModel>, Contracts.Account.IIdentitiesAccess<TOutModel>
    {
        new private Contracts.Account.IIdentitiesAccess<TOutModel> Controller => (ControllerObject as Contracts.Account.IIdentitiesAccess<TOutModel>)!;

        public IdentitiesFacade()
            : base(new Controllers.Account.IdentitiesController())
        {
        }
        public IdentitiesFacade(FacadeObject facadeObject)
            : base(new Controllers.Account.IdentitiesController(facadeObject.ControllerObject))
        {

        }

        public Task AddRoleAsync(IdType id, IdType roleId)
        {
            return Controller.AddRoleAsync(id, roleId);
        }
        public Task RemoveRoleAsync(IdType id, IdType roleId)
        {
            return Controller.RemoveRoleAsync(id, roleId);
        }
    }
}
#endif
//MdEnd