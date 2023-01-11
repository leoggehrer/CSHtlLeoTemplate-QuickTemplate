//@BaseCode
//MdStart
#if ACCOUNT_ON && ACCESSRULES_ON
namespace QuickTemplate.Logic.Modules.Exceptions
{
    /// <summary>
    /// Represents errors that occur during application execution.
    /// </summary>
    public partial class AccessRuleException : LogicException
    {
        /// <summary>
        /// Initializes a new instance of the AccessRuleException class with a specified error message.
        /// </summary>
        /// <param name="errorType">Identification of the error message.</param>
        public AccessRuleException(ErrorType errorType)
            : base(errorType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the AccessRuleException class with a specified error message.
        /// </summary>
        /// <param name="errorType">Identification of the error message.</param>
        /// <param name="message">The message that describes the error.</param>
        public AccessRuleException(ErrorType errorType, string message)
            : base(errorType, message)
        {
        }
    }
}
#endif
//MdEnd