using System;
using System.Collections.Generic;
using System.Text;

namespace NorthShoreSurfApp
{
    public class AppValuesService
    {
        /// <summary>
        /// Save value on local device
        /// </summary>
        /// <param name="localDataKey">Key for the value</param>
        /// <param name="value">Value to save</param>
        public static void SaveValue(LocalDataKeys localDataKey, string value)
        {
            var key = Enum.GetName(typeof(LocalDataKeys), (int)localDataKey);
            App.LocalDataService.SaveValue(key, value);
        }

        public static void LogOut()
        {
            // Remove user id from local data
            SaveValue(LocalDataKeys.UserId, null);
        }

        /// <summary>
        /// Get ID for the user that is logged in
        /// </summary>
        /// <returns></returns>
        public static int? GetUserId()
        {
            var userId = App.LocalDataService.GetValue(nameof(LocalDataKeys.UserId));
            if (int.TryParse(userId, out int id))
                return id;
            else
                return null;
        }

        public static int? UserId
        {
            get
            {
                var userId = App.LocalDataService.GetValue(nameof(LocalDataKeys.UserId));
                if (int.TryParse(userId, out int id))
                    return id;
                else
                    return null;
            }
        }

        public static bool IsGuest
        {
            get
            {
                var isGuest = App.LocalDataService.GetValue(nameof(LocalDataKeys.IsGuest));
                if (bool.TryParse(isGuest, out bool flag))
                    return flag;
                else
                    return false;
            }
        }
    }
}
