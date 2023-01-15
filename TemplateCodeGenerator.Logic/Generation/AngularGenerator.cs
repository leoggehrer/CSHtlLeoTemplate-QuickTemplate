//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic.Generation
{
    using System.IO;
    using System.Reflection;
    using System.Text;
    using TemplateCodeGenerator.Logic.Contracts;
    internal sealed partial class AngularGenerator : ClassGenerator
    {
        public bool GenerateEnums { get; set; }
        public bool GenerateModels { get; set; }
        public bool GenerateServices { get; set; }

        #region AngularApp-Definitions
        public static string CodeExtension => "ts";
        public static string EnumsSubFolder => Path.Combine("src", "app", "core", "enums", "gen");
        public static string ModelsSubFolder => Path.Combine("src", "app", "core", "models", "gen");
        public static string ServicesSubFolder => Path.Combine("src", "app", "core", "services", "http", "gen");

        public static string SourceNameSpace => "src";
        public static string ContractsNameSpace => $"{SourceNameSpace}.contracts";
        public static string CreateContractsNameSpace(Type type)
        {
            return $"{ContractsNameSpace}.{CreateSubNamespaceFromType(type)}".ToLower();
        }
        public static string CreateTypeScriptFullName(Type type)
        {
            type.CheckArgument(nameof(type));

            return $"{CreateContractsNameSpace(type)}.{(type.IsInterface ? ItemProperties.CreateEntityName(type) : type.Name)}";
        }
        #endregion AngularApp-Definitions

        public AngularGenerator(ISolutionProperties solutionProperties) : base(solutionProperties)
        {
            GenerateEnums = QuerySetting<bool>(Common.ItemType.TypeScriptEnum, "All", StaticLiterals.Generate, "True");
            GenerateModels = QuerySetting<bool>(Common.ItemType.TypeScriptModel, "All", StaticLiterals.Generate, "True");
            GenerateServices = QuerySetting<bool>(Common.ItemType.TypeScriptService, "All", StaticLiterals.Generate, "True");
        }
        private bool CanCreate(Type type)
        {
            bool create = EntityProject.IsNotAGenerationEntity(type) ? false : true;

            CanCreateModel(type, ref create);
            return create;
        }
        partial void CanCreateModel(Type type, ref bool create);

        public IEnumerable<IGeneratedItem> GenerateAll()
        {
            var result = new List<IGeneratedItem>();

            result.AddRange(CreateEnums());
            result.AddRange(CreateModels());
            result.AddRange(CreateServices());
            return result;
        }
        public IEnumerable<IGeneratedItem> CreateEnums()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);

            foreach (var type in entityProject.EnumTypes)
            {
                if (CanCreate(type) && QuerySetting<bool>(Common.ItemType.TypeScriptEnum, type, StaticLiterals.Generate, GenerateEnums.ToString()))
                {
                    result.Add(CreateEnumFromType(type));
                }
            }
            return result;
        }
        public IGeneratedItem CreateEnumFromType(Type type)
        {
            var subPath = ConvertFileItem(CreateSubPathFromType(type));
            var projectPath = Path.Combine(SolutionProperties.SolutionPath, SolutionProperties.AngularAppProjectName);
            var fileName = $"{ConvertFileItem(type.Name)}.{CodeExtension}";
            var result = new Models.GeneratedItem(Common.UnitType.Angular, Common.ItemType.TypeScriptEnum)
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
        partial void StartCreateEnum(Type type, List<string> lines);
        partial void FinishCreateEnum(Type type, List<string> lines);

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
        public IGeneratedItem CreateModelFromType(Type type, IEnumerable<Type> types)
        {
            var subPath = ConvertFileItem(CreateSubPathFromType(type));
            var projectPath = Path.Combine(SolutionProperties.SolutionPath, SolutionProperties.AngularAppProjectName);
            var entityName = ItemProperties.CreateEntityName(type);
            var fileName = $"{ConvertFileItem(entityName)}.{CodeExtension}";
            var typeProperties = type.GetAllPropertyInfos();
            var declarationTypeName = string.Empty;
            var result = new Models.GeneratedItem(Common.UnitType.Angular, Common.ItemType.TypeScriptModel)
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

            var imports = new List<string>();

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
        partial void StartCreateModel(Type type, List<string> lines);
        partial void FinishCreateModel(Type type, List<string> lines);

        private IEnumerable<IGeneratedItem> CreateServices()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);

            foreach (var type in entityProject.EntityTypes)
            {
                if (CanCreate(type) && QuerySetting<bool>(Common.ItemType.TypeScriptService, type, StaticLiterals.Generate, GenerateServices.ToString()))
                {
                    result.Add(CreateServiceFromType(type, Common.UnitType.Angular, Common.ItemType.TypeScriptService));
                }
            }
            return result;
        }
        private IGeneratedItem CreateServiceFromType(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var subPath = ConvertFileItem(CreateSubPathFromType(type));
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
        partial void StartCreateService(Type type, List<string> lines);
        partial void FinishCreateService(Type type, List<string> lines);

        private T QuerySetting<T>(Common.ItemType itemType, Type type, string valueName, string defaultValue)
        {
            T result;

            try
            {
                result = (T)Convert.ChangeType(QueryGenerationSettingValue(Common.UnitType.Angular, itemType, CreateEntitiesSubTypeFromType(type), valueName, defaultValue), typeof(T));
            }
            catch (Exception ex)
            {
                result = (T)Convert.ChangeType(defaultValue, typeof(T));
                System.Diagnostics.Debug.WriteLine($"Error in {System.Reflection.MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
            }
            return result;
        }
        private T QuerySetting<T>(Common.ItemType itemType, string itemName, string valueName, string defaultValue)
        {
            T result;

            try
            {
                result = (T)Convert.ChangeType(QueryGenerationSettingValue(Common.UnitType.Angular, itemType, itemName, valueName, defaultValue), typeof(T));
            }
            catch (Exception ex)
            {
                result = (T)Convert.ChangeType(defaultValue, typeof(T));
                System.Diagnostics.Debug.WriteLine($"Error in {System.Reflection.MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
            }
            return result;
        }

        #region Helpers
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
        public static string CreateImport(string alias, string typeName, string subPath)
        {
            return "import { " + typeName + " } from " + $"'{alias}/gen/{ConvertFileItem(subPath)}/{ConvertFileItem(typeName)}';";
        }
        public static void InsertTypeImports(IEnumerable<string> imports, List<string> lines)
        {
            foreach (var item in imports.Reverse().Distinct())
            {
                lines.Insert(0, item);
            }
        }

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
                        var subPath = GeneratorObject.CreateSubPathFromType(propertyInfo.PropertyType).ToLower();

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
                            var subPath = GeneratorObject.CreateSubPathFromType(modelType).ToLower();

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
                            var subPath = GeneratorObject.CreateSubPathFromType(modelType).ToLower();

                            result.Add(CreateImport("@app-core-models", modelName, subPath));
                        }
                    }
                }
            }
            return result.Distinct();
        }
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
                    var subPath = GeneratorObject.CreateSubPathFromType(other).ToLower();

                    result.Add(CreateImport("@app-core-models", refTypeName, subPath));
                }
            }
            return result.Distinct();
        }
        #endregion Helpers
    }
}
//MdEnd
