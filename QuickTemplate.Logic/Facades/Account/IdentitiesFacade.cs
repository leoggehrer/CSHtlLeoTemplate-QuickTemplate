//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Facades.Account
{
    using TOutModel = Models.Account.Identity;
    using TAccessContract = Contracts.Account.IIdentitiesAccess;
    
    /// <summary>
    /// Represents a facade for managing identities.
    /// </summary>
    /// <typeparam name="TOutModel">The type of the output model.</typeparam>
    /// <typeparam name="TAccessContract">The type of the access contract.</typeparam>
    public sealed partial class IdentitiesFacade : ControllerFacade<TOutModel, TAccessContract>, TAccessContract
    {
        /// <summary>
        /// Gets the instance of <see cref="Contracts.Account.IIdentitiesAccess"/> that the property
        /// is currently pointing to.
        /// </summary>
        /// <value>
        /// The instance of <see cref="Contracts.Account.IIdentitiesAccess"/>.
        /// </value>
        new private Contracts.Account.IIdentitiesAccess Controller => (ControllerObject as Contracts.Account.IIdentitiesAccess)!;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentitiesFacade"/> class.
        /// </summary>
        public IdentitiesFacade()
        : base(new Controllers.Account.IdentitiesController())
        {
        }
        /// <summary>
        /// This class represents a facade for working with identity-related operations.
        /// </summary>
        /// <param name="facadeObject">The facade object.</param>
        public IdentitiesFacade(FacadeObject facadeObject)
        : base(new Controllers.Account.IdentitiesController(facadeObject.ControllerObject))
        {
            
        }
        
        /// <summary>
        /// Adds a role asynchronously using the provided ID and role ID.
        /// </summary>
        /// <param name="id">The ID of the item.</param>
        /// <param name="roleId">The ID of the role to be added.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task AddRoleAsync(IdType id, IdType roleId)
        {
            return Controller.AddRoleAsync(id, roleId);
        }
        /// <summary>
        /// Removes a role asynchronously from a given ID.
        /// </summary>
        /// <param name="id">The ID to remove the role from.</param>
        /// <param name="roleId">The ID of the role to be removed.</param>
        /// <returns>A task representing the removal of the role.</returns>
        public Task RemoveRoleAsync(IdType id, IdType roleId)
        {
            return Controller.RemoveRoleAsync(id, roleId);
        }
    }
}
#endif
//MdEnd
