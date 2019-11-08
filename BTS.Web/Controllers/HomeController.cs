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
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BTS.Web.Controllers
{
    [AuthorizeRoles]
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
            return View();
        }

        [HttpPost]
        public ActionResult CerStatByOperator()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<CertStatByOperatorVM> ByOperator = _stattisticService.GetCertStatByOperator();

                DataTable pivotTable = ByOperator.ToDataTable();
                List<String> ColumnNames = new List<string>();
                foreach (DataColumn col in pivotTable.Columns)
                {
                    ColumnNames.Add(col.ColumnName);
                }
                chartData.Add(ColumnNames);

                List<object> seriesNo = new List<object>();

                for (int i = 0; i < ColumnNames.Count; i++)
                {
                    seriesNo = pivotTable.AsEnumerable().Select(r => r.Field<object>(ColumnNames.ElementAt(i))).ToList();

                    chartData.Add(seriesNo);
                }
                return Json(new
                {
                    status = CommonConstants.Status_Success,
                    message = "StatisticByOperator Finished !",
                    chartData = chartData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                // Base Controller đã ghi Log Error rồi
                return Json(new
                {
                    status = CommonConstants.Status_Error,
                    message = e.Message,
                    chartData = chartData
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult BtsStatInProcess()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatBtsVm> BtsInProcess = _stattisticService.GetStatAllBtsInProcess();

                DataTable pivotTable = BtsInProcess.ToDataTable();
                List<String> ColumnNames = new List<string>();
                foreach (DataColumn col in pivotTable.Columns)
                {
                    ColumnNames.Add(col.ColumnName);
                }
                chartData.Add(ColumnNames);

                List<object> seriesNo = new List<object>();

                for (int i = 0; i < ColumnNames.Count; i++)
                {
                    seriesNo = pivotTable.AsEnumerable().Select(r => r.Field<object>(ColumnNames.ElementAt(i))).ToList();

                    chartData.Add(seriesNo);
                }
                return Json(new
                {
                    status = CommonConstants.Status_Success,
                    message = "StatisticBtsInProcess Finished !",
                    chartData = chartData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                // Base Controller đã ghi Log Error rồi
                return Json(new
                {
                    status = CommonConstants.Status_Error,
                    message = e.Message,
                    chartData = chartData
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CertStatByOperatorYear()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<IssuedCertStatByOperatorYearVM> ByOperatorYear = _stattisticService.GetIssuedCertStatByOperatorYear();
                DataTable pivotTable = ByOperatorYear.ToPivotTable(item => item.OperatorID, item => item.Year, items => items.Any() ? items.Sum(item => item.IssuedCertificates) : 0);

                List<String> ColumnNames = new List<string>();
                foreach (DataColumn col in pivotTable.Columns)
                {
                    ColumnNames.Add(col.ColumnName);
                }
                chartData.Add(ColumnNames);

                List<object> seriesNo = new List<object>();

                for (int i = 0; i < ColumnNames.Count; i++)
                {
                    seriesNo = pivotTable.AsEnumerable().Select(r => r.Field<object>(ColumnNames.ElementAt(i))).ToList();

                    chartData.Add(seriesNo);
                }

                return Json(new
                {
                    status = CommonConstants.Status_Success,
                    message = "IssuedStatisticByOperatorYear Finished !",
                    chartData = chartData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                // Base Controller đã ghi Log Error rồi
                return Json(new
                {
                    status = CommonConstants.Status_Error,
                    message = e.Message,
                    chartData = chartData
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CertStatByOperatorCity()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<IssuedCertStatByOperatorCityVM> ByOperatorCity = _stattisticService.GetIssuedCertStatByOperatorCity();
                ByOperatorCity = ByOperatorCity.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID));

                DataTable pivotTable = ByOperatorCity.ToPivotTable(item => item.OperatorID, item => item.CityID, items => items.Any() ? items.Sum(item => item.IssuedCertificates) : 0);

                List<String> ColumnNames = new List<string>();
                foreach (DataColumn col in pivotTable.Columns)
                {
                    ColumnNames.Add(col.ColumnName);
                }
                chartData.Add(ColumnNames);

                List<object> seriesNo = new List<object>();

                for (int i = 0; i < ColumnNames.Count; i++)
                {
                    seriesNo = pivotTable.AsEnumerable().Select(r => r.Field<object>(ColumnNames.ElementAt(i))).ToList();

                    chartData.Add(seriesNo);
                }

                return Json(new
                {
                    status = CommonConstants.Status_Success,
                    message = "IssuedStatisticByOperatorCity Finished !",
                    chartData = chartData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                // Base Controller đã ghi Log Error rồi
                return Json(new
                {
                    status = CommonConstants.Status_Error,
                    message = e.Message,
                    chartData = chartData
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult BtsStatByBand()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<BtsStatByOperatorBandVM> ByBand = _stattisticService.GetBtsStatByOperatorBand();

                DataTable pivotTable = ByBand.ToPivotTable(item => item.OperatorID, item => item.Band, items => items.Any() ? items.Sum(item => item.Btss) : 0);

                //DataTable pivotTable = ByBand.ToDataTable();
                List<String> ColumnNames = new List<string>();
                foreach (DataColumn col in pivotTable.Columns)
                {
                    ColumnNames.Add(col.ColumnName);
                }
                chartData.Add(ColumnNames);

                List<object> seriesNo = new List<object>();

                for (int i = 0; i < ColumnNames.Count; i++)
                {
                    seriesNo = pivotTable.AsEnumerable().Select(r => r.Field<object>(ColumnNames.ElementAt(i))).ToList();

                    chartData.Add(seriesNo);
                }
                return Json(new
                {
                    status = CommonConstants.Status_Success,
                    message = "BtsStatByBand Finished !",
                    chartData = chartData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                // Base Controller đã ghi Log Error rồi
                return Json(new
                {
                    status = CommonConstants.Status_Error,
                    message = e.Message,
                    chartData = chartData
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult BtsStatByManufactory()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<BtsStatByOperatorManufactoryVM> ByManufactory = _stattisticService.GetBtsStatByOperatorManufactory();

                DataTable pivotTable = ByManufactory.ToPivotTable(item => item.OperatorID, item => item.Manufactory, items => items.Any() ? items.Sum(item => item.Btss) : 0);
                List<String> ColumnNames = new List<string>();
                foreach (DataColumn col in pivotTable.Columns)
                {
                    ColumnNames.Add(col.ColumnName);
                }
                chartData.Add(ColumnNames);

                List<object> seriesNo = new List<object>();

                for (int i = 0; i < ColumnNames.Count; i++)
                {
                    seriesNo = pivotTable.AsEnumerable().Select(r => r.Field<object>(ColumnNames.ElementAt(i))).ToList();

                    chartData.Add(seriesNo);
                }
                return Json(new
                {
                    status = CommonConstants.Status_Success,
                    message = "BtsStatByManufactory Finished !",
                    chartData = chartData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                // Base Controller đã ghi Log Error rồi
                return Json(new
                {
                    status = CommonConstants.Status_Error,
                    message = e.Message,
                    chartData = chartData
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult BtsStatByEquipment()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<BtsStatByEquipmentVM> ByEquipment = _stattisticService.GetBtsStatByEquipment();

                DataTable pivotTable = ByEquipment.ToDataTable();
                List<String> ColumnNames = new List<string>();
                foreach (DataColumn col in pivotTable.Columns)
                {
                    ColumnNames.Add(col.ColumnName);
                }
                chartData.Add(ColumnNames);

                List<object> seriesNo = new List<object>();

                for (int i = 0; i < ColumnNames.Count; i++)
                {
                    seriesNo = pivotTable.AsEnumerable().Select(r => r.Field<object>(ColumnNames.ElementAt(i))).ToList();

                    chartData.Add(seriesNo);
                }
                return Json(new
                {
                    status = CommonConstants.Status_Success,
                    message = "BtsStatByEquipment Finished !",
                    chartData = chartData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                // Base Controller đã ghi Log Error rồi
                return Json(new
                {
                    status = CommonConstants.Status_Error,
                    message = e.Message,
                    chartData = chartData
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult BtsStatByBandCity()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<BtsStatByBandCityVM> ByBandCity = _stattisticService.GetBtsStatByBandCity();
                ByBandCity = ByBandCity.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID));

                DataTable pivotTable = ByBandCity.ToPivotTable(item => item.Band, item => item.CityID, items => items.Any() ? items.Sum(item => item.Btss) : 0);

                //DataTable pivotTable = ByBand.ToDataTable();
                List<String> ColumnNames = new List<string>();
                foreach (DataColumn col in pivotTable.Columns)
                {
                    ColumnNames.Add(col.ColumnName);
                }
                chartData.Add(ColumnNames);

                List<object> seriesNo = new List<object>();

                for (int i = 0; i < ColumnNames.Count; i++)
                {
                    seriesNo = pivotTable.AsEnumerable().Select(r => r.Field<object>(ColumnNames.ElementAt(i))).ToList();

                    chartData.Add(seriesNo);
                }
                return Json(new
                {
                    status = CommonConstants.Status_Success,
                    message = "BtsStatByBandCity Finished !",
                    chartData = chartData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                // Base Controller đã ghi Log Error rồi
                return Json(new
                {
                    status = CommonConstants.Status_Error,
                    message = e.Message,
                    chartData = chartData
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult BtsStatByOperatorCity()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<BtsStatByOperatorCityVM> ByOperatorCity = _stattisticService.GetBtsStatByOperatorCity();
                ByOperatorCity = ByOperatorCity.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID));

                DataTable pivotTable = ByOperatorCity.ToPivotTable(item => item.OperatorID, item => item.CityID, items => items.Any() ? items.Sum(item => item.Btss) : 0);

                //DataTable pivotTable = ByBand.ToDataTable();
                List<String> ColumnNames = new List<string>();
                foreach (DataColumn col in pivotTable.Columns)
                {
                    ColumnNames.Add(col.ColumnName);
                }
                chartData.Add(ColumnNames);

                List<object> seriesNo = new List<object>();

                for (int i = 0; i < ColumnNames.Count; i++)
                {
                    seriesNo = pivotTable.AsEnumerable().Select(r => r.Field<object>(ColumnNames.ElementAt(i))).ToList();

                    chartData.Add(seriesNo);
                }
                return Json(new
                {
                    status = CommonConstants.Status_Success,
                    message = "BtsStatByOperatorCity Finished !",
                    chartData = chartData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                // Base Controller đã ghi Log Error rồi
                return Json(new
                {
                    status = CommonConstants.Status_Error,
                    message = e.Message,
                    chartData = chartData
                }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult StatisticByYear(string selectOperatorId, string selectCityId)
        //{
        //    List<object> chartData = new List<object>();
        //    try
        //    {
        //        IEnumerable<CertificateStatiticsViewModel> ByYear = _stattisticService.GetCertificateStatisticByYearOperator();
        //        DataTable dtValidCertificates = ByYear.ToPivotTable(x => x.Year, x => x.OperatorID, items => items.Any() ? items.Sum(x => x.ValidCertificates) : 0);
        //        DataTable dtExpiredInYearCertificates = ByYear.ToPivotTable(x => x.Year, x => x.OperatorID, items => items.Any() ? items.Sum(x => x.ExpiredInYearCertificates) : 0);

        //        //List<String> ColumnNames = new List<string>() {
        //        //    "Operator", "ValidCertificates","ExpiredInYearCertificates"};
        //        //chartData.Add(ColumnNames);
        //        //List<String> labels = dtValidCertificates.Select(x => x.).ToList();
        //        //chartData.Add(labels);
        //        //List<int> seriesNo = dtValidCertificates.Select(x => x.ValidCertificates).ToList();
        //        //chartData.Add(seriesNo);
        //        //seriesNo = dtValidCertificates.Select(x => x.ExpiredInYearCertificates).ToList();
        //        //chartData.Add(seriesNo);
        //        return Json(new
        //        {
        //            status = CommonConstants.Status_Success,
        //            message = "Import Certificate Finished !",
        //            chartData = chartData
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        // Base Controller đã ghi Log Error rồi
        //        return Json(new
        //        {
        //            status = CommonConstants.Status_Error,
        //            message = e.Message,
        //            chartData = chartData
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //}

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