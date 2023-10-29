//@BaseCode
//MdStart
namespace CommonBase.Extensions
{
    /// <summary>
    /// Provides extension methods for formatting C# code.
    /// </summary>
    public static partial class FormatterExtensions
    {
        /// <summary>
        /// Gets or sets the indentation space used for formatting.
        /// </summary>
        /// <value>
        /// The indentation space used for formatting.
        /// </value>
        public static string IndentSpace { get; set; } = "    ";
        
        /// <summary>
        /// Formats a given C# code by replacing line breaks with the platform-specific line break and then formatting the code using a predefined set of rules.
        /// </summary>
        /// <param name="source">The C# code to be formatted.</param>
        /// <returns>The formatted C# code.</returns>
        public static string FormatCSharpCode(this string source)
        {
            var lines = source.Replace("\n", Environment.NewLine).ToLines();
            
            return lines.FormatCSharpCode().ToText();
        }
        /// <summary>
        /// Formats the given C# code by applying appropriate indentation.
        /// </summary>
        /// <param name="lines">An enumerable collection of C# code lines to be formatted.</param>
        /// <returns>An enumerable collection of formatted C# code lines.</returns>
        public static IEnumerable<string> FormatCSharpCode(this IEnumerable<string> lines)
        {
            int indent = 0;
            var result = new List<string>();
            
            foreach (var line in lines)
            {
                var formatLine = line.Trim();
                var hasOpenBlock = formatLine.Contains('{', '"', '"');
                var hasCloseBlock = formatLine.Contains('}', '"', '"');
                
                if (formatLine.StartsWith("#if")
                    || formatLine.StartsWith("#else")
                    || formatLine.StartsWith("#endif"))
                {
                    result.Add(formatLine);
                }
                else if (hasOpenBlock && hasCloseBlock)
                {
                    result.Add(formatLine.SetIndent(IndentSpace, indent));
                }
                else
                {
                    var offest = 0;
                    indent = hasCloseBlock ? indent - 1 : indent;
                    if (formatLine.StartsWith(".")
                    && result.Any()
                    && (offest = result.Last().IndexOf(".")) > -1)
                    {
                        result.Add(formatLine.SetIndent(" ", offest));
                    }
                    else
                    {
                        result.Add(formatLine.SetIndent(IndentSpace, indent));
                    }
                    indent = hasOpenBlock ? indent + 1 : indent;
                }
            }
            return result;
        }

        /// <summary>
        /// Checks if the given line is a comment line.
        /// </summary>
        /// <param name="line">The line to check.</param>
        /// <returns>Returns true if the line is a comment line; otherwise, false.</returns>
        public static bool IsCommentLine(this string line)
        {
            return IsXmlLineComment(line) || IsLineComment(line) || IsBlockCommentLine(line);
        }
        /// <summary>
        /// Determines whether the given string represents a line comment.
        /// </summary>
        /// <param name="line">The string to be checked.</param>
        /// <returns>
        /// <c>true</c> if the given string represents a line comment; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLineComment(this string line)
        {
            return line.Trim().StartsWith(@"//");
        }
        /// <summary>
        /// Checks if the provided string line is a XML line comment.
        /// </summary>
        /// <param name="line">The string line to be checked.</param>
        /// <returns>True if the line is a XML line comment, otherwise false.</returns>
        public static bool IsXmlLineComment(this string line)
        {
            return line.Trim().StartsWith(@"///");
        }
        /// <summary>
        /// Checks if a line of code contains a block comment.
        /// </summary>
        /// <param name="line">The line of code to check.</param>
        /// <returns>True if the line contains a block comment, otherwise false.</returns>
        public static bool IsBlockCommentLine(this string line)
        {
            var result = line.Trim();

            return result.StartsWith(@"/*") || result.StartsWith(@"*/") || result.StartsWith(@"*");
        }
    }
}
//MdEnd


