//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Contracts.Account
{
    using CommonBase.Contracts;
    using TOutModel = Models.Account.Identity;
    
    /// <summary>
    /// Represents an interface for accessing identities.
    /// </summary>
    /// <typeparam name="TOutModel">The type of the output model.</typeparam>
    public partial interface IIdentitiesAccess : IDataAccess<TOutModel>
    {
        /// <summary>
        /// Adds a role asynchronously to the specified identifier.
        /// </summary>
        /// <param name="id">The identifier to which the role will be added.</param>
        /// <param name="roleId">The role identifier to be added.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task AddRoleAsync(IdType id, IdType roleId);
        /// <summary>
        /// Removes a role asynchronously from a given id.
        /// </summary>
        /// <param name="id">The id from which the role should be removed.</param>
        /// <param name="roleId">The id of the role to be removed.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task RemoveRoleAsync(IdType id, IdType roleId);
    }
}
#endif
//MdEnd
