﻿//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic
{
    using System.Text;
    using TemplateCodeGenerator.Logic.Common;
    using TemplateCodeGenerator.Logic.Contracts;
    public static partial class Writer
    {
        #region Class-Constructors
        /// <summary>
        /// Represents the static writer class.
        /// </summary>
        static Writer()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called before the class is constructed.
        /// </summary>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when the class is constructed.
        /// </summary>
        static partial void ClassConstructed();
        #endregion Class-Constructors
        
        /// <summary>
        /// Gets or sets a value indicating whether the data should be written to the group file.
        /// </summary>
        /// <value>
        /// True if the data should be written to the group file; otherwise, false.
        /// </value>
        public static bool WriteToGroupFile { get; set; } = true;
        /// <summary>
        /// Writes various components to the specified solution path based on the solution properties and generated items.
        /// </summary>
        /// <param name="solutionPath">The path of the solution.</param>
        /// <param name="solutionProperties">The solution properties.</param>
        /// <param name="generatedItems">The collection of generated items.</param>
        public static void WriteAll(string solutionPath, ISolutionProperties solutionProperties, IEnumerable<IGeneratedItem> generatedItems)
        {
            var tasks = new List<Task>();
            
            #region WriteLogicComponents
            tasks.Add(Task.Factory.StartNew((Action)(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.LogicProjectName);
                var projectName = solutionProperties.GetProjectNameFromPath(projectPath);
                
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where<IGeneratedItem>((Func<IGeneratedItem, bool>)(e => e.UnitType == UnitType.Logic && e.ItemType == ItemType.DbContext));
                    
                    Console.WriteLine("Write Logic-DataContext...");
                    WriteItems(projectPath, writeItems, WriteToGroupFile);
                }
            })));
            tasks.Add(Task.Factory.StartNew((Action)(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.LogicProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where<IGeneratedItem>((Func<IGeneratedItem, bool>)(e => e.UnitType == UnitType.Logic && e.ItemType == ItemType.AccessModel));
                    
                    Console.WriteLine("Write Logic-Models...");
                    WriteItems(projectPath, writeItems, WriteToGroupFile);
                }
            })));
            tasks.Add(Task.Factory.StartNew((Action)(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.LogicProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where<IGeneratedItem>((Func<IGeneratedItem, bool>)(e => e.UnitType == UnitType.Logic && e.ItemType == ItemType.ModelContract));
                    
                    Console.WriteLine("Write Logic-Models-Contracts...");
                    WriteItems(projectPath, writeItems, WriteToGroupFile);
                }
            })));
            tasks.Add(Task.Factory.StartNew((Action)(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.LogicProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where<IGeneratedItem>((Func<IGeneratedItem, bool>)(e => e.UnitType == UnitType.Logic && (e.ItemType == ItemType.AccessContract || e.ItemType == ItemType.ServiceContract)));
                    
                    Console.WriteLine("Write Logic-Contracts...");
                    WriteItems(projectPath, writeItems, WriteToGroupFile);
                }
            })));
            tasks.Add(Task.Factory.StartNew((Action)(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.LogicProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where<IGeneratedItem>((Func<IGeneratedItem, bool>)(e => e.UnitType == UnitType.Logic && e.ItemType == ItemType.Controller));
                    
                    Console.WriteLine("Write Logic-Controllers...");
                    WriteItems(projectPath, writeItems, WriteToGroupFile);
                }
            })));
            tasks.Add(Task.Factory.StartNew((Action)(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.LogicProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where<IGeneratedItem>((Func<IGeneratedItem, bool>)(e => e.UnitType == UnitType.Logic && e.ItemType == ItemType.Service));
                    
                    Console.WriteLine("Write Logic-Services...");
                    WriteItems(projectPath, writeItems, WriteToGroupFile);
                }
            })));
            tasks.Add(Task.Factory.StartNew((Action)(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.LogicProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where<IGeneratedItem>((Func<IGeneratedItem, bool>)(e => e.UnitType == UnitType.Logic && e.ItemType == ItemType.Facade));
                    
                    Console.WriteLine("Write Logic-Facades...");
                    WriteItems(projectPath, writeItems, WriteToGroupFile);
                }
            })));
            tasks.Add(Task.Factory.StartNew((Action)(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.LogicProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where<IGeneratedItem>((Func<IGeneratedItem, bool>)(e => e.UnitType == UnitType.Logic && e.ItemType == ItemType.Factory));
                    
                    Console.WriteLine("Write Logic-Factory...");
                    WriteItems(projectPath, writeItems, WriteToGroupFile);
                }
            })));
            #endregion WriteLogicComponents
            
            #region WriteWebApiComponents
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.WebApiProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.WebApi && (e.ItemType == ItemType.AccessModel || e.ItemType == ItemType.EditModel));
                    
                    Console.WriteLine("Write WebApi-Models...");
                    WriteItems(projectPath, writeItems, WriteToGroupFile);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.WebApiProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.WebApi && e.ItemType == ItemType.Controller);
                    
                    Console.WriteLine("Write WebApi-Controllers...");
                    WriteItems(projectPath, writeItems, WriteToGroupFile);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.WebApiProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.WebApi && e.ItemType == ItemType.AddServices);
                    
                    Console.WriteLine("Write WebApi-AddServices...");
                    WriteItems(projectPath, writeItems, WriteToGroupFile);
                }
            }));
            #endregion WriteWebApiComponents
            
            #region WriteAspMvcComponents
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.AspMvcAppProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.AspMvc && (e.ItemType == ItemType.AccessModel || e.ItemType == ItemType.AccessFilterModel));
                    
                    Console.WriteLine("Write AspMvc-AccessModels and FilterModels...");
                    WriteItems(projectPath, writeItems, WriteToGroupFile);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.AspMvcAppProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.AspMvc && (e.ItemType == ItemType.ServiceModel || e.ItemType == ItemType.ServiceFilterModel));
                    
                    Console.WriteLine("Write AspMvc-SercviceModels...");
                    WriteItems(projectPath, writeItems, WriteToGroupFile);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.AspMvcAppProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.AspMvc && e.ItemType == ItemType.Controller);
                    
                    Console.WriteLine("Write AspMvc-Controllers...");
                    WriteItems(projectPath, writeItems, WriteToGroupFile);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.AspMvcAppProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.AspMvc && e.ItemType == ItemType.AddServices);
                    
                    Console.WriteLine("Write AspMvc-AddServices...");
                    WriteItems(projectPath, writeItems, WriteToGroupFile);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.AspMvcAppProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.AspMvc && e.ItemType == ItemType.View);
                    
                    Console.WriteLine("Write AspMvc-Views...");
                    WriteItems(projectPath, writeItems, false);
                }
            }));
            #endregion WriteAspMvcModels
            
            #region WriteMVVMComponents
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.MVVMAppProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.MVVMApp && e.ItemType == ItemType.AccessModel);
                    
                    Console.WriteLine("Write MVVM-Models...");
                    WriteItems(projectPath, writeItems, false);
                }
            }));
            #endregion WriteMVVMComponents
            
            #region WriteClientBlazorComponents
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.ClientBlazorProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.ClientBlazorApp && e.ItemType == ItemType.AccessModel);
                    
                    Console.WriteLine("Write Client-Blazor-Models...");
                    WriteItems(projectPath, writeItems, false);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.ClientBlazorProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.ClientBlazorApp && e.ItemType == ItemType.ServiceModel);
                    
                    Console.WriteLine("Write Client-Blazor-ServiceModels...");
                    WriteItems(projectPath, writeItems, false);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.ClientBlazorProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.ClientBlazorApp && e.ItemType == ItemType.ServiceContract);
                    
                    Console.WriteLine("Write Client-Blazor-ServiceContracts...");
                    WriteItems(projectPath, writeItems, false);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.ClientBlazorProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.ClientBlazorApp && e.ItemType == ItemType.ServiceAccessContract);
                    
                    Console.WriteLine("Write Client-Blazor-AccessContracts...");
                    WriteItems(projectPath, writeItems, false);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.ClientBlazorProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.ClientBlazorApp && e.ItemType == ItemType.Service);
                    
                    Console.WriteLine("Write Client-Blazor-Services...");
                    WriteItems(projectPath, writeItems, false);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.ClientBlazorProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.ClientBlazorApp && e.ItemType == ItemType.AddServices);
                    
                    Console.WriteLine("Write ClientBlazor-AddServices...");
                    WriteItems(projectPath, writeItems, WriteToGroupFile);
                }
            }));
            #endregion WriteClientBlazorComponents
            
            #region WriteAngularComponents
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.AngularAppProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.AngularApp && (e.ItemType == ItemType.TypeScriptEnum));
                    
                    Console.WriteLine("Write Angular-Enums...");
                    WriteItems(projectPath, writeItems, false);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.AngularAppProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.AngularApp && (e.ItemType == ItemType.TypeScriptModel));
                    
                    Console.WriteLine("Write Angular-Models...");
                    WriteItems(projectPath, writeItems, false);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.AngularAppProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.AngularApp && (e.ItemType == ItemType.TypeScriptService));
                    
                    Console.WriteLine("Write Angular-Services...");
                    WriteItems(projectPath, writeItems, false);
                }
            }));
            #endregion WriteAngularComponents
            
            Task.WaitAll(tasks.ToArray());
            
            var defines = Preprocessor.ProjectFile.ReadDefinesInProjectFiles(solutionPath);
            
            Preprocessor.ProjectFile.SwitchDefine(defines, Preprocessor.ProjectFile.GeneratedCodePrefix, Preprocessor.ProjectFile.OnPostfix);
            Preprocessor.ProjectFile.WriteDefinesInProjectFiles(solutionPath, defines);
            Preprocessor.PreprocessorCommentHelper.SetPreprocessorDefineCommentsInFiles(solutionPath, defines);
        }
        
        #region Write methods
        /// <summary>
        /// Writes the generated items to either a group file or individual code files.
        /// </summary>
        /// <param name="projectPath">The path of the project where the generated items will be written to.</param>
        /// <param name="generatedItems">The collection of generated items to be written.</param>
        /// <param name="writeToGroupFile">A boolean value indicating whether the generated items should be written to a group file or not.</param>
        internal static void WriteItems(string projectPath, IEnumerable<IGeneratedItem> generatedItems, bool writeToGroupFile)
        {
            if (writeToGroupFile)
            {
                WriteGeneratedCodeFile(projectPath, StaticLiterals.GeneratedCodeFileName, generatedItems);
            }
            else
            {
                WriteCodeFiles(projectPath, generatedItems);
            }
        }
        /// <summary>
        /// Writes the generated code file.
        /// </summary>
        /// <param name="projectPath">The project path.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="generatedItems">The collection of generated items.</param>
        /// <remarks>
        /// This method checks if the generatedItems collection is not empty. It then determines the minimum common subpath from the generated items' subfile paths and combines it with the project path and given file name to create the full file path. Finally, it calls the overloaded WriteGeneratedCodeFile method with the full file path and the generated items.
        /// </remarks>
        internal static void WriteGeneratedCodeFile(string projectPath, string fileName, IEnumerable<IGeneratedItem> generatedItems)
        {
            if (generatedItems.Any())
            {
                var subPaths = generatedItems.Select(e => Path.GetDirectoryName(e.SubFilePath));
                var subPathItems = subPaths.Select(e => e!.Split(Path.DirectorySeparatorChar));
                var intersect = subPathItems.Any() ? subPathItems.ElementAt(0) : Array.Empty<string>().Select(e => e);
                
                for (int i = 1; i < subPathItems.Count(); i++)
                {
                    intersect = intersect.Intersect(subPathItems.ElementAt(i));
                }
                
                var minSubPath = string.Join(Path.DirectorySeparatorChar, intersect);
                var fullFilePath = Path.Combine(projectPath, minSubPath, fileName);
                
                WriteGeneratedCodeFile(fullFilePath, generatedItems);
            }
        }
        /// <summary>
        /// Writes generated code to a file at the specified file path.
        /// </summary>
        /// <param name="fullFilePath">The full file path where the generated code file should be written.</param>
        /// <param name="generatedItems">An IEnumerable of IGeneratedItem containing the generated items to be written.</param>
        /// <remarks>
        /// This method iterates through each item in the generatedItems collection, retrieves the source code of each item,
        /// and writes it to the file specified by the fullFilePath parameter. If the lines of source code exist, the directory
        /// of the fullFilePath is checked, and if it does not exist, it is created. The generated code is then written to the file.
        /// </remarks>
        internal static void WriteGeneratedCodeFile(string fullFilePath, IEnumerable<IGeneratedItem> generatedItems)
        {
            var lines = new List<string>();
            var directory = Path.GetDirectoryName(fullFilePath);
            
            foreach (var item in generatedItems)
            {
                lines.AddRange(item.SourceCode);
            }
            
            if (lines.Any())
            {
                var sourceLines = new List<string>(lines);
                
                if (string.IsNullOrEmpty(directory) == false && Directory.Exists(directory) == false)
                {
                    Directory.CreateDirectory(directory);
                }
                
                sourceLines.Insert(0, $"//{StaticLiterals.GeneratedCodeLabel}");
                File.WriteAllLines(fullFilePath, sourceLines);
            }
        }
        
        /// <summary>
        /// Writes code files to the specified project path.
        /// </summary>
        /// <param name="projectPath">The path of the project where the code files are to be written.</param>
        /// <param name="generatedItems">The collection of generated items representing the code files.</param>
        /// <returns>Returns nothing.</returns>
        internal static void WriteCodeFiles(string projectPath, IEnumerable<IGeneratedItem> generatedItems)
        {
            foreach (var item in generatedItems)
            {
                var sourceLines = new List<string>(item.SourceCode);
                var filePath = Path.Combine(projectPath, item.SubFilePath);
                
                if (item.FileExtension == StaticLiterals.CSharpHtmlFileExtension)
                {
                    sourceLines.Insert(0, $"@*{StaticLiterals.GeneratedCodeLabel}*@");
                }
                else
                {
                    sourceLines.Insert(0, $"//{StaticLiterals.GeneratedCodeLabel}");
                }
                WriteCodeFile(filePath, sourceLines);
            }
        }
        /// <summary>
        /// Writes the specified source code to a file.
        /// </summary>
        /// <param name="sourceFilePath">The path of the file to be written.</param>
        /// <param name="source">The collection of strings representing the source code to be written.</param>
        internal static void WriteCodeFile(string sourceFilePath, IEnumerable<string> source)
        {
            var canCreate = true;
            var sourcePath = Path.GetDirectoryName(sourceFilePath);
            var customFilePath = Generation.FileHandler.CreateCustomFilePath(sourceFilePath);
            var generatedCode = StaticLiterals.GeneratedCodeLabel;
            
            if (File.Exists(sourceFilePath))
            {
                var lines = File.ReadAllLines(sourceFilePath);
                var header = lines.FirstOrDefault(l => l.Contains(StaticLiterals.GeneratedCodeLabel)
                || l.Contains(StaticLiterals.CustomizedAndGeneratedCodeLabel));
                
                if (header != null)
                {
                    File.Delete(sourceFilePath);
                }
                else
                {
                    canCreate = false;
                }
            }
            else if (string.IsNullOrEmpty(sourcePath) == false
            && Directory.Exists(sourcePath) == false)
            {
                Directory.CreateDirectory(sourcePath);
            }
            
            if (canCreate && source.Any())
            {
                File.WriteAllLines(sourceFilePath, source, Encoding.UTF8);
            }
            
            if (File.Exists(customFilePath))
            {
                File.Delete(customFilePath);
            }
        }
        #endregion Write methods
    }
}
//MdEnd
