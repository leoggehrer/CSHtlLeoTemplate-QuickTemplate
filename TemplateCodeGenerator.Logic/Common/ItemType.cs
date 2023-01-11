//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic.Common
{
    [Flags]
    public enum ItemType : ulong
    {
        Model,
        ModelProperty,
        EditModel,
        EditProperty,
        FilterModel,
        FilterProperty,

        AccessContract,
        ServiceContract,

        Property,

        DbContext,

        Controller,
        Service,
        Facade,

        Factory,
        FactoryControllerMethode,
        FactoryFacadeMethode,

        AddServices,

        View,
        ViewTableProperty,
        ViewFilterProperty,
        ViewEditProperty,

        TypeScriptEnum,
        TypeScriptModel,
        TypeScriptService,
    }
}
//MdEnd