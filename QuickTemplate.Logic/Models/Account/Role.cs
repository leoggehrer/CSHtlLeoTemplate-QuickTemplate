//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Models.Account
{
    using QuickTemplate.Logic.Entities;
    using TEntity = Entities.Account.Role;
    using TModel = Models.Account.Role;
    public partial class Role : VersionExtendedModel, Contracts.Account.IRole
    {
        static Role()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        public Role()
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
        public Guid Guid
        {
            get => Source.Guid;
            set => Source.Guid = value;
        }
        public String Designation
        {
            get => Source.Designation;
            set => Source.Designation = value;
        }
        public String? Description
        {
            get => Source.Description;
            set => Source.Description = value;
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
                Guid = other.Guid;
                Designation = other.Designation;
                Description = other.Description;
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
                Guid = other.Guid;
                Designation = other.Designation;
                Description = other.Description;
            }
            AfterCopyProperties(other);
        }
        partial void BeforeCopyProperties(TModel other, ref bool handled);
        partial void AfterCopyProperties(TModel other);

        public override bool Equals(object? obj)
        {
            bool result = false;
            if (obj is Models.Account.Role other)
            {
                result = IsEqualsWith(GetType().Name, other.GetType().Name) && Id == other.Id;
            }
            return result;
        }
        public override int GetHashCode()
        {
#if ROWVERSION_ON
            return HashCode.Combine(Guid, Designation, Description, RowVersion, Id);
#else
            return HashCode.Combine(Guid, Designation, Description, Id);
#endif
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
            result.Source = other;
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
