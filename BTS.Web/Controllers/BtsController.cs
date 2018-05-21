using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Controllers
{
    public class BtsController : Controller
    {
        // GET: Bts
        public ActionResult Index()
        {
            return View();
        }
    }
}