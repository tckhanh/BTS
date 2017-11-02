using BTS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Controllers
{
    public class AboutController : BaseController
    {
        public AboutController(IErrorService errorService) : base(errorService)
        {
        }
        // GET: About
        public ActionResult Index()
        {
            return View();
        }
    }
}