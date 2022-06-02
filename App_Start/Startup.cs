using GSF.Web.Shared;
using GSF.Web.Security;
using Microsoft.Owin;
using Owin;
using System.Reflection;
using System;

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
            AuthenticationOptions options = new AuthenticationOptions
            {
                LoginPage = "/Login",
                LoginHeader = $"<h3><img src=\"{Resources.Root}/Shared/Images/gpa-smalllock.png\"/> GPA ASP.NET Authentication</h3>",
                AnonymousResourceExpression = "^/Login/|^/@|^/Scripts/|^/Content/|^/Images/|^/fonts/|^/api/Feedback/|^/favicon.ico$"
            };

            s_authenticationOptions = options;
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