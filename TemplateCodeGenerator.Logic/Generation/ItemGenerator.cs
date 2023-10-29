﻿//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic.Generation
{
    using System.Reflection;
    using TemplateCodeGenerator.Logic.Common;
    using TemplateCodeGenerator.Logic.Contracts;
    /// <summary>
    /// This class provides many methods for generating program parts.
    /// </summary>
    internal partial class ItemGenerator : GeneratorObject
    {
        #region can queries
        /// <summary>
        /// Determines whether or not a given type can be created.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// Returns true if the type can be created, false otherwise.
        /// </returns>
        protected virtual bool CanCreate(Type type)
        {
            return EntityProject.IsNotAGenerationEntity(type) == false;
        }
        /// <summary>
        /// Determines whether a property can be created.
        /// </summary>
        /// <param name="propertyInfo">The property info of the property.</param>
        /// <returns>Returns true if the property can be created; otherwise, false.</returns>
        protected virtual bool CanCreate(PropertyInfo propertyInfo)
        {
            return QuerySetting<bool>(UnitType.General, ItemType.Property, propertyInfo.DeclaringName(), StaticLiterals.Generate, "True");
        }
        /// <summary>
        /// Determines whether a property can be copied.
        /// </summary>
        /// <param name="propertyInfo">The PropertyInfo object representing the property.</param>
        /// <returns>
        /// True if the property can be copied; otherwise, False.
        /// </returns>
        protected virtual bool CanCopyProperty(PropertyInfo propertyInfo)
        {
            return QuerySetting<bool>(UnitType.General, ItemType.Property, propertyInfo.DeclaringName(), StaticLiterals.Copy, "True");
        }
        #endregion can queries

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemGenerator"/> class.
        /// </summary>
        /// <param name="solutionProperties">The solution properties.</param>
        protected ItemGenerator(ISolutionProperties solutionProperties)
        : base(solutionProperties)
        {

        }
        /// <summary>
        /// Creates a new instance of the <see cref="ItemGenerator"/> class with the specified <see cref="ISolutionProperties"/> object.
        /// </summary>
        /// <param name="solutionProperties">The <see cref="ISolutionProperties"/> object to be used for generating items.</param>
        /// <returns>A new instance of the <see cref="ItemGenerator"/> class.</returns>
        public static ItemGenerator Create(ISolutionProperties solutionProperties)
        {
            return new ItemGenerator(solutionProperties);
        }

        #region create constructors
        /// <summary>
        /// Creates a partial static constructor for a class with optional initialization statements.
        /// </summary>
        /// <param name="className">The name of the class.</param>
        /// <param name="initStatements">Optional initialization statements.</param>
        /// <returns>An enumerable collection of strings representing the lines of the partial static constructor.</returns>
        public virtual IEnumerable<string> CreatePartialStaticConstrutor(string className, IEnumerable<string>? initStatements = null)
        {
            var lines = new List<string>(CreateComment("Initializes the class (created by the generator)."))
            {
                $"static {className}()",
                "{",
                "ClassConstructing();"
            };
            if (initStatements != null)
            {
                foreach (var item in initStatements)
                {
                    lines.Add($"{item}");
                }
            }
            lines.Add($"ClassConstructed();");
            lines.Add("}");
            lines.Add("static partial void ClassConstructing();");
            lines.Add("static partial void ClassConstructed();");
            lines.Add(string.Empty);
            return lines;
        }
        /// <summary>
        /// Creates a partial constructor with the specified visibility, class name, argument list, and base constructor.
        /// </summary>
        /// <param name="visibility">The visibility of the constructor (e.g., public, private).</param>
        /// <param name="className">The name of the class.</param>
        /// <param name="argumentList">The list of arguments for the constructor.</param>
        /// <param name="baseConstructor">The base constructor to be called.</param>
        /// <param name="initStatements">The list of initialization statements.</param>
        /// <param name="withPartials">Indicates whether to include partial methods for constructing and constructed.</param>
        /// <returns>An IEnumerable of strings representing the lines of code for the constructor.</returns>
        public virtual IEnumerable<string> CreatePartialConstrutor(string visibility, string className, string? argumentList = null, string? baseConstructor = null, IEnumerable<string>? initStatements = null, bool withPartials = true)
        {
            var lines = new List<string>(CreateComment($"Initializes a new instance of the <see cref=\"{className}\"/> class (created by the generator.)"))
            {
                $"{visibility} {className}({argumentList})",
            };

            if (string.IsNullOrEmpty(baseConstructor) == false)
                lines.Add($" : {baseConstructor}");

            lines.Add("{");
            lines.Add("Constructing();");
            if (initStatements != null)
            {
                foreach (var item in initStatements)
                {
                    lines.Add($"{item}");
                }
            }
            else
            {
                lines.Add(string.Empty);
            }
            lines.Add($"Constructed();");
            lines.Add("}");
            if (withPartials)
            {
                lines.Add("partial void Constructing();");
                lines.Add("partial void Constructed();");
            }
            return lines;
        }
        #endregion create constructors

        #region create factory methode
        /// <summary>
        /// Creates a factory method for the specified item type.
        /// </summary>
        /// <param name="newPrefix">True to include "new" keyword in the method signature, otherwise false.</param>
        /// <param name="itemType">The type of the item.</param>
        /// <returns>An IEnumerable of strings containing the XML documentation for the specified factory method.</returns>
        public IEnumerable<string> CreateFactoryMethod(bool newPrefix, string itemType)
        {
            var result = new List<string>();

            result.AddRange(CreateComment());
            result.Add($"public{(newPrefix ? " new " : " ")}static {itemType} Create()");
            result.Add("{");
            result.Add("BeforeCreate();");
            result.Add($"var result = new {itemType}();");
            result.Add("AfterCreate(result);");
            result.Add("return result;");
            result.Add("}");

            result.AddRange(CreateComment());
            result.Add($"public{(newPrefix ? " new " : " ")}static {itemType} Create(object other)");
            result.Add("{");
            result.Add("BeforeCreate(other);");
            //result.Add("CommonBase.Extensions.ObjectExtensions.CheckArgument(other, nameof(other));");
            result.Add($"var result = new {itemType}();");
            result.Add("CommonBase.Extensions.ObjectExtensions.CopyFrom(result, other);");
            result.Add("AfterCreate(result, other);");
            result.Add("return result;");
            result.Add("}");

            result.Add("static partial void BeforeCreate();");
            result.Add($"static partial void AfterCreate({itemType} instance);");

            result.Add("static partial void BeforeCreate(object other);");
            result.Add($"static partial void AfterCreate({itemType} instance, object other);");
            return result;
        }
        /// <summary>
        /// Create a factory method that creates an instance of the specified <paramref name="itemType"/>
        /// by copying properties from another instance of the specified <paramref name="copyType"/>.
        /// </summary>
        /// <param name="newPrefix">A boolean value indicating whether to prefix the method with "new" keyword.</param>
        /// <param name="itemType">The type of the item to create.</param>
        /// <param name="copyType">The type of the item from which to copy properties.</param>
        /// <returns>An IEnumerable of strings representing the lines of the factory method.</returns>
        public IEnumerable<string> CreateFactoryMethod(bool newPrefix, string itemType, string copyType)
        {
            var result = new List<string>();

            result.AddRange(CreateComment());
            result.Add($"public{(newPrefix ? " new " : " ")}static {itemType} Create({copyType} other)");
            result.Add("{");
            result.Add("BeforeCreate(other);");
            result.Add($"var result = new {itemType}();");
            result.Add("result.CopyProperties(other);");
            result.Add("AfterCreate(result, other);");
            result.Add("return result;");
            result.Add("}");

            result.Add($"static partial void BeforeCreate({copyType} other);");
            result.Add($"static partial void AfterCreate({itemType} instance, {copyType} other);");
            return result;
        }
        /// <summary>
        /// Creates delegate factory methods for the specified item type.
        /// </summary>
        /// <param name="itemType">The type of the item.</param>
        /// <param name="delegateName">The name of the delegate.</param>
        /// <param name="isPublic">Specifies whether the created methods should be public or internal.</param>
        /// <param name="newPrefix">Specifies whether the 'new' prefix should be used.</param>
        /// <returns>An enumerable collection of strings representing the delegate factory methods.</returns>
        public IEnumerable<string> CreateDelegateFactoryMethods(string itemType, string delegateName, bool isPublic, bool newPrefix)
        {
#pragma warning disable IDE0028 // Simplify collection initialization
            var result = new List<string>(CreateComment());
#pragma warning restore IDE0028 // Simplify collection initialization

            result.Add($"public{(newPrefix ? " new " : " ")}static {itemType} Create()");
            result.Add("{");
            result.Add("BeforeCreate();");
            result.Add($"var result = new {itemType}();");
            result.Add("AfterCreate(result);");
            result.Add("return result;");
            result.Add("}");

            result.AddRange(CreateComment());
            result.Add($"public{(newPrefix ? " new " : " ")}static {itemType} Create(object other)");
            result.Add("{");
            result.Add("BeforeCreate(other);");
            result.Add($"var result = new {itemType}();");
            result.Add("CommonBase.Extensions.ObjectExtensions.CopyFrom(result, other);");
            result.Add("AfterCreate(result, other);");
            result.Add("return result;");
            result.Add("}");

            result.AddRange(CreateComment());
            result.Add($"public{(newPrefix ? " new " : " ")}static {itemType} Create({itemType} other)");
            result.Add("{");
            result.Add("BeforeCreate(other);");
            result.Add($"var result = new {itemType}();");
            result.Add("result.CopyProperties(other);");
            result.Add("AfterCreate(result, other);");
            result.Add("return result;");
            result.Add("}");

            var visibility = isPublic ? "public" : "internal";

            result.AddRange(CreateComment());
            result.Add($"{visibility}{(newPrefix ? " new " : " ")}static {itemType} Create({delegateName} other)");
            result.Add("{");
            result.Add("BeforeCreate(other);");
            result.Add($"var result = new {itemType}();");
            result.Add("result.Source = other;");
            result.Add("AfterCreate(result, other);");
            result.Add("return result;");
            result.Add("}");

            result.Add("static partial void BeforeCreate();");
            result.Add($"static partial void AfterCreate({itemType} instance);");

            result.Add("static partial void BeforeCreate(object other);");
            result.Add($"static partial void AfterCreate({itemType} instance, object other);");

            result.Add($"static partial void BeforeCreate({itemType} other);");
            result.Add($"static partial void AfterCreate({itemType} instance, {itemType} other);");

            result.Add($"static partial void BeforeCreate({delegateName} other);");
            result.Add($"static partial void AfterCreate({itemType} instance, {delegateName} other);");
            return result;
        }
        #endregion create factory methode

        #region create property
        /// <summary>
        /// Creates the property attributes for the given PropertyInfo.
        /// </summary>
        /// <param name="propertyInfo">The PropertyInfo object representing the property.</param>
        /// <param name="codeLines">The list of code lines to add the attributes to.</param>
        /// <remarks>
        /// This method is used to generate and add the necessary attributes for a property based on its PropertyInfo.
        /// </remarks>
        /// <seealso cref="PropertyInfo"/>
        /// <seealso cref="List{T}"/>
        protected virtual void CreatePropertyAttributes(PropertyInfo propertyInfo, List<string> codeLines) { }
        /// <summary>
        /// Creates XML documentation for the method that retrieves attributes of a property.
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/> object representing the property.</param>
        /// <param name="codeLines">The list of string representing the code lines.</param>
        protected virtual void CreateGetPropertyAttributes(PropertyInfo propertyInfo, List<string> codeLines) { }
        /// <summary>
        /// Creates the set property attributes for the specified property.
        /// </summary>
        /// <param name="propertyInfo">The PropertyInfo object that represents the property.</param>
        /// <param name="codeLines">The list of code lines where the attributes will be added.</param>
        protected virtual void CreateSetPropertyAttributes(PropertyInfo propertyInfo, List<string> codeLines) { }
        /// <summary>
        /// Retrieves the default value for a given property.
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/> object representing the property.</param>
        /// <param name="defaultValue">A reference to the default value, which will be updated with the result.</param>
        protected virtual void GetPropertyDefaultValue(PropertyInfo propertyInfo, ref string defaultValue) { }

        /// <summary>
        /// Creates a collection of strings representing the properties of a given type.
        /// </summary>
        /// <param name="type">The type for which properties are to be created.</param>
        /// <param name="propertyInfo">The PropertyInfo object representing the property.</param>
        /// <returns>A collection of strings representing the properties.</returns>
        /// <remarks>
        /// This method first calls the BeforeCreateProperty method passing in the type, PropertyInfo,
        /// and a list of strings as parameters. If the BeforeCreateProperty method sets the handled
        /// flag to true, no further action is taken. Otherwise, the method calls the CreateAutoProperty
        /// method passing in the type and PropertyInfo, and appends the returned strings to the result
        /// collection. Finally, the AfterCreateProperty method is invoked passing in the type, PropertyInfo,
        /// and result collection to perform any additional functionality. The resulting list of strings is returned.
        /// </remarks>
        public virtual IEnumerable<string> CreateProperty(Type type, PropertyInfo propertyInfo)
        {
            var handled = false;
            var result = new List<string>();

            BeforeCreateProperty(type, propertyInfo, result, ref handled);
            if (handled == false)
            {
                result.AddRange(CreateAutoProperty(type, propertyInfo));
            }
            AfterCreateProperty(type, propertyInfo, result);
            return result;
        }
        /// <summary>
        /// This method is called before creating a property in the specified type.
        /// </summary>
        /// <param name="type">The type where the property is being created.</param>
        /// <param name="propertyInfo">The property information being created.</param>
        /// <param name="codeLines">The list of code lines being generated for the property.</param>
        /// <param name="handled">A boolean indicating whether the operation has already been handled.</param>
        partial void BeforeCreateProperty(Type type, PropertyInfo propertyInfo, List<string> codeLines, ref bool handled);
        /// <summary>
        /// This method is called after a property is created in the specified type.
        /// </summary>
        /// <param name="type">The type containing the newly created property.</param>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/> object representing the newly created property.</param>
        /// <param name="codeLines">The list of code lines for the property.</param>
        /// <remarks>
        /// This method can be overridden in a partial class to perform custom actions after a property is created.
        /// </remarks>
        partial void AfterCreateProperty(Type type, PropertyInfo propertyInfo, List<string> codeLines);
        /// <summary>
        /// Creates an auto-implemented property for the specified type and property info.
        /// </summary>
        /// <param name="type">The type of the property.</param>
        /// <param name="propertyInfo">The property info object containing information about the property.</param>
        /// <returns>An IEnumerable of strings representing the created auto-implemented property.</returns>
        public virtual IEnumerable<string> CreateAutoProperty(Type type, PropertyInfo propertyInfo)
        {
            var result = new List<string>();
            var defaultValue = GetDefaultValue(propertyInfo);
            var propertyType = GetPropertyType(propertyInfo);

            if (defaultValue.HasContent())
                defaultValue = defaultValue.Replace($"{StaticLiterals.TProperty}", propertyType.Replace("[]", string.Empty));

            var property = $"public {propertyType} {propertyInfo.Name} ";

            GetPropertyDefaultValue(propertyInfo, ref defaultValue);
            if (string.IsNullOrEmpty(defaultValue))
            {
                property += "{ get; set; }";
            }
            else
            {
                property += "{ get; set; }" + $" = {defaultValue};";
            }
            result.AddRange(CreateComment(propertyInfo));
            result.Add(property);
            return result;
        }
        /// Creates XML documentation for the method CreatePartialProperty.
        /// @param propertyInfo - A PropertyInfo object representing the property
        /// @return An IEnumerable<string> representing the generated XML documentation
        public virtual IEnumerable<string> CreatePartialProperty(PropertyInfo propertyInfo)
        {
            var result = new List<string>();
            var defaultValue = GetDefaultValue(propertyInfo);
            var propertyType = GetPropertyType(propertyInfo);
            var fieldType = GetPropertyType(propertyInfo);
            var fieldName = CreateFieldName(propertyInfo, "_");
            var paramName = CreateFieldName(propertyInfo, "_");

            if (defaultValue.HasContent())
                defaultValue = defaultValue.Replace($"{StaticLiterals.TProperty}", propertyType.Replace("[]", string.Empty));

            result.Add(string.Empty);
            result.AddRange(CreateComment(propertyInfo));
            CreatePropertyAttributes(propertyInfo, result);
            result.Add($"public {fieldType} {propertyInfo.Name}");
            result.Add("{");
            result.AddRange(CreatePartialGetProperty(propertyInfo));
            result.AddRange(CreatePartialSetProperty(propertyInfo));
            result.Add("}");

            GetPropertyDefaultValue(propertyInfo, ref defaultValue);
            result.Add(string.IsNullOrEmpty(defaultValue)
            ? $"private {fieldType} {fieldName}{(ItemProperties.IsListType(propertyInfo.PropertyType) ? " = new();" : ";")}"
            : $"private {fieldType} {fieldName} = {defaultValue};");

            result.Add($"partial void On{propertyInfo.Name}Reading();");
            result.Add($"partial void On{propertyInfo.Name}Changing(ref bool handled, {fieldType} value, ref {fieldType} {paramName});");
            result.Add($"partial void On{propertyInfo.Name}Changed();");
            return result;
        }
        /// <summary>
        /// Creates the XML documentation for the <see cref="CreatePartialGetProperty"/> method.
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/> object representing the property.</param>
        /// <returns>An <see cref="IEnumerable{String}"/> containing the created XML documentation.</returns>
        public virtual IEnumerable<string> CreatePartialGetProperty(PropertyInfo propertyInfo)
        {
            var result = new List<string>();
            var fieldName = CreateFieldName(propertyInfo, "_");

            CreateGetPropertyAttributes(propertyInfo, result);
            result.Add("get");
            result.Add("{");
            result.Add($"On{propertyInfo.Name}Reading();");
            result.Add($"return {fieldName};");
            result.Add("}");
            return result;
        }
        /// <summary>
        /// Creates a partial set property for a given PropertyInfo object.
        /// </summary>
        /// <param name="propertyInfo">The PropertyInfo object representing the property.</param>
        /// <returns>An IEnumerable of strings representing the lines of code for the partial set property.</returns>
        public virtual IEnumerable<string> CreatePartialSetProperty(PropertyInfo propertyInfo)
        {
            var result = new List<string>();
            var propName = propertyInfo.Name;
            var fieldName = CreateFieldName(propertyInfo, "_");

            CreateSetPropertyAttributes(propertyInfo, result);
            result.Add("set");
            result.Add("{");
            result.Add("bool handled = false;");
            result.Add($"On{propName}Changing(ref handled, value, ref {fieldName});");
            result.Add("if (handled == false)");
            result.Add("{");
            result.Add($"{fieldName} = value;");
            result.Add("}");
            result.Add($"On{propName}Changed();");
            result.Add("}");
            return result.ToArray();
        }
        #endregion create properties

        #region delegate property helpers
        /// <summary>
        /// Creates a delegate property for the given property using the specified delegate object and delegate property.
        /// </summary>
        /// <param name="propertyInfo">The PropertyInfo of the property for which the delegate property is being created.</param>
        /// <param name="delegateObjectName">The name of the delegate object.</param>
        /// <param name="delegatePropertyInfo">The PropertyInfo of the delegate property.</param>
        /// <returns>An IEnumerable of strings representing the created delegate property.</returns>
        public virtual IEnumerable<string> CreateDelegateProperty(PropertyInfo propertyInfo, string delegateObjectName, PropertyInfo delegatePropertyInfo)
        {
            var handled = false;
            var result = new List<string>();

            BeforeCreateDelegateProperty(propertyInfo, delegateObjectName, delegatePropertyInfo, result, ref handled);
            if (handled == false)
            {
                result.AddRange(CreateDelegateAutoProperty(propertyInfo, delegateObjectName, delegatePropertyInfo));
            }
            AfterCreateDelegateProperty(propertyInfo, delegateObjectName, delegatePropertyInfo, result);
            return result;
        }
        /// <summary>
        /// This method is called before creating a delegate property.
        /// </summary>
        /// <param name="propertyInfo">The PropertyInfo object representing the main property.</param>
        /// <param name="delegateObjectName">The name of the object containing the delegate property.</param>
        /// <param name="delegatePropertyInfo">The PropertyInfo object representing the delegate property.</param>
        /// <param name="codeLines">A list of string containing the code lines for the delegate property.</param>
        /// <param name="handled">A boolean flag indicating whether the event has been handled.</param>
        partial void BeforeCreateDelegateProperty(PropertyInfo propertyInfo, string delegateObjectName, PropertyInfo delegatePropertyInfo, List<string> codeLines, ref bool handled);
        /// <summary>
        /// This method is called after a delegate property is created.
        /// </summary>
        /// <param name="propertyInfo">The property info of the delegate.</param>
        /// <param name="delegateObjectName">The name of the delegate object.</param>
        /// <param name="delegatePropertyInfo">The property info of the delegate property.</param>
        /// <param name="codeLines">The list of code lines for the delegate.</param>
        partial void AfterCreateDelegateProperty(PropertyInfo propertyInfo, string delegateObjectName, PropertyInfo delegatePropertyInfo, List<string> codeLines);

        /// <summary>
        /// Creates an auto-implemented property with delegate backing, given the property information, delegate object name, and delegate property information.
        /// </summary>
        /// <param name="propertyInfo">The information of the property.</param>
        /// <param name="delegateObjectName">The name of the delegate object.</param>
        /// <param name="delegatePropertyInfo">The information of the delegate property.</param>
        /// <returns>An enumerable collection of generated code for the auto-implemented property.</returns>
        public virtual IEnumerable<string> CreateDelegateAutoProperty(PropertyInfo propertyInfo, string delegateObjectName, PropertyInfo delegatePropertyInfo)
        {
            var result = new List<string>();
            var propertyType = GetPropertyType(propertyInfo);

            result.Add(string.Empty);
            result.AddRange(CreateComment(propertyInfo));
            CreatePropertyAttributes(propertyInfo, result);
            result.Add($"public {propertyType} {propertyInfo.Name}");
            result.Add("{");
            if (propertyInfo.CanRead)
            {
                result.AddRange(CreateDelegateAutoGet(propertyInfo, delegateObjectName, delegatePropertyInfo));
            }
            if (propertyInfo.CanWrite)
            {
                result.AddRange(CreateDelegateAutoSet(propertyInfo, delegateObjectName, delegatePropertyInfo));
            }
            result.Add("}");
            return result;
        }
        /// <summary>
        /// Creates a delegate for auto-getting a property using the provided <paramref name="propertyInfo"/>, <paramref name="delegateObjectName"/>, and <paramref name="delegatePropertyInfo"/>.
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/> of the property to get.</param>
        /// <param name="delegateObjectName">The name of the delegate object.</param>
        /// <param name="delegatePropertyInfo">The <see cref="PropertyInfo"/> of the property in the delegate object to get.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="string"/> containing the generated source code for the auto-get delegate.</returns>
        public virtual IEnumerable<string> CreateDelegateAutoGet(PropertyInfo propertyInfo, string delegateObjectName, PropertyInfo delegatePropertyInfo)
        {
            var visibility = propertyInfo.GetGetMethod(true)!.IsPublic ? string.Empty : "internal ";

            return new[] { $"{visibility}get => {delegateObjectName}.{delegatePropertyInfo.Name};" };
        }
        /// <summary>
        /// Creates a delegate that will automatically set the value of a property using the provided delegate object and property information.
        /// </summary>
        /// <param name="propertyInfo">The PropertyInfo of the property whose value needs to be set.</param>
        /// <param name="delegateObjectName">The name of the delegate object that will be used to set the property value.</param>
        /// <param name="delegatePropertyInfo">The PropertyInfo of the property that will store the value.</param>
        /// <returns>An IEnumerable&lt;string&gt; containing the generated code for the delegate.</returns>
        public virtual IEnumerable<string> CreateDelegateAutoSet(PropertyInfo propertyInfo, string delegateObjectName, PropertyInfo delegatePropertyInfo)
        {
            var visibility = propertyInfo.GetSetMethod(true)!.IsPublic ? string.Empty : "internal ";

            return new[] { $"{visibility}set => {delegateObjectName}.{delegatePropertyInfo.Name} = value;" };
        }
        /// <summary>
        /// Creates XML documentation for the method.
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/> object representing the property.</param>
        /// <param name="delegateObjectName">The name of the delegate object.</param>
        /// <param name="delegatePropertyInfo">The <see cref="PropertyInfo"/> object representing the delegate property.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="string"/> containing the generated documentation.</returns>
        public virtual IEnumerable<string> CreateDelegatePartialProperty(PropertyInfo propertyInfo, string delegateObjectName, PropertyInfo delegatePropertyInfo)
        {
            var result = new List<string>();
            var fieldType = GetPropertyType(propertyInfo);
            var paramName = CreateParameterName(propertyInfo);

            result.Add(string.Empty);
            CreatePropertyAttributes(propertyInfo, result);
            result.Add($"public {fieldType} {propertyInfo.Name}");
            result.Add("{");
            result.AddRange(CreateDelegatePartialGetProperty(propertyInfo, delegateObjectName, delegatePropertyInfo));
            result.AddRange(CreateDelegatePartialSetProperty(propertyInfo, delegateObjectName, delegatePropertyInfo));
            result.Add("}");

            result.Add($"partial void On{propertyInfo.Name}Reading();");
            result.Add($"partial void On{propertyInfo.Name}Changing(ref bool handled, {fieldType} value, ref {fieldType} {paramName});");
            result.Add($"partial void On{propertyInfo.Name}Changed();");
            return result;
        }
        /// <summary>
        /// Creates a partial get property delegate for the specified property information and delegate object.
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/> of the property to create the delegate for.</param>
        /// <param name="delegateObjectName">The name of the delegate object.</param>
        /// <param name="delegatePropertyInfo">The <see cref="PropertyInfo"/> of the delegate property.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the lines of code for the created get property delegate.</returns>
        public virtual IEnumerable<string> CreateDelegatePartialGetProperty(PropertyInfo propertyInfo, string delegateObjectName, PropertyInfo delegatePropertyInfo)
        {
            var result = new List<string>();

            CreateGetPropertyAttributes(propertyInfo, result);
            result.Add("get");
            result.Add("{");
            result.Add($"On{propertyInfo.Name}Reading();");
            result.Add($"return {delegateObjectName}.{delegatePropertyInfo.Name};");
            result.Add("}");
            return result;
        }
        /// <summary>
        /// Creates a partial set property delegate for a given property.
        /// </summary>
        /// <param name="propertyInfo">The PropertyInfo object representing the property.</param>
        /// <param name="delegateObjectName">The name of the object containing the delegate property.</param>
        /// <param name="delegatePropertyInfo">The PropertyInfo object representing the delegate property.</param>
        /// <returns>An IEnumerable of strings representing the generated code for the set property delegate.</returns>
        public virtual IEnumerable<string> CreateDelegatePartialSetProperty(PropertyInfo propertyInfo, string delegateObjectName, PropertyInfo delegatePropertyInfo)
        {
            var result = new List<string>();
            var propName = propertyInfo.Name;
            var fieldType = GetPropertyType(propertyInfo);
            var fieldName = CreateFieldName(propertyInfo, "_");
            var defaultValue = GetDefaultValue(propertyInfo);

            CreateSetPropertyAttributes(propertyInfo, result);
            result.Add("set");
            result.Add("{");
            result.Add("bool handled = false;");
            result.Add(string.IsNullOrEmpty(defaultValue)
            ? $"{fieldType} {fieldName} = default;"
            : $"{fieldType} {fieldName} = {defaultValue};");
            result.Add($"On{propName}Changing(ref handled, value, ref {fieldName});");
            result.Add("if (handled == false)");
            result.Add("{");
            result.Add($"{delegateObjectName}.{delegatePropertyInfo.Name} = value;");
            result.Add("}");
            result.Add("else");
            result.Add("{");
            result.Add($"{delegateObjectName}.{delegatePropertyInfo.Name} = {fieldName};");
            result.Add("}");
            result.Add($"On{propName}Changed();");
            result.Add("}");
            return result.ToArray();
        }
        #endregion delegate property helpers

        #region copy properties
        /// <summary>
        /// Copies the value of the specified property from the given object to this object.
        /// </summary>
        /// <param name="copyType">The type of object to copy the property from.</param>
        /// <param name="propertyInfo">The PropertyInfo object representing the property to copy.</param>
        /// <returns>A string representation of the copied property assignment.</returns>
        protected virtual string CopyProperty(string copyType, PropertyInfo propertyInfo)
        {
            return $"{propertyInfo.Name} = other.{propertyInfo.Name};";
        }
        /// <summary>
        /// Creates a copy of the properties of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="visibility">The visibility of the copy properties method.</param>
        /// <param name="type">The type whose properties will be copied.</param>
        /// <param name="copyType">The type to which the properties will be copied.</param>
        /// <param name="filter">An optional filter function to determine which properties to copy.</param>
        /// <returns>
        /// An enumerable collection of strings representing the generated copy properties method.
        /// </returns>
        public virtual IEnumerable<string> CreateCopyProperties(string visibility, Type type, string copyType, Func<PropertyInfo, bool>? filter = null)
        {
#pragma warning disable IDE0028 // Simplify collection initialization
            var result = new List<string>(CreateComment(type));
#pragma warning restore IDE0028 // Simplify collection initialization

            result.Add($"{visibility} void CopyProperties({copyType} other)");
            result.Add("{");
            result.Add("bool handled = false;");
            result.Add("BeforeCopyProperties(other, ref handled);");
            result.Add("if (handled == false)");
            result.Add("{");

            foreach (var item in type.GetAllPropertyInfos().Where(filter ?? (p => true)))
            {
                if (item.CanRead && CanCreate(item) && CanCopyProperty(item))
                {
                    result.Add(CopyProperty(copyType, item));
                }
            }

            result.Add("}");
            result.Add("AfterCopyProperties(other);");
            result.Add("}");

            result.Add($"partial void BeforeCopyProperties({copyType} other, ref bool handled);");
            result.Add($"partial void AfterCopyProperties({copyType} other);");
            return result.Where(l => string.IsNullOrEmpty(l) == false);
        }
        /// <summary>
        /// Copies the value of a delegate property from one object to another.
        /// </summary>
        /// <param name="copyType">The type of copy operation.</param>
        /// <param name="propertyInfo">The PropertyInfo of the delegate property.</param>
        /// <returns>A string representation of the copied delegate value.</returns>
        protected virtual string CopyDelegateProperty(string copyType, PropertyInfo propertyInfo)
        {
            return $"{propertyInfo.Name} = other.{propertyInfo.Name};";
        }
        /// <summary>
        /// Creates a copy of delegate properties for a given visibility and type.
        /// </summary>
        /// <param name="visibility">The visibility level of the properties (e.g., public, private, etc.).</param>
        /// <param name="type">The type of the object whose properties will be copied.</param>
        /// <returns>An enumerable collection of string representing the copied delegate properties.</returns>
        public virtual IEnumerable<string> CreateDelegateCopyProperties(string visibility, Type type)
        {
            return CreateDelegateCopyProperties(visibility, type, type.FullName ?? string.Empty);
        }
        /// <summary>
        ///     Creates a delegate to copy properties from one object instance to another.
        /// </summary>
        /// <param name="visibility">The visibility of the generated method.</param>
        /// <param name="type">The type of the object whose properties are copied.</param>
        /// <param name="copyType">The type of the object to copy the properties into.</param>
        /// <param name="filter">Optional filter to specify which properties to copy.</param>
        /// <returns>
        ///     An enumerable collection of strings representing the generated code for the copy properties delegate method.
        /// </returns>
        /// <remarks>
        ///     This method generates a delegate method for copying properties from one object instance to another,
        ///     based on the specified parameters. By providing the desired visibility for the generated method,
        ///     the generated method will have the specified visibility modifier.
        ///     The specified type indicates the type of object whose properties are copied, while the copyType
        ///     parameter denotes the type of object to copy the properties into.
        ///     An optional filter parameter can be passed to selectively copy properties based on specific conditions.
        ///     The generated code for the copy properties delegate method is then returned as an enumerable collection
        ///     of strings.
        ///     Before performing the copy operation, the delegate method invokes the BeforeCopyProperties method, which
        ///     can be partially implemented to customize the behavior before the copy operation takes place.
        ///     After the copy operation, the delegate method invokes the AfterCopyProperties method, which can also be
        ///     partially implemented to perform any necessary post-copy operations.
        /// </remarks>
        public virtual IEnumerable<string> CreateDelegateCopyProperties(string visibility, Type type, string copyType, Func<PropertyInfo, bool>? filter = null)
        {
#pragma warning disable IDE0028 // Simplify collection initialization
            var result = new List<string>(CreateComment(type));
#pragma warning restore IDE0028 // Simplify collection initialization

            result.Add($"{visibility} void CopyProperties({copyType} other)");
            result.Add("{");
            result.Add("bool handled = false;");
            result.Add("BeforeCopyProperties(other, ref handled);");
            result.Add("if (handled == false)");
            result.Add("{");

            foreach (var item in type.GetAllPropertyInfos().Where(filter ?? (p => true)))
            {
                if (item.CanRead && item.CanWrite && CanCreate(item))
                {
                    result.Add(CopyDelegateProperty(copyType, item));
                }
            }

            result.Add("}");
            result.Add("AfterCopyProperties(other);");
            result.Add("}");

            result.Add($"partial void BeforeCopyProperties({copyType} other, ref bool handled);");
            result.Add($"partial void AfterCopyProperties({copyType} other);");
            return result.Where(l => string.IsNullOrEmpty(l) == false);
        }
        #endregion copy properties

        /// <summary>
        /// Creates an XML documentation for the method <c>CreateEquals</c>.
        /// </summary>
        /// <param name="type">The <see cref="System.Type"/> parameter representing the type.</param>
        /// <param name="otherType">The <see cref="System.String"/> parameter representing the other type.</param>
        /// <returns>An <see cref="System.Collections.Generic.IEnumerable{System.String}"/> containing the XML documentation for the method.</returns>
        public virtual IEnumerable<string> CreateEquals(Type type, string otherType)
        {
            var result = new List<string>();
            var counter = 0;
            var typeProperties = type.GetAllPropertyInfos();
            var filteredProperties = typeProperties.Where(e => StaticLiterals.VersionProperties.Any(p => p.Equals(e.Name)));

            if (filteredProperties.Any())
            {
                result.AddRange(CreateComment(type));
                result.Add($"public override bool Equals(object? obj)");
                result.Add("{");
                result.Add("bool result = false;");
                result.Add($"if (obj is {otherType} other)");
                result.Add("{");

                foreach (var pi in filteredProperties)
                {
                    if (pi.CanRead)
                    {
                        var codeLine = counter == 0 ? "result = " : "       && ";

                        if (pi.PropertyType.IsValueType)
                        {
                            codeLine += $"{pi.Name} == other.{pi.Name}";
                        }
                        else
                        {
                            codeLine += $"IsEqualsWith({pi.Name}, other.{pi.Name})";
                        }
                        result.Add(codeLine);
                        counter++;
                    }
                }

                if (counter > 0)
                {
                    result[^1] = $"{result[^1]};";
                }
                result.Add("}");
                result.Add("return result;");
                result.Add("}");
            }
            return result;
        }
        /// <summary>
        /// Creates the XML documentation for the <see cref="CreateGetHashCode"/> method.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to create the GetHashCode method for.</param>
        /// <returns>An <see cref="IEnumerable{string}"/> containing the XML documentation.</returns>
        public virtual IEnumerable<string> CreateGetHashCode(Type type)
        {
            var result = new List<string>();

            var braces = 0;
            var counter = 0;
            var codeLine = string.Empty;
            var properties = type.GetAllPropertyInfos();
            var filteredProperties = properties;

            if (filteredProperties.Any())
            {
                result.AddRange(CreateComment(type));
                result.Add($"public override int GetHashCode()");
                result.Add("{");

                foreach (var pi in filteredProperties)
                {
                    if (pi.CanRead && CanCreate(pi))
                    {
                        if (counter == 0)
                        {
                            braces++;
                            codeLine = "this.CalculateHashCode(";
                        }
                        else
                        {
                            codeLine += ", ";
                        }
                        codeLine += pi.Name;
                        counter++;
                    }
                }
                for (int i = 0; i < braces; i++)
                {
                    codeLine += ")";
                }

                if (counter > 0)
                {
                    result.Add($"return {codeLine};");
                }
                else
                {
                    result.Add($"return base.GetHashCode();");
                }
                result.Add("}");
            }
            return result;
        }

        #region query settings
        /// <summary>
        /// Queries the setting value based on specified parameters and returns the value of type <typeparamref name="T"/>.
        /// If an error occurs during the query or conversion, the default value of type <typeparamref name="T"/> is returned,
        /// and the error is logged in the debug output window.
        /// </summary>
        /// <typeparam name="T">The type of the setting value to query and return.</typeparam>
        /// <param name="unitType">The unit type.</param>
        /// <param name="itemType">The item type.</param>
        /// <param name="type">The type.</param>
        /// <param name="valueName">The name of the value to query.</param>
        /// <param name="defaultValue">The default value to return if query or conversion fails.</param>
        /// <returns>The value of type <typeparamref name="T"/> for the specified setting, or the default value if an error occurs.</returns>
        protected T QuerySetting<T>(UnitType unitType, ItemType itemType, Type type, string valueName, string defaultValue)
        {
            T result;

            try
            {
                result = (T)Convert.ChangeType(QueryGenerationSettingValue(unitType, itemType, ItemProperties.CreateSubTypeFromEntity(type), valueName, defaultValue), typeof(T));
            }
            catch (Exception ex)
            {
                result = (T)Convert.ChangeType(defaultValue, typeof(T));
                System.Diagnostics.Debug.WriteLine($"Error in {MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
            }
            return result;
        }
        /// <summary>
        ///     Retrieves a setting of type T based on the given parameters.
        /// </summary>
        /// <typeparam name="T">The type of the setting to retrieve.</typeparam>
        /// <param name="unitType">The unit type.</param>
        /// <param name="itemType">The item type.</param>
        /// <param name="type">The type of the item.</param>
        /// <param name="itemSubName">The subname of the item.</param>
        /// <param name="valueName">The name of the value.</param>
        /// <param name="defaultValue">The default value for the setting.</param>
        /// <returns>
        ///     The setting of type T.
        /// </returns>
        /// <exception cref="System.Exception">
        ///     Thrown if an error occurs while retrieving the setting.
        /// </exception>
        /// <remarks>
        ///     If an error occurs while retrieving the setting, the default value of type T
        ///     will be returned, and the error will be logged using the Debug.WriteLine method.
        /// </remarks>
        protected T QuerySetting<T>(UnitType unitType, ItemType itemType, Type type, string itemSubName, string valueName, string defaultValue)
        {
            T result;

            try
            {
                result = (T)Convert.ChangeType(QueryGenerationSettingValue(unitType, itemType, $"{ItemProperties.CreateSubTypeFromEntity(type)}.{itemSubName}", valueName, defaultValue), typeof(T));
            }
            catch (Exception ex)
            {
                result = (T)Convert.ChangeType(defaultValue, typeof(T));
                System.Diagnostics.Debug.WriteLine($"Error in {MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
            }
            return result;
        }
        /// <summary>
        /// Queries the setting value based on the specified parameters.
        /// </summary>
        /// <typeparam name="T">The type of the setting value.</typeparam>
        /// <param name="unitType">The unit type.</param>
        /// <param name="itemType">The item type.</param>
        /// <param name="itemName">The name of the item.</param>
        /// <param name="valueName">The name of the value.</param>
        /// <param name="defaultValue">The default value to be used if the query fails.</param>
        /// <returns>The queried setting value of type T.</returns>
        /// <remarks>
        /// This method queries the setting value for the specified unit type, item type, item name, value name,
        /// and default value using the QueryGenerationSettingValue method. It attempts to convert the result
        /// to type T using Convert.ChangeType. If any exception occurs during the process, the method catches
        /// the exception, assigns the default value of type T, and writes an error message to the Debug output.
        /// The final result is returned.
        /// </remarks>
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
