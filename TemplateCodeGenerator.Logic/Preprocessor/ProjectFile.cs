//@BaseCode
//MdStart
using System;
using System.Text;

namespace TemplateCodeGenerator.Logic.Preprocessor
{
    public static partial class ProjectFile
    {
        #region Class-Constructors
        static ProjectFile()
        {
            ClassConstructing();
            Defines = new string[]
            {
                "ACCOUNT_OFF",
                "DBOPERATION_OFF",
                "ROWVERSION_ON",
                "IDINT_ON",
                "IDLONG_OFF",
                "IDGUID_OFF",
                "SQLSERVER_ON",
                "SQLITE_OFF",
                $"{GeneratedCodePrefix}_OFF",
            };
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        #endregion Class-Constructors

        public static string OnPostfix => "_ON";
        public static string OffPostfix => "_OFF";
        public static string GeneratedCodePrefix => "GENERATEDCODE";
        public static string[] Defines { get; set; }

        public static void SwitchDefine(string[] defines, int idx)
        {
            if (idx >= 0 && idx < defines.Length)
            {
                if (defines[idx].EndsWith(OnPostfix))
                {
                    defines[idx] = defines[idx].Replace(OnPostfix, OffPostfix);
                }
                else if (defines[idx].EndsWith(OffPostfix))
                {
                    defines[idx] = defines[idx].Replace(OffPostfix, OnPostfix);
                }
            }
        }
        public static void SwitchDefine(string[] defines, string definePrefix, string definePostfix)
        {
            bool hasSet = false;

            for (int i = 0; i < defines.Length && hasSet == false; i++)
            {
                if (defines[i].StartsWith(definePrefix))
                {
                    hasSet = true;
                    defines[i] = $"{definePrefix}{definePostfix}";
                }
            }
        }

        private static string[] ReadDefinesInProjectFile(string filePath, params string[] startDefines)
        {
            var result = new List<string>();

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath, Encoding.Default);

                foreach (var line in lines)
                {
                    var defines = line.ExtractBetween("<DefineConstants>", "</DefineConstants>");

                    if (defines.HasContent())
                    {
                        defines.Split(";", StringSplitOptions.RemoveEmptyEntries)
                               .ToList()
                               .ForEach(e =>
                               {
                                   var tmp = result.FirstOrDefault(x => x.RemoveAll("OFF", "ON") == e.RemoveAll("OFF", "ON"));

                                   if (string.IsNullOrEmpty(tmp))
                                   {
                                       result.Add(e);
                                   }
                               });
                    }
                }
            }

            foreach (var startDefine in startDefines)
            {
                var tmp = result.FirstOrDefault(x => x.RemoveAll("OFF", "ON") == startDefine.RemoveAll("OFF", "ON"));

                if (string.IsNullOrEmpty(tmp))
                {
                    result.Add(startDefine);
                }
            }
            return result.ToArray();
        }
        public static string[] ReadDefinesInProjectFiles(string path)
        {
            return ReadDefinesInProjectFiles(path, Defines);
        }
        public static string[] ReadDefinesInProjectFiles(string path, string[] startDefines)
        {
            var result = new List<string>();
            var files = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories)
                                 .Where(f => f.Contains(".AngularApp") == false);

            foreach (var file in files)
            {
                var defines = ReadDefinesInProjectFile(file);

                defines.ToList()
                       .ForEach(e =>
                       {
                           var tmp = result.FirstOrDefault(x => x.RemoveAll("OFF", "ON") == e.RemoveAll("OFF", "ON"));

                           if (string.IsNullOrEmpty(tmp))
                           {
                               result.Add(e);
                           }
                       });
            }

            foreach (var startDefine in startDefines)
            {
                var tmp = result.FirstOrDefault(x => x.RemoveAll("OFF", "ON") == startDefine.RemoveAll("OFF", "ON"));

                if (string.IsNullOrEmpty(tmp))
                {
                    result.Add(startDefine);
                }
            }
            return result.ToArray();
        }

        private static void WriteDefinesInProjectFile(string filePath, string[] defines)
        {
            var defineConstants = string.Join(";", defines);

            if (File.Exists(filePath))
            {
                var hasChanged = false;
                var result = new List<string>();
                var lines = File.ReadAllLines(filePath, Encoding.Default);

                foreach (var line in lines)
                {
                    if (line.Contains("<DefineConstants>", "</DefineConstants>"))
                    {
                        hasChanged = true;
                        result.Add(line.ReplaceBetween("<DefineConstants>", "</DefineConstants>", defineConstants));
                    }
                    else
                    {
                        result.Add(line);
                    }
                }
                if (hasChanged == false && defineConstants.Length > 0)
                {
                    var insertIdx = result.FindIndex(e => e.Contains("</PropertyGroup>"));

                    insertIdx = insertIdx < 0 ? result.Count - 2 : insertIdx;
                    hasChanged = true;

                    result.InsertRange(insertIdx + 1, new string[]
                        {
                            string.Empty,
                            "  <PropertyGroup>",
                            $"    <DefineConstants>{defineConstants}</DefineConstants>",
                            "  </PropertyGroup>",
                        });
                }
                if (hasChanged)
                {
                    File.WriteAllLines(filePath, result.ToArray(), Encoding.Default);
                }
            }
        }
        public static void WriteDefinesInProjectFiles(string path, string[] defines)
        {
            var files = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                WriteDefinesInProjectFile(file, defines);
            }
        }
    }
}
//MdEnd
