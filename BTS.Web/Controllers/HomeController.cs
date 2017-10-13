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
    public class HomeController : Controller
    {
        //IProductCategoryService _productCategoryService;
        IBTSCertificateService _btsCertificateService;
        IStatisticService _stattisticService;
        ICommonService _commonService;

        public HomeController(IBTSCertificateService btscertificateService,ICommonService commonService, IStatisticService stattisticService)
        {            
            _commonService = commonService;
            _btsCertificateService = btscertificateService;
            _stattisticService = stattisticService;
        }

        [OutputCache(Duration = 60, Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {


            var statisticData = _stattisticService.GetStatisticCertificateByOperator();
            
            //var slideModel = _commonService.GetSlides();
            //var slideView = Mapper.Map<IEnumerable<Slide>, IEnumerable<SlideViewModel>>(slideModel);
            var homeViewModel = new HomeViewModel();
            //homeViewModel.Slides = slideView;

            //var lastestProductModel = _CertificateService.GetLastest(3);
            //var topSaleProductModel = _CertificateService.GetHotProduct(3);
            //var lastestProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lastestProductModel);
            //var topSaleProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(topSaleProductModel);
            homeViewModel.StatisticCertificateByOperator = _stattisticService.GetStatisticCertificateByOperator();
            homeViewModel.StatisticCertificateByOperatorCity = _stattisticService.GetStatisticCertificateByOperatorCity();
            return View(homeViewModel);            
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


        public JsonResult GetCertificateSumary()
        {
            var homeViewModel = new HomeViewModel();
            homeViewModel.StatisticCertificateByOperator = _stattisticService.GetStatisticCertificateByOperator();
            if (homeViewModel!= null)
            {
                return Json(new
                {
                    data = homeViewModel.StatisticCertificateByOperator,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new
            {
                status = false
            });
        }

    }
}