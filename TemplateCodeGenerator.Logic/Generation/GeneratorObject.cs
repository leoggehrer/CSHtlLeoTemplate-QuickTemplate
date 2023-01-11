//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic.Generation
{
    using System.Reflection;
    using TemplateCodeGenerator.Logic.Common;
    using TemplateCodeGenerator.Logic.Contracts;
    using TemplateCodeGenerator.Logic.Extensions;
    internal abstract partial class GeneratorObject
    {
        static GeneratorObject()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        public Configuration Configuration { get; init; }
        public ISolutionProperties SolutionProperties { get; init; }

        public GeneratorObject(ISolutionProperties solutionProperties)
        {
            Constructing();
            SolutionProperties = solutionProperties;
            Configuration = new Configuration(solutionProperties);
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();

        public string QueryGenerationSettingValue(string unitType, string itemType, string itemName, string valueName, string defaultValue)
        {
            return Configuration.QuerySettingValue(unitType, itemType, itemName, valueName, defaultValue);
        }
        public string QueryGenerationSettingValue(UnitType unitType, ItemType itemType, string itemName, string valueName, string defaultValue)
        {
            return Configuration.QuerySettingValue(unitType, itemType, itemName, valueName, defaultValue);
        }

        #region Helpers
        #region Namespace-Helpers
        public static IEnumerable<string> EnvelopeWithANamespace(IEnumerable<string> source, string nameSpace, params string[] usings)
        {
            var result = new List<string>();

            if (nameSpace.HasContent())
            {
                result.Add($"namespace {nameSpace}");
                result.Add("{");
                result.AddRange(usings);
            }
            result.AddRange(source);
            if (nameSpace.HasContent())
            {
                result.Add("}");
            }
            return result;
        }
        #endregion Namespace-Helpers

        #region Assemply-Helpers
        public static IEnumerable<Type> GetEntityTypes(Assembly assembly)
        {
            return assembly.GetTypes()
                           .Where(t => t.IsInterface == false
                                    && (t.BaseType != null && t.BaseType.Name.Equals(StaticLiterals.EntityObjectName)
                                        || t.BaseType != null && t.BaseType.Name.Equals(StaticLiterals.VersionEntityName)));
        }
        #endregion Assembly-Helpers

        public static bool IsArrayType(Type type)
        {
            return type.IsArray;
        }
        public static bool IsListType(Type type)
        {
            return type.FullName!.StartsWith("System.Collections.Generic.List");
        }

        /// <summary>
        /// Diese Methode ermittelt den Solutionname aus seinem Schnittstellen Typ.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>Schema der Entitaet.</returns>
        public static string GetSolutionNameFromType(Type type)
        {
            var result = string.Empty;
            var data = type.Namespace?.Split('.');

            if (data?.Length > 0)
            {
                result = data[0];
            }
            return result;
        }
        /// <summary>
        /// Diese Methode ermittelt den Teilnamensraum aus einem Typ.
        /// </summary>
        /// <param name="type">Typ</param>
        /// <returns>Teil-Namensraum</returns>
        public static string CreateSubNamespaceFromType(Type type)
        {
            var result = string.Empty;
            var data = type.Namespace?.Split('.');

            for (var i = 2; i < data?.Length; i++)
            {
                if (string.IsNullOrEmpty(result))
                {
                    result = $"{data[i]}";
                }
                else
                {
                    result = $"{result}.{data[i]}";
                }
            }
            return result;
        }
        /// <summary>
        /// Diese Methode ermittelt den Teilnamensraum aus einem Typ.
        /// </summary>
        /// <param name="type">Typ</param>
        /// <returns>Teil-Namensraum</returns>
        public static string CreateSubNamespaceFromEntityType(Type type)
        {
            var result = CreateSubNamespaceFromType(type);

            if (result.Equals(StaticLiterals.EntitiesFolder))
            {
                result = string.Empty;
            }
            return result.Replace($"{StaticLiterals.EntitiesFolder}.", string.Empty);
        }
        /// <summary>
        /// Diese Methode ermittelt den Teil-Path aus einem Typ.
        /// </summary>
        /// <param name="type">Typ</param>
        /// <returns>Teil-Path</returns>
        public static string CreateSubPathFromType(Type type)
        {
            return CreateSubNamespaceFromType(type).Replace(".", "/");
        }

        /// <summary>
        /// Diese Methode ermittelt den Entity Namen aus seinem Typ.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>Name der Entitaet.</returns>
        public static string CreateEntityNameFromType(Type type)
        {
            return type.Name;
        }
        /// <summary>
        /// Diese Methode ermittelt den Model Namen aus seinem Typ.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>Name des Models.</returns>
        public static string CreateModelName(Type type)
        {
            return type.Name;
        }
        /// <summary>
        /// Diese Methode ermittelt den Entity-Typ aus seiner Type (eg. Entities.App.Type).
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>Typ der Entitaet.</returns>
        public static string CreateEntityTypeFromType(Type type)
        {
            var entityName = CreateEntityNameFromType(type);

            return $"{CreateSubNamespaceFromType(type)}.{entityName}";
        }
        /// <summary>
        /// Diese Methode ermittelt den Entity-Typ aus seiner Type (eg. App.Type).
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>Typ der Entitaet.</returns>
        public static string CreateEntitiesSubTypeFromType(Type type)
        {
            var entityName = CreateEntityNameFromType(type);

            return $"{CreateSubNamespaceFromType(type)}.{entityName}".Replace($"{StaticLiterals.EntitiesFolder}.", string.Empty);
        }
        /// <summary>
        /// Diese Methode ermittelt den Entity Namen aus seinem Schnittstellen Typ.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>Name der Entitaet.</returns>
        public static string CreateEntityFullNameFromType(Type type)
        {
            var result = string.Empty;

            if (type.FullName != null)
            {
                var entityName = CreateEntityNameFromType(type);

                result = type.FullName.Replace(type.Name, entityName);
                result = result.Replace(".Contracts", ".Logic.Entities");
            }
            return result;
        }
        /// <summary>
        /// Diese Methode ermittelt den Teil-Pfad aus der Schnittstelle.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <param name="pathPrefix">Ein optionaler Pfad-Prefix.</param>
        /// <param name="filePostfix">Ein optionaler Datei-Postfix.</param>
        /// <param name="fileExtension">Die Datei-Extension.</param>
        /// <returns></returns>
        public static string CreateSubFilePathFromType(Type type, string pathPrefix, string filePostfix, string fileExtension)
        {
            var entityName = CreateEntityNameFromType(type);

            string? result;
            if (pathPrefix.IsNullOrEmpty())
            {
                result = CreateSubPathFromType(type);
            }
            else
            {
                result = Path.Combine(pathPrefix, CreateSubPathFromType(type));
            }
            result = Path.Combine(result, $"{entityName}{filePostfix}{fileExtension}");
            return result;
        }
        /// <summary>
        /// Diese Methode ermittelt den Teil-Pfad aus der Schnittstelle.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <param name="pathPrefix">Ein optionaler Pfad-Prefix.</param>
        /// <param name="filePostfix">Ein optionaler Datei-Postfix.</param>
        /// <param name="fileExtension">Die Datei-Extension.</param>
        /// <returns></returns>
        public static string CreatePluralSubFilePathFromInterface(Type type, string pathPrefix, string filePostfix, string fileExtension)
        {
            var result = string.Empty;

            if (type.IsInterface)
            {
                var entityName = CreateEntityNameFromType(type);

                if (pathPrefix.IsNullOrEmpty())
                {
                    result = CreateSubPathFromType(type);
                }
                else
                {
                    result = Path.Combine(pathPrefix, CreateSubPathFromType(type));
                }
                result = Path.Combine(result, $"{entityName.CreatePluralWord()}{filePostfix}{fileExtension}");
            }
            return result;
        }
        /// <summary>
        /// Diese Methode ermittelt den Kontroller Namen aus seinem Schnittstellen Typ.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>Name des Kontrollers.</returns>
        public static string CreateLogicControllerFullNameFromInterface(Type type)
        {
            var result = string.Empty;

            if (type.FullName != null)
            {
                var entityName = type.Name[1..];

                result = type.FullName.Replace(type.Name, entityName);
                result = result.Replace(".Contracts", ".Logic.Controllers");
                result = $"{result}Controller";
            }
            return result;
        }
        /// <summary>
        /// Diese Methode ermittelt den Kontroller Namen aus seinem Schnittstellen Typ.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>Name des Kontrollers.</returns>
        public static string CreateWebApiControllerFullNameFromInterface(Type type)
        {
            var result = string.Empty;

            if (type.FullName != null)
            {
                var entityName = $"{type.Name[1..]}s";

                result = type.FullName.Replace(type.Name, entityName);
                result = result.Replace(".Contracts", ".WebApi.Controllers");
                result = $"{result}Controller";
            }
            return result;
        }
        /// <summary>
        /// Diese Methode ermittelt den Kontroller Namen aus seinem Schnittstellen Typ.
        /// </summary>
        /// <param name="type">Schnittstellen-Typ</param>
        /// <returns>Name des Kontrollers.</returns>
        public static string CreateAspMvcControllerFullNameFromInterface(Type type)
        {
            var result = string.Empty;

            if (type.FullName != null)
            {
                var entityName = $"{type.Name[1..]}s";

                result = type.FullName.Replace(type.Name, entityName);
                result = result.Replace(".Contracts", ".AspMvc.Controllers");
                result = $"{result}Controller";
            }
            return result;
        }

        #region Comment-Helpers
        public virtual IEnumerable<string> CreateComment()
        {
            var result = new List<string>()
            {
                "///",
                "/// Generated by the generator",
                "///",
            };
            return result;
        }
        public virtual IEnumerable<string> CreateComment(Type type)
        {
            var result = new List<string>()
            {
                "///",
                "/// Generated by the generator",
                "///",
            };
            return result;
        }
        public virtual IEnumerable<string> CreateComment(PropertyInfo propertyInfo)
        {
            var result = new List<string>()
            {
                "///",
                "/// Generated by the generator",
                "///",
            };
            return result;
        }
        #endregion Comment-Helpers

        #region Property-Helpers
        /// <summary>
        /// Determines whether the property is a reference property.
        /// </summary>
        /// <param name="propertyInfo">The property</param>
        /// <returns>True if it is a reference property, false otherwise.</returns>
        public virtual bool IsReferenceProperty(PropertyInfo propertyInfo)
        {
            var result = false;
            var idText = "Id";

            if (propertyInfo.Name.Length > 2 && propertyInfo.Name.EndsWith(idText))
            {
                result = true;
            }
            else if (propertyInfo.Name.Contains($"{idText}_"))
            {
                var idx = propertyInfo.Name.IndexOf($"{idText}_");

                if (idx > 0 && idx + idText.Length + 1 < propertyInfo.Name.Length)
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Diese Methode konvertiert den Eigenschaftstyp in eine Zeichenfolge.
        /// </summary>
        /// <param name="propertyInfo">Das Eigenschaftsinfo-Objekt.</param>
        /// <returns>Der Eigenschaftstyp als Zeichenfolge.</returns>
        public virtual string GetPropertyType(PropertyInfo propertyInfo)
        {
            var nullable = propertyInfo.IsNullable();
            var result = IsReferenceProperty(propertyInfo) ? StaticLiterals.IdType : propertyInfo.PropertyType.GetCodeDefinition();

            if (nullable && result.EndsWith('?') == false)
            {
                result += '?';
            }
            return result;
        }
        /// <summary>
        /// Diese Methode ermittelt den Feldnamen der Eigenschaft.
        /// </summary>
        /// <param name="propertyInfo">Das Eigenschaftsinfo-Objekt.</param>
        /// <param name="prefix">Prefix der dem Namen vorgestellt ist.</param>
        /// <returns>Der Feldname als Zeichenfolge.</returns>
        public static string CreateFieldName(PropertyInfo propertyInfo, string prefix)
        {
            return $"{prefix}{char.ToLower(propertyInfo.Name.First())}{propertyInfo.Name[1..]}";
        }
        public static string GetDefaultValue(PropertyInfo propertyInfo)
        {
            string result = string.Empty;

            if (propertyInfo.IsNullable() == false)
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    result = "string.Empty";
                }
                else if (IsArrayType(propertyInfo.PropertyType))
                {
                    result = $"Array.Empty<{StaticLiterals.TProperty}>()";
                }
                else if (IsListType(propertyInfo.PropertyType))
                {
                    result = "new()";
                }
            }
            return result;
        }
        public static string CreateParameterName(PropertyInfo propertyInfo) => $"_{char.ToLower(propertyInfo.Name[0])}{propertyInfo.Name[1..]}";

        #endregion Property-Helpers
        #endregion Helpers
    }
}
//MdEnd