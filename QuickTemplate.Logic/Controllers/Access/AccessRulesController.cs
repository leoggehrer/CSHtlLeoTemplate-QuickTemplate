//@BaseCode
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
namespace QuickTemplate.Logic.Controllers.Access
{
    using TEntity = Entities.Access.AccessRule;
    using TOutModel = Models.Access.AccessRule;
    
    /// <summary>
    /// Represents a controller for managing access rules.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity</typeparam>
    /// <typeparam name="TOutModel">The type of the output model</typeparam>
    /// <remarks>
    /// This class is internally accessible and is declared as sealed and partial.
    /// </remarks>
    internal sealed partial class AccessRulesController : EntitiesController<TEntity, TOutModel>, Contracts.Access.IAccessRulesAccess
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessRulesController"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is called when the class is constructed.
        /// </remarks>
        static AccessRulesController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called during the construction of the class.
        /// </summary>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when the class is constructed.
        /// </summary>
        /// <remarks>
        /// This method is a partial method, which means it can be implemented in a partial class to add additional logic during object construction.
        /// </remarks>
        /// <seealso cref="Class"/>
        /// <seealso cref="partial"/>
        static partial void ClassConstructed();
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessRulesController"/> class.
        /// </summary>
        public AccessRulesController()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called during the construction of the object.
        /// </summary>
        /// <remarks>
        /// Use this method to perform additional initialization tasks or to execute code
        /// that should be run only during object construction.
        /// </remarks>
        /// <seealso cref="MyClass"/>
        partial void Constructing();
        /// <summary>
        /// Represents a method that is called when the object is constructed.
        /// </summary>
        /// <remarks>
        /// This method should be implemented in a partial class or struct.
        /// </remarks>
        partial void Constructed();
        /// <summary>
        /// Initializes a new instance of the AccessRulesController class using the specified ControllerObject.
        /// </summary>
        /// <param name="other">The ControllerObject to use as the base for this AccessRulesController.</param>
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

