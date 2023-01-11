//@BaseCode
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
namespace QuickTemplate.Logic.Facades.Access
{
    using TOutModel = Models.Access.AccessRule;
    public partial class AccessRulesFacade : ControllerFacade<TOutModel>, Contracts.Access.IAccessRulesAccess<TOutModel>
    {
        public AccessRulesFacade() 
            : base(new Controllers.Access.AccessRulesController())
        {
        }
        public AccessRulesFacade(FacadeObject facadeObject)
            : base(new Controllers.Access.AccessRulesController(facadeObject.ControllerObject))
        {

        }
    }
}
#endif
//MdEnd