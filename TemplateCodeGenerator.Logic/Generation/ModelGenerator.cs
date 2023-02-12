﻿//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic.Generation
{
    using System.Reflection;
    using TemplateCodeGenerator.Logic.Contracts;
    internal abstract partial class ModelGenerator : ClassGenerator
    {
        protected abstract ItemProperties ItemProperties { get; }
        public ModelGenerator(ISolutionProperties solutionProperties)
            : base(solutionProperties)
        {
        }


        #region overrides
        public override string GetPropertyType(PropertyInfo propertyInfo)
        {
            var result = base.GetPropertyType(propertyInfo);
            var modelType = ItemProperties.ConvertEntityToModelType(result);

            return modelType;
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

        protected T QueryModelSetting<T>(Common.UnitType unitType, Common.ItemType itemType, Type type, string valueName, string defaultValue)
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
        protected T QueryModelSetting<T>(Common.UnitType unitType, Common.ItemType itemType, Type type, string itemSubName, string valueName, string defaultValue)
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
        protected T QueryModelSetting<T>(Common.UnitType unitType, Common.ItemType itemType, string itemName, string valueName, string defaultValue)
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

        protected virtual bool CanCreate(Type type)
        {
            bool create = EntityProject.IsNotAGenerationEntity(type) == false;

            CanCreateModel(type, ref create);
            return create;
        }
        partial void CanCreateModel(Type type, ref bool create);
        partial void CreateModelAttributes(Type type, List<string> codeLines);
        protected virtual void CreateModelPropertyAttributes(PropertyInfo propertyInfo, List<string> codeLines)
        {
            var handled = false;

            BeforeCreateModelPropertyAttributes(propertyInfo, codeLines, ref handled);
            if (handled == false)
            {
            }
            AfterCreateModelPropertyAttributes(propertyInfo, codeLines);
        }

        partial void BeforeCreateModelPropertyAttributes(PropertyInfo propertyInfo, List<string> codeLines, ref bool handled);
        partial void AfterCreateModelPropertyAttributes(PropertyInfo propertyInfo, List<string> codeLines);

        protected virtual IGeneratedItem CreateModelFromType(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var modelName = CreateModelName(type);
            var visibility = QueryModelSetting<string>(unitType, itemType, type, StaticLiterals.Visibility, "public");
            var attributes = QueryModelSetting<string>(unitType, itemType, type, StaticLiterals.Attributes, string.Empty);
            var typeProperties = type.GetAllPropertyInfos();
            var generateProperties = typeProperties.Where(e => StaticLiterals.NoGenerationProperties.Any(p => p.Equals(e.Name)) == false) ?? Array.Empty<PropertyInfo>();
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = CreateModelFullNameFromType(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = ItemProperties.CreateModelSubPath(type, string.Empty, StaticLiterals.CSharpFileExtension),
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
                var propertyAttributes = QueryModelSetting<string>(unitType, Common.ItemType.Property, type, propertyInfo.Name, StaticLiterals.Attributes, string.Empty);

                result.AddRange(CreateComment(propertyInfo));
                CreateModelPropertyAttributes(propertyInfo, result.Source);
                result.Add($"{(propertyAttributes.HasContent() ? $"[{propertyAttributes}]" : string.Empty)}");
                result.AddRange(CreateProperty(type, propertyInfo));
            }

            result.AddRange(CreateFactoryMethod(false, ItemProperties.CreateModelType(type)));

            if (unitType == Common.UnitType.Logic)
            {
                var accessType = type.FullName!;
                var modelType = ItemProperties.CreateModelType(type);

                result.AddRange(CreateFactoryMethod(false, modelType, accessType));
                result.AddRange(CreateCopyProperties("internal", type, accessType));
                result.AddRange(CreateCopyProperties("public", type, modelType));
            }
            else if (unitType == Common.UnitType.WebApi)
            {
                if (type.IsPublic)
                {
                    var logicProject = $"{ItemProperties.SolutionName}{StaticLiterals.LogicExtension}";
                    var accessType = $"{logicProject}.{ItemProperties.CreateModelSubType(type)}";
                    var modelType = ItemProperties.CreateModelType(type);

                    result.AddRange(CreateFactoryMethod(false, modelType, accessType));
                    result.AddRange(CreateCopyProperties("public", type, accessType));
                    result.AddRange(CreateCopyProperties("public", type, modelType));
                }
                else
                {
                    var logicProject = $"{ItemProperties.SolutionName}{StaticLiterals.LogicExtension}";
                    var accessType = $"{logicProject}.{ItemProperties.CreateModelSubType(type)}";

                    result.AddRange(CreateCopyProperties("public", type, accessType));
                }
            }
            else if (unitType == Common.UnitType.AspMvc)
            {
                if (type.IsPublic)
                {
                    var logicProject = $"{ItemProperties.SolutionName}{StaticLiterals.LogicExtension}";
                    var accessType = $"{logicProject}.{ItemProperties.CreateModelSubType(type)}";
                    var modelType = ItemProperties.CreateModelType(type);

                    result.AddRange(CreateFactoryMethod(false, modelType, accessType));
                    result.AddRange(CreateCopyProperties("public", type, accessType));
                    result.AddRange(CreateCopyProperties("public", type, modelType));
                }
                else
                {
                    var logicProject = $"{ItemProperties.SolutionName}{StaticLiterals.LogicExtension}";
                    var accessType = $"{logicProject}.{ItemProperties.CreateModelSubType(type)}";

                    result.AddRange(CreateCopyProperties("public", type, accessType));
                }
            }
            result.AddRange(OverrideEquals(type));
            result.AddRange(CreateGetHashCode(type));
            result.Add("}");
            result.EnvelopeWithANamespace(ItemProperties.CreateModelNamespace(type), "using System;");
            result.FormatCSharpCode();
            return result;
        }
        protected virtual IGeneratedItem CreateDelegateModelFromType(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var entityType = ItemProperties.CreateSubType(type);
            var modelType = ItemProperties.CreateModelType(type);
            var modelName = CreateModelName(type);
            var typeProperties = type.GetAllPropertyInfos();
            var generateProperties = typeProperties.Where(e => StaticLiterals.NoGenerationProperties.Any(p => p.Equals(e.Name)) == false) ?? Array.Empty<PropertyInfo>();
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = CreateModelFullNameFromType(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = ItemProperties.CreateModelSubPath(type, string.Empty, StaticLiterals.CSharpFileExtension),
            };

            result.AddRange(CreateComment(type));
            CreateModelAttributes(type, result.Source);
            result.Add($"public partial class {modelName}");
            result.Add("{");
            result.AddRange(CreatePartialStaticConstrutor(modelName));
            result.AddRange(CreatePartialConstrutor("public", modelName, initStatements: new string[] { $"_source = new {entityType}();" }));
            result.AddRange(CreatePartialConstrutor("internal", modelName, argumentList: $"{entityType} source", initStatements: new string[] { $"_source = source;" }, withPartials: false));

            result.Add(string.Empty);
            result.Add($"new internal {entityType} Source");
            result.Add("{");
            //result.Add($"get => ({entityType})(_source ??= new {entityType}());");
            result.Add($"get => ({entityType})(_source!);");
            result.Add("set => _source = value;");
            result.Add("}");

            foreach (var modelItem in generateProperties)
            {
                if (QueryModelSetting<bool>(unitType, Common.ItemType.ModelProperty, modelItem.DeclaringName(), StaticLiterals.Generate, "True"))
                {
                    CreateModelPropertyAttributes(modelItem, result.Source);
                    result.AddRange(CreateDelegateProperty(modelItem, "Source", modelItem));
                }
            }
            if (unitType == Common.UnitType.Logic)
            {
                var visibility = type.IsPublic ? "public" : "internal";

                result.AddRange(CreateDelegateCopyProperties("internal", type, entityType));
                result.AddRange(CreateDelegateCopyProperties(visibility, type, modelType));
            }
            else if (unitType == Common.UnitType.WebApi)
            {
                result.AddRange(CreateCopyProperties("public", type, modelType));
            }
            else if (unitType == Common.UnitType.AspMvc)
            {
                result.AddRange(CreateCopyProperties("public", type, modelType, p => true));
            }
            result.AddRange(OverrideEquals(type));
            result.AddRange(CreateGetHashCode(type));
            result.AddRange(CreateDelegateFactoryMethods(modelType, entityType, type.IsPublic, false));
            result.Add("}");
            result.EnvelopeWithANamespace(ItemProperties.CreateModelNamespace(type));
            result.FormatCSharpCode();
            return result;
        }

        protected string GetBaseClassByType(Type type, string subNamespace)
        {
            var result = "object";

            while (type.BaseType != null
                   && StaticLiterals.BaseClasses.Any(e => e.Equals(type.BaseType.Name)) == false)
            {
                type = type.BaseType;
            }

            if (type.BaseType != null)
            {
                var idx = StaticLiterals.BaseClasses.IndexOf(e => e.Equals(type.BaseType.Name)) % 2;

                if (idx > -1 && idx < StaticLiterals.ModelBaseClasses.Length)
                {
                    var ns = ItemProperties.Namespace;

                    if (string.IsNullOrEmpty(subNamespace) == false)
                        ns = $"{ns}.{subNamespace}";

                    result = $"{ns}.{StaticLiterals.ModelBaseClasses[idx]}";
                }
            }
            return result;
        }
        protected string CreateModelFullNameFromType(Type type)
        {
            return $"{ItemProperties.CreateModelNamespace(type)}.{type.Name}";
        }
    }
}
//MdEnd
