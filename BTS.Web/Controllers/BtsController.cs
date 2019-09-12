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
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Controllers
{
    [AuthorizeRoles(CommonConstants.Data_CanView_Role)]
    public class BtsController : BaseController
    {
        private IBtsService _btsService;
        private IOperatorService _operatorService;

        public BtsController(IErrorService errorService, IOperatorService operatorService, IBtsService btsService) : base(errorService)
        {
            _operatorService = operatorService;
            _btsService = btsService;
        }

        public ActionResult Index()
        {
            IEnumerable<OperatorViewModel> operators = Mapper.Map<List<OperatorViewModel>>(_operatorService.getAll());
            IEnumerable<ProfileViewModel> profiles = Mapper.Map<List<ProfileViewModel>>(_btsService.getAllProfileInProcess());
            IEnumerable<CityViewModel> cities = Mapper.Map<List<CityViewModel>>(_btsService.getAllCity());
            ViewBag.operators = operators;
            ViewBag.profiles = profiles;
            ViewBag.cities = cities;
            TempData["ImagePath"] = User.Identity.GetImagePath();
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult loadBts()
        {
            int countItem;

            var CityID = Request.Form.GetValues("CityID").FirstOrDefault();
            var OperatorID = Request.Form.GetValues("OperatorID").FirstOrDefault();
            var ProfileID = Request.Form.GetValues("ProfileID").FirstOrDefault();
            var BtsCodeOrAddress = Request.Form.GetValues("BtsCodeOrAddress").FirstOrDefault().ToLower();

            DateTime StartDate, EndDate;
            if (!DateTime.TryParse(Request.Form.GetValues("StartDate").FirstOrDefault(), out StartDate))
                Console.Write("Loi chuyen doi kieu");
            if (!DateTime.TryParse(Request.Form.GetValues("EndDate").FirstOrDefault(), out EndDate))
                Console.Write("Loi chuyen doi kieu");

            // searching ...
            IEnumerable<Bts> Items;

            if (StartDate != null && EndDate != null)
            {
                //Items = _btsService.getAll(out countItem, StartDate, EndDate).ToList();
                Items = _btsService.getAll(out countItem).ToList();
                Items = Items.Where(x => x.Profile.ApplyDate >= StartDate && x.Profile.ApplyDate <= EndDate);
            }
            else
            {
                Items = _btsService.getAll(out countItem).ToList();
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
                Items = Items.Where(x => x.ProfileID?.ToString() == ProfileID);
            }

            if (!(string.IsNullOrEmpty(BtsCodeOrAddress)))
            {
                Items = Items.Where(x => x.BtsCode.ToLower().Contains(BtsCodeOrAddress) || x.Address.ToLower().Contains(BtsCodeOrAddress));
            }

            var recordsFiltered = Items.Count();

            IEnumerable<BtsViewModel> dataViewModel = Mapper.Map<List<BtsViewModel>>(Items);
            if (recordsFiltered > 0)
            {
                //var tbcat = from c in dataViewModel select new { c.Id, c.title, c.descriptions, action = "<a href='" + Url.Action("edit", "Category", new { id = c.Id }) + "'>Edit</a> | <a href='javascript:;' onclick='MyStore.Delete(" + c.Id + ")'>Delete</a>" };                
                JsonResult result = Json(new { data = dataViewModel }, JsonRequestBehavior.AllowGet);
                result.MaxJsonLength = Int32.MaxValue;
                return result;
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewAll()
        {
            // searching ...
            IEnumerable<BtsViewModel> Items = GetAll();
            return View(Items);
        }

        private IEnumerable<BtsViewModel> GetAll()
        {
            int countItem;
            var model = _btsService.getAll(out countItem).ToList();
            return Mapper.Map<IEnumerable<BtsViewModel>>(model);
        }

        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanEdit_Role)]
        public async Task<ActionResult> AddOrEdit(string act, string id = "0")
        {
            BtsViewModel ItemVm = new BtsViewModel();
            if (!string.IsNullOrEmpty(id))
            {                
                var DbItem = _btsService.getByID(id);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<BtsViewModel>(DbItem);
                }
            }

            IEnumerable<Operator> operatorList = _btsService.getAllOperator().ToList();
            foreach (var operatorItem in operatorList)
            {
                var listItem = new SelectListItem()
                {
                    Text = operatorItem.Name,
                    Value = operatorItem.Id,
                    Selected = false
                };
                ItemVm.OperatorList.Add(listItem);
            }

            IEnumerable<Model.Models.Profile> profileList = _btsService.getAllProfile().ToList();
            foreach (var profileItem in profileList)
            {
                var listItem = new SelectListItem()
                {
                    Text = profileItem.BtsQuantity + "-" + profileItem.ApplicantID + "-" + profileItem.ProfileNum,
                    Value = profileItem.Id?.ToString(),
                    Selected = false
                };
                ItemVm.ProfileList.Add(listItem);
            }

            IEnumerable<City> cityList = _btsService.getAllCity().ToList();
            foreach (var cityItem in cityList)
            {
                var listItem = new SelectListItem()
                {
                    Text = cityItem.Name,
                    Value = cityItem.Id,
                    Selected = false
                };
                ItemVm.CityList.Add(listItem);
            }

            IEnumerable<InCaseOf> inCaseOfList = _btsService.getAllInCaseOf().ToList();
            foreach (var inCaseOfItem in inCaseOfList)
            {
                var listItem = new SelectListItem()
                {
                    Text = inCaseOfItem.Name,
                    Value = inCaseOfItem.Id.ToString(),
                    Selected = false
                };
                ItemVm.InCaseOfList.Add(listItem);
            }

            if (act == CommonConstants.Action_Edit)
                return View("Edit", ItemVm);
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
        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanEdit_Role)]
        public async Task<ActionResult> AddOrEdit(string act, BtsViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (act == CommonConstants.Action_Add)
                    {
                        var newItem = new Bts();
                        newItem.UpdateBts(Item);

                        newItem.CreatedBy = User.Identity.Name;
                        newItem.CreatedDate = DateTime.Now;

                        _btsService.Add(newItem);
                        _btsService.SaveChanges();
                        return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<BtsViewModel>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var editItem = _btsService.getByID(Item.Id);
                        editItem.UpdateBts(Item);
                        editItem.UpdatedBy = User.Identity.Name;
                        editItem.UpdatedDate = DateTime.Now;

                        _btsService.Update(editItem);
                        _btsService.SaveChanges();
                        return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<BtsViewModel>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập liệu" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRoles(CommonConstants.Data_CanDelete_Role)]
        public async Task<ActionResult> Delete(string id = "0")
        {
            try
            {
                var dbItem = _btsService.getByID(id);
                if (dbItem == null)
                {
                    return HttpNotFound();
                }

                //if (_btsService.IsUsed(id))
                //{
                //    return Json(new { status = CommonConstants.Status_Error, message = "Không thể xóa Trường hợp kiểm định này do đã được sử dụnd" }, JsonRequestBehavior.AllowGet);
                //}

                _btsService.Delete(id);
                _btsService.SaveChanges();

                return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}