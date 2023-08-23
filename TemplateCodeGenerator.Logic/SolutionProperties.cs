//@CodeCopy
//MdStart

namespace TemplateCodeGenerator.Logic
{
    public sealed partial class SolutionProperties : Contracts.ISolutionProperties
    {
        #region Project-postfixes
        public string LogicExtension => StaticLiterals.LogicExtension;
        public string WebApiExtension => StaticLiterals.WebApiExtension;
        public string AspMvcExtension => StaticLiterals.AspMvcExtension;
        public string MVVMExtension => StaticLiterals.MVVMExtension;
        public string AngularExtension => StaticLiterals.AngularExtension;
        public string ClientBlazorExtension => StaticLiterals.ClientBlazorExtension;
        #endregion Project-postfixes

        public string SolutionPath { get; }
        public string SolutionName { get; }
        public string SolutionFilePath { get; }
        public string? CompilePath { get; set; }
        public Type[]? LogicAssemblyTypes { get; set; }
        public string? CompileLogicAssemblyFilePath
        {
            get
            {
                var result = string.Empty;

                if (CompilePath.HasContent())
                {
                    result = GetCompileAssemblyFilePath(SolutionName, CompilePath!);
                }
                return result;
            }
        }

        #region ProjectNames
        public IEnumerable<string> TemplateProjectNames => CommonBase.StaticLiterals.TemplateProjectExtensions.Select(e => $"{SolutionName}{e}");
        public IEnumerable<string> AllTemplateProjectNames
        {
            get
            {
                var result = new List<string>(CommonBase.StaticLiterals.TemplateProjects);

                foreach (var extension in CommonBase.StaticLiterals.TemplateProjectExtensions)
                {
                    result.Add($"{SolutionName}{extension}");
                }
                result.AddRange(CommonBase.StaticLiterals.TemplateToolProjects);
                return result.ToArray();
            }
        }
        public IEnumerable<string> TemplateProjectPaths => TemplateProjectNames.Select(tpn => Path.Combine(SolutionPath, tpn));

        public string LogicAssemblyFilePath { get; }
        public string LogicCSProjectFilePath { get; }

        public string LogicProjectName => TemplateProjectNames.First(e => e.EndsWith($"{LogicExtension}"));
        public string LogicSubPath => LogicProjectName;
        public string LogicControllersSubPath => StaticLiterals.ControllersFolder;
        public string LogicEntitiesSubPath => StaticLiterals.EntitiesFolder;
        public string LogicDataContextSubPath => StaticLiterals.DataContextFolder;

        public string WebApiProjectName => TemplateProjectNames.First(e => e.EndsWith($"{WebApiExtension}"));
        public string WebApiSubPath => WebApiProjectName;
        public string WebApiControllersSubPath => Path.Combine(WebApiSubPath, StaticLiterals.ControllersFolder);

        public string AspMvcAppProjectName => TemplateProjectNames.First(e => e.EndsWith($"{AspMvcExtension}"));
        public string AspMvcAppSubPath => AspMvcAppProjectName;
        public string AspMvcControllersSubPath => Path.Combine(AspMvcAppSubPath, StaticLiterals.ControllersFolder);

        public string MVVMAppProjectName => TemplateProjectNames.First(e => e.EndsWith($"{MVVMExtension}"));
        public string MVVMAppSubPath => MVVMAppProjectName;

        public string ClientBlazorProjectName => TemplateProjectNames.First(e => e.EndsWith($"{ClientBlazorExtension}"));
        public string ClientBlazorSubPath => ClientBlazorProjectName;

        public string AngularAppProjectName => TemplateProjectNames.First(e => e.EndsWith($"{AngularExtension}"));
        #endregion ProjectNames

        private SolutionProperties(string solutionPath)
        {
            SolutionPath = solutionPath;
            SolutionName = GetSolutionName(solutionPath);
            SolutionFilePath = GetSolutionFilePath(solutionPath);

            LogicAssemblyFilePath = GetLogicAssemblyFilePath(solutionPath);
            LogicCSProjectFilePath = GetLogicCSProjectFilePath(solutionPath);
        }

        public string GetProjectNameFromPath(string projectPath)
        {
            return projectPath.Replace(SolutionPath, string.Empty);
        }
        public string GetProjectNameFromFile(string filePath)
        {
            var result = string.Empty;
            var data = filePath.Split(Path.DirectorySeparatorChar);
            var idx = data.IndexOf(SolutionName);

            if (idx + 1 < data.Length)
            {
                result = data[idx + 1];
            }
            return result;
        }
        public bool IsTemplateProjectFile(string filePath)
        {
            return TemplateProjectPaths.Any(tpp => filePath.StartsWith(tpp, StringComparison.CurrentCultureIgnoreCase));
        }

        #region factorey methods
        public static SolutionProperties Create()
        {
            return new SolutionProperties(GetCurrentSolutionPath());
        }
        public static SolutionProperties Create(string solutionPath)
        {
            return new SolutionProperties(solutionPath);
        }
        public static SolutionProperties Create(string solutionPath, Type[] locigAssemblyTypes)
        {
            return new SolutionProperties(solutionPath)
            {
                LogicAssemblyTypes = locigAssemblyTypes,
            };
        }
        #endregion factory methods

        private static string GetCurrentSolutionPath()
        {
            int endPos = AppContext.BaseDirectory.IndexOf($"{nameof(TemplateCodeGenerator)}", StringComparison.CurrentCultureIgnoreCase);

            return AppContext.BaseDirectory[..endPos];
        }
        private static string GetSolutionName(string solutionPath)
        {
            var fileInfo = new DirectoryInfo(solutionPath).GetFiles().SingleOrDefault(f => f.Extension.Equals(".sln", StringComparison.CurrentCultureIgnoreCase));

            return fileInfo != null ? Path.GetFileNameWithoutExtension(fileInfo.Name) : string.Empty;
        }
        private static string GetSolutionFilePath(string solutionPath)
        {
            var result = default(string);
            var solutionName = GetSolutionName(solutionPath);

            if (Directory.Exists(solutionPath))
            {
                var fileName = $"{solutionName}.sln";
                var fileInfos = new DirectoryInfo(solutionPath).GetFiles(fileName, SearchOption.AllDirectories)
                                                          .Where(f => f.FullName.EndsWith(fileName))
                                                          .OrderByDescending(f => f.LastWriteTime);

                var fileInfo = fileInfos.Where(f => f.FullName.ToLower().Contains("\\ref\\") == false)
                                        .FirstOrDefault();

                if (fileInfo != null)
                {
                    result = fileInfo.FullName;
                }
            }
            return result ?? string.Empty;
        }
        private static string GetLogicCSProjectFilePath(string solutionPath)
        {
            var result = default(string);
            var solutionName = GetSolutionName(solutionPath);
            var projectName = $"{solutionName}{StaticLiterals.LogicExtension}";
            var path = Path.Combine(solutionPath, projectName);

            if (Directory.Exists(path))
            {
                var fileName = $"{projectName}.csproj";
                var fileInfos = new DirectoryInfo(path).GetFiles(fileName, SearchOption.AllDirectories)
                                                          .Where(f => f.FullName.EndsWith(fileName))
                                                          .OrderByDescending(f => f.LastWriteTime);

                var fileInfo = fileInfos.Where(f => f.FullName.ToLower().Contains("\\ref\\") == false)
                                        .FirstOrDefault();

                if (fileInfo != null)
                {
                    result = fileInfo.FullName;
                }
            }
            return result ?? string.Empty;
        }
        private static string GetCompileAssemblyFilePath(string solutionName, string compilePath)
        {
            var result = default(string);
            var projectName = $"{solutionName}{StaticLiterals.LogicExtension}";

            if (Directory.Exists(compilePath))
            {
                var fileName = $"{projectName}.dll";
                var fileInfos = new DirectoryInfo(compilePath).GetFiles(fileName, SearchOption.AllDirectories)
                                                              .Where(f => f.FullName.EndsWith(fileName))
                                                              .OrderByDescending(f => f.LastWriteTime);

                var fileInfo = fileInfos.Where(f => f.FullName.ToLower().Contains("\\ref\\") == false)
                                        .FirstOrDefault();

                if (fileInfo != null)
                {
                    result = fileInfo.FullName;
                }
            }
            return result ?? string.Empty;
        }
        private static string GetLogicAssemblyFilePath(string solutionPath)
        {
            var result = default(string);
            var solutionName = GetSolutionName(solutionPath);
            var projectName = $"{solutionName}{StaticLiterals.LogicExtension}";
            var binPath = Path.Combine(solutionPath, projectName, "bin");

            if (Directory.Exists(binPath))
            {
                var fileName = $"{projectName}.dll";
                var fileInfos = new DirectoryInfo(binPath).GetFiles(fileName, SearchOption.AllDirectories)
                                                          .Where(f => f.FullName.EndsWith(fileName))
                                                          .OrderByDescending(f => f.LastWriteTime);

                var fileInfo = fileInfos.Where(f => f.FullName.ToLower().Contains("\\ref\\") == false)
                                        .FirstOrDefault();

                if (fileInfo != null)
                {
                    result = fileInfo.FullName;
                }
            }
            return result ?? string.Empty;
        }
    }
}
//MdEnd
