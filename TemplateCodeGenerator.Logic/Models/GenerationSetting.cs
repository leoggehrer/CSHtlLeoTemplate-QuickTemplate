//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic.Models
{
    using TemplateCodeGenerator.Logic.Common;
    internal class GenerationSetting
    {
        public string UnitType { get; set; } = string.Empty;
        public string ItemType { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{UnitType}-{ItemType}-{ItemName}-{Name}-{Value}";
        }
    }
}
//MdEnd
