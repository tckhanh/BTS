using AutoMapper;
using BTS.Common;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Infrastructure.Core;
using BTS.Web.Infrastructure.Extensions;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BTS.Web.Controllers
{
    [AuthorizeRoles(CommonConstants.System_CanView_Role)]
    public class LicenceController : BaseController
    {
        private ILicenceService _licenceService;


        public LicenceController(IErrorService errorService, ILicenceService labService) : base(errorService)
        {
            _licenceService = labService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAll());
        }

        private IEnumerable<LicenceViewModel> GetAll()
        {
            List<Licence> model = _licenceService.getAll().ToList();
            List<LicenceViewModel> viewModel = new List<LicenceViewModel>();
            foreach (var item in model)
            {
                viewModel.Add(checkLicence.GetLicenceInfo(item));
            }
            return viewModel;
        }

        public ActionResult Add()
        {
            LicenceViewModel ItemVm = new LicenceViewModel();
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.System_CanViewDetail_Role)]
        public ActionResult Detail(string id = "0")
        {
            LicenceViewModel ItemVm = new LicenceViewModel();
            Licence DbItem = _licenceService.getByID(id);
            if (DbItem != null)
            {
                ItemVm = checkLicence.GetLicenceInfo(DbItem);                
            }
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.System_CanEdit_Role)]
        public ActionResult Edit(string id = "0")
        {
            LicenceViewModel ItemVm = new LicenceViewModel();
            Licence DbItem = _licenceService.getByID(id);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<LicenceViewModel>(DbItem);
            }
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.System_CanAdd_Role, CommonConstants.System_CanViewDetail_Role, CommonConstants.System_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, string id = "0")
        {
            LicenceViewModel ItemVm = new LicenceViewModel();
            if ((act == CommonConstants.Action_Detail || act == CommonConstants.Action_Edit) && !string.IsNullOrEmpty(id))
            {
                Licence DbItem = _licenceService.getByID(id);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<LicenceViewModel>(DbItem);
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
        [AuthorizeRoles(CommonConstants.System_CanAdd_Role)]
        public ActionResult Add(LicenceViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Licence newItem = new Licence();
                    newItem.UpdateLicence(Item);

                    newItem.CreatedBy = User.Identity.Name;
                    newItem.CreatedDate = DateTime.Now;

                    _licenceService.Add(newItem);
                    _licenceService.Save();
                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<LicenceViewModel>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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
        [AuthorizeRoles(CommonConstants.System_CanEdit_Role)]
        public ActionResult Edit(LicenceViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Licence editItem = _licenceService.getByID(Item.Id);
                    editItem.UpdateLicence(Item);
                    editItem.UpdatedBy = User.Identity.Name;
                    editItem.UpdatedDate = DateTime.Now;

                    _licenceService.Update(editItem);
                    _licenceService.Save();
                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<LicenceViewModel>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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
        [AuthorizeRoles(CommonConstants.System_CanAdd_Role, CommonConstants.System_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, LicenceViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (act == CommonConstants.Action_Add)
                    {
                        Licence newItem = new Licence();
                        newItem.UpdateLicence(Item);

                        newItem.CreatedBy = User.Identity.Name;
                        newItem.CreatedDate = DateTime.Now;

                        _licenceService.Add(newItem);
                        _licenceService.Save();
                        return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<LicenceViewModel>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Licence editItem = _licenceService.getByID(Item.Id);
                        editItem.UpdateLicence(Item);
                        editItem.UpdatedBy = User.Identity.Name;
                        editItem.UpdatedDate = DateTime.Now;

                        _licenceService.Update(editItem);
                        _licenceService.Save();
                        return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<LicenceViewModel>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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

        [AuthorizeRoles(CommonConstants.System_CanDelete_Role)]
        public async Task<ActionResult> Delete(string id = "0")
        {
            try
            {
                Licence dbItem = _licenceService.getByID(id);
                if (dbItem == null)
                {
                    return HttpNotFound();
                }

                if (_licenceService.IsUsed(id))
                {
                    return Json(new { status = CommonConstants.Status_Error, message = "Không thể xóa Trường hợp kiểm định này do đã được sử dụnd" }, JsonRequestBehavior.AllowGet);
                }

                _licenceService.Delete(id);
                _licenceService.Save();

                return Json(new { data_restUrl = "/Licence/Add", status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}