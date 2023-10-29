//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Facades.Account
{
    using TOutModel = Models.Account.Role;
    using TAccessContract = Contracts.Account.IRolesAccess;
    
    /// <summary>
    /// Represents a facade for managing roles.
    /// </summary>
    /// <typeparam name="TOutModel">The type of the output model.</typeparam>
    /// <typeparam name="TAccessContract">The type of the access contract.</typeparam>
    /// <remarks>
    /// This class inherits from the <see cref="ControllerFacade{TOutModel, TAccessContract}"/> and <see cref="TAccessContract"/> interfaces.
    /// </remarks>
    public sealed partial class RolesFacade : ControllerFacade<TOutModel, TAccessContract>, TAccessContract
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RolesFacade"/> class.
        /// </summary>
        /// <remarks>
        /// The RolesFacade class is responsible for managing roles.
        /// </remarks>
        public RolesFacade()
        : base(new Controllers.Account.RolesController())
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RolesFacade"/> class.
        /// </summary>
        /// <param name="facadeObject">The facade object.</param>
        public RolesFacade(FacadeObject facadeObject)
        : base(new Controllers.Account.RolesController(facadeObject.ControllerObject))
        {
            
        }
    }
}
#endif
//MdEnd
