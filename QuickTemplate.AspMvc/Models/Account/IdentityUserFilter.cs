//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.AspMvc.Models.Account
{
    public partial class IdentityUserFilter : Models.View.IFilterModel
    {
        static IdentityUserFilter()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        public IdentityUserFilter()
        {
            Constructing();
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        public IdType? IdentityId
        {
            get;
            set;
        }
        public string? Firstname
        {
            get;
            set;
        }
        public string? Lastname
        {
            get;
            set;
        }
        public bool HasEntityValue => IdentityId != null || Firstname != null || Lastname != null;
        private bool show = true;
        public bool Show => show;
        public string CreateEntityPredicate()
        {
            var result = new System.Text.StringBuilder();
            if (IdentityId != null)
            {
                if (result.Length > 0)
                {
                    result.Append(" || ");
                }
                result.Append($"(IdentityId != null && IdentityId == {IdentityId})");
            }
            if (Firstname != null)
            {
                if (result.Length > 0)
                {
                    result.Append(" || ");
                }
                result.Append($"(Firstname != null && Firstname.Contains(\"{Firstname}\"))");
            }
            if (Lastname != null)
            {
                if (result.Length > 0)
                {
                    result.Append(" || ");
                }
                result.Append($"(Lastname != null && Lastname.Contains(\"{Lastname}\"))");
            }
            return result.ToString();
        }
        public override string ToString()
        {
            System.Text.StringBuilder sb = new();
            if (IdentityId != null)
            {
                sb.Append($"IdentityId: {IdentityId}");
            }
            if (string.IsNullOrEmpty(Lastname) == false)
            {
                sb.Append($"Lastname: {Lastname} ");
            }
            if (string.IsNullOrEmpty(Firstname) == false)
            {
                sb.Append($"Firstname: {Firstname} ");
            }
            return sb.ToString();
        }
    }
}
#endif
//MdEnd