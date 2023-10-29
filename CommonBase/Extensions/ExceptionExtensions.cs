//@BaseCode
//MdStart
namespace CommonBase.Extensions
{
    /// <summary>
    /// Provides extension methods for handling exceptions.
    /// </summary>
    public static partial class ExceptionExtensions
    {
        /// <summary>
        /// Retrieves the error message from an exception, including all inner exception messages.
        /// </summary>
        /// <param name="source">The exception from which to retrieve the error message.</param>
        /// <returns>
        /// The error message from the exception, including all inner exception messages.
        /// </returns>
        public static string GetError(this Exception source)
        {
            var result = source.Message.Replace("See the inner exception for details.", string.Empty).Trim();
            Exception? innerException = source.InnerException;
            
            while (innerException != null)
            {
                result = $"{result} {innerException.Message}";
                innerException = innerException.InnerException;
            }
            return result;
        }
    }
}
//MdEnd


