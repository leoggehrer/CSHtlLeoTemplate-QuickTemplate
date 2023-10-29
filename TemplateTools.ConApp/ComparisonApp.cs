//@BaseCode
//MdStart
using System.Text;
using CommonStaticLiterals = CommonBase.StaticLiterals;

namespace TemplateTools.ConApp
{
    /// <summary>
    /// Represents an application for comparing and synchronizing source code files between a source path and multiple target paths.
    /// </summary>
    internal partial class ComparisonApp
    {
        #region Class-Constructors
        /// <summary>
        /// Initializes static members of the ComparisonApp class.
        /// </summary>
        /// <remarks>
        /// This constructor sets up the initial values for the static properties and arrays used in the ComparisonApp class.
        /// </remarks>
        static ComparisonApp()
        {
            ClassConstructing();
            SolutionPath = Program.SolutionPath;
            TargetPaths = Array.Empty<string>();
            AddTargetPaths = Array.Empty<string>();
            SourceLabels = new string[] { CommonStaticLiterals.BaseCodeLabel, CommonStaticLiterals.BaseCodeLabel };
            TargetLabels = new string[] { CommonStaticLiterals.CodeCopyLabel, CommonStaticLiterals.BaseCodeLabel };
            ClassConstructed();
        }
        /// <summary>
        /// This method is called just before the class constructor is called.
        /// </summary>
        /// <remarks>
        /// This method can be used to perform any necessary initialization or setup
        /// logic specific to the class being constructed, before the constructor
        /// is executed.
        /// </remarks>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when the class is constructed.
        /// </summary>
        static partial void ClassConstructed();
        #endregion Class-Constructors
        
        #region Properties
        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        private static string SolutionPath { get; set; }
        /// <summary>
        /// Gets or sets the target paths.
        /// </summary>
        /// <value>
        /// An array of strings representing the target paths.
        /// </value>
        private static string[] TargetPaths { get; set; }
        /// <summary>
        /// Gets or sets the target paths to be added.
        /// </summary>
        /// <value>
        /// An array of strings representing the target paths to be added.
        /// </value>
        private static string[] AddTargetPaths { get; set; }
        /// <summary>
        /// Gets an array of search patterns used for searching source files.
        /// </summary>
        /// <value>
        /// An array of search patterns.
        /// </value>
        private static string[] SearchPatterns => CommonStaticLiterals.SourceFileExtensions.Split('|');
        /// <summary>
        /// Gets or sets the source labels.
        /// </summary>
        /// <value>
        /// The source labels.
        /// </value>
        private static string[] SourceLabels { get; set; }
        /// <summary>
        /// Gets or sets the target labels.
        /// </summary>
        /// <value>
        /// An array of strings representing the target labels.
        /// </value>
        private static string[] TargetLabels { get; set; }
        #endregion Properties
        
        #region App methods
        ///<summary>
        /// Runs the application.
        ///</summary>
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
                BeforeGetTargetPaths(Program.SourcePath, targetPaths, ref handled);
                if (handled == false)
                {
                    TargetPaths = Program.GetQuickTemplateSolutions(Program.SourcePath);
                    TargetPaths = TargetPaths.Union(AddTargetPaths).ToArray();
                }
                else
                {
                    TargetPaths = TargetPaths.Union(AddTargetPaths).ToArray();
                }
                Program.RunBusyProgress = false;
                Task.Delay(250).Wait();
                PrintHeader(SolutionPath, TargetPaths);
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
                        BalancingSolutions(SolutionPath, SourceLabels, TargetPaths, TargetLabels);
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
                        var numbers = input!.Trim()
                                            .Split(',').Where(s => Int32.TryParse(s, out int n))
                                            .Select(s => Int32.Parse(s))
                                            .Distinct()
                                            .ToArray();
                        
                        Program.PrintBusyProgress();
                        foreach (var number in numbers)
                        {
                            if (number == TargetPaths.Length + 1)
                            {
                                BalancingSolutions(SolutionPath, SourceLabels, TargetPaths, TargetLabels);
                            }
                            else if (number > 0 && number <= TargetPaths.Length)
                            {
                                BalancingSolutions(SolutionPath, SourceLabels, new string[] { TargetPaths[number - 1] }, TargetLabels);
                            }
                        }
                    }
                }
                AfterGetTargetPaths(Program.SourcePath, targetPaths);
            } while (running);
        }
        /// <summary>
        /// Prints the header for the template comparison.
        /// </summary>
        /// <param name="sourcePath">The path of the source.</param>
        /// <param name="targetPaths">An array of target paths.</param>
        /// <remarks>
        /// This method prints the header for the template comparison by displaying the balance label(s), the source path, and the target paths.
        /// If the source path or any of the target paths do not exist, an appropriate message is printed.
        /// </remarks>
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
        /// <summary>
        /// Balances the solutions by deleting target labeled files and copying source labeled files.
        /// </summary>
        /// <param name="sourcePath">The path of the source directory.</param>
        /// <param name="sourceLabels">An array of source labels.</param>
        /// <param name="targetPaths">An IEnumerable containing the target paths.</param>
        /// <param name="targetLabels">An array of target labels.</param>
        /// <remarks>
        /// This method deletes all target labeled files in the targetPaths and then copies all source labeled files from the sourcePath to the targetPaths.
        /// </remarks>
        private static void BalancingSolutions(string sourcePath, string[] sourceLabels, IEnumerable<string> targetPaths, string[] targetLabels)
        {
            var sourcePathExists = Directory.Exists(sourcePath);
            
            if (sourcePathExists && sourceLabels.Length == targetLabels.Length)
            {
                var targetPathsExists = new List<string>();
                
                foreach (var item in targetPaths)
                {
                    if (Directory.Exists(item))
                    {
                        targetPathsExists.Add(item);
                    }
                }
                for (int i = 0; i < sourceLabels.Length; i++)
                {
                    var targetLabel = targetLabels[i];
                    var sourceLabel = sourceLabels[i];
                    
                    // Delete all target labeled files
                    foreach (var targetPath in targetPathsExists)
                    {
                        foreach (var searchPattern in SearchPatterns)
                        {
                            var targetCodeFiles = GetSourceCodeFiles(targetPath, searchPattern, targetLabel);
                            
                            foreach (var targetCodeFile in targetCodeFiles)
                            {
                                if (CanDeleteTargetCodeFile(sourcePath, targetCodeFile))
                                {
                                    File.Delete(targetCodeFile);
                                }
                            }
                        }
                    }
                    // Copy all source labeled files
                    foreach (var searchPattern in SearchPatterns)
                    {
                        var sourceCodeFiles = GetSourceCodeFiles(sourcePath, searchPattern, sourceLabel);
                        
                        foreach (var targetPath in targetPathsExists)
                        {
                            foreach (var sourceCodeFile in sourceCodeFiles)
                            {
                                SynchronizeSourceCodeFile(sourcePath, sourceCodeFile, targetPath, sourceLabel, targetLabel);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Determines whether the target code file can be deleted.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="targetFilePath">The target file path.</param>
        /// <returns>True if the target code file can be deleted; otherwise, false.</returns>
        private static bool CanDeleteTargetCodeFile(string sourcePath, string targetFilePath)
        {
            var result = false;
            var targetProjectPath = Program.GetPathFromPath(targetFilePath, ".csproj");
            
            if (targetProjectPath.HasContent())
            {
                var subFilePath = targetFilePath.Replace(targetProjectPath, string.Empty);
                var targetSolutionName = Program.GetDirectoryNameFromPath(targetFilePath, ".sln");
                var targetProjectName = Program.GetDirectoryNameFromPath(targetFilePath, ".csproj");
                var sourceSolutionName = Program.GetDirectoryNameFromPath(sourcePath, ".sln");
                var sourceProjectName = targetProjectName.Replace(targetSolutionName, sourceSolutionName);
                var sourceFilePath = $"{sourcePath}{Path.DirectorySeparatorChar}{sourceProjectName}{Path.DirectorySeparatorChar}{subFilePath}";
                var sourceProjectPath = Program.GetPathFromPath(sourceFilePath, ".csproj");
                
                if (sourceProjectPath.HasContent())
                {
                    result = true;
                }
            }
            return result;
        }
        /// <summary>
        /// Copies a source code file from the source path to the target path and synchronizes it
        /// with the specified labels.
        /// </summary>
        /// <param name="sourcePath">The path of the source directory.</param>
        /// <param name="sourceFilePath">The path of the source code file to be synchronized.</param>
        /// <param name="targetPath">The path of the target directory where the file will be synchronized.</param>
        /// <param name="sourceLabel">The label associated with the source code file.</param>
        /// <param name="targetLabel">The label associated with the target code file.</param>
        /// <returns>True if the synchronization is successful, otherwise false.</returns>
        private static bool SynchronizeSourceCodeFile(string sourcePath, string sourceFilePath, string targetPath, string sourceLabel, string targetLabel)
        {
            var result = false;
            var canCopy = true;
            var sourceSolutionName = Program.GetSolutionNameFromPath(sourcePath);
            var sourceProjectName = Program.GetProjectNameFromPath(sourceFilePath);
            var sourceSubFilePath = Program.GetSubFilePath(sourceFilePath);
            var targetSolutionName = Program.GetSolutionNameFromPath(targetPath);
            var targetProjectName = sourceProjectName.Replace(sourceSolutionName, targetSolutionName);
            var targetFilePath = Path.Combine(targetPath, targetProjectName, sourceSubFilePath);
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
                    if (lines.Any() && lines.First().Contains(targetLabel))
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
                        cpyLines.Add(srcFirst.Replace(sourceLabel, targetLabel));
                    }
                    cpyLines.AddRange(File.ReadAllLines(sourceFilePath, Encoding.Default)
                            .Skip(1)
                            .Select(i => i.Replace(sourceSolutionName, targetSolutionName)));
                    File.WriteAllLines(targetFilePath, cpyLines.ToArray(), Encoding.UTF8);
                }
            }
            return result;
        }
        /// <summary>
        /// Retrieves a collection of source code files from a given directory path.
        /// </summary>
        /// <param name="path">The root directory path where the search will begin.</param>
        /// <param name="searchPattern">The search pattern used to filter the files.</param>
        /// <param name="label">The label used to identify relevant files.</param>
        /// <returns>A collection of file paths that match the search pattern and contain the specified label.</returns>
        private static IEnumerable<string> GetSourceCodeFiles(string path, string searchPattern, string label)
        {
            var result = new List<string>();
            var files = Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories)
                                 .Where(f => CommonStaticLiterals.GenerationIgnoreFolders.Any(e => f.Contains(e) == false))
                                 .OrderBy(i => i);
            
            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file, Encoding.Default);
                
                if (lines.Any() && lines.First().Contains(label))
                {
                    result.Add(file);
                }
                //System.Diagnostics.Debug.WriteLine($"{file}");
            }
            return result;
        }
        #endregion App methods
        
        #region Partial methods
        /// <summary>
        /// Represents a method that is called before the GetTargetPaths method is executed.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="targetPaths">The list of target paths.</param>
        /// <param name="handled">A reference to a boolean value indicating whether the method has been handled.</param>
        static partial void BeforeGetTargetPaths(string sourcePath, List<string> targetPaths, ref bool handled);
        /// <summary>
        /// This method is called after getting the target paths for a source path.
        /// </summary>
        /// <param name="sourcePath">The source path for which the target paths were retrieved.</param>
        /// <param name="targetPaths">The list of target paths corresponding to the provided source path.</param>
        /// <remarks>
        /// This method allows performing additional tasks or modifications after obtaining the target paths
        /// for a given source path. It can be overridden in derived classes to customize the behavior.
        /// </remarks>
        static partial void AfterGetTargetPaths(string sourcePath, List<string> targetPaths);
        #endregion Partial methods
    }
}
//MdEnd

