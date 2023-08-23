//@CodeCopy
//MdStart
#if ACCOUNT_ON && REVISION_ON
namespace QuickTemplate.Logic.Models.Revision
{
    using System;
    public partial class History : ModelObject
    {
        static History()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        public History()
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        new internal QuickTemplate.Logic.Entities.Revision.History Source
        {
            get => (QuickTemplate.Logic.Entities.Revision.History)(_source ??= new QuickTemplate.Logic.Entities.Revision.History());
            set => _source = value;
        }
        public IdType IdentityId
        {
            get => Source.IdentityId;
            set => Source.IdentityId = value;
        }
        public System.String ActionType
        {
            get => Source.ActionType;
            set => Source.ActionType = value;
        }
        public System.DateTime ActionTime
        {
            get => Source.ActionTime;
            set => Source.ActionTime = value;
        }
        public System.String SubjectName
        {
            get => Source.SubjectName;
            set => Source.SubjectName = value;
        }
        public IdType SubjectId
        {
            get => Source.SubjectId;
            set => Source.SubjectId = value;
        }
        public System.String JsonData
        {
            get => Source.JsonData;
            set => Source.JsonData = value;
        }
        internal void CopyProperties(QuickTemplate.Logic.Entities.Revision.History other)
        {
            bool handled = false;
            BeforeCopyProperties(other, ref handled);
            if (handled == false)
            {
                IdentityId = other.IdentityId;
                ActionType = other.ActionType;
                ActionTime = other.ActionTime;
                SubjectName = other.SubjectName;
                SubjectId = other.SubjectId;
                JsonData = other.JsonData;
                Id = other.Id;
            }
            AfterCopyProperties(other);
        }
        partial void BeforeCopyProperties(QuickTemplate.Logic.Entities.Revision.History other, ref bool handled);
        partial void AfterCopyProperties(QuickTemplate.Logic.Entities.Revision.History other);
        internal void CopyProperties(QuickTemplate.Logic.Models.Revision.History other)
        {
            bool handled = false;
            BeforeCopyProperties(other, ref handled);
            if (handled == false)
            {
                IdentityId = other.IdentityId;
                ActionType = other.ActionType;
                ActionTime = other.ActionTime;
                SubjectName = other.SubjectName;
                SubjectId = other.SubjectId;
                JsonData = other.JsonData;
                Id = other.Id;
            }
            AfterCopyProperties(other);
        }
        partial void BeforeCopyProperties(QuickTemplate.Logic.Models.Revision.History other, ref bool handled);
        partial void AfterCopyProperties(QuickTemplate.Logic.Models.Revision.History other);
        public override bool Equals(object? obj)
        {
            bool result = false;
            if (obj is Models.Revision.History other)
            {
                result = Id == other.Id;
            }
            return result;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(IdentityId, ActionType, ActionTime, SubjectName, SubjectId, JsonData, HashCode.Combine(Id));
        }
        public static QuickTemplate.Logic.Models.Revision.History Create()
        {
            BeforeCreate();
            var result = new QuickTemplate.Logic.Models.Revision.History();
            AfterCreate(result);
            return result;
        }
        public static QuickTemplate.Logic.Models.Revision.History Create(object other)
        {
            BeforeCreate(other);
            CommonBase.Extensions.ObjectExtensions.CheckArgument(other, nameof(other));
            var result = new QuickTemplate.Logic.Models.Revision.History();
            CommonBase.Extensions.ObjectExtensions.CopyFrom(result, other);
            AfterCreate(result, other);
            return result;
        }
        public static QuickTemplate.Logic.Models.Revision.History Create(QuickTemplate.Logic.Models.Revision.History other)
        {
            BeforeCreate(other);
            var result = new QuickTemplate.Logic.Models.Revision.History();
            result.CopyProperties(other);
            AfterCreate(result, other);
            return result;
        }
        internal static QuickTemplate.Logic.Models.Revision.History Create(QuickTemplate.Logic.Entities.Revision.History other)
        {
            BeforeCreate(other);
            var result = new QuickTemplate.Logic.Models.Revision.History();
            result.Source = other;
            AfterCreate(result, other);
            return result;
        }
        static partial void BeforeCreate();
        static partial void AfterCreate(QuickTemplate.Logic.Models.Revision.History instance);
        static partial void BeforeCreate(object other);
        static partial void AfterCreate(QuickTemplate.Logic.Models.Revision.History instance, object other);
        static partial void BeforeCreate(QuickTemplate.Logic.Models.Revision.History other);
        static partial void AfterCreate(QuickTemplate.Logic.Models.Revision.History instance, QuickTemplate.Logic.Models.Revision.History other);
        static partial void BeforeCreate(QuickTemplate.Logic.Entities.Revision.History other);
        static partial void AfterCreate(QuickTemplate.Logic.Models.Revision.History instance, QuickTemplate.Logic.Entities.Revision.History other);
    }
}
#endif
//MdEnd
