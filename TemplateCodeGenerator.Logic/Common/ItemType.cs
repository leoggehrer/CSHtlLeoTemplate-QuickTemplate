//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic.Common
{
    [Flags]
    public enum ItemType : ulong
    {
        #region contracts
        AccessContract,
        ServiceAccessContract,
        ServiceContract,
        #endregion contracts

        #region models and properties
        Property,

        ModelProperty,
        AccessModel,
        ServiceModel,
        ServiceModelProperty,

        EditModel,
        EditProperty,

        FilterProperty,
        AccessFilterModel,
        ServiceFilterModel,
        #endregion models and properties

        DbContext,
        AddServices,

        Controller,
        AccessController,
        ServiceController,
        AccessService,
        Service,
        Facade,

        Factory,
        FactoryControllerMethode,
        FactoryFacadeMethode,

        View,
        ViewTableProperty,
        ViewFilterProperty,
        ViewEditProperty,

        TypeScriptEnum,
        TypeScriptModel,
        TypeScriptService,

        AllItems,
        Lambda,
    }
}
//MdEnd
