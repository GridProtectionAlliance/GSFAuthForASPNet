# GSFAuthForASPNet
Example for using GSF Authentication within an ASP.NET MVC App

---

When properly deployed, accessing any page should navigate to [`/Login`](/Views/Login/Index.cshtml) page first, requiring user authentication.

Add link to [`_Layout.cshtml`](/Views/Shared/_Layout.cshtml) (or equivalent) to [`/Logout`](/Views/Login/Logout.cshtml) page for user logout action (included example layout does this already).

See critical sections in root [`Web.config`](Web.config) for GSF ADO database connection plus key `runtime/assemblBinding` section to load proper file versions and `system.webServer/handlers` that access all needed embedded resources from `GSF.Web`.

See [`About.cshtml`](/Views/Home/About.cshtml) view for examples on checking user authentication state and roles.

Both the [`Views/Login/*`](Views/Login/) folder (and files) and [`Controllers/LoginController.cs`](/Controllers/LoginController.cs) need to be copied to the destination ASP.NET application.

Should only need NuGet reference to [`GSF.Web`](https://www.nuget.org/packages/GSF.Web/).

## Deployment Notes

### >> Make sure `Microsoft.Owin.Host.SystemWeb` Package is Referenced

The `Microsoft.Owin.Host.SystemWeb` package must be referenced from MVC project to ensure Owin starts up correctly.
If the reference is missing, try this from NuGet console:
```
update-package Microsoft.Owin.Host.SystemWeb -reinstall
```

### >> Force Login Screen to Display

In order for IIS hosted screens to properly go to Login screen first, validate the following:

1) Make sure IIS setting for "Anonymous Authentication" is `Enabled`. Check project properties.
2) Make sure `AuthFailureRedirectResourceExpression` auth option is set to `"^/$|^/.+$"`
3) It may be necessary to add the following the view controller:
```c#

        public ActionResult Index()
        {
            if (!(User.Identity?.IsAuthenticated ?? false))
                return RedirectToAction("Index", "Login");
            
            return View();
        }                
```