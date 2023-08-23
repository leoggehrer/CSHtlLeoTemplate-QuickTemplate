//@BaseCode
//MdStart
namespace CommonBase
{
    public static partial class StaticLiterals
    {
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
        static partial void BeforeClassInitialize();
        static partial void AfterClassInitialize();

        public static string SolutionFileExtension => ".sln";
        public static string ProjectFileExtension => ".csproj";

        #region Template project extensions
        public static string ConsoleExtension => ".ConApp";
        public static string CodeGenerationExtension => ".CodeGenApp";
        public static string LogicExtension => ".Logic";
        public static string LogicUnitTestExtension => ".Logic.UnitTest";
        public static string WebApiExtension => ".WebApi";
        public static string MVVMExtension => ".WpfApp";
        public static string AspMvcExtension => ".AspMvc";
        public static string AngularExtension => ".AngularApp";
        public static string ClientBlazorExtension => ".ClientBlazorApp";
        #endregion Template project extensions

        public static string[] TemplateProjects { get; private set; }
        public static string[] TemplateToolProjects { get; private set; }
        public static string[] TemplateProjectExtensions { get; private set; }

        public static string[] GenerationIgnoreFolders { get; private set; }
        public static string GeneratedCodeLabel => "@GeneratedCode";
        public static string CustomizedAndGeneratedCodeLabel => "@CustomAndGeneratedCode";
        public static string IgnoreLabel => "@Ignore";
        public static string BaseCodeLabel => "@BaseCode";
        public static string CodeCopyLabel => "@CodeCopy";
        public static string CSharpFileExtension => ".cs";
        public static string SourceFileExtensions => "*.css|*.cs|*.ts|*.cshtml|*.razor|*.razor.cs|*.template";

        public static int MaxPageSize => 500;
    }
}
//MdEnd
