//@BaseCode
//MdStart
#if ACCOUNT_ON && LOGGING_ON
namespace QuickTemplate.Logic.Controllers.Logging
{
    using TEntity = Entities.Logging.ActionLog;
    using TOutModel = Models.Logging.ActionLog;
    
    /// <summary>
    /// This class represents a controller for action logs, providing access to logging functionality.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that the controller operates on.</typeparam>
    /// <typeparam name="TOutModel">The type of the output model.</typeparam>
    internal sealed partial class ActionLogsController : EntitiesController<TEntity, TOutModel>, Contracts.Logging.IActionLogsAccess<TOutModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionLogsController"/> class.
        /// </summary>
        static ActionLogsController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called before the class is constructed.
        /// </summary>
        /// <remarks>
        /// This is a partial method and can be implemented in the partial class definition.
        /// </remarks>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when the class is constructed.
        /// </summary>
        static partial void ClassConstructed();
        /// <summary>
        /// Constructs a new instance of the <see cref="ActionLogsController"/> class.
        /// </summary>
        public ActionLogsController()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// Invoked during the construction of the object.
        /// This method can be implemented in a partial class and will be called automatically by the compiler.
        /// </summary>
        partial void Constructing();
        /// <summary>
        /// This method is called when the object is constructed.
        /// </summary>
        partial void Constructed();
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionLogsController"/> class with the specified <paramref name="other"/> ControllerObject.
        /// </summary>
        /// <param name="other">The ControllerObject to be passed as a parameter to the base class constructor.</param>
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

