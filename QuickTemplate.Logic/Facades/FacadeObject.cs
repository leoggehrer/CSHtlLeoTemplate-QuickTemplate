//@BaseCode
//MdStart

using QuickTemplate.Logic.Controllers;

namespace QuickTemplate.Logic.Facades
{
    public abstract partial class FacadeObject : IDisposable
    {
        internal ControllerObject ControllerObject { get; private set; }

        protected FacadeObject(ControllerObject controllerObject)
        {
            ControllerObject = controllerObject;
        }

        #region Dispose pattern
        private bool disposedValue;

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
