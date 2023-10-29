//@CodeCopy
//MdStart
using System;

namespace TemplateTools.ConApp
{
    /// <summary>
    /// Represents an internal class used for cleaning up directories.
    /// </summary>
    internal class CleanupApp
    {
        public static string[] DropFolders = new string[] { "\\bin", "\\obj", "\\target" };
        /// <summary>
        /// Clear the console, set the foreground color, display a header, and then call the CleanDirectories method.
        /// </summary>
        /// <param name="path">The path for the CleanDirectories method.</param>
        public static void RunApp(string path)
        {
            Console.Clear();
            Console.ForegroundColor = Program.ForegroundColor;
            Console.WriteLine("  Cleanup directories  ");
            Console.WriteLine("=======================");
            Console.WriteLine();
            Console.WriteLine($"Drop folders: {string.Join(", ", DropFolders)}");
            Console.WriteLine();
            Console.Write($"Path: {path}");
            Console.WriteLine();
            Console.WriteLine();
            
            Program.PrintBusyProgress();
            CleanDirectories(path, DropFolders);
        }
        /// <summary>
        /// Recursively deletes files and empty directories from the specified path,
        /// excluding directories specified in the dropFolders parameter.
        /// </summary>
        /// <param name="path">The root path from where the cleaning operation begins.</param>
        /// <param name="dropFolders">The array of folder names to exclude from deletion.</param>
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
                        int fileCount = CleanDirectories(item, dropFolders);
                        
                        try
                        {
                            if (fileCount == 0)
                            {
                                item.Delete();
                            }
                            else if ((dropFolders.FirstOrDefault(df => item.FullName.EndsWith(df))) != null)
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
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in {System.Reflection.MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
                }
                return result;
            }
            
            CleanDirectories(new DirectoryInfo(path), dropFolders);
        }
    }
}
//MdEnd
