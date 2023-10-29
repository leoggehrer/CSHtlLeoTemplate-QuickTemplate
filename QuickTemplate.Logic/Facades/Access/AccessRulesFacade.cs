//@BaseCode
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
namespace QuickTemplate.Logic.Facades.Access
{
    using TOutModel = Models.Access.AccessRule;
    using TAccessContract = Contracts.Access.IAccessRulesAccess;
    
    /// <summary>
    /// Represents a facade for accessing and managing access rules.
    /// </summary>
    /// <typeparam name="TOutModel">The type of the output model for the access rules.</typeparam>
    /// <typeparam name="TAccessContract">The type of the access contract.</typeparam>
    public partial class AccessRulesFacade : ControllerFacade<TOutModel, TAccessContract>, TAccessContract
    {
        /// <summary>
        /// Initializes a new instance of the AccessRulesFacade class.
        /// </summary>
        public AccessRulesFacade()
        : base(new Controllers.Access.AccessRulesController())
        {
        }
        /// <summary>
        /// Initializes a new instance of the AccessRulesFacade class with the specified facade object.
        /// </summary>
        /// <param name="facadeObject">The facade object used to create the instance.</param>
        /// <remarks>
        /// The AccessRulesFacade class serves as a facade for accessing and managing access rules.
        /// This constructor initializes a new instance by creating an instance of the AccessRulesController class using the controller object
        /// from the specified facade object, and passing it to the base class constructor.
        /// </remarks>
        public AccessRulesFacade(FacadeObject facadeObject)
        : base(new Controllers.Access.AccessRulesController(facadeObject.ControllerObject))
        {
            
        }
    }
}
#endif
//MdEnd
