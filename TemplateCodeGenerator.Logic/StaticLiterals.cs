//@BaseCode
//MdStart
using CommonStaticLiterals = CommonBase.StaticLiterals;

namespace TemplateCodeGenerator.Logic
{
    /// <summary>
    /// Static class containing various literals used throughout the application.
    /// </summary>
    public static partial class StaticLiterals
    {
        #region Code-Generation
        /// <summary>
        /// Gets the value "All" representing all items.
        /// </summary>
        /// <value>The value representing all items.</value>
        public static string AllItems => "All";
        /// <summary>
        /// Gets the name of the generated code file.
        /// </summary>
        /// <value>
        /// The name of the generated code file.
        /// </value>
        public static string GeneratedCodeFileName => "_GeneratedCode.cs";
        /// <summary>
        /// Gets the custom file extension.
        /// </summary>
        /// <value>The custom file extension.</value>
        public static string CustomFileExtension => ".custom";
        /// <summary>
        /// Gets or sets the source file extensions.
        /// </summary>
        /// <value>
        /// A string representing the source file extensions.
        /// </value>
        public static string SourceFileExtensions => CommonStaticLiterals.SourceFileExtensions;
        /// <summary>
        /// Gets the file extension for C# files.
        /// </summary>
        /// <value>
        /// The file extension for C# files.
        /// </value>
        public static string CSharpFileExtension => CommonStaticLiterals.CSharpFileExtension;
        /// <summary>
        /// Gets the HTML file extension for C# files.
        /// </summary>
        /// <value>
        /// A string representing the HTML file extension for C# files.
        /// </value>
        public static string CSharpHtmlFileExtension => $"{CommonStaticLiterals.CSharpFileExtension}html";
        /// <summary>
        /// Gets the label for generated code.
        /// </summary>
        /// <value>
        /// A string representing the label for generated code.
        /// </value>
        public static string GeneratedCodeLabel => CommonStaticLiterals.GeneratedCodeLabel;
        /// <summary>
        /// Gets or sets the customized and generated code label.
        /// </summary>
        /// <value>
        /// The customized and generated code label.
        /// </value>
        public static string CustomizedAndGeneratedCodeLabel => CommonStaticLiterals.CustomizedAndGeneratedCodeLabel;
        
        /// <summary>
        /// Gets or sets the beginning label for custom imports in Angular.
        /// </summary>
        /// <value>The beginning label for custom imports in Angular.</value>
        public static string AngularCustomImportBeginLabel => "//@CustomImportBegin";
        ///<summary>
        /// Gets or sets the end label for custom imports in Angular.
        ///</summary>
        public static string AngularCustomImportEndLabel => "//@CustomImportEnd";
        /// <summary>
        /// Gets or sets the Angular custom code begin label.
        /// </summary>
        /// <value>The Angular custom code begin label.</value>
        public static string AngularCustomCodeBeginLabel => "//@CustomCodeBegin";
        /// <summary>
        /// Gets the end label of custom code for Angular.
        /// </summary>
        /// <value>The end label of custom code for Angular.</value>
        public static string AngularCustomCodeEndLabel => "//@CustomCodeEnd";
        
        /// <summary>
        /// Gets or sets the alias for the entity.
        /// </summary>
        /// <value>
        /// The alias for the entity.
        /// </value>
        public static string EntityAlias => "TEntity";
        /// <summary>
        /// Gets or sets the alias for the model.
        /// </summary>
        /// <value>
        /// The alias for the model.
        /// </value>
        public static string ModelAlias => "TModel";
        #endregion Code-Generation
        
        #region Project Extensions
        /// <summary>
        /// Gets or sets the logic extension for the property.
        /// </summary>
        /// <remarks>
        /// The logic extension determines the file extension used for logic files.
        /// </remarks>
        /// <value>
        /// The logic extension as a string.
        /// </value>
        public static string LogicExtension => CommonStaticLiterals.LogicExtension;
        /// <summary>
        /// Gets the WebApiExtension property.
        /// This property represents the Web API extension.
        /// </summary>
        /// <value>
        /// A string that contains the Web API extension.
        /// </value>
        public static string WebApiExtension => CommonStaticLiterals.WebApiExtension;
        /// <summary>
        /// Gets the ASP.NET MVC extension.
        /// </summary>
        /// <value>The ASP.NET MVC extension.</value>
        public static string AspMvcExtension => CommonStaticLiterals.AspMvcExtension;
        /// <summary>
        /// Gets the MVVM extension.
        /// </summary>
        /// <value>
        /// The MVVM extension.
        /// </value>
        public static string MVVMExtension => CommonStaticLiterals.MVVMExtension;
        /// <summary>
        /// Gets the angular extension used by the application.
        /// </summary>
        /// <value>
        /// The angular extension.
        /// </value>
        public static string AngularExtension => CommonStaticLiterals.AngularExtension;
        /// <summary>
        /// Gets the extension for the Client Blazor.
        /// </summary>
        /// <value>
        /// The Client Blazor extension.
        /// </value>
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
        /// <summary>
        /// Gets an array of properties that should not go through the generation process.
        /// </summary>
        /// <value>
        /// An array of strings representing the properties that should not be generated.
        /// </value>
        public static string[] NoGenerationProperties => IdentityProperties.Union(VersionProperties).ToArray();
        #endregion Entity and service properties
        
        #region Model properties
        public static readonly string IdType = nameof(IdType);
        public static readonly string ModelObjectName = "ModelObject";
        public static readonly string VersionModelName = "VersionModel";
        public static readonly string ServiceModelName = "ServiceModel";
        #endregion Model properties
        
        public static readonly string[] ModelBaseClasses = new[]
        {
            ModelObjectName,
            VersionModelName
        };
        public static readonly Dictionary<string, string> BaseClassMapping = new()
        {
            { EntityObjectName, ModelObjectName },
            { VersionEntityName, VersionModelName },
            { ServiceModelName, $"BaseModels.{ServiceModelName}" },
        };
        
        #region Folders and Files
        /// <summary>
        /// Gets the folder path where entities are stored.
        /// </summary>
        /// <value>
        /// The folder path where entities are stored.
        /// </value>
        public static string EntitiesFolder => "Entities";
        /// <summary>
        /// Gets the name of the folder where the models are stored.
        /// </summary>
        /// <value>
        /// The name of the folder where the models are stored.
        /// </value>
        public static string ModelsFolder => "Models";
        /// <summary>
        /// Gets the path of the service models folder.
        /// </summary>
        /// <value>The path of the service models folder.</value>
        public static string ServiceModelsFolder => "ServiceModels";
        /// <summary>
        /// Gets or sets an array of module folders.
        /// </summary>
        /// <value>
        /// The array of module folders.
        /// </value>
        public static string[] ModuleFolders => new[] { EntitiesFolder, ModelsFolder, ServiceModelsFolder };
        
        /// <summary>
        /// Gets the folder name for the account.
        /// </summary>
        /// <value>
        /// The folder name for the account.
        /// </value>
        public static string AccountFolder => "Account";
        /// <summary>
        /// Gets or sets the folder where log files are stored.
        /// </summary>
        /// <value>
        /// The folder name for logging. Default value is "Logging".
        /// </value>
        public static string LoggingFolder => "Logging";
        /// <summary>
        /// Gets the folder name for revisions.
        /// </summary>
        /// <value>The folder name for revisions.</value>
        public static string RevisionFolder => "Revision";
        /// <summary>
        /// Gets the folder name for contracts.
        /// </summary>
        /// <value>
        /// The folder name for contracts.
        /// </value>
        public static string ContractsFolder => "Contracts";
        /// <summary>
        /// Gets the path of the DataContext folder.
        /// </summary>
        /// <value>
        /// The path of the DataContext folder.
        /// </value>
        public static string DataContextFolder => "DataContext";
        /// <summary>
        /// Gets the path to the Controllers folder.
        /// </summary>
        /// <value>
        /// The path to the Controllers folder.
        /// </value>
        public static string ControllersFolder => "Controllers";
        ///<summary>
        /// Gets or sets the folder name where services are stored.
        ///</summary>
        ///<value>
        /// A <see cref="System.String"/> representing the folder name.
        ///</value>
        public static string ServicesFolder => "Services";
        /// <summary>
        /// Gets or sets the folder name for facades.
        /// </summary>
        /// <value>
        /// The folder name for facades.
        /// </value>
        public static string FacadesFolder => "Facades";
        /// <summary>
        /// Gets the folder name where views are stored.
        /// </summary>
        /// <value>
        /// The folder name where views are stored.
        /// </value>
        /// <remarks>
        /// This property is used to specify the folder name where views are located.
        /// </remarks>
        public static string ViewsFolder => "Views";
        #endregion Folders and Files
        
        #region Settings
        /// <summary>
        /// Represents the name of the TProperty property as a string.
        /// </summary>
        /// <value>
        /// The name of the TProperty property.
        /// </value>
        public static string TProperty => nameof(TProperty);
        /// <summary>
        /// Gets the name of the Copy property.
        /// </summary>
        /// <value>
        /// The name of the Copy property.
        /// </value>
        public static string Copy => nameof(Copy);
        /// <summary>
        /// Gets the name of the property "Generate".
        /// </summary>
        /// <value>
        /// The name of the property "Generate".
        /// </value>
        /// <remarks>
        /// This property is used to obtain the name of the "Generate" property.
        /// </remarks>
        public static string Generate => nameof(Generate);
        /// <summary>
        /// Gets the name of the get accessor.
        /// </summary>
        public static string GetAccessor => nameof(GetAccessor);
        /// <summary>
        /// Gets the string value of the set accessor.
        /// </summary>
        /// <value>The name of the set accessor.</value>
        public static string SetAccessor => nameof(SetAccessor);
        /// <summary>
        /// Gets the name of the "Visibility" property.
        /// </summary>
        /// <value>
        /// The name of the "Visibility" property.
        /// </value>
        public static string Visibility => nameof(Visibility);
        /// <summary>
        /// Gets the name of the Attributes property.
        /// </summary>
        public static string Attributes => nameof(Attributes);
        /// <summary>
        /// Gets the name of the ControllerGenericType property.
        /// </summary>
        /// <value>
        /// The name of the ControllerGenericType property.
        /// </value>
        public static string ControllerGenericType => nameof(ControllerGenericType);
        /// <summary>
        /// Gets the name of the FacadeGenericType property.
        /// </summary>
        /// <value>A string representing the name of the FacadeGenericType property.</value>
        public static string FacadeGenericType => nameof(FacadeGenericType);
        /// <summary>
        /// Gets the name of the ServiceGenericType property.
        /// </summary>
        /// <value>
        /// The name of the ServiceGenericType property.
        /// </value>
        public static string ServiceGenericType => nameof(ServiceGenericType);
        #endregion Settings
        
        #region Modules
        /// <summary>
        /// Gets the name of the Account property.
        /// </summary>
        /// <value>
        /// The name of the Account property.
        /// </value>
        public static string Account => nameof(Account);
        /// <summary>
        /// Gets the string representation of the 'Access' property name.
        /// </summary>
        public static string Access => nameof(Access);
        /// <summary>
        /// Gets the name of the Logging property.
        /// </summary>
        /// <value>
        /// The name of the Logging property.
        /// </value>
        public static string Logging => nameof(Logging);
        /// <summary>
        /// Gets the name of the Revision property.
        /// </summary>
        /// <value>The name of the Revision property.</value>
        public static string Revision => nameof(Revision);
        /// <summary>
        /// Gets the name of the secure identity.
        /// </summary>
        /// <value>
        /// The secure identity name.
        /// </value>
        public static string SecureIdentity => nameof(SecureIdentity);
        /// <summary>
        /// Gets the login session property name.
        /// </summary>
        /// <value>The login session property name.</value>
        public static string LoginSession => nameof(LoginSession);
        #endregion Modules
    }
}
//MdEnd


