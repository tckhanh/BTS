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

namespace BTS.Web.Controllers
{
    public class ApplicationUserController : BaseController
    {
        private ApplicationUserManager _userManager;
        private IApplicationGroupService _appGroupService;
        private IApplicationRoleService _appRoleService;

        public ApplicationUserController(
            IApplicationGroupService appGroupService,
            IApplicationRoleService appRoleService,
            ApplicationUserManager userManager,
            IErrorService errorService)
            : base(errorService)
        {
            _appRoleService = appRoleService;
            _appGroupService = appGroupService;
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

        private IEnumerable<ApplicationUserViewModel> GetAll()
        {
            var model = _userManager.Users;
            return Mapper.Map<IEnumerable<ApplicationUserViewModel>>(model);
        }

        public async Task<ActionResult> AddOrEdit(string id = "")
        {
            ApplicationUserViewModel Item = new ApplicationUserViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                var DbItem = await _userManager.FindByIdAsync(id);
                if (DbItem == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    var allGroup = _appGroupService.GetAll();
                    var listGroup = _appGroupService.GetListGroupByUserId(id);

                    Item = Mapper.Map<ApplicationUserViewModel>(DbItem);
                    Item.Password = DbItem.PasswordHash;

                    // load the roles/Roles for selection in the form:
                    foreach (var groupItem in allGroup)
                    {
                        var listItem = new SelectListItem()
                        {
                            Text = groupItem.Description,
                            Value = groupItem.ID,
                            Selected = listGroup.Any(g => g.ID == groupItem.ID)
                        };
                        Item.GroupsList.Add(listItem);
                    }
                    return View(Item);
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
                        Value = groupItem.ID,
                        Selected = false
                    };
                    Item.GroupsList.Add(listItem);
                }
                return View(Item);
            }
        }

        [HttpPost]
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

                    if (string.IsNullOrEmpty(Item.ID))
                    {
                        newAppUser = new ApplicationUser();
                        newAppUser.UpdateUser(Item);
                        result = await _userManager.CreateAsync(newAppUser, Item.Password);
                    }
                    else
                    {
                        newAppUser = await _userManager.FindByIdAsync(Item.ID);

                        newAppUser.UpdateUser(Item);
                        result = await _userManager.UpdateAsync(newAppUser);
                    }
                    if (result.Succeeded)
                    {
                        var listAppUserGroup = new List<ApplicationUserGroup>();
                        selectedItems = selectedItems ?? new string[] { };
                        foreach (var group in selectedItems)
                        {
                            listAppUserGroup.Add(new ApplicationUserGroup()
                            {
                                GroupId = group,
                                UserId = newAppUser.Id
                            });
                            //add role to user
                            var listRole = _appRoleService.GetListRoleByGroupId(group);
                            foreach (var role in listRole)
                            {
                                await _userManager.RemoveFromRoleAsync(newAppUser.Id, role.Name);
                                await _userManager.AddToRoleAsync(newAppUser.Id, role.Name);
                            }
                        }
                        _appGroupService.AddUserToGroups(listAppUserGroup, newAppUser.Id);
                        _appGroupService.Save();

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

        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var appUser = await _userManager.FindByIdAsync(id);
                var result = await _userManager.DeleteAsync(appUser);
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