//@BaseCode
//MdStart
using TemplateTooles.ConApp;

namespace TemplateTools.ConApp
{
    public partial class CopierApp
    {
        #region Class-Constructors
        static CopierApp()
        {
            ClassConstructing();
            SourcePath = Program.SourcePath;
            TargetPath = Directory.GetParent(SourcePath)?.FullName ?? String.Empty;
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        #endregion Class-Constructors

        #region Properties
        private static string SourcePath { get; set; }
        private static string TargetPath { get; set; }
        #endregion Properties

        public static void RunApp()
        {
            var input = string.Empty;
            var targetSolutionName = "TargetSolution";
            var saveForeColor = Console.ForegroundColor;

            while (input.Equals("x") == false)
            {
                var sourceSolutionName = Program.GetSolutionNameByPath(SourcePath);
                var sourceProjects = StaticLiterals.SolutionProjects
                                                   .Concat(StaticLiterals.ProjectExtensions.Select(e => $"{sourceSolutionName}{e}"));

                Console.Clear();
                Console.ForegroundColor = Program.ForegroundColor;
                Console.WriteLine("Template Copier");
                Console.WriteLine("===============");
                Console.WriteLine();
                Console.WriteLine($"Copy '{sourceSolutionName}' from: {Program.SourcePath}");
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
                        var qtProjects = Program.GetQuickTemplateProjects(Program.UserPath).Union(new[] { solutionPath }).ToArray();

                        for (int i = 0; i < qtProjects.Length; i++)
                        {
                            if (i == 0)
                                Console.WriteLine();

                            Console.WriteLine($"Change path to: [{i + 1}] {qtProjects[i]}");
                        }
                        Console.WriteLine();
                        Console.Write("Select or enter source path: ");
                        var selectOrPath = Console.ReadLine();

                        if (Int32.TryParse(selectOrPath, out int number))
                        {
                            if ((number - 1) >= 0 && (number - 1) < qtProjects.Length)
                            {
                                SourcePath = qtProjects[number - 1];
                            }
                        }
                        else if (string.IsNullOrEmpty(selectOrPath) == false)
                        {
                            SourcePath = selectOrPath;
                        }
                    }
                    else if (select == 2)
                    {
                        var solutionPath = Program.GetCurrentSolutionPath();
                        var qtProjects = Program.GetQuickTemplateProjects(Program.UserPath).Union(new[] { solutionPath }).ToArray();
                        var qtPaths = qtProjects.Select(p => Program.GetParentDirectory(p)).Distinct().OrderBy(p => p).ToArray();

                        for (int i = 0; i < qtPaths.Length; i++)
                        {
                            if (i == 0)
                                Console.WriteLine();

                            Console.WriteLine($"Change path to: [{i + 1}] {qtPaths[i]}");
                        }
                        Console.WriteLine();
                        Console.Write("Select or enter target path: ");
                        var selectOrPath = Console.ReadLine();

                        if (Int32.TryParse(selectOrPath, out int number))
                        {
                            if ((number - 1) >= 0 && (number - 1) < qtPaths.Length)
                            {
                                TargetPath = qtPaths[number - 1];
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
                        copier.Copy(SourcePath, targetSolutionPath, sourceProjects);
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
