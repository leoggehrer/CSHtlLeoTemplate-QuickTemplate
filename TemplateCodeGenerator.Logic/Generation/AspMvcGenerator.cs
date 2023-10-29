//@BaseCode
//MdStart
using System.Reflection;
using System.Text;
using TemplateCodeGenerator.Logic.Contracts;

namespace TemplateCodeGenerator.Logic.Generation
{
    internal sealed partial class AspMvcGenerator : ModelGenerator
    {
        #region fields
        private ItemProperties? _itemProperties;
        #endregion fields
        
        #region properties
        /// <summary>
        /// Gets the ItemProperties object for the current instance.
        /// If _itemProperties is null, a new ItemProperties object is created with the SolutionName from SolutionProperties and the AspMvcExtension from StaticLiterals.
        /// </summary>
        /// <remarks>
        /// The ItemProperties object contains properties related to the items in the solution.
        /// </remarks>
        /// <value>
        /// The ItemProperties object for the current instance.
        /// </value>
        protected override ItemProperties ItemProperties => _itemProperties ??= new ItemProperties(SolutionProperties.SolutionName, StaticLiterals.AspMvcExtension);
        
        /// <summary>
        /// Gets or sets a value indicating whether to generate all access models.
        /// </summary>
        /// <value>
        ///   <c>true</c> if all access models should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateAllAccessModels { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether all service models should be generated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if all service models should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateAllServiceModels { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the GenerateAllFilterModels is enabled or not.
        /// </summary>
        /// <value>True if all filter models should be generated; otherwise, false.</value>
        public bool GenerateAllFilterModels { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to generate all access controllers.
        /// </summary>
        /// <value>
        ///   <c>true</c> if all access controllers should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateAllAccessControllers { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether all service controllers should be generated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if all service controllers should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateAllServiceControllers { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether all services should be generated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if all services should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateAllServices { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to generate add services.
        /// </summary>
        /// <value>
        ///   <c>true</c> if add services should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateAddServices { get; set; }
        ///<summary>
        /// Gets or sets a value indicating whether all views should be generated.
        ///</summary>
        ///<value>
        /// true if all views should be generated; otherwise, false.
        ///</value>
        public bool GenerateAllViews { get; set; }
        #endregion properties
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AspMvcGenerator"/> class.
        /// </summary>
        /// <param name="solutionProperties">The solution properties.</param>
        /// <remarks>
        /// This constructor is used to create an instance of the <see cref="AspMvcGenerator"/> class,
        /// which is responsible for generating different types of code components in an ASP.NET MVC application.
        /// The <paramref name="solutionProperties"/> parameter represents the properties of the solution.
        /// </remarks>
        public AspMvcGenerator(ISolutionProperties solutionProperties) : base(solutionProperties)
        {
            var generateAll = QuerySetting<string>(Common.ItemType.AllItems, StaticLiterals.AllItems, StaticLiterals.Generate, "True");
            
            GenerateAllAccessModels = QuerySetting<bool>(Common.ItemType.AccessModel, StaticLiterals.AllItems, StaticLiterals.Generate, generateAll);
            GenerateAllServiceModels = QuerySetting<bool>(Common.ItemType.ServiceModel, StaticLiterals.AllItems, StaticLiterals.Generate, generateAll);
            
            GenerateAllFilterModels = QuerySetting<bool>(Common.ItemType.AccessFilterModel, StaticLiterals.AllItems, StaticLiterals.Generate, generateAll);
            
            GenerateAllAccessControllers = QuerySetting<bool>(Common.ItemType.AccessController, StaticLiterals.AllItems, StaticLiterals.Generate, generateAll);
            GenerateAllServiceControllers = QuerySetting<bool>(Common.ItemType.ServiceController, StaticLiterals.AllItems, StaticLiterals.Generate, generateAll);
            
            GenerateAllServices = QuerySetting<bool>(Common.ItemType.Service, StaticLiterals.AllItems, StaticLiterals.Generate, generateAll);
            GenerateAddServices = QuerySetting<bool>(Common.ItemType.AddServices, StaticLiterals.AllItems, StaticLiterals.Generate, generateAll);
            
            GenerateAllViews = QuerySetting<bool>(Common.ItemType.View, StaticLiterals.AllItems, StaticLiterals.Generate, generateAll);
        }
        /// <summary>
        /// Determines whether the given property is a nullable primitive type.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns>True if the property is a nullable primitive type; otherwise, false.</returns>
        private static bool IsPrimitiveNullable(PropertyInfo propertyInfo)
        {
            var result = propertyInfo.PropertyType.IsNullableType();
            
            if (result)
            {
                result = propertyInfo.PropertyType.GetGenericArguments()[0].IsPrimitive;
            }
            return result;
        }
        /// <summary>
        /// Creates the filter model name for a given type.
        /// </summary>
        /// <param name="type">The type for which to create the filter model name.</param>
        /// <returns>The filter model name for the specified type.</returns>
        private static string CreateFilterModelName(Type type)
        {
            return $"{ItemProperties.CreateModelName(type)}Filter";
        }
        /// <summary>
        /// Creates the fully-qualified name of the filter model type.
        /// </summary>
        /// <param name="type">The type of the model.</param>
        /// <returns>The fully-qualified name of the filter model type.</returns>
        private string CreateFilterModelType(Type type)
        {
            return $"{ItemProperties.ProjectNamespace}.{ItemProperties.CreateModelSubNamespace(type)}.{CreateFilterModelName(type)}";
        }
        
        /// <summary>
        /// Generates all the required items.
        /// </summary>
        /// <returns>A collection of generated items.</returns>
        public IEnumerable<IGeneratedItem> GenerateAll()
        {
            var result = new List<IGeneratedItem>();
            
            result.AddRange(CreateModels());
            result.AddRange(CreateAccessControllers());
            result.AddRange(CreateServiceControllers());
            result.Add(CreateAddServices());
            result.AddRange(CreateViews());
            return result;
        }
        /// <summary>
        /// This method creates models for entities and services.
        /// </summary>
        /// <returns>Returns an IEnumerable of IGeneratedItem.</returns>
        // Create access models
        // Create service models
        // Create filter models for access models
        // Create filter models for service models
        private IEnumerable<IGeneratedItem> CreateModels()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);
            
            // Create access models
            foreach (var type in entityProject.EntityTypes)
            {
                var settingDefault = GenerateAllAccessModels.ToString();
                
                if (CanCreate(type) && QuerySetting<bool>(Common.UnitType.AspMvc, Common.ItemType.AccessModel, type, StaticLiterals.Generate, settingDefault))
                {
                    result.Add(CreateModelFromType(type, Common.UnitType.AspMvc, Common.ItemType.AccessModel));
                    result.Add(CreateModelInheritance(type, Common.UnitType.AspMvc, Common.ItemType.AccessModel));
                }
            }
            
            // Create service models
            foreach (var type in entityProject.ServiceTypes)
            {
                var settingDefault = GenerateAllServiceModels.ToString();
                
                if (CanCreate(type) && QuerySetting<bool>(Common.UnitType.AspMvc, Common.ItemType.ServiceModel, type, StaticLiterals.Generate, settingDefault))
                {
                    result.Add(CreateModelFromType(type, Common.UnitType.AspMvc, Common.ItemType.ServiceModel));
                    result.Add(CreateModelInheritance(type, Common.UnitType.AspMvc, Common.ItemType.ServiceModel));
                }
            }
            
            // Create filter models for access models
            foreach (var type in entityProject.EntityTypes)
            {
                var settingDefault = GenerateAllFilterModels.ToString();
                
                if (CanCreate(type) && QuerySetting<bool>(Common.UnitType.AspMvc, Common.ItemType.AccessFilterModel, type, StaticLiterals.Generate, settingDefault))
                {
                    result.Add(CreateFilterModelFromType(type, Common.UnitType.AspMvc, Common.ItemType.AccessFilterModel));
                }
            }
            
            // Create filter models for service models
            foreach (var type in entityProject.ServiceTypes)
            {
                var settingDefault = GenerateAllFilterModels.ToString();
                
                if (CanCreate(type) && QuerySetting<bool>(Common.UnitType.AspMvc, Common.ItemType.ServiceFilterModel, type, StaticLiterals.Generate, settingDefault))
                {
                    result.Add(CreateFilterModelFromType(type, Common.UnitType.AspMvc, Common.ItemType.ServiceFilterModel));
                }
            }
            return result;
        }
        
        /// <summary>
        /// Creates a filter model from the specified <see cref="Type"/>, <see cref="Common.UnitType"/>, and <see cref="Common.ItemType"/>.
        /// </summary>
        /// <param name="type">The type of the model.</param>
        /// <param name="unitType">The unit type of the model.</param>
        /// <param name="itemType">The item type of the model.</param>
        /// <returns>The generated filter model.</returns>
        private IGeneratedItem CreateFilterModelFromType(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var sbToString = new StringBuilder();
            var sbHasEntityValue = new StringBuilder();
            var modelName = CreateFilterModelName(type);
            var filterContract = "Models.View.IFilterModel";
            var viewProperties = GetViewProperties(type);
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = CreateFilterModelType(type),
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = ItemProperties.CreateModelSubPath(type, "Filter", StaticLiterals.CSharpFileExtension),
            };
            
            int idx = 0;
            result.AddRange(CreateComment(type));
            CreateModelAttributes(type, result.Source);
            result.Add($"public partial class {modelName} : {filterContract}");
            result.Add("{");
            result.AddRange(CreatePartialStaticConstrutor(modelName));
            result.AddRange(CreatePartialConstrutor("public", modelName));
            
            foreach (var modelItem in viewProperties)
            {
                var canGenerate = QuerySetting<bool>(unitType, Common.ItemType.FilterProperty, $"{ItemProperties.CreateSubTypeFromEntity(type)}Filter.{modelItem.Name}", StaticLiterals.Generate, "True");
                
                if (canGenerate)
                {
                    if (idx++ > 0)
                    {
                        sbHasEntityValue.Append(" || ");
                    }
                    
                    if (modelItem.PropertyType == typeof(string))
                    {
                        sbToString.AppendLine($"if (string.IsNullOrEmpty({modelItem.Name}) == false)");
                        sbToString.AppendLine("{");
                        sbToString.AppendLine("sb.Append($\"" + $"{modelItem.Name}: " + "{" + $"{modelItem.Name}" + "} \");");
                        sbToString.AppendLine("}");
                    }
                    else
                    {
                        sbToString.AppendLine($"if ({modelItem.Name} != null)");
                        sbToString.AppendLine("{");
                        sbToString.AppendLine("sb.Append($\"" + $"{modelItem.Name}: " + "{" + $"{modelItem.Name}" + "} \");");
                        sbToString.AppendLine("}");
                    }
                    sbHasEntityValue.Append($"{modelItem.Name} != null");
                    result.AddRange(CreateFilterAutoProperty(modelItem));
                }
            }
            
            if (sbHasEntityValue.Length > 0)
            {
                result.AddRange(CreateComment(type));
                result.Add($"public bool HasEntityValue => {sbHasEntityValue};");
            }
            
            result.Add("private bool show = true;");
            result.AddRange(CreateComment(type));
            result.Add("public bool Show => show;");
            
            result.AddRange(CreateComment(type));
            result.Add("public string CreateEntityPredicate()");
            result.Add("{");
            result.Add("var result = new System.Text.StringBuilder();");
            result.Add(string.Empty);
            foreach (var item in viewProperties)
            {
                var canGenerate = QuerySetting<bool>(unitType, Common.ItemType.Property, $"{ItemProperties.CreateSubTypeFromEntity(type)}Filter.{item.Name}", StaticLiterals.Generate, "True");
                
                if (canGenerate)
                {
                    result.Add($"if ({item.Name} != null)");
                    result.Add("{");
                    
                    result.Add("if (result.Length > 0)");
                    result.Add("{");
                    result.Add("result.Append(\" || \");");
                    result.Add("}");
                    
                    if (item.PropertyType.IsEnum)
                    {
                        result.Add($"var ev = Convert.ChangeType({item.Name}, typeof(int));");
                        
                        result.Add("result.Append($\"(" + $"{item.Name} != null && {item.Name} ==" + "{ev})\");");
                    }
                    else if (item.PropertyType == typeof(string))
                    {
                        result.Add("result.Append($\"(" + $"{item.Name} != null && {item.Name}.Contains(\\\"" + "{" + $"{item.Name}" + "}" + "\\\"))\");");
                    }
                    else
                    {
                        result.Add("result.Append($\"(" + $"{item.Name} != null && {item.Name} == " + "{" + $"{item.Name}" + "})\");");
                    }
                    result.Add("}");
                }
            }
            result.Add("return result.ToString();");
            result.Add("}");
            
            if (sbToString.Length > 0)
            {
                result.AddRange(CreateComment(type));
                result.Add("public override string ToString()");
                result.Add("{");
                result.Add("System.Text.StringBuilder sb = new();");
                result.Add(sbToString.ToString());
                result.Add("return sb.ToString();");
                result.Add("}");
            }
            
            result.Add("}");
            result.EnvelopeWithANamespace(ItemProperties.CreateModelNamespace(type), "using System;");
            result.FormatCSharpCode();
            return result;
        }
        
        /// <summary>
        /// Creates access controllers for the given entity types.
        /// </summary>
        /// <returns>An IEnumerable of IGeneratedItem representing the access controllers.</returns>
        private IEnumerable<IGeneratedItem> CreateAccessControllers()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);
            var settingDefault = GenerateAllAccessControllers.ToString();
            
            foreach (var type in entityProject.EntityTypes)
            {
                if (CanCreate(type)
                && QuerySetting<bool>(Common.ItemType.AccessController, type, StaticLiterals.Generate, settingDefault))
                {
                    result.Add(CreateAccessControllerFromType(type, Common.UnitType.AspMvc, Common.ItemType.Controller));
                }
            }
            return result;
        }
        /// <summary>
        /// Creates an access controller from a specified type, unit type, and item type.
        /// </summary>
        /// <param name="type">The type used to create the access controller.</param>
        /// <param name="unitType">The unit type of the access controller.</param>
        /// <param name="itemType">The item type of the access controller.</param>
        /// <returns>The generated access controller.</returns>
        private IGeneratedItem CreateAccessControllerFromType(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var visibility = "public";
            var accessType = ItemProperties.CreateFullLogicModelType(type);
            var genericType = $"Controllers.FilterGenericController";
            var modelType = ItemProperties.CreateModelSubType(type);
            var controllerName = ItemProperties.CreateControllerName(type);
            var controllerClassName = ItemProperties.CreateControllerClassName(type);
            var contractType = ItemProperties.CreateFullLogicAccessContractType(type);
            var accessAlias = "TAccessModel";
            var accessTypeUsing = $"using {accessAlias} = {accessType};";
            var modelAlias = "TViewModel";
            var modelTypeUsing = $"using {modelAlias} = {modelType};";
            var filterAlias = "TFilterModel";
            var filterTypeUsing = $"using {filterAlias} = {CreateFilterModelType(type)};";
            var contractAlias = "TAccessContract";
            var contractTypeUsing = $"using {contractAlias} = {contractType};";
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = $"{ItemProperties.CreateControllerType(type)}",
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = ItemProperties.CreateControllersSubPathFromType(type, string.Empty, StaticLiterals.CSharpFileExtension),
            };
            result.AddRange(CreateComment(type));
            CreateControllerAttributes(type, result.Source);
            result.Add($"{visibility} sealed partial class {controllerClassName} : {genericType}<{accessAlias}, {modelAlias}, {filterAlias}, {contractAlias}>");
            result.Add("{");
            result.AddRange(CreatePartialStaticConstrutor(controllerClassName));
            result.Add($"protected override string ControllerName => \"{controllerName}\";");
            result.AddRange(CreatePartialConstrutor("public", controllerClassName, $"{contractType} other", "base(other)", null, true));
            result.AddRange(CreateComment(type));
            result.Add($"protected override {modelAlias} ToViewModel({accessAlias} accessModel, ActionMode actionMode)");
            result.Add("{");
            result.Add($"var handled = false;");
            result.Add($"var result = default({modelAlias});");
            result.Add("BeforeToViewModel(accessModel, actionMode, ref result, ref handled);");
            result.Add("if (handled == false || result == null)");
            result.Add("{");
            result.Add($"result = {modelAlias}.Create(accessModel);");
            result.Add("}");
            result.Add("AfterToViewModel(result, actionMode);");
            result.Add("return BeforeView(result, actionMode);");
            result.Add("}");
            
            result.Add($"partial void BeforeToViewModel({accessAlias} accessModel, ActionMode actionMode, ref {modelAlias}? viewModel, ref bool handled);");
            result.Add($"partial void AfterToViewModel({modelAlias} viewModel, ActionMode actionMode);");
            result.Add("}");
            result.EnvelopeWithANamespace(ItemProperties.CreateControllerNamespace(type), "using Microsoft.AspNetCore.Mvc;", accessTypeUsing, modelTypeUsing, filterTypeUsing, contractTypeUsing);
            result.FormatCSharpCode();
            return result;
        }
        /// <summary>
        /// Creates service controllers based on the types found in the entity project.
        /// </summary>
        /// <returns>
        /// A collection of objects implementing the <see cref="IGeneratedItem"/> interface, representing the generated service controllers.
        /// </returns>
        private IEnumerable<IGeneratedItem> CreateServiceControllers()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);
            
            foreach (var type in entityProject.ServiceTypes)
            {
                if (CanCreate(type)
                && QuerySetting<bool>(Common.ItemType.ServiceController, type, StaticLiterals.Generate, GenerateAllServices.ToString()))
                {
                    result.Add(CreateServiceControllerFromType(type, Common.UnitType.AspMvc, Common.ItemType.Controller));
                }
            }
            return result;
        }
        /// <summary>
        /// Creates a service controller from a specified type, unit type, and item type.
        /// </summary>
        /// <param name="type">The type to create the service controller from.</param>
        /// <param name="unitType">The unit type of the generated item.</param>
        /// <param name="itemType">The item type of the generated item.</param>
        /// <returns>The generated service controller.</returns>
        private IGeneratedItem CreateServiceControllerFromType(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var visibility = "public";
            var logicProject = $"{ItemProperties.SolutionName}{StaticLiterals.LogicExtension}";
            var accessType = $"{logicProject}.{ItemProperties.CreateSubType(type)}";
            var genericType = $"Controllers.FilterGenericController";
            var modelType = ItemProperties.CreateModelType(type);
            var controllerName = ItemProperties.CreateServiceName(type);
            var controllerClassName = ItemProperties.CreateControllerClassName(type);
            var contractType = ItemProperties.CreateFullLogicServiceContractType(type);
            var accessAlias = "AccessType";
            var accessTypeUsing = $"using {accessAlias} = {accessType};";
            var modelAlias = "ModelType";
            var modelTypeUsing = $"using {modelAlias} = {modelType};";
            var filterAlias = "FilterType";
            var filterTypeUsing = $"using {filterAlias} = {CreateFilterModelType(type)};";
            var contractAlias = "TAccessContract";
            var contractTypeUsing = $"using {contractAlias} = {contractType};";
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = $"{ItemProperties.CreateControllerType(type)}",
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = ItemProperties.CreateControllersSubPathFromType(type, string.Empty, StaticLiterals.CSharpFileExtension),
            };
            result.AddRange(CreateComment(type));
            CreateControllerAttributes(type, result.Source);
            result.Add($"{visibility} sealed partial class {controllerClassName} : {genericType}<{accessAlias}, {modelAlias}, {filterAlias}, {contractAlias}>");
            result.Add("{");
            result.AddRange(CreatePartialStaticConstrutor(controllerClassName));
            result.Add($"protected override string ControllerName => \"{controllerName}\";");
            result.AddRange(CreatePartialConstrutor("public", controllerClassName, $"{contractType} other", "base(other)", null, true));
            result.AddRange(CreateComment(type));
            result.Add($"protected override {modelAlias} ToViewModel({accessAlias} accessModel, ActionMode actionMode)");
            result.Add("{");
            result.Add($"var handled = false;");
            result.Add($"var result = default({modelAlias});");
            result.Add("BeforeToViewModel(accessModel, actionMode, ref result, ref handled);");
            result.Add("if (handled == false || result == null)");
            result.Add("{");
            result.Add($"result = {modelAlias}.Create(accessModel);");
            result.Add("}");
            result.Add("AfterToViewModel(result, actionMode);");
            result.Add("return BeforeView(result, actionMode);");
            result.Add("}");
            
            result.Add($"partial void BeforeToViewModel({accessAlias} accessModel, ActionMode actionMode, ref {modelAlias}? viewModel, ref bool handled);");
            result.Add($"partial void AfterToViewModel({modelAlias} viewModel, ActionMode actionMode);");
            result.Add("}");
            result.EnvelopeWithANamespace(ItemProperties.CreateControllerNamespace(type), "using Microsoft.AspNetCore.Mvc;", accessTypeUsing, modelTypeUsing, filterTypeUsing, contractTypeUsing);
            result.FormatCSharpCode();
            return result;
        }
        
        /// <summary>
        /// Creates an instance of an <see cref="IGeneratedItem"/> representing the method "CreateAddServices".
        /// </summary>
        /// <returns>An <see cref="IGeneratedItem"/> object representing the method "CreateAddServices".</returns>
        private IGeneratedItem CreateAddServices()
        {
            var entityProject = EntityProject.Create(SolutionProperties);
            var result = new Models.GeneratedItem(Common.UnitType.AspMvc, Common.ItemType.AddServices)
            {
                FullName = $"{ItemProperties.ProjectNamespace}.Program",
                FileExtension = StaticLiterals.CSharpFileExtension,
                SubFilePath = $"ProgramGeneration{StaticLiterals.CSharpFileExtension}",
            };
            result.AddRange(CreateComment());
            result.Add("partial class Program");
            result.Add("{");
            
            result.Add("static partial void AddServices(WebApplicationBuilder builder)");
            result.Add("{");
            foreach (var type in entityProject.EntityTypes.Where(t => EntityProject.IsNotAGenerationEntity(t) == false))
            {
                var generate = CanCreate(type) && QuerySetting<bool>(Common.ItemType.AddServices, type, StaticLiterals.Generate, GenerateAddServices.ToString());
                
                if (generate && type.IsPublic)
                {
                    var contractType = ItemProperties.CreateFullLogicAccessContractType(type);
                    var controllerType = ItemProperties.CreateFullLogicControllerType(type);
                    
                    result.Add($"builder.Services.AddTransient<{contractType}, {controllerType}>();");
                }
                else if (generate)
                {
                    var contractType = ItemProperties.CreateFullLogicAccessContractType(type);
                    var facadeType = ItemProperties.CreateFullLogicFacadeType(type);
                    
                    result.Add($"builder.Services.AddTransient<{contractType}, {facadeType}>();");
                }
            }
            foreach (var type in entityProject.ServiceTypes)
            {
                var generate = CanCreate(type) && QuerySetting<bool>(Common.ItemType.AddServices, type, StaticLiterals.Generate, GenerateAddServices.ToString());
                
                if (generate)
                {
                    var contractType = ItemProperties.CreateFullLogicServiceContractType(type);
                    var serviceType = ItemProperties.CreateFullLogicServiceType(type);
                    
                    result.Add($"builder.Services.AddTransient<{contractType}, {serviceType}>();");
                }
            }
            result.Add("}");
            
            result.Add("}");
            result.EnvelopeWithANamespace(ItemProperties.ProjectNamespace);
            result.FormatCSharpCode();
            return result;
        }
        
        /// <summary>
        /// Creates views for each entity and service type in the project.
        /// </summary>
        /// <returns>An IEnumerable of IGeneratedItem representing the views created.</returns>
        private IEnumerable<IGeneratedItem> CreateViews()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);
            var createTypes = entityProject.EntityTypes.Union(entityProject.ServiceTypes);
            
            foreach (var type in createTypes)
            {
                if (CanCreate(type)
                && QuerySetting<bool>(Common.ItemType.View, type, StaticLiterals.Generate, GenerateAllViews.ToString()))
                {
                    result.Add(CreatePartialTableHeaderView(type, Common.UnitType.AspMvc, Common.ItemType.View));
                    result.Add(CreatePartialTableRowView(type, Common.UnitType.AspMvc, Common.ItemType.View));
                    result.Add(CreatePartialEditModelView(type, Common.UnitType.AspMvc, Common.ItemType.View));
                    result.Add(CreatePartialDisplayModelView(type, Common.UnitType.AspMvc, Common.ItemType.View));
                    result.Add(CreatePartialFilterView(type, Common.UnitType.AspMvc, Common.ItemType.View));
                }
            }
            return result;
        }
        /// <summary>
        /// Retrieves the view properties of the specified type.
        /// </summary>
        /// <param name="type">The type to get the view properties from.</param>
        /// <returns>An enumerable collection of PropertyInfo objects representing the view properties of the specified type.</returns>
        private static IEnumerable<PropertyInfo> GetViewProperties(Type type)
        {
            var typeProperties = type.GetAllPropertyInfos();
            var viewProperties = typeProperties.Where(e => StaticLiterals.VersionProperties.Any(p => p.Equals(e.Name)) == false
            && StaticLiterals.ExtendedProperties.Any(p => p.Equals(e.Name)) == false
            && ItemProperties.IsListType(e.PropertyType) == false
            && (e.PropertyType.IsEnum || e.PropertyType.IsValueType || e.PropertyType.IsPrimitive || IsPrimitiveNullable(e) || e.PropertyType == typeof(string)));
            
            return viewProperties;
        }
        /// <summary>
        /// Creates a partial table header view for a given type, unitType, and itemType.
        /// </summary>
        /// <param name="type">The type of the view.</param>
        /// <param name="unitType">The unit type of the view.</param>
        /// <param name="itemType">The item type of the view.</param>
        /// <returns>The generated item representing the partial table header view.</returns>
        private IGeneratedItem CreatePartialTableHeaderView(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var viewProperties = GetViewProperties(type);
            var modelType = ItemProperties.CreateModelType(type);
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = ItemProperties.CreateModelType(type),
                FileExtension = StaticLiterals.CSharpHtmlFileExtension,
                SubFilePath = ItemProperties.CreateViewSubPathFromType(type, "_TableHeader", StaticLiterals.CSharpHtmlFileExtension),
            };
            result.Add($"@model IEnumerable<{modelType}>");
            
            result.Add("@{");
            result.Add("  var orderBy = (string)ViewBag.OrderBy ?? string.Empty;");
            result.Add("}");
            
            result.Add("<thead>");
            result.Add(" <tr>");
            
            foreach (var viewItem in viewProperties)
            {
                if (CanCreate(viewItem) && QuerySetting<bool>(Common.ItemType.ViewTableProperty, viewItem.DeclaringName(), StaticLiterals.Generate, "True"))
                {
                    result.Add("  <th>");
                    result.Add($"   @if (orderBy.StartsWith(\"{viewItem.Name}\") && orderBy.EndsWith(\"ASC\"))");
                    result.Add("    {");
                    result.Add($"     <a asp-action=\"OrderBy\" asp-route-orderBy=\"{viewItem.Name} DESC\" class=\"btn btn-outline\"><strong>{viewItem.Name}</strong> <i class=\"fa fa-sort-desc\"></i></a>");
                    result.Add("    }");
                    result.Add($"   else if (orderBy.StartsWith(\"{viewItem.Name}\") && orderBy.EndsWith(\"DESC\"))");
                    result.Add("    {");
                    result.Add($"     <a asp-action=\"OrderBy\" asp-route-orderBy=\"\" class=\"btn btn-outline\"><strong>{viewItem.Name}</strong> <i class=\"fa fa-sort-asc\"></i></a>");
                    result.Add("    }");
                    result.Add($"   else");
                    result.Add("    {");
                    result.Add($"     <a asp-action=\"OrderBy\" asp-route-orderBy=\"{viewItem.Name} ASC\" class=\"btn btn-outline\"><strong>{viewItem.Name}</strong></a>");
                    result.Add("    }");
                    result.Add("  </th>");
                }
            }
            
            result.Add("  <th></th>");
            result.Add(" </tr>");
            result.Add("</thead>");
            return result;
        }
        /// <summary>
        /// Creates a partial table row view for the specified type, unit type, and item type.
        /// </summary>
        /// <param name="type">The type of the view.</param>
        /// <param name="unitType">The unit type of the generated item.</param>
        /// <param name="itemType">The item type of the generated item.</param>
        /// <returns>The generated partial table row view.</returns>
        private IGeneratedItem CreatePartialTableRowView(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var viewProperties = GetViewProperties(type);
            var modelType = ItemProperties.CreateModelType(type);
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = ItemProperties.CreateModelType(type),
                FileExtension = StaticLiterals.CSharpHtmlFileExtension,
                SubFilePath = ItemProperties.CreateViewSubPathFromType(type, "_TableRow", StaticLiterals.CSharpHtmlFileExtension),
            };
            result.Add($"@model {modelType}");
            result.Add(string.Empty);
            result.Add("<tr>");
            
            foreach (var viewItem in viewProperties)
            {
                if (CanCreate(viewItem) && QuerySetting<bool>(Common.ItemType.ViewTableProperty, viewItem.DeclaringName(), StaticLiterals.Generate, "True"))
                {
                    result.Add(" <td>");
                    result.Add($"  @Html.DisplayFor(model => model.{viewItem.Name})");
                    result.Add(" </td>");
                }
            }
            
            result.Add(" <td>");
            result.Add("  @Html.ActionLink(\"Edit\", \"Edit\", new { id=Model.Id }) |");
            result.Add("  @Html.ActionLink(\"Details\", \"Details\", new { id=Model.Id }) |");
            result.Add("  @Html.ActionLink(\"Delete\", \"Delete\", new { id=Model.Id })");
            result.Add(" </td>");
            result.Add("</tr>");
            return result;
        }
        /// <summary>
        /// Creates a partial edit model view for a given type.
        /// </summary>
        /// <param name="type">The type of the model.</param>
        /// <param name="unitType">The unit type.</param>
        /// <param name="itemType">The item type.</param>
        /// <returns>The generated partial edit model view.</returns>
        private IGeneratedItem CreatePartialEditModelView(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var viewProperties = GetViewProperties(type);
            var modelType = ItemProperties.CreateModelType(type);
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = ItemProperties.CreateModelType(type),
                FileExtension = StaticLiterals.CSharpHtmlFileExtension,
                SubFilePath = ItemProperties.CreateViewSubPathFromType(type, "_EditModel", StaticLiterals.CSharpHtmlFileExtension),
            };
            result.Add($"@model {modelType}");
            result.Add(string.Empty);
            
            result.Add("<div class=\"row\">");
            result.Add(" <div class=\"col-md-4\">");
            result.Add("  <div asp-validation-summary=\"ModelOnly\" class=\"text-danger\"></div>");
            result.Add("  <input asp-for=\"Id\" type=\"hidden\" />");
            
            foreach (var viewItem in viewProperties)
            {
                if (CanCreate(viewItem))
                {
                    var attribute = StaticLiterals.ExtendedProperties.Any(e => e.Equals(viewItem.Name)) ? "readonly=\"readonly\"" : string.Empty;
                    
                    result.Add("  <div class=\"form-group\">");
                    result.Add($"   <label asp-for=\"{viewItem.Name}\" class=\"control-label\"></label>");
                    if (viewItem.PropertyType.IsEnum)
                    {
                        result.Add("   @{");
                        result.Add($"    var values{viewItem.Name} = Enum.GetValues(typeof({viewItem.PropertyType})).Cast<{viewItem.PropertyType}>().Select(e => new SelectListItem(e.ToString(), e.ToString()));");
                        result.Add("   }");
                        result.Add($"   @Html.DropDownListFor(m => m.{viewItem.Name}, values{viewItem.Name}, null" + ", new { @class = \"form-select\" })");
                    }
                    else if (viewItem.PropertyType == typeof(bool) || viewItem.PropertyType == typeof(bool?))
                    {
                        result.Add($"   <input asp-for=\"{viewItem.Name}\" class=\"form-check\" {attribute} />");
                    }
                    else
                    {
                        result.Add($"   <input asp-for=\"{viewItem.Name}\" class=\"form-control\" {attribute} />");
                    }
                    result.Add($"   <span asp-validation-for=\"{viewItem.Name}\" class=\"text-danger\"></span>");
                    result.Add("  </div>");
                }
            }
            
            result.Add(" </div>");
            result.Add("</div>");
            return result;
        }
        /// <summary>
        /// Creates a partial display model view.
        /// </summary>
        /// <param name="type">The type of the model.</param>
        /// <param name="unitType">The unit type of the generated item.</param>
        /// <param name="itemType">The item type of the generated item.</param>
        /// <returns>The generated item representing the partial display model view.</returns>
        // Method code here...
        private IGeneratedItem CreatePartialDisplayModelView(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var viewProperties = GetViewProperties(type);
            var modelType = ItemProperties.CreateModelType(type);
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = ItemProperties.CreateModelType(type),
                FileExtension = StaticLiterals.CSharpHtmlFileExtension,
                SubFilePath = ItemProperties.CreateViewSubPathFromType(type, "_DisplayModel", StaticLiterals.CSharpHtmlFileExtension),
            };
            result.Add($"@model {modelType}");
            result.Add(string.Empty);
            
            result.Add("<dl class=\"row\">");
            
            foreach (var viewItem in viewProperties)
            {
                if (CanCreate(viewItem))
                {
                    result.Add(" <dt class=\"col-sm-2\">");
                    result.Add($"  @Html.DisplayNameFor(model => model.{viewItem.Name})");
                    result.Add(" </dt>");
                    result.Add(" <dd class=\"col-sm-10\">");
                    result.Add($"  @Html.DisplayFor(model => model.{viewItem.Name})");
                    result.Add(" </dd>");
                }
            }
            
            result.Add("</dl>");
            return result;
        }
        
        /// <summary>
        /// Creates a partial filter view for a given type, unit type, and item type.
        /// </summary>
        /// <param name="type">The type to create the filter view for.</param>
        /// <param name="unitType">The unit type of the filter view.</param>
        /// <param name="itemType">The item type of the filter view.</param>
        /// <returns>An <see cref="IGeneratedItem"/> representing the created filter view.</returns>
        private IGeneratedItem CreatePartialFilterView(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var viewProperties = GetViewProperties(type);
            var modelType = ItemProperties.CreateFilterModelType(type);
            var result = new Models.GeneratedItem(unitType, itemType)
            {
                FullName = ItemProperties.CreateModelType(type),
                FileExtension = StaticLiterals.CSharpHtmlFileExtension,
                SubFilePath = ItemProperties.CreateViewSubPathFromType(type, "_Filter", StaticLiterals.CSharpHtmlFileExtension),
            };
            result.Add($"@model {modelType}");
            result.Add(string.Empty);
            
            result.Add("@{");
            result.Add("  var boolSelect = new SelectList(new[] { new { Id = \"\", Value = \"---\" }, new { Id = \"True\", Value = \"True\" }, new { Id = \"False\", Value = \"False\" } }, \"Id\", \"Value\");");
            result.Add("}");
            result.Add(string.Empty);
            result.Add("<div class=\"row\">");
            result.Add(" <div class=\"col-md-4\">");
            result.Add("  <form asp-action=\"Filter\">");
            result.Add("   <div asp-validation-summary=\"ModelOnly\" class=\"text-danger\"></div>");
            
            foreach (var viewItem in viewProperties)
            {
                if (CanCreate(viewItem) && QuerySetting<bool>(unitType, Common.ItemType.ViewFilterProperty, viewItem.DeclaringName(), StaticLiterals.Generate, "True"))
                {
                    result.Add("   <div class=\"form-group\">");
                    result.Add($"    <label asp-for=\"{viewItem.Name}\" class=\"control-label\"></label>");
                    if (viewItem.PropertyType.IsEnum)
                    {
                        result.Add("   @{");
                        result.Add($"    var values{viewItem.Name} = new SelectListItem[]" + "{ new SelectListItem() }" + $".Union(Enum.GetValues(typeof({viewItem.PropertyType})).Cast<{viewItem.PropertyType}>().Select(e => new SelectListItem(e.ToString(), e.ToString())));");
                        result.Add("   }");
                        result.Add($"   @Html.DropDownListFor(m => m.{viewItem.Name}, values{viewItem.Name}, null" + ", new { @class = \"form-select\" })");
                    }
                    else if (viewItem.PropertyType == typeof(bool) || viewItem.PropertyType == typeof(bool?))
                    {
                        result.Add($"    @Html.DropDownListFor(model => model.{viewItem.Name}, boolSelect" + ", new { @class = \"form-select\" })");
                    }
                    else
                    {
                        result.Add($"    <input asp-for=\"{viewItem.Name}\" class=\"form-control\" />");
                    }
                    result.Add("   </div>");
                }
            }
            
            result.Add("   <p></p>");
            result.Add("   <div class=\"form-group\">");
            result.Add("    <input type=\"submit\" value=\"Apply\" class=\"btn btn-primary\" style=\"min-width: 100px;\" />");
            result.Add("    @Html.ActionLink(\"Clear\", \"Clear\", null, null, new { @class=\"btn btn-outline-success\", @style=\"min-width: 100px;\" })");
            result.Add("   </div>");
            result.Add("  </form>");
            
            result.Add(" </div>");
            result.Add("</div>");
            return result;
        }
        /// <summary>
        /// Creates the filter auto property.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns>An enumerable of string containing the filter auto property.</returns>
        private IEnumerable<string> CreateFilterAutoProperty(PropertyInfo propertyInfo)
        {
            var result = new List<string>();
            var propertyType = GetPropertyType(propertyInfo);
            
            if (propertyType.EndsWith("?") == false)
            {
                propertyType = $"{propertyType}?";
            }
            result.Add(string.Empty);
            result.AddRange(CreateComment(propertyInfo));
            result.Add($"public {propertyType} {propertyInfo.Name}");
            result.Add("{ get; set; }");
            return result;
        }
        
        #region query configuration
        /// <summary>
        /// Queries a setting value of type T for a specific item type, type, value name, and default value.
        /// </summary>
        /// <typeparam name="T">The type of the setting value to query.</typeparam>
        /// <param name="itemType">The item type to query the setting for.</param>
        /// <param name="type">The type to query the setting for.</param>
        /// <param name="valueName">The name of the setting value to query.</param>
        /// <param name="defaultValue">The default value to be returned if the setting value is not found.</param>
        /// <returns>The queried setting value of type T.</returns>
        private T QuerySetting<T>(Common.ItemType itemType, Type type, string valueName, string defaultValue)
        {
            return QuerySetting<T>(Common.UnitType.AspMvc, itemType, type, valueName, defaultValue);
        }
        /// <summary>
        /// Queries the setting value of type <typeparamref name="T"/> for the given <paramref name="itemType"/>, <paramref name="itemName"/>, <paramref name="valueName"/>, and <paramref name="defaultValue"/>.
        /// </summary>
        /// <typeparam name="T">The type of the setting value.</typeparam>
        /// <param name="itemType">The type of the item.</param>
        /// <param name="itemName">The name of the item.</param>
        /// <param name="valueName">The name of the value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The setting value of type <typeparamref name="T"/>.</returns>
        private T QuerySetting<T>(Common.ItemType itemType, string itemName, string valueName, string defaultValue)
        {
            return QuerySetting<T>(Common.UnitType.AspMvc, itemType, itemName, valueName, defaultValue);
        }
        #endregion query configuration
        
        #region Partial methods
        /// <summary>
        /// Creates model attributes for a given type.
        /// </summary>
        /// <param name="type">The type for which model attributes are to be created.</param>
        /// <param name="source">The list of strings containing the source data for attribute creation.</param>
        /// <remarks>
        /// This method is used to create model attributes based on the provided source data for the given type.
        /// Only a partial implementation is provided, allowing for further customization in other parts of the code.
        /// </remarks>
        /// <seealso cref="Type"/>
        /// <seealso cref="List{T}"/>
        /// <exception cref="ArgumentNullException">
        /// Thrown when either <paramref name="type"/> or <paramref name="source"/> is null.
        /// </exception>
        partial void CreateModelAttributes(Type type, List<string> source);
        /// <summary>
        /// Creates controller attributes for a given type and a list of code lines.
        /// </summary>
        /// <param name="type">The type of the controller.</param>
        /// <param name="codeLines">The list of code lines to generate attributes.</param>
        /// <remarks>
        /// This method is used to generate controller attributes based on the provided type and code lines.
        /// It is a partial method, so its implementation can be defined in separate files of the same class.
        /// </remarks>
        partial void CreateControllerAttributes(Type type, List<string> codeLines);
        #endregion Partial methods
    }
}
//MdEnd

