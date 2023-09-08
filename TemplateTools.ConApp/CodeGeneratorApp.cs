//@BaseCode
//MdStart
using System.Diagnostics;
using TemplateCodeGenerator.Logic;

namespace TemplateTools.ConApp
{
    internal partial class CodeGeneratorApp
    {
        #region Class-Constructors
        static CodeGeneratorApp()
        {
            ClassConstructing();
            ToGroupFile = false;
            IgnoreFiles = true;
            SolutionPath = Program.SourcePath;
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        #endregion Class-Constructors

        #region Properties
        private static bool ToGroupFile { get; set; }
        private static bool IgnoreFiles { get; set; }
        private static string SolutionPath { get; set; }
        #endregion Properties

        #region Console methods
        public static void RunApp()
        {
            var toGroupFile = ToGroupFile;
            var ignoreFiles = IgnoreFiles;
            var input = string.Empty;
            var saveForeColor = Console.ForegroundColor;

            while (input.Equals("x") == false)
            {
                var menuIndex = 0;
                var sourceSolutionName = Program.GetSolutionNameByPath(SolutionPath);

                Console.Clear();
                Console.ForegroundColor = Program.ForegroundColor;
                Console.WriteLine("Template Code Generator");
                Console.WriteLine("=======================");
                Console.WriteLine();
                Console.WriteLine($"Code generation for: {sourceSolutionName}");
                Console.WriteLine($"From file path:  {SolutionPath}");
                Console.WriteLine($"Generation into: {(toGroupFile ? "Group files" : "Single files")}");
                Console.WriteLine($"Ignore files:    {(ignoreFiles ? "Yes" : "No")}");
                Console.WriteLine();
                Console.WriteLine($"[{++menuIndex}] Change source path");           // 1
                Console.WriteLine($"[{++menuIndex}] Compile logic project...");     // 2
                Console.WriteLine($"[{++menuIndex}] Change group file flag");       // 3
                Console.WriteLine($"[{++menuIndex}] Change ignore files flag");     // 4
                Console.WriteLine($"[{++menuIndex}] Delete generated files...");   // 5
                Console.WriteLine($"[{++menuIndex}] Start code generation...");     // 6
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
                        var qtSolutions = Program.GetQuickTemplateSolutions(Program.UserPath).Union(new[] { solutionPath }).ToArray();

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
                        ignoreFiles = !ignoreFiles;
                    }
                    if (select == 5)
                    {
                        Generator.DeleteGeneratedFiles(SolutionPath);
                    }
                    if (select == 6)
                    {
                        string command = "4";

                        if (toGroupFile)
                        {
                            command = "1 " + command;
                        }

                        if (ignoreFiles == false)
                        {
                            command = "2 " + command;
                        }
                        ExecuteRunProject(solutionProperties, command);
                    }
                    Thread.Sleep(700);
                }
            }
        }
        #endregion Console methods
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
