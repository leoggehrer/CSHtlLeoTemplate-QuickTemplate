//@CodeCopy
//MdStart
using System.Text;

namespace TemplateCodeGenerator.Logic.Preprocessor
{
    public class RazorFile
    {
        public static void SetPreprocessorDefineCommentsInRazorFiles(string path, params string[] defineItems)
        {
            foreach (var define in defineItems.Select(d => d.ToUpper()))
            {
                SetPreprocessorDefineCommentsInRazorFiles(path, define);
            }
        }
        public static void SetPreprocessorDefineCommentsInRazorFiles(string path, string define)
        {
            var csHtmlfiles = Directory.GetFiles(path, "*.cshtml", SearchOption.AllDirectories);
            var razorfiles = Directory.GetFiles(path, "*.razor", SearchOption.AllDirectories);
            var files = csHtmlfiles.Union(razorfiles);
            var searchIfStart = define.EndsWith("_ON") ? $"@*#if {define}*@@*{Environment.NewLine}"
                                                       : $"@*#if {define.Replace("_OFF", "_ON")}*@{Environment.NewLine}";
            var replaceIfStart = define.EndsWith("_ON") ? $"@*#if {define}*@{Environment.NewLine}"
                                                        : $"@*#if {define.Replace("_OFF", "_ON")}*@@*{Environment.NewLine}";
            var searchIfEnd = define.EndsWith("_ON") ? $"{Environment.NewLine}*@@*#endif*@"
                                                     : $"{Environment.NewLine}@*#endif*@";
            var replaceIfEnd = define.EndsWith("_ON") ? $"{Environment.NewLine}@*#endif*@"
                                                      : $"{Environment.NewLine}*@@*#endif*@";

            foreach (var file in files)
            {
                var startIndex = 0;
                var hasChanged = false;
                var result = string.Empty;
                var text = File.ReadAllText(file, Encoding.Default);

                foreach (var tag in text.GetAllTags(searchIfStart, searchIfEnd))
                {
                    hasChanged = true;

                    if (tag.StartTagIndex >= startIndex)
                    {
                        result += text.Partialstring(startIndex, tag.StartTagIndex - 1);
                        result += replaceIfStart;
                        result += tag.InnerText;
                        result += replaceIfEnd;

                        startIndex += tag.EndTagIndex + tag.EndTag.Length;
                    }
                }
                if (hasChanged && startIndex < text.Length)
                {
                    result += text.Partialstring(startIndex, text.Length);
                }
                if (hasChanged)
                {
                    File.WriteAllText(file, result, Encoding.Default);
                }
            }
        }
    }
}
//MdEnd
