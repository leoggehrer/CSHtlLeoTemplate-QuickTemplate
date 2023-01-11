//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.AspMvc.Models.Account
{
    public partial class IdentityFilter : Models.View.IFilterModel
    {
        public string? Email { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;

        public bool HasEntityValue => Email != null || Name != null;
        private bool show = true;
        public bool Show => show;
        public string CreateEntityPredicate()
        {
            var result = new System.Text.StringBuilder();

            if (Email != null)
            {
                if (result.Length > 0)
                {
                    result.Append(" || ");
                }
                result.Append($"(Email != null && Email.Contains(\"{Email}\"))");
            }
            if (Name != null)
            {
                if (result.Length > 0)
                {
                    result.Append(" || ");
                }
                result.Append($"(Name != null && Name.Contains(\"{Name}\"))");
            }
            return result.ToString();
        }
        public override string ToString()
        {
            System.Text.StringBuilder sb = new();
            if (string.IsNullOrEmpty(Email) == false)
            {
                sb.Append($"Email: {Email} ");
            }
            if (string.IsNullOrEmpty(Name) == false)
            {
                sb.Append($"Name: {Name} ");
            }
            return sb.ToString();
        }
    }
}
#endif
//MdEnd