//@BaseCode
//MdStart
using CommonStaticLiterals = CommonBase.StaticLiterals;

namespace TemplateCodeGenerator.Logic
{
    public static partial class StaticLiterals
    {
        #region Code-Generation
        public static string AllItems => "All";
        public static string GeneratedCodeFileName => "_GeneratedCode.cs";
        public static string CustomFileExtension => ".custom";
        public static string SourceFileExtensions => CommonStaticLiterals.SourceFileExtensions;
        public static string CSharpFileExtension => CommonStaticLiterals.CSharpFileExtension;
        public static string CSharpHtmlFileExtension => $"{CommonStaticLiterals.CSharpFileExtension}html";
        public static string GeneratedCodeLabel => CommonStaticLiterals.GeneratedCodeLabel;
        public static string CustomizedAndGeneratedCodeLabel => CommonStaticLiterals.CustomizedAndGeneratedCodeLabel;

        public static string AngularCustomImportBeginLabel => "//@CustomImportBegin";
        public static string AngularCustomImportEndLabel => "//@CustomImportEnd";
        public static string AngularCustomCodeBeginLabel => "//@CustomCodeBegin";
        public static string AngularCustomCodeEndLabel => "//@CustomCodeEnd";

        public static string EntityAlias => "TEntity";
        public static string ModelAlias => "TModel";
        #endregion Code-Generation

        #region Project Extensions
        public static string LogicExtension => CommonStaticLiterals.LogicExtension;
        public static string WebApiExtension => CommonStaticLiterals.WebApiExtension;
        public static string AspMvcExtension => CommonStaticLiterals.AspMvcExtension;
        public static string MVVMExtension => CommonStaticLiterals.MVVMExtension;
        public static string AngularExtension => CommonStaticLiterals.AngularExtension;
        public static string ClientBlazorExtension => CommonStaticLiterals.ClientBlazorExtension;
        #endregion Project Extensions

        #region Entity and service properties
        public static readonly string IdentityProperty = "Id";
        public static readonly string EntityObjectName = "EntityObject";
        public static readonly string VersionEntityName = "VersionEntity";
        public static readonly string RowVersionProperty = "RowVersion";
        public static readonly string[] IdentityProperties = new string[] { IdentityProperty };
        public static readonly string[] VersionProperties = new string[] { IdentityProperty, RowVersionProperty };
        public static readonly string[] ExtendedProperties = new string[] { "Guid", "CreatedOn", "ModifiedOn", "IdentityId_CreatedBy", "IdentityId_ModifiedBy" };
        public static string[] NoGenerationProperties => IdentityProperties.Union(VersionProperties).ToArray();
        #endregion Entity and service properties

        #region Model properties
        public static readonly string IdType = nameof(IdType);
        public static readonly string ModelObjectName = "ModelObject";
        public static readonly string VersionModelName = "VersionModel";
        public static readonly string ServiceModelName = "ServiceModel";
        #endregion Model properties

        public static readonly string[] ModelBaseClasses = new[] { ModelObjectName, VersionModelName };
        public static readonly Dictionary<string, string> BaseClassMapping = new()
        {
            { EntityObjectName, ModelObjectName },
            { VersionEntityName, VersionModelName },
            { ServiceModelName, $"BaseModels.{ServiceModelName}" },
        };

        #region Folders
        public static string EntitiesFolder => "Entities";
        public static string AccountFolder => "Account";
        public static string LoggingFolder => "Logging";
        public static string RevisionFolder => "Revision";
        public static string ModelsFolder => "Models";
        public static string ServiceModelsFolder => "ServiceModels";
        public static string ContractsFolder => "Contracts";
        public static string DataContextFolder => "DataContext";
        public static string ControllersFolder => "Controllers";
        public static string ServicesFolder => "Services";
        public static string FacadesFolder => "Facades";
        public static string ViewsFolder => "Views";
        #endregion Folders

        #region Settings
        public static string TProperty => nameof(TProperty);
        public static string Generate => nameof(Generate);
        public static string Visibility => nameof(Visibility);
        public static string Attributes => nameof(Attributes);
        public static string ControllerGenericType => nameof(ControllerGenericType);
        public static string FacadeGenericType => nameof(FacadeGenericType);
        public static string ServiceGenericType => nameof(ServiceGenericType);
        #endregion Settings

        #region Modules
        public static string Account => nameof(Account);
        public static string Access => nameof(Access);
        public static string Logging => nameof(Logging);
        public static string Revision => nameof(Revision);
        public static string SecureIdentity => nameof(SecureIdentity);
        public static string LoginSession => nameof(LoginSession);
        #endregion Modules
    }
}
//MdEnd
