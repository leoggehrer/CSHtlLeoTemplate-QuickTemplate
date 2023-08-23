//@CodeCopy
//MdStart
using System.Reflection;
using TemplateCodeGenerator.Logic.Contracts;

namespace TemplateCodeGenerator.Logic.Generation
{
    internal sealed partial class ClientBlazorGenerator : ModelGenerator
    {
        private ItemProperties? _itemProperties;
        protected override ItemProperties ItemProperties => _itemProperties ??= new ItemProperties(SolutionProperties.SolutionName, StaticLiterals.ClientBlazorExtension);

        #region properties
        public bool GenerateModels { get; set; }
        public bool GenerateServiceModels { get; set; }

        private bool GenerateServiceAccessContracts { get; set; }
        private bool GenerateAccessServices { get; set; }

        private bool GenerateServiceModelContracts { get; set; }
        public bool GenerateServiceModelServices { get; set; }

        public bool GenerateAddServices { get; set; }
        private string BaseAddress { get; set; }
        #endregion properties

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
        protected override string ConvertPropertyType(string type)
        {
            var result = base.ConvertPropertyType(type);

            if (ItemProperties.IsModelType(type))
            {
                result = result.Replace($"{StaticLiterals.ModelsFolder}.", $"{StaticLiterals.ServiceModelsFolder}.");
            }
            else if (ItemProperties.IsServiceModelType(type))
            {
                result = ItemProperties.CreateSolutionTypeSubName(type);
            }
            return result;
        }
        protected override string ConvertModelSubType(string modelSubType)
        {
            var result = base.ConvertModelSubType(modelSubType);

            if (result.StartsWith($"{StaticLiterals.ModelsFolder}."))
            {
                result = result.Replace($"{StaticLiterals.ModelsFolder}.", $"{StaticLiterals.ServiceModelsFolder}.");
            }
            return result;
        }
        protected override string ConvertModelNamespace(string modelNamespace)
        {
            var result = base.ConvertModelNamespace(modelNamespace);

            if (result.Contains($".{StaticLiterals.ModelsFolder}."))
            {
                result = result.Replace($".{StaticLiterals.ModelsFolder}.", $".{StaticLiterals.ServiceModelsFolder}.");
            }
            return result;
        }
        protected override string ConvertModelFullName(string modelFullName)
        {
            var result = base.ConvertModelFullName(modelFullName);

            if (result.Contains($".{StaticLiterals.ModelsFolder}."))
            {
                result = result.Replace($".{StaticLiterals.ModelsFolder}.", $".{StaticLiterals.ServiceModelsFolder}.");
            }
            return result;
        }
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
        public IEnumerable<IGeneratedItem> GenerateAll()
        {
            var result = new List<IGeneratedItem>();

            result.AddRange(CreateModels());
            result.AddRange(CreateContracts());
            result.AddRange(CreateServices());
            result.Add(CreateAddServices());
            return result;
        }

        private static bool GetGenerateDefault(Type type)
        {
            return !EntityProject.IsNotAGenerationEntity(type);
        }
        private IEnumerable<IGeneratedItem> CreateModels()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);

            foreach (var type in entityProject.EntityTypes)
            {
                var generate = CanCreate(type)
                            && QuerySetting<bool>(Common.UnitType.ClientBlazor, Common.ItemType.AccessModel, type, StaticLiterals.Generate, GenerateModels.ToString());

                if (generate)
                {
                    result.Add(CreateModelFromType(type, Common.UnitType.ClientBlazor, Common.ItemType.AccessModel));
                    result.Add(CreateModelInheritance(type, Common.UnitType.ClientBlazor, Common.ItemType.AccessModel));
                }
            }
            foreach (var type in entityProject.ServiceTypes)
            {
                var generate = CanCreate(type)
                            && QuerySetting<bool>(Common.UnitType.ClientBlazor, Common.ItemType.ServiceModel, type, StaticLiterals.Generate, GenerateServiceModels.ToString());

                if (generate)
                {
                    result.Add(CreateModelFromType(type, Common.UnitType.ClientBlazor, Common.ItemType.ServiceModel));
                    result.Add(CreateModelInheritance(type, Common.UnitType.ClientBlazor, Common.ItemType.ServiceModel));
                }
            }
            return result;
        }

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
                    result.Add(CreateServiceModelContract(type, Common.UnitType.ClientBlazor, Common.ItemType.ServiceContract));
                }
            }

            foreach (var type in entityProject.EntityTypes)
            {
                var defaultValue = (GenerateServiceAccessContracts && GetGenerateDefault(type)).ToString();

                if (CanCreate(type)
                    && QuerySetting<bool>(Common.ItemType.ServiceAccessContract, type, StaticLiterals.Generate, defaultValue))
                {
                    result.Add(CreateServiceAccessContract(type, Common.UnitType.ClientBlazor, Common.ItemType.ServiceAccessContract));
                }
            }
            return result;
        }
        private IGeneratedItem CreateServiceModelContract(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var contractName = ItemProperties.CreateServiceContractName(type);
            var outModelType = ConvertModelSubType(ItemProperties.CreateModelSubType(type));
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = ItemProperties.CreateServiceContractType(type),
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
        private IGeneratedItem CreateServiceAccessContract(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var contractName = ItemProperties.CreateServiceContractName(type);
            var outModelType = ConvertModelSubType(ItemProperties.CreateModelSubType(type));
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = ItemProperties.CreateAccessContractType(type),
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

        private IEnumerable<IGeneratedItem> CreateServices()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);

            if (GenerateServiceModelServices)
            {
                foreach (var type in entityProject.ServiceTypes)
                {
                    var defaultValue = (GenerateServiceModelServices).ToString();

                    if (CanCreate(type)
                        && QuerySetting<bool>(Common.ItemType.Service, type, StaticLiterals.Generate, defaultValue))
                    {
                        result.Add(CreateServiceFromType(type, Common.UnitType.ClientBlazor, Common.ItemType.Service));
                    }
                }
            }

            foreach (var type in entityProject.EntityTypes)
            {
                var defaultValue = (GenerateAccessServices && GetGenerateDefault(type)).ToString();

                if (CanCreate(type)
                    && QuerySetting<bool>(Common.ItemType.ServiceAccessContract, type, StaticLiterals.Generate, defaultValue))
                {
                    result.Add(CreateServiceFromType(type, Common.UnitType.ClientBlazor, Common.ItemType.ServiceAccessContract));
                }
            }
            return result;
        }
        private IGeneratedItem CreateServiceFromType(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var visibility = QuerySetting<string>(itemType, type, StaticLiterals.Visibility, type.IsPublic ? "public" : "internal");
            var attributes = QuerySetting<string>(itemType, type, StaticLiterals.Attributes, string.Empty);
            var baseAddress = QuerySetting<string>(itemType, type, nameof(BaseAddress), BaseAddress);
            var requestUri = QuerySetting<string>(itemType, type, "RequestUri", type.Name.CreatePluralWord());
            var serviceGenericType = QuerySetting<string>(itemType, "All", StaticLiterals.ServiceGenericType, "BaseServices.GenericService");
            var serviceType = ItemProperties.CreateServiceModelSubType(type);
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

        private IGeneratedItem CreateAddServices()
        {
            var entityProject = EntityProject.Create(SolutionProperties);
            var result = new Models.GeneratedItem(Common.UnitType.ClientBlazor, Common.ItemType.AddServices)
            {
                FullName = $"{ItemProperties.Namespace}.Program",
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
            result.EnvelopeWithANamespace(ItemProperties.Namespace);
            result.FormatCSharpCode();
            return result;
        }

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
        private T QuerySetting<T>(Common.ItemType itemType, Type type, string valueName, string defaultValue)
        {
            return QuerySetting<T>(Common.UnitType.ClientBlazor, itemType, type, valueName, defaultValue);
        }
        private T QuerySetting<T>(Common.ItemType itemType, string itemName, string valueName, string defaultValue)
        {
            return QuerySetting<T>(Common.UnitType.ClientBlazor, itemType, itemName, valueName, defaultValue);
        }
        #endregion query settings
    }
}
//MdEnd
