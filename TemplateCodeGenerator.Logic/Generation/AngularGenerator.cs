//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic.Generation
{
    using System.IO;
    using System.Reflection;
    using System.Text;
    using TemplateCodeGenerator.Logic.Contracts;
    /// <summary>
    /// Represents a class for generating Angular code items.
    /// </summary>
    internal sealed partial class AngularGenerator : ItemGenerator
    {
        /// <summary>
        /// Gets or sets a value indicating whether to generate enums.
        /// </summary>
        /// <value>
        ///   <c>true</c> if generating enums; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateEnums { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether models should be generated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if models should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateModels { get; set; }
        ///<summary>
        ///Gets or sets a value indicating whether services should be generated.
        ///</summary>
        /// <value>
        ///   <c>true</c> if services should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateServices { get; set; }
        
        #region AngularApp-Definitions
        /// <summary>
        /// Gets the code extension.
        /// </summary>
        /// <value>The code extension.</value>
        public static string CodeExtension => "ts";
        /// <summary>
        /// Gets the subfolder path for the generated enums.
        /// </summary>
        /// <value>
        /// The subfolder path containing the generated enums.
        /// </value>
        public static string EnumsSubFolder => Path.Combine("src", "app", "core", "enums", "gen");
        /// <summary>
        /// Gets the subfolder path for models under the core app.
        /// </summary>
        /// <value>
        /// The subfolder path for models under the core app.
        /// </value>
        public static string ModelsSubFolder => Path.Combine("src", "app", "core", "models", "gen");
        /// <summary>
        ///     Gets the subfolder path for the services in the application's core.
        /// </summary>
        /// <value>
        ///     A <see cref="string"/> representing the subfolder path for the services.
        /// </value>
        /// <remarks>
        ///     The subfolder path is obtained by combining the path elements "src", "app", "core", "services", "http", and "gen" using the <see cref="Path.Combine"/> method.
        /// </remarks>
        public static string ServicesSubFolder => Path.Combine("src", "app", "core", "services", "http", "gen");
        
        /// <summary>
        /// Gets or sets the source namespace.
        /// </summary>
        /// <value>The source namespace value.</value>
        public static string SourceNameSpace => "src";
        /// <summary>
        /// Gets the namespace for the contracts.
        /// </summary>
        /// <remarks>
        /// This property returns the namespace for the contracts, which is derived
        /// from the <see cref="SourceNameSpace"/> property.
        /// </remarks>
        /// <value>The namespace for the contracts.</value>
        public static string ContractsNameSpace => $"{SourceNameSpace}.contracts";
        /// <summary>
        /// Creates the namespace for the contracts based on the provided <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type used to generate the subnamespace.</param>
        /// <returns>The fully qualified namespace for the contracts.</returns>
        public static string CreateContractsNameSpace(Type type)
        {
            return $"{ContractsNameSpace}.{ItemProperties.CreateSubNamespaceFromType(type)}".ToLower();
        }
        /// <summary>
        /// Creates a fully qualified TypeScript name for a given type.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> object representing the type.</param>
        /// <returns>A string representing the full TypeScript name of the type.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is null.</exception>
        public static string CreateTypeScriptFullName(Type type)
        {
            type.CheckArgument(nameof(type));
            
            return $"{CreateContractsNameSpace(type)}.{(type.IsInterface ? ItemProperties.CreateEntityName(type) : type.Name)}";
        }
        #endregion AngularApp-Definitions
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AngularGenerator"/> class.
        /// </summary>
        /// <param name="solutionProperties">The solution properties.</param>
        public AngularGenerator(ISolutionProperties solutionProperties) : base(solutionProperties)
        {
            GenerateEnums = QuerySetting<bool>(Common.ItemType.TypeScriptEnum, "All", StaticLiterals.Generate, "True");
            GenerateModels = QuerySetting<bool>(Common.ItemType.TypeScriptModel, "All", StaticLiterals.Generate, "True");
            GenerateServices = QuerySetting<bool>(Common.ItemType.TypeScriptService, "All", StaticLiterals.Generate, "True");
        }
        /// <summary>
        /// Determines whether the specified type can be created.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the type can be created; otherwise, <c>false</c>.</returns>
        protected override bool CanCreate(Type type)
        {
            bool create = EntityProject.IsNotAGenerationEntity(type) == false;
            
            CanCreateModel(type, ref create);
            return create;
        }
        /// <summary>
        /// Determines if a model of the specified type can be created.
        /// </summary>
        /// <param name="type">The type of the model.</param>
        /// <param name="create">A reference to a boolean value indicating whether the model can be created.</param>
        /// <remarks>
        /// This method is called to determine if a model of the specified type can be created.
        /// The <paramref name="type"/> parameter represents the type of the model being checked.
        /// The <paramref name="create"/> parameter is a reference to a boolean value indicating whether the model can be created.
        /// If the create value is set to false, it means the model cannot be created; otherwise, it can be created.
        /// </remarks>
        partial void CanCreateModel(Type type, ref bool create);
        
        /// <summary>
        /// Generates all the generated items, including enums, models, and services.
        /// </summary>
        /// <returns>An enumerable collection of the generated items.</returns>
        public IEnumerable<IGeneratedItem> GenerateAll()
        {
            var result = new List<IGeneratedItem>();
            
            result.AddRange(CreateEnums());
            result.AddRange(CreateModels());
            result.AddRange(CreateServices());
            return result;
        }
        /// <summary>
        /// Creates a collection of generated items representing enums.
        /// </summary>
        /// <returns>A collection of generated items representing enums.</returns>
        public IEnumerable<IGeneratedItem> CreateEnums()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);
            
            foreach (var type in entityProject.EnumTypes)
            {
                if (CanCreate(type)
                && QuerySetting<bool>(Common.ItemType.TypeScriptEnum, type, StaticLiterals.Generate, GenerateEnums.ToString()))
                {
                    result.Add(CreateEnumFromType(type));
                }
            }
            return result;
        }
        /// <summary>
        /// Creates a TypeScript enum from a specified System.Type object.
        /// </summary>
        /// <param name="type">The System.Type object to create the enum from.</param>
        /// <returns>An instance of the IGeneratedItem interface representing the created enum.</returns>
        public IGeneratedItem CreateEnumFromType(Type type)
        {
            var subPath = ConvertFileItem(ItemProperties.CreateSubPathFromType(type));
            var projectPath = Path.Combine(SolutionProperties.SolutionPath, SolutionProperties.AngularAppProjectName);
            var fileName = $"{ConvertFileItem(type.Name)}.{CodeExtension}";
            var result = new Models.GeneratedItem(Common.UnitType.AngularApp, Common.ItemType.TypeScriptEnum)
            {
                FullName = CreateTypeScriptFullName(type),
                FileExtension = CodeExtension,
                SubFilePath = Path.Combine(EnumsSubFolder, subPath, fileName),
            };
            
            StartCreateEnum(type, result.Source);
            result.Add($"export enum {type.Name}" + " {");
            
            foreach (var item in Enum.GetNames(type))
            {
                var value = Enum.Parse(type, item);
                
                result.Add($"{item} = {(int)value},");
            }
            
            result.Add("}");
            
            result.Source.Insert(result.Source.Count - 1, StaticLiterals.AngularCustomCodeBeginLabel);
            result.Source.InsertRange(result.Source.Count - 1, ReadCustomCode(projectPath, result));
            result.Source.Insert(result.Source.Count - 1, StaticLiterals.AngularCustomCodeEndLabel);
            
            result.AddRange(result.Source.Eject().Distinct());
            result.FormatCSharpCode();
            FinishCreateEnum(type, result.Source);
            return result;
        }
        /// <summary>
        ///   Starts the creation of an enum based on the provided <paramref name="type"/> and <paramref name="lines"/>.
        /// </summary>
        /// <param name="type">The <see cref="System.Type"/> representing the enum type.</param>
        /// <param name="lines">The list of string values representing the enum values.</param>
        /// <remarks>
        ///    This method is used to initiate the creation of an enum by providing the enum <paramref name="type"/> and relevant <paramref name="lines"/>.
        ///    It should be implemented partially by other methods which handle the actual creation process.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown if <paramref name="type"/> is null.
        ///   Thrown if <paramref name="lines"/> is null.
        /// </exception>
        partial void StartCreateEnum(Type type, List<string> lines);
        /// <summary>
        /// Concludes the process of creating an enumeration by performing additional operations on the generated code.
        /// </summary>
        /// <param name="type">The Type object representing the enumeration.</param>
        /// <param name="lines">A List of strings containing the generated code lines for the enumeration.</param>
        /// <remarks>
        /// This method is intended for internal use within the code generation process and should not be called directly.
        /// </remarks>
        /// <seealso cref="CreateEnum(Type)"/>
        /// <seealso cref="GenerateCode(Type)"/>
        partial void FinishCreateEnum(Type type, List<string> lines);
        
        /// <summary>
        /// Creates models based on entity types in the entity project.
        /// </summary>
        /// <returns>An enumerable collection of generated items.</returns>
        public IEnumerable<IGeneratedItem> CreateModels()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);
            
            foreach (var type in entityProject.EntityTypes)
            {
                if (CanCreate(type)
                && QuerySetting<bool>(Common.ItemType.TypeScriptModel, type, StaticLiterals.Generate, GenerateModels.ToString()))
                {
                    result.Add(CreateModelFromType(type, entityProject.EntityTypes));
                }
            }
            return result;
        }
        /// <summary>
        /// Creates a TypeScript model from a given C# type.
        /// </summary>
        /// <param name="type">The C# type to create the model from.</param>
        /// <param name="types">Additional types used for property generation.</param>
        /// <returns>The generated TypeScript model.</returns>
        public IGeneratedItem CreateModelFromType(Type type, IEnumerable<Type> types)
        {
            var subPath = ConvertFileItem(ItemProperties.CreateSubPathFromType(type));
            var projectPath = Path.Combine(SolutionProperties.SolutionPath, SolutionProperties.AngularAppProjectName);
            var entityName = ItemProperties.CreateEntityName(type);
            var fileName = $"{ConvertFileItem(entityName)}.{CodeExtension}";
            var typeProperties = type.GetAllPropertyInfos();
            var declarationTypeName = string.Empty;
            var result = new Models.GeneratedItem(Common.UnitType.AngularApp, Common.ItemType.TypeScriptModel)
            {
                FullName = CreateTypeScriptFullName(type),
                FileExtension = CodeExtension,
                SubFilePath = Path.Combine(ModelsSubFolder, subPath, fileName),
            };
            
            StartCreateModel(type, result.Source);
            result.Add($"export interface {entityName} extends IVersionEntity" + " {");
            
            foreach (var item in typeProperties)
            {
                if (declarationTypeName.Equals(item.DeclaringType!.Name) == false)
                {
                    declarationTypeName = item.DeclaringType.Name;
                }
                result.AddRange(CreateTypeScriptProperty(item, types));
            }
            
            result.Add("}");
            
            result.Source.Insert(result.Source.Count - 1, StaticLiterals.AngularCustomCodeBeginLabel);
            result.Source.InsertRange(result.Source.Count - 1, ReadCustomCode(projectPath, result));
            result.Source.Insert(result.Source.Count - 1, StaticLiterals.AngularCustomCodeEndLabel);
            
            #pragma warning disable IDE0028 // Simplify collection initialization
            var imports = new List<string>();
            #pragma warning restore IDE0028 // Simplify collection initialization
            
            imports.Add("import { IVersionEntity } from '@app-core-models/i-version-entity';");
            imports.AddRange(CreateTypeImports(type, types));
            imports.AddRange(CreateModelToModelImports(type, types));
            imports.Add(StaticLiterals.AngularCustomImportBeginLabel);
            imports.AddRange(ReadCustomImports(projectPath, result));
            imports.Add(StaticLiterals.AngularCustomImportEndLabel);
            
            InsertTypeImports(imports, result.Source);
            FinishCreateModel(type, result.Source);
            return result;
        }
        /// <summary>
        /// Starts the creation of a model.
        /// </summary>
        /// <param name="type">The type of the model.</param>
        /// <param name="lines">The list of strings used to create the model.</param>
        /// <remarks>
        /// This method is declared as 'partial' which means it can be implemented in separate files,
        /// allowing for modular code organization.
        /// </remarks>
        partial void StartCreateModel(Type type, List<string> lines);
        /// <summary>
        /// Finishes creating the model with the provided <paramref name="type"/> and <paramref name="lines"/>.
        /// </summary>
        /// <param name="type">The specified type used for creating the model.</param>
        /// <param name="lines">The list of strings used for creating the model.</param>
        /// <remarks>
        /// This method is partially implemented and needs to be completed in another class file.
        /// </remarks>
        partial void FinishCreateModel(Type type, List<string> lines);
        
        /// <summary>
        /// Creates a collection of generated items representing services.
        /// </summary>
        /// <returns>A collection of <see cref="IGeneratedItem"/> representing services.</returns>
        private IEnumerable<IGeneratedItem> CreateServices()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);
            
            foreach (var type in entityProject.EntityTypes)
            {
                if (CanCreate(type) && QuerySetting<bool>(Common.ItemType.TypeScriptService, type, StaticLiterals.Generate, GenerateServices.ToString()))
                {
                    result.Add(CreateServiceFromType(type, Common.UnitType.AngularApp, Common.ItemType.TypeScriptService));
                }
            }
            return result;
        }
        /// <summary>
        /// Creates a service from the specified type.
        /// </summary>
        /// <param name="type">The type from which the service is created.</param>
        /// <param name="unitType">The unit type.</param>
        /// <param name="itemType">The item type.</param>
        /// <returns>The generated item representing the service.</returns>
        private IGeneratedItem CreateServiceFromType(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var subPath = ConvertFileItem(ItemProperties.CreateSubPathFromType(type));
            var projectPath = Path.Combine(SolutionProperties.SolutionPath, SolutionProperties.AngularAppProjectName);
            var entityName = ItemProperties.CreateEntityName(type);
            var fileName = $"{ConvertFileItem($"{entityName}Service")}.{CodeExtension}";
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = CreateTypeScriptFullName(type),
                FileExtension = CodeExtension,
                SubFilePath = Path.Combine(ServicesSubFolder, subPath, fileName),
            };
            
            StartCreateService(type, result.Source);
            result.Add("import { HttpClient } from '@angular/common/http';");
            result.Add("import { Injectable } from '@angular/core';");
            result.Add("import { ApiBaseService } from '@app-core/services/api-base.service';");
            result.Add("import { environment } from '@environment/environment';");
            result.Add(CreateImport("@app-core-models", entityName, subPath));
            
            result.Add(StaticLiterals.AngularCustomImportBeginLabel);
            result.AddRange(ReadCustomImports(projectPath, result));
            result.Add(StaticLiterals.AngularCustomImportEndLabel);
            
            result.Add("@Injectable({");
            result.Add("  providedIn: 'root',");
            result.Add("})");
            result.Add($"export class {entityName}Service extends ApiBaseService<{entityName}>" + " {");
            result.Add("  constructor(public override http: HttpClient) {");
            result.Add($"    super(http, environment.API_BASE_URL + '/{entityName.CreatePluralWord().ToLower()}');");
            result.Add("  }");
            result.Add("}");
            
            result.Source.Insert(result.Source.Count - 1, StaticLiterals.AngularCustomCodeBeginLabel);
            result.Source.InsertRange(result.Source.Count - 1, ReadCustomCode(projectPath, result));
            result.Source.Insert(result.Source.Count - 1, StaticLiterals.AngularCustomCodeEndLabel);
            FinishCreateService(type, result.Source);
            return result;
        }
        /// <summary>
        /// Starts the process of creating a service of the specified type, using the given list of lines.
        /// </summary>
        /// <param name="type">The type of service to be created.</param>
        /// <param name="lines">The list of lines to be used for creating the service.</param>
        partial void StartCreateService(Type type, List<string> lines);
        /// <summary>
        /// FinishCreateService is a method that finishes the creation of a service with the specified type and lines.
        /// </summary>
        /// <param name="type">The Type object representing the type of the service.</param>
        /// <param name="lines">A List of strings containing the lines of the service.</param>
        partial void FinishCreateService(Type type, List<string> lines);
        
        /// <summary>
        /// Queries a setting value of type <typeparamref name="T"/> from a specific item type, using the specified value name and default value.
        /// </summary>
        /// <typeparam name="T">The type of the setting value to be queried.</typeparam>
        /// <param name="itemType">The itemType of the setting.</param>
        /// <param name="type">The type of the setting.</param>
        /// <param name="valueName">The name of the setting value to be queried.</param>
        /// <param name="defaultValue">The default value to be used if the queried setting value is not found or cannot be converted to type <typeparamref name="T"/>.</param>
        /// <returns>The queried setting value of type <typeparamref name="T"/> or the default value if the queried setting value is not found or cannot be converted to type <typeparamref name="T"/>.</returns>
        private T QuerySetting<T>(Common.ItemType itemType, Type type, string valueName, string defaultValue)
        {
            T result;
            
            try
            {
                result = (T)Convert.ChangeType(QueryGenerationSettingValue(Common.UnitType.AngularApp, itemType, ItemProperties.CreateSubTypeFromEntity(type), valueName, defaultValue), typeof(T));
            }
            catch (Exception ex)
            {
                result = (T)Convert.ChangeType(defaultValue, typeof(T));
                System.Diagnostics.Debug.WriteLine($"Error in {System.Reflection.MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
            }
            return result;
        }
        /// <summary>
        ///   Queries a setting value and converts it to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to which the setting value will be converted.</typeparam>
        /// <param name="itemType">The type of item for which the setting is being queried.</param>
        /// <param name="itemName">The name of the item for which the setting is being queried.</param>
        /// <param name="valueName">The name of the setting being queried.</param>
        /// <param name="defaultValue">The default value to be used if the setting value cannot be queried or converted.</param>
        /// <returns>
        ///   The queried setting value converted to the specified type, or the default value if an error occurs.
        /// </returns>
        /// <remarks>
        ///   If querying or converting the setting value throws an exception, the default value will be used
        ///   and an error message will be written to the debug output.
        /// </remarks>
        private T QuerySetting<T>(Common.ItemType itemType, string itemName, string valueName, string defaultValue)
        {
            T result;
            
            try
            {
                result = (T)Convert.ChangeType(QueryGenerationSettingValue(Common.UnitType.AngularApp, itemType, itemName, valueName, defaultValue), typeof(T));
            }
            catch (Exception ex)
            {
                result = (T)Convert.ChangeType(defaultValue, typeof(T));
                System.Diagnostics.Debug.WriteLine($"Error in {System.Reflection.MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
            }
            return result;
        }
        
        #region Helpers
        /// <summary>
        /// Reads the custom imports from a source file and returns them as a sequence of strings.
        /// </summary>
        /// <param name="sourcePath">The path to the source file directory.</param>
        /// <param name="generatedItem">The generated item.</param>
        /// <returns>A sequence of strings representing the custom imports.</returns>
        public static IEnumerable<string> ReadCustomImports(string sourcePath, Models.GeneratedItem generatedItem)
        {
            var result = new List<string>();
            var sourceFilePath = Path.Combine(sourcePath, generatedItem.SubFilePath);
            var customFilePath = FileHandler.CreateCustomFilePath(sourceFilePath);
            
            if (File.Exists(sourceFilePath))
            {
                result.AddRange(FileHandler.ReadAngularCustomImports(sourceFilePath));
            }
            else
            {
                result.AddRange(FileHandler.ReadAngularCustomImports(customFilePath));
            }
            return result.Where(l => string.IsNullOrEmpty(l.Trim()) == false);
        }
        /// <summary>
        /// Reads custom code from a source file and a generated item.
        /// </summary>
        /// <param name="sourcePath">The path to the source file.</param>
        /// <param name="generatedItem">The generated item representing the file.</param>
        /// <returns>An enumerable collection of strings containing the custom code.</returns>
        public static IEnumerable<string> ReadCustomCode(string sourcePath, Models.GeneratedItem generatedItem)
        {
            var result = new List<string>();
            var sourceFilePath = Path.Combine(sourcePath, generatedItem.SubFilePath);
            var customFilePath = FileHandler.CreateCustomFilePath(sourceFilePath);
            
            if (File.Exists(sourceFilePath))
            {
                result.AddRange(FileHandler.ReadAngularCustomCode(sourceFilePath));
            }
            else
            {
                result.AddRange(FileHandler.ReadAngularCustomCode(customFilePath));
            }
            return result.Where(l => string.IsNullOrEmpty(l.Trim()) == false);
        }
        /// <summary>
        /// Converts a file item into a normalized format.
        /// </summary>
        /// <param name="fileItem">The file item to be converted.</param>
        /// <returns>A string representing the normalized file item.</returns>
        public static string ConvertFileItem(string fileItem)
        {
            var result = new StringBuilder();
            
            foreach (var item in fileItem)
            {
                if (result.Length == 0)
                {
                    result.Append(Char.ToLower(item));
                }
                else if (item == '\\')
                {
                    result.Append('/');
                }
                else if (Char.IsUpper(item))
                {
                    if (result[^1] != '/' && result[^1] != '\\')
                    {
                        result.Append('-');
                    }
                    result.Append(Char.ToLower(item));
                }
                else
                {
                    result.Append(Char.ToLower(item));
                }
            }
            return result.ToString();
        }
        /// <summary>
        /// Creates an import statement for a given alias, typeName, and subPath.
        /// </summary>
        /// <param name="alias">The alias to be used for the import statement.</param>
        /// <param name="typeName">The type name to be imported.</param>
        /// <param name="subPath">The sub path where the file is located.</param>
        /// <returns>The import statement string.</returns>
        public static string CreateImport(string alias, string typeName, string subPath)
        {
            return "import { " + typeName + " } from " + $"'{alias}/gen/{ConvertFileItem(subPath)}/{ConvertFileItem(typeName)}';";
        }
        /// <summary>
        /// Inserts a collection of import statements into a list of lines.
        /// </summary>
        /// <param name="imports">The collection of import statements to be inserted.</param>
        /// <param name="lines">The list of lines to insert the import statements into.</param>
        /// <remarks>
        /// The import statements will be inserted at the beginning of the list in the reverse order
        /// of the original collection, with duplicate statements removed.
        /// </remarks>
        public static void InsertTypeImports(IEnumerable<string> imports, List<string> lines)
        {
            foreach (var item in imports.Reverse().Distinct())
            {
                lines.Insert(0, item);
            }
        }
        
        /// <summary>
        /// Creates a list of type imports based on the provided type and a collection of types.
        /// </summary>
        /// <param name="type">The type for which type imports are being created.</param>
        /// <param name="types">The collection of types used to create type imports.</param>
        /// <returns>A distinct collection of type imports.</returns>
        public static IEnumerable<string> CreateTypeImports(Type type, IEnumerable<Type> types)
        {
            var result = new List<string>();
            var typeProperties = type.GetAllPropertyInfos();
            var entityName = ItemProperties.CreateEntityName(type);
            
            foreach (var propertyInfo in typeProperties)
            {
                if (propertyInfo.PropertyType.IsEnum)
                {
                    var typeName = $"{propertyInfo.PropertyType.Name}";
                    
                    if (typeName.Equals(entityName) == false)
                    {
                        var subPath = ConvertFileItem(ItemProperties.CreateSubPathFromType(propertyInfo.PropertyType));
                        
                        result.Add(CreateImport("@app-core-enums", typeName, subPath));
                    }
                }
                else if (propertyInfo.PropertyType.IsGenericType)
                {
                    var subType = propertyInfo.PropertyType.GetGenericArguments().First();
                    var modelType = types.FirstOrDefault(e => e.FullName == subType.FullName);
                    
                    if (modelType != null && modelType.IsClass)
                    {
                        var modelName = ItemProperties.CreateEntityName(modelType);
                        
                        if (modelName.Equals(entityName) == false)
                        {
                            var subPath = ConvertFileItem(ItemProperties.CreateSubPathFromType(modelType));
                            
                            result.Add(CreateImport("@app-core-models", modelName, subPath));
                        }
                    }
                }
                else if (propertyInfo.PropertyType.IsClass)
                {
                    var modelType = types.FirstOrDefault(e => e.FullName == propertyInfo.PropertyType.FullName);
                    
                    if (modelType != null && modelType.IsClass)
                    {
                        var modelName = ItemProperties.CreateEntityName(modelType);
                        
                        if (modelName.Equals(entityName) == false)
                        {
                            var subPath = ConvertFileItem(ItemProperties.CreateSubPathFromType(modelType));
                            
                            result.Add(CreateImport("@app-core-models", modelName, subPath));
                        }
                    }
                }
            }
            return result.Distinct();
        }
        /// <summary>
        /// Creates TypeScript properties based on the given PropertyInfo and types.
        /// </summary>
        /// <param name="propertyInfo">The PropertyInfo object.</param>
        /// <param name="types">The collection of types.</param>
        /// <returns>An IEnumerable of strings containing the TypeScript properties.</returns>
        public static IEnumerable<string> CreateTypeScriptProperty(PropertyInfo propertyInfo, IEnumerable<Type> types)
        {
            var result = new List<string>();
            var tsPropertyName = ItemProperties.CreateTSPropertyName(propertyInfo);
            var navigationType = types.FirstOrDefault(t => t.FullName!.Equals(propertyInfo.PropertyType.FullName));
            
            if (navigationType != null)
            {
                result.Add($"  {tsPropertyName}: {ItemProperties.CreateEntityName(navigationType)};");
            }
            else if (propertyInfo.PropertyType.IsEnum)
            {
                var enumName = $"  {propertyInfo.PropertyType.Name}";
                
                result.Add($"  {tsPropertyName}: {enumName};");
            }
            else if (propertyInfo.PropertyType == typeof(DateTime)
            || propertyInfo.PropertyType == typeof(DateTime?))
            {
                result.Add($"  {tsPropertyName}: Date;");
            }
            else if (propertyInfo.PropertyType == typeof(string))
            {
                result.Add($"  {tsPropertyName}: string;");
            }
            else if (propertyInfo.PropertyType == typeof(Guid))
            {
                result.Add($"  {tsPropertyName}: string;");
            }
            else if (propertyInfo.PropertyType == typeof(bool))
            {
                result.Add($" {tsPropertyName}: boolean;");
            }
            else if (propertyInfo.PropertyType.IsNumericType())
            {
                result.Add($"  {tsPropertyName}: number;");
            }
            else if (propertyInfo.PropertyType.IsGenericType)
            {
                Type subType = propertyInfo.PropertyType.GetGenericArguments().First();
                
                if (subType.IsInterface)
                {
                    result.Add($"  {tsPropertyName}: {subType.Name[1..]}[];");
                }
                else if (subType == typeof(Guid))
                {
                    result.Add($"  {tsPropertyName}: string;");
                }
                else
                {
                    result.Add($"  {tsPropertyName}: {subType.Name}[];");
                }
            }
            else if (propertyInfo.PropertyType.IsInterface)
            {
                result.Add($"  {tsPropertyName}: {propertyInfo.PropertyType.Name[1..]};");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Unknown property type: {propertyInfo.PropertyType.FullName}");
            }
            return result;
        }
        /// <summary>
        /// Creates model to model imports for a given type and a collection of types.
        /// </summary>
        /// <param name="type">The type for which model to model imports are created.</param>
        /// <param name="types">The collection of types to search for model to model imports.</param>
        /// <returns>An IEnumerable collection of strings representing the model to model imports.</returns>
        private static IEnumerable<string> CreateModelToModelImports(Type type, IEnumerable<Type> types)
        {
            var result = new List<string>();
            var typeName = ItemProperties.CreateEntityName(type);
            
            foreach (var pi in type.GetProperties())
            {
                var other = types.FirstOrDefault(t => t == pi.PropertyType);
                
                if (other != null && other != type)
                {
                    var refTypeName = ItemProperties.CreateEntityName(other);
                    var subPath = ConvertFileItem(ItemProperties.CreateSubPathFromType(other));
                    
                    result.Add(CreateImport("@app-core-models", refTypeName, subPath));
                }
            }
            return result.Distinct();
        }
        #endregion Helpers
    }
}
//MdEnd

