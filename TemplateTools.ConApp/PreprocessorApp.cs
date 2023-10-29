//@BaseCode
//MdStart
using CodeGenPreprocessor = TemplateCodeGenerator.Logic.Preprocessor;

namespace TemplateTools.ConApp
{
    /// <summary>
    /// Represents an application for preprocessing templates.
    /// </summary>
    public partial class PreprocessorApp
    {
        #region Class-Constructors
        /// <summary>
        /// Represents the PreprocessorApp class.
        /// </summary>
        /// <summary>
        /// Initializes a new instance of the <see cref="PreprocessorApp"/> class.
        /// </summary>
        static PreprocessorApp()
        {
            ClassConstructing();
            SourcePath = Program.SolutionPath;
            ClassConstructed();
        }
        /// <summary>
        /// This method is called before the constructor of the class is executed.
        /// </summary>
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
        private static string SourcePath { get; set; }
        /// <summary>
        /// Gets or sets the defines used for code generation in the project file.
        /// </summary>
        /// <value>
        /// An array of strings representing the defines used for code generation in the project file.
        /// </value>
        private static string[] Defines { get; set; } = CodeGenPreprocessor.ProjectFile.Defines;
        #endregion Properties
        
        #region Console methods
        /// <summary>
        /// Runs the application.
        /// </summary>
        // Read defines from the solution
        public static void RunApp()
        {
            var input = string.Empty;
            var saveForeColor = Console.ForegroundColor;
            
            Program.PrintBusyProgress();
            while (input!.Equals("x") == false)
            {
                var changedDefines = false;
                var sourceSolutionName = Program.GetSolutionNameFromPath(SourcePath);
                
                // Read defines from the solution
                Defines = CodeGenPreprocessor.ProjectFile.ReadDefinesInProjectFiles(SourcePath);
                
                Program.RunBusyProgress = false;
                Console.Clear();
                Console.ForegroundColor = Program.ForegroundColor;
                Console.WriteLine("Template Preprocessor");
                Console.WriteLine("=====================");
                Console.WriteLine();
                PrintDefines(Defines);
                Console.WriteLine();
                Console.WriteLine($"Define constants for: '{sourceSolutionName}' from: {SourcePath}");
                Console.WriteLine();
                PrintMenu(Defines);
                Console.WriteLine();
                Console.Write("Choose [n|n,n|x|X]: ");
                
                input = Console.ReadLine()?.ToLower() ?? String.Empty;
                var numbers = input?.Trim()
                                    .Split(',').Where(s => Int32.TryParse(s, out int n))
                                    .Select(s => Int32.Parse(s))
                                    .ToArray();
                
                for (int n = 0; n < numbers!.Length; n++)
                {
                    var select = numbers[n];
                    
                    if (select == 1)
                    {
                        var solutionPath = Program.GetCurrentSolutionPath();
                        var qtSolutions = Program.GetQuickTemplateSolutions(Program.SourcePath).Union(new string[] { solutionPath }).ToArray();
                        
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
                                Defines = CodeGenPreprocessor.ProjectFile.ReadDefinesInProjectFiles(SourcePath, Defines);
                            }
                        }
                        else if (string.IsNullOrEmpty(selectOrPath) == false && Directory.Exists(selectOrPath))
                        {
                            SourcePath = selectOrPath;
                            Defines = CodeGenPreprocessor.ProjectFile.ReadDefinesInProjectFiles(SourcePath, Defines);
                        }
                    }
                    else if ((select - 2) >= 0 && (select - 2) < Defines.Length)
                    {
                        var defIdx = select - 2;
                        
                        changedDefines = true;
                        SwitchDefine(Defines, defIdx);
                    }
                    else if ((select - 2) == Defines.Length)
                    {
                        Program.PrintBusyProgress();
                        CodeGenPreprocessor.ProjectFile.WriteDefinesInProjectFiles(SourcePath, Defines);
                        CodeGenPreprocessor.PreprocessorCommentHelper.SetPreprocessorDefineCommentsInFiles(SourcePath, Defines);
                    }
                }
                
                if (changedDefines)
                {
                    Program.PrintBusyProgress();
                    CodeGenPreprocessor.ProjectFile.WriteDefinesInProjectFiles(SourcePath, Defines);
                    CodeGenPreprocessor.PreprocessorCommentHelper.SetPreprocessorDefineCommentsInFiles(SourcePath, Defines);
                }
                
                changedDefines = false;
                Program.RunBusyProgress = false;
                Console.ResetColor();
            }
        }
        
        /// <summary>
        /// Prints the define values.
        /// </summary>
        /// <param name="defines">An array of strings representing the defines.</param>
        private static void PrintDefines(string[] defines)
        {
            Console.WriteLine("Define-Values:");
            Console.WriteLine("--------------");
            foreach (var define in defines)
            {
                PrintDefine(define);
                Console.Write(" ");
            }
            Console.WriteLine();
        }
        /// <summary>
        /// Prints the given define string to the console with a specific color based on the last characters of the string.
        /// </summary>
        /// <param name="define">The string to be printed.</param>
        private static void PrintDefine(string define)
        {
            var saveColor = Console.ForegroundColor;
            
            if (define.EndsWith("_ON"))
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }
            Console.Write($"{define}");
            Console.ForegroundColor = saveColor;
        }
        /// <summary>
        /// Prints the menu options for setting definitions and changing source paths.
        /// </summary>
        /// <param name="defines">An array of string values representing the definitions.</param>
        /// <remarks>
        /// The method displays a menu with options for changing the source path and setting definitions.
        /// It iterates through the <paramref name="defines"/> array and checks if each value ends with "_ON".
        /// If it does, it displays the option to turn the definition off, otherwise, it displays the option to turn it on.
        /// The menu also includes an option to start the assignment process and an option to exit.
        /// </remarks>
        private static void PrintMenu(string[] defines)
        {
            var menuIndex = 0;
            var saveColor = Console.ForegroundColor;
            
            Console.WriteLine($"[{++menuIndex,-2}] Change source path");
            
            for (int i = 0; i < defines.Length; i++)
            {
                if (defines[i].EndsWith("_ON"))
                {
                    Console.ForegroundColor = saveColor;
                    Console.Write($"[{++menuIndex,-2}] Set definition ");
                    
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{defines[i],-20}");
                    
                    Console.ForegroundColor = saveColor;
                    Console.Write(" ==> ");
                    
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"{defines[i].Replace("_ON", "_OFF")}");
                }
                else
                {
                    Console.ForegroundColor = saveColor;
                    Console.Write($"[{++menuIndex,-2}] Set definition ");
                    
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write($"{defines[i],-20}");
                    
                    Console.ForegroundColor = saveColor;
                    Console.Write(" ==> ");
                    
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{defines[i].Replace("_OFF", "_ON")}");
                }
            }
            Console.ForegroundColor = saveColor;
            Console.WriteLine($"[{++menuIndex,-2}] Start assignment process...");
            Console.WriteLine("[x|X] Exit");
        }
        #endregion Console methods
        
        /// <summary>
        /// Switches a defined value based on the given index.
        /// </summary>
        /// <param name="defines">An array of defined values.</param>
        /// <param name="idx">The index of the value to switch.</param>
        internal static void SwitchDefine(string[] defines, int idx)
        {
            if (idx >= 0 && idx < defines.Length)
            {
                if (defines[idx].EndsWith("_ON"))
                {
                    if (defines[idx].StartsWith("IDINT_") == false
                    && defines[idx].StartsWith("IDLONG_") == false
                    && defines[idx].StartsWith("IDGUID_") == false
                    && defines[idx].StartsWith("SQLSERVER_") == false
                    && defines[idx].StartsWith("SQLITE_") == false)
                    {
                        defines[idx] = defines[idx].Replace("_ON", "_OFF");
                    }
                }
                else
                {
                    if (defines[idx].StartsWith("IDINT_") == true)
                    {
                        SwitchDefine(defines, "IDINT_", "ON");
                        SwitchDefine(defines, "IDLONG_", "OFF");
                        SwitchDefine(defines, "IDGUID_", "OFF");
                    }
                    else if (defines[idx].StartsWith("IDLONG_") == true)
                    {
                        SwitchDefine(defines, "IDINT_", "OFF");
                        SwitchDefine(defines, "IDLONG_", "ON");
                        SwitchDefine(defines, "IDGUID_", "OFF");
                    }
                    else if (defines[idx].StartsWith("IDGUID_") == true)
                    {
                        SwitchDefine(defines, "IDINT_", "OFF");
                        SwitchDefine(defines, "IDLONG_", "OFF");
                        SwitchDefine(defines, "IDGUID_", "ON");
                    }
                    else if (defines[idx].StartsWith("SQLSERVER_") == true)
                    {
                        SwitchDefine(defines, "SQLITE_", "OFF");
                        SwitchDefine(defines, "SQLSERVER_", "ON");
                    }
                    else if (defines[idx].StartsWith("SQLITE_") == true)
                    {
                        SwitchDefine(defines, "SQLSERVER_", "OFF");
                        SwitchDefine(defines, "SQLITE_", "ON");
                    }
                    else
                    {
                        defines[idx] = defines[idx].Replace("_OFF", "_ON");
                    }
                }
            }
        }
        /// <summary>
        /// Switches the defines in the project file using the specified prefix and postfix.
        /// </summary>
        /// <param name="defines">An array of strings that represents the defines to be switched.</param>
        /// <param name="definePrefix">A string that represents the prefix to be added to the defines.</param>
        /// <param name="definePostfix">A string that represents the postfix to be added to the defines.</param>
        /// <remarks>
        /// This method is used to switch the defines in a project file. It takes an array of defines, a prefix, and a postfix.
        /// The defines in the project file are switched by adding the prefix and postfix to each define.
        /// </remarks>
        internal static void SwitchDefine(string[] defines, string definePrefix, string definePostfix)
        {
            CodeGenPreprocessor.ProjectFile.SwitchDefine(defines, definePrefix, definePostfix);
        }
    }
}
//MdEnd

