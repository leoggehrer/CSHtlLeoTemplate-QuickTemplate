//@BaseCode
//MdStart
#if ACCOUNT_ON
using CommonBase.ThreadSafe;
using Microsoft.IdentityModel.Tokens;
using QuickTemplate.Logic.Entities.Account;
using QuickTemplate.Logic.Modules.Exceptions;
using QuickTemplate.Logic.Modules.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace QuickTemplate.Logic.Modules.Account
{
    internal static partial class AccountManager
    {
        static AccountManager()
        {
            ClassConstructing();
            UpdateSessionAysnc();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        private static int UpdateDelay => 60000;
        private static DateTime LastLoginUpdate { get; set; } = DateTime.Now;
        private static ThreadSafeList<LoginSession> LoginSessions { get; } = new ThreadSafeList<LoginSession>();

        #region Create accounts
        public static async Task InitAppAccessAsync(string name, string email, string password, bool enableJwtAuth)
        {
            if (CheckPasswordSyntax(password) == false)
                throw new AuthorizationException(ErrorType.InvalidPasswordSyntax, PasswordRules.SyntaxRoles);

            using var identitiesCtrl = new Controllers.Account.IdentitiesController()
            {
                SessionToken = Authorization.SystemAuthorizationToken,
            };
            var identityCount = await identitiesCtrl.CountAsync().ConfigureAwait(false);

            if (identityCount == 0)
            {
                using var rolesCtrl = new Controllers.Account.RolesController(identitiesCtrl);
                using var identityXRolesCtrl = new Controllers.Account.IdentityXRolesController(identitiesCtrl);

                try
                {
                    var (Hash, Salt) = CreatePasswordHash(password);
                    var role = new Role
                    {
                        Designation = StaticLiterals.RoleSysAdmin,
                        Description = "Created by the system (first identity).",
                    };
                    var identity = new SecureIdentity
                    {
                        Guid = Guid.NewGuid(),
                        Name = name,
                        Email = email,
                        PasswordHash = Hash,
                        PasswordSalt = Salt,
                        EnableJwtAuth = enableJwtAuth,
                    };
                    var IdentityXRole = new IdentityXRole
                    {
                        Identity = identity,
                        Role = role,
                    };

                    await rolesCtrl.ExecuteInsertAsync(role).ConfigureAwait(false);
                    await identityXRolesCtrl.ExecuteInsertAsync(IdentityXRole).ConfigureAwait(false);
                    await identitiesCtrl.ExecuteInsertAsync(identity).ConfigureAwait(false);
                    await identitiesCtrl.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                throw new AuthorizationException(ErrorType.InitAppAccess);
            }
        }
        public static async Task AddAppAccessAsync(string sessionToken, string name, string email, string password, int timeOutInMinutes, bool enableJwtAuth, params string[] roles)
        {
            if (CheckPasswordSyntax(password) == false)
                throw new AuthorizationException(ErrorType.InvalidPasswordSyntax, PasswordRules.SyntaxRoles);

            using var identitiesCtrl = new Controllers.Account.IdentitiesController()
            {
                SessionToken = sessionToken,
            };
            try
            {
                var (Hash, Salt) = CreatePasswordHash(password);
                var identity = new SecureIdentity
                {
                    Guid = Guid.NewGuid(),
                    Name = name,
                    Email = email,
                    PasswordHash = Hash,
                    PasswordSalt = Salt,
                    TimeOutInMinutes = timeOutInMinutes,
                    EnableJwtAuth = enableJwtAuth,
                };

                if (roles.Length > 0)
                {
                    using var rolesCtrl = new Controllers.Account.RolesController(identitiesCtrl);
                    using var identityXRolesCtrl = new Controllers.Account.IdentityXRolesController(identitiesCtrl);
                    var rolesInDb = await rolesCtrl.GetAllAsync().ConfigureAwait(false);

                    foreach (var role in roles)
                    {
                        var accRole = role.Trim();
                        var dbRole = rolesInDb.FirstOrDefault(r => r.Designation.Equals(accRole, StringComparison.CurrentCultureIgnoreCase));

                        if (dbRole != null)
                        {
                            var identityXRole = new IdentityXRole
                            {
                                RoleId = dbRole.Id,
                                Identity = identity,
                            };
                            await identityXRolesCtrl.InsertAsync(identityXRole).ConfigureAwait(false);
                        }
                        else
                        {
                            var newRole = new Role
                            {
                                Designation = accRole,
                                Description = "Created by the system.",
                            };
                            var identityXRole = new IdentityXRole
                            {
                                Role = newRole,
                                Identity = identity,
                            };
                            await rolesCtrl.InsertAsync(newRole).ConfigureAwait(false);
                            await identityXRolesCtrl.InsertAsync(identityXRole).ConfigureAwait(false);
                        }
                        await identitiesCtrl.InsertAsync(identity).ConfigureAwait(false);
                    }
                }
                else
                {
                    await identitiesCtrl.InsertAsync(identity).ConfigureAwait(false);
                }
                await identitiesCtrl.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion Create accounts

        #region Logon and Logout
        public static async Task<LoginSession> LogonAsync(string jsonWebToken)
        {
            var result = default(LoginSession);

            if (JsonWebToken.CheckToken(jsonWebToken, out SecurityToken? validatedToken))
            {
                if (validatedToken != null && validatedToken.ValidTo < DateTime.UtcNow)
                    throw new AuthorizationException(ErrorType.AuthorizationTimeOut);

                if (validatedToken is JwtSecurityToken jwtValidatedToken)
                {
                    var email = jwtValidatedToken.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Email);

                    if (email != null && email.Value != null)
                    {
                        using var identitiesCtrl = new Controllers.Account.IdentitiesController()
                        {
                            SessionToken = Authorization.SystemAuthorizationToken
                        };
                        var identity = await identitiesCtrl.EntitySet
                                                           .FirstOrDefaultAsync(e => e.State == Common.State.Active
                                                                                && e.EnableJwtAuth == true
                                                                                && e.Email.ToLower() == email.Value.ToString().ToLower())
                                                           .ConfigureAwait(false);

                        if (identity != null)
                        {
                            result = await QueryLoginByEmailAsync(identity.Email, identity.Password, string.Empty).ConfigureAwait(false);

                            if (result != null)
                            {
                                result.IsRemoteAuth = true;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new AuthorizationException(ErrorType.InvalidJsonWebToken);
            }
            return result ?? throw new AuthorizationException(ErrorType.InvalidAccount);
        }
        public static Task<LoginSession> LogonAsync(string email, string password)
        {
            return LogonAsync(email, password, string.Empty);
        }
        public static async Task<LoginSession> LogonAsync(string email, string password, string optionalInfo)
        {
            var result = await QueryLoginByEmailAsync(email, password, optionalInfo).ConfigureAwait(false);

            return result ?? throw new AuthorizationException(ErrorType.InvalidAccount);
        }
        [Authorize]
        public static async Task LogoutAsync(string sessionToken)
        {
            await Authorization.CheckAuthorizationAsync(sessionToken, typeof(AccountManager), nameof(LogoutAsync)).ConfigureAwait(false);

            try
            {
                var saveChanges = false;
                var logoutTime = DateTime.UtcNow;
                using var sessionCtrl = new Controllers.Account.LoginSessionsController()
                {
                    SessionToken = Authorization.SystemAuthorizationToken
                };
                var dbSessions = await sessionCtrl.EntitySet
                                                  .Where(e => e.SessionToken.Equals(sessionToken))
                                                  .ToArrayAsync()
                                                  .ConfigureAwait(false);

                foreach (var dbSession in dbSessions)
                {
                    if (dbSession != null && dbSession.IsActive)
                    {
                        saveChanges = true;
                        dbSession.LogoutTime = logoutTime;

                        await sessionCtrl.UpdateAsync(dbSession).ConfigureAwait(false);
                    }
                }
                if (saveChanges)
                {
                    await sessionCtrl.SaveChangesAsync().ConfigureAwait(false);
                }

                var memSessions = LoginSessions.Where(ls => ls.SessionToken.Equals(sessionToken));

                foreach (var memSession in memSessions)
                {
                    if (memSession != null)
                    {
                        memSession.LogoutTime = logoutTime;
                    }
                }
            }
            catch (AuthorizationException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in {typeof(AccountManager)?.Name}: {ex.Message}");
            }
        }
        #endregion Logon and Logout

        #region Query logon data
        public static async Task<bool> IsSessionAliveAsync(string sessionToken)
        {
            return await QueryAliveSessionAsync(sessionToken).ConfigureAwait(false) != null;
        }
        [Authorize]
        public static async Task<IEnumerable<string>> QueryRolesAsync(string sessionToken)
        {
            await Authorization.CheckAuthorizationAsync(sessionToken, typeof(AccountManager), nameof(QueryRolesAsync)).ConfigureAwait(false);

            var loginSession = await QueryAliveSessionAsync(sessionToken).ConfigureAwait(false);

            return loginSession != null ? loginSession.Roles.Select(r => r.Designation) : Array.Empty<string>();
        }
        [Authorize]
        public static async Task<bool> HasRoleAsync(string sessionToken, string role)
        {
            await Authorization.CheckAuthorizationAsync(sessionToken, typeof(AccountManager), nameof(HasRoleAsync)).ConfigureAwait(false);

            var loginSession = await QueryAliveSessionAsync(sessionToken).ConfigureAwait(false);

            return loginSession != null && loginSession.Roles.Any(r => r.Designation.Equals(role, StringComparison.CurrentCultureIgnoreCase));
        }
        [Authorize]
        public static async Task<LoginSession?> QueryLoginAsync(string sessionToken)
        {
            await Authorization.CheckAuthorizationAsync(sessionToken, typeof(AccountManager), nameof(QueryLoginAsync)).ConfigureAwait(false);

            return await QueryAliveSessionAsync(sessionToken).ConfigureAwait(false);
        }
        #endregion Query logon data

        #region Change and reset password
        [Authorize]
        public static async Task ChangePasswordAsync(string sessionToken, string oldPassword, string newPassword)
        {
            await Authorization.CheckAuthorizationAsync(sessionToken, typeof(AccountManager), nameof(ChangePasswordAsync)).ConfigureAwait(false);

            if (CheckPasswordSyntax(newPassword) == false)
                throw new AuthorizationException(ErrorType.InvalidPasswordSyntax, PasswordRules.SyntaxRoles);

            var login = await QueryAliveSessionAsync(sessionToken).ConfigureAwait(false)
                        ?? throw new AuthorizationException(ErrorType.InvalidToken);

            using var identitiesCtrl = new Controllers.Account.IdentitiesController()
            {
                SessionToken = Authorization.SystemAuthorizationToken
            };
            var identity = await identitiesCtrl.EntitySet
                                               .FirstOrDefaultAsync(e => e.Id == login.IdentityId)
                                               .ConfigureAwait(false);

            if (identity != null)
            {
                if (VerifyPasswordHash(oldPassword, identity.PasswordHash, identity.PasswordSalt) == false)
                    throw new AuthorizationException(ErrorType.InvalidPassword);

                var (Hash, Salt) = CreatePasswordHash(newPassword);

                identity.Password = newPassword;
                identity.PasswordHash = Hash;
                identity.PasswordSalt = Salt;

                await identitiesCtrl.UpdateAsync(identity).ConfigureAwait(false);
                await identitiesCtrl.SaveChangesAsync().ConfigureAwait(false);
                if (login.Identity != null)
                {
                    login.Identity.PasswordHash = Hash;
                    login.Identity.PasswordSalt = Salt;
                }
            }
        }
        [Authorize("SysAdmin", "AppAdmin")]
        public static async Task ChangePasswordForAsync(string sessionToken, string email, string newPassword)
        {
            await Authorization.CheckAuthorizationAsync(sessionToken, typeof(AccountManager), nameof(ChangePasswordForAsync)).ConfigureAwait(false);

            if (CheckPasswordSyntax(newPassword) == false)
                throw new AuthorizationException(ErrorType.InvalidPasswordSyntax, PasswordRules.SyntaxRoles);

            var login = await QueryAliveSessionAsync(sessionToken).ConfigureAwait(false)
                        ?? throw new AuthorizationException(ErrorType.InvalidToken);

            using var identitiesCtrl = new Controllers.Account.IdentitiesController()
            {
                SessionToken = sessionToken
            };
            var identity = await identitiesCtrl.EntitySet
                                               .FirstOrDefaultAsync(e => e.State == Common.State.Active
                                                                      && e.AccessFailedCount < 4
                                                                      && e.Email.ToLower() == email.ToLower())
                                               .ConfigureAwait(false);

            if (identity == null)
                throw new AuthorizationException(ErrorType.InvalidAccount);

            var (Hash, Salt) = CreatePasswordHash(newPassword);

            identity.AccessFailedCount = 0;
            identity.Password = newPassword;
            identity.PasswordHash = Hash;
            identity.PasswordSalt = Salt;

            await identitiesCtrl.UpdateAsync(identity).ConfigureAwait(false);
            await identitiesCtrl.SaveChangesAsync().ConfigureAwait(false);
            if (login.Identity != null)
            {
                login.Identity.PasswordHash = Hash;
                login.Identity.PasswordSalt = Salt;
            }
        }
        [Authorize("SysAdmin", "AppAdmin")]
        public static async Task ResetFailedCountForAsync(string sessionToken, string email)
        {
            await Authorization.CheckAuthorizationAsync(sessionToken, typeof(AccountManager), nameof(ResetFailedCountForAsync)).ConfigureAwait(false);

            var login = await QueryAliveSessionAsync(sessionToken).ConfigureAwait(false)
                        ?? throw new AuthorizationException(ErrorType.InvalidToken);

            using var identitiesCtrl = new Controllers.Account.IdentitiesController()
            {
                SessionToken = sessionToken
            };
            var identity = await identitiesCtrl.EntitySet
                                               .FirstOrDefaultAsync(e => e.State == Common.State.Active
                                                                      && e.Email.ToLower() == email.ToLower())
                                               .ConfigureAwait(false);

            if (identity == null)
                throw new AuthorizationException(ErrorType.InvalidAccount);

            identity.AccessFailedCount = 0;
            await identitiesCtrl.UpdateAsync(identity).ConfigureAwait(false);
            await identitiesCtrl.SaveChangesAsync().ConfigureAwait(false);
        }
        #endregion Change and reset password

        #region Internal logon
        internal static LoginSession? QueryLoginSession(string sessionToken)
        {
            return LoginSessions.FirstOrDefault(ls => ls.IsActive
                                                   && ls.SessionToken.Equals(sessionToken));
        }
        internal static async Task<LoginSession?> QueryAliveSessionAsync(string sessionToken)
        {
            var result = LoginSessions.FirstOrDefault(ls => ls.IsActive
                                                         && ls.SessionToken.Equals(sessionToken));

            if (result == null)
            {
                using var sessionsCtrl = new Controllers.Account.LoginSessionsController()
                {
                    SessionToken = Authorization.SystemAuthorizationToken
                };
                var session = await sessionsCtrl.EntitySet
                                                .FirstOrDefaultAsync(e => e.SessionToken.Equals(sessionToken))
                                                .ConfigureAwait(false);

                if (session != null && session.IsActive)
                {
                    using var identitiesCtrl = new Controllers.Account.IdentitiesController(sessionsCtrl);
                    var identity = await identitiesCtrl.EntitySet
                                                       .Include(e => e.IdentityXRoles)
                                                       .ThenInclude(e => e.Role)
                                                       .FirstOrDefaultAsync(e => e.Id == session.IdentityId)
                                                       .ConfigureAwait(false);

                    if (identity != null)
                    {
                        session.Name = identity.Name;
                        session.Email = identity.Email;
                        session.Identity = identity;
                        session.Roles.AddRange(identity.IdentityXRoles.Select(e => e.Role!));
                        session.JsonWebToken = JsonWebToken.GenerateToken(new Claim[]
                        {
                            new Claim(ClaimTypes.Email, identity.Email),
                            new Claim(ClaimTypes.System, nameof(QuickTemplate)),
                        }.Union(session.Roles.Select(e => new Claim(ClaimTypes.Role, e.Designation))));

                        result = session.Clone();
                        LoginSessions.Add(session);
                    }
                }
            }
            return result;
        }
        internal static async Task<LoginSession?> QueryAliveSessionAsync(string email, string password)
        {
            var result = LoginSessions.FirstOrDefault(e => e.IsActive
                                                        && e.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));

            if (result == null)
            {
                using var identitiesCtrl = new Controllers.Account.IdentitiesController()
                {
                    SessionToken = Authorization.SystemAuthorizationToken,
                };
                var identity = await identitiesCtrl.GetValidIdentityByEmailAsync(email).ConfigureAwait(false);

                if (identity != null && VerifyPasswordHash(password, identity.PasswordHash, identity.PasswordSalt))
                {
                    using var sessionsCtrl = new Controllers.Account.LoginSessionsController(identitiesCtrl);
                    var session = await sessionsCtrl.EntitySet
                                                    .FirstOrDefaultAsync(e => e.LogoutTime == null
                                                                           && e.IdentityId == identity.Id)
                                                    .ConfigureAwait(false);

                    if (session != null && session.IsActive)
                    {
                        session.Name = identity.Name;
                        session.Email = identity.Email;
                        session.Identity = identity;
                        session.Roles.AddRange(identity.IdentityXRoles.Select(e => e.Role!));
                        session.JsonWebToken = JsonWebToken.GenerateToken(new Claim[]
                        {
                            new Claim(ClaimTypes.Email, identity.Email),
                            new Claim(ClaimTypes.System, nameof(QuickTemplate)),
                        }.Union(session.Roles.Select(e => new Claim(ClaimTypes.Role, e.Designation))));

                        result = session.Clone();
                        LoginSessions.Add(session);
                    }
                }
            }
            return result;
        }
        internal static async Task<LoginSession?> QueryLoginByEmailAsync(string email, string password, string optionalInfo)
        {
            var result = default(LoginSession);
            var querySession = await QueryAliveSessionAsync(email, password).ConfigureAwait(false);

            if (querySession == null)
            {
                using var identitiesCtrl = new Controllers.Account.IdentitiesController()
                {
                    SessionToken = Authorization.SystemAuthorizationToken,
                };
                var identity = await identitiesCtrl.GetValidIdentityByEmailAsync(email).ConfigureAwait(false);

                if (identity != null && VerifyPasswordHash(password, identity.PasswordHash, identity.PasswordSalt))
                {
                    using var sessionsCtrl = new Controllers.Account.LoginSessionsController(identitiesCtrl);
                    var session = new LoginSession
                    {
                        IdentityId = identity.Id,
                        Name = identity.Name,
                        Email = identity.Email,
                        OptionalInfo = optionalInfo,
                        Identity = identity,
                    };
                    session.Roles.AddRange(identity.IdentityXRoles.Select(e => e.Role!));
                    session.JsonWebToken = JsonWebToken.GenerateToken(new Claim[]
                    {
                        new Claim(ClaimTypes.Email, identity.Email),
                        new Claim(ClaimTypes.System, nameof(QuickTemplate)),
                    }.Union(session.Roles.Select(e => new Claim(ClaimTypes.Role, e.Designation))));

                    var entity = await sessionsCtrl.InsertAsync(session).ConfigureAwait(false);

                    if (identity.AccessFailedCount > 0)
                    {
                        identity.AccessFailedCount = 0;
                        await identitiesCtrl.UpdateAsync(identity).ConfigureAwait(false);
                    }
                    await sessionsCtrl.SaveChangesAsync().ConfigureAwait(false);

                    result = entity.Clone();
                    LoginSessions.Add(entity);
                }
            }
            else if (VerifyPasswordHash(password, querySession.PasswordHash, querySession.PasswordSalt))
            {
                querySession.LastAccess = DateTime.UtcNow;
                result = querySession.Clone();
            }
            return result;
        }
        internal static async Task RefreshAliveSessionsAsync(IdType identityId)
        {
            using var identitiesCtrl = new Controllers.Account.IdentitiesController()
            {
                SessionToken = Authorization.SystemAuthorizationToken,
            };
            foreach (var session in LoginSessions.Where(s => s.IdentityId == identityId))
            {
                var identity = await identitiesCtrl.GetValidIdentityByIdAsync(identityId).ConfigureAwait(false);

                if (identity != null)
                {
                    session.Identity = identity;
                    session.Roles.Clear();
                    session.Roles.AddRange(identity.IdentityXRoles.Select(e => e.Role!));
                    session.JsonWebToken = JsonWebToken.GenerateToken(new Claim[]
                    {
                        new Claim(ClaimTypes.Email, identity.Email),
                        new Claim(ClaimTypes.System, nameof(QuickTemplate)),
                    }.Union(session.Roles.Select(e => new Claim(ClaimTypes.Role, e.Designation))));
                }
            }
        }
        #endregion Internal logon

        #region Update thread
        private static Task UpdateSessionAysnc()
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        using var sessionsCtrl = new Controllers.Account.LoginSessionsController()
                        {
                            SessionToken = Authorization.SystemAuthorizationToken,
                        };
                        var saveChanges = false;
                        var dbSessions = await sessionsCtrl.QueryOpenLoginSessionsAsync().ConfigureAwait(false);
                        var uncheckSessions = LoginSessions.Where(i => dbSessions.Any() == false
                                                                    || dbSessions.Any(e => e.Id != i.Id));

                        foreach (var dbSession in dbSessions)
                        {
                            var dbUpdate = false;
                            var memSessionRemove = false;
                            var memSession = LoginSessions.FirstOrDefault(e => e.Id == dbSession.Id);

                            if (memSession != null && dbSession.LastAccess != memSession.LastAccess)
                            {
                                dbUpdate = true;
                                dbSession.LastAccess = memSession.LastAccess;
                            }
                            if (dbSession.IsTimeout)
                            {
                                dbUpdate = true;
                                if (memSession != null)
                                {
                                    memSessionRemove = true;
                                }
                                if (dbSession.LogoutTime.HasValue == false)
                                {
                                    dbSession.LogoutTime = DateTime.UtcNow;
                                }
                            }
                            if (dbUpdate)
                            {
                                saveChanges = true;
                                await sessionsCtrl.UpdateAsync(dbSession).ConfigureAwait(false);
                            }
                            if (memSessionRemove && memSession != null)
                            {
                                LoginSessions.Remove(memSession);
                            }
                        }
                        if (saveChanges)
                        {
                            await sessionsCtrl.SaveChangesAsync().ConfigureAwait(false);
                        }
                        foreach (var memItem in uncheckSessions)
                        {
                            var dbSession = await sessionsCtrl.EntitySet
                                                              .FirstOrDefaultAsync(e => e.Id == memItem.Id)
                                                              .ConfigureAwait(false);

                            if (dbSession != null)
                            {
                                memItem.LastAccess = dbSession.LastAccess;
                                memItem.LogoutTime = dbSession.LogoutTime;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error in {typeof(AccountManager)?.Name}: {ex.Message}");
                    }
                    LastLoginUpdate = DateTime.Now;
                    await Task.Delay(UpdateDelay).ConfigureAwait(false);
                }
            });
        }
        #endregion Update thread

        #region Helpers
        internal static (byte[] Hash, byte[] Salt) CreatePasswordHash(string password)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();

            var passwordSalt = hmac.Key;
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return (passwordHash, passwordSalt);
        }
        internal static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var result = computedHash.Length == passwordHash.Length;

            for (int i = 0; i < passwordHash.Length && result; i++)
            {
                result = passwordHash[i] == computedHash[i];
            }
            return result;
        }
        /// <summary>
        /// Checks if the password matches the password rules.
        /// </summary>
        /// <param name="password">The password to check</param>
        /// <returns>True if the password matches Password Rules, false otherwise</returns>
        public static bool CheckPasswordSyntax(string password)
        {
            var handled = false;
            var result = false;

            BeforeCheckPasswordSyntax(password, ref result, ref handled);
            if (handled == false)
            {
                var digitCount = 0;
                var letterCount = 0;
                var lowerLetterCount = 0;
                var upperLetterCount = 0;
                var specialLetterCount = 0;

                foreach (char ch in password)
                {
                    if (char.IsDigit(ch))
                    {
                        digitCount++;
                    }
                    else
                    {
                        if (char.IsLetter(ch))
                        {
                            letterCount++;
                            if (char.IsLower(ch))
                            {
                                lowerLetterCount++;
                            }
                            else
                            {
                                upperLetterCount++;
                            }
                        }
                        else
                        {
                            specialLetterCount++;
                        }
                    }
                }
                result = password.Length >= PasswordRules.MinimumLength
                        && password.Length <= PasswordRules.MaximumLength
                        && letterCount >= PasswordRules.MinLetterCount
                        && upperLetterCount >= PasswordRules.MinUpperLetterCount
                        && lowerLetterCount >= PasswordRules.MinLowerLetterCount
                        && specialLetterCount >= PasswordRules.MinSpecialLetterCount
                        && digitCount >= PasswordRules.MinDigitCount;
            }
            return result;
        }
        static partial void BeforeCheckPasswordSyntax(string password, ref bool result, ref bool handled);

        /// <summary>
        /// Eine gueltige Mailadresse besteht aus einem mindestens zwei Zeichen vor dem @, 
        /// einem Hostname, der genau einen oder mehrere Punkte enthaelt (Domainname mindestens dreistellig)
        /// und als Topleveldomaene (letzter Teil) mindestens zweistellig ist
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <returns>Mailadresse ist gültig</returns>
        public static bool CheckMailAddressSyntax(string mailAddress)
        {
            //return Regex.IsMatch(mailAddress, @"^([\w-\.]+){2,}@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            ////@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
            ////@"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$"); 
            return Regex.IsMatch(mailAddress, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            //return Regex.IsMatch(mailAddress, @"^\w{2,}@[a-zA-Z]{3,}\.[a-zA-Z]{2,}$");
        }
        #endregion Helpers
    }
}
#endif
//MdEnd