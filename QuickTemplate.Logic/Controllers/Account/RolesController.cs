//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Controllers.Account
{
    using TEntity = Entities.Account.Role;
    using TOutModel = Models.Account.Role;
    
    /// <summary>
    /// Represents a controller for managing roles within the application.
    /// </summary>
    /// <remarks>
    /// This class is marked as internal and sealed, meaning it cannot be inherited from or instantiated outside of the assembly.
    /// </remarks>
    /// <seealso cref="EntitiesController{TEntity, TOutModel}"/>
    /// <seealso cref="Contracts.Account.IRolesAccess"/>
    [Modules.Security.Authorize("SysAdmin", "AppAdmin")]
    internal sealed partial class RolesController : EntitiesController<TEntity, TOutModel>, Contracts.Account.IRolesAccess
    {
        /// <summary>
        /// Represents a controller for managing roles.
        /// </summary>
        public RolesController()
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RolesController"/> class with the specified <paramref name="other"/> controller object.
        /// </summary>
        /// <param name="other">The controller object to use as a base.</param>
        public RolesController(ControllerObject other) : base(other)
        {
        }
        
        /// <summary>
        /// Executes actions before executing the main action.
        /// </summary>
        /// <param name="actionType">The type of action being executed.</param>
        /// <param name="entity">The entity being processed.</param>
        protected override void BeforeActionExecute(ActionType actionType, TEntity entity)
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
