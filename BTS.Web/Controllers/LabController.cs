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
    public class LabController : BaseController
    {
        private ILabService _labService;

        public LabController(IErrorService errorService, ILabService labService) : base(errorService)
        {
            _labService = labService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAll());
        }

        private IEnumerable<LabViewModel> GetAll()
        {
            List<Lab> model = _labService.getAll().ToList();
            return Mapper.Map<IEnumerable<LabViewModel>>(model);
        }

        public ActionResult Add()
        {
            LabViewModel ItemVm = new LabViewModel();
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanViewDetail_Role)]
        public ActionResult Detail(string id = "0")
        {
            LabViewModel ItemVm = new LabViewModel();
            Lab DbItem = _labService.getByID(id);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<LabViewModel>(DbItem);
            }
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(string id = "0")
        {
            LabViewModel ItemVm = new LabViewModel();
            Lab DbItem = _labService.getByID(id);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<LabViewModel>(DbItem);
            }
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, string id = "0")
        {
            LabViewModel ItemVm = new LabViewModel();
            if ((act == CommonConstants.Action_Detail || act == CommonConstants.Action_Edit) && !string.IsNullOrEmpty(id))
            {
                Lab DbItem = _labService.getByID(id);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<LabViewModel>(DbItem);
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
        public ActionResult Add(LabViewModel ItemVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Lab newItem = new Lab();
                    newItem.UpdateLab(ItemVm);
                    newItem.Id = ItemVm.Id;

                    newItem.CreatedBy = User.Identity.Name;
                    newItem.CreatedDate = DateTime.Now;

                    _labService.Add(newItem);
                    _labService.Save();
                    return Json(new { resetUrl = Url.Action("Add", "Lab"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<LabViewModel>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "Lab"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Lab"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(LabViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Lab editItem = _labService.getByID(Item.Id);
                    editItem.UpdateLab(Item);
                    editItem.UpdatedBy = User.Identity.Name;
                    editItem.UpdatedDate = DateTime.Now;

                    _labService.Update(editItem);
                    _labService.Save();
                    return Json(new { resetUrl = Url.Action("Add", "Lab"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<LabViewModel>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "Lab"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Lab"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, LabViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (act == CommonConstants.Action_Add)
                    {
                        Lab newItem = new Lab();
                        newItem.UpdateLab(Item);

                        newItem.CreatedBy = User.Identity.Name;
                        newItem.CreatedDate = DateTime.Now;

                        _labService.Add(newItem);
                        _labService.Save();
                        return Json(new { resetUrl = Url.Action("Add", "Lab"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<LabViewModel>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Lab editItem = _labService.getByID(Item.Id);
                        editItem.UpdateLab(Item);
                        editItem.UpdatedBy = User.Identity.Name;
                        editItem.UpdatedDate = DateTime.Now;

                        _labService.Update(editItem);
                        _labService.Save();
                        return Json(new { resetUrl = Url.Action("Add", "Lab"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<LabViewModel>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "Lab"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Lab"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRoles(CommonConstants.Data_CanDelete_Role)]
        public async Task<ActionResult> Delete(string id = "0")
        {
            try
            {
                Lab dbItem = _labService.getByID(id);
                if (dbItem == null)
                {
                    return HttpNotFound();
                }

                if (_labService.IsUsed(id))
                {
                    return Json(new { resetUrl = Url.Action("Add", "Lab"), status = CommonConstants.Status_Error, message = "Không thể xóa Trường hợp kiểm định này do đã được sử dụnd" }, JsonRequestBehavior.AllowGet);
                }

                _labService.Delete(id);
                _labService.Save();

                return Json(new { resetUrl = Url.Action("Add", "Lab"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Lab"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}