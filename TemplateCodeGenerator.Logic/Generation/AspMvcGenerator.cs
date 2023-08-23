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
        protected override ItemProperties ItemProperties => _itemProperties ??= new ItemProperties(SolutionProperties.SolutionName, StaticLiterals.AspMvcExtension);

        public bool GenerateAllAccessModels { get; set; }
        public bool GenerateAllServiceModels { get; set; }
        public bool GenerateAllFilterModels { get; set; }
        public bool GenerateAllAccessControllers { get; set; }
        public bool GenerateAllServiceControllers { get; set; }
        public bool GenerateAllServices { get; set; }
        public bool GenerateAddServices { get; set; }
        public bool GenerateAllViews { get; set; }
        #endregion properties

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
        private static bool IsPrimitiveNullable(PropertyInfo propertyInfo)
        {
            var result = propertyInfo.PropertyType.IsNullableType();

            if (result)
            {
                result = propertyInfo.PropertyType.GetGenericArguments()[0].IsPrimitive;
            }
            return result;
        }
        private static string CreateFilterModelName(Type type)
        {
            return $"{CreateModelName(type)}Filter";
        }
        private string CreateFilterModelType(Type type)
        {
            return $"{ItemProperties.Namespace}.{ItemProperties.CreateModelSubNamespace(type)}.{CreateFilterModelName(type)}";
        }

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
        private IEnumerable<IGeneratedItem> CreateModels()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);

            // Create access models
            foreach (var type in entityProject.EntityTypes)
            {
                var settingDefault = GenerateAllAccessModels.ToString();

                if (CanCreate(type)
                    && QuerySetting<bool>(Common.UnitType.AspMvc, Common.ItemType.AccessModel, type, StaticLiterals.Generate, settingDefault))
                {
                    result.Add(CreateModelFromType(type, Common.UnitType.AspMvc, Common.ItemType.AccessModel));
                    result.Add(CreateModelInheritance(type, Common.UnitType.AspMvc, Common.ItemType.AccessModel));
                }
            }

            // Create service models
            foreach (var type in entityProject.ServiceTypes)
            {
                var settingDefault = GenerateAllServiceModels.ToString();

                if (CanCreate(type)
                    && QuerySetting<bool>(Common.UnitType.AspMvc, Common.ItemType.ServiceModel, type, StaticLiterals.Generate, settingDefault))
                {
                    result.Add(CreateModelFromType(type, Common.UnitType.AspMvc, Common.ItemType.ServiceModel));
                    result.Add(CreateModelInheritance(type, Common.UnitType.AspMvc, Common.ItemType.ServiceModel));
                }
            }

            // Create filter models for access models
            foreach (var type in entityProject.EntityTypes)
            {
                var settingDefault = GenerateAllFilterModels.ToString();

                if (CanCreate(type)
                    && QuerySetting<bool>(Common.UnitType.AspMvc, Common.ItemType.AccessFilterModel, type, StaticLiterals.Generate, settingDefault))
                {
                    result.Add(CreateFilterModelFromType(type, Common.UnitType.AspMvc, Common.ItemType.AccessFilterModel));
                }
            }

            // Create filter models for service models
            foreach (var type in entityProject.ServiceTypes)
            {
                var settingDefault = GenerateAllFilterModels.ToString();

                if (CanCreate(type)
                    && QuerySetting<bool>(Common.UnitType.AspMvc, Common.ItemType.ServiceFilterModel, type, StaticLiterals.Generate, settingDefault))
                {
                    result.Add(CreateFilterModelFromType(type, Common.UnitType.AspMvc, Common.ItemType.ServiceFilterModel));
                }
            }
            return result;
        }

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
                var canGenerate = QuerySetting<bool>(unitType, Common.ItemType.FilterProperty, $"{CreateEntitiesSubTypeFromType(type)}Filter.{modelItem.Name}", StaticLiterals.Generate, "True");

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
                var canGenerate = QuerySetting<bool>(unitType, Common.ItemType.Property, $"{CreateEntitiesSubTypeFromType(type)}Filter.{item.Name}", StaticLiterals.Generate, "True");

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
        private IGeneratedItem CreateAccessControllerFromType(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var visibility = "public";
            var logicProject = $"{ItemProperties.SolutionName}{StaticLiterals.LogicExtension}";
            var accessType = $"{logicProject}.{ItemProperties.CreateModelSubType(type)}";
            var genericType = $"Controllers.FilterGenericController";
            var modelType = ItemProperties.CreateModelSubType(type);
            var controllerName = ItemProperties.CreateControllerName(type);
            var controllerClassName = ItemProperties.CreateControllerClassName(type);
            var contractType = ItemProperties.CreateAccessContractType(type);
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
        private IGeneratedItem CreateServiceControllerFromType(Type type, Common.UnitType unitType, Common.ItemType itemType)
        {
            var visibility = "public";
            var logicProject = $"{ItemProperties.SolutionName}{StaticLiterals.LogicExtension}";
            var accessType = $"{logicProject}.{ItemProperties.CreateSolutionTypeSubName(type)}";
            var genericType = $"Controllers.FilterGenericController";
            var modelType = ItemProperties.CreateModelType(type);
            var controllerName = ItemProperties.CreateServiceName(type);
            var controllerClassName = ItemProperties.CreateControllerClassName(type);
            var contractType = ItemProperties.CreateServiceContractType(type);
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

        private IGeneratedItem CreateAddServices()
        {
            var entityProject = EntityProject.Create(SolutionProperties);
            var result = new Models.GeneratedItem(Common.UnitType.AspMvc, Common.ItemType.AddServices)
            {
                FullName = $"{ItemProperties.Namespace}.Program",
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
                var generate = CanCreate(type)
                            && QuerySetting<bool>(Common.ItemType.AddServices, type, StaticLiterals.Generate, GenerateAddServices.ToString());

                if (generate && type.IsPublic)
                {
                    var logicProject = $"{ItemProperties.SolutionName}{StaticLiterals.LogicExtension}";
                    var contractType = ItemProperties.CreateAccessContractType(type);
                    var controllerType = ItemProperties.CreateLogicControllerType(type);

                    result.Add($"builder.Services.AddTransient<{contractType}, {controllerType}>();");
                }
                else if (generate)
                {
                    var logicProject = $"{ItemProperties.SolutionName}{StaticLiterals.LogicExtension}";
                    var contractType = ItemProperties.CreateAccessContractType(type);
                    var facadeType = $"{logicProject}.{ItemProperties.CreateFacadeSubType(type)}";

                    result.Add($"builder.Services.AddTransient<{contractType}, {facadeType}>();");
                }
            }
            foreach (var type in entityProject.ServiceTypes)
            {
                var generate = CanCreate(type)
                            && QuerySetting<bool>(Common.ItemType.AddServices, type, StaticLiterals.Generate, GenerateAddServices.ToString());

                if (generate)
                {
                    var contractType = ItemProperties.CreateServiceContractType(type);
                    var controllerType = ItemProperties.CreateLogicServiceType(type);

                    result.Add($"builder.Services.AddTransient<{contractType}, {controllerType}>();");
                }
            }
            result.Add("}");

            result.Add("}");
            result.EnvelopeWithANamespace(ItemProperties.Namespace);
            result.FormatCSharpCode();
            return result;
        }

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
        private static IEnumerable<PropertyInfo> GetViewProperties(Type type)
        {
            var typeProperties = type.GetAllPropertyInfos();
            var viewProperties = typeProperties.Where(e => StaticLiterals.VersionProperties.Any(p => p.Equals(e.Name)) == false
                                                        && StaticLiterals.ExtendedProperties.Any(p => p.Equals(e.Name)) == false
                                                        && IsListType(e.PropertyType) == false
                                                        && (e.PropertyType.IsEnum || e.PropertyType.IsValueType || e.PropertyType.IsPrimitive || IsPrimitiveNullable(e) || e.PropertyType == typeof(string)));

            return viewProperties;
        }
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
                if (QuerySetting<bool>(Common.ItemType.ViewTableProperty, viewItem.DeclaringName(), StaticLiterals.Generate, "True"))
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
                if (QuerySetting<bool>(Common.ItemType.ViewTableProperty, viewItem.DeclaringName(), StaticLiterals.Generate, "True"))
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

            foreach (var item in viewProperties)
            {
                var attribute = StaticLiterals.ExtendedProperties.Any(e => e.Equals(item.Name)) ? "readonly=\"readonly\"" : string.Empty;

                result.Add("  <div class=\"form-group\">");
                result.Add($"   <label asp-for=\"{item.Name}\" class=\"control-label\"></label>");
                if (item.PropertyType.IsEnum)
                {
                    result.Add("   @{");
                    result.Add($"    var values{item.Name} = Enum.GetValues(typeof({item.PropertyType})).Cast<{item.PropertyType}>().Select(e => new SelectListItem(e.ToString(), e.ToString()));");
                    result.Add("   }");
                    result.Add($"   @Html.DropDownListFor(m => m.{item.Name}, values{item.Name}, null" + ", new { @class = \"form-select\" })");
                }
                else if (item.PropertyType == typeof(bool) || item.PropertyType == typeof(bool?))
                {
                    result.Add($"   <input asp-for=\"{item.Name}\" class=\"form-check\" {attribute} />");
                }
                else
                {
                    result.Add($"   <input asp-for=\"{item.Name}\" class=\"form-control\" {attribute} />");
                }
                result.Add($"   <span asp-validation-for=\"{item.Name}\" class=\"text-danger\"></span>");
                result.Add("  </div>");
            }

            result.Add(" </div>");
            result.Add("</div>");
            return result;
        }
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

            foreach (var item in viewProperties)
            {
                result.Add(" <dt class=\"col-sm-2\">");
                result.Add($"  @Html.DisplayNameFor(model => model.{item.Name})");
                result.Add(" </dt>");
                result.Add(" <dd class=\"col-sm-10\">");
                result.Add($"  @Html.DisplayFor(model => model.{item.Name})");
                result.Add(" </dd>");
            }

            result.Add("</dl>");
            return result;
        }

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
                var canCreate = QuerySetting<bool>(unitType, Common.ItemType.ViewFilterProperty, viewItem.DeclaringName(), StaticLiterals.Generate, "True");

                if (canCreate)
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
        private T QuerySetting<T>(Common.ItemType itemType, Type type, string valueName, string defaultValue)
        {
            return QuerySetting<T>(Common.UnitType.AspMvc, itemType, type, valueName, defaultValue);
        }
        private T QuerySetting<T>(Common.ItemType itemType, string itemName, string valueName, string defaultValue)
        {
            return QuerySetting<T>(Common.UnitType.AspMvc, itemType, itemName, valueName, defaultValue);
        }
        #endregion query configuration

        #region Partial methods
        partial void CreateModelAttributes(Type type, List<string> source);
        partial void CreateControllerAttributes(Type type, List<string> codeLines);
        #endregion Partial methods
    }
}
//MdEnd
