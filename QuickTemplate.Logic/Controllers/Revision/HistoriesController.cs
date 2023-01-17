//@CodeCopy
//MdStart
#if ACCOUNT_ON && REVISION_ON
namespace QuickTemplate.Logic.Controllers.Revision
{
    using TEntity = Entities.Revision.History;
    using TOutModel = Models.Revision.History;

    internal sealed partial class HistoriesController : EntitiesController<TEntity, TOutModel>, Contracts.Revision.IHistoriesAccess<TOutModel>
    {
        static HistoriesController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        public HistoriesController()
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        public HistoriesController(ControllerObject other)
        : base(other)
        {
            Constructing();
            Constructed();
        }
    }
}
#endif
//MdEnd
