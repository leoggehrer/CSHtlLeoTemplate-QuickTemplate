//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic
{
    using System.Text;
    using TemplateCodeGenerator.Logic.Common;
    using TemplateCodeGenerator.Logic.Contracts;
    public static partial class Writer
    {
        #region Class-Constructors
        static Writer()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        #endregion Class-Constructors

        public static bool WriteToGroupFile { get; set; } = true;
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
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.MVVM && e.ItemType == ItemType.AccessModel);

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
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.ClientBlazor && e.ItemType == ItemType.AccessModel);

                    Console.WriteLine("Write Client-Blazor-Models...");
                    WriteItems(projectPath, writeItems, false);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.ClientBlazorProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.ClientBlazor && e.ItemType == ItemType.ServiceModel);

                    Console.WriteLine("Write Client-Blazor-ServiceModels...");
                    WriteItems(projectPath, writeItems, false);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.ClientBlazorProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.ClientBlazor && e.ItemType == ItemType.ServiceContract);

                    Console.WriteLine("Write Client-Blazor-ServiceContracts...");
                    WriteItems(projectPath, writeItems, false);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.ClientBlazorProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.ClientBlazor && e.ItemType == ItemType.ServiceAccessContract);

                    Console.WriteLine("Write Client-Blazor-AccessContracts...");
                    WriteItems(projectPath, writeItems, false);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.ClientBlazorProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.ClientBlazor && e.ItemType == ItemType.Service);

                    Console.WriteLine("Write Client-Blazor-Services...");
                    WriteItems(projectPath, writeItems, false);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.ClientBlazorProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.ClientBlazor && e.ItemType == ItemType.AddServices);

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
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.Angular && (e.ItemType == ItemType.TypeScriptEnum));

                    Console.WriteLine("Write Angular-Enums...");
                    WriteItems(projectPath, writeItems, false);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.AngularAppProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.Angular && (e.ItemType == ItemType.TypeScriptModel));

                    Console.WriteLine("Write Angular-Models...");
                    WriteItems(projectPath, writeItems, false);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                var projectPath = Path.Combine(solutionPath, solutionProperties.AngularAppProjectName);
                if (Directory.Exists(projectPath))
                {
                    var writeItems = generatedItems.Where(e => e.UnitType == UnitType.Angular && (e.ItemType == ItemType.TypeScriptService));

                    Console.WriteLine("Write Angular-Services...");
                    WriteItems(projectPath, writeItems, false);
                }
            }));
            #endregion WriteAngularComponents

            Task.WaitAll(tasks.ToArray());

            var defines = Preprocessor.ProjectFile.ReadDefinesInProjectFiles(solutionPath);

            Preprocessor.ProjectFile.SwitchDefine(defines, Preprocessor.ProjectFile.GeneratedCodePrefix, Preprocessor.ProjectFile.OnPostfix);
            Preprocessor.ProjectFile.WriteDefinesInProjectFiles(solutionPath, defines);
            Preprocessor.RazorFile.SetPreprocessorDefineCommentsInRazorFiles(solutionPath, defines);
        }

        #region Write methods
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
