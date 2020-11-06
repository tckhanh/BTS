﻿using AutoMapper;
using BTS.Common;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Infrastructure.Extensions;
using BTS.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Areas.Controllers
{
    [AuthorizeRoles(CommonConstants.Data_CanViewReport_Role)]
    public class ReportController : BaseController
    {
        private INoCertificateService _noCertificateService;
        private ICertificateService _certificateService;
        private IOperatorService _operatorService;
        private IProfileService _profileService;
        private ICityService _cityService;

        public ReportController(ICertificateService certificateService, INoCertificateService noCertificateService, IOperatorService operatorService, IProfileService profileService, ICityService cityService, IErrorService errorService) : base(errorService)
        {
            _noCertificateService = noCertificateService;
            _certificateService = certificateService;
            _operatorService = operatorService;
            _profileService = profileService;
            _cityService = cityService;
        }

        // GET: Certificate
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult loadCertificate()
        {
            int countItem = 0;

            int Month = Int32.Parse(Request.Form.GetValues("Month").FirstOrDefault());
            int Year = Int32.Parse(Request.Form.GetValues("Year").FirstOrDefault());

            DateTime StartDate = new DateTime(Year, Month + 1, 1);
            DateTime EndDate = StartDate.AddMonths(1).AddDays(-1);

            // searching ...
            IEnumerable<Certificate> Items = new List<Certificate>();

            if (StartDate != null && EndDate != null)
            {
                Items = _certificateService.getAll(out countItem, false, StartDate, EndDate).ToList();
            }

            if (getEnableCityIDsScope() == "True")
            {
                Items = Items.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID)).ToList();
            }
            //Items = Items.OrderBy(x => x.Id);

            IEnumerable<CertificateViewModel> dataViewModel = Mapper.Map<List<CertificateViewModel>>(Items);
            if (countItem > 0)
            {
                //var tbcat = from c in dataViewModel select new { c.Id, c.title, c.descriptions, action = "<a href='" + Url.Action("edit", "Category", new { id = c.Id }) + "'>Edit</a> | <a href='javascript:;' onclick='MyStore.Delete(" + c.Id + ")'>Delete</a>" };                
                JsonResult result = Json(new { data = dataViewModel }, JsonRequestBehavior.AllowGet);
                result.MaxJsonLength = Int32.MaxValue;
                return result;
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult loadReportTT18Cert()
        {
            int countItem = 0;

            int Month = Int32.Parse(Request.Form.GetValues("Month").FirstOrDefault());
            int Year = Int32.Parse(Request.Form.GetValues("Year").FirstOrDefault());

            DateTime StartDate = new DateTime(Year, Month + 1, 1);
            DateTime EndDate = StartDate.AddMonths(1).AddDays(-1);

            // searching ...
            IEnumerable<ReportTT18Cert> Items = new List<ReportTT18Cert>();

            if (StartDate != null && EndDate != null)
            {
                Items = _certificateService.getReportTT18Cert(out countItem, StartDate, EndDate).ToList();
            }

            if (getEnableCityIDsScope() == "True")
            {
                Items = Items.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID)).ToList();
            }

            Items = Items.OrderBy(x => x.CertificateId + x.BtsSerialNo);

            IEnumerable<ReportTT18CertViewModel> dataViewModel = Mapper.Map<List<ReportTT18CertViewModel>>(Items);
            if (countItem > 0)
            {
                //var tbcat = from c in dataViewModel select new { c.Id, c.title, c.descriptions, action = "<a href='" + Url.Action("edit", "Category", new { id = c.Id }) + "'>Edit</a> | <a href='javascript:;' onclick='MyStore.Delete(" + c.Id + ")'>Delete</a>" };                
                JsonResult result = Json(new { data = dataViewModel }, JsonRequestBehavior.AllowGet);
                result.MaxJsonLength = Int32.MaxValue;
                return result;
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult loadReportTT18NoCert()
        {
            int countItem = 0;

            int Month = Int32.Parse(Request.Form.GetValues("Month").FirstOrDefault());
            int Year = Int32.Parse(Request.Form.GetValues("Year").FirstOrDefault());

            DateTime StartDate = new DateTime(Year, Month + 1, 1);
            DateTime EndDate = StartDate.AddMonths(1).AddDays(-1);

            // searching ...
            IEnumerable<ReportTT18NoCert> Items = new List<ReportTT18NoCert>();

            if (StartDate != null && EndDate != null)
            {
                Items = _noCertificateService.getReportTT18NoCert(out countItem, StartDate, EndDate).ToList();
            }

            if (getEnableCityIDsScope() == "True")
            {
                Items = Items.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID)).ToList();
            }
            //Items = Items.OrderByDescending(x => x.TestReportDate.ToString());

            IEnumerable<ReportTT18NoCertViewModel> dataViewModel = Mapper.Map<List<ReportTT18NoCertViewModel>>(Items);
            if (countItem > 0)
            {
                //var tbcat = from c in dataViewModel select new { c.Id, c.title, c.descriptions, action = "<a href='" + Url.Action("edit", "Category", new { id = c.Id }) + "'>Edit</a> | <a href='javascript:;' onclick='MyStore.Delete(" + c.Id + ")'>Delete</a>" };                
                JsonResult result = Json(new { data = dataViewModel }, JsonRequestBehavior.AllowGet);
                result.MaxJsonLength = Int32.MaxValue;
                return result;
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PivotTable()
        {
            return View();
        }

        public ActionResult GetCertificateByCity(string cityID)
        {
            IEnumerable<Certificate> CertificateData = _certificateService.getCertificateByCity(cityID).ToList();
            if (getEnableCityIDsScope() == "True")
            {
                CertificateData = CertificateData.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID)).ToList();
            }
            var model = Mapper.Map<IEnumerable<Certificate>, IEnumerable<CertificateViewModel>>(CertificateData);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCertificate()
        {
            int countBTS = 0;
            List<Certificate> dataSumary = _certificateService.getAll(out countBTS, true).ToList();
            if (getEnableCityIDsScope() == "True")
            {
                dataSumary = dataSumary.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID)).ToList();
            }
            return Json(dataSumary, JsonRequestBehavior.AllowGet);
        }
    }
}