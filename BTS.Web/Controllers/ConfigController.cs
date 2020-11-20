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
    public class ConfigController : BaseController
    {
        private IConfigService _configService;

        public ConfigController(IErrorService errorService, IConfigService configService) : base(errorService)
        {
            _configService = configService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAll());
        }

        private IEnumerable<ConfigVM> GetAll()
        {
            List<SystemConfig> model = _configService.getAll().ToList();
            return Mapper.Map<IEnumerable<ConfigVM>>(model);
        }

        public ActionResult Add()
        {
            ConfigVM ItemVm = new ConfigVM();
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanViewDetail_Role)]
        public ActionResult Detail(string id = "0")
        {
            int ID = Convert.ToInt32(id);
            ConfigVM ItemVm = new ConfigVM();
            SystemConfig DbItem = _configService.getByID(ID);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<ConfigVM>(DbItem);
            }
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(string id = "0")
        {
            int ID = Convert.ToInt32(id);
            ConfigVM ItemVm = new ConfigVM();
            SystemConfig DbItem = _configService.getByID(ID);
            if (DbItem != null)
            {
                ItemVm = Mapper.Map<ConfigVM>(DbItem);
            }
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, string id = "0")
        {
            ConfigVM ItemVm = new ConfigVM();
            if ((act == CommonConstants.Action_Detail || act == CommonConstants.Action_Edit) && !string.IsNullOrEmpty(id))
            {
                SystemConfig DbItem = _configService.getByID(id);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<ConfigVM>(DbItem);
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
        public ActionResult Add(ConfigVM ItemVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SystemConfig  newItem = new SystemConfig();
                    newItem.UpdateConfig(ItemVm);
                    newItem.Id = ItemVm.Id;

                    //newItem.CreatedBy = User.Identity.Name;
                    //newItem.CreatedDate = DateTime.Now;

                    _configService.Add(newItem);
                    _configService.Save();
                    return Json(new { resetUrl = Url.Action("Add", "Config"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ConfigVM>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "Config"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Config"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(ConfigVM Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SystemConfig editItem = _configService.getByID(Item.Id);
                    editItem.UpdateConfig(Item);

                    //editItem.UpdatedBy = User.Identity.Name;
                    //editItem.UpdatedDate = DateTime.Now;

                    _configService.Update(editItem);
                    _configService.Save();
                    return Json(new { resetUrl = Url.Action("Add", "Config"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ConfigVM>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "Config"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Config"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, ConfigVM Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (act == CommonConstants.Action_Add)
                    {
                        SystemConfig newItem = new SystemConfig();
                        newItem.UpdateConfig(Item);

                        //newItem.CreatedBy = User.Identity.Name;
                        //newItem.CreatedDate = DateTime.Now;

                        _configService.Add(newItem);
                        _configService.Save();
                        return Json(new { resetUrl = Url.Action("Add", "Config"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ConfigVM>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        SystemConfig editItem = _configService.getByID(Item.Id);
                        editItem.UpdateConfig(Item);

                        //editItem.UpdatedBy = User.Identity.Name;
                        //editItem.UpdatedDate = DateTime.Now;

                        _configService.Update(editItem);
                        _configService.Save();
                        return Json(new { resetUrl = Url.Action("Add", "Config"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ConfigVM>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "Config"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Config"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRoles(CommonConstants.Data_CanDelete_Role)]
        public async Task<ActionResult> Delete(string id = "0")
        {
            int ID = Convert.ToInt32(id);
            try
            {
                SystemConfig dbItem = _configService.getByID(ID);
                if (dbItem == null)
                {
                    return HttpNotFound();
                }

                if (_configService.IsUsed(ID))
                {
                    return Json(new { resetUrl = Url.Action("Add", "Config"), status = CommonConstants.Status_Error, message = "Không thể xóa Thông số này do đã được sử dụnd" }, JsonRequestBehavior.AllowGet);
                }

                _configService.Delete(ID);
                _configService.Save();

                return Json(new { resetUrl = Url.Action("Add", "Config"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Config"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}