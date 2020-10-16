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
    public class DistrictController : BaseController
    {
        private IDistrictService _DistrictService;
        private ICityService _cityService;

        public DistrictController(IErrorService errorService, IDistrictService districtService, ICityService cityService) : base(errorService)
        {
            _DistrictService = districtService;
            _cityService = cityService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAll());
        }

        private IEnumerable<DistrictVM> GetAll()
        {
            List<District> model = _DistrictService.getAll(new string[] { "City" }).OrderBy(x=> x.CityId + x.Name).ToList();
            return Mapper.Map<IEnumerable<DistrictVM>>(model);
        }

        private DistrictVM FillInDistrictVM(DistrictVM ItemVm)
        {            

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
            return ItemVm;
        }

        public ActionResult Add()
        {
            DistrictVM ItemVm = new DistrictVM();
            ItemVm = FillInDistrictVM(ItemVm);
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanViewDetail_Role)]
        public ActionResult Detail(string id = "0")
        {
            DistrictVM ItemVm = new DistrictVM();
            District DbItem = _DistrictService.getByID(id);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<DistrictVM>(DbItem);
                ItemVm = FillInDistrictVM(ItemVm);
            }
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanViewDetail_Role)]
        public ActionResult Edit(string id = "0")
        {
            DistrictVM ItemVm = new DistrictVM();
            District DbItem = _DistrictService.getByID(id);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<DistrictVM>(DbItem);
            }
            ItemVm = FillInDistrictVM(ItemVm);
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, string id = "0")
        {
            DistrictVM ItemVm = new DistrictVM();
            if ((act == CommonConstants.Action_Detail || act == CommonConstants.Action_Edit) && !string.IsNullOrEmpty(id))
            {
                District DbItem = _DistrictService.getByID(id);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<DistrictVM>(DbItem);
                }
                if (act == CommonConstants.Action_Edit)
                {
                    return View("Edit", ItemVm);
                }
                else
                {
                    return View("Detail", ItemVm);
                }
            }
            else
            {
                return View("Add", ItemVm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role)]
        public ActionResult Add(DistrictVM Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    District newItem = new District();
                    newItem.UpdateDistrict(Item);

                    newItem.CreatedBy = User.Identity.Name;
                    newItem.CreatedDate = DateTime.Now;

                    _DistrictService.Add(newItem);
                    _DistrictService.Save();
                    return Json(new { resetUrl = Url.Action("Add", "District"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<DistrictVM>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "District"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "District"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(DistrictVM Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    District editItem = _DistrictService.getByID(Item.Id);
                    editItem.UpdateDistrict(Item);
                    editItem.UpdatedBy = User.Identity.Name;
                    editItem.UpdatedDate = DateTime.Now;

                    _DistrictService.Update(editItem);
                    _DistrictService.Save();
                    return Json(new { resetUrl = Url.Action("Add", "District"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<DistrictVM>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "District"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "District"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, DistrictVM ItemVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (act == CommonConstants.Action_Add)
                    {
                        District newItem = new District();
                        newItem.UpdateDistrict(ItemVm);
                        newItem.Id = ItemVm.Id;

                        newItem.CreatedBy = User.Identity.Name;
                        newItem.CreatedDate = DateTime.Now;

                        _DistrictService.Add(newItem);
                        _DistrictService.Save();
                        return Json(new { resetUrl = Url.Action("Add", "District"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<DistrictVM>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        District editItem = _DistrictService.getByID(ItemVm.Id);
                        editItem.UpdateDistrict(ItemVm);
                        editItem.UpdatedBy = User.Identity.Name;
                        editItem.UpdatedDate = DateTime.Now;

                        _DistrictService.Update(editItem);
                        _DistrictService.Save();
                        return Json(new { resetUrl = Url.Action("Add", "District"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<DistrictVM>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "District"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "District"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRoles(CommonConstants.Data_CanDelete_Role)]
        public ActionResult Delete(string id = "0")
        {
            try
            {
                District dbItem = _DistrictService.getByID(id);
                if (dbItem == null)
                {
                    return HttpNotFound();
                }

                if (_DistrictService.IsUsed(id))
                {
                    return Json(new { resetUrl = Url.Action("Add", "District"), status = CommonConstants.Status_Error, message = "Không thể xóa Trường hợp kiểm định này do đã được sử dụnd" }, JsonRequestBehavior.AllowGet);
                }

                _DistrictService.Delete(id);
                _DistrictService.Save();

                return Json(new { resetUrl = Url.Action("Add", "District"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "District"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}