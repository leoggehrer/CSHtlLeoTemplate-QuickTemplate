//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Models.Account
{
    using TEntity = Entities.Account.IdentityXRole;
    using TModel = Models.Account.IdentityXRole;
    internal partial class IdentityXRole : VersionExtendedModel
    {
        static IdentityXRole()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        public IdentityXRole()
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
        public IdType RoleId
        {
            get => Source.RoleId;
            set => Source.RoleId = value;
        }
        public Models.Account.Identity? Identity
        {
            get => Source.Identity != null ? Models.Account.Identity.Create(Source.Identity) : null;
            set => Source.Identity = value?.Source;
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
                IdentityId= other.IdentityId;
                RoleId= other.RoleId;
                Identity = other.Identity != null ? Models.Account.Identity.Create((object)other.Identity) : null;
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
#if ROWVERSION_ON
                RowVersion = other.RowVersion;
#endif
                IdentityId = other.IdentityId;
                RoleId = other.RoleId;
                Identity = other.Identity != null ? Models.Account.Identity.Create((object)other.Identity) : null;
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
            return HashCode.Combine(IdentityId, RoleId);
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
