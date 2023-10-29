//@BaseCode
//MdStart
using TemplateCodeGenerator.Logic;
using TemplateTooles.ConApp;

namespace TemplateTools.ConApp
{
    /// <summary>
    /// Represents an application for copying template solutions to a target solution.
    /// </summary>
    public partial class CopierApp
    {
        #region Class-Constructors
        /// <summary>
        /// This is the static constructor for the CopierApp class.
        /// </summary>
        /// <remarks>
        /// This constructor is responsible for initializing the static members of the CopierApp class.
        /// </remarks>
        static CopierApp()
        {
            ClassConstructing();
            SourcePath = Program.SolutionPath;
            TargetPath = Directory.GetParent(SourcePath)?.FullName ?? String.Empty;
            ClassConstructed();
        }
        /// <summary>
        /// This method is called when the class is being constructed.
        /// </summary>
        /// <remarks>
        /// This is a partial method and must be implemented in a partial class.
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
        /// <value>
        /// The source path.
        /// </value>
        private static string SourcePath { get; set; }
        /// <summary>
        /// Gets or sets the target path.
        /// </summary>
        /// <value>
        /// The target path.
        /// </value>
        private static string TargetPath { get; set; }
        #endregion Properties
        
        /// <summary>
        /// Runs the template copier application.
        /// </summary>
        public static void RunApp()
        {
            var input = string.Empty;
            var targetSolutionName = "TargetSolution";
            var saveForeColor = Console.ForegroundColor;
            
            while (input.Equals("x") == false)
            {
                var solutionProperties = SolutionProperties.Create(SourcePath);
                var sourceSolutionName = solutionProperties.SolutionName;
                var allSourceProjectNames = solutionProperties.AllTemplateProjectNames;
                
                Console.Clear();
                Console.ForegroundColor = Program.ForegroundColor;
                Console.WriteLine("Template Copier");
                Console.WriteLine("===============");
                Console.WriteLine();
                Console.WriteLine($"Copy '{sourceSolutionName}' from: {SourcePath}");
                Console.WriteLine($"Copy to '{targetSolutionName}':   {Path.Combine(TargetPath, targetSolutionName)}");
                Console.WriteLine();
                Console.WriteLine("[1] Change source path");
                Console.WriteLine("[2] Change target path");
                Console.WriteLine("[3] Change target solution name");
                Console.WriteLine("[4] Start copy process");
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
                            Console.WriteLine();
                            
                            Console.WriteLine($"Change path to: [{i + 1}] {qtSolutions[i]}");
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
                        else if (string.IsNullOrEmpty(selectOrPath) == false)
                        {
                            SourcePath = selectOrPath;
                        }
                    }
                    else if (select == 2)
                    {
                        var qtParentPaths = Program.GetQuickTemplateParentPaths(Program.SourcePath, SourcePath);
                        
                        for (int i = 0; i < qtParentPaths.Length; i++)
                        {
                            if (i == 0)
                            Console.WriteLine();
                            
                            Console.WriteLine($"Change path to: [{i + 1}] {qtParentPaths[i]}");
                        }
                        Console.WriteLine();
                        Console.Write("Select or enter target path: ");
                        var selectOrPath = Console.ReadLine();
                        
                        if (Int32.TryParse(selectOrPath, out int number))
                        {
                            if ((number - 1) >= 0 && (number - 1) < qtParentPaths.Length)
                            {
                                TargetPath = qtParentPaths[number - 1];
                            }
                        }
                        else if (string.IsNullOrEmpty(selectOrPath) == false)
                        {
                            TargetPath = selectOrPath;
                        }
                    }
                    else if (select == 3)
                    {
                        Console.Write("Enter target solution name: ");
                        targetSolutionName = Console.ReadLine() ?? String.Empty;
                    }
                    else if (select == 4)
                    {
                        var copier = new Copier();
                        var targetSolutionPath = Path.Combine(TargetPath, targetSolutionName);
                        
                        Program.PrintBusyProgress();
                        copier.Copy(SourcePath, targetSolutionPath, allSourceProjectNames);
                        Program.RunBusyProgress = false;
                        
                        Program.OpenSolutionFolder(targetSolutionPath);
                    }
                    Console.ResetColor();
                }
            }
        }
    }
}
//MdEnd
