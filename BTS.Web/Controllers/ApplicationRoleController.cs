using AutoMapper;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.App_Start;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BTS.Web.Infrastructure.Extensions;

namespace BTS.Web.Controllers
{
    public class ApplicationRoleController : BaseController
    {
        private IApplicationRoleService _appRoleService;
        private ApplicationUserManager _userManager;

        public ApplicationRoleController(IErrorService errorService,
            IApplicationRoleService appRoleService,
            ApplicationUserManager userManager,
            IApplicationRoleService appGroupService) : base(errorService)
        {
            _appRoleService = appRoleService;
            _userManager = userManager;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAll());
        }

        IEnumerable<ApplicationRoleViewModel> GetAll()
        {
            var model = _appRoleService.GetAll();
            return Mapper.Map<IEnumerable<ApplicationRoleViewModel>>(model);
        }

        public ActionResult AddOrEdit(string id = "")
        {
            ApplicationRoleViewModel ItemVm = new ApplicationRoleViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                var DbItem = _appRoleService.GetDetail(id);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<ApplicationRoleViewModel>(DbItem);
                }

            }
            return View(ItemVm);
        }


        [HttpPost]
        public async Task<ActionResult> AddOrEdit(ApplicationRoleViewModel Item)
        {
            try
            {
                if (string.IsNullOrEmpty(Item.ID))
                {
                    ApplicationRole newItem = new ApplicationRole();
                    newItem.UpdateApplicationRole(Item);
                    
                    _appRoleService.Add(newItem);
                    _appRoleService.Save();

                    return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    var editItem = _appRoleService.GetDetail(Item.ID);

                    editItem.UpdateApplicationRole(Item,"update");
                    _appRoleService.Update(editItem);
                    _appRoleService.Save();      
                    return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                _appRoleService.Delete(id);
                _appRoleService.Save();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}