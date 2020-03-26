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
    public class WardController : BaseController
    {
        private IWardService _WardService;
        private IDistrictService _districtService;

        public WardController(IErrorService errorService, IWardService wardService, IDistrictService districtService) : base(errorService)
        {
            _WardService = wardService;
            _districtService = districtService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAll());
        }

        private IEnumerable<WardVM> GetAll()
        {
            List<Ward> model = _WardService.getAll(new string[] { "District" }).OrderBy(x=> x.District.CityId + x.District.Name + x.Name).ToList();
            return Mapper.Map<IEnumerable<WardVM>>(model);            
        }

        private WardVM FillInWardVM(WardVM ItemVm)
        {            

            IEnumerable<District> districtList = _districtService.getAll().OrderBy(x=> x.CityId + x.Name).ToList();
            foreach (District districtItem in districtList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = districtItem.CityId + "-" + districtItem.Name,
                    Value = districtItem.Id,
                    Selected = false
                };
                ItemVm.DistrictList.Add(listItem);
            }           
            return ItemVm;
        }

        public ActionResult Add()
        {
            WardVM ItemVm = new WardVM();
            ItemVm = FillInWardVM(ItemVm);
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanViewDetail_Role)]
        public ActionResult Detail(string id = "0")
        {
            WardVM ItemVm = new WardVM();
            Ward DbItem = _WardService.getByID(id);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<WardVM>(DbItem);
                ItemVm = FillInWardVM(ItemVm);
            }
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanViewDetail_Role)]
        public ActionResult Edit(string id = "0")
        {
            WardVM ItemVm = new WardVM();
            Ward DbItem = _WardService.getByID(id);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<WardVM>(DbItem);
            }
            ItemVm = FillInWardVM(ItemVm);
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, string id = "0")
        {
            WardVM ItemVm = new WardVM();
            if ((act == CommonConstants.Action_Detail || act == CommonConstants.Action_Edit) && !string.IsNullOrEmpty(id))
            {
                Ward DbItem = _WardService.getByID(id);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<WardVM>(DbItem);
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
        public ActionResult Add(WardVM Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Ward newItem = new Ward();
                    newItem.UpdateWard(Item);

                    newItem.CreatedBy = User.Identity.Name;
                    newItem.CreatedDate = DateTime.Now;

                    _WardService.Add(newItem);
                    _WardService.Save();
                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<WardVM>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult Edit(WardVM Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Ward editItem = _WardService.getByID(Item.Id);
                    editItem.UpdateWard(Item);
                    editItem.UpdatedBy = User.Identity.Name;
                    editItem.UpdatedDate = DateTime.Now;

                    _WardService.Update(editItem);
                    _WardService.Save();
                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<WardVM>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult AddOrEdit(string act, WardVM Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (act == CommonConstants.Action_Add)
                    {
                        Ward newItem = new Ward();
                        newItem.UpdateWard(Item);

                        newItem.CreatedBy = User.Identity.Name;
                        newItem.CreatedDate = DateTime.Now;

                        _WardService.Add(newItem);
                        _WardService.Save();
                        return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<WardVM>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Ward editItem = _WardService.getByID(Item.Id);
                        editItem.UpdateWard(Item);
                        editItem.UpdatedBy = User.Identity.Name;
                        editItem.UpdatedDate = DateTime.Now;

                        _WardService.Update(editItem);
                        _WardService.Save();
                        return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<WardVM>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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
                Ward dbItem = _WardService.getByID(id);
                if (dbItem == null)
                {
                    return HttpNotFound();
                }

                if (_WardService.IsUsed(id))
                {
                    return Json(new { status = CommonConstants.Status_Error, message = "Không thể xóa Trường hợp kiểm định này do đã được sử dụnd" }, JsonRequestBehavior.AllowGet);
                }

                _WardService.Delete(id);
                _WardService.Save();

                return Json(new { data_restUrl = "/Ward/Add", status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}