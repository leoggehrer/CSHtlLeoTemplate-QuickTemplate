//@CodeCopy
//MdStart
namespace QuickTemplate.AspMvc
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
            AddConfigures(app);
        }
        /// <summary>
        /// Adds services to the web application builder.
        /// </summary>
        /// <param name="builder">The web application builder.</param>
        static partial void AddServices(WebApplicationBuilder builder);
        /// <summary>
        /// Adds the specified configurations to the web application.
        /// </summary>
        /// <param name="app">The web application to which configurations are added.</param>
        /// <remarks>
        /// This method is used to add custom configurations to the web application.
        /// It is called when configuring the web application.
        /// </remarks>
        /// <seealso cref="WebApplication"/>
        static partial void AddConfigures(WebApplication app);
    }
}
//MdEnd

