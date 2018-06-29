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
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using BTS.Data.ApplicationModels;
using BTS.Common;

namespace BTS.Web.Controllers
{
    [AuthorizeRoles(CommonConstants.System_CanView_Role)]
    public class ApplicationRoleController : BaseController
    {
        private IApplicationGroupService _appGroupService;

        public ApplicationRoleController(IErrorService errorService, IApplicationGroupService appGroupService) : base(errorService)
        {
            _appGroupService = appGroupService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(Mapper.Map<IEnumerable<ApplicationRoleViewModel>>(RoleManager.Roles.OrderByDescending(x => x.Name)));
        }

        [AuthorizeRoles(CommonConstants.System_CanAdd_Role, CommonConstants.System_CanViewDetail_Role, CommonConstants.System_CanEdit_Role)]
        public async Task<ActionResult> AddOrEdit(string act, string id = "")
        {
            ApplicationRoleViewModel ItemVm = new ApplicationRoleViewModel();
            if ((act == CommonConstants.Action_Detail || act == CommonConstants.Action_Edit) && !string.IsNullOrEmpty(id))
            {
                var DbItem = await RoleManager.FindByIdAsync(id);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<ApplicationRoleViewModel>(DbItem);

                    var allGroup = _appGroupService.GetAll();
                    var listGroup = _appGroupService.GetGroupsByRoleId(id);
                    foreach (var groupItem in allGroup)
                    {
                        var listItem = new SelectListItem()
                        {
                            Text = groupItem.Description,
                            Value = groupItem.Id,
                            Selected = listGroup.Any(g => g.Id == groupItem.Id)
                        };
                        ItemVm.GroupList.Add(listItem);
                    }

                    var allUser = UserManager.Users;
                    foreach (var userItem in allUser)
                    {
                        if (await UserManager.IsInRoleAsync(userItem.Id, ItemVm.Name))
                        {
                            var listItem = new SelectListItem()
                            {
                                Text = userItem.FullName,
                                Value = userItem.Id,
                                Selected = await UserManager.IsInRoleAsync(userItem.Id, ItemVm.Name)
                            };
                            ItemVm.UserList.Add(listItem);
                        }
                    }
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

        //public async Task<ActionResult> Detail(string id = "")
        //{
        //    ApplicationRoleViewModel ItemVm = new ApplicationRoleViewModel();
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        var DbItem = await RoleManager.FindByIdAsync(id);

        //        if (DbItem != null)
        //        {
        //            ItemVm = Mapper.Map<ApplicationRoleViewModel>(DbItem);

        //            var allGroup = _appGroupService.GetAll();
        //            var listGroup = _appGroupService.GetGroupsByRoleId(id);
        //            foreach (var groupItem in allGroup)
        //            {
        //                var listItem = new SelectListItem()
        //                {
        //                    Text = groupItem.Name,
        //                    Value = groupItem.Id,
        //                    Selected = listGroup.Any(g => g.Id == groupItem.Id)
        //                };
        //                ItemVm.GroupList.Add(listItem);
        //            }

        //            var allUser = UserManager.Users;
        //            foreach (var userItem in allUser)
        //            {
        //                if (await UserManager.IsInRoleAsync(userItem.Id, ItemVm.Name))
        //                {
        //                    var listItem = new SelectListItem()
        //                    {
        //                        Text = userItem.FullName,
        //                        Value = userItem.Id,
        //                        Selected = await UserManager.IsInRoleAsync(userItem.Id, ItemVm.Name)
        //                    };
        //                    ItemVm.UserList.Add(listItem);
        //                }
        //            }
        //        }
        //    }
        //    return View(ItemVm);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.System_CanAdd_Role, CommonConstants.System_CanEdit_Role)]
        public async Task<ActionResult> AddOrEdit(ApplicationRoleViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(Item.Id))
                    {
                        var role = new ApplicationRole(Item.Name, Item.Description);
                        role.CreatedBy = User.Identity.Name;
                        role.CreatedDate = DateTime.Now;

                        var roleresult = await RoleManager.CreateAsync(role);
                        if (!roleresult.Succeeded)
                        {
                            return Json(new { success = false, message = roleresult.Errors.First() }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ApplicationRoleViewModel>>(RoleManager.Roles)), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var editItem = await RoleManager.FindByIdAsync(Item.Id);
                        editItem.UpdateApplicationRole(Item, "update");
                        editItem.UpdatedBy = User.Identity.Name;
                        editItem.UpdatedDate = DateTime.Now;

                        await RoleManager.UpdateAsync(editItem);
                        return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ApplicationRoleViewModel>>(RoleManager.Roles)), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Lỗi nhập liệu" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRoles(CommonConstants.System_CanDelete_Role)]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var role = await RoleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return HttpNotFound();
                }

                if (_appGroupService.GetGroupsByRoleId(id) != null)
                {
                    return Json(new { success = false, message = "Không thể xóa Quyền đã cấp cho Nhóm người dùng" }, JsonRequestBehavior.AllowGet);
                }

                IdentityResult result = await RoleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", RoleManager.Roles), message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, message = "Xóa dữ liệu không thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}