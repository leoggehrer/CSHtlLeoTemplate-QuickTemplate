//@CodeCopy
//MdStart
namespace CommonBase.Services
{
    public abstract partial class ServiceObject : IDisposable
    {
        static ServiceObject()
        {
            BeforeClassInitialize();
            AfterClassInitialize();
        }
        static partial void BeforeClassInitialize();
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
        internal ServiceObject(string baseAddress, string requestUri)
            : this(string.Empty, baseAddress, requestUri)
        {
            Constructing();
            Constructed();
        }
        internal ServiceObject(string sessionToken, string baseAddress, string requestUri)
        {
            Constructing();
            SessionToken = sessionToken;
            BaseAddress = baseAddress;
            RequestUri = requestUri;
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        #endregion Instance-Constructors

        #region Dispose pattern
        private bool disposedValue;

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
