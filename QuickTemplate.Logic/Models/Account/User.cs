//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Models.Account
{
    using TEntity = Entities.Account.User;
    using TModel = Models.Account.User;
    public partial class User : VersionExtendedModel
    {
        static User()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        public User()
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        new internal TEntity Source
        {
            get => (TEntity)(_source ??= new TEntity());
            set => _source = value;
        }
        public IdType IdentityId
        {
            get => Source.IdentityId;
            set => Source.IdentityId = value;
        }
        public String FirstName
        {
            get => Source.FirstName;
            set => Source.FirstName = value;
        }
        public String LastName
        {
            get => Source.LastName;
            set => Source.LastName = value;
        }

        internal void CopyProperties(TEntity other)
        {
            bool handled = false;
            BeforeCopyProperties(other, ref handled);
            if (handled == false)
            {
                Id = other.Id;
#if ROWVERSION_ON
                RowVersion = other.RowVersion;
#endif
                IdentityId = other.IdentityId;
                FirstName = other.FirstName;
                LastName = other.LastName;
            }
            AfterCopyProperties(other);
        }
        partial void BeforeCopyProperties(TEntity other, ref bool handled);
        partial void AfterCopyProperties(TEntity other);

        public void CopyProperties(TModel other)
        {
            bool handled = false;
            BeforeCopyProperties(other, ref handled);
            if (handled == false)
            {
                Id = other.Id;
#if ROWVERSION_ON
                RowVersion = other.RowVersion;
#endif
                IdentityId = other.IdentityId;
                FirstName = other.FirstName;
                LastName = other.LastName;
            }
            AfterCopyProperties(other);
        }
        partial void BeforeCopyProperties(TModel other, ref bool handled);
        partial void AfterCopyProperties(TModel other);

        public override bool Equals(object? obj)
        {
            bool result = false;
            if (obj is Models.Account.User other)
            {
                result = IsEqualsWith(IdentityId, other.IdentityId)
                && Id == other.Id;
            }
            return result;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(IdentityId, FirstName, LastName, Id);
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
            var result = new TModel();
            result.CopyProperties(other);
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