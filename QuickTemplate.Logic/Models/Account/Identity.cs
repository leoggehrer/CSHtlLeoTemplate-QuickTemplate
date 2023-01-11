//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Models.Account
{
    using TEntity = Entities.Account.Identity;
    using TModel = Models.Account.Identity;
    public partial class Identity : VersionExtendedModel, Contracts.Account.IIdentity
    {
        static Identity()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        public Identity()
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        new internal TEntity Source
        {
            get => (TEntity)(_source ??= new Entities.Account.SecureIdentity());
            set => _source = value;
        }
#if GUID_OFF
        public Guid Guid
        {
            get => Source.Guid;
            set => Source.Guid = value;
        }
#endif
        public System.String Name
        {
            get => Source.Name;
            set => Source.Name = value;
        }
        public System.String Email
        {
            get => Source.Email;
            set => Source.Email = value;
        }
        public System.Int32 TimeOutInMinutes
        {
            get => Source.TimeOutInMinutes;
            set => Source.TimeOutInMinutes = value;
        }
        public System.Int32 AccessFailedCount
        {
            get => Source.AccessFailedCount;
            set => Source.AccessFailedCount = value;
        }
        public Modules.Common.State State
        {
            get => Source.State;
            set => Source.State = value;
        }
        public Contracts.Account.IRole[] Roles => Source.Roles;

        public bool HasRole(Guid guid) => Source.HasRole(guid);

        internal void CopyProperties(TEntity other)
        {
            bool handled = false;
            BeforeCopyProperties(other, ref handled);
            if (handled == false)
            {
                Id = other.Id;
                Guid = other.Guid;
                Name = other.Name;
                Email = other.Email;
                TimeOutInMinutes = other.TimeOutInMinutes;
                AccessFailedCount = other.AccessFailedCount;
                State = other.State;
                CopyExendedProperties(other);
            }
            AfterCopyProperties(other);
        }
        partial void BeforeCopyProperties(TEntity other, ref bool handled);
        partial void AfterCopyProperties(TEntity other);

        internal void CopyProperties(TModel other)
        {
            bool handled = false;
            BeforeCopyProperties(other, ref handled);
            if (handled == false)
            {
                Id = other.Id;
                Guid = other.Guid;
                Name = other.Name;
                Email = other.Email;
                TimeOutInMinutes = other.TimeOutInMinutes;
                AccessFailedCount = other.AccessFailedCount;
                State = other.State;
                CopyExendedProperties(other);
            }
            AfterCopyProperties(other);
        }
        partial void BeforeCopyProperties(TModel other, ref bool handled);
        partial void AfterCopyProperties(TModel other);

        public override bool Equals(object? obj)
        {
            bool result = false;
            if (obj is TModel other)
            {
                result = IsEqualsWith(this.GetType().Name, other.GetType().Name) && Id == other.Id;
            }
            return result;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode(Guid, Name, Email, TimeOutInMinutes, AccessFailedCount, State);
        }

        public static TModel Create()
        {
            BeforeCreate();
            var result = new TModel();
            AfterCreate(result);
            return result;
        }
        public static TModel Create(object other)
        {
            BeforeCreate(other);
            CommonBase.Extensions.ObjectExtensions.CheckArgument(other, nameof(other));
            var result = new TModel();
            CommonBase.Extensions.ObjectExtensions.CopyFrom(result, other);
            AfterCreate(result, other);
            return result;
        }
        public static TModel Create(TModel other)
        {
            BeforeCreate(other);
            var result = new TModel();
            result.CopyProperties(other);
            AfterCreate(result, other);
            return result;
        }
        internal static TModel Create(TEntity other)
        {
            BeforeCreate(other);
            var result = new TModel
            {
                Source = other
            };
            AfterCreate(result, other);
            return result;
        }
        static partial void BeforeCreate();
        static partial void AfterCreate(TModel instance);
        static partial void BeforeCreate(object other);
        static partial void AfterCreate(TModel instance, object other);
        static partial void BeforeCreate(TModel other);
        static partial void AfterCreate(TModel instance, TModel other);
        static partial void BeforeCreate(TEntity other);
        static partial void AfterCreate(TModel instance, TEntity other);
    }
}
#endif
//MdEnd