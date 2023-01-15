//@CodeCopy
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
namespace QuickTemplate.Logic.Controllers.Access
{
    using TEntity = Entities.Access.AccessRule;
    using TOutModel = Models.Access.AccessRule;

    internal sealed partial class AccessRulesController : EntitiesController<TEntity, TOutModel>, Contracts.Access.IAccessRulesAccess<TOutModel>
    {
        static AccessRulesController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        public AccessRulesController()
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        public AccessRulesController(ControllerObject other)
        : base(other)
        {
            Constructing();
            Constructed();
        }
    }
}
#endif
//MdEnd
