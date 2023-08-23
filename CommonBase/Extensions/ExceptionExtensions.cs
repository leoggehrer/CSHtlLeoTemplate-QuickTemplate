//@BaseCode
//MdStart
namespace CommonBase.Extensions
{
    public static partial class ExceptionExtensions
    {
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
