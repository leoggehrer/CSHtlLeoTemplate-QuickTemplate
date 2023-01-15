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
            ISolutionProperties solutionProperties = Generation.SolutionProperties.Create(solutionPath);

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
        public static void DeleteGeneratedFiles(string path)
        {
            Console.WriteLine("Delete all generation files...");

            foreach (var searchPattern in StaticLiterals.SourceFileExtensions.Split("|"))
            {
                var deleteFiles = GetGeneratedFiles(path, searchPattern, new[] { StaticLiterals.GeneratedCodeLabel });

                foreach (var file in deleteFiles)
                {
                    if (Generation.FileHandler.IsTypeScriptFile(file))
                    {
                        Generation.FileHandler.SaveAngularCustomParts(file);
                    }
                    File.Delete(file);
                }
            }
            var defines = Preprocessor.ProjectFile.ReadDefinesInProjectFiles(path);

            Preprocessor.ProjectFile.SwitchDefine(defines, Preprocessor.ProjectFile.GeneratedCodePrefix, Preprocessor.ProjectFile.OffPostfix);
            Preprocessor.ProjectFile.WriteDefinesInProjectFiles(path, defines);
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
