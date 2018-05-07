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

        IEnumerable<ApplicationUserViewModel> GetAll()
        {
            var model = _userManager.Users;
            return Mapper.Map<IEnumerable<ApplicationUserViewModel>>(model);
        }

        public ActionResult AddOrEdit(string id = "")
        {
            ApplicationUserViewModel Item = new ApplicationUserViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                var DbItem = _userManager.FindByIdAsync(id);


                if (DbItem != null)
                {
                    Item = Mapper.Map<ApplicationUserViewModel>(DbItem.Result);
                    var listGroup = _appGroupService.GetListGroupByUserId(id);
                    Item.Groups = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(listGroup);
                }

            }
            return View(Item);
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEdit(ApplicationUserViewModel Item)
        {
            try
            {
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
                    var newAppUser = new ApplicationUser();
                    newAppUser.UpdateUser(Item);

                    newAppUser.Id = Guid.NewGuid().ToString();
                    var result = await _userManager.CreateAsync(newAppUser, Item.Password);
                    if (result.Succeeded)
                    {
                        var listAppUserGroup = new List<ApplicationUserGroup>();
                        foreach (var group in Item.Groups)
                        {
                            listAppUserGroup.Add(new ApplicationUserGroup()
                            {
                                GroupId = group.ID,
                                UserId = newAppUser.Id
                            });
                            //add role to user
                            var listRole = _appRoleService.GetListRoleByGroupId(group.ID);
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
                    var appUser = await _userManager.FindByIdAsync(Item.ID);

                    appUser.UpdateUser(Item);
                    var result = await _userManager.UpdateAsync(appUser);
                    if (result.Succeeded)
                    {
                        var listAppUserGroup = new List<ApplicationUserGroup>();
                        foreach (var group in Item.Groups)
                        {
                            listAppUserGroup.Add(new ApplicationUserGroup()
                            {
                                GroupId = group.ID,
                                UserId = Item.ID
                            });
                            //add role to user
                            var listRole = _appRoleService.GetListRoleByGroupId(group.ID);
                            foreach (var role in listRole)
                            {
                                await _userManager.RemoveFromRoleAsync(appUser.Id, role.Name);
                                await _userManager.AddToRoleAsync(appUser.Id, role.Name);
                            }
                        }
                        _appGroupService.AddUserToGroups(listAppUserGroup, Item.ID);
                        _appGroupService.Save();

                        return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = string.Join(",", result.Errors) }, JsonRequestBehavior.AllowGet);

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