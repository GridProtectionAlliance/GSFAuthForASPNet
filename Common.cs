﻿using GSF.Configuration;
using GSF.Web.Security;

namespace AuthTest
{
    public static class Common
    {
        private static string s_applicationName;
        private static string s_anonymousResourceExpression;

        public static string ApplicationName => s_applicationName ??= GetApplicationName();

        public static string AnonymousResourceExpression => s_anonymousResourceExpression ??= GetAnonymousResourceExpression();

        private static string GetApplicationName() =>
            GetSettingValue("SecurityProvider", "ApplicationName", "GSF Authentication");

        private static string GetAnonymousResourceExpression() =>
            GetSettingValue("SystemSettings", "AnonymousResourceExpression", AuthenticationOptions.DefaultAnonymousResourceExpression);

        private static string GetSettingValue(string section, string keyName, string defaultValue)
        {
            try
            {
                ConfigurationFile config = ConfigurationFile.Current;
                CategorizedSettingsElementCollection settings = config.Settings[section];
                return settings[keyName].ValueAs(defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}