﻿using System;
using System.Globalization;
using System.Reflection;

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

            if (!DateTime.TryParseExact(dateTimeString, "yyyy-MM-ddTHH:mm:ss+0000", CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeLocal, out var result))
            {
                return null;
            }

            return result;
        }

        public static string GetPluginVersion()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            return $"{version.Major}.{version.Minor}.{version.Build}";
        }
    }
}