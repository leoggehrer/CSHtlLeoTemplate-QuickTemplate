//@BaseCode
//MdStart
using System.Text;

namespace TemplateTools.ConApp
{
    internal partial class ComparisonApp
    {
        #region Class-Constructors
        static ComparisonApp()
        {
            ClassConstructing();
            SourcePath = Program.SourcePath;
            TargetPaths = Array.Empty<string>();
            AddTargetPaths = Array.Empty<string>();
            SourceLabels = new string[] { StaticLiterals.BaseCodeLabel };
            TargetLabels = new string[] { StaticLiterals.CodeCopyLabel };
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        #endregion Class-Constructors

        #region Properties
        private static string SourcePath { get; set; }
        private static string[] TargetPaths { get; set; }
        private static string[] AddTargetPaths { get; set; }
        private static string[] SearchPatterns => StaticLiterals.SourceFileExtensions.Split('|');
        private static string[] SourceLabels { get; set; }
        private static string[] TargetLabels { get; set; }
        #endregion Properties

        #region App methods
        public static void RunApp()
        {
            var running = false;
            var saveForeColor = Console.ForegroundColor;

            do
            {
                var input = string.Empty;
                var handled = false;
                var targetPaths = new List<string>();

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Program.PrintBusyProgress();
                BeforeGetTargetPaths(Program.UserPath, targetPaths, ref handled);
                if (handled == false)
                {
                    TargetPaths = Program.GetQuickTemplateProjects(Program.UserPath);
                    TargetPaths = TargetPaths.Union(AddTargetPaths).ToArray();
                }
                else
                {
                    TargetPaths = TargetPaths.Union(AddTargetPaths).ToArray();
                }

                Program.RunBusyProgress = false;
                Task.Delay(250).Wait();
                PrintHeader(SourcePath, TargetPaths);
                Console.WriteLine($"[x|X...Quit]: ");
                Console.WriteLine();
                Console.Write("Choose [n|n,n|x|X]: ");

                input = Console.ReadLine()?.ToLower();
                Console.ForegroundColor = saveForeColor;
                running = input?.Equals("x") == false;
                if (running)
                {
                    if (input != null && input.Equals("a"))
                    {
                        Program.PrintBusyProgress();
                        BalancingSolutions(SourcePath, SourceLabels, TargetPaths, TargetLabels);
                    }
                    else if (input != null && input.Equals("+"))
                    {
                        Console.WriteLine();
                        Console.Write("Add path: ");
                        input = Console.ReadLine();

                        if (Directory.Exists(input))
                        {
                            AddTargetPaths = AddTargetPaths.Union(new string[] { input }).ToArray();
                        }
                    }
                    else
                    {
                        var numbers = input?.Trim()
                                            .Split(',').Where(s => Int32.TryParse(s, out int n))
                                            .Select(s => Int32.Parse(s))
                                            .ToArray();

                        Program.PrintBusyProgress();
                        foreach (var number in numbers ?? Array.Empty<int>())
                        {
                            if (number == TargetPaths.Length + 1)
                            {
                                BalancingSolutions(SourcePath, SourceLabels, TargetPaths, TargetLabels);
                            }
                            else if (number > 0 && number <= TargetPaths.Length)
                            {
                                BalancingSolutions(SourcePath, SourceLabels, new string[] { TargetPaths[number - 1] }, TargetLabels);
                            }
                        }
                    }
                }
            } while (running);
        }
        private static void PrintHeader(string sourcePath, string[] targetPaths)
        {
            var index = 0;

            Console.Clear();
            Console.ForegroundColor = Program.ForegroundColor;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Template Comparison");
            Console.WriteLine("===================");
            Console.WriteLine();
            Console.WriteLine("Balance label(s):");
            for (int i = 0; i < SourceLabels.Length && i < TargetLabels.Length; i++)
            {
                Console.WriteLine($"  {SourceLabels[i],-15} => {TargetLabels[i]}");
            }
            Console.WriteLine();
            Console.WriteLine($"Source: {sourcePath}");
            Console.WriteLine();
            Console.WriteLine("[ +] Add path: ADD...");
            foreach (var target in targetPaths)
            {
                Console.WriteLine($"[{++index,2}] Balancing for: {target}");
            }
            Console.WriteLine("[ a] Balancing for: ALL...");

            if (Directory.Exists(sourcePath) == false)
            {
                Console.WriteLine($"Source-Path '{sourcePath}' not exists");
            }
            foreach (var item in targetPaths)
            {
                if (Directory.Exists(item) == false)
                {
                    Console.WriteLine($" Target-Path '{item}' not exists");
                }
            }
        }
        private static void BalancingSolutions(string sourcePath, string[] sourceLabels, IEnumerable<string> targetPaths, string[] targetLabels)
        {
            var sourcePathExists = Directory.Exists(sourcePath);

            if (sourcePathExists)
            {
                var targetPathsExists = new List<string>();

                foreach (var item in targetPaths)
                {
                    if (Directory.Exists(item))
                    {
                        targetPathsExists.Add(item);
                    }
                }
                // Delete all target labeled files
                foreach (var targetPath in targetPathsExists)
                {
                    foreach (var searchPattern in SearchPatterns)
                    {
                        var targetCodeFiles = GetSourceCodeFiles(targetPath, searchPattern, targetLabels);

                        foreach (var targetCodeFile in targetCodeFiles)
                        {
                            File.Delete(targetCodeFile);
                        }
                    }
                }
                // Copy all source labeled files
                foreach (var searchPattern in SearchPatterns)
                {
                    var sourceCodeFiles = GetSourceCodeFiles(sourcePath, searchPattern, sourceLabels);

                    foreach (var targetPath in targetPathsExists)
                    {
                        foreach (var sourceCodeFile in sourceCodeFiles)
                        {
                            SynchronizeSourceCodeFile(sourcePath, sourceCodeFile, targetPath, sourceLabels, targetLabels);
                        }
                    }
                }
            }
        }
        private static bool SynchronizeSourceCodeFile(string sourcePath, string sourceFilePath, string targetPath, string[] sourceLabels, string[] targetLabels)
        {
            var result = false;
            var canCopy = true;
            var sourceSolutionName = Program.GetSolutionNameFromPath(sourcePath);
            var sourceProjectName = Program.GetProjectNameFromFilePath(sourceFilePath, sourceSolutionName);
            var sourceSubFilePath = sourceFilePath.Replace(sourcePath, string.Empty)
                                                  .Replace(sourceProjectName, string.Empty);
            var sourceSubFilePath2 = sourceFilePath.Replace(sourcePath, string.Empty)
                                                   .Replace(sourceProjectName, string.Empty)
                                                   .Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

            var targetSolutionName = Program.GetSolutionNameFromPath(targetPath);
            var targetProjectName = sourceProjectName.Replace(sourceSolutionName, targetSolutionName);
            var targetFilePath = Path.Combine(targetPath, targetProjectName, string.Join(Path.DirectorySeparatorChar, sourceSubFilePath2));
            var targetFileFolder = Path.GetDirectoryName(targetFilePath);
            var targetProjectPath = Path.Combine(targetPath, targetProjectName);

            if (Directory.Exists(targetProjectPath))
            {
                if (targetFileFolder != null && Directory.Exists(targetFileFolder) == false)
                {
                    Directory.CreateDirectory(targetFileFolder);
                }
                if (File.Exists(targetFilePath))
                {
                    var lines = File.ReadAllLines(targetFilePath, Encoding.Default);

                    canCopy = false;
                    if (lines.Any() && targetLabels.Any(l => lines.First().Contains(l)))
                    {
                        canCopy = true;
                    }
                }
                if (canCopy)
                {
                    var cpyLines = new List<string>();
                    var srcLines = File.ReadAllLines(sourceFilePath, Encoding.Default)
                                       .Select(i => i.Replace(sourceSolutionName, targetSolutionName));
                    var srcFirst = srcLines.FirstOrDefault();

                    if (srcFirst != null)
                    {
                        var label = sourceLabels.FirstOrDefault(l => srcFirst.Contains(l));

                        cpyLines.Add(srcFirst.Replace(label ?? string.Empty, StaticLiterals.CodeCopyLabel));
                    }
                    cpyLines.AddRange(File.ReadAllLines(sourceFilePath, Encoding.Default)
                                       .Skip(1)
                                       .Select(i => i.Replace(sourceSolutionName, targetSolutionName)));
                    File.WriteAllLines(targetFilePath, cpyLines.ToArray(), Encoding.UTF8);
                }
            }
            return result;
        }
        private static IEnumerable<string> GetSourceCodeFiles(string path, string searchPattern, string[] labels)
        {
            var result = new List<string>();
            var files = Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories)
                                 .Where(f => StaticLiterals.GenerationIgnoreFolders.Any(e => f.Contains(e) == false))
                                 .OrderBy(i => i);

            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file, Encoding.Default);

                if (lines.Any() && labels.Any(l => lines.First().Contains(l)))
                {
                    result.Add(file);
                }
                //System.Diagnostics.Debug.WriteLine($"{file}");
            }
            return result;
        }
        #endregion App methods

        #region Partial methods
        static partial void BeforeGetTargetPaths(string sourcePath, List<string> targetPaths, ref bool handled);
        static partial void AfterGetTargetPaths(string sourcePath, List<string> targetPaths);
        #endregion Partial methods
    }
}
//MdEnd