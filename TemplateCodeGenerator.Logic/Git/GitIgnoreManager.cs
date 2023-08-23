//@BaseCode
//MdStart
namespace TemplateCodeGenerator.Logic.Git
{
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Text;
    public partial class GitIgnoreManager
    {
        #region Properties
        private static string GitIgnoreFile => ".gitignore";
        private static string[] SearchPatterns => StaticLiterals.SourceFileExtensions.Split('|');

        private static string BeginGitIgnoreBlock => "#QuickTemplateStart";
        private static string EndGitIgnoreBlock => "#QuickTemplateEnd";
        #endregion Properties

        public static void Run(string path)
        {
            var gitIgnoreResult = new List<string>();

            Console.WriteLine("Remove all generated files from git...");
            Parallel.ForEach(SearchPatterns, searchPattern =>
            {
                var sourceFiles = GetLabeledFiles(path, searchPattern, new string[] { StaticLiterals.GeneratedCodeLabel });
                var gitIngoredFiles = GetGitIgnoredEntries(sourceFiles, path);

                lock (gitIgnoreResult)
                {
                    gitIgnoreResult.AddRange(gitIngoredFiles);
                }
            });
            WriteGitIgnoreFiles(gitIgnoreResult, path);
        }
        public static void DeleteIgnoreEntries(string path)
        {
            var files = Array.Empty<string>();

            Console.WriteLine("Delete all generated files ignored from git...");
            WriteGitIgnoreFiles(files, path);
        }
        private static IEnumerable<string> GetLabeledFiles(string path, string searchPattern, string[] labels)
        {
            var result = new ConcurrentBag<string>();
            var files = Directory.EnumerateFiles(path, searchPattern, SearchOption.AllDirectories);

            Parallel.ForEach(files, file =>
            {
                using var streamReader = new StreamReader(file, Encoding.Default);
                var line = streamReader.ReadLine();

                if (!string.IsNullOrWhiteSpace(line))
                {
                    if (labels.Any(l => line.Contains(l)))
                    {
                        result.Add(file);
                    }
                }
            });
            return result;
        }
        private static IEnumerable<string> GetGitIgnoredEntries(IEnumerable<string> files, string path)
        {
            var result = new List<string>();

            foreach (var file in files)
            {
                var gitIgnoreFile = file.Replace(path, "");

                if (!string.IsNullOrWhiteSpace(gitIgnoreFile))
                {
                    result.Add(gitIgnoreFile.Replace(@"\", "/"));
                }
            }
            return result;
        }
        private static void WriteGitIgnoreFiles(IEnumerable<string> files, string path)
        {
            var gitIgnoreFilePath = Path.Combine(path, GitIgnoreFile);

            if (File.Exists(gitIgnoreFilePath))
            {
                var lines = File.ReadAllLines(gitIgnoreFilePath);
                var start = 0;
                var end = 0;

                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];

                    if (line == BeginGitIgnoreBlock)
                    {
                        start = i;
                    }

                    if (start > 0 && line == EndGitIgnoreBlock)
                    {
                        end = i;
                    }
                }

                var result = new List<string>();

                if (end > start && start > 0)
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (i < start || i > end)
                        {
                            result.Add(lines[i]);
                        }
                        else
                        {
                            i = end;
                            result.Add(BeginGitIgnoreBlock);
                            result.AddRange(files);
                            result.Add(EndGitIgnoreBlock);
                        }
                    }
                }
                else
                {
                    result.AddRange(lines);
                    result.Add(string.Empty);
                    result.Add(BeginGitIgnoreBlock);
                    result.AddRange(files);
                    result.Add(EndGitIgnoreBlock);
                }

                File.WriteAllLines(gitIgnoreFilePath, result);

                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    var processInfo = new ProcessStartInfo();
                    var anyCommand = "git rm -r --cached .";

                    processInfo.WorkingDirectory = path;
                    processInfo.UseShellExecute = true;
                    processInfo.FileName = @"C:\Windows\System32\cmd.exe";
                    processInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    processInfo.Arguments = anyCommand;
                    Process.Start(processInfo);
                }
            }
        }
    }
}
//MdEnd
