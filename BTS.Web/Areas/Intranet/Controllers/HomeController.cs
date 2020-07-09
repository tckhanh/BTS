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

namespace BTS.Web.Areas.Intranet.Controllers
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
        [ValidateAntiForgeryToken]
        public ActionResult StatCoupleCerByOperator()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatCoupleCerByOperatorVM> ByOperator = _stattisticService.GetStatCoupleCerByOperator();

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
                    message = "StatCoupleCerByOperator Finished !",
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
        [ValidateAntiForgeryToken]
        public ActionResult StatBtsInProcess()
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
                    message = "StatBtsInProcess Finished !",
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
        [ValidateAntiForgeryToken]
        public ActionResult StatCerByOperator()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatCerByOperatorVM> ByOperator = _stattisticService.GetStatCerByOperator();

                DataTable pivotTable = ByOperator.ToDataTable();

                DataRow dr = pivotTable.NewRow();
                dr["OperatorID"] = "TOTAL";

                foreach (DataColumn col in pivotTable.Columns)
                {
                    if (col.ColumnName == "ValidCertificates")
                    {
                        long total = 0;
                        foreach (DataRow row in pivotTable.Rows)
                        {
                            total += Convert.ToInt32(row[col.ColumnName].ToString());
                        }
                        dr[col.ColumnName] = total;   // or set it to some other value
                    } 
                }
                pivotTable.Rows.Add(dr);

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
                    message = "StatCerByOperator Finished !",
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
        [ValidateAntiForgeryToken]
        public ActionResult StatExpiredCerByOperatorYear()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatExpiredCerByOperatorYearVM> ByOperatorYear = _stattisticService.GetStatExpiredCerByOperatorYear().ToList();
                
                DataTable pivotTable = ByOperatorYear.ToPivotTable(item => item.OperatorID, item => item.Year, items => items.Any() ? items.Sum(item => item.ExpiredCertificates) : 0);

                pivotTable.Columns.Add("TOTAL", typeof(System.Int32));

                foreach (DataRow row in pivotTable.Rows)
                {
                    long total = 0;
                    foreach (DataColumn col in pivotTable.Columns)
                    {
                        if (col.ColumnName != "Year" && col.ColumnName != "TOTAL")
                            total += Convert.ToInt32(row[col.ColumnName].ToString());
                    }
                    row["TOTAL"] = total;   // or set it to some other value
                }

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
                    message = "StatExpiredInYearCerByOperator Finished !",
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
        [ValidateAntiForgeryToken]
        public ActionResult StatCerByOperatorYear()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatIssuedCerByOperatorYearVM> ByOperatorYear = _stattisticService.GetIssuedStatCerByOperatorYear();
                DataTable pivotTable = ByOperatorYear.ToPivotTable(item => item.OperatorID, item => item.Year, items => items.Any() ? items.Sum(item => item.IssuedCertificates) : 0);
                
                pivotTable.Columns.Add("TOTAL", typeof(System.Int32));

                foreach (DataRow row in pivotTable.Rows)
                {
                    long total = 0;
                    foreach (DataColumn col in pivotTable.Columns)
                    {
                        if (col.ColumnName != "Year" && col.ColumnName != "TOTAL")
                            total += Convert.ToInt32(row[col.ColumnName].ToString());
                    }
                    row["TOTAL"] = total;   // or set it to some other value
                }

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
                    message = "StatCerByOperatorYear Finished !",
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
        [ValidateAntiForgeryToken]
        public ActionResult StatBtsByOperatorBand()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatBtsByOperatorBandVM> ByBand = _stattisticService.GetStatBtsByOperatorBand();

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
                    message = "StatBtsByBand Finished !",
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
        [ValidateAntiForgeryToken]
        public ActionResult StatBtsByOperatorManufactory()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatBtsByOperatorManufactoryVM> ByManufactory = _stattisticService.GetStatBtsByOperatorManufactory();

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
                    message = "StatBtsByManufactory Finished !",
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
        [ValidateAntiForgeryToken]
        public ActionResult StatBtsByOperatorEquipment()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatBtsByEquipmentVM> ByEquipment = _stattisticService.GetStatBtsByEquipment();

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
                    message = "StatBtsByEquipment Finished !",
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