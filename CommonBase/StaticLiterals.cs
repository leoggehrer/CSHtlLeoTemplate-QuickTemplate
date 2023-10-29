//@BaseCode
//MdStart
namespace CommonBase
{
    /// <summary>
    /// Provides a collection of static literals.
    /// </summary>
    public static partial class StaticLiterals
    {
        /// Initializes the static literals.
        static StaticLiterals()
        {
            BeforeClassInitialize();
            TemplateProjects = new string[]
            {
                "CommonBase",
            };
            TemplateToolProjects = new[]
            {
                "TemplateCodeGenerator.Logic",
                "TemplateTools.ConApp",
            };
            TemplateProjectExtensions = new string[]
            {
                ConsoleExtension,
                CodeGenerationExtension,
                LogicExtension,
                LogicUnitTestExtension,
                WebApiExtension,
                MVVMExtension,
                AspMvcExtension,
                AngularExtension,
                ClientBlazorExtension,
            };
            GenerationIgnoreFolders = new string[] { "node_module" };
            AfterClassInitialize();
        }
        /// <summary>
        /// This method is called before the class is initialized.
        /// </summary>
        static partial void BeforeClassInitialize();
        /// <summary>
        /// This method is called after the initialization of the class is completed.
        /// </summary>
        static partial void AfterClassInitialize();
        
        /// Gets the extension for solution files.
        /// @return The file extension for solution files as a string.
        public static string SolutionFileExtension => ".sln";
        /// <summary>
        /// Gets the file extension for project files.
        /// </summary>
        /// <value>
        /// The project file extension.
        /// </value>
        public static string ProjectFileExtension => ".csproj";
        
        #region Template project extensions
        /// <summary>
        /// Extension for Console Application file format.
        /// </summary>
        /// <value>
        /// The extension string ".ConApp".
        /// </value>
        public static string ConsoleExtension => ".ConApp";
        /// <summary>
        /// Gets the code generation extension.
        /// </summary>
        /// <value>The code generation extension.</value>
        public static string CodeGenerationExtension => ".CodeGenApp";
        /// <summary>
        /// Gets the logic extension.
        /// </summary>
        /// <value>
        /// The logic extension.
        /// </value>
        public static string LogicExtension => ".Logic";
        /// <summary>
        /// Gets the extension for Logic Unit Tests.
        /// </summary>
        /// <returns>The extension for Logic Unit Tests.</returns>
        public static string LogicUnitTestExtension => ".Logic.UnitTest";
        /// <summary>
        /// Gets the extension for WebApi files.
        /// </summary>
        /// <returns>The extension for WebApi files.</returns>
        public static string WebApiExtension => ".WebApi";
        /// <summary>
        /// Gets the MVVM extension for a WPF application.
        /// </summary>
        /// <returns>
        /// A string representing the MVVM extension.
        /// </returns>
        public static string MVVMExtension => ".WpfApp";
        /// <summary>
        /// Represents the file extension for ASP.NET MVC files.
        /// </summary>
        /// <remarks>
        /// This file extension is used to identify files that are related to ASP.NET MVC framework.
        /// </remarks>
        /// <value>
        /// A string representing the ".AspMvc" file extension.
        /// </value>
        public static string AspMvcExtension => ".AspMvc";
        /// <summary>
        /// Gets the extension for Angular app files.
        /// </summary>
        /// <returns>The extension for Angular app files.</returns>
        public static string AngularExtension => ".AngularApp";
        /// <summary>
        /// Gets the file extension for a client Blazor application.
        /// </summary>
        /// <returns>The file extension.</returns>
        public static string ClientBlazorExtension => ".ClientBlazorApp";
        #endregion Template project extensions
        
        /// <summary>
        /// Gets or sets the array of template projects.
        /// </summary>
        /// <value>
        /// An array of strings representing the template projects.
        /// </value>
        public static string[] TemplateProjects { get; private set; }
        /// Gets or sets the array of template tool projects.
        /// This property represents an array of strings that stores the names of template tool projects.
        /// The array is read-only and can only be set through the private setter.
        /// @returns An array of strings representing the names of template tool projects.
        public static string[] TemplateToolProjects { get; private set; }
        /// <summary>
        /// Gets the array of template project extensions.
        /// </summary>
        /// <value>
        /// The array of template project extensions.
        /// </value>
        public static string[] TemplateProjectExtensions { get; private set; }
        
        /// Gets or sets the array of folders that should be ignored during generation.
        /// This property is used to specify the folders that should not be included when generating something.
        /// @returns The array of folders to be ignored during generation.
        /// @since 1.0.0
        public static string[] GenerationIgnoreFolders { get; private set; }
        /// Gets the label for generated code.
        /// @returns The label for generated code.
        public static string GeneratedCodeLabel => "@GeneratedCode";
        /// <summary>
        /// Gets the label for customized and generated code.
        /// </summary>
        /// <returns>The label for customized and generated code.</returns>
        public static string CustomizedAndGeneratedCodeLabel => "@CustomAndGeneratedCode";
        /// <summary>
        /// Represents the label to ignore.
        /// </summary>
        /// <value>
        /// The label to ignore.
        /// </value>
        public static string IgnoreLabel => "@Ignore";
        /// <summary>
        /// Gets or sets the label for the base code.
        /// </summary>
        /// <value>
        /// The label for the base code.
        /// </value>
        public static string BaseCodeLabel => "@BaseCode";
        /// <summary>
        /// Gets a label for code copy.
        /// </summary>
        /// <value>A string representing the label for code copy.</value>
        public static string CodeCopyLabel => "@BaseCode";
        /// <summary>
        /// Gets the file extension for C# files.
        /// </summary>
        /// <returns>The file extension for C# files, which is '.cs'.</returns>
        public static string CSharpFileExtension => ".cs";
        /// <summary>
        /// Gets the extensions of source files.
        /// </summary>
        /// <value>
        /// The extensions of source files in the format "*.css|*.cs|*.ts|*.cshtml|*.razor|*.razor.cs|*.template".
        /// </value>
        public static string SourceFileExtensions => "*.css|*.cs|*.ts|*.cshtml|*.razor|*.razor.cs|*.template";
        
        /// <summary>
        /// Gets the maximum page size.
        /// </summary>
        /// <returns>The maximum page size as an integer.</returns>
        public static int MaxPageSize => 500;
        
        #region Folders and Files
        /// <summary>
        /// Gets the list of folders to be ignored.
        /// </summary>
        /// <value>
        /// An array of strings representing the folders to be ignored.
        /// </value>
        public static string[] IgnoreFolders { get; } = new string[]
        {
            $"{Path.DirectorySeparatorChar}.vs",
            $"{Path.DirectorySeparatorChar}.vs{Path.DirectorySeparatorChar}",
            $"{Path.DirectorySeparatorChar}.vscode",
            $"{Path.DirectorySeparatorChar}.vscode{Path.DirectorySeparatorChar}",
            $"{Path.DirectorySeparatorChar}.git",
            $"{Path.DirectorySeparatorChar}.git{Path.DirectorySeparatorChar}",
            $"{Path.DirectorySeparatorChar}bin",
            $"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}",
            $"{Path.DirectorySeparatorChar}obj",
            $"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}",
            $"{Path.DirectorySeparatorChar}node_modules{Path.DirectorySeparatorChar}",
        };
        /// <summary>
        /// Array of folder file paths to be ignored.
        /// </summary>
        /// <value>
        /// Array of folder file paths.
        /// </value>
        public static string[] IgnoreFolderFiles { get; } = new string[]
        {
            $"{Path.DirectorySeparatorChar}.vs{Path.DirectorySeparatorChar}",
            $"{Path.DirectorySeparatorChar}.vscode{Path.DirectorySeparatorChar}",
            $"{Path.DirectorySeparatorChar}.git{Path.DirectorySeparatorChar}",
            $"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}",
            $"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}",
            $"{Path.DirectorySeparatorChar}node_modules{Path.DirectorySeparatorChar}",
            $"{Path.DirectorySeparatorChar}Migrations{Path.DirectorySeparatorChar}",
        };
        #endregion Folders and Files
    }
}
//MdEnd
