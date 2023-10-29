//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic.Common
{
    [Flags]
    public enum ItemType : ulong
    {
        #region contracts
        AccessContract,         // controller access contract
        ServiceAccessContract,  // service access contract
        ServiceContract,        // service access contract
        ModelContract,          // model access contract
        #endregion contracts

        #region models and properties
        Property,

        ModelProperty,
        InterfaceProperty,
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
        File,
        Lambda,
    }
}
//MdEnd
