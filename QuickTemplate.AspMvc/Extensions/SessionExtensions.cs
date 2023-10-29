//@BaseCode
//MdStart
using System.Text.Json;

namespace QuickTemplate.AspMvc.Extensions
{
    /// <summary>
    /// Provides extension methods to store and retrieve values from an ISession object.
    /// </summary>
    public static partial class SessionExtensions
    {
        /// <summary>
        /// Sets the specified value in the session using the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="session">The session object.</param>
        /// <param name="key">The key to associate with the value.</param>
        /// <param name="value">The value to be set.</param>
        /// <remarks>
        /// Serializes the value using the JsonSerializer before storing it in the session.
        /// </remarks>
        public static void Set<T>(this ISession session, string key, T? value)
        {
            string strValue = JsonSerializer.Serialize(value);
            
            session.SetString(key, strValue);
        }
        
        /// <summary>
        /// Retrieves the value associated with the specified key from the session and deserializes it into the specified type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the value into.</typeparam>
        /// <param name="session">The session from which to retrieve the value.</param>
        /// <param name="key">The key of the value to retrieve.</param>
        /// <returns>
        /// The deserialized value associated with the specified key, or the default value of the specified type if the key is not found or the value is null.
        /// </returns>
        public static T? Get<T>(this ISession session, string key)
        {
            var strValue = session.GetString(key);
            
            return strValue == null ? default : JsonSerializer.Deserialize<T>(strValue);
        }
    }
}
//MdEnd

