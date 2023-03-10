﻿//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic.Contracts
{
    public interface ISolutionProperties
    {
        #region Project-postfixes
        string LogicPostfix { get; }
        string WebApiPostfix { get; }
        string AspMvcPostfix { get; }
        string MVVMPostfix { get; }
        #endregion Project-postfixes

        string SolutionPath { get; }
        string SolutionName { get; }
        string SolutionFilePath { get; }
        string? CompilePath { get; set; }
        Type[]? LogicAssemblyTypes { get; set; }
        string? CompileLogicAssemblyFilePath { get; }

        IEnumerable<string> ProjectNames { get; }

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

        string AngularAppProjectName { get; }
    }
}
//MdEnd
