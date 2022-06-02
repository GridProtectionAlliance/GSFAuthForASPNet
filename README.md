# GSFAuthForASPNet
Example for using GSF Authentication within an ASP.NET MVC App

---

When properly deployed, accessing any page should navigate to [`/Login`](/Views/Login/Index.cshtml) page first, requiring user authentication.

Add link to `_Layout.cshtml` (or equivalent) to [`/Logout`](/Views/Login/Logout.cshtml) page for logout action.

Root [`Web.config`](Web.config) is critical for GSF ADO database connection plus key `runtime/assemblBinding` section to load proper file versions and `system.webServer/handlers` that access all needed embedded resources from `GSF.Web`.

Both the [`Views/Login/*`](Views/Login/) folder (and files) and [`Controllers/LoginController.cs`](/Controllers/LoginController.cs) need to be copied to the destination ASP.NET application.

Should only need NuGet reference to [`GSF.Web`](https://www.nuget.org/packages/GSF.Web/).
