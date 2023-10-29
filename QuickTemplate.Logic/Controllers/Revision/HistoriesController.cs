//@BaseCode
//MdStart
#if ACCOUNT_ON && REVISION_ON
namespace QuickTemplate.Logic.Controllers.Revision
{
    using TEntity = Entities.Revision.History;
    using TOutModel = Models.Revision.History;
    
    /// <summary>
    /// Represents a controller for accessing histories. This class is internal and cannot be inherited.
    /// </summary>
    internal sealed partial class HistoriesController : EntitiesController<TEntity, TOutModel>, Contracts.Revision.IHistoriesAccess<TOutModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoriesController"/> class.
        /// </summary>
        static HistoriesController()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called when constructing an instance of the class.
        /// </summary>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when a class is constructed.
        /// </summary>
        static partial void ClassConstructed();
        /// <summary>
        /// Initializes a new instance of the HistoriesController class.
        /// </summary>
        /// <remarks>
        /// This constructor calls the Constructing and Constructed methods.
        /// </remarks>
        public HistoriesController()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called during the construction of the object.
        /// </summary>
        /// <remarks>
        /// This is a partial method, which means it may or may not be implemented.
        /// If implemented, it will be called automatically during the construction of the object.
        /// </remarks>
        partial void Constructing();
        /// <summary>
        /// This method is called when the object has been fully constructed but before it is fully initialized.
        /// </summary>
        /// <remarks>
        /// This method is declared with the 'partial' keyword, allowing it to be implemented in another part of the partial class.
        /// </remarks>
        partial void Constructed();
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoriesController"/> class.
        /// </summary>
        /// <param name="other">The base controller object.</param>
        /// <remarks>
        /// This constructor is used to create a new instance of the HistoriesController class, which inherits from the base controller object.
        /// It calls the Constructing method before construction and the Constructed method after construction.
        /// </remarks>
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

