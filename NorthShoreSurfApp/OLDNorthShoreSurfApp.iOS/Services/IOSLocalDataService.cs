using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using NorthShoreSurfApp.iOS.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(IOSLocalDataService))]
namespace NorthShoreSurfApp.iOS.Services
{
    public class IOSLocalDataService : ILocalDataService
    {
        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        private string RootFolderPath { get; set; }
        public string DataFilesFolderPath { get; set; }
        private NSUserDefaults UserDefaults { get; set; }

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public IOSLocalDataService()
        {
            RootFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        #endregion

        /*****************************************************************/
        // INTERFACE METHODS
        /*****************************************************************/
        #region Interface methods

        /// <summary>
        /// Initialize files by moving them from embedded resource into the root of the apps personal folder
        /// </summary>
        /// <param name="force">If true no check if the files already exist they will just be overwritten</param>
        public void InitializeFiles(bool force = false)
        {
            DataFilesFolderPath = Path.Combine(RootFolderPath, "..", "Library", "DataFiles");

            if (!Directory.Exists(DataFilesFolderPath))
            {
                Directory.CreateDirectory(DataFilesFolderPath);
            }

            foreach (string fileName in LocalDataFiles.AllFiles)
            {
                string path = Path.Combine(DataFilesFolderPath, fileName);

                // This is where we copy in the pre-created database
                if (force || !File.Exists(path))
                {
                    var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                    var extension = Path.GetExtension(fileName).Replace(".", "");
                    var resource = NSBundle.MainBundle.PathForResource(nameWithoutExtension, extension);
                    if (force)
                        File.Delete(path);
                    File.Copy(resource, path);
                }
            }
        }

        /// <summary>
        /// Read contents of a file
        /// </summary>
        /// <param name="fileName">Name of file</param>
        /// <returns></returns>
        public string ReadFile(string fileName)
        {
            var path = Path.Combine(DataFilesFolderPath, fileName);
            return File.ReadAllText(path);
        }

        /// <summary>
        /// Write contents to a file
        /// </summary>
        /// <param name="fileName">Name of file</param>
        /// <param name="content">Contents to be saved in a file</param>
        /// <param name="append">If true append at the end of the file else. If false overwrite existing content</param>
        public void WriteToFile(string fileName, string content, bool append)
        {
            var filePath = Path.Combine(DataFilesFolderPath, fileName);
            if (append)
                File.AppendAllText(filePath, content);
            else
                File.WriteAllText(filePath, content);
        }

        /// <summary>
        /// Save value on phone
        /// </summary>
        /// <param name="key">Key for value</param>
        /// <param name="value">Value to be saved</param>
        public void SaveValue(string key, string value)
        {
            UserDefaults = NSUserDefaults.StandardUserDefaults;
            UserDefaults.SetString(value, key);
        }

        /// <summary>
        /// Get value from key
        /// </summary>
        /// <param name="key">Key for a value</param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            UserDefaults = NSUserDefaults.StandardUserDefaults;
            return UserDefaults.StringForKey(key);
        }

        #endregion
    }
}