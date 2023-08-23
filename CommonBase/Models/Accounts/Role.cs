//@CodeCopy
//MdStart
namespace CommonBase.Models.Accounts
{
    public partial class Role
    {
        public IdType Id { get; set; }
        public String Designation { get; set; } = string.Empty;
        public String? Description { get; set; } = string.Empty;
    }
}
//MdEnd
