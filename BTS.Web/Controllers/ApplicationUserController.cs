using AutoMapper;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.App_Start;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using BTS.Web.Infrastructure.Extensions;
using System.Threading.Tasks;
using BTS.Common.Exceptions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using static BTS.Web.Models.AccountViewModel;
using BTS.Data.ApplicationModels;
using BTS.Common;
using BTS.Data;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BTS.Web.Controllers
{
    [AuthorizeRoles(CommonConstants.System_CanView_Role)]
    public class ApplicationUserController : BaseController
    {
        private IApplicationGroupService _appGroupService;

        public ApplicationUserController(
            IApplicationGroupService appGroupService,
            IErrorService errorService)
            : base(errorService)
        {
            _appGroupService = appGroupService;
        }

        public ICollection<SelectListItem> ListRolesOfUser(string id = "")
        {
            ICollection<SelectListItem> allRolesOfUser = new List<SelectListItem>();
            IEnumerable<ApplicationRole> listRoles = RoleManager.Roles;
            foreach (var roleItem in listRoles)
            {
                allRolesOfUser.Add(new SelectListItem()
                {
                    Value = roleItem.Id,
                    Text = roleItem.Name,
                    Selected = false
                });
            }

            IEnumerable<ApplicationGroup> listGroupOfUser = _appGroupService.GetGroupsByUserId(id);
            foreach (var groupItem in listGroupOfUser)
            {
                IEnumerable<ApplicationRole> listRoleOfGroup = _appGroupService.GetRolesByGroupId(groupItem.Id);
            }
            return allRolesOfUser;
        }

        public ActionResult Index()
        {
            TempData["ImagePath"] = User.Identity.GetImagePath();
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAll());
        }

        public async Task<ActionResult> DetailRoles(string id = "")
        {
            var user = await UserManager.FindByIdAsync(id);
            var model = new UserPermissionsViewModel(user);
            return View(model);
        }

        private IEnumerable<ApplicationUserViewModel> GetAll()
        {
            var model = UserManager.Users.ToList();
            return Mapper.Map<IEnumerable<ApplicationUserViewModel>>(model);
        }

        [AuthorizeRoles(CommonConstants.System_CanAdd_Role, CommonConstants.System_CanViewDetail_Role, CommonConstants.System_CanEdit_Role)]
        public async Task<ActionResult> AddOrEdit(string act, string id = "")
        {
            ApplicationUserViewModel Item = new ApplicationUserViewModel();
            if ((act == CommonConstants.Action_Detail || act == CommonConstants.Action_Edit || act == CommonConstants.Action_Reset) && !string.IsNullOrEmpty(id))
            {
                var DbItem = await UserManager.FindByIdAsync(id);
                if (DbItem == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    Item = Mapper.Map<ApplicationUserViewModel>(DbItem);

                    var allGroup = _appGroupService.GetAll().ToList();
                    var listGroup = _appGroupService.GetGroupsByUserId(id).ToList();
                    foreach (var groupItem in allGroup)
                    {
                        SelectListItem listItem = new SelectListItem()
                        {
                            Text = groupItem.Description,
                            Value = groupItem.Id,
                            Selected = listGroup.Any(g => g.Id == groupItem.Id)
                        };
                        Item.GroupList.Add(listItem);
                    }

                    var allRole = RoleManager.Roles.OrderByDescending(x => x.Name);
                    var listRole = await UserManager.GetRolesAsync(id);
                    foreach (var roleItem in allRole)
                    {
                        SelectListItem listItem = new SelectListItem()
                        {
                            Text = roleItem.Description,
                            Value = roleItem.Id,
                            Selected = listRole.Any(g => g == roleItem.Name)
                        };
                        Item.RoleList.Add(listItem);
                    }

                    if (act == CommonConstants.Action_Edit)
                        return View("Edit", Item);
                    else if (act == CommonConstants.Action_Reset)
                    {
                        ManageUserViewModel user = new ManageUserViewModel();
                        user.Id = Item.Id;
                        user.FullName = Item.FullName;
                        user.UserName = Item.UserName;
                        return View("Reset", user);
                    }
                    else
                        return View("Detail", Item);
                }
            }
            else
            {
                var allGroup = _appGroupService.GetAll().ToList();

                // load the roles/Roles for selection in the form:
                foreach (var groupItem in allGroup)
                {
                    SelectListItem listItem = new SelectListItem()
                    {
                        Text = groupItem.Description,
                        Value = groupItem.Id,
                        Selected = false
                    };
                    Item.GroupList.Add(listItem);
                }
                return View("Add", Item);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.System_CanAdd_Role, CommonConstants.System_CanEdit_Role)]
        public async Task<ActionResult> AddOrEdit(string act, ApplicationUserViewModel Item, params string[] selectedItems)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result;
                    ApplicationUser newAppUser;

                    if (Item.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(Item.ImageUpload.FileName);
                        string extension = Path.GetExtension(Item.ImageUpload.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        Item.ImagePath = "~/AppFiles/Images/" + fileName;
                        Item.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                    }

                    if (act == CommonConstants.Action_Add)
                    {
                        newAppUser = new ApplicationUser();
                        newAppUser.UpdateUser(Item);
                        newAppUser.CreatedBy = User.Identity.Name;
                        newAppUser.CreatedDate = DateTime.Now;

                        result = await UserManager.CreateAsync(newAppUser, Item.Password);
                    }
                    else
                    {
                        newAppUser = await UserManager.FindByIdAsync(Item.Id);
                        newAppUser.UpdateUser(Item);
                        newAppUser.UpdatedBy = User.Identity.Name;
                        newAppUser.UpdatedDate = DateTime.Now;

                        result = await UserManager.UpdateAsync(newAppUser);
                    }
                    if (result.Succeeded)
                    {
                        await UpdateUserGroups(newAppUser.Id, selectedItems);

                        return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { status = CommonConstants.Status_Error, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = string.Join(",", result.Errors) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập liệu" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.System_CanReset_Role)]
        public async Task<ActionResult> Reset(ManageUserViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BTSDbContext context = new BTSDbContext();
                    var store = new UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(context);
                    //UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(store);

                    String hashedNewPassword = UserManager.PasswordHasher.HashPassword(Item.Password);
                    ApplicationUser cUser = await store.FindByIdAsync(Item.Id);
                    cUser.UpdatedBy = User.Identity.Name;
                    cUser.UpdatedDate = DateTime.Now;

                    await store.SetPasswordHashAsync(cUser, hashedNewPassword);
                    await store.UpdateAsync(cUser);
                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập liệu" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRoles(CommonConstants.System_CanLock_Role)]
        public async Task<ActionResult> Lock(string id)
        {
            try
            {
                var appUser = await UserManager.FindByIdAsync(id);
                appUser.Locked = !appUser.Locked;
                appUser.UpdatedBy = User.Identity.Name;
                appUser.UpdatedDate = DateTime.Now;

                var result = await UserManager.UpdateAsync(appUser);
                if (result.Succeeded)
                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Locked/UnLocked Successfully" }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { status = CommonConstants.Status_Error, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = string.Join(",", result.Errors) }, JsonRequestBehavior.AllowGet);
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
                var appUser = await UserManager.FindByIdAsync(id);
                if (appUser == null)
                {
                    return HttpNotFound();
                }

                if (_appGroupService.GetGroupsByUserId(id) != null)
                {
                    return Json(new { status = CommonConstants.Status_Error, message = "Không thể xóa Người dùng còn thuộc nhóm" }, JsonRequestBehavior.AllowGet);
                }

                var result = await UserManager.DeleteAsync(appUser);
                if (result.Succeeded)
                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { status = CommonConstants.Status_Error, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = string.Join(",", result.Errors) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private async Task UpdateUserGroups(string userID, string[] selectedItems)
        {
            _appGroupService.DeleteUserFromGroups(userID);
            _appGroupService.Save();

            var listAppUserGroup = new List<ApplicationUserGroup>();
            selectedItems = selectedItems ?? new string[] { };

            foreach (var group in selectedItems)
            {
                listAppUserGroup.Add(new ApplicationUserGroup()
                {
                    GroupId = group,
                    UserId = userID,
                    CreatedBy = User.Identity.Name,
                    CreatedDate = DateTime.Now
                });
            }
            _appGroupService.AddUserToGroups(listAppUserGroup);
            _appGroupService.Save();

            var newUserRoles = _appGroupService.GetLogicRolesByUserId(userID);
            await updateRoles(userID, newUserRoles);
        }
    }
}