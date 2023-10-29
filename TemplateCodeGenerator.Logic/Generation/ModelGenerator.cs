//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic.Generation
{
    using System.Reflection;
    using TemplateCodeGenerator.Logic.Common;
    using TemplateCodeGenerator.Logic.Contracts;
    /// <summary>
    /// Represents a class that generates models based on a given type. This class is abstract and internal.
    /// </summary>
    /// <inheritdoc cref="ItemGenerator"/>
    internal abstract partial class ModelGenerator : ItemGenerator
    {
        /// <summary>
        /// Gets the properties of the item.
        /// </summary>
        /// <value>
        /// The properties of the item.
        /// </value>
        protected abstract ItemProperties ItemProperties { get; }
        
        /// <summary>
        /// Initializes a new instance of the ModelGenerator class.
        /// </summary>
        /// <param name="solutionProperties">The solution properties.</param>
        protected ModelGenerator(ISolutionProperties solutionProperties)
        : base(solutionProperties)
        {
        }
        
        #region overrides
        /// <summary>
        /// Returns the type of the property.
        /// </summary>
        /// <param name="propertyInfo">The PropertyInfo object representing the property.</param>
        /// <returns>The type of the property after converting it to the model type.</returns>
        protected override string GetPropertyType(PropertyInfo propertyInfo)
        {
            var propertyType = base.GetPropertyType(propertyInfo);
            var result = ItemProperties.ConvertEntityToModelType(propertyType);
            
            return ConvertPropertyType(result);
        }
        /// <summary>
        /// Copies the property value from one object to another.
        /// </summary>
        /// <param name="copyType">The type of the object to copy the property value to.</param>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/> object representing the property to be copied.</param>
        /// <returns>
        /// The copied property value, or the value returned by the base implementation of <see cref="CopyProperty"/>
        /// if the property does not meet the specified conditions for copying.
        /// </returns>
        protected override string CopyProperty(string copyType, PropertyInfo propertyInfo)
        {
            string? result = null;
            
            if (StaticLiterals.VersionProperties.Any(vp => vp.Equals(propertyInfo.Name)) == false
            && copyType.Equals(propertyInfo.DeclaringType!.FullName, StringComparison.CurrentCultureIgnoreCase) == false)
            {
                if (ItemProperties.IsArrayType(propertyInfo.PropertyType))
                {
                    var modelType = ItemProperties.ConvertEntityToModelType(propertyInfo.PropertyType.GetElementType()!.FullName!);
                    
                    result = $"{propertyInfo.Name} = other.{propertyInfo.Name}.Select(e => {modelType}.Create((object)e)).ToArray();";
                }
                else if (ItemProperties.IsListType(propertyInfo.PropertyType))
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
        /// <summary>
        /// Copies the specified property of the given copy type.
        /// </summary>
        /// <param name="copyType">The type to copy.</param>
        /// <param name="propertyInfo">The PropertyInfo object representing the property to be copied.</param>
        /// <returns>A string representation of the copied property.</returns>
        /// <remarks>
        /// This method is called to copy a specific property of the specified copy type.
        /// It checks if the property is not one of the version properties and if the copy type is not the same as the declaring type of the property.
        /// If the property is an array type, it converts the array elements to the corresponding model type and returns the resulting string representation.
        /// If the property is a list type, it returns an empty string.
        /// If the property is an entity type, it converts the entity to the corresponding model type and returns the resulting string representation.
        /// If none of the above conditions are met, it calls the base method to perform the default copy property behavior.
        /// </remarks>
        protected override string CopyDelegateProperty(string copyType, PropertyInfo propertyInfo)
        {
            string? result = null;
            
            if (StaticLiterals.VersionProperties.Any(vp => vp.Equals(propertyInfo.Name)) == false
            && copyType.Equals(propertyInfo.DeclaringType!.FullName, StringComparison.CurrentCultureIgnoreCase) == false)
            {
                if (ItemProperties.IsArrayType(propertyInfo.PropertyType))
                {
                    var modelType = ItemProperties.ConvertEntityToModelType(propertyInfo.PropertyType.GetElementType()!.FullName!);
                    
                    result = $"{propertyInfo.Name} = other.{propertyInfo.Name}.Select(e => {modelType}.Create((object)e)).ToArray();";
                }
                else if (ItemProperties.IsListType(propertyInfo.PropertyType))
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
        
        #region create attributes
        /// <summary>
        /// Creates model attributes for the specified type.
        /// </summary>
        /// <param name="type">The type for which model attributes need to be created.</param>
        /// <param name="codeLines">The list of code lines representing the model attributes.</param>
        /// <remarks>
        /// This method is used to generate model attributes for a given type. Model attributes are used
        /// to provide metadata or behavior information for the type, such as specifying validation rules
        /// or serialization behavior.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Thrown when the type argument is null.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when the codeLines argument is null.</exception>
        partial void CreateModelAttributes(Type type, List<string> codeLines);
        /// <summary>
        /// Creates the model property attributes for a given PropertyInfo object and UnitType.
        /// </summary>
        /// <param name="propertyInfo">The PropertyInfo object representing the property.</param>
        /// <param name="unitType">The UnitType associated with the property.</param>
        /// <param name="codeLines">The list of code lines to add the attributes to.</param>
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
        /// <summary>
        /// Method called before creating model property attributes.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="unitType">The unit type.</param>
        /// <param name="codeLines">The list of code lines.</param>
        /// <param name="handled">A reference to a bool indicating if the method has been handled.</param>
        /// <remarks>
        /// This method is called before creating attributes for a model property.
        /// It allows customization of the attribute creation process.
        /// The property information, unit type, and code lines for the property will be passed as parameters.
        /// The handled parameter can be modified by the method to indicate if it has handled the operation.
        /// </remarks>
        /// <seealso cref="AfterCreateModelPropertyAttributes(PropertyInfo, UnitType, List<string>)"/>
        /// <seealso cref="CreateModelPropertyAttributes(PropertyInfo, UnitType, List<string>)"/>
        partial void BeforeCreateModelPropertyAttributes(PropertyInfo propertyInfo, UnitType unitType, List<string> codeLines, ref bool handled);
        /// <summary>
        /// This method is called after creating model property attributes.
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/> object representing the property.</param>
        /// <param name="unitType">The <see cref="UnitType"/> representing the unit type.</param>
        /// <param name="codeLines">A list of strings representing the code lines.</param>
        partial void AfterCreateModelPropertyAttributes(PropertyInfo propertyInfo, UnitType unitType, List<string> codeLines);
        #endregion create attributes
        
        #region converters
        /// <summary>
        /// Converts the given model name to a string representation.
        /// </summary>
        /// <param name="modelName">The model name to be converted.</param>
        /// <returns>The converted string representation of the model name.</returns>
        protected virtual string ConvertModelName(string modelName) => modelName;
        /// <summary>
        /// Converts the given model subtype.
        /// </summary>
        /// <param name="modelSubType">The model subtype to be converted.</param>
        /// <returns>The converted model subtype.</returns>
        protected virtual string ConvertModelSubType(string modelSubType) => modelSubType;
        /// <summary>
        /// Converts the specified model namespace.
        /// </summary>
        /// <param name="modelNamespace">The model namespace to be converted.</param>
        /// <returns>The converted model namespace.</returns>
        protected virtual string ConvertModelNamespace(string modelNamespace) => modelNamespace;
        /// <summary>
        /// Converts the full name of the model.
        /// </summary>
        /// <param name="modelFullName">The full name of the model.</param>
        /// <returns>The converted full name of the model.</returns>
        protected virtual string ConvertModelFullName(string modelFullName) => modelFullName;
        /// <summary>
        /// Converts the model subpath to a string representation.
        /// </summary>
        /// <param name="modelSubPath">The model subpath to be converted.</param>
        /// <returns>The converted string representation of the model subpath.</returns>
        protected virtual string ConvertModelSubPath(string modelSubPath) => modelSubPath;
        /// <summary>
        /// Converts the specified model base type.
        /// </summary>
        /// <param name="modelBaseType">The model base type to convert.</param>
        /// <returns>The converted model base type.</returns>
        protected virtual string ConvertModelBaseType(string modelBaseType) => modelBaseType;
        #endregion converters
        
        /// <summary>
        /// Creates a model from a given type, unit type, and item type.
        /// </summary>
        /// <param name="type">The type of the model.</param>
        /// <param name="unitType">The unit type of the model.</param>
        /// <param name="itemType">The item type of the model.</param>
        /// <returns>The generated model as an IGeneratedItem object.</returns>
        protected virtual IGeneratedItem CreateModelFromType(Type type, UnitType unitType, ItemType itemType)
        {
            var modelName = ConvertModelName(ItemProperties.CreateModelName(type));
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
                if (CanCreate(propertyInfo) && QuerySetting<bool>(unitType, ItemType.ModelProperty, type, StaticLiterals.Generate, "True"))
                {
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
            else if (unitType == UnitType.ClientBlazorApp)
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
        /// <summary>
        /// Creates a delegate model from the specified type.
        /// </summary>
        /// <param name="type">The type to create the delegate model from.</param>
        /// <param name="unitType">The unit type of the generated item.</param>
        /// <param name="itemType">The item type of the generated item.</param>
        /// <returns>The generated delegate model.</returns>
        protected virtual IGeneratedItem CreateDelegateModelFromType(Type type, UnitType unitType, ItemType itemType)
        {
            var modelName = ItemProperties.CreateModelName(type);
            var modelType = ItemProperties.CreateModelType(type);
            var modelSubType = ItemProperties.CreateModelSubType(type);
            var entitySubType = ItemProperties.CreateSubType(type);
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
            
            foreach (var modelItem in generateProperties.Where(pi => ItemProperties.IsEntityType(pi.PropertyType) == false
            && ItemProperties.IsEntityListType(pi.PropertyType) == false))
            {
                if (CanCreate(modelItem) && QuerySetting<bool>(unitType, ItemType.ModelProperty, modelItem.DeclaringName(), StaticLiterals.Generate, "True"))
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
        /// <summary>
        /// Creates a delegate model navigation from the specified type, unit type, and item type.
        /// </summary>
        /// <param name="type">The type to create the delegate model navigation from.</param>
        /// <param name="unitType">The unit type of the generated item.</param>
        /// <param name="itemType">The item type of the generated item.</param>
        /// <returns>The generated delegate model navigation.</returns>
        protected virtual IGeneratedItem CreateDelegateModelNavigationsFromType(Type type, UnitType unitType, ItemType itemType)
        {
            var modelName = ItemProperties.CreateModelName(type);
            var modelType = ItemProperties.CreateModelType(type);
            var modelSubType = ItemProperties.CreateModelSubType(type);
            var entitySubType = ItemProperties.CreateSubType(type);
            var typeProperties = type.GetAllPropertyInfos();
            var generateProperties = typeProperties.Where(e => StaticLiterals.NoGenerationProperties.Any(p => p.Equals(e.Name)) == false) ?? Array.Empty<PropertyInfo>();
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = CreateModelFullName(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = ItemProperties.CreateModelSubPath(type, "Navigation", StaticLiterals.CSharpFileExtension),
            };
            
            result.AddRange(CreateComment(type));
            result.Add($"partial class {modelName}");
            result.Add("{");
            
            foreach (var modelItem in generateProperties.Where(pi => ItemProperties.IsEntityType(pi.PropertyType)
            || ItemProperties.IsEntityListType(pi.PropertyType)))
            {
                if (QuerySetting<bool>(unitType, ItemType.ModelProperty, modelItem.DeclaringName(), StaticLiterals.Generate, "True"))
                {
                    CreateModelPropertyAttributes(modelItem, unitType, result.Source);
                    result.AddRange(CreateDelegateProperty(modelItem, "Source", modelItem));
                }
            }
            
            result.Add("}");
            result.EnvelopeWithANamespace(ItemProperties.CreateModelNamespace(type));
            result.FormatCSharpCode();
            return result;
        }
        /// <summary>
        /// Creates a model inheritance based on the given type, unit type, and item type.
        /// </summary>
        /// <param name="type">The type to create the model inheritance for.</param>
        /// <param name="unitType">The unit type of the generated item.</param>
        /// <param name="itemType">The item type of the generated item.</param>
        /// <returns>The generated item representing the model inheritance.</returns>
        protected virtual IGeneratedItem CreateModelInheritance(Type type, UnitType unitType, ItemType itemType)
        {
            var modelName = ConvertModelName(ItemProperties.CreateModelName(type));
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
        
        /// <summary>
        /// Retrieves the base class by type.
        /// </summary>
        /// <param name="type">The type to get the base class for.</param>
        /// <returns>The name of the base class for the specified type.</returns>
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
        /// <summary>
        /// Creates the full name of the model by concatenating the namespace and the name of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of the model.</param>
        /// <returns>The full name of the model.</returns>
        protected string CreateModelFullName(Type type)
        {
            return $"{ItemProperties.CreateModelNamespace(type)}.{type.Name}";
        }
    }
}
//MdEnd
