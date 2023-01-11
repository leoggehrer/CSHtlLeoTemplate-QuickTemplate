//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Controllers.Account
{
    using EntityRole = Entities.Account.Role;
    using OutModelRole = Models.Account.Role;

    [Modules.Security.Authorize("SysAdmin", "AppAdmin")]
    internal sealed partial class RolesController : EntitiesController<EntityRole, OutModelRole>, Contracts.Account.IRolesAccess<OutModelRole>
    {
        public RolesController()
        {
        }

        public RolesController(ControllerObject other) : base(other)
        {
        }

        protected override void BeforeActionExecute(ActionType actionType, EntityRole entity)
        {
            if (actionType == ActionType.Insert)
            {
                entity.Guid = Guid.NewGuid();
            }
            else if (actionType == ActionType.Update)
            {
                using var ctrl = new RolesController();
                var dbEntity = ctrl.EntitySet.Find(entity.Id);

                if (dbEntity != null)
                {
                    entity.Guid = dbEntity.Guid;
                }
            }
            base.BeforeActionExecute(actionType, entity);
        }
    }
}
#endif
//MdEnd