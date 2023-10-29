//@BaseCode
//MdStart
namespace QuickTemplate.CodeGenApp
{
    using TemplateCodeGenerator.Logic;
    using TemplateCodeGenerator.Logic.Git;
    
    /// <summary>
    /// Represents the Program class.
    /// </summary>
    internal partial class Program
    {
        /// <summary>
        /// Initializes the <see cref="Program"/> class.
        /// </summary>
        /// <remarks>
        /// This static constructor is called when the <see cref="Program"/> class is loaded into memory.
        /// It performs necessary initialization tasks before the class can be used.
        /// </remarks>
        static Program()
        {
            ClassConstructing();
            ToGroupFile = false;
            ExcludeGeneratedFilesFromGIT = true;
            HomePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
            Environment.OSVersion.Platform == PlatformID.MacOSX)
            ? Environment.GetEnvironmentVariable("HOME")
            : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            
            UserPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            SourcePath = GetCurrentSolutionPath();
            TargetPaths = Array.Empty<string>();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called before the class is being constructed.
        /// </summary>
        /// <remarks>
        /// This method can be overridden in a partial class to add custom behavior before the class initialization.
        /// </remarks>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when the class is constructed.
        /// </summary>
        static partial void ClassConstructed();
        
        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether the file should be grouped.
        /// </summary>
        private static bool ToGroupFile { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether generated files are excluded from GIT.
        /// </summary>
        private static bool ExcludeGeneratedFilesFromGIT { get; set; }
        /// <summary>
        /// Gets or sets the home path.
        /// </summary>
        private static string? HomePath { get; set; }
        /// <summary>
        /// Gets or sets the user path.
        /// </summary>
        private static string UserPath { get; set; }
        /// <summary>
        /// Gets or sets the source path for the property.
        /// </summary>
        /// <value>The source path.</value>
        private static string SourcePath { get; set; }
        /// <summary>
        /// Gets or sets the target paths for the property.
        /// </summary>
        private static string[] TargetPaths { get; set; }
        /// <summary>
        /// Gets an array of search patterns for source files.
        /// </summary>
        private static string[] SearchPatterns => StaticLiterals.SourceFileExtensions.Split('|');
        #endregion Properties
        
        /// <summary>
        /// The entry point of the program.
        /// </summary>
        /// <param name="args">The arguments passed to the program.</param>
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                foreach (var command in args)
                {
                    ExecuteCommand(command);
                }
            }
            else
            {
                RunApp();
            }
        }
        #region Console methods
        /// <summary>
        /// Runs the application for code generation.
        /// </summary>
        private static void RunApp()
        {
            var input = string.Empty;
            
            while (input != "x")
            {
                Console.Clear();
                Console.WriteLine($"Code generation for: {nameof(QuickTemplate)}");
                Console.WriteLine("=============================================");
                Console.WriteLine();
                Console.WriteLine($"Code generation path: {SourcePath}");
                Console.WriteLine($"Code generation for:  {GetSolutionName(SourcePath)}");
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine($"Write generated code into:        {(ToGroupFile ? "Group files" : "Single files")}");
                Console.WriteLine($"Exclude generated files from git: {(ExcludeGeneratedFilesFromGIT ? "Yes" : "No")}");
                Console.WriteLine();
                Console.WriteLine("[1..] Change group file flag");
                Console.WriteLine("[2..] Change exclude generated files flag");
                Console.WriteLine("[3..] Delete generation files...");
                Console.WriteLine("[4..] Start code generation...");
                Console.WriteLine("[x|X] Exit");
                Console.WriteLine();
                Console.Write("Choose: ");
                input = Console.ReadLine()!.ToLower();
                Console.WriteLine();
                
                ExecuteCommand(input);
            }
        }
        
        /// <summary>
        /// Executes a command based on the provided input.
        /// </summary>
        /// <param name="command">The command to execute. Should be either 1, 2, 3, or 4.</param>
        /// <returns>Void.</returns>
        private static void ExecuteCommand(string command)
        {
            if (command == "1")
            {
                ToGroupFile = !ToGroupFile;
            }
            if (command == "2")
            {
                ExcludeGeneratedFilesFromGIT = !ExcludeGeneratedFilesFromGIT;
                if (ExcludeGeneratedFilesFromGIT)
                {
                    GitIgnoreManager.Run(SourcePath);
                }
                else
                {
                    GitIgnoreManager.DeleteIgnoreEntries(SourcePath);
                }
            }
            else if (command == "3")
            {
                Console.WriteLine("Delete all generated files...");
                Generator.DeleteGeneratedFiles(SourcePath);
                GitIgnoreManager.Run(SourcePath);
            }
            else if (command == "4")
            {
                var logicAssemblyTypes = Logic.Modules.CodeGenerator.AssemblyAccess.AllTypes;
                var solutionProperties = SolutionProperties.Create(SourcePath, logicAssemblyTypes);
                var generatedItems = Generator.Generate(solutionProperties);

                Console.WriteLine("Delete all generated files...");
                Generator.DeleteGeneratedFiles(SourcePath);
                Writer.WriteToGroupFile = ToGroupFile;
                Writer.WriteAll(SourcePath, solutionProperties, generatedItems);
                if (ExcludeGeneratedFilesFromGIT)
                {
                    GitIgnoreManager.Run(SourcePath);
                }
                else
                {
                    GitIgnoreManager.DeleteIgnoreEntries(SourcePath);
                }
                Thread.Sleep(700);
            }
        }
        #endregion Console methods

        #region Helpers
        /// <summary>
        /// Returns the current solution path.
        /// </summary>
        /// <returns>The path of the solution file without the extension, or an empty string if the file does not exist.</returns>
        private static string GetCurrentSolutionPath()
        {
            var codeGenApp = $"{nameof(QuickTemplate)}.{nameof(CodeGenApp)}";
            var endPos = AppContext.BaseDirectory.IndexOf(codeGenApp, StringComparison.CurrentCultureIgnoreCase);
            
            return AppContext.BaseDirectory[..endPos];
        }
        /// <summary>
        /// Retrieves the name of the solution file without the extension from the given solution path.
        /// </summary>
        /// <param name="solutionPath">The path to the solution file.</param>
        /// <returns>The name of the solution file without the extension, or an empty string if the file does not exist.</returns>
        private static string GetSolutionName(string solutionPath)
        {
            var fileInfo = new DirectoryInfo(solutionPath).GetFiles().SingleOrDefault(f => f.Extension.Equals(".sln", StringComparison.CurrentCultureIgnoreCase));
            
            return fileInfo != null ? Path.GetFileNameWithoutExtension(fileInfo.Name) : string.Empty;
        }
        /// <summary>
        /// Retrieves the name of the solution based on the provided solution path.
        /// </summary>
        /// <param name="solutionPath">The path of the solution.</param>
        /// <returns>The name of the solution.</returns>
        private static string GetSolutionNameByPath(string solutionPath)
        {
            return solutionPath.Split(new char[] { '\\', '/' })
                               .Where(e => string.IsNullOrEmpty(e) == false)
                               .Last();
        }
        #endregion Helpers
        
        #region Partial methods
        /// <summary>
        /// This method is called before retrieving the target paths. It can be used to perform any necessary operations or checks.
        /// </summary>
        /// <param name="sourcePath">The source path from where the target paths will be retrieved.</param>
        /// <param name="targetPaths">The list of target paths to be retrieved.</param>
        /// <param name="handled">A reference boolean value indicating whether the operation has been handled.</param>
        static partial void BeforeGetTargetPaths(string sourcePath, List<string> targetPaths, ref bool handled);
        /// <summary>
        /// Method called after getting the target paths.
        /// </summary>
        /// <param name="sourcePath">The path of the source.</param>
        /// <param name="targetPaths">The list of target paths.</param>
        static partial void AfterGetTargetPaths(string sourcePath, List<string> targetPaths);
        #endregion Partial methods
    }
}
//MdEnd
