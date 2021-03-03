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
    public class AuditController : BaseController
    {
        private IAuditService _auditService;

        public AuditController(IAuditService auditService, IErrorService errService) : base(errService)
        {
            _auditService = auditService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAll());
        }

        private IEnumerable<AuditVM> GetAll()
        {
            List<Audit> model = _auditService.getAll().ToList();
            return Mapper.Map<IEnumerable<AuditVM>>(model);
        }

        public ActionResult Add()
        {
            AuditVM ItemVm = new AuditVM();
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanViewDetail_Role)]
        public ActionResult Detail(string id = "0")
        {
            int ID = Convert.ToInt32(id);
            AuditVM ItemVm = new AuditVM();
            Audit DbItem = _auditService.getByID(ID);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<AuditVM>(DbItem);
            }
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(string id = "0")
        {
            int ID = Convert.ToInt32(id);
            AuditVM ItemVm = new AuditVM();
            Audit DbItem = _auditService.getByID(ID);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<AuditVM>(DbItem);
            }
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, string id = "0")
        {
            AuditVM ItemVm = new AuditVM();
            if ((act == CommonConstants.Action_Detail || act == CommonConstants.Action_Edit) && !string.IsNullOrEmpty(id))
            {
                Audit DbItem = _auditService.getByID(id);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<AuditVM>(DbItem);
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
        public ActionResult Add(AuditVM ItemVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Audit  newItem = new Audit();
                    newItem.UpdateAudit(ItemVm);
                    newItem.Id = ItemVm.Id;

                    //newItem.CreatedBy = User.Identity.Name;
                    //newItem.CreatedDate = DateTime.Now;

                    _auditService.Add(newItem);
                    _auditService.Save();
                    return Json(new { resetUrl = Url.Action("Add", "Audit"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<AuditVM>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "Audit"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Audit"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(AuditVM Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Audit editItem = _auditService.getByID(Item.Id);
                    editItem.UpdateAudit(Item);

                    //editItem.UpdatedBy = User.Identity.Name;
                    //editItem.UpdatedDate = DateTime.Now;

                    _auditService.Update(editItem);
                    _auditService.Save();
                    return Json(new { resetUrl = Url.Action("Add", "Audit"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<AuditVM>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "Audit"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
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
        public ActionResult AddOrEdit(string act, AuditVM Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (act == CommonConstants.Action_Add)
                    {
                        Audit newItem = new Audit();
                        newItem.UpdateAudit(Item);

                        //newItem.CreatedBy = User.Identity.Name;
                        //newItem.CreatedDate = DateTime.Now;

                        _auditService.Add(newItem);
                        _auditService.Save();
                        return Json(new { resetUrl = Url.Action("Add", "Audit"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<AuditVM>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Audit editItem = _auditService.getByID(Item.Id);
                        editItem.UpdateAudit(Item);

                        //editItem.UpdatedBy = User.Identity.Name;
                        //editItem.UpdatedDate = DateTime.Now;

                        _auditService.Update(editItem);
                        _auditService.Save();
                        return Json(new { resetUrl = Url.Action("Add", "Audit"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<AuditVM>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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
                Audit dbItem = _auditService.getByID(ID);
                if (dbItem == null)
                {
                    return HttpNotFound();
                }

                //if (_auditService.IsUsed(id))
                //{
                //    return Json(new { resetUrl = Url.Action("Add", "Audit"), status = CommonConstants.Status_Error, message = "Không thể xóa Trường hợp kiểm định này do đã được sử dụnd" }, JsonRequestBehavior.AllowGet);
                //}

                _auditService.Delete(ID);
                _auditService.Save();

                return Json(new { resetUrl = Url.Action("Add", "Audit"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Audit"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}