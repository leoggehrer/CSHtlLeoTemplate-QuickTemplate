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
        public static string LogicExtension => ".Logic";
        public static string WebApiExtension => ".WebApi";
        public static string AspMvsExtension => ".AspMvc";
        #endregion Project Extensions

        #region Entity and service properties
        public static string EntityObjectName => "EntityObject";
        public static string VersionEntityName => "VersionEntity";
        public static string[] EntityBaseClasses => new string[] { VersionEntityName, EntityObjectName };
        public static string IdentityServiceName => "IdentityService";
        public static string VersionServiceName => "VersionService";
        public static string[] ServiceBaseClasses => new string[] { VersionServiceName, IdentityServiceName };
        public static string[] BaseClasses => EntityBaseClasses.Union(ServiceBaseClasses).ToArray();
        public static string IdentityProperty => "Id";
        public static string RowVersionProperty => "RowVersion";
        public static string[] IdentityProperties => new string[] { IdentityProperty };
        public static string[] VersionProperties => new string[] { IdentityProperty, RowVersionProperty };
        public static string[] ExtendedProperties = new string[] { "Guid", "CreatedOn", "ModifiedOn", "IdentityId_CreatedBy", "IdentityId_ModifiedBy" };
        public static string[] NoGenerationProperties => IdentityProperties.Union(VersionProperties).ToArray();
        #endregion Entity and service properties

        #region Model properties
        public static string IdType => nameof(IdType);
        public static string ModelObjectName => "ModelObject";
        public static string VersionModelName => "VersionModel";
        public static string[] ModelBaseClasses => new string[] { VersionModelName, ModelObjectName };
        #endregion Model properties

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