//@BaseCode
//MdStart
#if ACCOUNT_ON
using QuickTemplate.Logic.Models.Account;
using QuickTemplate.Logic.Modules.Account;

namespace QuickTemplate.Logic
{
    public static partial class AccountAccess
    {
        public static Task<bool> IsSessionAliveAsync(string sessionToken)
        {
            return AccountManager.IsSessionAliveAsync(sessionToken);
        }

        public static Task InitAppAccessAsync(string name, string email, string password, bool enableJwtAuth)
        {
            return AccountManager.InitAppAccessAsync(name, email, password, enableJwtAuth);
        }
        public static Task AddAppAccessAsync(string sessionToken, string name, string email, string password, int timeOutInMinutes, bool enableJwtAuth, params string[] roles)
        {
            return AccountManager.AddAppAccessAsync(sessionToken, name, email, password, timeOutInMinutes, enableJwtAuth, roles);
        }

        public static async Task<LoginSession> LogonAsync(string jsonWebToken)
        {
            var result = await AccountManager.LogonAsync(jsonWebToken).ConfigureAwait(false);

            return LoginSession.Create(result);
        }
        public static async Task<LoginSession> LogonAsync(string email, string password)
        {
            var result = await AccountManager.LogonAsync(email, password).ConfigureAwait(false);

            return LoginSession.Create(result);
        }
        public static async Task<LoginSession> LogonAsync(string email, string password, string optionalInfo)
        {
            var result = await AccountManager.LogonAsync(email, password, optionalInfo).ConfigureAwait(false);

            return LoginSession.Create(result);
        }
        public static async Task<LoginSession?> QueryLoginAsync(string sessionToken)
        {
            var result = await AccountManager.QueryLoginAsync(sessionToken).ConfigureAwait(false);

            return result != null ? LoginSession.Create(result) : null;
        }

        public static Task<bool> HasRoleAsync(string sessionToken, string role)
        {
            return AccountManager.HasRoleAsync(sessionToken, role);
        }
        public static Task<IEnumerable<string>> QueryRolesAsync(string sessionToken)
        {
            return AccountManager.QueryRolesAsync(sessionToken);
        }

        public static Task ChangePasswordAsync(string sessionToken, string oldPassword, string newPassword)
        {
            return AccountManager.ChangePasswordAsync(sessionToken, oldPassword, newPassword);
        }
        public static Task ChangePasswordForAsync(string sessionToken, string email, string newPassword)
        {
            return AccountManager.ChangePasswordAsync(sessionToken, email, newPassword);
        }
        public static Task ResetFailedCountForAsync(string sessionToken, string email)
        {
            return AccountManager.ResetFailedCountForAsync(sessionToken, email);
        }

        public static Task LogoutAsync(string sessionToken)
        {
            return AccountManager.LogoutAsync(sessionToken);
        }

        public static async Task<Identity> GetIdentityByAsync(string sessionToken, IdType id)
        {
            using var ctrl = new Controllers.Account.IdentitiesController() { SessionToken = sessionToken };
            var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

            return entity != null ? Identity.Create(entity) : throw new Modules.Exceptions.LogicException(Modules.Exceptions.ErrorType.InvalidId);
        }
        public static async Task<Identity[]> GetIdentitiesAsync(string sessionToken)
        {
            using var ctrl = new Controllers.Account.IdentitiesController() { SessionToken = sessionToken };
            var entities = await ctrl.GetAllAsync().ConfigureAwait(false);

            return entities.Select(e => Identity.Create(e)).ToArray();
        }
        public static async Task<Identity> UpdateIdentityAsync(string sessionToken, IdType id, Identity identity)
        {
            using var ctrl = new Controllers.Account.IdentitiesController() { SessionToken = sessionToken };
            var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

            if (entity == null)
            {
                throw new Modules.Exceptions.LogicException(Modules.Exceptions.ErrorType.InvalidId);
            }

            entity.CopyFrom(identity, n => n.Equals("Guid", StringComparison.InvariantCultureIgnoreCase) == false);
            entity = await ctrl.UpdateAsync(entity).ConfigureAwait(false);
            await ctrl.SaveChangesAsync().ConfigureAwait(false);

            identity.CopyFrom(entity);
            return identity;
        }
        public static async Task DeleteIdentityAsync(string sessionToken, IdType id)
        {
            using var ctrl = new Controllers.Account.IdentitiesController() { SessionToken = sessionToken };
            var entity = await ctrl.GetByIdAsync(id).ConfigureAwait(false);

            if (entity == null)
            {
                throw new Modules.Exceptions.LogicException(Modules.Exceptions.ErrorType.InvalidId);
            }

            await ctrl.DeleteAsync(id).ConfigureAwait(false);
            await ctrl.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
#endif
//MdEnd