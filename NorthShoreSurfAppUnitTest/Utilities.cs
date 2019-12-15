using NorthShoreSurfApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NorthShoreSurfAppUnitTest
{
    public static class Utilities
    {
        /// <summary>
        /// Get project folder
        /// </summary>
        /// <returns>Path to project folder</returns>
        public static string GetProjectFolder()
        {
            string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            string[] pathItems = startupPath.Split(Path.DirectorySeparatorChar)
                                            .Where(x => x != string.Empty)
                                            .ToArray();
            Assembly assy = typeof(App).Assembly;
            do
            {
                var tempPath = string.Join(Path.DirectorySeparatorChar.ToString(), pathItems);

                var files = Directory.GetFiles(tempPath, "*.csproj")
                                     .Select(Path.GetFileName)
                                     .ToArray();

                if (files.Count() > 0)
                    return tempPath;

                pathItems = pathItems.Take(pathItems.Length - 1).ToArray();
            } while (pathItems.Length >= 1);
            return string.Empty;
        }
    }
}
