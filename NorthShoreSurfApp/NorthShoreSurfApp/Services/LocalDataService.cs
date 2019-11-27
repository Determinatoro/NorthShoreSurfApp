using System;
using System.Collections.Generic;
using System.Text;

namespace NorthShoreSurfApp
{
    public class LocalDataFiles
    {
        public const string Database = "NSS.db";

        public static readonly string[] AllFiles = { Database };
    }

    public enum LocalDataKeys
    {
        FirebaseAuthVerificationId,
        UserId
    }

    public interface ILocalDataService
    {
        void InitializeFiles(bool force = false);
        string ReadFile(string fileName);
        void WriteToFile(string fileName, string text, bool append);
        void SaveValue(string key, string value);
        string GetValue(string key);
    }
}
