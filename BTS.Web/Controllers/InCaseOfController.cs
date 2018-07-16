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
    public class InCaseOfController : BaseController
    {
        private IInCaseOfService _inCaseOfService;

        public InCaseOfController(IErrorService errorService, IInCaseOfService inCaseOfService) : base(errorService)
        {
            _inCaseOfService = inCaseOfService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAll());
        }

        private IEnumerable<InCaseOfViewModel> GetAll()
        {
            var model = _inCaseOfService.getAll();
            return Mapper.Map<IEnumerable<InCaseOfViewModel>>(model);
        }

        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanEdit_Role)]
        public async Task<ActionResult> AddOrEdit(string act, string id = "0")
        {
            int ID = Convert.ToInt32(id);
            InCaseOfViewModel ItemVm = new InCaseOfViewModel();
            if ((act == CommonConstants.Action_Detail || act == CommonConstants.Action_Edit) && !string.IsNullOrEmpty(id))
            {
                var DbItem = _inCaseOfService.getByID(ID);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<InCaseOfViewModel>(DbItem);
                }
                if (act == CommonConstants.Action_Edit)
                    return View("Edit", ItemVm);
                else
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
        public async Task<ActionResult> AddOrEdit(string act, InCaseOfViewModel ItemVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (act == CommonConstants.Action_Add)
                    {
                        var newItem = new InCaseOf();
                        newItem.UpdateInCaseOf(ItemVm);
                        newItem.Id = ItemVm.Id;

                        newItem.CreatedBy = User.Identity.Name;
                        newItem.CreatedDate = DateTime.Now;

                        _inCaseOfService.Add(newItem);
                        _inCaseOfService.Save();
                        return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<InCaseOfViewModel>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var editItem = _inCaseOfService.getByID(ItemVm.Id);
                        editItem.UpdateInCaseOf(ItemVm);
                        editItem.UpdatedBy = User.Identity.Name;
                        editItem.UpdatedDate = DateTime.Now;

                        _inCaseOfService.Update(editItem);
                        _inCaseOfService.Save();
                        return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<InCaseOfViewModel>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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
            int ID = Convert.ToInt32(id);
            try
            {
                var dbItem = _inCaseOfService.getByID(ID);
                if (dbItem == null)
                {
                    return HttpNotFound();
                }

                if (_inCaseOfService.IsUsed(ID))
                {
                    return Json(new { status = CommonConstants.Status_Error, message = "Không thể xóa Trường hợp kiểm định này do đã được sử dụnd" }, JsonRequestBehavior.AllowGet);
                }

                _inCaseOfService.Delete(ID);
                _inCaseOfService.Save();

                return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}