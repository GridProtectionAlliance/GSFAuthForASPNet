using System.Threading;
using System.Security.Principal;
using System.Web.Mvc;
using GSF.Identity;
using GSF.Security;

namespace AuthTest
{
    public static class ViewExtensions
    {
        public static string UserID(this WebViewPage page)
        {
            IIdentity identity = page.User?.Identity ?? Thread.CurrentPrincipal.Identity;
            return identity == null ? UserInfo.CurrentUserID : identity.Name;
        }

        public static bool UserIsAuthenticated(this WebViewPage page)
        {
            return page.User?.Identity?.IsAuthenticated ?? false;
        }

        public static bool UserIsAdmin(this WebViewPage page)
        {
            if (!(page.User is SecurityPrincipal securityPrincipal))
                return false;

            return securityPrincipal.IsInRole("Administrator");
        }

        public static bool UserIsAdminOrEditor(this WebViewPage page)
        {
            if (!(page.User is SecurityPrincipal securityPrincipal))
                return false;

            return securityPrincipal.IsInRole("Administrator,Editor");
        }
    }
}