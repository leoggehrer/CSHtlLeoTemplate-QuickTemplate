//@BaseCode
//MdStart
using System.Reflection;

namespace TemplateCodeGenerator.Logic.Generation
{
    internal partial class ItemProperties
    {
        public string SolutionName { get; }
        public string ProjectExtension { get; }
        public string Namespace => $"{SolutionName}{ProjectExtension}";
        public ItemProperties(string solutionName, string projectExtension)
        {
            SolutionName = solutionName;
            ProjectExtension = projectExtension;
        }

        /// <summary>
        /// Generates the entity name from the type.
        /// </summary>
        /// <param name="type">The entity type.</param>
        /// <returns>The entity name.</returns>
        public static string CreateEntityName(Type type) => type.Name;
        /// <summary>
        /// Generates the typescript property name from the property info.
        /// </summary>
        /// <param name="type">The property info object.</param>
        /// <returns>The typescript property name.</returns>
        public static string CreateTSPropertyName (PropertyInfo propertyInfo) => $"{Char.ToLower(propertyInfo.Name[0])}{propertyInfo.Name[1..]}";

        public string _RemoveSolutionName(string itemName)
        {
            return itemName.Replace($"{SolutionName}.", string.Empty);
        }
        public string CreateSubType(Type type)
        {
            return type.FullName!.Replace($"{Namespace}.", string.Empty);
        }
        public static string CreateModelName(Type type) => type.Name;
        public string CreateModelType(Type type)
        {
            return $"{CreateModelNamespace(type)}.{type.Name}";
        }
        public static string CreateEditModelName(Type type)
        {
            return $"{CreateModelName(type)}Edit";
        }
        public string CreateEditModelType(Type type)
        {
            return $"{CreateModelNamespace(type)}.{CreateEditModelName(type)}";
        }
        public static string CreateFilterModelName(Type type)
        {
            return $"{CreateModelName(type)}Filter";
        }
        public string CreateFilterModelType(Type type)
        {
            return $"{CreateModelNamespace(type)}.{CreateFilterModelName(type)}";
        }

        public static string CreateModelSubType(Type type)
        {
            return $"{CreateModelSubNamespace(type)}.{type.Name}";
        }
        public string CreateModelNamespace(Type type)
        {
            return $"{Namespace}.{CreateModelSubNamespace(type)}";
        }
        public static string CreateModelSubPath(Type type, string postFix, string fileExtension)
        {
            return Path.Combine(CreateModelSubNamespace(type).Replace(".", Path.DirectorySeparatorChar.ToString()), $"{type.Name}{postFix}{fileExtension}");
        }

        public string ConvertEntityToModelType(string typeFullname)
        {
            var result = typeFullname;
            var entitiesFolder = $".{StaticLiterals.EntitiesFolder}.";
            var modelsFolder = $".{StaticLiterals.ModelsFolder}.";

            if (result.Contains(entitiesFolder))
            {
                result = result.Replace(entitiesFolder, modelsFolder);
                result = result.Replace(StaticLiterals.LogicExtension, ProjectExtension);
            }
            return result;
        }

        #region Contracts properties
        public string CreateAccessContractType(Type type)
        {
            return $"{CreateContractNamespace(type)}.{CreateAccessContractName(type)}";
        }
        public string CreateServiceContractType(Type type)
        {
            return $"{CreateContractNamespace(type)}.{CreateServiceContractName(type)}";
        }
        public static string CreateAccessContractName(Type type)
        {
            return $"I{type.Name.CreatePluralWord()}Access";
        }
        public static string CreateServiceContractName(Type type)
        {
            return $"I{type.Name.CreatePluralWord()}Service";
        }
        public static string CreateAccessContractSubType(Type type)
        {
            return $"{CreateContractSubNamespace(type)}.{CreateAccessContractName(type)}";
        }
        public static string CreateServiceContractSubType(Type type)
        {
            return $"{CreateContractSubNamespace(type)}.{CreateServiceContractName(type)}";
        }
        public static string CreateFacadeContractSubType(Type type)
        {
            return $"{CreateContractSubNamespace(type)}.{CreateAccessContractName(type)}";
        }
        public string CreateContractNamespace(Type type)
        {
            return $"{SolutionName}{StaticLiterals.LogicExtension}.{CreateContractSubNamespace(type)}";
        }
        public static string CreateContractSubNamespace(Type type)
        {
            var entitySubNamespace = CreateSubNamespaceFromEntityType(type);

            return entitySubNamespace.HasContent() ? $"{StaticLiterals.ContractsFolder}.{entitySubNamespace}" : StaticLiterals.ContractsFolder;
        }
        public static string CreateAccessContractSubPathFromType(Type type, string postFix, string fileExtension)
        {
            return Path.Combine(CreateContractSubNamespace(type).Replace(".", Path.DirectorySeparatorChar.ToString()), $"{CreateAccessContractName(type)}{postFix}{fileExtension}");
        }
        public static string CreateServiceContractSubPathFromType(Type type, string postFix, string fileExtension)
        {
            return Path.Combine(CreateContractSubNamespace(type).Replace(".", Path.DirectorySeparatorChar.ToString()), $"{CreateServiceContractName(type)}{postFix}{fileExtension}");
        }
        #endregion Contracts properties

        #region Controller and service properties
        public string CreateLogicControllerType(Type type)
        {
            return $"{SolutionName}{StaticLiterals.LogicExtension}.{CreateControllerSubType(type)}";
        }
        public static string CreateControllerName(Type type)
        {
            return $"{type.Name.CreatePluralWord()}";
        }
        public static string CreateControllerClassName(Type type)
        {
            return $"{CreateControllerName(type)}Controller";
        }
        public string CreateControllerType(Type type)
        {
            return $"{CreateControllerNamespace(type)}.{CreateControllerClassName(type)}";
        }
        public static string CreateControllerSubType(Type type)
        {
            return $"{CreateControllerSubNamespace(type)}.{CreateControllerClassName(type)}";
        }
        public string CreateControllerNamespace(Type type)
        {
            return $"{Namespace}.{CreateControllerSubNamespace(type)}";
        }
        public static string CreateControllerSubNamespace(Type type)
        {
            var subNamespace = CreateSubNamespaceFromEntityType(type);

            return subNamespace.HasContent() ? $"{StaticLiterals.ControllersFolder}.{subNamespace}" : StaticLiterals.ControllersFolder;
        }
        public static string CreateControllersSubPathFromType(Type type, string postFix, string fileExtension)
        {
            return Path.Combine(CreateControllerSubNamespace(type).Replace(".", Path.DirectorySeparatorChar.ToString()), $"{CreateControllerClassName(type)}{postFix}{fileExtension}");
        }
        #endregion Controller service properties

        #region Service properties
        public string CreateLogicServiceType(Type type)
        {
            return $"{SolutionName}{StaticLiterals.LogicExtension}.{CreateServiceSubType(type)}";
        }
        public static string CreateServiceName(Type type)
        {
            return $"{type.Name.CreatePluralWord()}";
        }
        public static string CreateServiceClassName(Type type)
        {
            return $"{CreateServiceName(type)}Service";
        }
        public string CreateServiceType(Type type)
        {
            return $"{CreateServiceNamespace(type)}.{CreateServiceClassName(type)}";
        }
        public static string CreateServiceSubType(Type type)
        {
            return $"{CreateServiceSubNamespace(type)}.{CreateServiceClassName(type)}";
        }
        public string CreateServiceNamespace(Type type)
        {
            return $"{Namespace}.{CreateServiceSubNamespace(type)}";
        }
        public static string CreateServiceSubNamespace(Type type)
        {
            var subNamespace = CreateSubNamespaceFromEntityType(type);

            return subNamespace.HasContent() ? $"{StaticLiterals.ServicesFolder}.{subNamespace}" : StaticLiterals.ServicesFolder;
        }
        public static string CreateServicesSubPathFromType(Type type, string postFix, string fileExtension)
        {
            return Path.Combine(CreateServiceSubNamespace(type).Replace(".", Path.DirectorySeparatorChar.ToString()), $"{CreateServiceClassName(type)}{postFix}{fileExtension}");
        }
        #endregion Service properties

        #region Facade properties
        public string CreateFacadeType(Type type)
        {
            return $"{CreateFacadeNamespace(type)}.{CreateFactoryFacadeMethodName(type)}";
        }
        public static string CreateFacadeSubType(Type type)
        {
            return $"{CreateFacadeSubNamespace(type)}.{CreateFactoryFacadeMethodName(type)}";
        }
        public string CreateFacadeNamespace(Type type)
        {
            return $"{Namespace}.{CreateFacadeSubNamespace(type)}";
        }
        public static string CreateFacadeSubNamespace(Type type)
        {
            var subNamespace = CreateSubNamespaceFromEntityType(type);

            return subNamespace.HasContent() ? $"{StaticLiterals.FacadesFolder}.{subNamespace}" : StaticLiterals.FacadesFolder;
        }
        public static string CreateFacadesSubPathFromType(Type type, string postFix, string fileExtension)
        {
            return Path.Combine(CreateFacadeSubNamespace(type).Replace(".", Path.DirectorySeparatorChar.ToString()), $"{CreateFactoryFacadeMethodName(type)}{postFix}{fileExtension}");
        }
        #endregion Facade properties

        #region Factory properties
        public static string CreateFactoryControllerMethodName(Type type)
        {
            return $"{type.Name.CreatePluralWord()}Controller";
        }
        public static string CreateFactoryFacadeMethodName(Type type)
        {
            return $"{type.Name.CreatePluralWord()}Facade";
        }
        #endregion Factory properties

        #region View properties
        public static string CreateViewSubPathFromType(Type type, string fileName, string fileExtension)
        {
            return Path.Combine(StaticLiterals.ViewsFolder, $"{type.Name.CreatePluralWord()}", $"{fileName}{fileExtension}");
        }
        #endregion View properties

        public static bool IsEntityType(Type type)
        {
            return type.FullName!.Contains($".{StaticLiterals.EntitiesFolder}.");
        }
        public static bool IsModelType(Type type)
        {
            return type.FullName!.Contains($".{StaticLiterals.ModelsFolder}.");
        }
        public static bool IsModelType(string strType)
        {
            return strType.Contains($".{StaticLiterals.ModelsFolder}.");
        }

        public static string CreateModelSubNamespace(Type type)
        {
            var subNamespace = CreateSubNamespaceFromEntityType(type);

            return subNamespace.HasContent() ? $"{StaticLiterals.ModelsFolder}.{subNamespace}" : StaticLiterals.ModelsFolder;
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
            return result.Replace($"{StaticLiterals.EntitiesFolder}.", string.Empty)
                         .Replace($"{StaticLiterals.ServiceModelsFolder}.", string.Empty);
        }
    }
}
//MdEnd