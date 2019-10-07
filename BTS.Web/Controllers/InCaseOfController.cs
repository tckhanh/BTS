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
    public class InCaseOfController : BaseController
    {
        private readonly IInCaseOfService _inCaseOfService;

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
            List<InCaseOf> model = _inCaseOfService.getAll().ToList();
            return Mapper.Map<IEnumerable<InCaseOfViewModel>>(model);
        }


        public ActionResult Add()
        {
            InCaseOfViewModel ItemVm = new InCaseOfViewModel();
            return View("Add", ItemVm);
        }


        [AuthorizeRoles(CommonConstants.Data_CanViewDetail_Role)]
        public ActionResult Detail(string id = "0")
        {
            int ID = Convert.ToInt32(id);
            InCaseOfViewModel ItemVm = new InCaseOfViewModel();
            InCaseOf DbItem = _inCaseOfService.getByID(ID);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<InCaseOfViewModel>(DbItem);
            }
            return View("Detail", ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(string id = "0")
        {
            int ID = Convert.ToInt32(id);
            InCaseOfViewModel ItemVm = new InCaseOfViewModel();
            InCaseOf DbItem = _inCaseOfService.getByID(ID);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<InCaseOfViewModel>(DbItem);
            }
            return View("Edit", ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, string id = "0")
        {
            int ID = Convert.ToInt32(id);
            InCaseOfViewModel ItemVm = new InCaseOfViewModel();
            if ((act == CommonConstants.Action_Detail || act == CommonConstants.Action_Edit) && !string.IsNullOrEmpty(id))
            {
                InCaseOf DbItem = _inCaseOfService.getByID(ID);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<InCaseOfViewModel>(DbItem);
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
        public ActionResult Add(InCaseOfViewModel ItemVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    InCaseOf newItem = new InCaseOf();
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
                    return Json(new { status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x=> x.ErrorMessage)}, JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> Edit(InCaseOfViewModel ItemVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    InCaseOf editItem = _inCaseOfService.getByID(ItemVm.Id);
                    editItem.UpdateInCaseOf(ItemVm);
                    editItem.UpdatedBy = User.Identity.Name;
                    editItem.UpdatedDate = DateTime.Now;

                    _inCaseOfService.Update(editItem);
                    _inCaseOfService.Save();
                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<InCaseOfViewModel>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> AddOrEdit(string act, InCaseOfViewModel ItemVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (act == CommonConstants.Action_Add)
                    {
                        InCaseOf newItem = new InCaseOf();
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
                        InCaseOf editItem = _inCaseOfService.getByID(ItemVm.Id);
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
            int ID = Convert.ToInt32(id);
            try
            {
                InCaseOf dbItem = _inCaseOfService.getByID(ID);
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

                return Json(new { data_restUrl = "/InCaseOf/Add", status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}