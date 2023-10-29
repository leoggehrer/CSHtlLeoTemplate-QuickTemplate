//@CodeCopy
//MdStart
#if ACCOUNT_ON && REVISION_ON
using CommonBase.Contracts;

namespace QuickTemplate.Logic.Contracts.Revision
{
    /// <summary>
    /// Represents an interface for accessing histories.
    /// </summary>
    /// <typeparam name="T">The type of the data accessed.</typeparam>
    public partial interface IHistoriesAccess<T> : IDataAccess<T>
    {
    }
}
#endif
//MdEnd
