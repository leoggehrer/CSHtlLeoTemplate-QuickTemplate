//@BaseCode
//MdStart
using System;
using System.Reflection;
using TemplateCodeGenerator.Logic.Contracts;

namespace TemplateCodeGenerator.Logic.Generation
{
    internal sealed partial class MVVMGenerator : ModelGenerator
    {
        private ItemProperties? _itemProperties;
        protected override ItemProperties ItemProperties => _itemProperties ??= new ItemProperties(SolutionProperties.SolutionName, StaticLiterals.MVVMExtension);

        public bool GenerateModels { get; set; }

        public MVVMGenerator(ISolutionProperties solutionProperties) : base(solutionProperties)
        {
            GenerateModels = QuerySetting<bool>(Common.ItemType.Model, StaticLiterals.AllItems, StaticLiterals.Generate, "True");
        }

        public IEnumerable<IGeneratedItem> GenerateAll()
        {
            var result = new List<IGeneratedItem>();

            result.AddRange(CreateModels());
            return result;
        }

        private IEnumerable<IGeneratedItem> CreateModels()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);
            var createTypes = entityProject.EntityTypes.Union(entityProject.ServiceTypes);

            foreach (var type in createTypes)
            {
                var generate = CanCreate(type) && QueryModelSetting<bool>(Common.UnitType.MVVM, Common.ItemType.Model, type, StaticLiterals.Generate, GenerateModels.ToString());

                if (generate)
                {
                    result.Add(CreateModelFromType(type, Common.UnitType.MVVM, Common.ItemType.Model));
                    result.Add(CreateModelInheritance(type, Common.UnitType.MVVM, Common.ItemType.Model));
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

        private T QuerySetting<T>(Common.ItemType itemType, Type type, string valueName, string defaultValue)
        {
            return QuerySetting<T>(Common.UnitType.MVVM, itemType, type, valueName, defaultValue);
        }
        private T QuerySetting<T>(Common.UnitType unitType, Common.ItemType itemType, Type type, string valueName, string defaultValue)
        {
            T result;

            try
            {
                result = (T)Convert.ChangeType(QueryGenerationSettingValue(unitType, itemType, CreateEntitiesSubTypeFromType(type), valueName, defaultValue), typeof(T));
            }
            catch (Exception ex)
            {
                result = (T)Convert.ChangeType(defaultValue, typeof(T));
                System.Diagnostics.Debug.WriteLine($"Error in {MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
            }
            return result;
        }
        private T QuerySetting<T>(Common.ItemType itemType, string itemName, string valueName, string defaultValue)
        {
            T result;

            try
            {
                result = (T)Convert.ChangeType(QueryGenerationSettingValue(Common.UnitType.MVVM, itemType, itemName, valueName, defaultValue), typeof(T));
            }
            catch (Exception ex)
            {
                result = (T)Convert.ChangeType(defaultValue, typeof(T));
                System.Diagnostics.Debug.WriteLine($"Error in {MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
            }
            return result;
        }
    }
}
//MdEnd