namespace CleanDirectories.ConApp
{
    internal partial class Program
    {
        static Program()
        {
            ClassConstructing();
            SourcePath = GetCurrentSolutionPath();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        private static readonly bool canBusyPrint = true;
        private static bool runBusyProgress = false;
        internal static string SourcePath { get; set; }
        static void Main(string[] args)
        {
            var path = string.Empty;
            var dropFolders = new string[] { "\\bin", "\\obj", "\\target" };

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("********************************************");
            Console.WriteLine("*          Clean directories...            *");
            Console.WriteLine("********************************************");
            Console.WriteLine();
            Console.WriteLine($"Drop folders: {string.Join(", ", dropFolders)}");

            if (args.Length == 1 && Directory.Exists(args[0]))
            {
                path = args[0];
            }
            else
            {
                path = SourcePath;
            }

            if (string.IsNullOrEmpty(path) == false)
            {
                Console.WriteLine($"Path: {SourcePath}");
            }

            while (string.IsNullOrEmpty(path))
            {
                Console.Clear();
                Console.WriteLine("********************************************");
                Console.WriteLine("*          Clean directories...            *");
                Console.WriteLine("********************************************");
                Console.WriteLine();
                Console.WriteLine($"Drop folders: {string.Join(", ", dropFolders)}");
                Console.WriteLine();
                Console.Write("Path: ");

                var input = Console.ReadLine();

                if (Directory.Exists(input))
                {
                    path = input;
                }
            }

            PrintBusyProgress();
            CleanDirectories(path, dropFolders);
            runBusyProgress = false;
        }
        private static void PrintBusyProgress()
        {
            var sign = "\\";

            Console.WriteLine();
            runBusyProgress = true;
            Task.Factory.StartNew(async () =>
            {
                while (runBusyProgress)
                {
                    if (canBusyPrint)
                    {
                        if (Console.CursorLeft > 0)
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);

                        Console.Write($".{sign}");
                        sign = sign == "\\" ? "/" : "\\";
                    }
                    await Task.Delay(250).ConfigureAwait(false);
                }
            });
        }
        private static void CleanDirectories(string path, params string[] dropFolders)
        {
            static int CleanDirectories(DirectoryInfo dirInfo, params string[] dropFolders)
            {
                int result = 0;

                try
                {
                    result = dirInfo.GetFiles().Length;
                    foreach (var item in dirInfo.GetDirectories())
                    {
                        if (item.FullName.Contains($"{nameof(CleanDirectories)}.ConApp") == false)
                        {
                            int fileCount = CleanDirectories(item, dropFolders);

                            try
                            {
                                if (fileCount == 0)
                                {
                                    item.Delete();
                                }
                                else if (dropFolders.FirstOrDefault(df => item.FullName.EndsWith(df)) != null)
                                {
                                    fileCount = 0;
                                    item.Delete(true);
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error in {System.Reflection.MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
                            }
                            result += fileCount;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in {System.Reflection.MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
                }
                return result;
            }

            CleanDirectories(new DirectoryInfo(path), dropFolders);
        }
        internal static string GetCurrentSolutionPath()
        {
            int endPos = AppContext.BaseDirectory
                                   .IndexOf($"{nameof(CleanDirectories)}", StringComparison.CurrentCultureIgnoreCase);
            var result = AppContext.BaseDirectory[..endPos];

            while (result.EndsWith(Path.DirectorySeparatorChar))
            {
                result = result[0..^1];
            }
            return result;
        }
    }
}