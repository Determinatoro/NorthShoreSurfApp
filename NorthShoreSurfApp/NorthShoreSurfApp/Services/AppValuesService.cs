using System;
using System.Collections.Generic;
using System.Text;

namespace NorthShoreSurfApp
{
    public class AppValuesService
    {
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
    }
}
