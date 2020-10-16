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
using System.Web.SessionState;

namespace BTS.Web.Areas.Controllers
{    
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class StatisticsController : BaseController
    {
        //IProductCategoryService _productCategoryService;
        private ICertificateService _btsCertificateService;
        private ICityService _cityService;
        private IStatisticService _stattisticService;
        private ICommonService _commonService;
        private ICertificateService _certificateService;

        public StatisticsController(ICertificateService btscertificateService, ICommonService commonService, ICertificateService certificateService, ICityService cityService, IStatisticService stattisticService, IErrorService errorService) : base(errorService)
        {
            _commonService = commonService;
            _certificateService = certificateService;
            _btsCertificateService = btscertificateService;
            _cityService = cityService;
            _stattisticService = stattisticService;
        }

        [OutputCache(Duration = 60, Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            IEnumerable<CityViewModel> cities = Mapper.Map<List<CityViewModel>>(_cityService.getAll());
            
            //cities = cities.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.Id));

            IEnumerable<string> AreaList = cities.Select(x => x.Area).Distinct();

            ViewBag.AreaList = AreaList;
            //ViewBag.cities = cities;
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

        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatCoupleCerByArea()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatCoupleCerByAreaVM> ByArea = _stattisticService.GetStatCoupleCerByArea();
                DataTable pivotTable = ByArea.ToDataTable();
                
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
                    message = "StatCoupleCerByArea Finished !",
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

        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatCerByArea()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatCerByAreaVM> ByOperator = _stattisticService.GetStatCerByArea();

                DataTable pivotTable = ByOperator.ToDataTable();

                DataRow dr = pivotTable.NewRow();
                dr["Area"] = "TOTAL";

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
                    message = "StatCerByArea Finished !",
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

        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatExpiredCerByAreaYear()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatExpiredCerByAreaYearVM> ByAreaYear = _stattisticService.GetStatExpiredCerByAreaYear().ToList();

                DataTable pivotTable = ByAreaYear.ToPivotTable(item => item.Area, item => item.Year, items => items.Any() ? items.Sum(item => item.ExpiredCertificates) : 0);

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
                    message = "StatExpiredCerByAreaYear Finished !",
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


        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatCerByAreaYear()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatIssuedCerByAreaYearVM> ByAreaYear = _stattisticService.GetIssuedStatCerByAreaYear();
                DataTable pivotTable = ByAreaYear.ToPivotTable(item => item.Area, item => item.Year, items => items.Any() ? items.Sum(item => item.IssuedCertificates) : 0);

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
                    message = "StatCerByAreaYear Finished !",
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

        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatBtsByAreaBand()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatBtsByAreaBandVM> ByBand = _stattisticService.GetStatBtsByAreaBand();

                DataTable pivotTable = ByBand.ToPivotTable(item => item.Area, item => item.Band, items => items.Any() ? items.Sum(item => item.Btss) : 0);

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

        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatBtsByAreaManufactory()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatBtsByAreaManufactoryVM> ByManufactory = _stattisticService.GetStatBtsByAreaManufactory();

                DataTable pivotTable = ByManufactory.ToPivotTable(item => item.Area, item => item.Manufactory, items => items.Any() ? items.Sum(item => item.Btss) : 0);
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
                    message = "StatBtsByAreaManufactory Finished !",
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

        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatBtsInProcessArea()
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

        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatCerByOperatorArea()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatIssuedCerByOperatorAreaVM> ByOperatorArea = _stattisticService.GetIssuedStatCerByOperatorArea();
                DataTable pivotTable = ByOperatorArea.ToPivotTable(item => item.OperatorID, item => item.Area, items => items.Any() ? items.Sum(item => item.IssuedCertificates) : 0);

                pivotTable.Columns.Add("TOTAL", typeof(System.Int32));

                foreach (DataRow row in pivotTable.Rows)
                {
                    long total = 0;
                    foreach (DataColumn col in pivotTable.Columns)
                    {
                        if (col.ColumnName != "Area" && col.ColumnName != "TOTAL")
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
                    message = "StatCerByOperatorArea Finished !",
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

        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatNearExpiredInYearCerByOperatorCity()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatNearExpiredInYearCerByOperatorCityVM> ByOperatorCity = _stattisticService.GetStatNearExpiredInYearCerByOperatorCity();
                ByOperatorCity = ByOperatorCity.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID));

                DataTable pivotTable = ByOperatorCity.ToPivotTable(item => item.OperatorID, item => item.CityID, items => items.Any() ? items.Sum(item => item.NearExpiredInYearCertificates) : 0);

                //pivotTable.Columns.Add("TOTAL", typeof(System.Int32));

                //foreach (DataRow row in pivotTable.Rows)
                //{
                //    long total = 0;
                //    foreach (DataColumn col in pivotTable.Columns)
                //    {
                //        if (col.ColumnName != "CityID" && col.ColumnName != "TOTAL")
                //            total += Convert.ToInt32(row[col.ColumnName].ToString());
                //    }
                //    row["TOTAL"] = total;   // or set it to some other value
                //}

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
                    message = "StatNearExpiredInYearCerByOperatorCity Finished !",
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

        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatExpiredCerByOperatorCity()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatExpiredCerByOperatorCityVM> ByOperatorCity = _stattisticService.GetStatExpiredInYearCerByOperatorCity();
                ByOperatorCity = ByOperatorCity.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID));

                DataTable pivotTable = ByOperatorCity.ToPivotTable(item => item.OperatorID, item => item.CityID, items => items.Any() ? items.Sum(item => item.ExpiredCertificates) : 0);

                //pivotTable.Columns.Add("TOTAL", typeof(System.Int32));

                //foreach (DataRow row in pivotTable.Rows)
                //{
                //    long total = 0;
                //    foreach (DataColumn col in pivotTable.Columns)
                //    {
                //        if (col.ColumnName != "CityID" && col.ColumnName != "TOTAL")
                //            total += Convert.ToInt32(row[col.ColumnName].ToString());
                //    }
                //    row["TOTAL"] = total;   // or set it to some other value
                //}

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
                    message = "StatExpiredCerByOperatorCity Finished !",
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

        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatExpiredCerByOperatorYearArea()
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


        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatCerByOperatorYearArea()
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

        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatBtsByOperatorBandArea()
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

        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatBtsByOperatorManufactoryArea()
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

        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatBtsByOperatorEquipmentArea()
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

        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatCerByOperatorCity()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatIssuedCerByOperatorCityVM> ByOperatorCity = _stattisticService.GetIssuedStatCerByOperatorCity();
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
                    message = "StatCerByOperatorCity Finished !",
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

        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatInYearCerByOperatorCity()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatIssuedCerByOperatorCityVM> ByOperatorCity = _stattisticService.GetStatInYearCerByOperatorCity();
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
                    message = "StatInYearCerByOperatorCity Finished !",
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

        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatBtsByBandCity()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatBtsByBandCityVM> ByBandCity = _stattisticService.GetStatBtsByBandCity();
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
                    message = "StatBtsByBandCity Finished !",
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

        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatBtsByOperatorArea()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatBtsByOperatorAreaVM> ByOperatorArea = _stattisticService.GetStatBtsByOperatorArea();

                DataTable pivotTable = ByOperatorArea.ToPivotTable(item => item.OperatorID, item => item.Area, items => items.Any() ? items.Sum(item => item.Btss) : 0);

                pivotTable.Columns.Add("TOTAL", typeof(System.Int32));

                foreach (DataRow row in pivotTable.Rows)
                {
                    long total = 0;
                    foreach (DataColumn col in pivotTable.Columns)
                    {
                        if (col.ColumnName != "Area" && col.ColumnName != "TOTAL")
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
                    message = "StatBtsByOperatorArea Finished !",
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

        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatBtsByOperatorCity()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatBtsByOperatorCityVM> ByOperatorCity = _stattisticService.GetStatBtsByOperatorCity();
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
                    message = "StatBtsByOperatorCity Finished !",
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

        [AuthorizeRoles(CommonConstants.Info_CanViewStatitics_Role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatBtsByManufactoryCity()
        {
            List<object> chartData = new List<object>();
            try
            {
                IEnumerable<StatBtsByOperatorCityVM> ByOperatorCity = _stattisticService.GetStatBtsByManufactoryCity();
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
                    message = "StatBtsByOperatorCity Finished !",
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
    }
}