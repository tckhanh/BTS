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

namespace BTS.Web.Controllers
{
    [AuthorizeRoles(CommonConstants.Data_CanView_Role)]
    public class CertificateController : BaseController
    {
        private ICertificateService _certificateService;
        private IOperatorService _operatorService;
        private IProfileService _profileService;
        private ICityService _cityService;

        public CertificateController(ICertificateService certificateService, IOperatorService operatorService, IProfileService profileService, ICityService cityService, IErrorService errorService) : base(errorService)
        {
            _certificateService = certificateService;
            _operatorService = operatorService;
            _profileService = profileService;
            _cityService = cityService;
        }

        // GET: Certificate
        public ActionResult Index()
        {
            IEnumerable<OperatorViewModel> operators = Mapper.Map<List<OperatorViewModel>>(_operatorService.getAll());
            IEnumerable<ProfileViewModel> profiles = Mapper.Map<List<ProfileViewModel>>(_profileService.getAll());
            IEnumerable<CityViewModel> cities = Mapper.Map<List<CityViewModel>>(_cityService.getAll());

            ViewBag.operators = operators;
            ViewBag.profiles = profiles;
            ViewBag.cities = cities;
            return View();
        }

        [HttpPost]
        public JsonResult GetUserRoles()
        {
            return Json(new
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                Roles = UserManager.GetRolesAsync(User.Identity.GetUserId())
            });
        }

        [HttpPost]

        //[ValidateAntiForgeryToken]
        public JsonResult loadCertificate()
        {
            int countItem;

            var CityID = Request.Form.GetValues("CityID").FirstOrDefault();
            var OperatorID = Request.Form.GetValues("OperatorID").FirstOrDefault();
            var ProfileID = Request.Form.GetValues("ProfileID").FirstOrDefault();
            var BtsCodeOrAddress = Request.Form.GetValues("BtsCodeOrAddress").FirstOrDefault().ToLower();
            var IsExpired = Request.Form.GetValues("IsExpired").FirstOrDefault().ToLower();

            DateTime StartDate, EndDate;
            if (!DateTime.TryParse(Request.Form.GetValues("StartDate").FirstOrDefault(), out StartDate))
                Console.Write("Loi chuyen doi kieu");
            if (!DateTime.TryParse(Request.Form.GetValues("EndDate").FirstOrDefault(), out EndDate))
                Console.Write("Loi chuyen doi kieu");

            // searching ...
            IEnumerable<Certificate> Items;

            if (StartDate != null && EndDate != null)
            {
                Items = _certificateService.getAll(out countItem, false, StartDate, EndDate).ToList();
            }
            else
            {
                Items = _certificateService.getAll(out countItem, false).ToList();
            }

            if (IsExpired == "yes")
            {
                Items = Items.Where(x => x.ExpiredDate < DateTime.Today);
            }
            else
            {
                Items = Items.Where(x => x.ExpiredDate >= DateTime.Today);
            }

            if (!(string.IsNullOrEmpty(CityID)))
            {
                Items = Items.Where(x => x.CityID == CityID);
            }

            if (!(string.IsNullOrEmpty(OperatorID)))
            {
                Items = Items.Where(x => x.OperatorID.Contains(OperatorID));
            }

            if (!(string.IsNullOrEmpty(ProfileID)))
            {
                Items = Items.Where(x => x.ProfileID.ToString() == ProfileID);
            }

            if (!(string.IsNullOrEmpty(BtsCodeOrAddress)))
            {
                Items = Items.Where(x => x.BtsCode.ToLower().Contains(BtsCodeOrAddress) || x.Address.ToLower().Contains(BtsCodeOrAddress));
            }

            var recordsFiltered = Items.Count();

            IEnumerable<CertificateViewModel> dataViewModel = Mapper.Map<List<CertificateViewModel>>(Items);
            if (countItem > 0)
            {
                //var tbcat = from c in dataViewModel select new { c.Id, c.title, c.descriptions, action = "<a href='" + Url.Action("edit", "Category", new { id = c.Id }) + "'>Edit</a> | <a href='javascript:;' onclick='MyStore.Delete(" + c.Id + ")'>Delete</a>" };
                return Json(new { data = dataViewModel }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PivotTable()
        {
            return View();
        }

        public ActionResult GetCertificateByCity(string cityID)
        {
            var CertificateData = _certificateService.getCertificateByCity(cityID);
            var model = Mapper.Map<IEnumerable<Certificate>, IEnumerable<CertificateViewModel>>(CertificateData);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCertificate()
        {
            int countBTS = 0;
            List<Certificate> dataSumary = _certificateService.getAll(out countBTS, true).ToList();

            return Json(dataSumary, JsonRequestBehavior.AllowGet);
        }
    }
}