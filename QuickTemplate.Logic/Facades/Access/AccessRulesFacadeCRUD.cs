//@CodeCopy
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
namespace QuickTemplate.Logic.Facades.Access
{
    using CommonBase.Contracts;
    using TOutModel = Models.Access.AccessRule;
    
    /// <summary>
    /// Provides access to access rules operations for different models.
    /// </summary>
    partial class AccessRulesFacade
    {
        /// <summary>
        /// Provides access to the AccessController object implementing the IAccessRulesAccess interface.
        /// </summary>
        /// <remarks>
        /// This property returns the AccessController object casted as IAccessRulesAccess.
        /// It assumes that the ControllerObject property is of type Contracts.Access.IAccessRulesAccess.
        /// If the ControllerObject is not compatible with IAccessRulesAccess interface, a null reference will be returned.
        /// </remarks>
        new private Contracts.Access.IAccessRulesAccess Controller => (ControllerObject as Contracts.Access.IAccessRulesAccess)!;
        
        /// <summary>
        /// Determines whether an instance of the specified type can be created asynchronously.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the instance to be created.</param>
        /// <param name="identity">The <see cref="Contracts.Account.IIdentity"/> identity associated with the instance creation.</param>
        /// <returns>A Task of type bool that represents the asynchronous operation and returns true if an instance can be created, false otherwise.</returns>
        public Task<bool> CanBeCreatedAsync(Type type, Contracts.Account.IIdentity identity)
        {
            return Controller.CanBeCreatedAsync(type, identity);
        }
        /// <summary>
        /// Determines whether the specified model can be read asynchronously by the specified identity.
        /// </summary>
        /// <param name="model">The model to be checked.</param>
        /// <param name="identity">The identity checking the read permission.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a boolean value indicating
        /// whether the model can be read by the identity.
        /// </returns>
        public Task<bool> CanBeReadAsync(IIdentifyable model, Contracts.Account.IIdentity identity)
        {
            return Controller.CanBeReadAsync(model, identity);
        }
        /// <summary>
        /// Determines whether the specified model can be changed asynchronously for a given identity.
        /// </summary>
        /// <param name="model">The model to be checked for changeability.</param>
        /// <param name="identity">The identity for which the changeability of the model should be determined.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the model can be changed.
        /// </returns>
        /// <remarks>
        /// This method internally calls the <see cref="Controller.CanBeChangedAsync"/> method to determine if the specified model can be changed.
        /// </remarks>
        public Task<bool> CanBeChangedAsync(IIdentifyable model, Contracts.Account.IIdentity identity)
        {
            return Controller.CanBeChangedAsync(model, identity);
        }
        /// <summary>
        /// Determines whether a model can be deleted asynchronously.
        /// </summary>
        /// <param name="model">The model to be checked for deletability.</param>
        /// <param name="identity">The identity of the account performing the operation.</param>
        /// <returns>A Task representing the asynchronous operation, indicating whether the model can be deleted or not.</returns>
        public Task<bool> CanBeDeletedAsync(IIdentifyable model, Contracts.Account.IIdentity identity)
        {
            return Controller.CanBeDeletedAsync(model, identity);
        }
    }
}
#endif
//MdEnd
