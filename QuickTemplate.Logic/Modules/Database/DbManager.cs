//@BaseCode
//MdStart
#if DBOPERATION_ON
using QuickTemplate.Logic.DataContext;

namespace QuickTemplate.Logic.Modules.Database
{
    /// <summary>
    /// The DbManager class provides methods for managing the database.
    /// </summary>
    public static partial class DbManager
    {
        /// <summary>
        /// Deletes the database asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task DeleteDatabaseAsync()
        {
            using var dbContext = new ProjectDbContext();
            
            await dbContext.Database.EnsureDeletedAsync();
        }
        /// <summary>
        /// Asynchronously migrates the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task MigrateDatabaseAsync()
        {
            using var dbContext = new ProjectDbContext();
            
            await dbContext.Database.MigrateAsync();
        }
        /// <summary>
        /// Creates a new database asynchronously using the ProjectDbContext.
        /// </summary>
        /// <returns>
        /// A Task representing the asynchronous operation.
        /// </returns>
        public static async Task CreateDatabaseAsync()
        {
            using var dbContext = new ProjectDbContext();
            
            await dbContext.Database.EnsureCreatedAsync();
        }
        /// <summary>
        /// Executes a raw SQL query asynchronously.
        /// </summary>
        /// <param name="sql">The SQL query to be executed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task ExecuteRawSqlAsync(string sql)
        {
            using var dbContext = new ProjectDbContext();
            
            await dbContext.Database.ExecuteSqlRawAsync(sql);
        }
    }
}
#endif
//MdEnd
