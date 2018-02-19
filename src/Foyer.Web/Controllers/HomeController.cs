using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

namespace Foyer.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : FoyerControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}