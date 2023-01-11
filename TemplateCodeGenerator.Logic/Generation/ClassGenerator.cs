//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic.Generation
{
    using System.Reflection;
    using TemplateCodeGenerator.Logic.Contracts;
    internal partial class ClassGenerator : GeneratorObject
    {
        protected ClassGenerator(ISolutionProperties solutionProperties)
            : base(solutionProperties)
        {

        }
        public static ClassGenerator Create(ISolutionProperties solutionProperties)
        {
            return new ClassGenerator(solutionProperties);
        }

        #region Create constructors
        public virtual IEnumerable<string> CreatePartialStaticConstrutor(string className, IEnumerable<string>? initStatements = null)
        {
            var lines = new List<string>(CreateComment())
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
        public virtual IEnumerable<string> CreatePartialConstrutor(string visibility, string className, string? argumentList = null, string? baseConstructor = null, IEnumerable<string>? initStatements = null, bool withPartials = true)
        {
            var lines = new List<string>(CreateComment())
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
        #endregion Create constructors

        #region Create factory methode
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
        public IEnumerable<string> CreateDelegateFactoryMethods(string itemType, string delegateName, bool isPublic, bool newPrefix)
        {
            var result = new List<string>(CreateComment());

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
        #endregion Create factory methode

        #region Create property
        protected virtual void CreatePropertyAttributes(PropertyInfo propertyInfo, List<string> codeLines) { }
        protected virtual void CreateGetPropertyAttributes(PropertyInfo propertyInfo, List<string> codeLines) { }
        protected virtual void CreateSetPropertyAttributes(PropertyInfo propertyInfo, List<string> codeLines) { }
        protected virtual void GetPropertyDefaultValue(PropertyInfo propertyInfo, ref string defaultValue) { }

        /// <summary>
        /// Diese Methode erstellt den Programmcode einer Eigenschaft (Auto-Property oder Partial-Full-Property).
        /// </summary>
        /// <param name="propertyInfo">Property info object</param>
        /// <returns>Die Eigenschaft als Text</returns>
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
        partial void BeforeCreateProperty(Type type, PropertyInfo propertyInfo, List<string> codeLines, ref bool handled);
        partial void AfterCreateProperty(Type type, PropertyInfo propertyInfo, List<string> codeLines);

        /// <summary>
        /// Diese Methode erstellt den Programmcode einer Eigenschaft (Auto-Property).
        /// </summary>
        /// <param name="propertyInfo">Property info object</param>
        /// <returns>Die Eigenschaft als Text</returns>
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
            result.Add(property);
            return result;
        }
        /// <summary>
        /// Diese Methode erstellt den Programmcode einer Eigenschaft (Partial-Full-Property).
        /// </summary>
        /// <param name="propertyInfo">Property info object</param>
        /// <returns>Die Eigenschaft als Text</returns>
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
                ? $"private {fieldType} {fieldName}{(IsListType(propertyInfo.PropertyType) ? " = new();" : ";")}"
                : $"private {fieldType} {fieldName} = {defaultValue};");

            result.Add($"partial void On{propertyInfo.Name}Reading();");
            result.Add($"partial void On{propertyInfo.Name}Changing(ref bool handled, {fieldType} value, ref {fieldType} {paramName});");
            result.Add($"partial void On{propertyInfo.Name}Changed();");
            return result;
        }
        /// <summary>
        /// Diese Methode erstellt den Programmcode einer Getter-Eigenschaft aus dem Eigenschaftsinfo-Objekt.
        /// </summary>
        /// <param name="propertyInfo">Property info object</param>
        /// <returns>Die Getter-Eigenschaft als Text</returns>
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
        /// Diese Methode erstellt den Programmcode einer Setter-Eigenschaft aus dem Eigenschaftsinfo-Objekt.
        /// </summary>
        /// <param name="propertyInfo">Property info object</param>
        /// <returns>Die Setter-Eigenschaft als Text.</returns>
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
        #endregion Create properties

        #region Delegate property helpers
        /// <summary>
        /// Diese Methode erstellt den Programmcode einer Delegate-Eigenschaft (Auto-Property oder Partial-Full-Property).
        /// </summary>
        /// <param name="propertyInfo">Eigenschafts-Helper-Objekt</param>
        /// <param name="delegateObjectName">Variablenname vom Delegat-Objekt</param>
        /// <param name="delegatePropertyInfo">Eigenschafts-Helper-Objekt</param>
        /// <returns>Die Eigenschaft als Text.</returns>
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
        partial void BeforeCreateDelegateProperty(PropertyInfo propertyInfo, string delegateObjectName, PropertyInfo delegatePropertyInfo, List<string> codeLines, ref bool handled);
        partial void AfterCreateDelegateProperty(PropertyInfo propertyInfo, string delegateObjectName, PropertyInfo delegatePropertyInfo, List<string> codeLines);

        /// <summary>
        /// Diese Methode erstellt den Programmcode einer Delegate-Eigenschaft (Auto-Property).
        /// </summary>
        /// <param name="propertyInfo">Eigenschafts-Helper-Objekt</param>
        /// <param name="delegateObjectName">Variablenname vom Delegat-Objekt</param>
        /// <param name="delegatePropertyInfo">Eigenschafts-Helper-Objekt</param>
        /// <returns>Die Eigenschaft als Text.</returns>
        public virtual IEnumerable<string> CreateDelegateAutoProperty(PropertyInfo propertyInfo, string delegateObjectName, PropertyInfo delegatePropertyInfo)
        {
            var result = new List<string>();
            var propertyType = GetPropertyType(propertyInfo);

            result.Add(string.Empty);
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
        public virtual IEnumerable<string> CreateDelegateAutoGet(PropertyInfo propertyInfo, string delegateObjectName, PropertyInfo delegatePropertyInfo)
        {
            var visibility = propertyInfo.GetGetMethod(true)!.IsPublic ? string.Empty : "internal ";

            return new[] { $"{visibility}get => {delegateObjectName}.{delegatePropertyInfo.Name};" };
        }
        public virtual IEnumerable<string> CreateDelegateAutoSet(PropertyInfo propertyInfo, string delegateObjectName, PropertyInfo delegatePropertyInfo)
        {
            var visibility = propertyInfo.GetSetMethod(true)!.IsPublic ? string.Empty : "internal ";

            return new[] { $"{visibility}set => {delegateObjectName}.{delegatePropertyInfo.Name} = value;" };
        }
        /// <summary>
        /// Diese Methode erstellt den Programmcode einer Delegate-Eigenschaft (Partial-Full-Property).
        /// </summary>
        /// <param name="propertyInfo">Eigenschafts-Helper-Objekt</param>
        /// <param name="delegateObjectName">Variablenname vom Delegat-Objekt</param>
        /// <param name="delegatePropertyInfo">Eigenschafts-Helper-Objekt</param>
        /// <returns>Die Eigenschaft als Text.</returns>
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
        /// Diese Methode erstellt den Programmcode einer Getter-Delegate-Eigenschaft aus dem Eigenschaftsinfo-Objekt.
        /// </summary>
        /// <param name="propertyInfo">Eigenschafts-Helper-Objekt</param>
        /// <param name="delegateObjectName">Variablenname vom Delegat-Objekt</param>
        /// <param name="delegatePropertyInfo">Eigenschafts-Helper-Objekt</param>
        /// <returns>Die Getter-Eigenschaft als Text.</returns>
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
        /// Diese Methode erstellt den Programmcode einer Setter-Delegate-Eigenschaft aus dem Eigenschaftsinfo-Objekt.
        /// </summary>
        /// <param name="propertyInfo">Eigenschafts-Helper-Objekt</param>
        /// <param name="delegateObjectName">Variablenname vom Delegat-Objekt</param>
        /// <param name="delegatePropertyInfo">Eigenschafts-Helper-Objekt</param>
        /// <returns>Die Setter-Eigenschaft als Text.</returns>
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
        #endregion Delegate property helpers

        #region CopyProperties
        protected virtual bool CanCopyProperty(PropertyInfo propertyInfo) => true;
        protected virtual string CopyProperty(string copyType, PropertyInfo propertyInfo)
        {
            return $"{propertyInfo.Name} = other.{propertyInfo.Name};";
        }
        public virtual IEnumerable<string> CreateCopyProperties(string visibility, Type type, string copyType, Func<PropertyInfo, bool>? filter = null)
        {
            var result = new List<string>(CreateComment(type));

            result.Add($"{visibility} void CopyProperties({copyType} other)");
            result.Add("{");
            result.Add("bool handled = false;");
            result.Add("BeforeCopyProperties(other, ref handled);");
            result.Add("if (handled == false)");
            result.Add("{");

            foreach (var item in type.GetAllPropertyInfos().Where(filter ?? (p => true)))
            {
                if (item.CanRead && CanCopyProperty(item))
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
        protected virtual string CopyDelegateProperty(string copyType, PropertyInfo propertyInfo)
        {
            return $"{propertyInfo.Name} = other.{propertyInfo.Name};";
        }
        public virtual IEnumerable<string> CreateDelegateCopyProperties(string visibility, Type type)
        {
            return CreateDelegateCopyProperties(visibility, type, type.FullName ?? string.Empty);
        }
        public virtual IEnumerable<string> CreateDelegateCopyProperties(string visibility, Type type, string copyType, Func<PropertyInfo, bool>? filter = null)
        {
            var result = new List<string>(CreateComment(type));

            result.Add($"{visibility} void CopyProperties({copyType} other)");
            result.Add("{");
            result.Add("bool handled = false;");
            result.Add("BeforeCopyProperties(other, ref handled);");
            result.Add("if (handled == false)");
            result.Add("{");

            foreach (var item in type.GetAllPropertyInfos().Where(filter ?? (p => true)))
            {
                if (item.CanRead && item.CanWrite)
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
        #endregion CopyProperties

        /// <summary>
        /// Diese Methode erstellt den Programmcode fuer das Vergleichen der Eigenschaften.
        /// </summary>
        /// <param name="type">Die Schnittstellen-Typ Information.</param>
        /// <returns>Die Equals-Methode als Text.</returns>
        public virtual IEnumerable<string> OverrideEquals(Type type)
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
                result.Add($"if (obj is {ItemProperties.CreateModelSubType(type)} other)");
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
        /// Diese Methode erstellt den Programmcode fuer die Berechnung des Hash-Codes.
        /// </summary>
        /// <param name="type">Die Schnittstellen-Typ Information.</param>
        /// <returns>Die GetHashCode-Methode als Text.</returns>
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
                    if (pi.CanRead)
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
    }
}
//MdEnd