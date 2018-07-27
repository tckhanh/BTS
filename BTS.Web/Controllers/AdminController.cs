using BTS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(IErrorService errorService) : base(errorService)
        {
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}