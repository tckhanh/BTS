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
    public class ErrorController : BaseController
    {
        private IErrorService _errorService;

        public ErrorController(IErrorService errorService, IErrorService errService) : base(errorService)
        {
            _errorService = errService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAll());
        }

        private IEnumerable<ErrorVM> GetAll()
        {
            List<Error> model = _errorService.getAll().ToList();
            return Mapper.Map<IEnumerable<ErrorVM>>(model);
        }

        public ActionResult Add()
        {
            ErrorVM ItemVm = new ErrorVM();
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanViewDetail_Role)]
        public ActionResult Detail(string id = "0")
        {
            int ID = Convert.ToInt32(id);
            ErrorVM ItemVm = new ErrorVM();
            Error DbItem = _errorService.getByID(ID);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<ErrorVM>(DbItem);
            }
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(string id = "0")
        {
            int ID = Convert.ToInt32(id);
            ErrorVM ItemVm = new ErrorVM();
            Error DbItem = _errorService.getByID(ID);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<ErrorVM>(DbItem);
            }
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, string id = "0")
        {
            ErrorVM ItemVm = new ErrorVM();
            if ((act == CommonConstants.Action_Detail || act == CommonConstants.Action_Edit) && !string.IsNullOrEmpty(id))
            {
                Error DbItem = _errorService.getByID(id);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<ErrorVM>(DbItem);
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
        public ActionResult Add(ErrorVM ItemVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Error  newItem = new Error();
                    newItem.UpdateError(ItemVm);
                    newItem.Id = ItemVm.Id;

                    //newItem.CreatedBy = User.Identity.Name;
                    //newItem.CreatedDate = DateTime.Now;

                    _errorService.Add(newItem);
                    _errorService.Save();
                    return Json(new { resetUrl = Url.Action("Add", "Error"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ErrorVM>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "Error"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Error"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(ErrorVM Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Error editItem = _errorService.getByID(Item.Id);
                    editItem.UpdateError(Item);

                    //editItem.UpdatedBy = User.Identity.Name;
                    //editItem.UpdatedDate = DateTime.Now;

                    _errorService.Update(editItem);
                    _errorService.Save();
                    return Json(new { resetUrl = Url.Action("Add", "Error"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ErrorVM>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "Error"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Error"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, ErrorVM Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (act == CommonConstants.Action_Add)
                    {
                        Error newItem = new Error();
                        newItem.UpdateError(Item);

                        //newItem.CreatedBy = User.Identity.Name;
                        //newItem.CreatedDate = DateTime.Now;

                        _errorService.Add(newItem);
                        _errorService.Save();
                        return Json(new { resetUrl = Url.Action("Add", "Error"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ErrorVM>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Error editItem = _errorService.getByID(Item.Id);
                        editItem.UpdateError(Item);

                        //editItem.UpdatedBy = User.Identity.Name;
                        //editItem.UpdatedDate = DateTime.Now;

                        _errorService.Update(editItem);
                        _errorService.Save();
                        return Json(new { resetUrl = Url.Action("Add", "Error"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ErrorVM>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "Error"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Error"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRoles(CommonConstants.Data_CanDelete_Role)]
        public async Task<ActionResult> Delete(string id = "0")
        {
            int ID = Convert.ToInt32(id);
            try
            {
                Error dbItem = _errorService.getByID(ID);
                if (dbItem == null)
                {
                    return HttpNotFound();
                }

                //if (_errorService.IsUsed(id))
                //{
                //    return Json(new { resetUrl = Url.Action("Add", "Error"), status = CommonConstants.Status_Error, message = "Không thể xóa Trường hợp kiểm định này do đã được sử dụnd" }, JsonRequestBehavior.AllowGet);
                //}

                _errorService.Delete(ID);
                _errorService.Save();

                return Json(new { resetUrl = Url.Action("Add", "Error"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Error"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}