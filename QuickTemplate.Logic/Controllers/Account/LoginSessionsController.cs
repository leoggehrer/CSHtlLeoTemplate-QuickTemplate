//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Controllers.Account
{
    using TEntity = Entities.Account.LoginSession;
    using TOutModel = Models.Account.LoginSession;

    [Modules.Security.Authorize("SysAdmin", "AppAdmin")]
    internal sealed partial class LoginSessionsController : EntitiesController<TEntity, TOutModel>, Contracts.Account.ILoginSessionsAccess
    {
        static LoginSessionsController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        public LoginSessionsController()
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        public LoginSessionsController(ControllerObject other)
        : base(other)
        {
            Constructing();
            Constructed();
        }

        protected override void BeforeActionExecute(ActionType actionType, Entities.Account.LoginSession entity)
        {
            if (actionType == ActionType.Insert)
            {
                entity.SessionToken = $"{Guid.NewGuid()}-{Guid.NewGuid()}";
                entity.LoginTime = entity.LastAccess = DateTime.UtcNow;
            }
            base.BeforeActionExecute(actionType, entity);
        }
        public Task<TEntity[]> QueryOpenLoginSessionsAsync()
        {
            return EntitySet.Where(e => e.LogoutTime.HasValue == false)
                            .Include(e => e.Identity)
                            .ToArrayAsync();
        }
    }
}
#endif
//MdEnd
