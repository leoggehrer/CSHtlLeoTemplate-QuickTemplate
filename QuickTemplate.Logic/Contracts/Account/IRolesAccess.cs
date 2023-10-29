//@BaseCode
//MdStart
#if ACCOUNT_ON
using CommonBase.Contracts;

namespace QuickTemplate.Logic.Contracts.Account
{
    using TOutModel = Models.Account.Role;
    /// <summary>
    /// Represents an interface for accessing roles data.
    /// </summary>
    /// <typeparam name="TOutModel">The type of the output model.</typeparam>
    public partial interface IRolesAccess : IDataAccess<TOutModel>
    {
    }
}
#endif
//MdEnd
