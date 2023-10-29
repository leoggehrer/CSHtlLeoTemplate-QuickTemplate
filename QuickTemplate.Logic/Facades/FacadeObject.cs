//@BaseCode
//MdStart

using QuickTemplate.Logic.Controllers;

namespace QuickTemplate.Logic.Facades
{
    /// <summary>
    /// Base class for facade objects that interact with a controller object.
    /// </summary>
    /// <remarks>
    /// This class provides a convenient way to interact with the ControllerObject
    /// and handle the cleanup of resources.
    /// </remarks>
    /// <seealso cref="System.IDisposable" />
    public abstract partial class FacadeObject : IDisposable
    {
        /// <summary>
        /// Gets or sets the ControllerObject associated with this object.
        /// </summary>
        /// <value>
        /// The ControllerObject associated with this object.
        /// </value>
        internal ControllerObject ControllerObject { get; private set; }
        
#if ACCOUNT_ON
        #region SessionToken
        /// <summary>
        /// Sets the authorization token.
        /// </summary>
        public string SessionToken
        {
            set
            {
                ControllerObject.SessionToken = value;
            }
        }
        #endregion SessionToken
#endif
        /// <summary>
        /// Initializes a new instance of the FacadeObject class.
        /// </summary>
        /// <param name="controllerObject">The controllerObject parameter.</param>
        protected FacadeObject(ControllerObject controllerObject)
        {
            ControllerObject = controllerObject;
        }
        
        #region Dispose pattern
        private bool disposedValue;
        
        /// <summary>
        /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ControllerObject.Dispose();
                }
                disposedValue = true;
            }
        }
        
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// @remarks Call this method to free any unmanaged resources held by the object. This method should be called when the object is no longer needed, so that the resources can be properly released.
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
