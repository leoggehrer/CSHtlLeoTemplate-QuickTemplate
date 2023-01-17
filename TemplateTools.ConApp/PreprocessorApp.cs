//@BaseCode
//MdStart
using System;
using System.Text;
using CodeGenPreprocessor = TemplateCodeGenerator.Logic.Preprocessor;

namespace TemplateTools.ConApp
{
	public partial class PreprocessorApp
	{
        #region Class-Constructors
        static PreprocessorApp()
        {
            ClassConstructing();
            SourcePath = Program.SourcePath;
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        #endregion Class-Constructors

        #region Properties
        private static string SourcePath { get; set; }
        private static string[] Defines { get; set; } = CodeGenPreprocessor.ProjectFile.Defines;
        #endregion Properties

        #region Console methods
        public static void RunApp()
        {
            var input = string.Empty;
            var saveForeColor = Console.ForegroundColor;

            Program.PrintBusyProgress();
            while (input!.Equals("x") == false)
            {
                var changedDefines = false;
                var sourceSolutionName = Program.GetSolutionNameByPath(SourcePath);

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
                Console.WriteLine($"Set define-values '{sourceSolutionName}' from: {SourcePath}");
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
                        var qtSolutions = Program.GetQuickTemplateSolutions(Program.UserPath).Union(new string[] { solutionPath }).ToArray();

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
                        SetPreprocessorDefinesInRazorFiles(SourcePath, Defines);
                    }
                }

                if (changedDefines)
                {
                    Program.PrintBusyProgress();
                    CodeGenPreprocessor.ProjectFile.WriteDefinesInProjectFiles(SourcePath, Defines);
                    SetPreprocessorDefinesInRazorFiles(SourcePath, Defines);
                }

                changedDefines = false;
                Program.RunBusyProgress = false;
                Console.ResetColor();
            }
        }

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
        private static void PrintSolutionDirectives(string path, params string[] excludeDirectives)
        {
            var files = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var idx = 0;
                var lines = File.ReadAllLines(file, Encoding.Default);

                foreach (var line in lines)
                {
                    if (line.Trim().StartsWith("#if ") && excludeDirectives.Any(e => line.Contains(e)) == false)
                    {
                        var message = $"{line} in line {idx} of the {file} file";

                        //Console.WriteLine(message);
                        //Debug.WriteLine(message);
                    }
                    idx++;
                }
            }
        }
        #endregion Console methods

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
        internal static void SwitchDefine(string[] defines, string definePrefix, string definePostfix)
        {
            CodeGenPreprocessor.ProjectFile.SwitchDefine(defines, definePrefix, definePostfix);
        }

        private static void SetPreprocessorDefinesInRazorFiles(string path, params string[] defineItems)
        {
            foreach (var directive in defineItems)
            {
                var analyzeDirective = directive.ToUpper();

                if (analyzeDirective.EndsWith("_OFF", StringComparison.CurrentCultureIgnoreCase))
                {
                    SetPreprocessorDefinesCommentsInRazorFiles(path, directive.Replace("_OFF", "_ON"));
                    RemovePreprocessorDefinesCommentsInRazorFiles(path, directive);
                }
                else if (analyzeDirective.EndsWith("_ON", StringComparison.CurrentCultureIgnoreCase))
                {
                    SetPreprocessorDefinesCommentsInRazorFiles(path, directive.Replace("_ON", "_OFF"));
                    RemovePreprocessorDefinesCommentsInRazorFiles(path, directive);
                }
            }
        }
        private static void SetPreprocessorDefinesCommentsInRazorFiles(string path, params string[] defineItems)
        {
            var files = Directory.GetFiles(path, "*.cshtml", SearchOption.AllDirectories);

            foreach (var directive in defineItems)
            {
                foreach (var file in files)
                {
                    var startIndex = 0;
                    var hasChanged = false;
                    var result = string.Empty;
                    var text = File.ReadAllLines(file, Encoding.Default)
                                   .Select(l =>
                                   {
                                       if (l.Contains($"@*#if {directive}*@") || l.Contains("@*#endif*@"))
                                       {
                                           l = l.Trim();
                                       }
                                       return l;
                                   }).ToText();

                    foreach (var tag in text.GetAllTags($"@*#if {directive}*@", "@*#endif*@"))
                    {
                        if (tag.StartTagIndex > startIndex)
                        {
                            result += text.Partialstring(startIndex, tag.StartTagIndex - 1);
                            result += tag.StartTag;
                            if (tag.InnerText.Trim().StartsWith("@*"))
                            {
                                result += tag.InnerText;
                            }
                            else
                            {
                                hasChanged = true;
                                result += /*Environment.NewLine + */"@*";
                                result += tag.InnerText;
                                result += "*@";// + Environment.NewLine;
                            }
                            result += tag.EndTag;
                            startIndex += tag.EndTagIndex + tag.EndTag.Length;
                        }
                    }
                    if (hasChanged && startIndex < text.Length)
                    {
                        result += text.Partialstring(startIndex, text.Length);
                    }
                    if (hasChanged)
                    {
                        File.WriteAllText(file, result, Encoding.Default);
                    }
                }
            }
        }
        private static void RemovePreprocessorDefinesCommentsInRazorFiles(string path, params string[] defineItems)
        {
            var files = Directory.GetFiles(path, "*.cshtml", SearchOption.AllDirectories);

            foreach (var directive in defineItems)
            {
                foreach (var file in files)
                {
                    var startIndex = 0;
                    var hasChanged = false;
                    var result = string.Empty;
                    var text = File.ReadAllText(file, Encoding.Default);

                    foreach (var tag in text.GetAllTags($"@*#if {directive}*@", "@*#endif*@"))
                    {
                        if (tag.StartTagIndex > startIndex)
                        {
                            result += text.Partialstring(startIndex, tag.StartTagIndex - 1);
                            result += tag.StartTag;
                            var innerText = tag.InnerText.Trim(Environment.NewLine.ToCharArray());
                            if (innerText.Trim().StartsWith("@*") && innerText.Trim().EndsWith("*@"))
                            {
                                hasChanged = true;
                                result += innerText.Partialstring(2, innerText.Length - 5);
                                result += Environment.NewLine;
                            }
                            else
                            {
                                result += tag.InnerText;
                            }
                            startIndex += tag.EndTagIndex + tag.EndTag.Length;
                            result += tag.EndTag;
                        }
                    }
                    if (hasChanged && startIndex < text.Length)
                    {
                        result += text.Partialstring(startIndex, text.Length);
                    }
                    if (hasChanged)
                    {
                        File.WriteAllText(file, result, Encoding.Default);
                    }
                }
            }
        }
        private static void ReplacePreprocessorDefinesInRazorFiles(string path, params string[] defineItems)
        {
            var files = Directory.GetFiles(path, "*.cshtml", SearchOption.AllDirectories);

            foreach (var directive in defineItems)
            {
                var labels = new[] { $"#if {directive}", "#endif" };

                foreach (var file in files)
                {
                    var hasChanged = false;
                    var result = new List<string>();
                    var lines = File.ReadAllLines(file, Encoding.Default);

                    foreach (var line in lines)
                    {
                        var targetLine = line;

                        foreach (var label in labels)
                        {
                            if (line.StartsWith(label))
                            {
                                hasChanged = true;
                                targetLine = line.Replace(label, $"@*{label}*@");
                            }
                        }
                        result.Add(targetLine);
                    }
                    if (hasChanged)
                    {
                        File.WriteAllLines(file, result, Encoding.Default);
                    }
                }
            }
        }

    }
}
//MdEnd
