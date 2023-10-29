//@BaseCode
//MdStart
namespace QuickTemplate.Logic.DataContext
{
    /// <summary>
    /// Entity Framework data context for the domain project
    /// </summary>
    internal partial class ProjectDbContext : DbContext
    {
#if SQLSERVER_ON
        private static readonly string ConnectionString = "Data Source=127.0.0.1,1433; Database=QuickTemplateDb;User Id=sa; Password=1234!Passme";
#endif
#if SQLITE_ON
        private static readonly string ConnectionString = "Data Source=/Users/gerhardgehrer/Projects/Data/QuickTemplate.db";
#endif
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectDbContext"/> class
        /// </summary>
        static ProjectDbContext()
        {
            BeforeClassInitialize();
            try
            {
                var configuration = CommonBase.Modules.Configuration.Configurator.LoadAppSettings();
                var connectionString = string.Empty;
                
#if SQLSERVER_ON
                connectionString = configuration["ConnectionStrings:SqlServerDefaultConnection"];
#endif
#if SQLITE_ON
                connectionString = configuration["ConnectionStrings:SqliteDefaultConnection"];
#endif
                
                if (string.IsNullOrEmpty(connectionString) == false)
                {
                    ConnectionString = connectionString;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(message: $"Error in {System.Reflection.MethodBase.GetCurrentMethod()?.Name}: {ex.Message}");
            }
            AfterClassInitialize();
        }
        /// <summary>
        /// This method is called before the initialization of the containing class.
        /// </summary>
        static partial void BeforeClassInitialize();
        /// <summary>
        /// This method is called after the initialization of the class.
        /// </summary>
        static partial void AfterClassInitialize();
        
        /// <summary>
        /// Data sets for account entities
        /// </summary>
#if ACCOUNT_ON
        public DbSet<Entities.Account.SecureIdentity> IdentitySet { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of the <see cref="Entities.Account.Role"/> entities.
        /// </summary>
        public DbSet<Entities.Account.Role> RoleSet { get; set; }
        /// <summary>
        /// Gets or sets the DbSet representing the IdentityXRole entities in the database.
        /// </summary>
        /// <value>The DbSet representing the IdentityXRole entities.</value>
        public DbSet<Entities.Account.IdentityXRole> IdentityXRolesSet { get; set; }
        ///<summary>
        /// Gets or sets the DbSet of User entities within the Account namespace.
        ///</summary>
        ///<value>
        /// The DbSet of User entities.
        ///</value>
        public DbSet<Entities.Account.User> UserSet { get; set; }
        /// <summary>
        /// Gets or sets the login session DbSet.
        /// </summary>
        /// <value>The login session DbSet.</value>
        public DbSet<Entities.Account.LoginSession> LoginSessionSet { get; set; }
#if ACCESSRULES_ON
        /// <summary>
        /// Gets or sets the DbSet of AccessRule entities.
        /// </summary>
        public DbSet<Entities.Access.AccessRule> AccessRuleSet { get; set; }
#endif
#if LOGGING_ON
        /// <summary>
        /// Gets or sets the DbSet for the ActionLog entity in the logging context.
        /// </summary>
        /// <value>
        /// The DbSet for the ActionLog entity.
        /// </value>
        public DbSet<Entities.Logging.ActionLog> ActionLogSet { get; set; }
#endif
#if REVISION_ON
        /// <summary>
        /// Gets or sets the DbSet of Revision History entities.
        /// </summary>
        /// <value>The DbSet of Revision History entities.</value>
        public DbSet<Entities.Revision.History> HistorySet { get; set; }
#endif
#endif
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectDbContext"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor calls the <see cref="Constructing"/> method and the <see cref="Constructed"/> method.
        /// </remarks>
        public ProjectDbContext()
        {
            Constructing();
            Constructed();
        }
        ///<summary>
        ///This method is called during the construction of the object.
        ///</summary>
        partial void Constructing();
        /// <summary>
        /// This method is used to perform specific actions after the object is constructed.
        /// </summary>
        partial void Constructed();
        
        /// <summary>
        /// This method is called for each instance of the context that is created. The base implementation does nothing.
        /// </summary>
        /// <param name="optionsBuilder">A builder used to create or modify options for this context. Databases (and other extensions)
        /// typically define extension methods on this object that allow you to configure the context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var handled = false;
            
            BeforeOnConfiguring(optionsBuilder, ref handled);
            if (handled == false)
            {
#if SQLSERVER_ON
                optionsBuilder.UseSqlServer(ConnectionString);
#endif
#if SQLITE_ON
                optionsBuilder.UseSqlite(ConnectionString);
#endif
            }
            AfterOnConfiguring(optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }
        /// <summary>
        /// This method is invoked just before the <see cref="DbContext.OnConfiguring(DbContextOptionsBuilder)"/>
        /// method is called. It allows customization of the <see cref="DbContextOptionsBuilder"/>
        /// before it is used to configure the context.
        /// </summary>
        /// <param name="optionsBuilder">The <see cref="DbContextOptionsBuilder"/> to be customized.</param>
        /// <param name="handled">A boolean value indicating whether the method has already been handled.</param>
        static partial void BeforeOnConfiguring(DbContextOptionsBuilder optionsBuilder, ref bool handled);
        /// <summary>
        /// This method is called after the <see cref="DbContext"/> has been configured.
        /// </summary>
        /// <param name="optionsBuilder">The options builder used to configure the <see cref="DbContext"/>.</param>
        /// <remarks>
        /// This method can be implemented by partial classes to provide additional configurations or operations
        /// that need to be executed after the <see cref="DbContext"/> has been configured.
        /// </remarks>
        static partial void AfterOnConfiguring(DbContextOptionsBuilder optionsBuilder);
        
        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but before the model
        /// has been locked down and used to initialize the context. The default implementation of this method does
        /// nothing, but it can be overridden in a derived class such that the model can be further configured
        /// before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var handled = false;
            
            BeforeOnModelCreating(modelBuilder, ref handled);
            if (handled == false)
            {
                foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                {
                    relationship.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }
            AfterOnModelCreating(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
        /// <summary>
        /// This method is called before the OnModelCreating method is invoked on the ModelBuilder instance.
        /// It provides an opportunity to perform any customization or configuration before the model is created.
        /// </summary>
        /// <param name="modelBuilder">The ModelBuilder instance used to build the model.</param>
        /// <param name="handled">A boolean value indicating whether this event has been handled by other handlers.</param>
        /// <remarks>
        /// The BeforeOnModelCreating method is a partial method which can be implemented in other partial classes or generated code,
        /// allowing customization of the model creation process without modifying the original class file.
        /// </remarks>
        static partial void BeforeOnModelCreating(ModelBuilder modelBuilder, ref bool handled);
        /// <summary>
        /// This method is called after OnModelCreating method is executed in ModelBuilder,
        /// allowing additional configurations to be applied to the model before it is used to
        /// generate the database schema.
        /// </summary>
        /// <param name="modelBuilder">The ModelBuilder instance used for building the model.</param>
        static partial void AfterOnModelCreating(ModelBuilder modelBuilder);
        
        /// <summary>
        /// Discards all changes in the current context.
        /// </summary>
        /// <returns>Number of changed entities.</returns>
        public Task<int> RejectChangesAsync()
        {
            return Task.Run(() =>
            {
                int count = 0;
                
                foreach (var entry in ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList())
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                        count++;
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                        case EntityState.Added:
                        count++;
                        entry.State = EntityState.Detached;
                        break;
                        case EntityState.Deleted:
                        count++;
                        entry.State = EntityState.Unchanged;
                        break;
                    }
                }
                return count;
            });
        }
        
        /// <summary>
        /// Determines the DbSet depending on the type E
        /// </summary>
        /// <typeparam name="E">The entity type E</typeparam>
        /// <returns>The DbSet depending on the type E</returns>
        public DbSet<E> GetDbSet<E>() where E : Entities.EntityObject
        {
            var handled = false;
            var result = default(DbSet<E>);
            
            GetDbSet(ref result, ref handled);
            if (handled == false || result == null)
            {
                GetGeneratorDbSet(ref result, ref handled);
            }
            if (handled == false || result == null)
            {
                // if the ACCOUNT switched ON
#if ACCOUNT_ON
                if (typeof(E) == typeof(Entities.Account.SecureIdentity))
                {
                    handled = true;
                    result = IdentitySet as DbSet<E>;
                }
                else if (typeof(E) == typeof(Entities.Account.Role))
                {
                    handled = true;
                    result = RoleSet as DbSet<E>;
                }
                else if (typeof(E) == typeof(Entities.Account.IdentityXRole))
                {
                    handled = true;
                    result = IdentityXRolesSet as DbSet<E>;
                }
                else if (typeof(E) == typeof(Entities.Account.User))
                {
                    handled = true;
                    result = UserSet as DbSet<E>;
                }
                else if (typeof(E) == typeof(Entities.Account.LoginSession))
                {
                    handled = true;
                    result = LoginSessionSet as DbSet<E>;
                }
#if ACCESSRULES_ON
                else if (typeof(E) == typeof(Entities.Access.AccessRule))
                {
                    handled = true;
                    result = AccessRuleSet as DbSet<E>;
                }
#endif
#if LOGGING_ON
                else if (typeof(E) == typeof(Entities.Logging.ActionLog))
                {
                    handled = true;
                    result = ActionLogSet as DbSet<E>;
                }
#endif
#if REVISION_ON
                else if (typeof(E) == typeof(Entities.Revision.History))
                {
                    handled = true;
                    result = HistorySet as DbSet<E>;
                }
#endif
#endif
            }
            return result ?? Set<E>();
        }
        /// <summary>
        /// Determines the domain project DbSet depending on the type E
        /// </summary>
        /// <typeparam name="E">The entity type E</typeparam>
        /// <param name="dbSet">The DbSet depending on the type E</param>
        /// <param name="handled">Indicates whether the method found the DbSet</param>
        partial void GetDbSet<E>(ref DbSet<E>? dbSet, ref bool handled) where E : Entities.EntityObject;
        /// <summary>
        /// Determines the domain project DbSet depending on the type E
        /// </summary>
        /// <typeparam name="E">The entity type E</typeparam>
        /// <param name="dbSet">The DbSet depending on the type E</param>
        /// <param name="handled">Indicates whether the method found the DbSet</param>
        partial void GetGeneratorDbSet<E>(ref DbSet<E>? dbSet, ref bool handled) where E : Entities.EntityObject;
    }
}
//MdEnd

