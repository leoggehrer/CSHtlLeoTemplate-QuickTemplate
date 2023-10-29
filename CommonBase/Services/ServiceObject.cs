//@CodeCopy
//MdStart
namespace CommonBase.Services
{
    /// <summary>
    /// Represents a base class for service objects.
    /// </summary>
    /// <remarks>
    /// This class provides common functionality for service objects, such as setting the session token, base address, and request URI
    /// required for making requests to a service.
    /// </remarks>
    /// <seealso cref="IDisposable"/>
    public abstract partial class ServiceObject : IDisposable
    {
        /// <summary>
        /// This is a static constructor for the ServiceObject class.
        /// It performs some initialization tasks before and after class initialization.
        /// </summary>
        static ServiceObject()
        {
            BeforeClassInitialize();
            AfterClassInitialize();
        }
        /// This method is a partial method that is automatically called before the initialization of the class.
        /// It can be implemented in partial classes to perform any necessary operations before the class is initialized.
        static partial void BeforeClassInitialize();
        /// <summary>
        /// This method is called after the initialization of the class.
        /// It is a partial method, which means it can be implemented in separate partial classes.
        /// </summary>
        static partial void AfterClassInitialize();
        
        /// <summary>
        /// Sets the session token.
        /// </summary>
        public string SessionToken { protected get; set; }
        /// <summary>
        /// Sets the base address like https://localhost:7085/api
        /// </summary>
        public string BaseAddress { get; init; }
        /// <summary>
        /// Sets the request uri (controller name)
        /// </summary>
        public string RequestUri { get; init; }
        
#region Instance-Constructors
        /// <summary>
        /// Initializes a new instance of the ServiceObject class with the specified base address and request URI.
        /// </summary>
        /// <param name="baseAddress">The base address of the service.</param>
        /// <param name="requestUri">The request URI of the service.</param>
        internal ServiceObject(string baseAddress, string requestUri)
        : this(string.Empty, baseAddress, requestUri)
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// Initializes a new instance of the ServiceObject class with the specified session token, base address, and request URI.
        /// </summary>
        /// <param name="sessionToken">The session token used for authentication.</param>
        /// <param name="baseAddress">The base address of the service.</param>
        /// <param name="requestUri">The request URI to be used.</param>
        internal ServiceObject(string sessionToken, string baseAddress, string requestUri)
        {
            Constructing();
            SessionToken = sessionToken;
            BaseAddress = baseAddress;
            RequestUri = requestUri;
            Constructed();
        }
        /// <summary>
        /// This method is called during the construction of the object before any other logic is executed.
        /// </summary>
        /// <remarks>
        /// This method can be implemented in a partial class to provide custom construction logic.
        /// </remarks>
        /// <seealso cref="SomeClass"/>
        partial void Constructing();
        /// <summary>
        /// This method is called after the object is constructed.
        /// It can be used to initialize any required fields or perform additional setup.
        /// This method is marked as partial, meaning it can be implemented in multiple files.
        /// </summary>
        partial void Constructed();
#endregion Instance-Constructors
        
#region Dispose pattern
        private bool disposedValue;
        
        /// <summary>
        /// Releases the resources used by the object.
        /// </summary>
        /// <param name="disposing">A flag indicating whether to dispose of managed objects.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                }
                disposedValue = true;
            }
        }
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
#endregion Dispose pattern
    }
}
//MdEnd


