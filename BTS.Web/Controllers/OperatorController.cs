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
    public class OperatorController : BaseController
    {
        private IOperatorService _operatorService;

        public OperatorController(IErrorService errorService, IOperatorService operatorService) : base(errorService)
        {
            _operatorService = operatorService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAll());
        }

        private IEnumerable<OperatorViewModel> GetAll()
        {
            List<Operator> model = _operatorService.getAll().ToList();
            return Mapper.Map<IEnumerable<OperatorViewModel>>(model);
        }

        public ActionResult Add()
        {
            OperatorViewModel ItemVm = new OperatorViewModel();
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanViewDetail_Role)]
        public ActionResult Detail(string id = "0")
        {
            OperatorViewModel ItemVm = new OperatorViewModel();
            Operator DbItem = _operatorService.getByID(id);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<OperatorViewModel>(DbItem);
            }
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(string id = "0")
        {
            OperatorViewModel ItemVm = new OperatorViewModel();
            Operator DbItem = _operatorService.getByID(id);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<OperatorViewModel>(DbItem);
            }
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanEdit_Role)]
        public async Task<ActionResult> AddOrEdit(string act, string id = "0")
        {
            OperatorViewModel ItemVm = new OperatorViewModel();
            if ((act == CommonConstants.Action_Detail || act == CommonConstants.Action_Edit) && !string.IsNullOrEmpty(id))
            {
                Operator DbItem = _operatorService.getByID(id);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<OperatorViewModel>(DbItem);
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
        public ActionResult Add(OperatorViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Operator newItem = new Operator();
                    newItem.UpdateOperator(Item);

                    newItem.CreatedBy = User.Identity.Name;
                    newItem.CreatedDate = DateTime.Now;

                    _operatorService.Add(newItem);
                    _operatorService.SaveChanges();
                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<OperatorViewModel>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult Edit(OperatorViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Operator editItem = _operatorService.getByID(Item.Id);
                    editItem.UpdateOperator(Item);
                    editItem.UpdatedBy = User.Identity.Name;
                    editItem.UpdatedDate = DateTime.Now;

                    _operatorService.Update(editItem);
                    _operatorService.SaveChanges();
                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<OperatorViewModel>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> AddOrEdit(string act, OperatorViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (act == CommonConstants.Action_Add)
                    {
                        Operator newItem = new Operator();
                        newItem.UpdateOperator(Item);

                        newItem.CreatedBy = User.Identity.Name;
                        newItem.CreatedDate = DateTime.Now;

                        _operatorService.Add(newItem);
                        _operatorService.SaveChanges();
                        return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<OperatorViewModel>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Operator editItem = _operatorService.getByID(Item.Id);
                        editItem.UpdateOperator(Item);
                        editItem.UpdatedBy = User.Identity.Name;
                        editItem.UpdatedDate = DateTime.Now;

                        _operatorService.Update(editItem);
                        _operatorService.SaveChanges();
                        return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<OperatorViewModel>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> Delete(string id = "0")
        {
            try
            {
                Operator dbItem = _operatorService.getByID(id);
                if (dbItem == null)
                {
                    return HttpNotFound();
                }

                if (_operatorService.IsUsed(id))
                {
                    return Json(new { status = CommonConstants.Status_Error, message = "Không thể xóa Trường hợp kiểm định này do đã được sử dụnd" }, JsonRequestBehavior.AllowGet);
                }

                _operatorService.Delete(id);
                _operatorService.SaveChanges();

                return Json(new { data_restUrl = "/Operator/Add", status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}