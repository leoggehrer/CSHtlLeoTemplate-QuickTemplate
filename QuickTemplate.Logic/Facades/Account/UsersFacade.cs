//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Facades.Account
{
    using TOutModel = Models.Account.User;
    using TAccessContract = Contracts.Account.IUsersAccess;
    /// <summary>
    /// Represents a facade for managing user accounts.
    /// </summary>
    /// <typeparam name="TOutModel">The type of the output model.</typeparam>
    /// <typeparam name="TAccessContract">The type of the access contract.</typeparam>
    public sealed partial class UsersFacade : ControllerFacade<TOutModel, TAccessContract>, TAccessContract
    {
        /// <summary>
        /// Represents a facade for managing users.
        /// </summary>
        /// <remarks>
        /// This class acts as a wrapper around the UsersController class to provide a simplified interface for managing users.
        /// </remarks>
        public UsersFacade()
        : base(new Controllers.Account.UsersController())
        {
        }
        /// <summary>
        /// Initializes a new instance of the UsersFacade class with the specified FacadeObject.
        /// </summary>
        /// <param name="facadeObject">The FacadeObject to use for creating the UsersController.</param>
        public UsersFacade(FacadeObject facadeObject)
        : base(new Controllers.Account.UsersController(facadeObject.ControllerObject))
        {
            
        }
    }
}
#endif
//MdEnd
