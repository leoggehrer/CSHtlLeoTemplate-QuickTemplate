//@CodeCopy
//MdStart
#if ACCOUNT_ON && LOGGING_ON
namespace QuickTemplate.Logic.Controllers.Logging
{
    using TEntity = Entities.Logging.ActionLog;
    using TOutModel = Models.Logging.ActionLog;

    internal sealed partial class ActionLogsController : EntitiesController<TEntity, TOutModel>, Contracts.Logging.IActionLogsAccess<TOutModel>
    {
        static ActionLogsController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        public ActionLogsController()
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        public ActionLogsController(ControllerObject other)
        : base(other)
        {
            Constructing();
            Constructed();
        }
    }
}
#endif
//MdEnd
