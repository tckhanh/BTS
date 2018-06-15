using AutoMapper;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BTS.Web.Controllers
{
    //[Authorize]
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

        [HttpPost]
        public JsonResult loadDataTableBE()
        {
            int countItem;

            var ID = Request.Form.GetValues("ID").FirstOrDefault();
            var OperatorID = Request.Form.GetValues("OperatorID").FirstOrDefault();

            IEnumerable<Certificate> Items = _btsCertificateService.getAll(out countItem, true).ToList();

            // searching ...

            if (!(string.IsNullOrEmpty(ID)))
            {
                Items = Items.Where(x => x.ID.Contains(ID));
            }

            if (!(string.IsNullOrEmpty(OperatorID)))
            {
                Items = Items.Where(x => x.OperatorID.Contains(OperatorID));
            }

            var recordsFiltered = Items.Count();

            IEnumerable<CertificateViewModel> dataViewModel = Mapper.Map<List<CertificateViewModel>>(Items);
            if (countItem > 0)
            {
                //var tbcat = from c in dataViewModel select new { c.Id, c.title, c.descriptions, action = "<a href='" + Url.Action("edit", "Category", new { id = c.Id }) + "'>Edit</a> | <a href='javascript:;' onclick='MyStore.Delete(" + c.Id + ")'>Delete</a>" };
                return Json(new { aaData = dataViewModel }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { aaData = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult loadDataServerSide()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            // Get sort colunn name
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            IEnumerable<Certificate> Items = _btsCertificateService.getAll(out totalRecords, true);

            // Sorting ....
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                // Can phai add thu vien System.Linq.Dynamic
                Items = Items.OrderBy(sortColumn + " " + sortColumnDir);
            }

            // Paging ....
            IEnumerable<Certificate> data = Items.Skip(skip).Take(pageSize).ToList();

            IEnumerable<CertificateViewModel> dataViewModel = Mapper.Map<List<CertificateViewModel>>(data);
            if (totalRecords > 0)
            {
                //var tbcat = from c in dataViewModel select new { c.Id, c.title, c.descriptions, action = "<a href='" + Url.Action("edit", "Category", new { id = c.Id }) + "'>Edit</a> | <a href='javascript:;' onclick='MyStore.Delete(" + c.Id + ")'>Delete</a>" };
                return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = dataViewModel }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult loadCustomDataServerSide()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            var ID = Request.Form.GetValues("ID").FirstOrDefault();
            var OperatorID = Request.Form.GetValues("OperatorID").FirstOrDefault();

            // Get sort colunn name
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal, recordsFiltered = 0;

            IEnumerable<Certificate> Items = Items = _btsCertificateService.getAll(out recordsTotal, true);

            // searching ...

            if (!(string.IsNullOrEmpty(ID)))
            {
                Items = Items.Where(x => x.ID.Contains(ID));
            }

            if (!(string.IsNullOrEmpty(OperatorID)))
            {
                Items = Items.Where(x => x.OperatorID.Contains(OperatorID));
            }

            recordsFiltered = Items.Count();

            // Sorting ...
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                // Can phai add thu vien System.Linq.Dynamic
                Items = Items.OrderBy(sortColumn + " " + sortColumnDir);
            }

            // Paging ....
            IEnumerable<Certificate> data = Items.Skip(skip).Take(pageSize).ToList();

            IEnumerable<CertificateViewModel> dataViewModel = Mapper.Map<List<CertificateViewModel>>(data);
            if (recordsTotal > 0)
            {
                //var tbcat = from c in dataViewModel select new { c.Id, c.title, c.descriptions, action = "<a href='" + Url.Action("edit", "Category", new { id = c.Id }) + "'>Edit</a> | <a href='javascript:;' onclick='MyStore.Delete(" + c.Id + ")'>Delete</a>" };
                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsFiltered,
                    recordsTotal = recordsTotal,
                    data = dataViewModel
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }
    }
}