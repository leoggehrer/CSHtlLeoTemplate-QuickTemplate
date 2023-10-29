//@BaseCode
//MdStart
using TemplateCodeGenerator.Logic.Contracts;

namespace TemplateCodeGenerator.Logic.Generation
{
    /// <summary>
    /// Represents a class that generates MVVM (Model-View-ViewModel) items based on solution properties.
    /// </summary>
    /// <inheritdoc/>
    // ...
    internal sealed partial class MVVMGenerator : ModelGenerator
    {
        #region fields
        private ItemProperties? _itemProperties;
        #endregion fields
        /// <summary>
        /// Gets the item properties.
        /// </summary>
        /// <value>The item properties.</value>
        protected override ItemProperties ItemProperties => _itemProperties ??= new ItemProperties(SolutionProperties.SolutionName, StaticLiterals.MVVMExtension);
        
        #region properties
        /// <summary>
        /// Gets or sets a value indicating whether to generate models.
        /// </summary>
        /// <value>
        ///   <c>true</c> if models should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateModels { get; set; }
        #endregion properties
        
        /// <summary>
        /// Represents a class for generating MVVM architecture.
        /// </summary>
        /// <param name="solutionProperties">The solution properties.</param>
        public MVVMGenerator(ISolutionProperties solutionProperties) : base(solutionProperties)
        {
            string generateAll = QuerySetting<string>(Common.ItemType.AllItems, StaticLiterals.AllItems, StaticLiterals.Generate, "True");
            
            GenerateModels = QuerySetting<bool>(Common.ItemType.AccessModel, StaticLiterals.AllItems, StaticLiterals.Generate, generateAll);
        }
        
        /// <summary>
        /// Generates all the items and returns them as an enumerable collection.
        /// </summary>
        /// <returns>An enumerable collection of generated items.</returns>
        public IEnumerable<IGeneratedItem> GenerateAll()
        {
            var result = new List<IGeneratedItem>();
            
            result.AddRange(CreateModels());
            return result;
        }
        
        /// <summary>
        /// Creates models based on entity types and service types.
        /// </summary>
        /// <returns>An IEnumerable of IGeneratedItem representing the created models.</returns>
        private IEnumerable<IGeneratedItem> CreateModels()
        {
            var result = new List<IGeneratedItem>();
            var entityProject = EntityProject.Create(SolutionProperties);
            var createTypes = entityProject.EntityTypes.Union(entityProject.ServiceTypes);
            
            foreach (var type in createTypes)
            {
                var generate = CanCreate(type) && base.QuerySetting<bool>(Common.UnitType.MVVMApp, Common.ItemType.AccessModel, type, StaticLiterals.Generate, GenerateModels.ToString());
                
                if (generate)
                {
                    result.Add(CreateModelFromType(type, Common.UnitType.MVVMApp, Common.ItemType.AccessModel));
                    result.Add(CreateModelInheritance(type, Common.UnitType.MVVMApp, Common.ItemType.AccessModel));
                }
            }
            return result;
        }
        
        #region query configuration
        /// <summary>
        /// Queries the setting for the specified item type, item name, value name, and default value.
        /// </summary>
        /// <typeparam name="T">The type of the setting.</typeparam>
        /// <param name="itemType">The item type of the setting.</param>
        /// <param name="itemName">The name of the item.</param>
        /// <param name="valueName">The name of the value.</param>
        /// <param name="defaultValue">The default value if the setting is not found.</param>
        /// <returns>The queried setting of type T.</returns>
        private T QuerySetting<T>(Common.ItemType itemType, string itemName, string valueName, string defaultValue)
        {
            return QuerySetting<T>(Common.UnitType.AspMvc, itemType, itemName, valueName, defaultValue);
        }
        #endregion query configuration
    }
}
//MdEnd
