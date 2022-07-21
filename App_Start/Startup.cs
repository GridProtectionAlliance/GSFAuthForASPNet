using GSF.Diagnostics;
using GSF.IO;
using GSF.Web.Security;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Resources = GSF.Web.Shared.Resources;
using AuthenticationOptions = GSF.Web.Security.AuthenticationOptions;

using static AuthTest.Common;

[assembly: OwinStartup(typeof(AuthTest.Startup))]

namespace AuthTest
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Enable GSF role-based security authentication
            app.UseAuthentication(s_authenticationOptions);

            OwinLoaded = true;
        }
        
        private static readonly AuthenticationOptions s_authenticationOptions;
        private static Dictionary<string, Assembly> s_assemblyCache;
        private static bool s_addedResolver;

        static Startup()
        {
            // Steps to properly load desired version of AjaxMin (prevents version conflict between GSF and WebGrease):
            //   1) Reference new GSF version of AjaxMin via NuGet
            //   2) Set AjaxMin reference property "Copy Local" to false
            //   3) Add AjaxMin to project as an embedded resource
            //   4) Load AjaxMin into AppDomain from embedded resource
            LoadAssemblyFromResource("AjaxMin");
            SetupTempPath();

            s_authenticationOptions = new AuthenticationOptions
            {
                LoginPage = "/Login",
                LoginHeader = $"<h3><img src=\"{Resources.Root}/Shared/Images/gpa-smalllock.png\"/> {ApplicationName}</h3>",
                AnonymousResourceExpression = AnonymousResourceExpression,
                AuthFailureRedirectResourceExpression = @"^/$|^/.+$"
            };

            AuthenticationOptions = CreateInstance<ReadonlyAuthenticationOptions>(s_authenticationOptions);
        }

        public static bool OwinLoaded { get; private set; }

        public static ReadonlyAuthenticationOptions AuthenticationOptions { get; }
        
        private static T CreateInstance<T>(params object[] args)
        {
            Type type = typeof(T);
            object instance = type.Assembly.CreateInstance(type.FullName!, false, BindingFlags.Instance | BindingFlags.NonPublic, null, args, null, null);
            return (T)instance;
        }

        private static void LoadAssemblyFromResource(string assemblyName)
        {
            if (!s_addedResolver)
            {
                // Hook into assembly resolve event for current domain so it can load assembly from embedded resource
                AppDomain.CurrentDomain.AssemblyResolve += ResolveAssemblyFromResource;
                s_addedResolver = true;
            }

            // Load the assembly (this will invoke event that will resolve assembly from resource)
            AppDomain.CurrentDomain.Load(assemblyName);
        }

        private static Assembly ResolveAssemblyFromResource(object sender, ResolveEventArgs e)
        {
            string shortName = e.Name.Split(',')[0];

            if ((s_assemblyCache ??= new Dictionary<string, Assembly>()).TryGetValue(shortName, out Assembly resourceAssembly) && resourceAssembly is not null)
                return resourceAssembly;

            Assembly entryAssembly = typeof(Startup).Assembly;

            // Loops through all of the resources in the executing assembly
            foreach (string name in entryAssembly.GetManifestResourceNames())
            {
                // See if the embedded resource name matches the assembly trying to be loaded
                if (string.Compare(FilePath.GetFileNameWithoutExtension(name), $"{nameof(AuthTest)}.{shortName}", StringComparison.OrdinalIgnoreCase) != 0)
                    continue;

                // If so, load embedded resource assembly into a binary buffer
                Stream resourceStream = entryAssembly.GetManifestResourceStream(name);

                if (resourceStream is null)
                    continue;

                byte[] buffer = new byte[resourceStream.Length];
                resourceStream.Read(buffer, 0, (int)resourceStream.Length);
                resourceStream.Close();

                // Load assembly from binary buffer
                resourceAssembly = Assembly.Load(buffer);

                // Add assembly to the cache
                s_assemblyCache.Add(shortName, resourceAssembly);
                break;
            }

            return resourceAssembly;
        }

        private static void SetupTempPath()
        {
            const string DynamicAssembliesFolderName = "DynamicAssemblies";
            string assemblyDirectory = null;

            try
            {
                // Setup custom temp folder so that dynamically compiled razor assemblies can be more easily managed
                assemblyDirectory = FilePath.GetAbsolutePath(DynamicAssembliesFolderName);

                if (!Directory.Exists(assemblyDirectory))
                    Directory.CreateDirectory(assemblyDirectory);

                Environment.SetEnvironmentVariable("TEMP", assemblyDirectory);
                Environment.SetEnvironmentVariable("TMP", assemblyDirectory);
            }
            catch (Exception ex)
            {
                // This is not catastrophic
                Logger.SwallowException(ex, $"Failed to assign temp folder location to: {assemblyDirectory}");
            }
        }
    }
}