//@BaseCode
//MdStart
#if ACCOUNT_ON && LOGGING_ON
using CommonBase.Contracts;

namespace QuickTemplate.Logic.Contracts.Logging
{
    /// <summary>
    /// Represents an interface for accessing action logs data.
    /// </summary>
    /// <typeparam name="T">The type of data to be accessed.</typeparam>
    public partial interface IActionLogsAccess<T> : IDataAccess<T>
    {
    }
}
#endif
//MdEnd
