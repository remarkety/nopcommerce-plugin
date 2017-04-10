using System;
using System.Globalization;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Infrastructure
{
    public static class StringHelper
    {
        public static string RemarketyApiKey => "RemarketyApiKey";
        public static string RemarketyStoreIdSettingKey => "RemarketyWebApi.WebTracking.StoreId";

        public static DateTime? ParseDateTime(string dateTimeString)
        {
            if (string.IsNullOrEmpty(dateTimeString))
            {
                return null;
            }

            DateTime result;

            if (!DateTime.TryParseExact(dateTimeString, "yyyy-MM-ddTHH:mm:ss+0000", CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeLocal, out result))
            {
                return null;
            }

            return result;
        }
    }
}