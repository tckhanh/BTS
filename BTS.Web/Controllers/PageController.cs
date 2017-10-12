using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Models;

namespace BTS.Web.Controllers
{
    public class PageController : Controller
    {
        IPageService _pageService;
        public PageController(IPageService pageService)
        {
            this._pageService = pageService;
        }
        // GET: Page
        public ActionResult Index(string alias)
        {
            var page = _pageService.GetByAlias(alias);
            var model = Mapper.Map<Page,PageViewModel>(page);
            return View(model);
        }
    }
}