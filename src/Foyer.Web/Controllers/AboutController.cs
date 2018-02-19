using System.Web.Mvc;

namespace Foyer.Web.Controllers
{
    public class AboutController : FoyerControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}