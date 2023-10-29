﻿//@CodeCopy
//MdStart

namespace TemplateCodeGenerator.Logic
{
    /// <summary>
    /// Represents a solution's properties.
    /// </summary>
    public sealed partial class SolutionProperties : Contracts.ISolutionProperties
    {
        #region Project-postfixes
        /// <summary>
        /// Gets or sets the logic extension.
        /// </summary>
        /// <value>The logic extension.</value>
        public string LogicExtension => StaticLiterals.LogicExtension;
        /// <summary>
        /// Gets or sets the Web API extension.
        /// </summary>
        /// <value>
        /// The Web API extension.
        /// </value>
        public string WebApiExtension => StaticLiterals.WebApiExtension;
        /// <summary>
        /// Gets the ASP.NET MVC extension.
        /// </summary>
        /// <value>The ASP.NET MVC extension.</value>
        public string AspMvcExtension => StaticLiterals.AspMvcExtension;
        /// <summary>
        /// The MVVMExtension property represents the MVVM extension value.
        /// </summary>
        /// <value>The MVVM extension value.</value>
        public string MVVMExtension => StaticLiterals.MVVMExtension;
        /// <summary>
        /// Gets the Angular extension used for the property.
        /// </summary>
        /// <value>
        /// The Angular extension used for the property.
        /// </value>
        public string AngularExtension => StaticLiterals.AngularExtension;
        /// <summary>
        /// Gets the client Blazor extension.
        /// </summary>
        /// <value>
        /// The client Blazor extension.
        /// </value>
        public string ClientBlazorExtension => StaticLiterals.ClientBlazorExtension;
        #endregion Project-postfixes
        
        /// <summary>
        /// Gets the solution path.
        /// </summary>
        /// <value>The solution path.</value>
        public string SolutionPath { get; }
        /// <summary>
        /// Gets the solution name.
        /// </summary>
        /// <value>
        /// The solution name.
        /// </value>
        public string SolutionName { get; }
        /// <summary>
        /// Gets the file path of the solution.
        /// </summary>
        /// <value>The file path of the solution.</value>
        public string SolutionFilePath { get; }
        /// <summary>
        /// Gets or sets the path where the code should be compiled.
        /// </summary>
        /// <value>
        /// The compile path is the location where the generated code will be compiled.
        /// </value>
        /// <remarks>
        /// This property can be null if the code does not need to be compiled.
        /// </remarks>
        public string? CompilePath { get; set; }
        /// <summary>
        /// Gets or sets the logic assembly types.
        /// </summary>
        /// <value>
        /// The logic assembly types.
        /// </value>
        public Type[]? LogicAssemblyTypes { get; set; }
        /// <summary>
        /// Gets the file path of the compiled logic assembly.
        /// </summary>
        /// <returns>
        /// The file path of the compiled logic assembly, or an empty string if it does not exist.
        /// </returns>
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
        /// <summary>
        /// Gets the template project names.
        /// </summary>
        /// <value>
        /// An enumerable collection of template project names.
        /// </value>
        public IEnumerable<string> TemplateProjectNames => CommonBase.StaticLiterals.TemplateProjectExtensions.Select(e => $"{SolutionName}{e}");
        /// <summary>
        /// Gets a collection of all template project names.
        /// </summary>
        /// <returns>
        /// An IEnumerable of string representing the template project names.
        /// </returns>
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
        /// <summary>
        /// Gets the collection of file paths for template projects.
        /// </summary>
        /// <value>
        /// An <see cref="IEnumerable{T}"/> containing the file paths for template projects.
        /// </value>
        public IEnumerable<string> TemplateProjectPaths => TemplateProjectNames.Select(tpn => Path.Combine(SolutionPath, tpn));
        
        /// <summary>
        /// Gets the file path of the Logic Assembly.
        /// </summary>
        /// <value>
        /// The file path of the Logic Assembly.
        /// </value>
        public string LogicAssemblyFilePath { get; }
        /// <summary>
        /// Gets the file path of the Logic CS project.
        /// </summary>
        /// <returns>The file path of the Logic CS project.</returns>
        public string LogicCSProjectFilePath { get; }
        
        /// <summary>
        /// Gets the logic project name based on the template project names.
        /// </summary>
        /// <value>
        /// A string representing the logic project name.
        /// </value>
        public string LogicProjectName => TemplateProjectNames.First(e => e.EndsWith($"{LogicExtension}"));
        /// <summary>
        /// Gets the logic sub path.
        /// </summary>
        /// <value>The logic sub path.</value>
        public string LogicSubPath => LogicProjectName;
        /// <summary>
        /// Gets the subpath for logic controllers.
        /// </summary>
        /// <value>
        /// The subpath for logic controllers.
        /// </value>
        public string LogicControllersSubPath => StaticLiterals.ControllersFolder;
        /// <summary>
        /// Gets the subpath for logic entities.
        /// </summary>
        /// <value>The subpath for logic entities.</value>
        public string LogicEntitiesSubPath => StaticLiterals.EntitiesFolder;
        /// <summary>
        /// Gets the sub-path for the LogicDataContext.
        /// </summary>
        /// <value>
        /// The sub-path for the LogicDataContext.
        /// </value>
        public string LogicDataContextSubPath => StaticLiterals.DataContextFolder;
        
        /// <summary>
        /// The name of the Web API project.
        /// </summary>
        /// <remarks>
        /// This property returns the name of the Web API project by getting the first element
        /// from the collection of template project names that ends with the Web API extension.
        /// </remarks>
        public string WebApiProjectName => TemplateProjectNames.First(e => e.EndsWith($"{WebApiExtension}"));
        /// <summary>
        /// Gets the sub path of the Web API.
        /// </summary>
        /// <value>
        /// A <see cref="System.String"/> representing the sub path of the Web API.
        /// </value>
        public string WebApiSubPath => WebApiProjectName;
        /// <summary>
        /// Gets or sets the sub path for Web API controllers.
        /// </summary>
        /// <value>
        /// The sub path for Web API controllers.
        /// </value>
        public string WebApiControllersSubPath => Path.Combine(WebApiSubPath, StaticLiterals.ControllersFolder);
        
        /// <summary>
        /// Gets the name of the ASP.NET MVC application project.
        /// </summary>
        /// <value>
        /// The name of the ASP.NET MVC application project.
        /// </value>
        public string AspMvcAppProjectName => TemplateProjectNames.First(e => e.EndsWith($"{AspMvcExtension}"));
        /// <summary>
        /// Gets or sets the subpath of the ASP.NET MVC application.
        /// </summary>
        /// <value>
        /// The subpath of the ASP.NET MVC application.
        /// </value>
        public string AspMvcAppSubPath => AspMvcAppProjectName;
        /// <summary>
        /// Gets the sub-path to the ASP.NET MVC controllers folder.
        /// </summary>
        /// <value>The sub-path to the ASP.NET MVC controllers folder.</value>
        public string AspMvcControllersSubPath => Path.Combine(AspMvcAppSubPath, StaticLiterals.ControllersFolder);
        
        /// <summary>
        /// Gets the name of the MVVM app project.
        /// </summary>
        /// <value>
        /// The name of the MVVM app project.
        /// </value>
        public string MVVMAppProjectName => TemplateProjectNames.First(e => e.EndsWith($"{MVVMExtension}"));
        /// <summary>
        /// Gets the subpath of the MVVM app.
        /// </summary>
        /// <value>
        /// The subpath of the MVVM app.
        /// </value>
        public string MVVMAppSubPath => MVVMAppProjectName;
        
        /// <summary>
        /// Gets the name of the Blazor project for the client.
        /// </summary>
        /// <value>
        /// The name of the Blazor project for the client.
        /// </value>
        public string ClientBlazorProjectName => TemplateProjectNames.First(e => e.EndsWith($"{ClientBlazorExtension}"));
        /// <summary>
        /// Gets the sub path of the Blazor project for the client.
        /// </summary>
        /// <value>
        /// The sub path of the Blazor project for the client.
        /// </value>
        public string ClientBlazorSubPath => ClientBlazorProjectName;
        
        /// <summary>
        /// Gets the name of the Angular app project.
        /// </summary>
        /// <remarks>
        /// This property returns the first project name in the list of template project names that ends with the specified Angular extension.
        /// </remarks>
        /// <value>
        /// The name of the Angular app project.
        /// </value>
        public string AngularAppProjectName => TemplateProjectNames.First(e => e.EndsWith($"{AngularExtension}"));
        #endregion ProjectNames
        
        /// <summary>
        /// Initializes a new instance of the SolutionProperties class with the specified solution path.
        /// </summary>
        /// <param name="solutionPath">The path to the solution.</param>
        private SolutionProperties(string solutionPath)
        {
            SolutionPath = solutionPath;
            SolutionName = GetSolutionName(solutionPath);
            SolutionFilePath = GetSolutionFilePath(solutionPath);
            
            LogicAssemblyFilePath = GetLogicAssemblyFilePath(solutionPath);
            LogicCSProjectFilePath = GetLogicCSProjectFilePath(solutionPath);
        }
        
        ///<summary>
        /// Retrieves the project name from the given project path.
        ///</summary>
        ///<param name="projectPath">A string representing the path of the project.</param>
        ///<returns>The project name extracted from the project path.</returns>
        public string GetProjectNameFromPath(string projectPath)
        {
            return projectPath.Replace(SolutionPath, string.Empty);
        }
        /// <summary>
        /// Retrieves the project name from a file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The project name extracted from the file path.</returns>
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
        /// <summary>
        /// Checks if the given file path is a template project file.
        /// </summary>
        /// <param name="filePath">The file path to be checked.</param>
        /// <returns>
        ///   <c>true</c> if the file path is a template project file; otherwise, <c>false</c>.
        /// </returns>
        public bool IsTemplateProjectFile(string filePath)
        {
            return TemplateProjectPaths.Any(tpp => filePath.StartsWith(tpp, StringComparison.CurrentCultureIgnoreCase));
        }
        
        #region factorey methods
        /// <summary>
        /// Creates a new instance of SolutionProperties class.
        /// </summary>
        /// <returns>A new instance of SolutionProperties class.</returns>
        public static SolutionProperties Create()
        {
            return new SolutionProperties(GetCurrentSolutionPath());
        }
        /// <summary>
        /// Creates a new instance of the SolutionProperties class.
        /// </summary>
        /// <param name="solutionPath">The path to the solution file.</param>
        /// <returns>A new SolutionProperties object.</returns>
        public static SolutionProperties Create(string solutionPath)
        {
            return new SolutionProperties(solutionPath);
        }
        /// <summary>
        /// Creates an instance of SolutionProperties with the given solution path and logic assembly types.
        /// </summary>
        /// <param name="solutionPath">The path of the solution.</param>
        /// <param name="locigAssemblyTypes">An array of logic assembly types.</param>
        /// <returns>The created SolutionProperties instance.</returns>
        public static SolutionProperties Create(string solutionPath, Type[] locigAssemblyTypes)
        {
            return new SolutionProperties(solutionPath)
            {
                LogicAssemblyTypes = locigAssemblyTypes,
            };
        }
        #endregion factory methods
        
        /// <summary>
        /// Retrieves the current solution path where the code is executing.
        /// </summary>
        /// <returns>
        /// A string representing the current solution path.
        /// </returns>
        private static string GetCurrentSolutionPath()
        {
            int endPos = AppContext.BaseDirectory.IndexOf($"{nameof(TemplateCodeGenerator)}", StringComparison.CurrentCultureIgnoreCase);
            
            return AppContext.BaseDirectory[..endPos];
        }
        /// <summary>
        /// Retrieves the name of the solution file given its path.
        /// </summary>
        /// <param name="solutionPath">The path of the solution file.</param>
        /// <returns>The name of the solution file without its extension, or an empty string if no solution file found.</returns>
        private static string GetSolutionName(string solutionPath)
        {
            var fileInfo = new DirectoryInfo(solutionPath).GetFiles().SingleOrDefault(f => f.Extension.Equals(".sln", StringComparison.CurrentCultureIgnoreCase));
            
            return fileInfo != null ? Path.GetFileNameWithoutExtension(fileInfo.Name) : string.Empty;
        }
        /// <summary>
        /// Retrieves the full file path of a solution based on the provided solution path.
        /// </summary>
        /// <param name="solutionPath">The path to the solution to get the file path for.</param>
        /// <returns>The full file path of the solution, or an empty string if not found.</returns>
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
        /// <summary>
        /// Retrieves the file path of the logic C# project based on the specified solution path.
        /// </summary>
        /// <param name="solutionPath">The path of the solution file.</param>
        /// <returns>The file path of the logic C# project if found, or an empty string if not found.</returns>
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
        /// <summary>
        /// Returns the full path of the compile assembly file for a given solution name and compile path.
        /// </summary>
        /// <param name="solutionName">The name of the solution.</param>
        /// <param name="compilePath">The path where the assembly files are compiled.</param>
        /// <returns>The full path of the compile assembly file, or an empty string if it is not found.</returns>
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
        /// <summary>
        /// Gets the file path of the logic assembly for a given solution path.
        /// </summary>
        /// <param name="solutionPath">The path of the solution.</param>
        /// <returns>The file path of the logic assembly or an empty string if not found.</returns>
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



