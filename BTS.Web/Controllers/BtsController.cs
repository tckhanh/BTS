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

namespace BTS.Web.Areas.Controllers
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult loadBts()
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
            IEnumerable<Bts> Items;

            if (StartDate != null && EndDate != null)
            {
                //Items = _btsService.getAll(out countItem, StartDate, EndDate).ToList();
                Items = _btsService.getAll(out countItem).ToList();
                Items = Items.Where(x => x.Profile.ApplyDate >= StartDate && x.Profile.ApplyDate <= EndDate).ToList();
            }
            else
            {
                Items = _btsService.getAll(out countItem).ToList();
            }

            if (!(string.IsNullOrEmpty(CityID)))
            {
                Items = Items.Where(x => x.CityID == CityID).ToList();
            }
            Items = Items.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID)).ToList();


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

            int recordsFiltered = Items.Count();

            IEnumerable<BtsViewModel> dataViewModel = Mapper.Map<List<BtsViewModel>>(Items).ToList();
            if (recordsFiltered > 0)
            {
                //var tbcat = from c in dataViewModel select new { c.Id, c.title, c.descriptions, action = "<a href='" + Url.Action("edit", "Category", new { id = c.Id }) + "'>Edit</a> | <a href='javascript:;' onclick='MyStore.Delete(" + c.Id + ")'>Delete</a>" };                
                JsonResult result = Json(new { data = dataViewModel }, JsonRequestBehavior.AllowGet);
                result.MaxJsonLength = int.MaxValue;
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
            List<Bts> model = _btsService.getAll(out countItem).ToList();
            return Mapper.Map<IEnumerable<BtsViewModel>>(model);
        }

        public ActionResult Add()
        {
            BtsViewModel ItemVm = new BtsViewModel();
            ItemVm = FillInBtsVM(ItemVm);
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanViewDetail_Role)]
        public ActionResult Detail(string id = "0")
        {
            BtsViewModel ItemVm = new BtsViewModel();
            Bts DbItem = _btsService.getByID(id);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<BtsViewModel>(DbItem);
            }
            ItemVm = FillInBtsVM(ItemVm);
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(string id = "0")
        {
            BtsViewModel ItemVm = new BtsViewModel();
            Bts DbItem = _btsService.getByID(id);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<BtsViewModel>(DbItem);
            }
            ItemVm = FillInBtsVM(ItemVm);
            return View(ItemVm);
        }

        private BtsViewModel FillInBtsVM(BtsViewModel ItemVm)
        {
            IEnumerable<Operator> operatorList = _btsService.getAllOperator().ToList();
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

            IEnumerable<Model.Models.Profile> profileList = _btsService.getAllProfile().ToList();
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

            IEnumerable<City> cityList = _btsService.getAllCity().ToList();
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

            IEnumerable<InCaseOf> inCaseOfList = _btsService.getAllInCaseOf().ToList();
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
            return ItemVm;
        }

        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, string id = "0")
        {
            BtsViewModel ItemVm = new BtsViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                Bts DbItem = _btsService.getByID(id);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<BtsViewModel>(DbItem);
                }
            }

            IEnumerable<Operator> operatorList = _btsService.getAllOperator().ToList();
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

            IEnumerable<Model.Models.Profile> profileList = _btsService.getAllProfile().ToList();
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

            IEnumerable<City> cityList = _btsService.getAllCity().ToList();
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

            IEnumerable<InCaseOf> inCaseOfList = _btsService.getAllInCaseOf().ToList();
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
        public ActionResult Add(BtsViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Bts newItem = new Bts();
                    newItem.UpdateBts(Item);

                    newItem.CreatedBy = User.Identity.Name;
                    newItem.CreatedDate = DateTime.Now;

                    _btsService.Add(newItem);
                    _btsService.SaveChanges();
                    return Json(new { resetUrl = Url.Action("Add", "Bts"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<BtsViewModel>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "Bts"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Bts"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(BtsViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Bts editItem = _btsService.getByID(Item.Id);
                    editItem.UpdateBts(Item);
                    editItem.UpdatedBy = User.Identity.Name;
                    editItem.UpdatedDate = DateTime.Now;

                    _btsService.Update(editItem);
                    _btsService.SaveChanges();
                    return Json(new { resetUrl = Url.Action("Add", "Bts"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<BtsViewModel>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "Bts"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Bts"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, BtsViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (act == CommonConstants.Action_Add)
                    {
                        Bts newItem = new Bts();
                        newItem.UpdateBts(Item);

                        newItem.CreatedBy = User.Identity.Name;
                        newItem.CreatedDate = DateTime.Now;

                        _btsService.Add(newItem);
                        _btsService.SaveChanges();
                        return Json(new { resetUrl = Url.Action("Add", "Bts"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<BtsViewModel>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Bts editItem = _btsService.getByID(Item.Id);
                        editItem.UpdateBts(Item);
                        editItem.UpdatedBy = User.Identity.Name;
                        editItem.UpdatedDate = DateTime.Now;

                        _btsService.Update(editItem);
                        _btsService.SaveChanges();
                        return Json(new { resetUrl = Url.Action("Add", "Bts"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<BtsViewModel>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "Bts"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Bts"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRoles(CommonConstants.Data_CanDelete_Role)]
        public ActionResult Delete(string id = "0")
        {
            try
            {
                Bts dbItem = _btsService.getByID(id);
                if (dbItem == null)
                {
                    return HttpNotFound();
                }

                //if (_btsService.IsUsed(id))
                //{
                //    return Json(new {resetUrl = Url.Action("Add", "Bts"), status = CommonConstants.Status_Error, message = "Không thể xóa Trường hợp kiểm định này do đã được sử dụnd" }, JsonRequestBehavior.AllowGet);
                //}

                _btsService.Delete(id);
                _btsService.SaveChanges();

                return Json(new { resetUrl = Url.Action("Add", "Bts"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Bts"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}