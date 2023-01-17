//@CodeCopy
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
namespace QuickTemplate.Logic.Controllers.Access
{
    using QuickTemplate.Logic.Contracts;
    using QuickTemplate.Logic.Modules.Access;
    using QuickTemplate.Logic.Modules.Exceptions;
    using TEntity = Entities.Access.AccessRule;
    using TOutModel = Models.Access.AccessRule;

    [Modules.Security.Authorize("SysAdmin", "AppAdmin")]
    internal sealed partial class AccessRulesController
    {
        private List<TEntity> AccessRules = new List<TEntity>();

        protected override void ValidateEntity(ActionType actionType, TEntity entity)
        {
            // Check rule type
            if (entity.Type == RuleType.EntityType && string.IsNullOrEmpty(entity.EntityValue) == false)
            {
                throw new LogicException(ErrorType.InvalidAccessRuleEntityValue);
            }
            else if (entity.Type == RuleType.EntityBy && string.IsNullOrEmpty(entity.EntityValue))
            {
                throw new LogicException(ErrorType.InvalidAccessRuleEntityValue);
            }
            else if (entity.Type == RuleType.Entities && string.IsNullOrEmpty(entity.EntityValue) == false)
            {
                throw new LogicException(ErrorType.InvalidAccessRuleEntityValue);
            }
            // Check access type
            else if (entity.AccessType == AccessType.All && string.IsNullOrEmpty(entity.AccessValue) == false)
            {
                throw new LogicException(ErrorType.InvalidAccessRuleAccessValue);
            }
            else if (entity.AccessType == AccessType.Identity && string.IsNullOrEmpty(entity.AccessValue))
            {
                throw new LogicException(ErrorType.InvalidAccessRuleAccessValue);
            }
            else if (entity.AccessType == AccessType.IdentityRole && string.IsNullOrEmpty(entity.AccessValue))
            {
                throw new LogicException(ErrorType.InvalidAccessRuleAccessValue);
            }
            base.ValidateEntity(actionType, entity);
        }
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
                    throw new LogicException(ErrorType.InvalidAccessRuleAlreadyExits);
                }
            }
            base.BeforeActionExecute(actionType, entity);
        }
        private async Task<IEnumerable<TEntity>> GetAccessRulesAsync(string entityType)
        {
            var result = AccessRules.Where(ar => ar.EntityType == entityType).ToList();

            if (result.Any() == false)
            {
                result.AddRange(await EntitySet.Where(ar => ar.EntityType == entityType).ToArrayAsync().ConfigureAwait(false));
            }
            return result;
        }

        public Task<bool> CanBeCreatedAsync(Type type, Contracts.Account.IIdentity identity)
        {
            return GetCreateAccessAsync(type, identity);
        }
        public Task<bool> CanBeReadAsync(IIdentifyable item, Contracts.Account.IIdentity identity)
        {
            return GetReadAccessAsync(item, identity);
        }
        public Task<bool> CanBeChangedAsync(IIdentifyable item, Contracts.Account.IIdentity identity)
        {
            return GetUpdateAccessAsync(item, identity);
        }
        public Task<bool> CanBeDeletedAsync(IIdentifyable item, Contracts.Account.IIdentity identity)
        {
            return GetDeleteAccessAsync(item, identity);
        }

        private async Task<bool> GetCreateAccessAsync(Type type, Contracts.Account.IIdentity identity)
        {
            Func<TEntity, bool> getOperation = ar => ar.Creatable;
            var accessRules = await GetAccessRulesAsync(type.Name).ConfigureAwait(false);

            return GetEntityTypeAccess(accessRules, identity, getOperation);
        }
        private async Task<bool> GetReadAccessAsync(IIdentifyable item, Contracts.Account.IIdentity identity)
        {
            Func<TEntity, bool> getOperation = ar => ar.Readable;
            var accessRules = await GetAccessRulesAsync(item.GetType().Name).ConfigureAwait(false);

            return GetEntityTypeAccess(accessRules, identity, getOperation) 
                && GetEntitiesAccess(accessRules, identity, getOperation) 
                && GetEntityByAccess(accessRules, item, identity, getOperation);
        }
        private async Task<bool> GetUpdateAccessAsync(IIdentifyable item, Contracts.Account.IIdentity identity)
        {
            Func<TEntity, bool> getOperation = ar => ar.Updatable;
            var accessRules = await GetAccessRulesAsync(item.GetType().Name).ConfigureAwait(false);

            return GetEntityTypeAccess(accessRules, identity, getOperation)
                && GetEntitiesAccess(accessRules, identity, getOperation)
                && GetEntityByAccess(accessRules, item, identity, getOperation);
        }
        private async Task<bool> GetDeleteAccessAsync(IIdentifyable item, Contracts.Account.IIdentity identity)
        {
            Func<TEntity, bool> getOperation = ar => ar.Deletable;
            var accessRules = await GetAccessRulesAsync(item.GetType().Name).ConfigureAwait(false);

            return GetEntityTypeAccess(accessRules, identity, getOperation)
                && GetEntitiesAccess(accessRules, identity, getOperation)
                && GetEntityByAccess(accessRules, item, identity, getOperation);
        }

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
