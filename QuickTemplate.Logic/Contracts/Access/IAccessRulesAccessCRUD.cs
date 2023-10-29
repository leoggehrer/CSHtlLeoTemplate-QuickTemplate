//@BaseCode
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
namespace QuickTemplate.Logic.Contracts.Access
{
    using CommonBase.Contracts;
    /// <summary>
    /// Provides access control rules for creating, reading, changing, and deleting items.
    /// </summary>
    public partial interface IAccessRulesAccess
    {
        /// <summary>
        /// Determines whether an instance of the specified type can be created asynchronously.
        /// </summary>
        /// <param name="type">The type to check if an instance can be created.</param>
        /// <param name="identity">The identity associated with the account for which the instance is being created.</param>
        /// <returns>true if an instance of the specified type can be created asynchronously; otherwise, false.</returns>
        Task<bool> CanBeCreatedAsync(Type type, Account.IIdentity identity);
        /// <summary>
        /// Asynchronously checks if the specified item can be read by the provided identity.
        /// </summary>
        /// <param name="item">The item to be checked.</param>
        /// <param name="identity">The identity associated with the account.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a boolean value indicating if the item can be read.</returns>
        Task<bool> CanBeReadAsync(IIdentifyable item, Account.IIdentity identity);
        /// <summary>
        /// Checks whether the specified item can be changed asynchronously.
        /// </summary>
        /// <param name="item">The item to be checked.</param>
        /// <param name="identity">The identity of the account attempting to change the item.</param>
        /// <returns>
        /// A task representing the asynchronous operation that returns true if the item can be changed;
        /// otherwise, false.
        /// </returns>
        Task<bool> CanBeChangedAsync(IIdentifyable item, Account.IIdentity identity);
        /// <summary>
        /// Checks if the specified item can be deleted asynchronously, based on the provided identity.
        /// </summary>
        /// <param name="item">The item to be checked if it can be deleted.</param>
        /// <param name="identity">The identity of the account attempting the deletion.</param>
        /// <returns>A task representing the asynchronous operation. The task will return a boolean
        /// indicating if the item can be deleted (true) or not (false).</returns>
        Task<bool> CanBeDeletedAsync(IIdentifyable item, Account.IIdentity identity);
    }
}
#endif
//MdEnd
