using AutoMapper;
using BTS.Common;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Infrastructure.Extensions;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BTS.Web.Controllers
{
    [AuthorizeRoles(CommonConstants.Data_CanView_Role)]
    public class NoCertificateController : BaseController
    {
        private INoCertificateService _noCertificateService;
        private IOperatorService _operatorService;
        private IProfileService _profileService;
        private ICityService _cityService;
        private IInCaseOfService _inCaseOfService;
        private ILabService _labService;

        public NoCertificateController(INoCertificateService noCertificateService, IOperatorService operatorService, IProfileService profileService, ICityService cityService, IInCaseOfService inCaseOfService, ILabService labService, IErrorService errorService) : base(errorService)
        {
            _noCertificateService = noCertificateService;
            _operatorService = operatorService;
            _profileService = profileService;
            _cityService = cityService;
            _inCaseOfService = inCaseOfService;
            _labService = labService;
        }

        // GET: NoCertificate
        public ActionResult Index()
        {
            IEnumerable<OperatorViewModel> operators = Mapper.Map<List<OperatorViewModel>>(_operatorService.getAll().ToList());
            IEnumerable<ProfileViewModel> profiles = Mapper.Map<List<ProfileViewModel>>(_profileService.getAll().OrderByDescending(x => x.ApplyDate)).ToList();
            IEnumerable<CityViewModel> cities = Mapper.Map<List<CityViewModel>>(_cityService.getAll().ToList());

            cities = cities.Where(x => getCityIDsScope().ToString().Split(new char[] { ';' }).Contains(x.Id));

            ViewBag.operators = operators;
            ViewBag.profiles = profiles;
            ViewBag.cities = cities;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult loadNoCertificate()
        {
            int countItem;

            string CityID = Request.Form.GetValues("CityID").FirstOrDefault();
            string OperatorID = Request.Form.GetValues("OperatorID").FirstOrDefault();
            string ProfileID = Request.Form.GetValues("ProfileID").FirstOrDefault();
            string BtsCodeOrAddress = Request.Form.GetValues("BtsCodeOrAddress").FirstOrDefault().ToLower();
            DateTime StartDate, EndDate;
            if (!DateTime.TryParse(Request.Form.GetValues("StartDate").FirstOrDefault(), out StartDate))
            {
                Console.Write("Loi chuyen doi kieu");
            }

            if (!DateTime.TryParse(Request.Form.GetValues("EndDate").FirstOrDefault(), out EndDate))
            {
                Console.Write("Loi chuyen doi kieu");
            }

            // searching ...
            IEnumerable<NoCertificate> Items;

            if (!(string.IsNullOrEmpty(CityID)))
            {
                Items = _noCertificateService.getNoCertificateByCity(CityID).ToList();
            }
            else if (!(string.IsNullOrEmpty(OperatorID)))
            {
                Items = _noCertificateService.getNoCertificateByOperator(OperatorID).ToList();
            }
            else if (!(string.IsNullOrEmpty(ProfileID)))
            {
                Items = _noCertificateService.getNoCertificateByProfile(ProfileID).ToList();
            }
            else if (!(string.IsNullOrEmpty(BtsCodeOrAddress)))
            {
                Items = _noCertificateService.getNoCertificateByBtsCodeOrAddress(BtsCodeOrAddress).ToList();
            }
            else if (StartDate != null && EndDate != null)
            {
                Items = _noCertificateService.getAll(out countItem, false, StartDate, EndDate).ToList();
            }
            else
            {
                Items = _noCertificateService.getAll(out countItem).ToList();
            }


            if (StartDate != null && EndDate != null)
            {
                Items = Items.Where(x => x.TestReportDate >= StartDate && x.TestReportDate <= EndDate).ToList();
            }            

            if (!(string.IsNullOrEmpty(CityID)))
            {
                Items = Items.Where(x => x.CityID == CityID).ToList();
            }
            Items = Items.Where(x => getCityIDsScope().ToString().Split(new char[] { ';' }).Contains(x.CityID)).ToList();

            if (!(string.IsNullOrEmpty(OperatorID)))
            {
                Items = Items.Where(x => x.OperatorID.Contains(OperatorID)).ToList();
            }

            if (!(string.IsNullOrEmpty(ProfileID)))
            {
                Items = Items.Where(x => x.ProfileID?.ToString() == ProfileID).ToList();
            }

            if (!(string.IsNullOrEmpty(BtsCodeOrAddress)))
            {
                Items = Items.Where(x => x.BtsCode.ToLower().Contains(BtsCodeOrAddress) || x.Address.ToLower().Contains(BtsCodeOrAddress)).ToList();
            }

            //Items = Items.OrderByDescending(x => x.TestReportDate.ToString());

            int recordsFiltered = Items.Count();

            IEnumerable<NoCertificateViewModel> dataViewModel = Mapper.Map<List<NoCertificateViewModel>>(Items);
            if (recordsFiltered > 0)
            {
                //var tbcat = from c in dataViewModel select new { c.Id, c.title, c.descriptions, action = "<a href='" + Url.Action("edit", "Category", new { id = c.Id }) + "'>Edit</a> | <a href='javascript:;' onclick='MyStore.Delete(" + c.Id + ")'>Delete</a>" };
                JsonResult result = Json(new { data = dataViewModel }, JsonRequestBehavior.AllowGet);
                result.MaxJsonLength = int.MaxValue;
                return result;
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PivotTable()
        {
            return View();
        }

        public ActionResult GetNoCertificateByCity(string cityID)
        {
            List<NoCertificate> NoCertificateData = _noCertificateService.getNoCertificateByCity(cityID).ToList();
            NoCertificateData = NoCertificateData.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID)).ToList();

            IEnumerable<NoCertificateViewModel> model = Mapper.Map<IEnumerable<NoCertificate>, IEnumerable<NoCertificateViewModel>>(NoCertificateData);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNoCertificate()
        {
            int countBTS;
            List<NoCertificate> dataSumary = _noCertificateService.getAll(out countBTS).ToList();

            return Json(dataSumary, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewAll()
        {
            return View(GetAll());
        }

        private IEnumerable<NoCertificateViewModel> GetAll()
        {
            int rows;
            List<NoCertificate> model = _noCertificateService.getAll(out rows).ToList();
            return Mapper.Map<IEnumerable<NoCertificateViewModel>>(model);
        }

        private NoCertificateViewModel FillInNoCertificateVM(NoCertificateViewModel ItemVm)
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

        public ActionResult Add()
        {
            NoCertificateViewModel ItemVm = new NoCertificateViewModel();
            FillInNoCertificateVM(ItemVm);
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanViewDetail_Role)]
        public ActionResult Detail(string id = "0")
        {
            NoCertificateViewModel ItemVm = new NoCertificateViewModel();
            NoCertificate DbItem = _noCertificateService.getByID(id);

            if (DbItem != null)
            {
                ItemVm = Mapper.Map<NoCertificateViewModel>(DbItem);
            }
            FillInNoCertificateVM(ItemVm);
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(string id = "0")
        {
            NoCertificateViewModel ItemVm = new NoCertificateViewModel();
            NoCertificate DbItem = _noCertificateService.getByID(id);

            if (DbItem != null)
            {
                ItemVm = Mapper.Map<NoCertificateViewModel>(DbItem);
            }
            FillInNoCertificateVM(ItemVm);
            return View(ItemVm);
        }


        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, string id = "0")
        {
            NoCertificateViewModel ItemVm = new NoCertificateViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                NoCertificate DbItem = _noCertificateService.getByID(id);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<NoCertificateViewModel>(DbItem);
                }
            }

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


            if (act == CommonConstants.Action_Edit)
            {
                return View("Edit", ItemVm);
            }
            else if (act == CommonConstants.Action_Detail)
            {
                return View("Detail", ItemVm);
            }
            else
            {
                return View("Add", ItemVm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role)]
        public ActionResult Add(NoCertificateViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    NoCertificate newItem = new NoCertificate();
                    newItem.UpdateNoCertificate(Item);

                    newItem.CreatedBy = User.Identity.Name;
                    newItem.CreatedDate = DateTime.Now;

                    _noCertificateService.Add(newItem);
                    _noCertificateService.SaveChanges();
                    return Json(new { status = CommonConstants.Status_Success, message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    // html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<BtsViewModel>>(GetAll()))
                }
                else
                {
                    return Json(new { status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(NoCertificateViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    NoCertificate editItem = _noCertificateService.getByID(Item.Id);
                    editItem.UpdateNoCertificate(Item);
                    editItem.UpdatedBy = User.Identity.Name;
                    editItem.UpdatedDate = DateTime.Now;

                    _noCertificateService.Update(editItem);
                    _noCertificateService.SaveChanges();
                    return Json(new { status = CommonConstants.Status_Success, message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    // html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<BtsViewModel>>(GetAll()))
                }
                else
                {
                    return Json(new { status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, NoCertificateViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (act == CommonConstants.Action_Add)
                    {
                        NoCertificate newItem = new NoCertificate();
                        newItem.UpdateNoCertificate(Item);

                        newItem.CreatedBy = User.Identity.Name;
                        newItem.CreatedDate = DateTime.Now;

                        _noCertificateService.Add(newItem);
                        _noCertificateService.SaveChanges();
                        return Json(new { status = CommonConstants.Status_Success, message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                        // html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<BtsViewModel>>(GetAll()))
                    }
                    else
                    {
                        NoCertificate editItem = _noCertificateService.getByID(Item.Id);
                        editItem.UpdateNoCertificate(Item);
                        editItem.UpdatedBy = User.Identity.Name;
                        editItem.UpdatedDate = DateTime.Now;

                        _noCertificateService.Update(editItem);
                        _noCertificateService.SaveChanges();
                        return Json(new { status = CommonConstants.Status_Success, message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                        // html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<BtsViewModel>>(GetAll()))
                    }
                }
                else
                {
                    return Json(new { status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRoles(CommonConstants.Data_CanDelete_Role)]
        public ActionResult Delete(string id = "0")
        {
            try
            {
                NoCertificate dbItem = _noCertificateService.getByID(id);
                if (dbItem == null)
                {
                    return HttpNotFound();
                }

                //if (_btsService.IsUsed(id))
                //{
                //    return Json(new { status = CommonConstants.Status_Error, message = "Không thể xóa Trường hợp kiểm định này do đã được sử dụnd" }, JsonRequestBehavior.AllowGet);
                //}

                _noCertificateService.Delete(id);
                _noCertificateService.SaveChanges();

                return Json(new { data_restUrl = "/NoCertificate/Add", status = CommonConstants.Status_Success, message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                // html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()),
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}