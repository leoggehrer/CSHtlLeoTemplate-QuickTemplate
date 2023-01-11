//@BaseCode
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
namespace QuickTemplate.Logic.Modules.Access
{
    public enum AccessType
    {
        Identity = 1,
        IdentityRole = 2 * Identity,
        Entity = 2 * IdentityRole,
        CustomRole = 2 * Entity,
        All = Identity + IdentityRole + Entity + CustomRole,
    }
}
#endif
//MdEnd