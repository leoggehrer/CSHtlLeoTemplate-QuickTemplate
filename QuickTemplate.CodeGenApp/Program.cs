//@BaseCode
//MdStart
namespace QuickTemplate.CodeGenApp
{
    using TemplateCodeGenerator.Logic;
    using TemplateCodeGenerator.Logic.Generation;
    internal partial class Program
    {
        static Program()
        {
            ClassConstructing();
            ToGroupFile = false;
            HomePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
                        Environment.OSVersion.Platform == PlatformID.MacOSX)
                       ? Environment.GetEnvironmentVariable("HOME")
                       : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

            UserPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            SourcePath = GetCurrentSolutionPath();
            TargetPaths = Array.Empty<string>();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        #region Properties
        private static bool ToGroupFile { get; set; } = false;
        private static string? HomePath { get; set; }
        private static string UserPath { get; set; }
        private static string SourcePath { get; set; }
        private static string[] TargetPaths { get; set; }
        private static string[] SearchPatterns => StaticLiterals.SourceFileExtensions.Split('|');
        #endregion Properties

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
        private static void RunApp()
        {
            var input = string.Empty;

            while (input != "x")
            {
                Console.Clear();
                Console.WriteLine($"Code generation for: {nameof(QuickTemplate)}");
                Console.WriteLine("=============================================");
                Console.WriteLine();
                Console.WriteLine($"From file path:  {SourcePath}");
                Console.WriteLine($"Generation into: {(ToGroupFile ? "Group files" : "Single files")}");
                Console.WriteLine();
                Console.WriteLine("[1..] Change group file flag");
                Console.WriteLine("[2..] Delete generation files...");
                Console.WriteLine("[3..] Start code generation...");
                Console.WriteLine("[x|X] Exit");
                Console.WriteLine();
                Console.Write("Choose: ");
                input = Console.ReadLine()!.ToLower();
                Console.WriteLine();

                ExecuteCommand(input);
            }
        }

        private static void ExecuteCommand(string command)
        {
            if (command == "1")
            {
                ToGroupFile = !ToGroupFile;
            }
            else if (command == "2")
            {
                Generator.DeleteGeneratedFiles(SourcePath);
            }
            else if (command == "3")
            {
                var logicAssemblyTypes = Logic.Modules.CodeGenerator.AssemblyAccess.AllTypes;
                var solutionProperties = SolutionProperties.Create(SourcePath, logicAssemblyTypes);
                var generatedItems = Generator.Generate(solutionProperties);

                Generator.DeleteGeneratedFiles(SourcePath);
                Writer.WriteToGroupFile = ToGroupFile;
                Writer.WriteAll(SourcePath, solutionProperties, generatedItems);
                Thread.Sleep(700);
            }
        }
        #endregion Console methods

        #region Helpers
        private static string GetCurrentSolutionPath()
        {
            int endPos = AppContext.BaseDirectory
                                   .IndexOf("QuickTemplate.CodeGenApp", StringComparison.CurrentCultureIgnoreCase);

            return AppContext.BaseDirectory[..endPos];
        }
        private static string GetSolutionName(string solutionPath)
        {
            var fileInfo = new DirectoryInfo(solutionPath).GetFiles().SingleOrDefault(f => f.Extension.Equals(".sln", StringComparison.CurrentCultureIgnoreCase));

            return fileInfo != null ? Path.GetFileNameWithoutExtension(fileInfo.Name) : string.Empty;
        }
        private static string GetSolutionNameByPath(string solutionPath)
        {
            return solutionPath.Split(new char[] { '\\', '/' })
                               .Where(e => string.IsNullOrEmpty(e) == false)
                               .Last();
        }
        #endregion Helpers

        #region Partial methods
        static partial void BeforeGetTargetPaths(string sourcePath, List<string> targetPaths, ref bool handled);
        static partial void AfterGetTargetPaths(string sourcePath, List<string> targetPaths);
        #endregion Partial methods
    }
}
//MdEnd