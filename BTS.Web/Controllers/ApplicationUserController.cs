﻿using AutoMapper;
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

namespace BTS.Web.Controllers
{
    public class ApplicationUserController : BaseController
    {
        private ApplicationUserManager _userManager;
        private IApplicationGroupService _appGroupService;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

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

        public async Task<ActionResult> AddOrEdit(string act, string id = "")
        {
            ApplicationUserViewModel Item = new ApplicationUserViewModel();
            if ((act == CommonConstants.Action_Detail || act == CommonConstants.Action_Edit) && !string.IsNullOrEmpty(id))
            {
                var DbItem = await UserManager.FindByIdAsync(id);
                if (DbItem == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    Item = Mapper.Map<ApplicationUserViewModel>(DbItem);

                    var allGroup = _appGroupService.GetAll();
                    var listGroup = _appGroupService.GetGroupsByUserId(id);
                    foreach (var groupItem in allGroup)
                    {
                        var listItem = new SelectListItem()
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
                        var listItem = new SelectListItem()
                        {
                            Text = roleItem.Description,
                            Value = roleItem.Id,
                            Selected = listRole.Any(g => g == roleItem.Name)
                        };
                        Item.RoleList.Add(listItem);
                    }

                    if (act == CommonConstants.Action_Detail)
                        return View("Detail", Item);
                    else
                        return View("Edit", Item);
                }
            }
            else
            {
                var allGroup = _appGroupService.GetAll();

                // load the roles/Roles for selection in the form:
                foreach (var groupItem in allGroup)
                {
                    var listItem = new SelectListItem()
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
        public async Task<ActionResult> AddOrEdit(ApplicationUserViewModel Item, params string[] selectedItems)
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

                    if (string.IsNullOrEmpty(Item.Id))
                    {
                        newAppUser = new ApplicationUser();
                        newAppUser.UpdateUser(Item);
                        result = await UserManager.CreateAsync(newAppUser, Item.Password);
                    }
                    else
                    {
                        newAppUser = await UserManager.FindByIdAsync(Item.Id);

                        newAppUser.UpdateUser(Item);
                        result = await UserManager.UpdateAsync(newAppUser);
                    }
                    if (result.Succeeded)
                    {
                        _appGroupService.DeleteUserFromGroups(newAppUser.Id);
                        _appGroupService.Save();

                        var listAppUserGroup = new List<ApplicationUserGroup>();
                        selectedItems = selectedItems ?? new string[] { };

                        foreach (var group in selectedItems)
                        {
                            listAppUserGroup.Add(new ApplicationUserGroup()
                            {
                                GroupId = group,
                                UserId = newAppUser.Id
                            });

                            _appGroupService.AddUserToGroups(listAppUserGroup);
                            _appGroupService.Save();
                        }
                        var newUserRoles = _appGroupService.GetLogicRolesByUserId(newAppUser.Id);
                        await updateRoles(newAppUser.Id, newUserRoles);

                        return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = string.Join(",", result.Errors) }, JsonRequestBehavior.AllowGet);
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

        public async Task<ActionResult> Lock(string id)
        {
            try
            {
                var appUser = await UserManager.FindByIdAsync(id);
                appUser.Locked = !appUser.Locked;
                var result = await UserManager.UpdateAsync(appUser);
                if (result.Succeeded)
                    return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Locked/UnLocked Successfully" }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = string.Join(",", result.Errors) }, JsonRequestBehavior.AllowGet);
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
                var appUser = await UserManager.FindByIdAsync(id);

                var result = await UserManager.DeleteAsync(appUser);
                if (result.Succeeded)
                    return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = string.Join(",", result.Errors) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}