//@BaseCode
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
namespace QuickTemplate.Logic.Modules.Access
{
    public enum RuleType
    {
        EntityType = 1,
        PropertyType = 2 * EntityType,
        Entities = 2 * PropertyType,
        EntityBy = 2 * Entities,
        Properties = 2 * EntityBy,
        PropertyBy = 2 * Properties,
    }
}
#endif
//MdEnd
