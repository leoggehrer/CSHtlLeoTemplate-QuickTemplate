//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic.Generation
{
    using System.Reflection;
    using TemplateCodeGenerator.Logic.Common;
    using TemplateCodeGenerator.Logic.Contracts;
    internal abstract partial class ModelGenerator : ClassGenerator
    {
        protected abstract ItemProperties ItemProperties { get; }

        protected ModelGenerator(ISolutionProperties solutionProperties)
            : base(solutionProperties)
        {
        }

        #region overrides
        protected override string GetPropertyType(PropertyInfo propertyInfo)
        {
            var propertyType = base.GetPropertyType(propertyInfo);
            var result = ItemProperties.ConvertEntityToModelType(propertyType);

            return ConvertPropertyType(result);
        }
        protected override string CopyProperty(string copyType, PropertyInfo propertyInfo)
        {
            string? result = null;

            if (StaticLiterals.VersionProperties.Any(vp => vp.Equals(propertyInfo.Name)) == false
                && copyType.Equals(propertyInfo.DeclaringType!.FullName, StringComparison.CurrentCultureIgnoreCase) == false)
            {
                if (IsArrayType(propertyInfo.PropertyType))
                {
                    var modelType = ItemProperties.ConvertEntityToModelType(propertyInfo.PropertyType.GetElementType()!.FullName!);

                    result = $"{propertyInfo.Name} = other.{propertyInfo.Name}.Select(e => {modelType}.Create((object)e)).ToArray();";
                }
                else if (IsListType(propertyInfo.PropertyType))
                {
                    var modelType = ItemProperties.ConvertEntityToModelType(propertyInfo.PropertyType.GenericTypeArguments[0].FullName!);

                    result = $"{propertyInfo.Name} = other.{propertyInfo.Name}.Select(e => {modelType}.Create((object)e)).ToList();";
                }
                else if (ItemProperties.IsEntityType(propertyInfo.PropertyType))
                {
                    var modelType = ItemProperties.ConvertEntityToModelType(propertyInfo.PropertyType.FullName!);

                    result = $"{propertyInfo.Name} = other.{propertyInfo.Name} != null ? {modelType}.Create((object)other.{propertyInfo.Name}) : null;";
                }
            }
            return result ?? base.CopyProperty(copyType, propertyInfo);
        }
        protected override string CopyDelegateProperty(string copyType, PropertyInfo propertyInfo)
        {
            string? result = null;

            if (StaticLiterals.VersionProperties.Any(vp => vp.Equals(propertyInfo.Name)) == false
                && copyType.Equals(propertyInfo.DeclaringType!.FullName, StringComparison.CurrentCultureIgnoreCase) == false)
            {
                if (IsArrayType(propertyInfo.PropertyType))
                {
                    var modelType = ItemProperties.ConvertEntityToModelType(propertyInfo.PropertyType.GetElementType()!.FullName!);

                    result = $"{propertyInfo.Name} = other.{propertyInfo.Name}.Select(e => {modelType}.Create((object)e)).ToArray();";
                }
                else if (IsListType(propertyInfo.PropertyType))
                {
                    result = string.Empty;
                }
                else if (ItemProperties.IsEntityType(propertyInfo.PropertyType))
                {
                    var modelType = ItemProperties.ConvertEntityToModelType(propertyInfo.PropertyType.FullName!);

                    result = $"{propertyInfo.Name} = other.{propertyInfo.Name} != null ? {modelType}.Create((object)other.{propertyInfo.Name}) : null;";
                }
            }
            return result ?? base.CopyProperty(copyType, propertyInfo);
        }
        #endregion overrides

        #region can creates
        protected virtual bool CanCreate(Type type)
        {
            bool create = EntityProject.IsNotAGenerationEntity(type) == false;

            CanCreateModel(type, ref create);
            return create;
        }
        protected virtual bool CanCreate(PropertyInfo propertyInfo)
        {
            bool create = true;

            CanCreateProperty(propertyInfo, ref create);
            return create;
        }
        partial void CanCreateModel(Type type, ref bool create);
        partial void CanCreateProperty(PropertyInfo propertyInfo, ref bool create);
        #endregion can creates

        #region create attributes
        partial void CreateModelAttributes(Type type, List<string> codeLines);
        protected virtual void CreateModelPropertyAttributes(PropertyInfo propertyInfo, UnitType unitType, List<string> codeLines)
        {
            var handled = false;

            BeforeCreateModelPropertyAttributes(propertyInfo, unitType, codeLines, ref handled);
            if (handled == false)
            {
                var itemName = $"{propertyInfo.DeclaringType!.Name}.{propertyInfo.Name}";
                var attributes = QuerySetting<string>(unitType, ItemType.ModelProperty, itemName, StaticLiterals.Attributes, string.Empty);

                if (string.IsNullOrEmpty(attributes) == false)
                {
                    codeLines.Add($"[{attributes}]");
                }
            }
            AfterCreateModelPropertyAttributes(propertyInfo, unitType, codeLines);
        }
        partial void BeforeCreateModelPropertyAttributes(PropertyInfo propertyInfo, UnitType unitType, List<string> codeLines, ref bool handled);
        partial void AfterCreateModelPropertyAttributes(PropertyInfo propertyInfo, UnitType unitType, List<string> codeLines);
        #endregion create attributes

        #region converters
        protected virtual string ConvertModelName(string modelName) => modelName;
        protected virtual string ConvertModelSubType(string modelSubType) => modelSubType;
        protected virtual string ConvertModelNamespace(string modelNamespace) => modelNamespace;
        protected virtual string ConvertModelFullName(string modelFullName) => modelFullName;
        protected virtual string ConvertModelSubPath(string modelSubPath) => modelSubPath;
        protected virtual string ConvertModelBaseType(string modelBaseType) => modelBaseType;
        #endregion converters

        protected virtual IGeneratedItem CreateModelFromType(Type type, UnitType unitType, ItemType itemType)
        {
            var modelName = ConvertModelName(CreateModelName(type));
            var modelSubType = ConvertModelSubType(ItemProperties.CreateModelSubType(type));
            var modelNamespace = ConvertModelNamespace(ItemProperties.CreateModelNamespace(type));
            var modelFullName = ConvertModelFullName(CreateModelFullName(type));
            var modelSubFilePath = ConvertModelSubPath(ItemProperties.CreateModelSubPath(type, string.Empty, StaticLiterals.CSharpFileExtension));
            var visibility = QuerySetting<string>(unitType, itemType, type, StaticLiterals.Visibility, "public");
            var attributes = QuerySetting<string>(unitType, itemType, type, StaticLiterals.Attributes, string.Empty);
            var typeProperties = type.GetAllPropertyInfos();
            var generateProperties = typeProperties.Where(e => StaticLiterals.NoGenerationProperties.Any(p => p.Equals(e.Name)) == false) ?? Array.Empty<PropertyInfo>();
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = modelFullName,
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = modelSubFilePath,
            };
            result.AddRange(CreateComment(type));
            CreateModelAttributes(type, result.Source);
            result.Add($"{(attributes.HasContent() ? $"[{attributes}]" : string.Empty)}");
            result.Add($"{visibility} partial class {modelName}");
            result.Add("{");
            result.AddRange(CreatePartialStaticConstrutor(modelName));
            result.AddRange(CreatePartialConstrutor("public", modelName));

            foreach (var propertyInfo in generateProperties)
            {
                if (CanCreate(propertyInfo)
                    && QuerySetting<bool>(unitType, ItemType.ModelProperty, type, StaticLiterals.Generate, "True"))
                {
                    result.AddRange(CreateComment(propertyInfo));
                    CreateModelPropertyAttributes(propertyInfo, unitType, result.Source);
                    result.AddRange(CreateProperty(type, propertyInfo));
                }
            }

            var lambda = QuerySetting<string>(unitType, itemType, type, ItemType.Lambda.ToString(), string.Empty);

            if (lambda.HasContent())
            {
                result.Add($"{lambda};");
            }

            if (unitType == UnitType.Logic)
            {
                var copyType = type.FullName!;
                var modelType = ItemProperties.CreateModelType(type);

                result.AddRange(CreateFactoryMethod(false, ItemProperties.CreateModelType(type)));
                result.AddRange(CreateFactoryMethod(false, modelType, copyType));
                result.AddRange(CreateCopyProperties("internal", type, copyType));
                result.AddRange(CreateCopyProperties("public", type, modelType));
            }
            else if (unitType == UnitType.WebApi)
            {
                result.AddRange(CreateFactoryMethod(false, ItemProperties.CreateModelType(type)));
                if (type.IsPublic)
                {
                    var logicProject = $"{ItemProperties.SolutionName}{StaticLiterals.LogicExtension}";
                    var copyType = $"{logicProject}.{ItemProperties.CreateModelSubType(type)}";
                    var modelType = ItemProperties.CreateModelType(type);

                    result.AddRange(CreateFactoryMethod(false, modelType, copyType));
                    result.AddRange(CreateCopyProperties("public", type, copyType));
                    result.AddRange(CreateCopyProperties("public", type, modelType));
                }
                else
                {
                    var logicProject = $"{ItemProperties.SolutionName}{StaticLiterals.LogicExtension}";
                    var copyType = $"{logicProject}.{ItemProperties.CreateModelSubType(type)}";

                    result.AddRange(CreateCopyProperties("public", type, copyType));
                }
            }
            else if (unitType == UnitType.AspMvc)
            {
                result.AddRange(CreateFactoryMethod(false, ItemProperties.CreateModelType(type)));
                if (type.IsPublic)
                {
                    var logicProject = $"{ItemProperties.SolutionName}{StaticLiterals.LogicExtension}";
                    var copyType = $"{logicProject}.{ItemProperties.CreateModelSubType(type)}";
                    var modelType = ItemProperties.CreateModelType(type);

                    result.AddRange(CreateFactoryMethod(false, modelType, copyType));
                    result.AddRange(CreateCopyProperties("public", type, copyType));
                    result.AddRange(CreateCopyProperties("public", type, modelType));
                }
                else
                {
                    var logicProject = $"{ItemProperties.SolutionName}{StaticLiterals.LogicExtension}";
                    var copyType = $"{logicProject}.{ItemProperties.CreateModelSubType(type)}";

                    result.AddRange(CreateCopyProperties("public", type, copyType));
                }
            }
            else if (unitType == UnitType.ClientBlazor)
            {
            }
            else
            {
                result.AddRange(CreateFactoryMethod(false, ItemProperties.CreateModelType(type)));
            }
            result.AddRange(CreateEquals(type, modelSubType));
            result.AddRange(CreateGetHashCode(type));
            result.Add("}");
            result.EnvelopeWithANamespace(modelNamespace, "using System;");
            result.FormatCSharpCode();
            return result;
        }
        protected virtual IGeneratedItem CreateModelInheritance(Type type, UnitType unitType, ItemType itemType)
        {
            var modelName = ConvertModelName(CreateModelName(type));
            var modelNamespace = ConvertModelNamespace(ItemProperties.CreateModelNamespace(type));
            var modelFullName = ConvertModelFullName(CreateModelFullName(type));
            var modelSubFilePath = ConvertModelSubPath(ItemProperties.CreateModelSubPath(type, "Inheritance", StaticLiterals.CSharpFileExtension));
            var modelBaseType = ConvertModelBaseType(GetBaseClassByType(type));
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = modelFullName,
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = modelSubFilePath,
            };
            result.Source.Add($"partial class {modelName} : {modelBaseType}");
            result.Source.Add("{");
            result.Source.Add("}");
            result.EnvelopeWithANamespace(modelNamespace);
            result.FormatCSharpCode();
            return result;
        }
        protected virtual IGeneratedItem CreateDelegateModelFromType(Type type, UnitType unitType, ItemType itemType)
        {
            var modelName = CreateModelName(type);
            var modelType = ItemProperties.CreateModelType(type);
            var modelSubType = ItemProperties.CreateModelSubType(type);
            var entitySubType = ItemProperties.CreateSolutionTypeSubName(type);
            var typeProperties = type.GetAllPropertyInfos();
            var generateProperties = typeProperties.Where(e => StaticLiterals.NoGenerationProperties.Any(p => p.Equals(e.Name)) == false) ?? Array.Empty<PropertyInfo>();
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = CreateModelFullName(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = ItemProperties.CreateModelSubPath(type, string.Empty, StaticLiterals.CSharpFileExtension),
            };

            result.AddRange(CreateComment(type));
            CreateModelAttributes(type, result.Source);
            result.Add($"public partial class {modelName}");
            result.Add("{");
            result.AddRange(CreatePartialStaticConstrutor(modelName));
            result.AddRange(CreatePartialConstrutor("public", modelName, initStatements: new string[] { $"_source = new {entitySubType}();" }));
            result.AddRange(CreatePartialConstrutor("internal", modelName, argumentList: $"{entitySubType} source", initStatements: new string[] { $"_source = source;" }, withPartials: false));

            result.Add(string.Empty);
            result.Add($"new internal {entitySubType} Source");
            result.Add("{");
            result.Add($"get => ({entitySubType})(_source!);");
            result.Add("set => _source = value;");
            result.Add("}");

            foreach (var modelItem in generateProperties)
            {
                if (QuerySetting<bool>(unitType, ItemType.ModelProperty, modelItem.DeclaringName(), StaticLiterals.Generate, "True"))
                {
                    CreateModelPropertyAttributes(modelItem, unitType, result.Source);
                    result.AddRange(CreateDelegateProperty(modelItem, "Source", modelItem));
                }
            }
            if (unitType == UnitType.Logic)
            {
                var visibility = type.IsPublic ? "public" : "internal";

                result.AddRange(CreateDelegateCopyProperties("internal", type, entitySubType));
                result.AddRange(CreateDelegateCopyProperties(visibility, type, modelType));
            }
            else if (unitType == UnitType.WebApi)
            {
                result.AddRange(CreateCopyProperties("public", type, modelType));
            }
            else if (unitType == UnitType.AspMvc)
            {
                result.AddRange(CreateCopyProperties("public", type, modelType, p => true));
            }
            result.AddRange(CreateEquals(type, modelSubType));
            result.AddRange(CreateGetHashCode(type));
            result.AddRange(CreateDelegateFactoryMethods(modelType, entitySubType, type.IsPublic, false));
            result.Add("}");
            result.EnvelopeWithANamespace(ItemProperties.CreateModelNamespace(type));
            result.FormatCSharpCode();
            return result;
        }

        protected static string GetBaseClassByType(Type type)
        {
            var result = "object";
            var found = false;
            var runType = type.BaseType;

            while (runType != null && found == false)
            {
                if (StaticLiterals.BaseClassMapping.ContainsKey(runType.Name))
                {
                    found = true;
                    result = StaticLiterals.BaseClassMapping[runType.Name];
                }
                runType = runType.BaseType;
            }
            return result;
        }
        protected string CreateModelFullName(Type type)
        {
            return $"{ItemProperties.CreateModelNamespace(type)}.{type.Name}";
        }

        #region query settings
        protected T QuerySetting<T>(UnitType unitType, ItemType itemType, Type type, string valueName, string defaultValue)
        {
            T result;

            try
            {
                result = (T)Convert.ChangeType(QueryGenerationSettingValue(unitType, itemType, CreateEntitiesSubTypeFromType(type), valueName, defaultValue), typeof(T));
            }
            catch (Exception ex)
            {
                result = (T)Convert.ChangeType(defaultValue, typeof(T));
                System.Diagnostics.Debug.WriteLine($"Error in {MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
            }
            return result;
        }
        protected T QuerySetting<T>(UnitType unitType, ItemType itemType, Type type, string itemSubName, string valueName, string defaultValue)
        {
            T result;

            try
            {
                result = (T)Convert.ChangeType(QueryGenerationSettingValue(unitType, itemType, $"{CreateEntitiesSubTypeFromType(type)}.{itemSubName}", valueName, defaultValue), typeof(T));
            }
            catch (Exception ex)
            {
                result = (T)Convert.ChangeType(defaultValue, typeof(T));
                System.Diagnostics.Debug.WriteLine($"Error in {MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
            }
            return result;
        }
        protected T QuerySetting<T>(UnitType unitType, ItemType itemType, string itemName, string valueName, string defaultValue)
        {
            T result;

            try
            {
                result = (T)Convert.ChangeType(QueryGenerationSettingValue(unitType, itemType, $"{itemName}", valueName, defaultValue), typeof(T));
            }
            catch (Exception ex)
            {
                result = (T)Convert.ChangeType(defaultValue, typeof(T));
                System.Diagnostics.Debug.WriteLine($"Error in {MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
            }
            return result;
        }
        #endregion query settings
    }
}
//MdEnd
