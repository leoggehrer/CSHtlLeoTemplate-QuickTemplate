//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Controllers.Account
{
    using TEntity = Entities.Account.LoginSession;
    using TOutModel = Models.Account.LoginSession;
    
    /// <summary>
    /// Represents a controller for managing login sessions.
    /// </summary>
    [Modules.Security.Authorize("SysAdmin", "AppAdmin")]
    internal sealed partial class LoginSessionsController : EntitiesController<TEntity, TOutModel>, Contracts.Account.ILoginSessionsAccess
    {
        /// <summary>
        /// Initializes the <see cref="LoginSessionsController"/> class.
        /// </summary>
        static LoginSessionsController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called before the class is constructed.
        /// </summary>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when the class is constructed.
        /// </summary>
        static partial void ClassConstructed();
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginSessionsController"/> class.
        /// </summary>
        public LoginSessionsController()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called during the construction of an object.
        /// </summary>
        /// <remarks>
        /// This method is meant to be implemented in a partial class. It is called before the object is fully constructed.
        /// Any initialization or pre-construction logic can be put in this method.
        /// </remarks>
        partial void Constructing();
        /// <summary>
        /// This method is called after the object is constructed.
        /// It can be overridden in a partial class to add custom initialization logic.
        /// </summary>
        /// <remarks>
        /// This method is invoked before the object is fully initialized.
        /// It should not be called directly, but rather should be overridden in a partial class.
        /// </remarks>
        partial void Constructed();
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginSessionsController"/> class with a specified <paramref name="other"/> controller object.
        /// </summary>
        /// <param name="other">The other controller object.</param>
        public LoginSessionsController(ControllerObject other)
        : base(other)
        {
            Constructing();
            Constructed();
        }
        
        /// <summary>
        /// Executes actions before executing the specified action type in the application.
        /// </summary>
        /// <param name="actionType">The type of action being executed.</param>
        /// <param name="entity">The login session entity associated with the action.</param>
        /// <remarks>
        /// This method is called before executing the specified action type. It updates the login session entity by 
        /// setting the session token and login time. The session token is generated using the combination of 
        /// two unique identifiers. The login time and last access time are set to the current UTC time.
        /// </remarks>
        protected override void BeforeActionExecute(ActionType actionType, Entities.Account.LoginSession entity)
        {
            if (actionType == ActionType.Insert)
            {
                entity.SessionToken = $"{Guid.NewGuid()}-{Guid.NewGuid()}";
                entity.LoginTime = entity.LastAccess = DateTime.UtcNow;
            }
            base.BeforeActionExecute(actionType, entity);
        }
        /// <summary>
        /// Queries the database for open login sessions asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an array of TEntity.</returns>
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
