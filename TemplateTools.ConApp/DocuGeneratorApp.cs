//@CodeCopy
//MdStart
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;
using TemplateCodeGenerator.Logic;
using TemplateTools.ConApp.Models;
using CommonStaticLiterals = CommonBase.StaticLiterals;

namespace TemplateTools.ConApp
{
    /// <summary>
    /// Represents the DocuGeneratorApp class.
    /// </summary>
    internal partial class DocuGeneratorApp
    {
        #region Class=Constructors
        /// <summary>
        /// Static constructor for the <see cref="DocuGeneratorApp"/> class.
        /// </summary>
        /// <remarks>
        /// This static constructor initializes the DocuGeneratorApp class by performing class-specific operations.
        /// </remarks>
        static DocuGeneratorApp()
        {
            ClassConstructing();
            Force = false;
            SolutionPath = Program.SolutionPath;
            DocuPath = SolutionPath;
            ClassConstructed();
        }
        /// <summary>
        /// This method is called when a class is being constructed.
        /// </summary>
        /// <remarks>
        /// Use this method to perform additional tasks before the class is fully constructed.
        /// This is a partial method and is intended to be implemented in a separate file.
        /// </remarks>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when the class is constructed.
        /// </summary>
        /// <remarks>
        /// This method is declared as "partial" to allow adding implementation in a separate file.
        /// </remarks>
        static partial void ClassConstructed();
        #endregion Class-Constructors
        
        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether the force property should be used.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the force property should be used; otherwise, <c>false</c>.
        /// </value>
        private static bool Force { get; set; }
        /// <summary>
        /// Gets or sets the solution path.
        /// </summary>
        /// <value>
        /// A string representing the solution path.
        /// </value>
        /// <remarks>
        /// This property holds the file path of the solution directory.
        /// </remarks>
        private static string SolutionPath { get; set; }
        /// <summary>
        /// Gets or sets the path of the documentation file.
        /// </summary>
        private static string DocuPath { get; set; }
        #endregion Properties
        
        #region Console methods
        /// <summary>
        /// Runs the application and displays a menu for documentation generation.
        /// </summary>
        public static void RunApp()
        {
            var force = Force;
            var input = string.Empty;
            var saveForeColor = Console.ForegroundColor;
            
            while (input.Equals("x") == false)
            {
                var menuIndex = 0;
                var sourceSolutionName = Program.GetSolutionNameByPath(SolutionPath);
                
                Console.Clear();
                Console.ForegroundColor = Program.ForegroundColor;
                Console.WriteLine("Documentation Generator");
                Console.WriteLine("=======================");
                Console.WriteLine();
                Console.WriteLine($"Code generation path: {SolutionPath}");
                Console.WriteLine($"Code generation for:  {sourceSolutionName}");
                Console.WriteLine();
                Console.WriteLine($"Docu generation path: {DocuPath}");
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine($"Force documentation:  {(force ? "Yes" : "No")}");
                Console.WriteLine();
                Console.WriteLine($"[{++menuIndex}] Change source path");
                Console.WriteLine($"[{++menuIndex}] Change documentation path");
                Console.WriteLine($"[{++menuIndex}] Change force flag");
                Console.WriteLine($"[{++menuIndex}] Start documentation generation...");
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
                    if (select == 2)
                    {
                        Console.Write("Select or documentation path: ");
                        var selectPath = Console.ReadLine();
                        
                        if (string.IsNullOrEmpty(selectPath) == false)
                        {
                            DocuPath = selectPath;
                        }
                    }
                    if (select == 3)
                    {
                        force = !force;
                    }
                    if (select == 4)
                    {
                        Program.PrintBusyProgress();
                        ExecuteDocumentation(solutionProperties, force);
                    }
                    Thread.Sleep(500);
                    Program.RunBusyProgress = false;
                }
            }
        }
        #endregion Console methods
        /// <summary>
        /// Executes the documentation process for the specified SolutionProperties and force flag.
        /// </summary>
        /// <param name="solutionProperties">The SolutionProperties object containing information about the solution.</param>
        /// <param name="force">A boolean indicating whether to force the documentation process.</param>
        /// <returns>void</returns>
        private static void ExecuteDocumentation(SolutionProperties solutionProperties, bool force)
        {
            var chatGPTClient = new ChatGPTClient();
            var searchPatterns = new[] { "*.cs" };
            var thisNamespace = $"{nameof(TemplateTooles)}.{nameof(TemplateTooles.ConApp)}";
            var files = searchPatterns.SelectMany(searchPattern => Directory.GetFiles(solutionProperties.SolutionPath, searchPattern, SearchOption.AllDirectories))
                                      .Where(f => CommonStaticLiterals.IgnoreFolderFiles.Any(i => f.Contains(i, StringComparison.CurrentCultureIgnoreCase)) == false)
                                      .Where(f => f.Contains(thisNamespace, StringComparison.CurrentCultureIgnoreCase) == false)
                                      .ToArray();
            if (Directory.Exists(DocuPath) == false)
            {
                Directory.CreateDirectory(DocuPath);
            }
            
            foreach (var file in files)
            {
                var sourceText = File.ReadAllText(file);
                var docuSourceText = force ? RemoveXmlComments(sourceText) : sourceText;
                var syntaxSource = QuerySyntaxSource(docuSourceText);
                var syntaxTree = CSharpSyntaxTree.ParseText(syntaxSource);
                var syntaxRoot = syntaxTree.GetRoot();
                var docuFilePath = file.Replace(solutionProperties.SolutionPath, DocuPath);
                var docuPath = Path.GetDirectoryName(docuFilePath);
                var sourceNodes = syntaxRoot.DescendantNodes().OfType<ClassDeclarationSyntax>().Select(n => new SourceNode(n))
                                            .Union(syntaxRoot.DescendantNodes().OfType<InterfaceDeclarationSyntax>().Select(n => new SourceNode(n)));
                
                foreach (var sourceNode in sourceNodes)
                {
                    var memberNodes = sourceNode.Properties
                                                .Union(sourceNode.Constructors)
                                                .Union(sourceNode.Methods);
                    
                    if (sourceNode.Documentation.IsNullOrEmpty())
                    {
                        try
                        {
                            var documentation = GetChatGPTDocumentation(chatGPTClient, sourceNode);
                            
                            docuSourceText = docuSourceText.Replace(sourceNode.ToString(), $"{documentation}{sourceNode}");
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error in {MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
                        }
                    }
                    
                    foreach (var memberNode in memberNodes.Where(m => m.Documentation.IsNullOrEmpty()))
                    {
                        try
                        {
                            var documentation = GetChatGPTDocumentation(chatGPTClient, memberNode);
                            
                            docuSourceText = docuSourceText.Replace(memberNode.ToString(), $"{documentation}{memberNode}");
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error in {MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
                        }
                    }
                }
                
                if (sourceText != docuSourceText)
                {
                    if (Directory.Exists(docuPath) == false)
                    {
                        Directory.CreateDirectory(docuPath!);
                    }
                    File.WriteAllText(docuFilePath, docuSourceText.FormatCSharpCode());
                }
            }
        }
        /// <summary>
        /// Retrieves the XML documentation for a C# method.
        /// </summary>
        /// <param name="chatGPTClient">The ChatGPTClient instance used for communicating with the ChatGPT API.</param>
        /// <param name="sourceNode">The SyntaxNode representing the method for which the XML documentation is requested.</param>
        /// <returns>The XML documentation for the specified C# method.</returns>
        private static string GetChatGPTDocumentation(ChatGPTClient chatGPTClient, SourceNode sourceNode)
        {
            string? result;
            
            try
            {
                var chatGPTRequest = string.Empty;
                
                if (sourceNode.Kind == SyntaxKind.ClassDeclaration)
                {
                    chatGPTRequest = $"Please create a xml documentation for the following c# class:{Environment.NewLine} {sourceNode} {Environment.NewLine}Please write only the xml documentation for class!";
                }
                else if (sourceNode.Kind == SyntaxKind.InterfaceDeclaration)
                {
                    chatGPTRequest = $"Please create a xml documentation for the following c# interface:{Environment.NewLine} {sourceNode} {Environment.NewLine}Please write only the xml documentation for interface!";
                }
                else if (sourceNode.Kind == SyntaxKind.PropertyDeclaration)
                {
                    chatGPTRequest = $"Please create a xml documentation for the following c# property:{Environment.NewLine} {sourceNode} {Environment.NewLine}Please write only the xml documentation for property!";
                }
                else if (sourceNode.Kind == SyntaxKind.MethodDeclaration)
                {
                    chatGPTRequest = $"Please create a xml documentation for the following c# method:{Environment.NewLine} {sourceNode} {Environment.NewLine}Please write only the xml documentation for method!";
                }
                else if (sourceNode.Kind == SyntaxKind.EnumDeclaration)
                {
                    chatGPTRequest = $"Please create a xml documentation for the following c# enum:{Environment.NewLine} {sourceNode} {Environment.NewLine}Please write only the xml documentation for enum!";
                }
                else
                {
                    chatGPTRequest = $"Please create a xml documentation for the following c# code:{Environment.NewLine} {sourceNode} {Environment.NewLine}Please write only the xml documentation!";
                }
                var chatGPTResponse = chatGPTClient.SendMessage(chatGPTRequest);
                
                result = FormatChatGPTDocumentation(chatGPTResponse.ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in {MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
                throw;
            }
            return result;
        }
        /// <summary>
        /// Removes any lines in a string that start with "#if", "#else", or "#endif".
        /// </summary>
        /// <param name="source">The string containing the source code.</param>
        /// <returns>A modified string with the specified lines removed.</returns>
        private static string QuerySyntaxSource(string source)
        {
            var lines = source.ToLines();
            var result = new List<string>();
            
            foreach (var line in lines)
            {
                var formatLine = line.Trim();
                
                if (formatLine.StartsWith("#if") == false
                && formatLine.StartsWith("#else") == false
                && formatLine.StartsWith("#endif") == false)
                {
                    result.Add(line);
                }
            }
            return result.ToText();
        }
        /// <summary>
        /// Formats the given source string for ChatGPT documentation.
        /// </summary>
        /// <param name="source">The source string to be formatted.</param>
        /// <returns>The formatted string.</returns>
        private static string FormatChatGPTDocumentation(string source)
        {
            bool ignoreLine;
            var summaryCount = 0;
            var exampleCount = 0;
            var result = new List<string>();
            var lines = source.Replace("\n", Environment.NewLine)
                              .ToLines()
                              .Select(l => l.Trim());
            
            foreach (var line in lines.Select(l => l.Trim()))
            {
                if (line.Contains("<summary>"))
                {
                    summaryCount++;
                }
                else if (line.Contains("<example>"))
                {
                    exampleCount++;
                }
                
                ignoreLine = (summaryCount > 2) || (exampleCount % 2 > 0);
                ignoreLine = ignoreLine || line.Equals("```xml") || line.Equals("```csharp") || line.Equals("```");
                ignoreLine = ignoreLine || line.StartsWith("/**") || line.StartsWith("*/");
                ignoreLine = ignoreLine || line.StartsWith("// Method implementation");
                
                if (ignoreLine)
                {
                    ; // Ignore line
                }
                else if (line.StartsWith("*"))
                {
                    result.Add($"/// {line[2..]}");
                }
                else if (line.IsCommentLine())
                {
                    result.Add(line);
                }
                
                if (line.Contains("</summary>"))
                {
                    summaryCount++;
                }
                else if (line.Contains("</example>"))
                {
                    exampleCount++;
                }
            }
            return result.ToText();
        }
        /// <summary>
        /// Removes XML comments from the given source code.
        /// </summary>
        /// <param name="source">The source code containing XML comments.</param>
        /// <returns>The source code with XML comments removed.</returns>
        private static string RemoveXmlComments(string source)
        {
            var result = new List<string>();
            var lines = source.ToLines();
            
            foreach (var line in lines)
            {
                if (line.IsXmlLineComment() == false)
                {
                    result.Add(line);
                }
            }
            return result.ToText();
        }
        
        /// <summary>
        /// Returns the name of the given member declaration.
        /// </summary>
        /// <param name="member">The member declaration syntax.</param>
        /// <returns>The name of the member.</returns>
        private static string GetMemberName(MemberDeclarationSyntax member)
        {
            string? result;

            // Check the specific type of the member and extract the name accordingly
            if (member is ClassDeclarationSyntax classDeclaration)
            {
                result = classDeclaration.Identifier.ValueText;
            }
            else if (member is FieldDeclarationSyntax fieldDeclaration)
            {
                // For fields, there can be multiple variables declared, so we take the name of the first one
                result = fieldDeclaration.Declaration.Variables.First().Identifier.ValueText;
            }
            else if (member is PropertyDeclarationSyntax propertyDeclaration)
            {
                result = propertyDeclaration.Identifier.ValueText;
            }
            else if (member is MethodDeclarationSyntax methodDeclaration)
            {
                result = methodDeclaration.Identifier.ValueText;
            }
            else if (member is ConstructorDeclarationSyntax constructorDeclaration)
            {
                result = constructorDeclaration.Identifier.ValueText;
            }
            else if (member is OperatorDeclarationSyntax operatorDeclaration)
            {
                result = operatorDeclaration.OperatorToken.ToString();
            }
            else if (member is IndexerDeclarationSyntax indexDeclaration)
            {
                result = indexDeclaration.GetType().Name.Replace("Syntax", string.Empty);
            }
            else if (member is EnumDeclarationSyntax enumDeclaration)
            {
                result = enumDeclaration.Identifier.ValueText;
            }
            else if (member is DelegateDeclarationSyntax delegateDeclaration)
            {
                result = delegateDeclaration.Identifier.ValueText;
            }
            else if (member is EventFieldDeclarationSyntax eventFieldDeclaration)
            {
                result = eventFieldDeclaration.GetType().Name.Replace("Syntax", string.Empty);
            }
            // Add more cases for other types of members as needed
            else
            {
                result = member.GetType().Name.Replace("Syntax", string.Empty);
            }
            return result;
        }
        /// <summary>
        /// Returns the type of the member declaration.
        /// </summary>
        /// <param name="member">The member declaration syntax.</param>
        /// <returns>The type of the member declaration as a string.</returns>
        private static string GetMemberType(MemberDeclarationSyntax member)
        {
            return member.Kind().ToString();
        }
    }
}
//MdEnd


