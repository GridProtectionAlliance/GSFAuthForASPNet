using System;
using System.Reflection;
using Microsoft.Owin;
using Owin;
using GSF.Configuration;
using GSF.Web.Shared;
using GSF.Web.Security;

[assembly: OwinStartup(typeof(AuthTest.Startup))]

namespace AuthTest
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use<AuthenticationMiddleware>(s_authenticationOptions);
        }

        private static readonly AuthenticationOptions s_authenticationOptions;

        static Startup()
        {
            const string DefaultApplicationName = "GSF Authentication";

            string applicationName = DefaultApplicationName;

            try
            {
                // Attempt to load application name from the Web.config file
                ConfigurationFile config = ConfigurationFile.Current;
                CategorizedSettingsElementCollection settings = config.Settings["SecurityProvider"];
                settings.Add("ApplicationName", applicationName, "Name of the application being secured as defined in the backend security data store.");
                applicationName= settings["ApplicationName"].ValueAs(applicationName);
            }
            catch (Exception)
            {
                applicationName = DefaultApplicationName;
            }
            
            s_authenticationOptions = new AuthenticationOptions
            {
                LoginPage = "/Login",
                LoginHeader = $"<h3><img src=\"{Resources.Root}/Shared/Images/gpa-smalllock.png\"/> {applicationName}</h3>",
                AnonymousResourceExpression = "^/Login/|^/@|^/Scripts/|^/Content/|^/Images/|^/fonts/|^/api/Feedback/|^/favicon.ico$"
            };

            AuthenticationOptions = CreateInstance<ReadonlyAuthenticationOptions>(s_authenticationOptions);
        }

        public static ReadonlyAuthenticationOptions AuthenticationOptions { get; }
        
        private static T CreateInstance<T>(params object[] args)
        {
            Type type = typeof(T);
            object instance = type.Assembly.CreateInstance(type.FullName, false, BindingFlags.Instance | BindingFlags.NonPublic, null, args, null, null);
            return (T)instance;
        }
    }
}