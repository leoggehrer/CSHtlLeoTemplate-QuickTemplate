//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Controllers.Account
{
    using TEntity = Entities.Account.User;
    using TOutModel = Models.Account.User;
    
    /// <summary>
    /// Represents a controller for managing users.
    /// </summary>
    /// <remarks>
    /// This class is restricted to only be accessed by users with "SysAdmin" and "AppAdmin" roles.
    /// </remarks>
    [Modules.Security.Authorize("SysAdmin", "AppAdmin")]
    internal sealed partial class UsersController : EntitiesController<TEntity, TOutModel>, Contracts.Account.IUsersAccess
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        public UsersController()
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class with a specified <paramref name="other"/> <see cref="ControllerObject"/>.
        /// </summary>
        /// <param name="other">The <see cref="ControllerObject"/> instance to be passed to the base constructor.</param>
        public UsersController(ControllerObject other) : base(other)
        {
        }
    }
}
#endif
//MdEnd
