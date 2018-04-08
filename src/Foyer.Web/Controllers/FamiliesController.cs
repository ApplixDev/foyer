using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Foyer.Web.Controllers
{
    public class FamiliesController : FoyerControllerBase
    {
        public FamiliesController()
        {

        }

        public ActionResult Index()
        {
            return View();
        }
    }
}