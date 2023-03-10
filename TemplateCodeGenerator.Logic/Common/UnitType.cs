//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic.Common
{
    [Flags]
    public enum UnitType : long
    {
        General = 1,

        Logic = 2 * General,
        WebApi = 2 * Logic,

        AspMvc = 2 * WebApi,
        Angular = 2 * AspMvc,

        MVVM = 2 * Angular,
    }
}
//MdEnd
