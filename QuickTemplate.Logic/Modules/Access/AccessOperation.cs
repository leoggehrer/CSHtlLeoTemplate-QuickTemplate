//@BaseCode
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON

namespace QuickTemplate.Logic.Modules.Access
{
    public enum AccessOperation
    {
        None = 0,
        Create = 1,
        Read = 2 * Create,
        Update = 2 * Read,
        Delete = 2 * Update,
        Display = 2 * Delete,

        CRUD = Create + Read + Update + Delete,
    }
}
#endif
//MdEnd
