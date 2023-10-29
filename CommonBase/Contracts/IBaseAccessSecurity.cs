//@BaseCode
//MdStart
#if ACCOUNT_ON
namespace CommonBase.Contracts
{
    /// <summary>
    /// Extends IBaseAccess with SessionToken.
    /// </summary>
    /// <typeparam name="T">The generic type.</typeparam>
    partial interface IBaseAccess<T>
    {
        /// <summary>
        /// Sets the authorization token.
        /// </summary>
        string SessionToken { set; }
    }
}
#endif
//MdEnd


