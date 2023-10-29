//@BaseCode
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
namespace QuickTemplate.Logic.Controllers.Access
{
    using CommonBase.Contracts;
    using QuickTemplate.Logic.Modules.Access;
    using QuickTemplate.Logic.Modules.Exceptions;
    using Error = CommonBase.Modules.Exceptions.ErrorType;
    using TEntity = Entities.Access.AccessRule;
    using TOutModel = Models.Access.AccessRule;
    
    /// <summary>
    /// Represents a controller for access rules.
    /// </summary>
    /// <remarks>
    /// This controller handles the validation and authorization of access rules.
    /// </remarks>
    [Modules.Security.Authorize("SysAdmin", "AppAdmin")]
    internal sealed partial class AccessRulesController
    {
        private List<TEntity> AccessRules = new List<TEntity>();
        
        /// <summary>
        /// Validates an entity based on the specified action type and entity object.
        /// </summary>
        /// <param name="actionType">The action type to be validated.</param>
        /// <param name="entity">The entity object to be validated.</param>
        /// <exception cref="LogicException">Thrown when the entity value or access value is invalid.</exception>
        protected override void ValidateEntity(ActionType actionType, TEntity entity)
        {
            // Check rule type
            if (entity.Type == RuleType.EntityType && string.IsNullOrEmpty(entity.EntityValue) == false)
            {
                throw new LogicException(Error.InvalidAccessRuleEntityValue);
            }
            else if (entity.Type == RuleType.EntityBy && string.IsNullOrEmpty(entity.EntityValue))
            {
                throw new LogicException(Error.InvalidAccessRuleEntityValue);
            }
            else if (entity.Type == RuleType.Entities && string.IsNullOrEmpty(entity.EntityValue) == false)
            {
                throw new LogicException(Error.InvalidAccessRuleEntityValue);
            }
            // Check access type
            else if (entity.AccessType == AccessType.All && string.IsNullOrEmpty(entity.AccessValue) == false)
            {
                throw new LogicException(Error.InvalidAccessRuleAccessValue);
            }
            else if (entity.AccessType == AccessType.Identity && string.IsNullOrEmpty(entity.AccessValue))
            {
                throw new LogicException(Error.InvalidAccessRuleAccessValue);
            }
            else if (entity.AccessType == AccessType.IdentityRole && string.IsNullOrEmpty(entity.AccessValue))
            {
                throw new LogicException(Error.InvalidAccessRuleAccessValue);
            }
            base.ValidateEntity(actionType, entity);
        }
        /// <summary>
        ///   Executes a code block before executing an action.
        /// </summary>
        /// <param name="actionType">The type of action being performed (insert, update).</param>
        /// <param name="entity">The entity on which the action is being performed.</param>
        /// <remarks>
        ///   This method checks if the given action type is insert or update. If it is, it searches for an existing access rule in the EntitySet that matches the specified conditions by comparing properties of the given entity. If an access rule is found, it throws a LogicException with the error message InvalidAccessRuleAlreadyExits. Finally, the base implementation is invoked to perform the action.
        /// </remarks>
        protected override void BeforeActionExecute(ActionType actionType, TEntity entity)
        {
            if (actionType == ActionType.Insert || actionType == ActionType.Update)
            {
                var dbAccessRule = EntitySet.FirstOrDefault(e => e.Id != entity.Id
                && e.EntityType == entity.EntityType
                && e.EntityValue == entity.EntityValue
                && e.RelationshipEntityType == entity.RelationshipEntityType
                && e.AccessType == entity.AccessType
                && e.AccessRoleType == entity.AccessRoleType
                && e.AccessValue == entity.AccessValue);
                if (dbAccessRule != null)
                {
                    throw new LogicException(Error.InvalidAccessRuleAlreadyExits);
                }
            }
            base.BeforeActionExecute(actionType, entity);
        }
        /// <summary>
        /// Retrieves a collection of access rules asynchronously based on the specified entity type.
        /// </summary>
        /// <param name="entityType">The type of entity for which access rules will be retrieved.</param>
        /// <returns>
        /// A collection of access rules associated with the specified entity type.
        /// </returns>
        private async Task<IEnumerable<TEntity>> GetAccessRulesAsync(string entityType)
        {
            var result = AccessRules.Where(ar => ar.EntityType == entityType).ToList();
            
            if (result.Any() == false)
            {
                result.AddRange(await EntitySet.Where(ar => ar.EntityType == entityType).ToArrayAsync().ConfigureAwait(false));
            }
            return result;
        }
        
        /// <summary>
        /// Checks if an object of specified type can be created asynchronously.
        /// </summary>
        /// <param name="type">The type of object to be checked.</param>
        /// <param name="identity">The identity object representing the account.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a
        /// boolean value indicating whether the object can be created or not.
        /// </returns>
        public Task<bool> CanBeCreatedAsync(Type type, Contracts.Account.IIdentity identity)
        {
            return GetCreateAccessAsync(type, identity);
        }
        ///<summary>
        ///Checks if the item can be read asynchronously by the specified identity.
        ///</summary>
        ///<param name="item">The item to be checked.</param>
        ///<param name="identity">The identity used to check read access.</param>
        ///<returns>A task representing the asynchronous operation. The task result is true if the item can be read, otherwise false.</returns>
        public Task<bool> CanBeReadAsync(IIdentifyable item, Contracts.Account.IIdentity identity)
        {
            return GetReadAccessAsync(item, identity);
        }
        /// <summary>
        ///     Determines whether the specified item can be changed asynchronously.
        /// </summary>
        /// <param name="item">The item to check if it can be changed.</param>
        /// <param name="identity">The identity used to check the update access.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation. The task result contains a boolean value
        ///     indicating whether the specified item can be changed.
        /// </returns>
        public Task<bool> CanBeChangedAsync(IIdentifyable item, Contracts.Account.IIdentity identity)
        {
            return GetUpdateAccessAsync(item, identity);
        }
        /// <summary>
        /// Checks if the specified item can be deleted asynchronously.
        /// </summary>
        /// <param name="item">The item to be checked for delete access.</param>
        /// <param name="identity">The identity of the user account.</param>
        /// <returns>A Task representing the asynchronous operation, returning true if the item can be deleted, false otherwise.</returns>
        public Task<bool> CanBeDeletedAsync(IIdentifyable item, Contracts.Account.IIdentity identity)
        {
            return GetDeleteAccessAsync(item, identity);
        }
        
        /// <summary>
        /// Retrieves the create access asynchronously for the specified type and identity.
        /// </summary>
        /// <param name="type">The type for which to retrieve the create access.</param>
        /// <param name="identity">The identity for which to retrieve the create access.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation that returns a boolean indicating whether the create access is granted or not.</returns>
        private async Task<bool> GetCreateAccessAsync(Type type, Contracts.Account.IIdentity identity)
        {
            Func<TEntity, bool> getOperation = ar => ar.Creatable;
            var accessRules = await GetAccessRulesAsync(type.Name).ConfigureAwait(false);
            
            return GetEntityTypeAccess(accessRules, identity, getOperation);
        }
        /// <summary>
        /// Retrieves the read access for the specified item and identity asynchronously.
        /// </summary>
        /// <param name="item">The item for which read access is being checked.</param>
        /// <param name="identity">The identity for which read access is being checked.</param>
        /// <returns>A task representing the asynchronous operation. The task result is a boolean value indicating whether read access is granted.</returns>
        private async Task<bool> GetReadAccessAsync(IIdentifyable item, Contracts.Account.IIdentity identity)
        {
            Func<TEntity, bool> getOperation = ar => ar.Readable;
            var accessRules = await GetAccessRulesAsync(item.GetType().Name).ConfigureAwait(false);
            
            return GetEntityTypeAccess(accessRules, identity, getOperation)
            && GetEntitiesAccess(accessRules, identity, getOperation)
            && GetEntityByAccess(accessRules, item, identity, getOperation);
        }
        ///<summary>
        /// Asynchronously determines if the given item has update access for the specified identity.
        ///</summary>
        ///<param name="item">The item to check update access for.</param>
        ///<param name="identity">The identity to check against.</param>
        ///<returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        ///<exception cref="ArgumentNullException">Thrown when <paramref name="item"/> or <paramref name="identity"/> is null.</exception>
        private async Task<bool> GetUpdateAccessAsync(IIdentifyable item, Contracts.Account.IIdentity identity)
        {
            Func<TEntity, bool> getOperation = ar => ar.Updatable;
            var accessRules = await GetAccessRulesAsync(item.GetType().Name).ConfigureAwait(false);
            
            return GetEntityTypeAccess(accessRules, identity, getOperation)
            && GetEntitiesAccess(accessRules, identity, getOperation)
            && GetEntityByAccess(accessRules, item, identity, getOperation);
        }
        /// <summary>
        /// Asynchronously checks if the specified item can be deleted by the given identity.
        /// </summary>
        /// <param name="item">The item to be checked for delete access.</param>
        /// <param name="identity">The identity of the account performing the operation.</param>
        /// <returns><c>true</c> if the item can be deleted by the given identity; otherwise, <c>false</c>.</returns>
        private async Task<bool> GetDeleteAccessAsync(IIdentifyable item, Contracts.Account.IIdentity identity)
        {
            Func<TEntity, bool> getOperation = ar => ar.Deletable;
            var accessRules = await GetAccessRulesAsync(item.GetType().Name).ConfigureAwait(false);
            
            return GetEntityTypeAccess(accessRules, identity, getOperation)
            && GetEntitiesAccess(accessRules, identity, getOperation)
            && GetEntityByAccess(accessRules, item, identity, getOperation);
        }
        
        /// <summary>
        /// Determines if the specified access rules allow access to the entity type based on the identity and operation.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="accessRules">The access rules for the entity type.</param>
        /// <param name="identity">The identity of the user.</param>
        /// <param name="getOperation">A function that returns a boolean indicating if the operation is allowed.</param>
        /// <returns>True if access to the entity type is allowed, otherwise false.</returns>
        private bool GetEntityTypeAccess(IEnumerable<TEntity> accessRules, Contracts.Account.IIdentity identity, Func<TEntity, bool> getOperation)
        {
            var result = false;
            var typeRules = accessRules.Where(ar => ar.Type == RuleType.EntityType);
            
            if (typeRules.Any())
            {
                var accessRule = typeRules.FirstOrDefault(r => r.AccessType == AccessType.All);
                
                if (accessRule != null)
                {
                    result = getOperation(accessRule);
                }
                accessRule = typeRules.FirstOrDefault(r => r.AccessType == AccessType.Identity && r.AccessValue == identity.Guid.ToString());
                if (accessRule != null)
                {
                    result = getOperation(accessRule);
                }
                accessRule = typeRules.FirstOrDefault(r => r.AccessType == AccessType.IdentityRole && identity.HasRole(Guid.Parse(r.AccessValue!)));
                if (accessRule != null)
                {
                    result = getOperation(accessRule);
                }
            }
            else
            {
                result = true;
            }
            return result;
        }
        /// <summary>
        /// Gets the access to entities based on the provided access rules, identity, and operation.
        /// </summary>
        /// <typeparam name="TEntity">The type of entities.</typeparam>
        /// <param name="accessRules">The collection of access rules.</param>
        /// <param name="identity">The identity object.</param>
        /// <param name="getOperation">The function to get the operation.</param>
        /// <returns>True if access is granted, false otherwise.</returns>
        private bool GetEntitiesAccess(IEnumerable<TEntity> accessRules, Contracts.Account.IIdentity identity, Func<TEntity, bool> getOperation)
        {
            var result = false;
            var typeRules = accessRules.Where(ar => ar.Type == RuleType.Entities);
            
            if (typeRules.Any())
            {
                var accessRule = typeRules.FirstOrDefault(r => r.AccessType == AccessType.All);
                
                if (accessRule != null)
                {
                    result = getOperation(accessRule);
                }
                accessRule = typeRules.FirstOrDefault(r => r.AccessType == AccessType.Identity && r.AccessValue == identity.Guid.ToString());
                if (accessRule != null)
                {
                    result = getOperation(accessRule);
                }
                accessRule = typeRules.FirstOrDefault(r => r.AccessType == AccessType.IdentityRole && identity.HasRole(Guid.Parse(r.AccessValue!)));
                if (accessRule != null)
                {
                    result = getOperation(accessRule);
                }
            }
            else
            {
                result = true;
            }
            return result;
        }
        /// <summary>
        /// Retrieves an entity based on access rules.
        /// </summary>
        /// <param name="accessRules">The collection of access rules for the entity.</param>
        /// <param name="item">The identifiable item.</param>
        /// <param name="identity">The identity of the user.</param>
        /// <param name="getOperation">The function to retrieve the entity.</param>
        /// <returns>True if the entity can be retrieved, otherwise false.</returns>
        private bool GetEntityByAccess(IEnumerable<TEntity> accessRules, IIdentifyable item, Contracts.Account.IIdentity identity, Func<TEntity, bool> getOperation)
        {
            var result = false;
            var typeRules = accessRules.Where(ar => ar.Type == RuleType.EntityBy && ar.EntityValue == item.Id.ToString());
            
            if (typeRules.Any())
            {
                var accessRule = typeRules.FirstOrDefault(r => r.AccessType == AccessType.All);
                
                if (accessRule != null)
                {
                    result = getOperation(accessRule);
                }
                accessRule = typeRules.FirstOrDefault(r => r.AccessType == AccessType.Identity && r.AccessValue == identity.Guid.ToString());
                if (accessRule != null)
                {
                    result = getOperation(accessRule);
                }
                accessRule = typeRules.FirstOrDefault(r => r.AccessType == AccessType.IdentityRole && identity.HasRole(Guid.Parse(r.AccessValue!)));
                if (accessRule != null)
                {
                    result = getOperation(accessRule);
                }
            }
            else
            {
                result = true;
            }
            return result;
        }
    }
}
#endif
//MdEnd
