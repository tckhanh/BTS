using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Models;
using System.Web.Script.Serialization;
using BTS.Common.ViewModels;
using System.Web.Helpers;

namespace BTS.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        //IProductCategoryService _productCategoryService;
        private ICertificateService _btsCertificateService;

        private IStatisticService _stattisticService;
        private ICommonService _commonService;

        public HomeController(ICertificateService btscertificateService, ICommonService commonService, IStatisticService stattisticService, IErrorService errorService) : base(errorService)
        {
            _commonService = commonService;
            _btsCertificateService = btscertificateService;
            _stattisticService = stattisticService;
        }

        [OutputCache(Duration = 60, Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            int countBTS = 0;
            IEnumerable<Certificate> data = _btsCertificateService.getAll(out countBTS, true).ToList();
            ViewBag.Certificates = Mapper.Map<List<CertificateViewModel>>(data);

            var statisticData = _stattisticService.GetStatisticCertificateByYear();

            //var slideModel = _commonService.GetSlides();
            //var slideView = Mapper.Map<IEnumerable<Slide>, IEnumerable<SlideViewModel>>(slideModel);
            var homeViewModel = new HomeViewModel();
            //homeViewModel.Slides = slideView;

            //var lastestProductModel = _CertificateService.GetLastest(3);

            //var topSaleProductModel = _CertificateService.GetHotProduct(3);
            //var lastestProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lastestProductModel);
            //var topSaleProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(topSaleProductModel);
            //homeViewModel.StatisticCertificateByOperator = _stattisticService.GetStatisticCertificateByOperator();
            //homeViewModel.StatisticCertificateByOperatorCity = _stattisticService.GetStatisticCertificateByOperatorCity();
            return View(ViewBag.Certificates);
        }

        public ActionResult Index_Test()
        {
            return View();
        }

        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult Footer()
        {
            var footerModel = _commonService.GetFooter();
            var footerViewModel = Mapper.Map<Footer, FooterViewModel>(footerModel);
            return PartialView(footerViewModel);
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult LeftSideBar()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult RightSideBar()
        {
            return PartialView();
        }

        //[ChildActionOnly]
        //[OutputCache(Duration = 3600)]
        //public ActionResult Category()
        //{
        //    var model = _productCategoryService.GetAll();
        //    var listProductCategoryViewModel = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);
        //    return PartialView(listProductCategoryViewModel);
        //}

        public ActionResult GetCertificateSumary()
        {
            List<BTS.Common.ViewModels.StatisticCertificate> dataSumary = _stattisticService.GetStatisticCertificateByYear().ToList();

            return Json(dataSumary, JsonRequestBehavior.AllowGet);
        }

        //GET: Ajax List Category

        public JsonResult loadDataTableBE()
        {
            int countItem;
            IEnumerable<Certificate> data = _btsCertificateService.getAll(out countItem, true).ToList();
            IEnumerable<CertificateViewModel> dataViewModel = Mapper.Map<List<CertificateViewModel>>(data);
            if (countItem > 0)
            {
                //var tbcat = from c in dataViewModel select new { c.Id, c.title, c.descriptions, action = "<a href='" + Url.Action("edit", "Category", new { id = c.Id }) + "'>Edit</a> | <a href='javascript:;' onclick='MyStore.Delete(" + c.Id + ")'>Delete</a>" };
                return Json(new { aaData = dataViewModel }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { aaData = "" }, JsonRequestBehavior.AllowGet);
        }
    }
}