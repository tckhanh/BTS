using AutoMapper;
using BTS.Common;
using BTS.Data.ApplicationModels;
using BTS.Service;
using BTS.Web.Infrastructure.Extensions;
using BTS.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BTS.Web.Areas.Admin.Controllers
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


        public ActionResult Add()
        {
            ApplicationRoleViewModel ItemVm = new ApplicationRoleViewModel();
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.System_CanViewDetail_Role)]
        public async Task<ActionResult> Detail(string id = "")
        {
            ApplicationRoleViewModel ItemVm = new ApplicationRoleViewModel();
            ApplicationRole DbItem = await RoleManager.FindByIdAsync(id);

            if (DbItem == null)
            {
                return HttpNotFound();
            }
            else
            {
                ItemVm = Mapper.Map<ApplicationRoleViewModel>(DbItem);

                List<ApplicationGroup> allGroup = _appGroupService.GetAll().ToList();
                List<ApplicationGroup> listGroup = _appGroupService.GetGroupsByRoleId(id).ToList();
                foreach (ApplicationGroup groupItem in allGroup)
                {
                    SelectListItem listItem = new SelectListItem()
                    {
                        Text = groupItem.Description,
                        Value = groupItem.Id,
                        Selected = listGroup.Any(g => g.Id == groupItem.Id)
                    };
                    ItemVm.GroupList.Add(listItem);
                }

                List<ApplicationUser> allUser = UserManager.Users.ToList();
                foreach (ApplicationUser userItem in allUser)
                {
                    if (await UserManager.IsInRoleAsync(userItem.Id, ItemVm.Name))
                    {
                        SelectListItem listItem = new SelectListItem()
                        {
                            Text = userItem.FullName,
                            Value = userItem.Id,
                            Selected = await UserManager.IsInRoleAsync(userItem.Id, ItemVm.Name)
                        };
                        ItemVm.UserList.Add(listItem);
                    }
                }
                return View(ItemVm);
            }
        }

        [AuthorizeRoles(CommonConstants.System_CanEdit_Role)]
        public async Task<ActionResult> Edit(string id = "")
        {
            ApplicationRoleViewModel ItemVm = new ApplicationRoleViewModel();
            ApplicationRole DbItem = await RoleManager.FindByIdAsync(id);

            if (DbItem == null)
            {
                return HttpNotFound();
            }
            else
            {
                ItemVm = Mapper.Map<ApplicationRoleViewModel>(DbItem);

                List<ApplicationGroup> allGroup = _appGroupService.GetAll().ToList();
                List<ApplicationGroup> listGroup = _appGroupService.GetGroupsByRoleId(id).ToList();
                foreach (ApplicationGroup groupItem in allGroup)
                {
                    SelectListItem listItem = new SelectListItem()
                    {
                        Text = groupItem.Description,
                        Value = groupItem.Id,
                        Selected = listGroup.Any(g => g.Id == groupItem.Id)
                    };
                    ItemVm.GroupList.Add(listItem);
                }

                List<ApplicationUser> allUser = UserManager.Users.ToList();
                foreach (ApplicationUser userItem in allUser)
                {
                    if (await UserManager.IsInRoleAsync(userItem.Id, ItemVm.Name))
                    {
                        SelectListItem listItem = new SelectListItem()
                        {
                            Text = userItem.FullName,
                            Value = userItem.Id,
                            Selected = await UserManager.IsInRoleAsync(userItem.Id, ItemVm.Name)
                        };
                        ItemVm.UserList.Add(listItem);
                    }
                }
                return View(ItemVm);
            }
        }

        [AuthorizeRoles(CommonConstants.System_CanAdd_Role, CommonConstants.System_CanViewDetail_Role, CommonConstants.System_CanEdit_Role)]
        public async Task<ActionResult> AddOrEdit(string act, string id = "")
        {
            ApplicationRoleViewModel ItemVm = new ApplicationRoleViewModel();
            if ((act == CommonConstants.Action_Detail || act == CommonConstants.Action_Edit) && !string.IsNullOrEmpty(id))
            {
                ApplicationRole DbItem = await RoleManager.FindByIdAsync(id);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<ApplicationRoleViewModel>(DbItem);

                    List<ApplicationGroup> allGroup = _appGroupService.GetAll().ToList();
                    List<ApplicationGroup> listGroup = _appGroupService.GetGroupsByRoleId(id).ToList();
                    foreach (ApplicationGroup groupItem in allGroup)
                    {
                        SelectListItem listItem = new SelectListItem()
                        {
                            Text = groupItem.Description,
                            Value = groupItem.Id,
                            Selected = listGroup.Any(g => g.Id == groupItem.Id)
                        };
                        ItemVm.GroupList.Add(listItem);
                    }

                    List<ApplicationUser> allUser = UserManager.Users.ToList();
                    foreach (ApplicationUser userItem in allUser)
                    {
                        if (await UserManager.IsInRoleAsync(userItem.Id, ItemVm.Name))
                        {
                            SelectListItem listItem = new SelectListItem()
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
        public async Task<ActionResult> Add(ApplicationRoleViewModel Item, params string[] selectedGroupItems)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationRole appRole;

                    appRole = new ApplicationRole(Item.Name, Item.Description)
                    {
                        CreatedBy = User.Identity.Name,
                        CreatedDate = DateTime.Now
                    };

                    IdentityResult roleresult = await RoleManager.CreateAsync(appRole);
                    if (!roleresult.Succeeded)
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = roleresult.Errors.First() }, JsonRequestBehavior.AllowGet);
                    }

                    UpdateFromRoleToGroupUser(appRole, selectedGroupItems);

                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ApplicationRoleViewModel>>(RoleManager.Roles)), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> Edit(ApplicationRoleViewModel Item, params string[] selectedGroupItems)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationRole appRole;
                    appRole = await RoleManager.FindByIdAsync(Item.Id);
                    appRole.UpdateApplicationRole(Item, "update");
                    appRole.UpdatedBy = User.Identity.Name;
                    appRole.UpdatedDate = DateTime.Now;
                    
                    IdentityResult roleresult = await RoleManager.UpdateAsync(appRole);
                    if (!roleresult.Succeeded)
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = roleresult.Errors.First() }, JsonRequestBehavior.AllowGet);
                    }

                    UpdateFromRoleToGroupUser(appRole, selectedGroupItems);
                    
                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ApplicationRoleViewModel>>(RoleManager.Roles)), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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

        private async void UpdateFromRoleToGroupUser(ApplicationRole appRole, string[] selectedGroupItems)
        {
            //delete ApplicationRoleGroup
            List<string> oldUserIds = _appGroupService.GetLogicUsersByRoleId(appRole.Id).Select(x => x.UserId).ToList();

            _appGroupService.DeleteRoleFromGroups(appRole.Id);
            _appGroupService.Save();

            //add ApplicationRoleGroup
            List<ApplicationRoleGroup> listRoleGroup = new List<ApplicationRoleGroup>();
            selectedGroupItems = selectedGroupItems ?? new string[] { };
            foreach (string appGroup in selectedGroupItems)
            {
                listRoleGroup.Add(new ApplicationRoleGroup()
                {
                    GroupId = appGroup,
                    RoleId = appRole.Id,
                    CreatedBy = User.Identity.Name,
                    CreatedDate = DateTime.Now
                });
            }
            _appGroupService.AddRoleGroups(listRoleGroup);
            _appGroupService.Save();

            foreach (string userIdItem in oldUserIds)
            {
                await removeUserRole(userIdItem, appRole.Name);
            }

            ICollection<ApplicationUser> users = _appGroupService.GetUsersByGroupIds(selectedGroupItems);
            foreach (ApplicationUser userItem in users)
            {
                await addUserRole(userItem.Id, appRole.Name);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.System_CanAdd_Role, CommonConstants.System_CanEdit_Role)]
        public async Task<ActionResult> AddOrEdit(string act, ApplicationRoleViewModel Item, params string[] selectedGroupItems)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationRole appRole;

                    if (act == CommonConstants.Action_Add)
                    {
                        appRole = new ApplicationRole(Item.Name, Item.Description)
                        {
                            CreatedBy = User.Identity.Name,
                            CreatedDate = DateTime.Now
                        };

                        IdentityResult roleresult = await RoleManager.CreateAsync(appRole);
                        if (!roleresult.Succeeded)
                        {
                            return Json(new { status = CommonConstants.Status_Error, message = roleresult.Errors.First() }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        appRole = await RoleManager.FindByIdAsync(Item.Id);
                        appRole.UpdateApplicationRole(Item, "update");
                        appRole.UpdatedBy = User.Identity.Name;
                        appRole.UpdatedDate = DateTime.Now;

                        await RoleManager.UpdateAsync(appRole);
                    }

                    //delete ApplicationRoleGroup
                    List<string> oldUserIds = _appGroupService.GetLogicUsersByRoleId(appRole.Id).Select(x => x.UserId).ToList();

                    _appGroupService.DeleteRoleFromGroups(appRole.Id);
                    _appGroupService.Save();

                    //add ApplicationRoleGroup
                    List<ApplicationRoleGroup> listRoleGroup = new List<ApplicationRoleGroup>();
                    selectedGroupItems = selectedGroupItems ?? new string[] { };
                    foreach (string appGroup in selectedGroupItems)
                    {
                        listRoleGroup.Add(new ApplicationRoleGroup()
                        {
                            GroupId = appGroup,
                            RoleId = appRole.Id,
                            CreatedBy = User.Identity.Name,
                            CreatedDate = DateTime.Now
                        });
                    }
                    _appGroupService.AddRoleGroups(listRoleGroup);
                    _appGroupService.Save();

                    foreach (string userIdItem in oldUserIds)
                    {
                        await removeUserRole(userIdItem, appRole.Name);
                    }

                    ICollection<ApplicationUser> users = _appGroupService.GetUsersByGroupIds(selectedGroupItems);
                    foreach (ApplicationUser userItem in users)
                    {
                        await addUserRole(userItem.Id, appRole.Name);
                    }
                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ApplicationRoleViewModel>>(RoleManager.Roles)), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                ApplicationRole role = await RoleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return HttpNotFound();
                }

                if (_appGroupService.GetGroupsByRoleId(id) != null)
                {
                    return Json(new { status = CommonConstants.Status_Error, message = "Không thể xóa Quyền đã cấp cho Nhóm người dùng" }, JsonRequestBehavior.AllowGet);
                }

                IdentityResult result = await RoleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return Json(new { data_restUrl = "/ApplicationRole/Add", status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", RoleManager.Roles), message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = CommonConstants.Status_Error, message = "Xóa dữ liệu không thành công" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}