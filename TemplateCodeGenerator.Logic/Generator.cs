//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic
{
    using System.Collections.Concurrent;
    using System.Text;
    using TemplateCodeGenerator.Logic.Contracts;

    public static partial class Generator
    {
        public static IEnumerable<IGeneratedItem> Generate(string solutionPath)
        {
            ISolutionProperties solutionProperties = SolutionProperties.Create(solutionPath);

            return Generate(solutionProperties);
        }
        public static IEnumerable<IGeneratedItem> Generate(ISolutionProperties solutionProperties)
        {
            var result = new ConcurrentBag<IGeneratedItem>();
            var configuration = new Generation.Configuration(solutionProperties);
            var tasks = new List<Task>();

            #region Logic
            if (configuration.QuerySettingValue<bool>(Common.UnitType.Logic.ToString(), "All", "All", "Generate", "True"))
            {
                var generator = new Generation.LogicGenerator(solutionProperties);

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    var generatedItems = new List<IGeneratedItem>();

                    Console.WriteLine("Create Logic-Components...");
                    generatedItems.AddRange(generator.GenerateAll());
                    result.AddRangeSafe(generatedItems);
                }));
            }
            #endregion Logic

            #region WebApiApp
            if (configuration.QuerySettingValue<bool>(Common.UnitType.WebApi.ToString(), "All", "All", "Generate", "True"))
            {
                var generator = new Generation.WebApiGenerator(solutionProperties);

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    var generatedItems = new List<IGeneratedItem>();

                    Console.WriteLine("Create WebApi-Components...");
                    generatedItems.AddRange(generator.GenerateAll());
                    result.AddRangeSafe(generatedItems);
                }));
            }
            #endregion WebApiApp

            #region MVVMApp
            if (configuration.QuerySettingValue<bool>(Common.UnitType.MVVM.ToString(), "All", "All", "Generate", "True"))
            {
                var generator = new Generation.MVVMGenerator(solutionProperties);

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    var generatedItems = new List<IGeneratedItem>();

                    Console.WriteLine("Create MVVM-Components...");
                    generatedItems.AddRange(generator.GenerateAll());
                    result.AddRangeSafe(generatedItems);
                }));
            }
            #endregion MVVMApp

            #region AspMvcApp
            if (configuration.QuerySettingValue<bool>(Common.UnitType.AspMvc.ToString(), "All", "All", "Generate", "True"))
            {
                var generator = new Generation.AspMvcGenerator(solutionProperties);

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    var generatedItems = new List<IGeneratedItem>();

                    Console.WriteLine("Create AspMvc-Components...");
                    generatedItems.AddRange(generator.GenerateAll());
                    result.AddRangeSafe(generatedItems);
                }));
            }
            #endregion AspMvcApp

            #region ClientBlazorApp
            if (configuration.QuerySettingValue<bool>(Common.UnitType.ClientBlazor.ToString(), "All", "All", "Generate", "True"))
            {
                var generator = new Generation.ClientBlazorGenerator(solutionProperties);

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    var generatedItems = new List<IGeneratedItem>();

                    Console.WriteLine("Create Client-Blazor-Components...");
                    generatedItems.AddRange(generator.GenerateAll());
                    result.AddRangeSafe(generatedItems);
                }));
            }
            #endregion ClientBlazorApp

            #region AngularApp
            if (configuration.QuerySettingValue<bool>(Common.UnitType.Angular.ToString(), "All", "All", "Generate", "True"))
            {
                var generator = new Generation.AngularGenerator(solutionProperties);

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    var generatedItems = new List<IGeneratedItem>();

                    Console.WriteLine("Create AspMvc-Components...");
                    generatedItems.AddRange(generator.GenerateAll());
                    result.AddRangeSafe(generatedItems);
                }));
            }
            #endregion AngularApp

            Task.WaitAll(tasks.ToArray());
            return result;
        }

        public static void DeleteGeneratedFiles(string sourcePath)
        {
            var solutionProperties = SolutionProperties.Create(sourcePath);
            var configuration = new Generation.Configuration(solutionProperties);

            Console.WriteLine("Delete all generated files...");

            foreach (var searchPattern in StaticLiterals.SourceFileExtensions.Split("|"))
            {
                var deleteFiles = GetGeneratedFiles(sourcePath, searchPattern, new[] { StaticLiterals.GeneratedCodeLabel });

                foreach (var file in deleteFiles)
                {
                    var isTemplateProjectFile = solutionProperties.IsTemplateProjectFile(file);

                    if (isTemplateProjectFile)
                    {
                        var projectName = solutionProperties.GetProjectNameFromFile(file); ;
                        var defaultValue = configuration.QuerySettingValue(projectName, "All", "All", "Delete", true.ToString());
                        var canDelete = configuration.QuerySettingValue<bool>(projectName, "File", Path.GetFileName(file), "Delete", defaultValue);

                        if (canDelete)
                        {
                            if (Generation.FileHandler.IsTypeScriptFile(file))
                            {
                                Generation.FileHandler.SaveAngularCustomParts(file);
                            }

                            File.Delete(file);
                        }
                    }
                }
            }
            var defines = Preprocessor.ProjectFile.ReadDefinesInProjectFiles(sourcePath);

            Preprocessor.ProjectFile.SwitchDefine(defines, Preprocessor.ProjectFile.GeneratedCodePrefix, Preprocessor.ProjectFile.OffPostfix);
            Preprocessor.ProjectFile.WriteDefinesInProjectFiles(sourcePath, defines);
            Preprocessor.RazorFile.SetPreprocessorDefineCommentsInRazorFiles(sourcePath, defines);
        }
        private static IEnumerable<string> GetGeneratedFiles(string path, string searchPattern, string[] labels)
        {
            var result = new List<string>();

            foreach (var file in Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories)
                                          .Where(f => CommonBase.StaticLiterals.GenerationIgnoreFolders.Any(e => f.Contains(e) == false)))
            {
                var lines = File.ReadAllLines(file, Encoding.Default);

                if (lines.Length == 0 || labels.Any(l => lines.First().Contains(l)))
                {
                    result.Add(file);
                }
            }
            return result;
        }
    }
}
//MdEnd
