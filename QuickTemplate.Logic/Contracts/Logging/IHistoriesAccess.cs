//@CodeCopy
//MdStart
#if ACCOUNT_ON && LOGGING_ON
using CommonBase.Contracts;

namespace QuickTemplate.Logic.Contracts.Logging
{
    public partial interface IActionLogsAccess<T> : IDataAccess<T>
    {
    }
}
#endif
//MdEnd
