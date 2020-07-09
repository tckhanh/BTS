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

namespace BTS.Web.Areas.Intranet.Controllers
{
    [AuthorizeRoles(CommonConstants.Data_CanView_Role)]
    public class CityController : BaseController
    {
        private ICityService _cityService;

        public CityController(IErrorService errorService, ICityService cityService) : base(errorService)
        {
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

        private IEnumerable<CityViewModel> GetAll()
        {
            List<City> model = _cityService.getAll().ToList();
            return Mapper.Map<IEnumerable<CityViewModel>>(model);
        }

        public ActionResult Add()
        {
            CityViewModel ItemVm = new CityViewModel();
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanViewDetail_Role)]
        public ActionResult Detail(string id = "0")
        {
            CityViewModel ItemVm = new CityViewModel();
            City DbItem = _cityService.getByID(id);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<CityViewModel>(DbItem);
            }
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanViewDetail_Role)]
        public ActionResult Edit(string id = "0")
        {
            CityViewModel ItemVm = new CityViewModel();
            City DbItem = _cityService.getByID(id);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<CityViewModel>(DbItem);
            }
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, string id = "0")
        {
            CityViewModel ItemVm = new CityViewModel();
            if ((act == CommonConstants.Action_Detail || act == CommonConstants.Action_Edit) && !string.IsNullOrEmpty(id))
            {
                City DbItem = _cityService.getByID(id);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<CityViewModel>(DbItem);
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
        public ActionResult Add(CityViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    City newItem = new City();
                    newItem.UpdateCity(Item);

                    newItem.CreatedBy = User.Identity.Name;
                    newItem.CreatedDate = DateTime.Now;

                    _cityService.Add(newItem);
                    _cityService.Save();
                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<CityViewModel>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult Edit(CityViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    City editItem = _cityService.getByID(Item.Id);
                    editItem.UpdateCity(Item);
                    editItem.UpdatedBy = User.Identity.Name;
                    editItem.UpdatedDate = DateTime.Now;

                    _cityService.Update(editItem);
                    _cityService.Save();
                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<CityViewModel>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult AddOrEdit(string act, CityViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (act == CommonConstants.Action_Add)
                    {
                        City newItem = new City();
                        newItem.UpdateCity(Item);

                        newItem.CreatedBy = User.Identity.Name;
                        newItem.CreatedDate = DateTime.Now;

                        _cityService.Add(newItem);
                        _cityService.Save();
                        return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<CityViewModel>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        City editItem = _cityService.getByID(Item.Id);
                        editItem.UpdateCity(Item);
                        editItem.UpdatedBy = User.Identity.Name;
                        editItem.UpdatedDate = DateTime.Now;

                        _cityService.Update(editItem);
                        _cityService.Save();
                        return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<CityViewModel>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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
                City dbItem = _cityService.getByID(id);
                if (dbItem == null)
                {
                    return HttpNotFound();
                }

                if (_cityService.IsUsed(id))
                {
                    return Json(new { status = CommonConstants.Status_Error, message = "Không thể xóa Trường hợp kiểm định này do đã được sử dụnd" }, JsonRequestBehavior.AllowGet);
                }

                _cityService.Delete(id);
                _cityService.Save();

                return Json(new { data_restUrl = "/City/Add", status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}