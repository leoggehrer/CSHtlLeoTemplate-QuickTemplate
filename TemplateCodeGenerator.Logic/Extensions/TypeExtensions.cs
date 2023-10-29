﻿//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic.Extensions
{
    using System.Text;
    /// <summary>
    /// Contains extension methods for <see cref="Type"/>.
    /// </summary>
    internal static partial class TypeExtensions
    {
        private const char GenericSpecialChar = '`';
        private const string GenericSeparator = ", ";
        
        /// <summary>
        /// Returns the clean name of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to get the clean name for</param>
        /// <returns>The clean name of the specified <paramref name="type"/></returns>
        public static string GetCleanName(this Type type)
        {
            var name = type.Name;
            
            if (type.IsGenericType)
            {
                name = name.Remove(name.IndexOf(GenericSpecialChar));
            }
            return name;
        }
        /// <summary>
        /// Generates a code definition for a given Type.
        /// </summary>
        /// <param name="type">The Type to generate code definition for.</param>
        /// <returns>A string representing the code definition of the Type.</returns>
        public static string GetCodeDefinition(this Type type)
        {
            var sb = new StringBuilder();
            
            sb.AppendFormat("{0}.{1}", type.Namespace, type.GetCleanName());
            if (type.IsGenericType)
            {
                var names = from genericArg in type.GetGenericArguments()
                select GetCodeDefinition(genericArg);
                sb.Append('<');
                sb.Append(string.Join(GenericSeparator, names.ToArray()));
                sb.Append('>');
            }
            if (sb.ToString().StartsWith("System.Nullable<")
            && sb.ToString().EndsWith(">"))
            {
                sb.Replace("System.Nullable<", string.Empty);
                sb.Replace(">", string.Empty);
                
                sb.Append('?');
            }
            return sb.ToString();
        }
    }
}
//MdEnd
