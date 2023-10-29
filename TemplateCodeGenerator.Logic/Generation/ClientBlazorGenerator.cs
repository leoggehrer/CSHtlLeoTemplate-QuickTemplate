//@BaseCode
//MdStart
using System.Reflection;
using TemplateCodeGenerator.Logic.Contracts;

namespace TemplateCodeGenerator.Logic.Generation
{
    /// <summary>
    /// Internal sealed partial class for generating a Blazor client.
    /// </summary>
    /// <remarks>
    /// This class extends the <see cref="ModelGenerator"/> class.
    /// </remarks>
    internal sealed partial class ClientBlazorGenerator : ModelGenerator
    {
        private ItemProperties? _itemProperties;
        /// <summary>
        /// Gets the properties of the item.
        /// </summary>
        /// <value>
        /// The properties of the item.
        /// </value>
        protected override ItemProperties ItemProperties => _itemProperties ??= new ItemProperties(SolutionProperties.SolutionName, StaticLiterals.ClientBlazorExtension);
        
        #region properties
        /// <summary>
        /// Gets or sets a value indicating whether models should be generated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if models should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateModels { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether service models should be generated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if service models should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateServiceModels { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether service access contracts should be generated.
        /// </summary>
        /// <value>
        /// <c>true</c> if service access contracts should be generated; otherwise, <c>false</c>.
        /// </value>
        private bool GenerateServiceAccessContracts { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether Access services should be generated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if Access services should be generated; otherwise, <c>false</c>.
        /// </value>
        private bool GenerateAccessServices { get; set; }
        
        /// Gets or sets a value indicating whether service model contracts are generated.
        private bool GenerateServiceModelContracts { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether service model services should be generated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if service model services should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateServiceModelServices { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to generate add services.
        /// </summary>
        public bool GenerateAddServices { get; set; }
        /// <summary>
        /// Gets or sets the base address.
        /// </summary>
        /// <value>The base address.</value>
        private string BaseAddress { get; set; }
        #endregion properties
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientBlazorGenerator"/> class.
        /// </summary>
        /// <param name="solutionProperties">The solution properties.</param>
        /// <remarks>
        /// This class is used to generate a Blazor client based on the given solution properties.
        /// </remarks>
        public ClientBlazorGenerator(ISolutionProperties solutionProperties) : base(solutionProperties)
        {
            string generateAll = QuerySetting<string>(Common.ItemType.AllItems, StaticLiterals.AllItems, StaticLiterals.Generate, "True");
            
            GenerateModels = QuerySetting<bool>(Common.ItemType.AccessModel, StaticLiterals.AllItems, StaticLiterals.Generate, generateAll);
            GenerateServiceModels = QuerySetting<bool>(Common.ItemType.ServiceModel, StaticLiterals.AllItems, StaticLiterals.Generate, generateAll);
            
            GenerateServiceAccessContracts = QuerySetting<bool>(Common.ItemType.ServiceAccessContract, "All", StaticLiterals.Generate, generateAll);
            GenerateAccessServices = QuerySetting<bool>(Common.ItemType.AccessService, StaticLiterals.AllItems, StaticLiterals.Generate, generateAll);
            
            GenerateServiceModelContracts = QuerySetting<bool>(Common.ItemType.ServiceContract, "All", StaticLiterals.Generate, generateAll);
            GenerateServiceModelServices = QuerySetting<bool>(Common.ItemType.Service, StaticLiterals.AllItems, StaticLiterals.Generate, generateAll);
            
            GenerateAddServices = QuerySetting<bool>(Common.ItemType.AddServices, StaticLiterals.AllItems, StaticLiterals.Generate, generateAll);
            
            BaseAddress = QuerySetting<string>(Common.ItemType.Service, "All", nameof(BaseAddress), "https://localhost:7085/api");
        }
        
        #region overrides
        /// <summary>
        /// Converts the property type by replacing the namespace if it is a model type or creating a subtype if it is a service model type.
        /// </summary>
        /// <param name="typeFullName">The full name of the property type.</param>
        /// <returns>The converted property type.</returns>
        protected override string ConvertPropertyType(string typeFullName)
        {
            var result = base.ConvertPropertyType(typeFullName);
            
            if (ItemProperties.IsModelType(typeFullName))
            {
                result = result.Replace($"{StaticLiterals.ModelsFolder}.", $"{StaticLiterals.ServiceModelsFolder}.");
            }
            else if (ItemProperties.IsServiceModelType(typeFullName))
            {
                result = ItemProperties.CreateSubType(typeFullName);
            }
            return result;
        }
        /// <summary>
        /// Converts the model subtype to a new value.
        /// </summary>
        /// <param name="modelSubType">The original model subtype to be converted.</param>
        /// <returns>The converted model subtype.</returns>
        protected override string ConvertModelSubType(string modelSubType)
        {
            var result = base.ConvertModelSubType(modelSubType);
            
            if (result.StartsWith($"{StaticLiterals.ModelsFolder}."))
            {
                result = result.Replace($"{StaticLiterals.ModelsFolder}.", $"{StaticLiterals.ServiceModelsFolder}.");
            }
            return result;
        }
        /// <summary>
        /// Converts the model namespace by replacing the "Models" folder with the "ServiceModels" folder if it exists.
        /// </summary>
        /// <param name="modelNamespace">The original model namespace.</param>
        /// <returns>The converted model namespace.</returns>
        protected override string ConvertModelNamespace(string modelNamespace)
        {
            var result = base.ConvertModelNamespace(modelNamespace);
            
            if (result.Contains($".{StaticLiterals.ModelsFolder}."))
            {
                result = result.Replace($".{StaticLiterals.ModelsFolder}.", $".{StaticLiterals.ServiceModelsFolder}.");
            }
            return result;
        }
        /// <summary>
        /// Converts the full name of a model by replacing the models folder with the service models folder.
        /// </summary>
        /// <param name="modelFullName">The full name of the model.</param>
        /// <returns>The converted model full name.</returns>
        protected override string ConvertModelFullName(string modelFullName)
        {
            var result = base.ConvertModelFullName(modelFullName);
            
            if (result.Contains($".{StaticLiterals.ModelsFolder}."))
            {
                result = result.Replace($".{StaticLiterals.ModelsFolder}.", $".{StaticLiterals.ServiceModelsFolder}.");
            }
            return result;
        }
        /// <summary>
        /// Converts the provided model sub path by replacing the folder path with a different folder path.
        /// </summary>
        /// <param name="modelSubPath">The model sub path to convert.</param>
        /// <returns>The converted model sub path.</returns>
        /// <remarks>
        /// The method replaces the folder path of the model sub path with a different folder path based on the predefined static literals.
        /// If the model sub path starts with the models folder path, it will be replaced with the service models folder path.
        /// If the model sub path starts with the entities folder path, it will also be replaced with the service models folder path.
        /// The converted model sub path is then returned.
        /// </remarks>
        protected override string ConvertModelSubPath(string modelSubPath)
        {
            var result = base.ConvertModelSubPath(modelSubPath);
            
            if (result.StartsWith($"{StaticLiterals.ModelsFolder}{Path.DirectorySeparatorChar}"))
            {
                result = result.Replace($"{StaticLiterals.ModelsFolder}{Path.DirectorySeparatorChar}", $"{StaticLiterals.ServiceModelsFolder}{Path.DirectorySeparatorChar}");
            }
            else if (result.StartsWith($"{StaticLiterals.EntitiesFolder}{Path.DirectorySeparatorChar}"))
            {
                result = result.Replace($"{StaticLiterals.EntitiesFolder}{Path.DirectorySeparatorChar}", $"{StaticLiterals.ServiceModelsFolder}{Path.DirectorySeparatorChar}");
            }
            return result;
        }
        /// <summary>
        /// Converts the given model base type into a string representation.
        /// </summary>
        /// <param name="modelBaseType">The model base type to convert.</param>
        /// <returns>The converted string representation of the model base type.</returns>
        protected override string ConvertModelBaseType(string modelBaseType)
        {
            var result = base.ConvertModelBaseType(modelBaseType);
            
            if (StaticLiterals.ModelBaseClasses.Contains(result))
            {
                result = StaticLiterals.ServiceModelName;
            }
            return result;
        }
        #endregion overrides
        /// <summary>
        /// Generates all the necessary items.
        /// </summary>
        /// <returns>A collection of generated items.</returns>
        public IEnumerable<IGeneratedItem> GenerateAll()
        {
            var result = new List<IGeneratedItem>();
            
            result.AddRange(CreateModels());
            result.AddRange(CreateContracts());
            result.AddRange(CreateServices());
            result.Add(CreateAddServices());
            return result;
        }
        
        /// <summary>
        /// Determines whether the specified type generates default attributes.
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns><c>true</c> if the type generates default attributes; otherwise, <c>false</c>.</returns>
        private static bool GetGenerateDefault(Type type)
        {
            return !EntityProject.IsNotAGenerationEntity(type);
        }
        /// <summary>
        /// Creates models for the specified entity and service types.
        /// </summary>
        /// <returns>An enumerable collection of generated items.</returns>
        private IEnumerable<IGeneratedItem> CreateModels()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);
            
            foreach (var type in entityProject.EntityTypes)
            {
                var generate = CanCreate(type)
                && QuerySetting<bool>(Common.UnitType.ClientBlazorApp, Common.ItemType.AccessModel, type, StaticLiterals.Generate, GenerateModels.ToString());
                
                if (generate)
                {
                    result.Add(CreateModelFromType(type, Common.UnitType.ClientBlazorApp, Common.ItemType.AccessModel));
                    result.Add(CreateModelInheritance(type, Common.UnitType.ClientBlazorApp, Common.ItemType.AccessModel));
                }
            }
            foreach (var type in entityProject.ServiceTypes)
            {
                var generate = CanCreate(type)
                && QuerySetting<bool>(Common.UnitType.ClientBlazorApp, Common.ItemType.ServiceModel, type, StaticLiterals.Generate, GenerateServiceModels.ToString());
                
                if (generate)
                {
                    result.Add(CreateModelFromType(type, Common.UnitType.ClientBlazorApp, Common.ItemType.ServiceModel));
                    result.Add(CreateModelInheritance(type, Common.UnitType.ClientBlazorApp, Common.ItemType.ServiceModel));
                }
            }
            return result;
        }
        
        /// <summary>
        /// Creates contracts for service types and entity types.
        /// </summary>
        /// <returns>An enumerable collection of generated items.</returns>
        private IEnumerable<IGeneratedItem> CreateContracts()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);
            
            foreach (var type in entityProject.ServiceTypes)
            {
                var defaultValue = (GenerateServiceModelContracts).ToString();
                
                if (CanCreate(type)
                && QuerySetting<bool>(Common.ItemType.ServiceContract, type, StaticLiterals.Generate, defaultValue))
                {
                    result.Add(CreateServiceModelContract(type, Common.UnitType.ClientBlazorApp, Common.ItemType.ServiceContract));
                }
            }
            
            foreach (var type in entityProject.EntityTypes)
            {
                var defaultValue = (GenerateServiceAccessContracts && GetGenerateDefault(type)).ToString();
                
                if (CanCreate(type)
                && QuerySetting<bool>(Common.ItemType.ServiceAccessContract, type, StaticLiterals.Generate, defaultValue))
                {
                    result.Add(CreateServiceAccessContract(type, Common.UnitType.ClientBlazorApp, Common.ItemType.ServiceAccessContract));
                }
            }
            return result;
        }
        /// <summary>
        /// Creates a service model contract based on the given parameters.
        /// </summary>
        /// <param name="type">The type of the service.</param>
        /// <param name="unitType">The unit type of the service.</param>
        /// <param name="itemType">The item type of the service.</param>
        /// <returns>The generated service model contract.</returns>
        private IGeneratedItem CreateServiceModelContract(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var contractName = ItemProperties.CreateServiceContractName(type);
            var modelType = ConvertModelSubType(ItemProperties.CreateModelSubType(type));
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = ItemProperties.CreateFullLogicServiceContractType(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = ItemProperties.CreateServiceContractSubPathFromType(type, string.Empty, StaticLiterals.CSharpFileExtension),
            };
            result.Add($"using TOutModel = {modelType};");
            result.AddRange(CreateComment(type));
            result.Add($"public partial interface {contractName} : BaseContracts.IBaseAccess<TOutModel>");
            result.Add("{");
            result.Add("}");
            result.EnvelopeWithANamespace(ItemProperties.CreateClientBlazorContractNamespace(type));
            result.FormatCSharpCode();
            return result;
        }
        /// <summary>
        /// Creates a service access contract.
        /// </summary>
        /// <param name="type">The type of the contract.</param>
        /// <param name="unitType">The unit type.</param>
        /// <param name="itemType">The item type.</param>
        /// <returns>The generated service access contract as an IGeneratedItem.</returns>
        private IGeneratedItem CreateServiceAccessContract(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var contractName = ItemProperties.CreateServiceContractName(type);
            var outModelType = ConvertModelSubType(ItemProperties.CreateModelSubType(type));
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = ItemProperties.CreateFullLogicAccessContractType(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = ItemProperties.CreateServiceContractSubPathFromType(type, string.Empty, StaticLiterals.CSharpFileExtension),
            };
            result.Add($"using TOutModel = {outModelType};");
            result.AddRange(CreateComment(type));
            result.Add($"public partial interface {contractName} : BaseContracts.IBaseAccess<TOutModel>");
            result.Add("{");
            result.Add("}");
            result.EnvelopeWithANamespace(ItemProperties.CreateClientBlazorContractNamespace(type));
            result.FormatCSharpCode();
            return result;
        }
        
        /// <summary>
        /// Creates a collection of generated items, which can include service models and service access contracts.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> of <see cref="IGeneratedItem"/> containing the generated items.
        /// </returns>
        /// <remarks>
        /// This method creates service models and service access contracts based on the available entity types in the specified entity project.
        /// </remarks>
        private IEnumerable<IGeneratedItem> CreateServices()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);
            
            if (GenerateServiceModelServices)
            {
                foreach (var type in entityProject.ServiceTypes)
                {
                    var defaultValue = (GenerateServiceModelServices).ToString();
                    
                    if (CanCreate(type) && QuerySetting<bool>(Common.ItemType.Service, type, StaticLiterals.Generate, defaultValue))
                    {
                        result.Add(CreateServiceFromType(type, Common.UnitType.ClientBlazorApp, Common.ItemType.Service));
                    }
                }
            }
            
            foreach (var type in entityProject.EntityTypes)
            {
                var defaultValue = (GenerateAccessServices && GetGenerateDefault(type)).ToString();
                
                if (CanCreate(type) && QuerySetting<bool>(Common.ItemType.ServiceAccessContract, type, StaticLiterals.Generate, defaultValue))
                {
                    result.Add(CreateServiceFromType(type, Common.UnitType.ClientBlazorApp, Common.ItemType.ServiceAccessContract));
                }
            }
            return result;
        }
        /// <summary>
        /// Creates a service from a given type.
        /// </summary>
        /// <param name="type">The type of the service.</param>
        /// <param name="unitType">The unit type of the service.</param>
        /// <param name="itemType">The item type of the service.</param>
        /// <returns>The generated service as an IGeneratedItem.</returns>
        private IGeneratedItem CreateServiceFromType(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var visibility = QuerySetting<string>(itemType, type, StaticLiterals.Visibility, type.IsPublic ? "public" : "internal");
            var attributes = QuerySetting<string>(itemType, type, StaticLiterals.Attributes, string.Empty);
            var baseAddress = QuerySetting<string>(itemType, type, nameof(BaseAddress), BaseAddress);
            var requestUri = QuerySetting<string>(itemType, type, "RequestUri", type.Name.CreatePluralWord());
            var serviceGenericType = QuerySetting<string>(itemType, "All", StaticLiterals.ServiceGenericType, "BaseServices.GenericService");
            var serviceType = ConvertModelSubType(ItemProperties.CreateModelSubType(type));
            var serviceName = ItemProperties.CreateServiceClassName(type);
            var contractSubType = ItemProperties.CreateServiceContractSubType(type);
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = $"{ItemProperties.CreateServiceType(type)}",
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = ItemProperties.CreateServicesSubPathFromType(type, string.Empty, StaticLiterals.CSharpFileExtension),
            };
            
            serviceGenericType = QuerySetting<string>(itemType, type, StaticLiterals.ServiceGenericType, serviceGenericType);
            result.AddRange(CreateComment(type));
            result.Add($"{(attributes.HasContent() ? $"[{attributes}]" : string.Empty)}");
            result.Add($"{visibility} sealed partial class {serviceName} : {serviceGenericType}<{serviceType}>, {contractSubType}");
            result.Add("{");
            result.Add("#pragma warning disable IDE0044 // Add readonly modifier");
            result.Add($"private static string ServiceBaseAddress = \"{baseAddress}\";");
            result.Add("#pragma warning disable IDE0044 // Add readonly modifier");
            result.Add($"private static string ServiceRequestUri = \"{requestUri}\";");
            result.AddRange(CreatePartialStaticConstrutor(serviceName));
            result.AddRange(CreatePartialConstrutor("public", serviceName, null, "base(ServiceBaseAddress, ServiceRequestUri)"));
            result.Add("}");
            result.EnvelopeWithANamespace(ItemProperties.CreateServiceNamespace(type));
            result.FormatCSharpCode();
            return result;
        }
        
        /// <summary>
        /// Creates an instance of the <see cref="IGeneratedItem"/> interface for adding services to a Blazor WebAssembly client project.
        /// </summary>
        /// <returns>An instance of <see cref="IGeneratedItem"/>.</returns>
        private IGeneratedItem CreateAddServices()
        {
            var entityProject = EntityProject.Create(SolutionProperties);
            var result = new Models.GeneratedItem(Common.UnitType.ClientBlazorApp, Common.ItemType.AddServices)
            {
                FullName = $"{ItemProperties.ProjectNamespace}.Program",
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = Path.Combine("Client", $"ProgramGeneration{StaticLiterals.CSharpFileExtension}"),
            };
            result.Add("using Microsoft.AspNetCore.Components.WebAssembly.Hosting;");
            result.AddRange(CreateComment());
            result.Add("partial class Program");
            result.Add("{");
            
            result.Add("static partial void AddServices(WebAssemblyHostBuilder builder)");
            result.Add("{");
            
            foreach (var type in entityProject.ServiceTypes)
            {
                var defaultValue = (GenerateAddServices).ToString();
                
                if (CanCreate(type)
                && QuerySetting<bool>(Common.ItemType.AddServices, type, StaticLiterals.Generate, defaultValue))
                {
                    var contractSubType = ItemProperties.CreateServiceContractSubType(type);
                    var serviceSubType = ItemProperties.CreateServiceSubType(type);
                    
                    result.Add($"builder.Services.AddTransient<{contractSubType}, {serviceSubType}>();");
                }
            }
            
            foreach (var type in entityProject.EntityTypes)
            {
                var defaultValue = (GenerateAddServices).ToString();
                
                if (CanCreate(type)
                && QuerySetting<bool>(Common.ItemType.AddServices, type, StaticLiterals.Generate, defaultValue))
                {
                    var contractSubType = ItemProperties.CreateServiceContractSubType(type);
                    var serviceSubType = ItemProperties.CreateServiceSubType(type);
                    
                    result.Add($"builder.Services.AddTransient<{contractSubType}, {serviceSubType}>();");
                }
            }
            result.Add("}");
            
            result.Add("}");
            result.EnvelopeWithANamespace(ItemProperties.ProjectNamespace);
            result.FormatCSharpCode();
            return result;
        }
        
        /// <summary>
        /// Gets the property type for the given PropertyInfo object.
        /// If the property type is an Enum, returns "int".
        /// </summary>
        /// <param name="propertyInfo">The PropertyInfo for the property.</param>
        /// <returns>The property type as a string.</returns>
        protected override string GetPropertyType(PropertyInfo propertyInfo)
        {
            var result = base.GetPropertyType(propertyInfo);
            
            if (propertyInfo.PropertyType.IsEnum)
            {
                result = "int";
            }
            return result;
        }
        
        #region query settings
        /// <summary>
        /// Queries a setting of type T based on the provided parameters.
        /// </summary>
        /// <typeparam name="T">The type of the setting to query.</typeparam>
        /// <param name="itemType">The ItemType associated with the setting.</param>
        /// <param name="type">The Type of the setting.</param>
        /// <param name="valueName">The name of the setting value.</param>
        /// <param name="defaultValue">The default value to use if the setting is not found.</param>
        /// <returns>The queried setting of type T.</returns>
        private T QuerySetting<T>(Common.ItemType itemType, Type type, string valueName, string defaultValue)
        {
            return QuerySetting<T>(Common.UnitType.ClientBlazorApp, itemType, type, valueName, defaultValue);
        }
        /// <summary>
        /// Queries a setting for the specified item type, item name, value name, and default value,
        /// with the unit type set to ClientBlazor.
        /// </summary>
        /// <typeparam name="T">The type of the setting to query.</typeparam>
        /// <param name="itemType">The type of the item to query the setting for.</param>
        /// <param name="itemName">The name of the item to query the setting for.</param>
        /// <param name="valueName">The name of the value to query.</param>
        /// <param name="defaultValue">The default value to use if the setting is not found.</param>
        /// <returns>The queried setting value.</returns>
        private T QuerySetting<T>(Common.ItemType itemType, string itemName, string valueName, string defaultValue)
        {
            return QuerySetting<T>(Common.UnitType.ClientBlazorApp, itemType, itemName, valueName, defaultValue);
        }
        #endregion query settings
    }
}
//MdEnd

