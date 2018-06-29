using AutoMapper;
using BTS.Common;
using BTS.Common.ViewModels;
using BTS.Data;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Infrastructure.Extensions;
using BTS.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Helpers;
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
        private ICertificateService _certificateService;

        public HomeController(ICertificateService btscertificateService, ICommonService commonService, ICertificateService certificateService, IStatisticService stattisticService, IErrorService errorService) : base(errorService)
        {
            _commonService = commonService;
            _certificateService = certificateService;
            _btsCertificateService = btscertificateService;
            _stattisticService = stattisticService;
        }

        [OutputCache(Duration = 60, Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            IEnumerable<Operator> operatorList = _stattisticService.GetOperator();
            IEnumerable<City> cityList = _stattisticService.GetCity();

            ViewBag.operatorList = operatorList;
            ViewBag.cityList = cityList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string selectOperatorId, string selectCityId)
        {
            try
            {
                IEnumerable<CertificateStatiticsViewModel> ByOperator = _stattisticService.GetCertificateStatisticByOperator();
                IEnumerable<CertificateStatiticsViewModel> ByOperatorCity = _stattisticService.GetCertificateStatisticByOperatorCity();
                int Number = 0;
                IEnumerable<Certificate> data = _certificateService.getAll(out Number, true);
                IEnumerable<Certificate> data1 = _certificateService.getAll(out Number, true);

                var pivotArray = data.ToPivotArray(item => item.OperatorID, item => item.CityID, items => items.Any() ? items.Count() : 0);

                String json = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());

                return Json(new
                {
                    Status = CommonConstants.Status_Success,
                    Message = "Import Certificate Finished !",
                    DataByOperator = ByOperator,
                    DataByOperatorCity = ByOperatorCity
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                // Base Controller đã ghi Log Error rồi
                return Json(new { Status = CommonConstants.Status_Error, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
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
            List<BTS.Common.ViewModels.CertificateStatiticsViewModel> dataSumary = _stattisticService.GetCertificateStatisticByYear().ToList();

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

        //public ActionResult CharterColumn(int nam)
        //{
        //    var _context = new BTSDbContext();
        //    ArrayList xValue = new ArrayList();
        //    ArrayList yValue = new ArrayList();

        //    var Results = (from c in _context.tblMVCCharts where c.Growth_Year > nam select c);
        //    Results.ToList().ForEach(rs => xValue.Add(rs.Growth_Year));
        //    Results.ToList().ForEach(rs => yValue.Add(rs.Growth_Value));

        //    new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
        //    .AddTitle("Chart for Growth.[Column Chart]")
        //    .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue)
        //    .Write("bmp");
        //    return null;
        //}

        //public ActionResult ChartBar()
        //{
        //    var _context = new BTSDbContext();
        //    ArrayList xValue = new ArrayList();
        //    ArrayList yValue = new ArrayList();
        //    var resutls = (from d in _context.Applicants select d);
        //    resutls.ToList().ForEach(x => xValue.Add(x.Growth_Year));
        //    resutls.ToList().ForEach(x => yValue.Add(x.Growth_Value));
        //    new Chart(width: 600, height: 400, theme: ChartTheme.Vanilla)
        //        .AddTitle("Chart for Growth.[Bar Chart]")
        //        .AddSeries("Default", chartType: "Bar", xValue: xValue, yValues: yValue)
        //        .Write("bmp");
        //    return null;
        //}

        //public ActionResult ChartPie()
        //{
        //    var _context = new MVCChartsEntities();
        //    ArrayList xValue = new ArrayList();
        //    ArrayList yValue = new ArrayList();
        //    var resutls = (from d in _context.tblMVCCharts select d);
        //    resutls.ToList().ForEach(x => xValue.Add(x.Growth_Year));
        //    resutls.ToList().ForEach(x => yValue.Add(x.Growth_Value));
        //    new Chart(width: 600, height: 400, theme: ChartTheme.Vanilla)
        //        .AddTitle("Chart for Growth.[Pie Chart]")
        //        .AddLegend("Summary")
        //        .AddSeries("Default", chartType: "Pie", xValue: xValue, yValues: yValue)
        //        .Write("bmp");
        //    return null;
        //}

        //public ActionResult DoughnutChart()
        //{
        //    var _context = new BTSDbContext();
        //    ArrayList xValue = new ArrayList();
        //    ArrayList yValue = new ArrayList();
        //    var resutls = (from d in _context.Applicants select d);
        //    resutls.ToList().ForEach(x => xValue.Add(x.Growth_Year));
        //    resutls.ToList().ForEach(x => yValue.Add(x.Growth_Value));
        //    new Chart(width: 600, height: 400, theme: ChartTheme.Vanilla)
        //        .AddTitle("Chart for Growth.[Doughnut Chart]")
        //        .AddLegend("Summary")
        //        .AddSeries("Default", chartType: "Doughnut", xValue: xValue, yValues: yValue)
        //        .Write("bmp");
        //    return null;
        //}

        //public ActionResult PyramidChart()
        //{
        //    var _context = new MVCChartsEntities();
        //    ArrayList xValue = new ArrayList();
        //    ArrayList yValue = new ArrayList();
        //    var resutls = (from d in _context.tblMVCCharts select d);
        //    resutls.ToList().ForEach(x => xValue.Add(x.Growth_Year));
        //    resutls.ToList().ForEach(x => yValue.Add(x.Growth_Value));
        //    new Chart(width: 600, height: 400, theme: ChartTheme.Vanilla)
        //        .AddTitle("Chart for Growth.[Pyramid Chart]")
        //        .AddLegend("Summary")
        //        .AddSeries("Default", chartType: "Pyramid", xValue: xValue, yValues: yValue)
        //        .Write("bmp");
        //    return null;
        //}
    }
}