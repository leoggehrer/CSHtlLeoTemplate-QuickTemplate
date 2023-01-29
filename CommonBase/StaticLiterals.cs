//@BaseCode
//MdStart
namespace CommonBase
{
    public static partial class StaticLiterals
    {
        static StaticLiterals()
        {
            BeforeClassInitialize();
            SolutionProjects = new string[]
            {
                "CommonBase",
                "TemplateCodeGenerator.Logic",
                "TemplateTools.ConApp",
            };
            ProjectExtensions = new string[]
            {
                ".ConApp",
                ".CodeGenApp",
                ".Logic",
                ".Logic.UnitTest",
                ".WebApi",
                ".AspMvc",
                ".AngularApp",
                ".WpfApp",
                ".MvvMApp",
            };
            SolutionToolProjects = new[]
            {
                "TemplateCodeGenerator.Logic",
            };
            GenerationIgnoreFolders = new string[] { "node_module" };
            AfterClassInitialize();
        }
        static partial void BeforeClassInitialize();
        static partial void AfterClassInitialize();

        public static string SolutionFileExtension => ".sln";
        public static string ProjectFileExtension => ".csproj";

        public static string[] SolutionProjects { get; private set; }
        public static string[] ProjectExtensions { get; private set; }
        public static string[] SolutionToolProjects { get; private set; }

        public static string[] GenerationIgnoreFolders { get; private set; }
        public static string GeneratedCodeLabel => "@GeneratedCode";
        public static string CustomizedAndGeneratedCodeLabel => "@CustomAndGeneratedCode";
        public static string IgnoreLabel => "@Ignore";
        public static string BaseCodeLabel => "@BaseCode";
        public static string CodeCopyLabel => "@CodeCopy";
        public static string CSharpFileExtension => ".cs";
        public static string SourceFileExtensions => "*.css|*.cs|*.ts|*.cshtml|*.razor|*.razor.cs|*.template";
    }
}
//MdEnd
