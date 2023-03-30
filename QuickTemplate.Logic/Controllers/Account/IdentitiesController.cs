//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace QuickTemplate.Logic.Controllers.Account
{
    using QuickTemplate.Logic.Entities.Account;
    using QuickTemplate.Logic.Modules.Account;
    using TEntity = Entities.Account.SecureIdentity;
    using TOutModel = Models.Account.Identity;

    [Modules.Security.Authorize("SysAdmin", "AppAdmin")]
    internal sealed partial class IdentitiesController : EntitiesController<TEntity, TOutModel>, Contracts.Account.IIdentitiesAccess
    {
        private List<IdType> identityIds = new();
        public IdentitiesController()
        {
        }
        public IdentitiesController(ControllerObject other)
            : base(other)
        {
        }

        private void ChangedIdentity(IdType id)
        {
            if (identityIds.Contains(id) == false)
            {
                identityIds.Add(id);
            }
        }

        #region Overrides
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
        internal override IEnumerable<TOutModel> BeforeReturn(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                BeforeReturn(entity);
            }
            return base.BeforeReturn(entities);
        }
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
        internal Task<SecureIdentity?> GetValidIdentityByIdAsync(IdType identityId)
        {
            return EntitySet.Include(e => e.IdentityXRoles)
                            .ThenInclude(e => e.Role)
                            .FirstOrDefaultAsync(e => e.State == Modules.Common.State.Active
                                                   && e.AccessFailedCount < 4
                                                   && e.Id == identityId);
        }
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
