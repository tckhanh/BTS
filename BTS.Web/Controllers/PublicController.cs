using AutoMapper;
using BTS.Common;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Infrastructure.Extensions;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.SessionState;

namespace BTS.Web.Areas.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class PublicController : BaseController
    {
        private ICertificateService _certificateService;
        private IOperatorService _operatorService;
        private IProfileService _profileService;
        private ICityService _cityService;
        private IInCaseOfService _inCaseOfService;
        private ILabService _labService;
        private ISubBtsInCertService _subBtsInCertService;


        public PublicController(ICertificateService certificateService, ISubBtsInCertService subBtsInCertService, IOperatorService operatorService, IProfileService profileService, ICityService cityService, IInCaseOfService inCaseOfService, ILabService labService, IErrorService errorService) : base(errorService)
        {
            _certificateService = certificateService;
            _subBtsInCertService = subBtsInCertService;
            _operatorService = operatorService;
            _profileService = profileService;
            _cityService = cityService;
            _inCaseOfService = inCaseOfService;
            _labService = labService;
        }

        // GET: Certificate
        public ActionResult Index()
        {
            IEnumerable<OperatorViewModel> operators = Mapper.Map<List<OperatorViewModel>>(_operatorService.getAll()).ToList();
            IEnumerable<ProfileViewModel> profiles = Mapper.Map<List<ProfileViewModel>>(_profileService.getAll().OrderByDescending(x => x.ApplyDate)).ToList();
            IEnumerable<CityViewModel> cities = Mapper.Map<List<CityViewModel>>(_cityService.getAll()).ToList();
            if (User.Identity.IsAuthenticated)
            {
                if (getEnableCityIDsScope() == "True")
                {
                    cities = cities.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.Id));
                }
            }
            else
            {
                cities = new List<CityViewModel>();
            }
            ViewBag.operators = operators;
            ViewBag.profiles = profiles;
            ViewBag.cities = cities;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult loadCertificate(string curCenter, string curBounds)
        {
            int countItem;

            //string CityID = Request.Form.GetValues("CityID")?.FirstOrDefault();
            //string curCenter = Request.Form.GetValues("curCenter")?.FirstOrDefault();
            //string curBounds = Request.Form.GetValues("curBounds")?.FirstOrDefault();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            LatLng center = javaScriptSerializer.Deserialize<LatLng>(curCenter);
            LatLngBounds centerBounds = javaScriptSerializer.Deserialize<LatLngBounds>(curBounds);

            string CertificateStatus = CommonConstants.CertStatus_Valid;

            // searching ...
            IEnumerable<Certificate> Items = new List<Certificate>();
            if (!(string.IsNullOrEmpty(curCenter)))
            {


                if (CertificateStatus == CommonConstants.CertStatus_WaitToSign)
                {
                    Items = _certificateService.getCertificateWaitToSign().ToList();
                }
                else if (CertificateStatus == CommonConstants.CertStatus_Expired)
                {
                    Items = _certificateService.getCertificateExpired().ToList();
                }
                else if (CertificateStatus == CommonConstants.CertStatus_Valid)
                {
                    if (!(string.IsNullOrEmpty(curCenter)))
                    {
                        Items = _certificateService.getCertificateByLocation(center, centerBounds, 10).ToList();
                    }
                    else
                    {
                        //Items = _certificateService.getAll(out countItem, false).ToList();
                    }
                }

                if (CertificateStatus == CommonConstants.CertStatus_WaitToSign)
                {
                    Items = Items.Where(x => x.IsSigned == false && x.IsCanceled == false);
                }
                if (CertificateStatus == CommonConstants.CertStatus_Expired)
                {
                    Items = Items.Where(x => x.IsCanceled == true || x.ExpiredDate < DateTime.Now);
                }

                if (CertificateStatus == CommonConstants.CertStatus_Valid)
                {
                    Items = Items.Where(x => x.IsSigned == true && x.IsCanceled == false && x.ExpiredDate >= DateTime.Now);
                }
                if (!(string.IsNullOrEmpty(curCenter)))
                {
                    Items = _certificateService.getCertificateByLocation(center, centerBounds, 10).ToList();
                }                
            }

            //Items = Items.OrderByDescending(x => x.IssuedDate.Year.ToString() + x.Id);

            IEnumerable<CertificateViewModel> dataViewModel = Mapper.Map<List<CertificateViewModel>>(Items);

            int recordsFiltered = Items.Count();

            //for (int i = 0; i < recordsFiltered; i++)
            //{
            //    IEnumerable<SubBtsInCert> SubItems = _certificateService.getDetailByID(dataViewModel.ElementAt(i).Id);
            //    IEnumerable<SubBtsInCertViewModel> SubItemsVM = Mapper.Map<List<SubBtsInCertViewModel>>(SubItems);
            //    foreach (var subItemVM in SubItemsVM)
            //    {
            //        dataViewModel.ElementAt(i).SubBtsList.Add(subItemVM);
            //    }
            //}

            if (recordsFiltered > 0)
            {
                //var tbcat = from c in dataViewModel select new { c.Id, c.title, c.descriptions, action = "<a href='" + Url.Action("edit", "Category", new { id = c.Id }) + "'>Edit</a> | <a href='javascript:;' onclick='MyStore.Delete(" + c.Id + ")'>Delete</a>" };
                JsonResult result = Json(new { data = dataViewModel }, JsonRequestBehavior.AllowGet);
                result.MaxJsonLength = int.MaxValue;
                return result;
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRoles(CommonConstants.Data_CanViewDetail_Role)]
        public JsonResult SubDetail(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                IEnumerable<SubBtsInCert> Items = _certificateService.getDetailByID(Id);

                int countItem = Items.Count();

                IEnumerable<SubBtsInCertViewModel> dataViewModel = Mapper.Map<List<SubBtsInCertViewModel>>(Items);
                if (countItem > 0)
                {
                    return Json(new { resetUrl = Url.Action("Add", "Certificate"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "SubDetail", dataViewModel), message = "Lấy dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "Certificate"), status = CommonConstants.Status_Error, message = "Không có dữ liệu" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { resetUrl = Url.Action("Add", "Certificate"), status = CommonConstants.Status_Error, message = "Lỗi lấy dữ liệu" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetCertificateByCity(string cityID)
        {
            IEnumerable<Certificate> Items = _certificateService.getCertificateByCity(cityID);

            if (getEnableCityIDsScope() == "True")
            {
                Items = Items.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID)).ToList();
            }
            IEnumerable<CertificateViewModel> model = Mapper.Map<IEnumerable<Certificate>, IEnumerable<CertificateViewModel>>(Items);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCertificate()
        {
            int countBTS;
            List<Certificate> dataSumary = _certificateService.getAll(out countBTS, true).ToList();

            return Json(dataSumary, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ViewAll()
        {
            return View(GetAll());
        }

        private IEnumerable<CertificateViewModel> GetAll()
        {
            int rows;
            List<Certificate> model = _certificateService.getAll(out rows, false).ToList();
            return Mapper.Map<IEnumerable<CertificateViewModel>>(model);
        }

        private CertificateViewModel FillInCertificateVM(CertificateViewModel ItemVm)
        {
            IEnumerable<Operator> operatorList = _operatorService.getAll().ToList();
            foreach (Operator operatorItem in operatorList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = operatorItem.Name,
                    Value = operatorItem.Id,
                    Selected = false
                };
                ItemVm.OperatorList.Add(listItem);
            }

            IEnumerable<Model.Models.Profile> profileList = _profileService.getAll().ToList();
            foreach (Model.Models.Profile profileItem in profileList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = profileItem.BtsQuantity + "-" + profileItem.ApplicantID + "-" + profileItem.ProfileNum,
                    Value = profileItem.Id?.ToString(),
                    Selected = false
                };
                ItemVm.ProfileList.Add(listItem);
            }

            IEnumerable<City> cityList = _cityService.getAll().ToList();
            foreach (City cityItem in cityList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = cityItem.Name,
                    Value = cityItem.Id,
                    Selected = false
                };
                ItemVm.CityList.Add(listItem);
            }

            IEnumerable<InCaseOf> inCaseOfList = _inCaseOfService.getAll().ToList();
            foreach (InCaseOf inCaseOfItem in inCaseOfList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = inCaseOfItem.Name,
                    Value = inCaseOfItem.Id.ToString(),
                    Selected = false
                };
                ItemVm.InCaseOfList.Add(listItem);
            }

            IEnumerable<Lab> labList = _labService.getAll().ToList();
            foreach (Lab labItem in labList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = labItem.Name,
                    Value = labItem.Id.ToString(),
                    Selected = false
                };
                ItemVm.LabList.Add(listItem);
            }
            return ItemVm;
        }
    }
}