//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Controllers.Account
{
    using QuickTemplate.Logic.Entities.Account;
    using QuickTemplate.Logic.Modules.Account;
    using TEntity = Entities.Account.SecureIdentity;
    using TOutModel = Models.Account.Identity;
    
    /// <summary>
    /// Represents a controller for handling identity-related operations and access control.
    /// </summary>
    [Modules.Security.Authorize("SysAdmin", "AppAdmin")]
    internal sealed partial class IdentitiesController : EntitiesController<TEntity, TOutModel>, Contracts.Account.IIdentitiesAccess
    {
        private List<IdType> identityIds = new();
        /// <summary>
        /// Initializes a new instance of the IdentitiesController class.
        /// </summary>
        public IdentitiesController()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentitiesController"/> class.
        /// </summary>
        /// <param name="other">The <see cref="ControllerObject"/> object to be used as the base for this controller.</param>
        public IdentitiesController(ControllerObject other)
        : base(other)
        {
        }
        
        /// <summary>
        /// Adds a new identity ID to the list if it does not already exist.
        /// </summary>
        /// <param name="id">The ID of the identity</param>
        private void ChangedIdentity(IdType id)
        {
            if (identityIds.Contains(id) == false)
            {
                identityIds.Add(id);
            }
        }
        
        #region Overrides
        /// <summary>
        /// Performs an action before executing the specified action type on the specified entity.
        /// </summary>
        /// <param name="actionType">The type of action to be executed.</param>
        /// <param name="entity">The entity on which the action is to be executed.</param>
        protected override void BeforeActionExecute(ActionType actionType, TEntity entity)
        {
            if (actionType == ActionType.Insert)
            {
                entity.Guid = Guid.NewGuid();
            }
            else if (actionType == ActionType.Update)
            {
                using var ctrl = new IdentitiesController();
                var dbEntity = ctrl.EntitySet.Find(entity.Id);
                
                if (dbEntity != null)
                {
                    entity.Guid = dbEntity.Guid;
                }
            }
            base.BeforeActionExecute(actionType, entity);
        }
        /// <summary>
        /// Performs actions before returning the specified collection of entities.
        /// </summary>
        /// <typeparam name="TOutModel">The type of the output model.</typeparam>
        /// <param name="entities">The collection of entities.</param>
        /// <returns>A collection of <typeparamref name="TOutModel"/>.</returns>
        internal override IEnumerable<TOutModel> BeforeReturn(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                BeforeReturn(entity);
            }
            return base.BeforeReturn(entities);
        }
        /// <summary>
        /// This method is called before returning the <paramref name="entity"/>. It performs a check
        /// and adds identity roles to the entity's IdentityXRoles property if it is empty.
        /// </summary>
        /// <typeparam name="TOutModel">The type of the entity to be returned</typeparam>
        /// <param name="entity">The entity for which to perform the operation</param>
        /// <returns>The modified entity after adding identity roles if needed</returns>
        internal override TOutModel BeforeReturn(TEntity entity)
        {
            if (entity.IdentityXRoles.Any() == false)
            {
                Task.Run(async () =>
                {
                    using var ctrl = new IdentityXRolesController(this);
                    
                    entity.IdentityXRoles.AddRange(await ctrl.QueryByIdentityAsync(entity.Id));
                }).Wait();
            }
            return base.BeforeReturn(entity);
        }
        #endregion Overrides
        
        #region Get identity
        /// <summary>
        /// Retrieves a valid secure identity by its ID asynchronously.
        /// </summary>
        /// <param name="identityId">The ID of the identity to retrieve.</param>
        /// <returns>A Task representing the asynchronous operation. The result is a SecureIdentity object if found, otherwise null.</returns>
        internal Task<SecureIdentity?> GetValidIdentityByIdAsync(IdType identityId)
        {
            return EntitySet.Include(e => e.IdentityXRoles)
                            .ThenInclude(e => e.Role)
                            .FirstOrDefaultAsync(e => e.State == Modules.Common.State.Active
            && e.AccessFailedCount < 4
            && e.Id == identityId);
        }
        /// <summary>
        /// Retrieves a valid secure identity with the specified email asynchronously.
        /// </summary>
        /// <param name="email">The email address to search for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the valid secure identity, if found; otherwise, null.</returns>
        /// <remarks>
        /// This method includes the associated identity and roles, filters by active state, ensures access failed count is less than 4, and compares email addresses in a case-insensitive manner.
        /// </remarks>
        internal Task<SecureIdentity?> GetValidIdentityByEmailAsync(string email)
        {
            return EntitySet.Include(e => e.IdentityXRoles)
                            .ThenInclude(e => e.Role)
                            .FirstOrDefaultAsync(e => e.State == Modules.Common.State.Active
            && e.AccessFailedCount < 4
            && e.Email.ToLower() == email.ToLower());
        }
        #endregion Get identity
        
        #region Add or remove role
        /// <summary>
        /// Adds a role to a specified identity asynchronously.
        /// </summary>
        /// <param name="identityId">The ID of the identity.</param>
        /// <param name="roleId">The ID of the role.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method first checks the authorization for adding a role asynchronously. If authorization is granted,
        /// it creates an instance of the RolesController class, retrieves the role by its ID asynchronously,
        /// and if the role exists, retrieves the identity by its ID asynchronously. If the identity exists,
        /// it creates an instance of the IdentityXRolesController class, and creates a new IdentityXRole object
        /// with the retrieved role and identity. Finally, it inserts the IdentityXRole object asynchronously
        /// and calls the ChangedIdentity method with the identity ID as an argument.
        /// </remarks>
        public async Task AddRoleAsync(IdType identityId, IdType roleId)
        {
            await CheckAuthorizationAsync(GetType(), nameof(AddRoleAsync)).ConfigureAwait(false);
            
            using var roleCtrl = new RolesController(this);
            var role = await roleCtrl.ExecuteGetByIdAsync(roleId).ConfigureAwait(false);
            
            if (role != null)
            {
                var identity = await ExecuteGetByIdAsync(identityId).ConfigureAwait(false);
                
                if (identity != null)
                {
                    using var identityXRolesCtrl = new IdentityXRolesController(this);
                    var identityXRole = new Entities.Account.IdentityXRole
                    {
                        Role = role,
                        Identity = identity,
                    };
                    await identityXRolesCtrl.ExecuteInsertAsync(identityXRole).ConfigureAwait(false);
                }
            }
            ChangedIdentity(identityId);
        }
        /// <summary>
        /// Removes a role from an identity asynchronously.
        /// </summary>
        /// <param name="identityId">The ID of the identity.</param>
        /// <param name="roleId">The ID of the role to be removed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task RemoveRoleAsync(IdType identityId, IdType roleId)
        {
            await CheckAuthorizationAsync(GetType(), nameof(RemoveRoleAsync)).ConfigureAwait(false);
            
            using var identityXRolesCtrl = new IdentityXRolesController(this);
            var identityXRoles = await identityXRolesCtrl.ExecuteQueryAsync(e => e.IdentityId == identityId && e.RoleId == roleId)
                                                         .ConfigureAwait(false);
            
            if (identityXRoles.Length == 1)
            {
                await identityXRolesCtrl.DeleteAsync(identityXRoles[0].Id).ConfigureAwait(false);
            }
            ChangedIdentity(identityId);
        }
        #endregion Add or remove role
        
        /// <summary>
        /// Method that is called after the execution of an action.
        /// </summary>
        /// <param name="actionType">The type of action that was executed.</param>
        protected override void AfterActionExecute(ActionType actionType)
        {
            if (actionType == ActionType.Save)
            {
                foreach (var id in identityIds)
                {
                    Task.Run(async () => await AccountManager.RefreshAliveSessionsAsync(id).ConfigureAwait(false)).Wait();
                }
            }
            identityIds.Clear();
            
            base.AfterActionExecute(actionType);
        }
    }
}
#endif
//MdEnd
