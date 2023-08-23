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
        public static string CreateTSPropertyName(PropertyInfo propertyInfo) => $"{Char.ToLower(propertyInfo.Name[0])}{propertyInfo.Name[1..]}";

        #region Solution properties
        public string[] TemplateProjects
        {
            get
            {
                var result = new List<string>(CommonBase.StaticLiterals.TemplateProjects);

                foreach (var extension in CommonBase.StaticLiterals.TemplateProjectExtensions)
                {
                    result.Add($"{SolutionName}{extension}");
                }
                result.AddRange(CommonBase.StaticLiterals.TemplateToolProjects);
                return result.ToArray();
            }
        }
        #endregion Solution properties

        #region Models properties
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
        #endregion Models properties

        public string CreateModelSubType(Type type)
        {
            return $"{CreateModelSubNamespace(type)}.{type.Name}";
        }
        public string CreateModelNamespace(Type type)
        {
            return $"{Namespace}.{CreateModelSubNamespace(type)}";
        }
        public string CreateModelSubPath(Type type, string postFix, string fileExtension)
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
            return $"{CreateLogicContractNamespace(type)}.{CreateAccessContractName(type)}";
        }
        public string CreateServiceContractType(Type type)
        {
            return $"{CreateLogicContractNamespace(type)}.{CreateServiceContractName(type)}";
        }
        public static string CreateAccessContractName(Type type)
        {
            return $"I{type.Name.CreatePluralWord()}Access";
        }
        public static string CreateServiceContractName(Type type)
        {
            return $"I{type.Name.CreatePluralWord()}Service";
        }
        public string CreateAccessContractSubType(Type type)
        {
            return $"{CreateContractSubNamespace(type)}.{CreateAccessContractName(type)}";
        }
        public string CreateServiceContractSubType(Type type)
        {
            return $"{CreateContractSubNamespace(type)}.{CreateServiceContractName(type)}";
        }
        public string CreateFacadeContractSubType(Type type)
        {
            return $"{CreateContractSubNamespace(type)}.{CreateAccessContractName(type)}";
        }
        public string CreateLogicContractNamespace(Type type)
        {
            return $"{SolutionName}{StaticLiterals.LogicExtension}.{CreateContractSubNamespace(type)}";
        }
        public string CreateClientBlazorContractNamespace(Type type)
        {
            return $"{SolutionName}{StaticLiterals.ClientBlazorExtension}.{CreateContractSubNamespace(type)}";
        }
        public string CreateAccessContractSubPathFromType(Type type, string postFix, string fileExtension)
        {
            return Path.Combine(CreateContractSubNamespace(type).Replace(".", Path.DirectorySeparatorChar.ToString()), $"{CreateAccessContractName(type)}{postFix}{fileExtension}");
        }
        public string CreateServiceContractSubPathFromType(Type type, string postFix, string fileExtension)
        {
            return Path.Combine(CreateContractSubNamespace(type).Replace(".", Path.DirectorySeparatorChar.ToString()), $"{CreateServiceContractName(type)}{postFix}{fileExtension}");
        }
        #endregion Contracts properties

        #region Controller properties
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
        public string CreateControllerSubType(Type type)
        {
            return $"{CreateControllerSubNamespace(type)}.{CreateControllerClassName(type)}";
        }
        public string CreateControllerNamespace(Type type)
        {
            return $"{Namespace}.{CreateControllerSubNamespace(type)}";
        }
        public string CreateControllerSubNamespace(Type type)
        {
            var subNamespace = CreateSolutionTypeSubNamespace(type);

            return subNamespace.Replace($"{StaticLiterals.EntitiesFolder}", StaticLiterals.ControllersFolder)
                               .Replace($"{StaticLiterals.ServiceModelsFolder}", StaticLiterals.ControllersFolder);
        }
        public string CreateControllersSubPathFromType(Type type, string postFix, string fileExtension)
        {
            return Path.Combine(CreateControllerSubNamespace(type).Replace(".", Path.DirectorySeparatorChar.ToString()), $"{CreateControllerClassName(type)}{postFix}{fileExtension}");
        }
        #endregion Controller properties

        #region Service properties
        public string CreateLogicServiceType(Type type)
        {
            return $"{SolutionName}{StaticLiterals.LogicExtension}.{CreateServiceSubType(type)}";
        }
        public string CreateServiceModelSubType(Type type)
        {
            var subNamespace = CreateServiceModelSubNamespace(type);

            return $"{subNamespace}.{type.Name}";
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
        public string CreateServiceSubType(Type type)
        {
            return $"{CreateServiceSubNamespace(type)}.{CreateServiceClassName(type)}";
        }
        public string CreateServiceNamespace(Type type)
        {
            return $"{Namespace}.{CreateServiceSubNamespace(type)}";
        }
        public string CreateServiceSubNamespace(Type type)
        {
            var subNamespace = CreateSolutionTypeSubNamespace(type);

            return subNamespace.Replace($"{StaticLiterals.EntitiesFolder}.", $"{StaticLiterals.ServicesFolder}.")
                               .Replace($"{StaticLiterals.ServiceModelsFolder}.", $"{StaticLiterals.ServicesFolder}.");
        }
        public string CreateServicesSubPathFromType(Type type, string postFix, string fileExtension)
        {
            return Path.Combine(CreateServiceSubNamespace(type).Replace(".", Path.DirectorySeparatorChar.ToString()), $"{CreateServiceClassName(type)}{postFix}{fileExtension}");
        }
        #endregion Service properties

        #region Facade properties
        public string CreateFacadeType(Type type)
        {
            return $"{CreateFacadeNamespace(type)}.{CreateFactoryFacadeMethodName(type)}";
        }
        public string CreateFacadeSubType(Type type)
        {
            return $"{CreateFacadeSubNamespace(type)}.{CreateFactoryFacadeMethodName(type)}";
        }
        public string CreateFacadeNamespace(Type type)
        {
            return $"{Namespace}.{CreateFacadeSubNamespace(type)}";
        }
        public string CreateFacadeSubNamespace(Type type)
        {
            var subNamespace = CreateSolutionTypeSubNamespace(type);

            return subNamespace.Replace($"{StaticLiterals.EntitiesFolder}.", $"{StaticLiterals.FacadesFolder}.")
                               .Replace($"{StaticLiterals.ServiceModelsFolder}.", $"{StaticLiterals.FacadesFolder}.");
        }
        public string CreateFacadesSubPathFromType(Type type, string postFix, string fileExtension)
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

        #region Query types
        public static bool IsEntityType(Type type)
        {
            return type.GetBaseTypes().FirstOrDefault(t => t.Name.Equals(StaticLiterals.EntityObjectName)) != null;
        }
        public static bool IsModelType(Type type)
        {
            return type.FullName!.Contains($".{StaticLiterals.ModelsFolder}.");
        }
        public static bool IsModelType(string strType)
        {
            return strType.Contains($".{StaticLiterals.ModelsFolder}.");
        }
        public static bool IsServiceModelType(Type type)
        {
            return type.GetBaseTypes().FirstOrDefault(t => t.Name.Equals(StaticLiterals.ServiceModelName)) != null;
        }
        public static bool IsServiceModelType(string strType)
        {
            return strType.Contains($".{StaticLiterals.ServiceModelsFolder}.");
        }
        #endregion Query types

        /// <summary>
        /// This method creates the sub namespace from a solution entity type. 
        /// For example: 
        ///     FullName QuickTemplate.Logic.Entities.Base.Artist becomes SubName Contracts.Base.Artist.
        /// </summary>
        /// <param name="type">The Type from which the subnamespace is created.</param>
        /// <returns>The subnamespace as a string.</returns>
        public string CreateContractSubNamespace(Type type)
        {
            var subNamespace = CreateSolutionTypeSubNamespace(type);

            return ReplaceSubNamespaceFolder(subNamespace, StaticLiterals.ContractsFolder);
        }
        /// <summary>
        /// This method creates the sub namespace from a solution entity type. 
        /// For example: 
        ///     FullName QuickTemplate.Logic.Entities.Base.Artist becomes SubName Models.Base.Artist.
        /// </summary>
        /// <param name="type">The Type from which the subnamespace is created.</param>
        /// <returns>The subnamespace as a string.</returns>
        public string CreateModelSubNamespace(Type type)
        {
            var subNamespace = CreateSolutionTypeSubNamespace(type);

            return ReplaceSubNamespaceFolder(subNamespace, StaticLiterals.ModelsFolder, StaticLiterals.EntitiesFolder);
        }
        /// <summary>
        /// This method creates the sub namespace from a solution entity type. 
        /// For example: 
        ///     FullName QuickTemplate.Logic.Entities.Base.Artist becomes SubName ServiceModels.Base.Artist.
        /// </summary>
        /// <param name="type">The Type from which the subnamespace is created.</param>
        /// <returns>The subnamespace as a string.</returns>
        public string CreateServiceModelSubNamespace(Type type)
        {
            var subNamespace = CreateSolutionTypeSubNamespace(type);

            return ReplaceSubNamespaceFolder(subNamespace, StaticLiterals.ServiceModelsFolder);
        }

        /// <summary>
        /// This method creates the part name from a solution type. 
        /// For example: 
        ///     FullName QuickTemplate.Logic.ServiceModel.Base.Artist becomes SubName ServiceModel.Base.Artist.
        /// </summary>
        /// <param name="type">The Type from which the SubName is created.</param>
        /// <returns>The SubName as a string.</returns>
        public string CreateSolutionTypeSubName(Type type)
        {
            return CreateSolutionTypeSubName(type.FullName!);
        }
        /// <summary>
        /// This method creates the part name from a solution type. 
        /// For example: 
        ///     FullName QuickTemplate.Logic.ServiceModel.Base.Artist becomes SubName ServiceModel.Base.Artist.
        /// </summary>
        /// <param name="typeFullName">The Fullname from which the SubName is created.</param>
        /// <returns>The SubName as a string.</returns>
        public string CreateSolutionTypeSubName(string typeFullName)
        {
            var result = typeFullName;

            if (result.StartsWith(SolutionName))
            {
                var data = result.Split('.');

                result = string.Empty;
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
            }
            return result;
        }
        /// <summary>
        /// This method creates the part namespace from a solution type. 
        /// For example: 
        ///     FullName QuickTemplate.Logic.ServiceModel.Base.Artist becomes SubName ServiceModel.Base.
        /// </summary>
        /// <param name="type">The Type from which the SubNamespace is created.</param>
        /// <returns>The SubNamespace as a string.</returns>
        public string CreateSolutionTypeSubNamespace(Type type)
        {
            var result = type.Namespace!;

            if (result.StartsWith(SolutionName))
            {
                var data = result.Split('.');

                result = string.Empty;
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
            }
            return result;
        }

        public static string ReplaceSubNamespaceFolder(string subNamespace, string newValue)
        {
            return ReplaceSubNamespaceFolder(subNamespace, newValue, StaticLiterals.EntitiesFolder, StaticLiterals.ModelsFolder, StaticLiterals.ServiceModelsFolder);
        }
        public static string ReplaceSubNamespaceFolder(string subNamespace, string newValue, params string[] oldValues)
        {
            var result = subNamespace;

            foreach (var oldValue in oldValues)
            {
                if (result.StartsWith($"{oldValue}."))
                {
                    result = result.Replace($"{oldValue}.", $"{newValue}.");
                }
            }
            return result;
        }
    }
}
//MdEnd
