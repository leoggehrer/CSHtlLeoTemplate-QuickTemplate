//@BaseCode
//MdStart
using System.Diagnostics;

namespace TemplateTools.ConApp
{
    partial class Program
    {
        static Program()
        {
            ClassConstructing();
            HomePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
                        Environment.OSVersion.Platform == PlatformID.MacOSX)
                       ? Environment.GetEnvironmentVariable("HOME")
                       : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

            UserPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            SourcePath = GetCurrentSolutionPath();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        #region Properties
        internal static string? HomePath { get; set; }
        internal static string UserPath { get; set; }
        internal static string SourcePath { get; set; }

        internal static bool CanBusyPrint { get; set; } = true;
        internal static bool RunBusyProgress { get; set; }
        internal static ConsoleColor ForegroundColor { get; set; } = Console.ForegroundColor;
        #endregion Properties

        static void Main(string[] args)
        {
            RunApp();
        }

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
                Console.WriteLine($"Source Path: {SourcePath}");
                Console.WriteLine();
                Console.WriteLine("Choose a tool:");
                Console.WriteLine();
                Console.WriteLine($"[{mnuIdx++}] Path............Change source path");
                Console.WriteLine($"[{mnuIdx++}] Copier..........Copy a quick template to a project");
                Console.WriteLine($"[{mnuIdx++}] Preprocessor....Setting defines for project options");
                Console.WriteLine($"[{mnuIdx++}] CodeGenerator...Generate code for template solutions");
                Console.WriteLine($"[{mnuIdx++}] Comparison......compares a project with the template and compares it");
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
                                SourcePath = qtSolutions[number - 1];
                            }
                        }
                        else if (Directory.Exists(selectOrPath))
                        {
                            SourcePath = selectOrPath;
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
                }
            }
        }

        internal static void PrintBusyProgress()
        {
            if (RunBusyProgress == false)
            {
                var sign = "\\";

                Console.WriteLine();
                RunBusyProgress = true;
                Task.Factory.StartNew(async () =>
                {
                    while (RunBusyProgress)
                    {
                        if (CanBusyPrint)
                        {
                            if (Console.CursorLeft > 0)
                                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);

                            Console.Write($".{sign}");
                            sign = sign == "\\" ? "/" : "\\";
                        }
                        await Task.Delay(250).ConfigureAwait(false);
                    }
                });
            }
        }
        internal static string GetParentDirectory(string path)
        {
            var result = Directory.GetParent(path);

            return result != null ? result.FullName : path;
        }
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
        internal static string GetSolutionNameByPath(string solutionPath)
        {
            return solutionPath.Split(Path.DirectorySeparatorChar)
                               .Where(e => string.IsNullOrEmpty(e) == false)
                               .Last();
        }
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
        internal static string GetProjectNameFromFilePath(string filePath, string solutionName)
        {
            var result = string.Empty;
            var data = filePath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < data.Length && result == string.Empty; i++)
            {
                for (int j = 0; j < StaticLiterals.SolutionProjects.Length; j++)
                {
                    if (data[i].Equals(StaticLiterals.SolutionProjects[j]))
                    {
                        result = data[i];
                    }
                }
                for (int j = 0; j < StaticLiterals.SolutionToolProjects.Length; j++)
                {
                    if (data[i].Equals(StaticLiterals.SolutionToolProjects[j]))
                    {
                        result = data[i];
                    }
                }
                if (string.IsNullOrEmpty(result))
                {
                    for (int j = 0; j < StaticLiterals.ProjectExtensions.Length; j++)
                    {
                        if (data[i].Equals($"{solutionName}{StaticLiterals.ProjectExtensions[j]}"))
                        {
                            result = data[i];
                        }
                    }
                }
            }
            return result;
        }
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
                    System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                }
            }
            var result = new List<string>();
            var directoryInfo = new DirectoryInfo(path);

            GetDirectoriesWithoutHidden(filter, directoryInfo, result, 8, 0, excludeFolders);
            return result.ToArray();
        }
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
        internal static string[] GetQuickTemplateProjects(string startPath)
        {
            return QueryDirectoryStructure(startPath, n => n.StartsWith("QT"), "bin", "obj", "node_modules");
        }
        internal static string[] GetQuickTemplatePaths(string startPath, params string[] includePaths)
        {
            var qtProjects = GetQuickTemplateProjects(startPath).Union(includePaths).ToArray();
            var qtPaths = qtProjects.Select(p => Program.GetParentDirectory(p))
                                    .Distinct()
                                    .OrderBy(p => p);

            return qtPaths.ToArray();
        }
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
