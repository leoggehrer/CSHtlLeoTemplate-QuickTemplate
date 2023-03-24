//@BaseCode
//MdStart
#if ACCOUNT_ON && LOGGING_ON
namespace QuickTemplate.Logic.Contracts.Logging
{
    public partial interface IActionLogsAccess<T> : Contracts.IDataAccess<T>
    {
    }
}
#endif
//MdEnd