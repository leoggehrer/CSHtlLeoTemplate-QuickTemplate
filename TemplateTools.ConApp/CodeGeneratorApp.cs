//@BaseCode
//MdStart
using System.Diagnostics;
using TemplateCodeGenerator.Logic;

namespace TemplateTools.ConApp
{
    /// <summary>
    /// Represents the CodeGeneratorApp class.
    /// </summary>
    internal partial class CodeGeneratorApp
    {
        #region Class-Constructors
        /// <summary>
        /// Initializes the CodeGeneratorApp class.
        /// </summary>
        static CodeGeneratorApp()
        {
            ClassConstructing();
            ToGroupFile = false;
            ExcludeGeneratedFilesFromGIT = true;
            SolutionPath = Program.SolutionPath;
            ClassConstructed();
        }
        /// <summary>
        /// This method is called when the class is being constructed.
        /// </summary>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when the class is constructed.
        /// </summary>
        /// <remarks>
        /// This method is called internally and is intended for internal use only.
        /// </remarks>
        static partial void ClassConstructed();
        #endregion Class-Constructors
        
        #region Properties
        /// <summary>
        /// Gets or sets the value indicating whether the file should be grouped.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the file should be grouped; otherwise, <c>false</c>.
        /// </value>
        private static bool ToGroupFile { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether generated files should be excluded from GIT.
        /// </summary>
        /// <remarks>
        /// By default, generated files are included in the GIT repository.
        /// However, setting this property to <c>true</c> will exclude generated files from being tracked by  GIT.
        /// </remarks>
        /// <value>
        /// <c>true</c> to exclude generated files from GIT; otherwise, <c>false</c> to include generated files in GIT.
        /// </value>
        private static bool ExcludeGeneratedFilesFromGIT { get; set; }
        /// <summary>
        /// Gets or sets the path of the solution.
        /// </summary>
        private static string SolutionPath { get; set; }
        #endregion Properties
        
        #region Console methods
        /// <summary>
        /// Runs the application and prompts the user to select various options related to code generation.
        /// </summary>
        public static void RunApp()
        {
            var toGroupFile = ToGroupFile;
            var excludeGeneratedFilesFromGIT = ExcludeGeneratedFilesFromGIT;
            var input = string.Empty;
            var saveForeColor = Console.ForegroundColor;
            
            while (input.Equals("x") == false)
            {
                var menuIndex = 0;
                var sourceSolutionName = Program.GetSolutionNameFromPath(SolutionPath);
                
                Console.Clear();
                Console.ForegroundColor = Program.ForegroundColor;
                Console.WriteLine("Template Code Generator");
                Console.WriteLine("=======================");
                Console.WriteLine();
                Console.WriteLine($"Code generation path: {SolutionPath}");
                Console.WriteLine($"Code generation for:  {sourceSolutionName}");
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine($"Write generated code into:        {(toGroupFile ? "Group files" : "Single files")}");
                Console.WriteLine($"Exclude generated files from git: {(excludeGeneratedFilesFromGIT ? "Yes" : "No")}");
                Console.WriteLine();
                Console.WriteLine($"[{++menuIndex}] Change source path");                   // 1
                Console.WriteLine($"[{++menuIndex}] Compile logic project...");             // 2
                Console.WriteLine($"[{++menuIndex}] Change group file flag");               // 3
                Console.WriteLine($"[{++menuIndex}] Change exclude generated files flag");  // 4
                Console.WriteLine($"[{++menuIndex}] Delete generated files...");            // 5
                Console.WriteLine($"[{++menuIndex}] Start code generation...");             // 6
                Console.WriteLine("[x|X] Exit");
                Console.WriteLine();
                Console.Write("Choose: ");
                
                input = Console.ReadLine()?.ToLower() ?? String.Empty;
                Console.ForegroundColor = saveForeColor;
                if (Int32.TryParse(input, out var select))
                {
                    var solutionProperties = SolutionProperties.Create(SolutionPath);
                    
                    if (select == 1)
                    {
                        var solutionPath = Program.GetCurrentSolutionPath();
                        var qtSolutions = Program.GetQuickTemplateSolutions(Program.SourcePath).Union(new[] { solutionPath }).ToArray();
                        
                        for (int i = 0; i < qtSolutions.Length; i++)
                        {
                            if (i == 0)
                            {
                                Console.WriteLine();
                            }
                            
                            Console.WriteLine($"Change path to: [{i + 1,2}] {qtSolutions[i]}");
                        }
                        Console.WriteLine();
                        Console.Write("Select or enter source path: ");
                        var selectOrPath = Console.ReadLine();
                        
                        if (Int32.TryParse(selectOrPath, out int number))
                        {
                            if ((number - 1) >= 0 && (number - 1) < qtSolutions.Length)
                            {
                                SolutionPath = qtSolutions[number - 1];
                            }
                        }
                        else if (Directory.Exists(selectOrPath))
                        {
                            SolutionPath = selectOrPath;
                        }
                    }
                    if (select == 2 || select == 6)
                    {
                        ExecuteBuildProject(solutionProperties);
                        if (select == 2)
                        {
                            Program.RunBusyProgress = false;
                            Console.Write("Press any key ");
                            Console.ReadKey();
                        }
                    }
                    if (select == 3)
                    {
                        toGroupFile = !toGroupFile;
                    }
                    if (select == 4)
                    {
                        excludeGeneratedFilesFromGIT = !excludeGeneratedFilesFromGIT;
                    }
                    if (select == 5)
                    {
                        Program.PrintBusyProgress();
                        Console.WriteLine("Delete all generated files...");
                        Generator.DeleteGeneratedFiles(SolutionPath);
                    }
                    if (select == 6)
                    {
                        string command = "4";

                        if (toGroupFile)
                        {
                            command = "1 " + command;
                        }
                        
                        if (excludeGeneratedFilesFromGIT == false)
                        {
                            command = "2 " + command;
                        }
                        ExecuteRunProject(solutionProperties, command);
                    }
                    Thread.Sleep(700);
                    Program.RunBusyProgress = false;
                }
            }
        }
        #endregion Console methods
        /// <summary>
        /// Executes the build process for the specified solution.
        /// </summary>
        /// <param name="solutionProperties">The SolutionProperties object containing the necessary information for the build process.</param>
        /// <returns>The path where the solution was compiled.</returns>
        private static string ExecuteBuildProject(SolutionProperties solutionProperties)
        {
            var counter = 0;
            var maxWaiting = 10 * 60 * 1000;    // 10 minutes
            var startCompilePath = Path.Combine(Path.GetTempPath(), solutionProperties.SolutionName);
            var compilePath = startCompilePath;
            bool deleteError;
            
            do
            {
                deleteError = false;
                if (Directory.Exists(compilePath))
                {
                    try
                    {
                        Directory.Delete(compilePath, true);
                    }
                    catch
                    {
                        deleteError = true;
                        compilePath = $"{startCompilePath}{++counter}";
                    }
                }
            } while (deleteError != false);
            
            var arguments = $"build \"{solutionProperties.LogicCSProjectFilePath}\" -c Release -o \"{compilePath}\"";
            
            Console.WriteLine($"dotnet {arguments}");
            
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                var csprojStartInfo = new ProcessStartInfo("dotnet")
                {
                    Arguments = arguments,
                    UseShellExecute = false
                };
                Process.Start(csprojStartInfo)?.WaitForExit(maxWaiting);
                solutionProperties.CompilePath = compilePath;
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                var csprojStartInfo = new ProcessStartInfo("dotnet")
                {
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                };
                Process.Start(csprojStartInfo)?.WaitForExit(maxWaiting);
                solutionProperties.CompilePath = compilePath;
            }
            return compilePath;
        }
        /// <summary>
        /// Executes the run project for the given solution properties and execute arguments.
        /// </summary>
        /// <param name="solutionProperties">The solution properties.</param>
        /// <param name="executeArgs">The execute arguments.</param>
        /// <returns>The project path.</returns>
        private static string ExecuteRunProject(SolutionProperties solutionProperties, string executeArgs)
        {
            var maxWaiting = 10 * 60 * 1000;    // 10 minutes
            var projectPath = $"{solutionProperties.SolutionName}.CodeGenApp{Path.DirectorySeparatorChar}{solutionProperties.SolutionName}.CodeGenApp.csproj";
            var arguments = $"run --project \"{solutionProperties.SolutionPath}{Path.DirectorySeparatorChar}{projectPath}\" {executeArgs}";
            
            Console.WriteLine($"dotnet {arguments}");
            
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                var csprojStartInfo = new ProcessStartInfo("dotnet")
                {
                    Arguments = arguments,
                    UseShellExecute = false
                };
                Process.Start(csprojStartInfo)?.WaitForExit(maxWaiting);
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                var csprojStartInfo = new ProcessStartInfo("dotnet")
                {
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                };
                Process.Start(csprojStartInfo)?.WaitForExit(maxWaiting);
            }
            return projectPath;
        }
    }
}
//MdEnd

