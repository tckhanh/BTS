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
using BTS.Data.ApplicationModels;

namespace BTS.Web.Controllers
{
    public class ApplicationGroupController : BaseController
    {
        private IApplicationGroupService _appGroupService;
        private ApplicationUserManager _userManager;

        public ApplicationGroupController(IErrorService errorService,
            IApplicationGroupService appGroupService) : base(errorService)
        {
            _appGroupService = appGroupService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAll());
        }

        private IEnumerable<ApplicationGroupViewModel> GetAll()
        {
            var model = _appGroupService.GetAll();
            return Mapper.Map<IEnumerable<ApplicationGroupViewModel>>(model);
        }

        public async Task<ActionResult> AddOrEdit(string id = "")
        {
            ApplicationGroupViewModel Item = new ApplicationGroupViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                var DbItem = _appGroupService.GetByID(id);
                if (DbItem == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    var allRole = RoleManager.Roles;
                    var listRole = _appGroupService.GetRolesByGroupId(id);

                    Item = Mapper.Map<ApplicationGroupViewModel>(DbItem);
                    foreach (var roleItem in allRole)
                    {
                        var listItem = new SelectListItem()
                        {
                            Text = roleItem.Description,
                            Value = roleItem.Id,
                            Selected = listRole.Any(g => g.Id == roleItem.Id)
                        };
                        Item.RoleList.Add(listItem);
                    }

                    var listUser = _appGroupService.GetUsersByGroupId(id);
                    foreach (var userItem in listUser)
                    {
                        var listItem = new SelectListItem()
                        {
                            Text = userItem.FullName,
                            Value = userItem.Id,
                            Selected = true
                        };
                        Item.UserList.Add(listItem);
                    }

                    return View(Item);
                }
            }
            else
            {
                var allRole = RoleManager.Roles;

                // load the roles/Roles for selection in the form:
                foreach (var roleItem in allRole)
                {
                    var listItem = new SelectListItem()
                    {
                        Text = roleItem.Description,
                        Value = roleItem.Id,
                        Selected = false
                    };
                    Item.RoleList.Add(listItem);
                }
                return View(Item);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddOrEdit(ApplicationGroupViewModel Item, params string[] selectedItems)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationGroup appGroup;
                    // Create new Group
                    if (string.IsNullOrEmpty(Item.Id))
                    {
                        ApplicationGroup newItem = new ApplicationGroup();
                        newItem.UpdateApplicationGroup(Item);
                        appGroup = _appGroupService.Add(newItem);
                        _appGroupService.Save();
                        //save group
                    }
                    else
                    {
                        appGroup = _appGroupService.GetDetail(Item.Id);

                        appGroup.UpdateApplicationGroup(Item);
                        _appGroupService.Update(appGroup);
                        _appGroupService.Save();
                    }

                    _appGroupService.DeleteRolesFromGroup(appGroup.Id);
                    _appGroupService.Save();

                    //add ApplicationRoleGroup
                    var listRoleGroup = new List<ApplicationRoleGroup>();
                    selectedItems = selectedItems ?? new string[] { };
                    foreach (var role in selectedItems)
                    {
                        listRoleGroup.Add(new ApplicationRoleGroup()
                        {
                            GroupId = appGroup.Id,
                            RoleId = role
                        });
                    }
                    _appGroupService.AddRolesToGroup(listRoleGroup);
                    _appGroupService.Save();

                    var users = _appGroupService.GetUsersByGroupId(appGroup.Id);
                    foreach (var userItem in users)
                    {
                        var newUserRoles = _appGroupService.GetLogicRolesByUserId(userItem.Id);
                        await updateRoles(userItem.Id, newUserRoles);
                    }
                    return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
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
                var appGroup = _appGroupService.Delete(id);
                _appGroupService.Save();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}