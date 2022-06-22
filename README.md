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

### >> Conflict with `AjaxMin.dll'

To get past the following issue with `AjaxMin.dll`:

error: (252, 18) The type 'Microsoft.Ajax.Utilities.Minifier' exists in both 'c:\\Users\\user\\AppData\\Local\\Temp\\Temporary ASP.NET Files\\vs\\3fcb85ea\\e51caa15\\assembly\\dl3\\5717bf02\\00697cc7_313bd001\\AjaxMin.DLL' and 'c:\\Users\\user\\AppData\\Local\\Temp\\Temporary ASP.NET Files\\vs\\3fcb85ea\\e51caa15\\assembly\\dl3\\f395d8c6\\001bc110_4318cf01\\WebGrease.DLL'

Set the `WebGrease` reference in the project to `Copy Local` = `False` in reference properties. Note that you might need to "clean" project and rebuild for this to take effect.

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