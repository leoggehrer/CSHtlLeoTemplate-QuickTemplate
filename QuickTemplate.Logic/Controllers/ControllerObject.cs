//@BaseCode
//MdStart
namespace QuickTemplate.Logic.Controllers
{
    using QuickTemplate.Logic.DataContext;
    /// <summary>
    /// Represents an abstract base class for controller objects that implement IDisposable interface.
    /// </summary>
    /// <remarks>
    /// This class provides a common implementation for creating and disposing controller objects.
    /// </remarks>
    public abstract partial class ControllerObject : IDisposable
    {
        /// <summary>
        /// Represents a static class that contains methods and actions related to the ControllerObject.
        /// </summary>
        /// <summary>
        /// Initializes the ControllerObject class before any of its members are accessed.
        /// </summary>
        static ControllerObject()
        {
            BeforeClassInitialize();
            AfterClassInitialize();
        }
        /// <summary>
        /// This method is called before the initialization of the class.
        /// </summary>
        /// <remarks>
        /// Use this method to perform any setup tasks that are required
        /// before the class is initialized.
        /// </remarks>
        static partial void BeforeClassInitialize();
        /// <summary>
        /// This method is called after the class has been initialized.
        /// </summary>
        /// <remarks>
        /// This method is a partial method, which means it is implemented in another part of the class,
        /// allowing for custom logic to be executed after the class initialization.
        /// </remarks>
        static partial void AfterClassInitialize();
        
        #region Fields
        private readonly bool contextOwner;
        #endregion Fields
        
        #region Properties
        /// <summary>
        /// Gets or sets the instance of the internal ProjectDbContext.
        /// </summary>
        /// <value>
        /// The instance of the internal ProjectDbContext. Nullable.
        /// </value>
        internal ProjectDbContext? Context { get; private set; }
        #endregion Properties
        
        #region Instance-Constructors
        /// <summary>
        /// Internal constructor for the ControllerObject class.
        /// </summary>
        /// <param name="context">The ProjectDbContext object used by the ControllerObject.</param>
        /// <remarks>
        /// This constructor initializes the ControllerObject with the provided ProjectDbContext context.
        /// </remarks>
        internal ControllerObject(ProjectDbContext context)
        {
            Constructing(context);
            
            contextOwner = true;
            Context = context;
            
            ConstructingSecurityPart(context);
            ConstructingAccessPart(context);
            
            ConstructedAccessPart();
            ConstructedSecurityPart();
            
            Constructed();
        }
        /// <summary>
        /// Creates a new instance of the <see cref="ControllerObject"/> class with the data from another <see cref="ControllerObject"/> object.
        /// </summary>
        /// <param name="other">The other <see cref="ControllerObject"/> object to copy data from.</param>
        /// <exception cref="Modules.Exceptions.LogicException">Thrown when the context from the other controller is null.</exception>
        /// <remarks>
        /// This constructor is used to create a new <see cref="ControllerObject"/> object by copying the data from another <see cref="ControllerObject"/> object.
        /// It ensures that the context from the other controller is not null and then calls the necessary constructing methods.
        /// It sets the <see cref="contextOwner"/> flag to false and assigns the context from the other controller to the current controller's context.
        /// It then calls the constructing methods for the security part and access part, followed by the constructing methods for the access part and security part.
        /// Finally, it calls the constructed method to complete the construction process.
        /// </remarks>
        internal ControllerObject(ControllerObject other)
        {
            if (other.Context == null)
            throw new Modules.Exceptions.LogicException("The context from the other controller must not be null.");
            
            Constructing(other);
            
            contextOwner = false;
            Context = other.Context;
            
            ConstructingSecurityPart(other);
            ConstructingAccessPart(other);
            
            ConstructedAccessPart();
            ConstructedSecurityPart();
            
            Constructed();
        }
        /// <summary>
        /// This method is called during the construction of the ProjectDbContext.
        /// It can be used to perform additional operations or configurations on the context before it is fully constructed.
        /// </summary>
        /// <param name="context">The ProjectDbContext being constructed.</param>
        partial void Constructing(ProjectDbContext context);
        /// <summary>
        /// This method is called during the construction of the ControllerObject and allows for additional initialization based on another ControllerObject.
        /// </summary>
        /// <param name="other">The other ControllerObject to base the initialization on.</param>
        /// <remarks>
        /// This method is implemented in a partial class and is meant to be completed in another partial class along with the rest of the Constructor logic.
        /// It is called after the actual construction of the ControllerObject, providing an opportunity to perform additional setup based on the other ControllerObject passed as an argument.
        /// </remarks>
        partial void Constructing(ControllerObject other);
        /// <summary>
        /// Constructs the security part of a project.
        /// </summary>
        /// <remarks>
        /// This method is called during project construction to handle security-related tasks in the ProjectDbContext.
        /// </remarks>
        /// <param name="context">The ProjectDbContext object used for project construction.</param>
        partial void ConstructingSecurityPart(ProjectDbContext context);
        /// <summary>
        /// Constructs the security part for the ControllerObject by using the provided other ControllerObject.
        /// </summary>
        /// <param name="other">The other ControllerObject to use for constructing the security part.</param>
        partial void ConstructingSecurityPart(ControllerObject other);
        /// <summary>
        /// Constructs the access part for a specific project database context.
        /// </summary>
        /// <param name="context">The project database context.</param>
        partial void ConstructingAccessPart(ProjectDbContext context);
        /// <summary>
        /// Constructs the access part of the ControllerObject using the given other ControllerObject as a reference.
        /// </summary>
        /// <param name="other">The other ControllerObject to use as a reference.</param>
        /// <remarks>
        /// The access part of the ControllerObject refers to the permissions and rights the object has in a specific context.
        /// This method constructs the access part of the ControllerObject by taking the access information from the other ControllerObject
        /// and using it as a reference.
        /// Only a part of the ControllerObject is constructed using this method, the rest of the object should be constructed separately.
        /// </remarks>
        partial void ConstructingAccessPart(ControllerObject other);
        /// <summary>
        /// This method is called after an object is constructed.
        /// </summary>
        partial void Constructed();
        ///<summary>
        ///The ConstructedSecurityPart method is used to create a security part.
        ///</summary>
        /// <remarks>
        /// This method is only executed when the partial class is implemented.
        ///</remarks>
        partial void ConstructedSecurityPart();
        /// <summary>
        /// This method handles the constructed access part.
        ///</summary>
        /// <remarks>
        /// This method is implemented as a partial method.
        /// Partial methods enable the implementation of a method to be split across multiple source files.
        /// The implementation of this method resides in a separate source file.
        ///</remarks>
        partial void ConstructedAccessPart();
        #endregion Instance-Constructors
        
        #region Dispose pattern
        private bool disposedValue;
        
        /// <summary>
        /// Disposes of the object and releases any resources used.
        /// </summary>
        /// <param name="disposing">
        /// A boolean value representing whether the method is being called from a disposing method.
        /// </param>
        /// <remarks>
        /// This method releases the security and access parts of the object if disposing is true.
        /// If the object owns the context, it is disposed of and set to null.
        /// </remarks>
        /// <returns>
        /// No value is returned.
        /// </returns>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeSecurityPart();
                    DisposeAccessPart();
                    
                    if (contextOwner)
                    {
                        Context?.Dispose();
                    }
                    Context = null;
                }
                disposedValue = true;
            }
        }
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        ///<summary>
        /// This method is used to dispose the security part of the object.
        ///</summary>
        ///<remarks>
        /// This method is called when the object needs to be disposed of. It is responsible for disposing any resources related to the security part of the object.
        ///</remarks>
        /// <seealso cref="Dispose"/>
        partial void DisposeSecurityPart();
        /// <summary>
        /// Performs any necessary cleanup operations for the DisposeAccessPart method.
        /// This method is called when the Dispose method is called.
        /// </summary>
        /// <remarks>
        /// This method should be implemented by a derived class to release any resources
        /// used by the DisposeAccessPart method.
        /// </remarks>
        partial void DisposeAccessPart();
        #endregion Dispose pattern
    }
}
//MdEnd
