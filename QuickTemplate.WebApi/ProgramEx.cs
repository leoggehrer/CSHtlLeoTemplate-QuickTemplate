//@CodeCopy
//MdStart
using Microsoft.Net.Http.Headers;

namespace QuickTemplate.WebApi
{
    /// <summary>
    /// Extension Program
    /// </summary>
    public partial class Program
    {
        /// <summary>
        /// Services can be added using this method.
        /// </summary>
        /// <param name="builder">The builder</param>
        public static void BeforeBuild(WebApplicationBuilder builder)
        {
#if ACCOUNT_ON
            builder.Services.AddTransient<Logic.Contracts.Account.IRolesAccess, Logic.Facades.Account.RolesFacade>();
            builder.Services.AddTransient<Logic.Contracts.Account.IUsersAccess, Logic.Facades.Account.UsersFacade>();
            builder.Services.AddTransient<Logic.Contracts.Account.IIdentitiesAccess, Logic.Facades.Account.IdentitiesFacade>();
#if ACCESSRULES_ON
            builder.Services.AddTransient<Logic.Contracts.Access.IAccessRulesAccess, Logic.Facades.Access.AccessRulesFacade>();
#endif
#endif
            AddServices(builder);
        }
        /// <summary>
        /// Configures can be added using this method.
        /// </summary>
        /// <param name="app"></param>
        public static void AfterBuild(WebApplication app)
        {
            app.UseCors(policy => policy.AllowAnyOrigin()
               .AllowAnyMethod()
               .WithHeaders(HeaderNames.ContentType));
            
            AddConfigures(app);
        }
        /// <summary>
        /// Adds services to the given WebApplicationBuilder instance.
        /// </summary>
        /// <param name="builder">The WebApplicationBuilder instance to which the services should be added.</param>
        /// <remarks>
        /// This method is a partial method and should be implemented in other partial classes
        /// or code files of the same class containing the rest of the implementation.
        /// </remarks>
        static partial void AddServices(WebApplicationBuilder builder);
        /// <summary>
        /// Adds the configuration for a web application.
        /// </summary>
        /// <param name="app">The <see cref="WebApplication"/> instance to configure.</param>
        /// <remarks>
        /// This method is used to add additional configurations to a <see cref="WebApplication"/> object.
        /// It is a partial method, which means the implementation can be found in a different file.
        /// </remarks>
        static partial void AddConfigures(WebApplication app);
    }
}
//MdEnd
