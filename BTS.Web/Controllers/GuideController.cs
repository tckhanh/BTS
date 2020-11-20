using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Controllers
{
    [Authorize()]
    public class GuideController : Controller
    {
        // GET: Guide
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View();
        }
    }
}