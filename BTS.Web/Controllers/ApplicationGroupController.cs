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
using BTS.Common;

namespace BTS.Web.Controllers
{
    [AuthorizeRoles(CommonConstants.System_CanView_Role)]
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

        [AuthorizeRoles(CommonConstants.System_CanAdd_Role, CommonConstants.System_CanViewDetail_Role, CommonConstants.System_CanEdit_Role)]
        public async Task<ActionResult> AddOrEdit(string act, string id = "")
        {
            ApplicationGroupViewModel Item = new ApplicationGroupViewModel();
            if ((act == CommonConstants.Action_Detail || act == CommonConstants.Action_Edit) && !string.IsNullOrEmpty(id))
            {
                var DbItem = _appGroupService.GetByID(id);
                if (DbItem == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    var allRole = RoleManager.Roles.OrderByDescending(x => x.Name); ;
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

                    if (act == CommonConstants.Action_Edit)
                        return View("Edit", Item);
                    else
                        return View("Detail", Item);
                }
            }
            else
            {
                var allRole = RoleManager.Roles;
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
                return View("Add", Item);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.System_CanAdd_Role, CommonConstants.System_CanEdit_Role)]
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
                        newItem.CreatedBy = User.Identity.Name;
                        newItem.CreatedDate = DateTime.Now;

                        appGroup = _appGroupService.Add(newItem);
                        _appGroupService.Save();
                        //save group
                    }
                    else
                    {
                        appGroup = _appGroupService.GetDetail(Item.Id);

                        appGroup.UpdateApplicationGroup(Item);
                        appGroup.UpdatedBy = User.Identity.Name;
                        appGroup.UpdatedDate = DateTime.Now;

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
                            RoleId = role,
                            CreatedBy = User.Identity.Name,
                            CreatedDate = DateTime.Now
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
                return Json(new
                {
                    success = false,
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRoles(CommonConstants.System_CanDelete_Role)]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var group = _appGroupService.GetByID(id);
                if (group == null)
                {
                    return HttpNotFound();
                }

                if (_appGroupService.GetUsersByGroupId(id) != null)
                {
                    return Json(new { success = false, message = "Không thể xóa Nhóm còn người dùng" }, JsonRequestBehavior.AllowGet);
                }

                _appGroupService.DeleteRolesFromGroup(id);
                _appGroupService.Save();

                _appGroupService.Delete(id);
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