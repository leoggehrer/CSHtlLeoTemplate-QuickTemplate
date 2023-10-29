//@BaseCode
//MdStart
namespace TemplateTools.ConApp
{
    using System.Diagnostics;
    using CommonStaticLiterals = CommonBase.StaticLiterals;
    /// <summary>
    /// Represents the main program class.
    /// </summary>
    // ... Rest of the code ...
    partial class Program
    {
        #region Class-Constructors
        /// <summary>
        /// Initializes the <see cref="Program"/> class.
        /// </summary>
        /// <remarks>
        /// This static constructor sets up the necessary properties for the program.
        /// </remarks>
        static Program()
        {
            ClassConstructing();
            HomePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
            Environment.OSVersion.Platform == PlatformID.MacOSX)
            ? Environment.GetEnvironmentVariable("HOME")
            : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

            UserPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            SourcePath = Path.Combine(UserPath, "source");
            if (Directory.Exists(SourcePath) == false)
            {
                SourcePath = UserPath;
            }
            SolutionPath = GetCurrentSolutionPath();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called during the construction of the class.
        /// </summary>
        static partial void ClassConstructing();
        /// <summary>
        /// Represents a method that is called when a class is constructed.
        /// </summary>
        static partial void ClassConstructed();
        #endregion Class-Constructors

        #region Properties
        /// <summary>
        /// Gets or sets the home path.
        /// </summary>
        /// <value>The home path.</value>
        internal static string? HomePath { get; set; }
        /// <summary>
        /// Gets or sets the user path.
        /// </summary>
        /// <value>
        /// The user path.
        /// </value>
        internal static string UserPath { get; set; }
        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        /// <value>
        /// The user path.
        /// </value>
        internal static string SourcePath { get; set; }
        /// <summary>
        /// Gets or sets the solution path.
        /// </summary>
        /// <value>
        /// The source path.
        /// </value>
        internal static string SolutionPath { get; set; }

        /// <summary>
        /// Indicates whether printing is allowed when the application is busy.
        /// </summary>
        /// <value>
        /// true if printing is allowed when the application is busy; otherwise, false.
        /// </value>
        internal static bool CanBusyPrint { get; set; } = true;
        /// <summary>
        /// Gets or sets a value indicating whether the RunBusyProgress is active or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the RunBusyProgress is active; otherwise, <c>false</c>.
        /// </value>
        internal static bool RunBusyProgress { get; set; }
        /// <summary>
        /// Gets or sets the foreground color of the console.
        /// </summary>
        /// <value>
        /// The foreground color of the console.
        /// </value>
        internal static ConsoleColor ForegroundColor { get; set; } = Console.ForegroundColor;
        #endregion Properties

        /// <summary>
        /// Entry point for the application.
        /// </summary>
        /// <remarks>
        /// This method calls the 'RunApp' method to start the application.
        /// </remarks>
        static void Main(/*string[] args*/)
        {
            RunApp();
        }

        /// <summary>
        ///     Runs the Template Tools application.
        /// </summary>
        /// <remarks>
        ///     This method displays a menu for the user to choose a tool to run.
        ///     The user can choose to change the source path, copy the solution to a domain solution,
        ///     set defines for project options, generate code for the solution, compare a project with the template,
        ///     generate documentation for the solution, or exit the application.
        /// </remarks>
        private static void RunApp()
        {
            var input = string.Empty;
            var saveForeColor = Console.ForegroundColor;

            while (input.Equals("x") == false)
            {
                int mnuIdx = 1;

                Console.Clear();
                Console.ForegroundColor = ForegroundColor;
                Console.WriteLine("Template Tools");
                Console.WriteLine("==============");
                Console.WriteLine();
                Console.WriteLine($"Source Path: {SolutionPath}");
                Console.WriteLine();
                Console.WriteLine("Choose a tool:");
                Console.WriteLine();
                Console.WriteLine($"[{mnuIdx++}] Path............Change source path");
                Console.WriteLine($"[{mnuIdx++}] Copier..........Copy this solution to a domain solution");
                Console.WriteLine($"[{mnuIdx++}] Preprocessor....Setting defines for project options");
                Console.WriteLine($"[{mnuIdx++}] CodeGenerator...Generate code for this solution");
                Console.WriteLine($"[{mnuIdx++}] Comparison......Compares a project with the template");
                Console.WriteLine($"[{mnuIdx++}] Documentation...Generate documentation for this solution");
                Console.WriteLine($"[{mnuIdx++}] Cleanup.........Deletes the temporary directories");
                Console.WriteLine("[x|X] Exit");
                Console.WriteLine();
                Console.Write("Choose: ");

                input = Console.ReadLine()?.ToLower() ?? String.Empty;
                Console.ForegroundColor = saveForeColor;
                if (Int32.TryParse(input, out var select))
                {
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
                    else if (select == 2)
                    {
                        CopierApp.RunApp();
                    }
                    else if (select == 3)
                    {
                        PreprocessorApp.RunApp();
                    }
                    else if (select == 4)
                    {
                        CodeGeneratorApp.RunApp();
                    }
                    else if (select == 5)
                    {
                        ComparisonApp.RunApp();
                    }
                    else if (select == 6)
                    {
                        DocuGeneratorApp.RunApp();
                    }
                    else if (select == 7)
                    {
                        CleanupApp.RunApp(Program.SolutionPath);
                    }
                }
                Program.RunBusyProgress = false;
            }
        }

        /// <summary>
        /// Prints a busy progress indicator in the console.
        /// </summary>
        /// <param name="title">The title that is displayed before the progress starts.</param>
        internal static void PrintBusyProgress(string title)
        {
            Console.WriteLine(title);
            PrintBusyProgress();
        }

        /// <summary>
        /// Prints a busy progress indicator in the console.
        /// </summary>
        internal static void PrintBusyProgress()
        {
            static void Write(int cursorLeft, int cursorTop, string output)
            {
                var saveCursorTop = Console.CursorTop;
                var saveCursorLeft = Console.CursorLeft;
                var saveForeColor = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(cursorLeft, cursorTop);
                Console.Write(output);
                Console.SetCursorPosition(saveCursorLeft, saveCursorTop);
                Console.ForegroundColor = saveForeColor;
            }
            if (RunBusyProgress == false)
            {
                var head = '\\';
                var runSign = '#';
                var counter = 0;

                RunBusyProgress = true;
                Console.WriteLine();

                var (Left, Top) = Console.GetCursorPosition();

                Console.WriteLine();
                Console.WriteLine();
                Task.Factory.StartNew(async () =>
                {
                    while (RunBusyProgress)
                    {
                        if (CanBusyPrint)
                        {
                            if (Left > 60)
                            {
                                var timeInSec = counter / 5;

                                runSign = runSign == '#' ? '+' : '#';
                                Write(Left, Top," ");
                                Left = 0;
                            }
                            else
                            {
                                Write(Left++, Top, $"{runSign}{head}");
                            }

                            if (counter % 5 == 0)
                            {
                                Write(65, Top, $" {counter / 5, 5} [sec]");
                            }
                            head = head == '\\' ? '/' : '\\';
                            counter++;
                        }
                        await Task.Delay(200);
                    }
                });
            }
        }
        /// <summary>
        /// Returns the parent directory of the specified path.
        /// </summary>
        /// <param name="path">The path for which to retrieve the parent directory.</param>
        /// <returns>
        /// The full path of the parent directory of the specified path if it exists; otherwise, the original path is returned.
        /// </returns>
        /// <remarks>
        /// This method uses the <see cref="Directory.GetParent(string)"/> method to retrieve the parent directory of the specified path.
        /// It returns the full path of the parent directory if it exists; otherwise, it returns the original path.
        /// </remarks>
        internal static string GetParentDirectory(string path)
        {
            var result = Directory.GetParent(path);

            return result != null ? result.FullName : path;
        }
        /// <summary>
        /// Retrieves the current solution path.
        /// </summary>
        /// <returns>
        /// The current solution path as a string.
        /// </returns>
        internal static string GetCurrentSolutionPath()
        {
            int endPos = AppContext.BaseDirectory
                                   .IndexOf($"{nameof(TemplateTools)}", StringComparison.CurrentCultureIgnoreCase);
            var result = AppContext.BaseDirectory[..endPos];

            while (result.EndsWith(Path.DirectorySeparatorChar))
            {
                result = result[0..^1];
            }
            return result;
        }
        /// <summary>
        /// Retrieves the name of the solution file from the given solution path.
        /// </summary>
        /// <param name="solutionPath">The full path of the solution file.</param>
        /// <returns>The name of the solution file as a string.</returns>
        internal static string GetSolutionNameByPath(string solutionPath)
        {
            return solutionPath.Split(Path.DirectorySeparatorChar)
                               .Where(e => string.IsNullOrEmpty(e) == false)
                               .Last();
        }
        /// <summary>
        /// Retrieves the solution name from a given file path.
        /// </summary>
        /// <param name="path">The file path from which to retrieve the solution name.</param>
        /// <returns>The solution name extracted from the file path.</returns>
        internal static string GetSolutionNameFromPath(string path)
        {
            var result = string.Empty;
            var data = path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

            if (data.Any())
            {
                result = data.Last();
            }
            return result;
        }

        /// <summary>
        /// Retrieves the path from the given path by checking for the presence of a file with the specified extension.
        /// </summary>
        /// <param name="path">The original path.</param>
        /// <param name="checkFileExtension">The file extension to check for.</param>
        /// <returns>
        /// The path up to the directory where the first file with the specified extension is found,
        /// or an empty string if no such file is found.
        /// </returns>
        internal static string GetPathFromPath(string path, string checkFileExtension)
        {
            var result = string.Empty;
            var checkPath = string.Empty;
            var data = path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < data.Length && result == string.Empty; i++)
            {
                checkPath = checkPath == string.Empty ? data[i] : Path.Combine(checkPath, data[i]);

                var projectFilePath = Path.Combine(checkPath, $"{data[i]}{checkFileExtension}");

                if (File.Exists(projectFilePath))
                {
                    result = checkPath;
                }
            }
            return result;
        }
        /// <summary>
        /// Retrieves the directory name from a given path by checking for a specific file extension.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <param name="checkFileExtension">The file extension to check for.</param>
        /// <returns>
        /// The directory name that contains the file with the specified extension, or an empty string if no such file is found.
        /// </returns>
        internal static string GetDirectoryNameFromPath(string path, string checkFileExtension)
        {
            var result = string.Empty;
            var checkPath = string.Empty;
            var data = path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < data.Length && result == string.Empty; i++)
            {
                checkPath = checkPath == string.Empty ? data[i] : Path.Combine(checkPath, data[i]);

                var projectFilePath = Path.Combine(checkPath, $"{data[i]}{checkFileExtension}");

                if (File.Exists(projectFilePath))
                {
                    result = data[i];
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the project name from the given file path and solution name.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="solutionName">The solution name.</param>
        /// <returns>The project name extracted from the file path.</returns>
        internal static string GetProjectNameFromFilePath(string filePath, string solutionName)
        {
            var result = string.Empty;
            var data = filePath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < data.Length && result == string.Empty; i++)
            {
                for (int j = 0; j < CommonStaticLiterals.TemplateProjects.Length; j++)
                {
                    if (data[i].Equals(CommonStaticLiterals.TemplateProjects[j]))
                    {
                        result = data[i];
                    }
                }
                for (int j = 0; j < CommonStaticLiterals.TemplateToolProjects.Length; j++)
                {
                    if (data[i].Equals(CommonStaticLiterals.TemplateToolProjects[j]))
                    {
                        result = data[i];
                    }
                }
                if (string.IsNullOrEmpty(result))
                {
                    for (int j = 0; j < CommonStaticLiterals.TemplateProjectExtensions.Length; j++)
                    {
                        if (data[i].Equals($"{solutionName}{CommonStaticLiterals.TemplateProjectExtensions[j]}"))
                        {
                            result = data[i];
                        }
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Retrieves the directory structure of a specified path.
        /// </summary>
        /// <param name="path">The path to the root directory.</param>
        /// <param name="filter">The optional filter function to determine which directories to include.</param>
        /// <param name="excludeFolders">The optional list of folder names to exclude from the directory structure.</param>
        /// <returns>An array of strings representing the full paths of the directories in the directory structure.</returns>
        internal static string[] QueryDirectoryStructure(string path, Func<string, bool>? filter, params string[] excludeFolders)
        {
            static void GetDirectoriesWithoutHidden(Func<string, bool>? filter, DirectoryInfo directoryInfo, List<string> list, int maxDeep, int deep, params string[] excludeFolders)
            {
                try
                {
                    if (directoryInfo.Attributes.HasFlag(FileAttributes.Hidden) == false)
                    {
                        if ((filter == null || filter(directoryInfo.Name)))
                        {
                            list.Add(directoryInfo.FullName);
                        }
                        if (deep < maxDeep)
                        {
                            foreach (var di in directoryInfo.GetDirectories())
                            {
                                if (excludeFolders.Any(e => e.Equals(di.Name, StringComparison.CurrentCultureIgnoreCase)) == false)
                                {
                                    GetDirectoriesWithoutHidden(filter, di, list, maxDeep, deep + 1, excludeFolders);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error: {ex.Message}");
                }
            }
            var result = new List<string>();
            var directoryInfo = new DirectoryInfo(path);

            GetDirectoriesWithoutHidden(filter, directoryInfo, result, 8, 0, excludeFolders);
            return result.ToArray();
        }
        /// <summary>
        /// Retrieves an array of string values representing the paths to QuickTemplate solutions within a specified directory.
        /// </summary>
        /// <param name="startPath">The starting directory path in which to search for QuickTemplate solutions.</param>
        /// <returns>
        /// An array of string value representing the paths to QuickTemplate solutions found in the specified directory.
        /// </returns>
        internal static string[] GetQuickTemplateSolutions(string startPath)
        {
            var result = new List<string>();
            var qtPaths = GetQuickTemplateProjects(startPath);

            foreach (var qtPath in qtPaths)
            {
                var di = new DirectoryInfo(qtPath);

                if (di.GetFiles().Any(f => Path.GetExtension(f.Name).Equals(".sln", StringComparison.CurrentCultureIgnoreCase)))
                {
                    result.Add(qtPath);
                }
            }
            return result.ToArray();
        }
        /// <summary>
        /// Retrieves an array of quick template projects from the specified starting path.
        /// </summary>
        /// <param name="startPath">The starting path to search for quick template projects.</param>
        /// <returns>An array of quick template projects.</returns>
        /// <remarks>
        /// Quick template projects are identified by their name starting with "QT" and will exclude
        /// directories with names "bin", "obj", and "node_modules" from the search results.
        /// </remarks>
        internal static string[] GetQuickTemplateProjects(string startPath)
        {
            return QueryDirectoryStructure(startPath, n => n.StartsWith("QT"), "bin", "obj", "node_modules");
        }
        /// <summary>
        /// Retrieves the paths of quick templates based on a start path and additional included paths.
        /// </summary>
        /// <param name="startPath">The starting path to search for quick template projects.</param>
        /// <param name="includePaths">Additional paths to include in the search for quick template projects.</param>
        /// <returns>An array of string paths representing the quick template paths.</returns>
        internal static string[] GetQuickTemplatePaths(string startPath, params string[] includePaths)
        {
            var qtProjects = GetQuickTemplateProjects(startPath).Union(includePaths).ToArray();
            var qtPaths = qtProjects.Select(p => Program.GetParentDirectory(p))
                                    .Distinct()
                                    .OrderBy(p => p);

            return qtPaths.ToArray();
        }
        /// <summary>
        /// Retrieves an array of parent paths for quick template projects starting from a specified path, as well as any additional paths to include.
        /// </summary>
        /// <param name="startPath">The starting path to search for quick template projects.</param>
        /// <param name="includePaths">The additional paths to include in the search.</param>
        /// <returns>An array of parent paths for quick template projects.</returns>
        /// <remarks>
        /// The method retrieves quick template projects using the GetQuickTemplateProjects method with the specified start path and includes the additional paths provided in the includePaths parameter.
        /// The method then determines the parent directory for each quick template project path and ensures that there are no duplicate or nested paths in the result.
        /// The resulting parent paths are ordered alphabetically and returned as an array.
        /// </remarks>
        internal static string[] GetQuickTemplateParentPaths(string startPath, params string[] includePaths)
        {
            var result = new List<string>();
            var qtProjects = GetQuickTemplateProjects(startPath).Union(includePaths).ToArray();
            var qtPaths = qtProjects.Select(p => Program.GetParentDirectory(p))
                                    .Distinct()
                                    .OrderBy(p => p);

            foreach (var qtPath in qtPaths)
            {
                if (result.Any(x => qtPath.Length > x.Length && qtPath.Contains(x)) == false)
                {
                    result.Add(qtPath);
                }
            }
            return result.ToArray();
        }

        #region CLI Argument methods
        /// <summary>
        /// Opens the solution folder in Windows Explorer.
        /// </summary>
        /// <param name="solutionPath">The path of the solution folder.</param>
        internal static void OpenSolutionFolder(string solutionPath)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                Process.Start(new ProcessStartInfo()
                {
                    WorkingDirectory = solutionPath,
                    FileName = "explorer",
                    Arguments = solutionPath,
                    CreateNoWindow = true,
                });
            }
        }
        #endregion CLI Argument methods
    }
}
//MdEnd

