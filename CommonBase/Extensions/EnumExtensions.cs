//@BaseCode
//MdStart
namespace CommonBase.Extensions
{
    /// <summary>
    /// Provides extension methods for working with enum types.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns the first value of the specified enumeration type.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="src">The enumeration of type T.</param>
        /// <returns>The first value of the enumeration.</returns>
        /// <exception cref="ArgumentException">Thrown when the argument is not an enumeration.</exception>
        public static T FirstEnum<T>(this T src) where T : struct, Enum
        {
            if (typeof(T).IsEnum == false)
            throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));
            
            var values = (T[])Enum.GetValues(src.GetType());
            
            return values[0];
        }
        
        /// <summary>
        /// Returns the next enum value of the specified enum.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="src">The current enum value.</param>
        /// <returns>The next enum value.</returns>
        /// <exception cref="ArgumentException">Thrown if the type is not an enum.</exception>
        public static T NextEnum<T>(this T src) where T : struct, Enum
        {
            if (typeof(T).IsEnum == false)
            throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));
            
            var values = (T[])Enum.GetValues(src.GetType());
            var idx = Array.IndexOf<T>(values, src) + 1;
            
            return (values.Length == idx) ? values[0] : values[idx];
        }
        
        /// <summary>
        /// Converts an enum type to a dictionary with integer keys and string values.
        /// </summary>
        /// <typeparam name="T">The enum type to convert.</typeparam>
        /// <param name="src">The enum type instance.</param>
        /// <returns>A dictionary with integer keys and string values representing the enum type.</returns>
        /// <exception cref="ArgumentException">Thrown when the generic argument type is not an enum.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the enum type is not a struct.</exception>
        public static Dictionary<int, string> ToDictionary<T>(this T src) where T : struct, Enum
        {
            return Enum.GetValues(src.GetType())
                       .Cast<T>()
                       .ToDictionary(t => (int)(object)t, t => t.ToString());
        }
    }
}
//MdEnd


