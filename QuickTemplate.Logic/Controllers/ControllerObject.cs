//@BaseCode
//MdStart
namespace QuickTemplate.Logic.Controllers
{
    using QuickTemplate.Logic.DataContext;
    public abstract partial class ControllerObject : IDisposable
    {
        static ControllerObject()
        {
            BeforeClassInitialize();
            AfterClassInitialize();
        }
        static partial void BeforeClassInitialize();
        static partial void AfterClassInitialize();

        #region Fields
        private readonly bool contextOwner;
        #endregion Fields

        #region Properties
        internal ProjectDbContext? Context { get; private set; }
        #endregion Properties

        #region Instance-Constructors
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
        partial void Constructing(ProjectDbContext context);
        partial void Constructing(ControllerObject other);
        partial void ConstructingSecurityPart(ProjectDbContext context);
        partial void ConstructingSecurityPart(ControllerObject other);
        partial void ConstructingAccessPart(ProjectDbContext context);
        partial void ConstructingAccessPart(ControllerObject other);
        partial void Constructed();
        partial void ConstructedSecurityPart();
        partial void ConstructedAccessPart();
        #endregion Instance-Constructors

        #region Dispose pattern
        private bool disposedValue;

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

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        partial void DisposeSecurityPart();
        partial void DisposeAccessPart();
        #endregion Dispose pattern
    }
}
//MdEnd
