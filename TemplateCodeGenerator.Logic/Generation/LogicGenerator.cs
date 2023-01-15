//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic.Generation
{
    using System.Reflection;
    using TemplateCodeGenerator.Logic.Contracts;
    internal sealed partial class LogicGenerator : ModelGenerator
    {
        private ItemProperties? _itemProperties;
        protected override ItemProperties ItemProperties => _itemProperties ??= new ItemProperties(SolutionProperties.SolutionName, StaticLiterals.LogicExtension);

        public bool GenerateDbContext { get; set; }
        public bool GenerateModels { get; set; }
        public bool GenerateAccessContracts { get; set; }
        public bool GenerateControllers { get; set; }
        public bool GenerateServiceContracts { get; set; }
        public bool GenerateServices { get; set; }
        public bool GenerateFacades { get; set; }
        public bool GenerateFactory { get; set; }

        public LogicGenerator(ISolutionProperties solutionProperties) : base(solutionProperties)
        {
            GenerateDbContext = QuerySetting<bool>(Common.ItemType.DbContext, "All", StaticLiterals.Generate, "True");
            GenerateModels = QuerySetting<bool>(Common.ItemType.Model, "All", StaticLiterals.Generate, "True");
            GenerateAccessContracts = QuerySetting<bool>(Common.ItemType.AccessContract, "All", StaticLiterals.Generate, "True");
            GenerateControllers = QuerySetting<bool>(Common.ItemType.Controller, "All", StaticLiterals.Generate, "True");
            GenerateServiceContracts = QuerySetting<bool>(Common.ItemType.ServiceContract, "All", StaticLiterals.Generate, "True");
            GenerateServices = QuerySetting<bool>(Common.ItemType.Service, "All", StaticLiterals.Generate, "True");
            GenerateFacades = QuerySetting<bool>(Common.ItemType.Facade, "All", StaticLiterals.Generate, "True");
            GenerateFactory = QuerySetting<bool>(Common.ItemType.Factory, "All", StaticLiterals.Generate, "True");
        }

        #region overrides
        public override IEnumerable<string> CreateDelegateAutoProperty(PropertyInfo propertyInfo, string delegateObjectName, PropertyInfo delegatePropertyInfo)
        {
            var result = new List<string>();

            if (IsListType(propertyInfo.PropertyType))
            {
                var entityType = propertyInfo.PropertyType.GenericTypeArguments[0].FullName;
                var modelType = ItemProperties.ConvertEntityToModelType(propertyInfo.PropertyType.GenericTypeArguments[0].FullName!);

                result.Add(string.Empty);
                CreatePropertyAttributes(propertyInfo, result);
                result.Add($"public System.Collections.Generic.IList<{modelType}> {propertyInfo.Name}");
                result.Add("{");
                result.Add($"get => new CommonBase.Modules.Collection.DelegateList<{entityType}, {modelType}>({delegateObjectName}.{delegatePropertyInfo.Name}, e => {modelType}.Create(e));");
                result.Add("}");
            }
            else
            {
                result.AddRange(base.CreateDelegateAutoProperty(propertyInfo, delegateObjectName, delegatePropertyInfo));
            }
            return result;
        }
        public override IEnumerable<string> CreateDelegateAutoGet(PropertyInfo propertyInfo, string delegateObjectName, PropertyInfo delegatePropertyInfo)
        {
            var result = new List<string>();
            var propertyType = GetPropertyType(propertyInfo);
            var delegateProperty = $"{delegateObjectName}.{delegatePropertyInfo.Name}";
            var visibility = propertyInfo.GetGetMethod(true)!.IsPublic ? string.Empty : "internal ";

            if (ItemProperties.IsModelType(propertyType))
            {
                if (IsArrayType(propertyInfo.PropertyType))
                {
                    var modelType = ItemProperties.ConvertEntityToModelType(propertyInfo.PropertyType.GetElementType()!.FullName!);

                    result.Add($"{visibility}get => {delegateProperty}.Select(e => {modelType}.Create(e)).ToArray();");
                }
                else if (IsListType(propertyInfo.PropertyType))
                {
                    var modelType = ItemProperties.ConvertEntityToModelType(propertyInfo.PropertyType.GenericTypeArguments[0].FullName!);

                    result.Add($"{visibility}get => {delegateProperty}.Select(e => {modelType}.Create(e)).ToList();");
                }
                else
                {
                    result.Add($"{visibility}get => {delegateProperty} != null ? {propertyType.Replace("?", string.Empty)}.Create({delegateProperty}) : null;");
                }
            }
            else
            {
                result.Add($"{visibility}get => {delegateObjectName}.{delegatePropertyInfo.Name};");
            }
            return result;
        }
        public override IEnumerable<string> CreateDelegateAutoSet(PropertyInfo propertyInfo, string delegateObjectName, PropertyInfo delegatePropertyInfo)
        {
            var result = new List<string>();
            var propertyType = GetPropertyType(propertyInfo);
            var delegateProperty = $"{delegateObjectName}.{delegatePropertyInfo.Name}";
            var visibility = propertyInfo.GetSetMethod(true)!.IsPublic ? string.Empty : "internal ";

            if (ItemProperties.IsModelType(propertyType))
            {
                if (IsArrayType(propertyInfo.PropertyType))
                {
                    result.Add($"{visibility}set => {delegateProperty} = value.Select(e => e.{delegateObjectName}).ToArray();");
                }
                else if (IsListType(propertyInfo.PropertyType))
                {
                    //                    result.Add($"{visibility}set => {delegateProperty} = value.Select(e => e.{delegateObjectName}).ToList();");
                }
                else
                {
                    result.Add($"{visibility}set => {delegateProperty} = value?.{delegateObjectName};");
                }
            }
            else
            {
                result.Add($"{visibility}set => {delegateObjectName}.{delegatePropertyInfo.Name} = value;");
            }
            return result;
        }
        #endregion overrides

        public IEnumerable<IGeneratedItem> GenerateAll()
        {
            var result = new List<IGeneratedItem>
            {
                CreateDbContext()
            };
            result.AddRange(CreateModels());
            result.AddRange(CreateContracts());
            result.AddRange(CreateControllers());
            result.AddRange(CreateServices());
            result.AddRange(CreateFacades());
            //result.Add(CreateFactory());
            return result;
        }

        private string GetGenerateDefault(Type type)
        {
            return EntityProject.IsNotAGenerationEntity(type) ? "False" : "True";
        }
        private string GetVisiblityDefault(Type type)
        {
            return type.IsPublic ? "public" : "internal";
        }

        private IGeneratedItem CreateDbContext()
        {
            var entityProject = EntityProject.Create(SolutionProperties);
            var dataContextNamespace = $"{ItemProperties.Namespace}.DataContext";
            var result = new Models.GeneratedItem(Common.UnitType.Logic, Common.ItemType.DbContext)
            {
                FullName = $"{dataContextNamespace}.ProjectDbContext",
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = $"DataContext{Path.DirectorySeparatorChar}ProjectDbContextGeneration{StaticLiterals.CSharpFileExtension}",
            };
            result.AddRange(CreateComment());
            result.Add($"partial class ProjectDbContext");
            result.Add("{");

            if (GenerateDbContext)
            {
                foreach (var type in entityProject.EntityTypes.Where(t => EntityProject.IsNotAGenerationEntity(t) == false))
                {

                    if (QuerySetting<bool>(Common.ItemType.DbContext, type, StaticLiterals.Generate, "True"))
                    {
                        var entityType = ItemProperties.CreateSubType(type);

                        result.AddRange(CreateComment(type));
                        result.Add($"public DbSet<{entityType}>? {type.Name}Set" + "{ get; set; }");
                    }
                }
                result.Add(string.Empty);

                result.AddRange(CreateComment());
                result.Add($"partial void GetGeneratorDbSet<E>(ref DbSet<E>? dbSet, ref bool handled) where E : Entities.{StaticLiterals.EntityObjectName}");
                result.Add("{");

                bool first = false;

                foreach (var type in entityProject.EntityTypes.Where(t => EntityProject.IsNotAGenerationEntity(t) == false))
                {
                    if (QuerySetting<bool>(Common.ItemType.DbContext, type, StaticLiterals.Generate, "True"))
                    {
                        var entityType = ItemProperties.CreateSubType(type);

                        result.Add($"{(first ? "else " : string.Empty)}if (typeof(E) == typeof({entityType}))");
                        result.Add("{");
                        result.Add($"dbSet = {type.Name}Set as DbSet<E>;");
                        result.Add("handled = true;");
                        result.Add("}");
                        first = true;
                    }
                }
                result.Add("}");
            }

            result.Add("}");
            result.EnvelopeWithANamespace(dataContextNamespace);
            result.FormatCSharpCode();
            return result;
        }

        private IEnumerable<IGeneratedItem> CreateModels()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);

            if (GenerateModels)
            {
                foreach (var type in entityProject.EntityTypes)
                {
                    if (CanCreate(type) && QuerySetting<bool>(Common.ItemType.Model, type, StaticLiterals.Generate, GetGenerateDefault(type)))
                    {
                        result.Add(CreateDelegateModelFromType(type, Common.UnitType.Logic, Common.ItemType.Model));
                        result.Add(CreateModelInheritance(type, Common.UnitType.Logic, Common.ItemType.Model));
                    }
                }
            }
            return result;
        }
        private IGeneratedItem CreateModelInheritance(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = CreateModelFullNameFromType(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = ItemProperties.CreateModelSubPath(type, "Inheritance", StaticLiterals.CSharpFileExtension),
            };
            result.Source.Add($"partial class {CreateModelName(type)} : {GetBaseClassByType(type, StaticLiterals.ModelsFolder)}");
            result.Source.Add("{");
            result.Source.Add("}");
            result.EnvelopeWithANamespace(ItemProperties.CreateModelNamespace(type));
            result.FormatCSharpCode();
            return result;
        }

        private IEnumerable<IGeneratedItem> CreateContracts()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);

            if (GenerateAccessContracts)
            {
                foreach (var type in entityProject.EntityTypes)
                {
                    if (CanCreate(type)
                        && QuerySetting<bool>(Common.ItemType.Controller, type, StaticLiterals.Generate, GetGenerateDefault(type))
                        && QuerySetting<bool>(Common.ItemType.AccessContract, type, StaticLiterals.Generate, GetGenerateDefault(type)))
                    {
                        result.Add(CreateAccessContract(type, Common.UnitType.Logic, Common.ItemType.AccessContract));
                    }
                }
            }

            if (GenerateServiceContracts)
            {
                foreach (var type in entityProject.ServiceTypes)
                {
                    if (CanCreate(type)
                        && QuerySetting<bool>(Common.ItemType.Service, type, StaticLiterals.Generate, GetGenerateDefault(type))
                        && QuerySetting<bool>(Common.ItemType.ServiceContract, type, StaticLiterals.Generate, GetGenerateDefault(type)))
                    {
                        result.Add(CreateServiceContract(type, Common.UnitType.Logic, Common.ItemType.ServiceContract));
                    }
                }
            }
            return result;
        }
        private IGeneratedItem CreateAccessContract(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var contractName = ItemProperties.CreateAccessContractName(type);
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = ItemProperties.CreateAccessContractType(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = ItemProperties.CreateAccessContractSubPathFromType(type, string.Empty, StaticLiterals.CSharpFileExtension),
            };
            result.AddRange(CreateComment(type));
            result.Add($"public partial interface {contractName}<T> : Contracts.IDataAccess<T>");
            result.Add("{");
            result.Add("}");
            result.EnvelopeWithANamespace(ItemProperties.CreateContractNamespace(type));
            result.FormatCSharpCode();
            return result;
        }
        private IGeneratedItem CreateServiceContract(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var contractName = ItemProperties.CreateServiceContractName(type);
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = ItemProperties.CreateServiceContractType(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = ItemProperties.CreateServiceContractSubPathFromType(type, string.Empty, StaticLiterals.CSharpFileExtension),
            };
            result.AddRange(CreateComment(type));
            result.Add($"public partial interface {contractName}<T> : Contracts.IBaseAccess<T>");
            result.Add("{");
            result.Add("}");
            result.EnvelopeWithANamespace(ItemProperties.CreateContractNamespace(type));
            result.FormatCSharpCode();
            return result;
        }

        private IEnumerable<IGeneratedItem> CreateControllers()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);

            if (GenerateControllers)
            {
                foreach (var type in entityProject.EntityTypes)
                {
                    if (CanCreate(type) && QuerySetting<bool>(Common.ItemType.Controller, type, StaticLiterals.Generate, GetGenerateDefault(type)))
                    {
                        result.Add(CreateControllerFromType(type, Common.UnitType.Logic, Common.ItemType.Controller));
                    }
                }
            }
            return result;
        }
        private IGeneratedItem CreateControllerFromType(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var visibility = QuerySetting<string>(itemType, type, StaticLiterals.Visibility, GetVisiblityDefault(type));
            var attributes = QuerySetting<string>(itemType, type, StaticLiterals.Attributes, string.Empty);
            var controllerGenericType = QuerySetting<string>(itemType, "All", StaticLiterals.ControllerGenericType, "EntitiesController");
            var entityType = ItemProperties.CreateSubType(type);
            var outModelType = ItemProperties.CreateModelSubType(type);
            var controllerName = ItemProperties.CreateControllerClassName(type);
            var contractSubType = ItemProperties.CreateAccessContractSubType(type);
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = $"{ItemProperties.CreateControllerType(type)}",
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = ItemProperties.CreateControllersSubPathFromType(type, string.Empty, StaticLiterals.CSharpFileExtension),
            };

            controllerGenericType = QuerySetting<string>(itemType, type, StaticLiterals.ControllerGenericType, controllerGenericType);
            result.Add($"using TEntity = {entityType};");
            result.Add($"using TOutModel = {outModelType};");
            result.AddRange(CreateComment(type));
            CreateControllerAttributes(type, result.Source);
            result.Add($"{(attributes.HasContent() ? $"[{attributes}]" : string.Empty)}");
            result.Add($"{visibility} sealed partial class {controllerName} : {controllerGenericType}<TEntity, TOutModel>, {contractSubType}<TOutModel>");
            result.Add("{");
            result.AddRange(CreatePartialStaticConstrutor(controllerName));
            result.AddRange(CreatePartialConstrutor("public", controllerName));
            result.AddRange(CreatePartialConstrutor("public", controllerName, "ControllerObject other", "base(other)", null, false));
            result.Add("}");
            result.EnvelopeWithANamespace(ItemProperties.CreateControllerNamespace(type));
            result.FormatCSharpCode();
            return result;
        }
        private IEnumerable<IGeneratedItem> CreateServices()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);

            if (GenerateServices)
            {
                foreach (var type in entityProject.ServiceTypes)
                {
                    if (CanCreate(type) && QuerySetting<bool>(Common.ItemType.Service, type, StaticLiterals.Generate, GetGenerateDefault(type)))
                    {
                        result.Add(CreateServiceFromType(type, Common.UnitType.Logic, Common.ItemType.Service));
                    }
                }
            }
            return result;
        }
        private IGeneratedItem CreateServiceFromType(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var visibility = QuerySetting<string>(itemType, type, StaticLiterals.Visibility, type.IsPublic ? "public" : "internal");
            var attributes = QuerySetting<string>(itemType, type, StaticLiterals.Attributes, string.Empty);
            var serviceGenericType = QuerySetting<string>(itemType, "All", StaticLiterals.ServiceGenericType, $"{StaticLiterals.ServicesFolder}.GenericService");
            var serviceType = ItemProperties.CreateSubType(type);
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
            CreateControllerAttributes(type, result.Source);
            result.Add($"{(attributes.HasContent() ? $"[{attributes}]" : string.Empty)}");
            result.Add($"{visibility} sealed partial class {serviceName} : {serviceGenericType}<{serviceType}>, {contractSubType}<{serviceType}>");
            result.Add("{");
            result.Add("private static string ServiceBaseAddress = \"https://localhost:7085/api\";");
            result.Add($"private static string ServiceRequestUri = \"{type.Name.CreatePluralWord()}\";");
            result.AddRange(CreatePartialStaticConstrutor(serviceName));
            result.AddRange(CreatePartialConstrutor("public", serviceName, null, "base(ServiceBaseAddress, ServiceRequestUri)"));
            result.Add("}");
            result.EnvelopeWithANamespace(ItemProperties.CreateServiceNamespace(type));
            result.FormatCSharpCode();
            return result;
        }

        private IEnumerable<IGeneratedItem> CreateFacades()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);

            if (GenerateFacades)
            {
                foreach (var type in entityProject.EntityTypes)
                {
                    if (CanCreate(type) && QuerySetting<bool>(Common.ItemType.Facade, type, StaticLiterals.Generate, GetGenerateDefault(type)))
                    {
                        result.Add(CreateFacadeFromType(type, Common.UnitType.Logic, Common.ItemType.Facade));
                    }
                }
            }
            return result;
        }
        private IGeneratedItem CreateFacadeFromType(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var facadeGenericType = QuerySetting<string>(itemType, "All", StaticLiterals.FacadeGenericType, "ControllerFacade");
            var outModelType = $"{ItemProperties.CreateModelSubType(type)}";
            var controllerType = ItemProperties.CreateControllerType(type);
            var facadeName = ItemProperties.CreateFactoryFacadeMethodName(type);
            var contractSubType = ItemProperties.CreateFacadeContractSubType(type);
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = ItemProperties.CreateFacadeType(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = ItemProperties.CreateFacadesSubPathFromType(type, string.Empty, StaticLiterals.CSharpFileExtension),
            };

            facadeGenericType = QuerySetting<string>(itemType, type, StaticLiterals.FacadeGenericType, facadeGenericType);
            result.Add($"using TOutModel = {outModelType};");
            result.AddRange(CreateComment(type));
            result.Add($"public sealed partial class {facadeName} : {facadeGenericType}<TOutModel>, {contractSubType}<TOutModel>");
            result.Add("{");
            result.AddRange(CreatePartialStaticConstrutor(facadeName));
            result.AddRange(CreatePartialConstrutor("public", facadeName, null, $"base(new {controllerType}())"));
            result.AddRange(CreatePartialConstrutor("public", facadeName, "FacadeObject facadeObject", $"base(new {controllerType}(facadeObject.ControllerObject))", withPartials: false));

            result.Add("}");
            result.EnvelopeWithANamespace(ItemProperties.CreateFacadeNamespace(type));
            result.FormatCSharpCode();
            return result;
        }

        private IGeneratedItem CreateFactory()
        {
            var entityProject = EntityProject.Create(SolutionProperties);
            var factoryNamespace = $"{ItemProperties.Namespace}";
            var result = new Models.GeneratedItem(Common.UnitType.Logic, Common.ItemType.Factory)
            {
                FullName = $"{factoryNamespace}.Factory",
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = $"Factory{StaticLiterals.CSharpFileExtension}",
            };
            result.AddRange(CreateComment());
            result.Add($"public static partial class Factory");
            result.Add("{");

            if (GenerateFactory)
            {
                if (GenerateControllers)
                {
                    foreach (var type in entityProject.EntityTypes)
                    {
                        if (CanCreate(type)
                            && QuerySetting<bool>(Common.ItemType.Controller, type, StaticLiterals.Generate, GetGenerateDefault(type))
                            && QuerySetting<bool>(Common.ItemType.FactoryControllerMethode, type, StaticLiterals.Generate, GetGenerateDefault(type)))
                        {
                            result.AddRange(CreateFactoryControllerMethod(type));
                        }
                    }
                }

                if (GenerateFacades)
                {
                    foreach (var type in entityProject.EntityTypes)
                    {
                        if (CanCreate(type)
                            && QuerySetting<bool>(Common.ItemType.Facade, type, StaticLiterals.Generate, GetGenerateDefault(type))
                            && QuerySetting<bool>(Common.ItemType.FactoryFacadeMethode, type, StaticLiterals.Generate, GetGenerateDefault(type)))
                        {
                            result.AddRange(CreateFactoryFacadeMethod(type));
                        }
                    }
                }
            }

            result.Add("}");
            result.EnvelopeWithANamespace(factoryNamespace);
            result.FormatCSharpCode();
            return result;
        }
        private IEnumerable<string> CreateFactoryControllerMethod(Type type)
        {
            var result = new List<string>();
            var entityType = $"{type.FullName}";
            var contractSubType = ItemProperties.CreateAccessContractSubType(type);
            var controllerName = ItemProperties.CreateFactoryControllerMethodName(type);
            var controllerType = ItemProperties.CreateControllerType(type);

            result.AddRange(CreateComment(type));
            result.Add($"public static {contractSubType}<{entityType}> Create{controllerName}() => new {controllerType}();");

            result.AddRange(CreateComment(type));
            result.Add($"public static {contractSubType}<{entityType}> Create{controllerName}(Object otherController)");
            result.Add("{");
            result.Add($"var controllerObject = otherController as Controllers.ControllerObject;");
            result.Add($"return new {controllerType}(controllerObject ?? throw new Modules.Exceptions.LogicException(Modules.Exceptions.ErrorType.InvalidControllerObject));");
            result.Add("}");
            return result;
        }
        private IEnumerable<string> CreateFactoryFacadeMethod(Type type)
        {
            var result = new List<string>();
            var modelType = $"{ItemProperties.CreateModelType(type)}";
            var contractSubType = ItemProperties.CreateAccessContractSubType(type);
            var facadeName = ItemProperties.CreateFactoryFacadeMethodName(type);
            var facadeType = ItemProperties.CreateFacadeType(type);

            result.AddRange(CreateComment(type));
            result.Add($"public static {contractSubType}<{modelType}> Create{facadeName}() => new {facadeType}();");

            result.AddRange(CreateComment(type));
            result.Add($"public static {contractSubType}<{modelType}> Create{facadeName}(Object otherFacade)");
            result.Add("{");
            result.Add($"var facadeObject = otherFacade as Facades.FacadeObject;");
            result.Add($"return new {facadeType}(facadeObject ?? throw new Modules.Exceptions.LogicException(Modules.Exceptions.ErrorType.InvalidFacadeObject));");
            result.Add("}");
            return result;
        }

        private T QuerySetting<T>(Common.ItemType itemType, Type type, string valueName, string defaultValue)
        {
            T result;

            try
            {
                result = (T)Convert.ChangeType(QueryGenerationSettingValue(Common.UnitType.Logic, itemType, CreateEntitiesSubTypeFromType(type), valueName, defaultValue), typeof(T));
            }
            catch (Exception ex)
            {
                result = (T)Convert.ChangeType(defaultValue, typeof(T));
                System.Diagnostics.Debug.WriteLine($"Error in {System.Reflection.MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
            }
            return result;
        }
        private T QuerySetting<T>(Common.ItemType itemType, string itemName, string valueName, string defaultValue)
        {
            T result;

            try
            {
                result = (T)Convert.ChangeType(QueryGenerationSettingValue(Common.UnitType.Logic, itemType, itemName, valueName, defaultValue), typeof(T));
            }
            catch (Exception ex)
            {
                result = (T)Convert.ChangeType(defaultValue, typeof(T));
                System.Diagnostics.Debug.WriteLine($"Error in {System.Reflection.MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
            }
            return result;
        }

        #region Partial methods
        partial void CreateControllerAttributes(Type type, List<string> codeLines);
        #endregion Partial methods
    }
}
//MdEnd
