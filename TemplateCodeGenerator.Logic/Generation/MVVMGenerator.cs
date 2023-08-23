//@BaseCode
//MdStart
using TemplateCodeGenerator.Logic.Contracts;

namespace TemplateCodeGenerator.Logic.Generation
{
    internal sealed partial class MVVMGenerator : ModelGenerator
    {
        #region fields
        private ItemProperties? _itemProperties;
        #endregion fields
        protected override ItemProperties ItemProperties => _itemProperties ??= new ItemProperties(SolutionProperties.SolutionName, StaticLiterals.MVVMExtension);

        #region properties
        public bool GenerateModels { get; set; }
        #endregion properties

        public MVVMGenerator(ISolutionProperties solutionProperties) : base(solutionProperties)
        {
            string generateAll = QuerySetting<string>(Common.ItemType.AllItems, StaticLiterals.AllItems, StaticLiterals.Generate, "True");

            GenerateModels = QuerySetting<bool>(Common.ItemType.AccessModel, StaticLiterals.AllItems, StaticLiterals.Generate, generateAll);
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
                var generate = CanCreate(type) && base.QuerySetting<bool>(Common.UnitType.MVVM, Common.ItemType.AccessModel, type, StaticLiterals.Generate, GenerateModels.ToString());

                if (generate)
                {
                    result.Add(CreateModelFromType(type, Common.UnitType.MVVM, Common.ItemType.AccessModel));
                    result.Add(CreateModelInheritance(type, Common.UnitType.MVVM, Common.ItemType.AccessModel));
                }
            }
            return result;
        }

        #region query configuration
        private T QuerySetting<T>(Common.ItemType itemType, string itemName, string valueName, string defaultValue)
        {
            return QuerySetting<T>(Common.UnitType.AspMvc, itemType, itemName, valueName, defaultValue);
        }
        #endregion query configuration
    }
}
//MdEnd
