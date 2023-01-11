//@BaseCode
//MdStart
namespace QuickTemplate.AspMvc.Modules.Session
{
    public partial interface ISessionWrapper
    {
        #region General
        bool HasValue(string key);
        void Remove(string key);
        #endregion General

        #region Type-Access
        void Set<T>(string key, T value);
        T? Get<T>(string key);
        #endregion Type-Access

        #region Object-Access
        void SetValue(string key, object value);
        object? GetValue(string key);
        #endregion Object-Access

        #region Int-Access
        void SetIntValue(string key, int value);
        int? GetIntValue(string key);
        #endregion Int-Access

        #region String-Access
        void SetStringValue(string key, string value);
        string? GetStringValue(string key);
        string? GetStringValue(string key, string defaultValue);
        #endregion String-Access

        #region Properties
        string? ReturnUrl { get; set; }
        string? ReturnController { get; set; }
        string? ReturnAction { get; set; }
        string? Hint { get; set; }
        string? Error { get; set; }
        #endregion Properties

#if ACCOUNT_ON
        #region Authentication
        Models.Account.LoginSession? LoginSession { get; set; }
        string SessionToken => LoginSession?.SessionToken ?? string.Empty;
        bool IsAuthenticated { get; }
        bool IsSessionAlive { get; }
        #endregion Authentication
#endif
    }
}
//MdEnd
