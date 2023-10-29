//@BaseCode
//MdStart
namespace QuickTemplate.ConApp
{
    /// <summary>
    /// Represents the Program class.
    /// </summary>
    public partial class Program
    {
        #region Class-Constructors
        /// <summary>
        /// Represents the entry point of a C# program.
        /// </summary>
        static Program()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// Performs a partial method that gets called when a class is being constructed.
        /// </summary>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when the class is constructed.
        /// </summary>
        /// <remarks>
        /// This method is partial and must be implemented in a separate class,
        /// which is responsible for handling the construction logic of the class.
        /// </remarks>
        static partial void ClassConstructed();
        #endregion Class-Constructors
        public static void Main(string[] args)
        {
            Console.WriteLine(nameof(QuickTemplate));
            Console.WriteLine(DateTime.Now);
            BeforeRun();
#if DEBUG && DBOPERATION_ON
            Task.Run(async () =>
            {
                await Logic.Modules.Database.DbManager.DeleteDatabaseAsync();
                await Logic.Modules.Database.DbManager.CreateDatabaseAsync();
            }).Wait();
#endif
            
#if ACCOUNT_ON
            CreateAccount();
#endif
            CreateImport();
            AfterRun();
            Console.WriteLine(DateTime.Now);
        }
        /// <summary>
        /// This method is called before the execution of the Run method.
        /// </summary>
        static partial void BeforeRun();
        /// <summary>
        /// This method is called after the Run method is executed.
        /// </summary>
        static partial void AfterRun();
#if ACCOUNT_ON
        /// <summary>
        /// Creates an account.
        /// </summary>
        /// <remarks>
        /// This method is used to create a new account for a user.
        /// </remarks>
        /// <seealso cref="Account"/>
        static partial void CreateAccount();
#endif
        /// <summary>
        /// Creates an import.
        /// </summary>
        /// <remarks>
        /// This method is used to create an import.
        /// </remarks>
        /// <returns>
        /// This method does not return any value.
        /// </returns>
        static partial void CreateImport();
    }
}
//MdEnd
