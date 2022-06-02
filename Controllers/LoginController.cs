using System.Web.Mvc;

namespace AuthTest.Controllers
{
    public class LoginController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [Route("~/AuthTest")]
        [Authorize]
        public ActionResult AuthTest()
        {
            return View();
        }

        [Route("~/Logout")]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            return View();
        }
    }
}