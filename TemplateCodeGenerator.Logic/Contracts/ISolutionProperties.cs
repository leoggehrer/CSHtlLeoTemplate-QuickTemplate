//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic.Contracts
{
    public interface ISolutionProperties
    {
        #region Project extensions
        string LogicExtension { get; }
        string WebApiExtension { get; }
        string AspMvcExtension { get; }
        string AngularExtension { get; }
        string MVVMExtension { get; }
        string ClientBlazorExtension { get; }
        #endregion Project extensions

        #region properties
        string SolutionPath { get; }
        string SolutionName { get; }
        string SolutionFilePath { get; }
        string? CompilePath { get; set; }
        Type[]? LogicAssemblyTypes { get; set; }
        string? CompileLogicAssemblyFilePath { get; }

        IEnumerable<string> TemplateProjectNames { get; }
        IEnumerable<string> AllTemplateProjectNames { get; }
        IEnumerable<string> TemplateProjectPaths { get; }

        string LogicCSProjectFilePath { get; }
        string LogicAssemblyFilePath { get; }
        string LogicProjectName { get; }
        string LogicSubPath { get; }
        string LogicControllersSubPath { get; }
        string LogicDataContextSubPath { get; }
        string LogicEntitiesSubPath { get; }

        string WebApiProjectName { get; }
        string WebApiSubPath { get; }
        string WebApiControllersSubPath { get; }

        string AspMvcAppProjectName { get; }
        string AspMvcAppSubPath { get; }
        string AspMvcControllersSubPath { get; }

        string MVVMAppProjectName { get; }
        string MVVMAppSubPath { get; }

        string ClientBlazorProjectName { get; }
        string ClientBlazorSubPath { get; }

        string AngularAppProjectName { get; }
        #endregion properties

        #region methods
        bool IsTemplateProjectFile(string filePath);
        string GetProjectNameFromPath(string projectPath);
        string GetProjectNameFromFile(string filePath);
        #endregion methods
    }
}
//MdEnd
