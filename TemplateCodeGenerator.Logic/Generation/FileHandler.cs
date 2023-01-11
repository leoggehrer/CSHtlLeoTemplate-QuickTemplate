//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic.Generation
{
    using System.Text;
    internal class FileHandler
    {
        public static bool IsTypeScriptFile(string filePath)
        {
            return Path.GetExtension(filePath).Equals(".ts", StringComparison.CurrentCultureIgnoreCase);
        }
        public static string CreateCustomFilePath(string filePath)
        {
            var path = Path.GetDirectoryName(filePath);
            var customFileName = $"{Path.GetFileNameWithoutExtension(filePath)}{StaticLiterals.CustomFileExtension}";
            
            return Path.Combine(path!, customFileName);
        }

        public static IEnumerable<string> ReadAngularCustomParts(string filePath)
        {
            var result = new List<string>();

            result.Add(StaticLiterals.AngularCustomImportBeginLabel);
            result.AddRange(ReadAngularCustomImports(filePath));
            result.Add(StaticLiterals.AngularCustomImportEndLabel);

            result.Add(StaticLiterals.AngularCustomCodeBeginLabel);
            result.AddRange(ReadAngularCustomCode(filePath));
            result.Add(StaticLiterals.AngularCustomCodeEndLabel);

            return result;
        }
        public static IEnumerable<string> ReadAngularCustomImports(string filePath)
        {
            var result = new List<string>();

            result.AddRange(ReadCustomPart(filePath, StaticLiterals.AngularCustomImportBeginLabel, StaticLiterals.AngularCustomImportEndLabel));

            return result;
        }
        public static IEnumerable<string> ReadAngularCustomCode(string filePath)
        {
            var result = new List<string>();

            result.AddRange(ReadCustomPart(filePath, StaticLiterals.AngularCustomCodeBeginLabel, StaticLiterals.AngularCustomCodeEndLabel));

            return result;
        }
        public static IEnumerable<string> ReadCustomPart(string filePath, string beginLabel, string endLabel)
        {
            var result = new List<string>();

            if (File.Exists(filePath))
            {
                var source = File.ReadAllText(filePath, Encoding.Default);

                foreach (var item in source.GetAllTags(new string[] { $"{beginLabel}{Environment.NewLine}", $"{endLabel}" })
                               .OrderBy(e => e.StartTagIndex))
                {
                    if (item.InnerText.HasContent())
                    {
                        result.AddRange(item.InnerText.ToLines().Where(l => l.HasContent()));
                    }
                }
            }
            return result;
        }

        public static string SaveAngularCustomParts(string filePath)
        {
            var result = CreateCustomFilePath(filePath);
            var lines = ReadAngularCustomParts(filePath);

            File.WriteAllLines(result, lines, Encoding.UTF8);

            return result;
        }
        public static IEnumerable<string> ReadAndDelete(string filePath)
        {
            var result = new List<string>();

            if (File.Exists(filePath))
            {
                result.AddRange(File.ReadAllLines(filePath, Encoding.UTF8));
                File.Delete(filePath);
            }
            return result;
        }
    }
}
//MdEnd