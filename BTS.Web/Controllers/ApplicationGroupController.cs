using AutoMapper;
using BTS.Common;
using BTS.Data.ApplicationModels;
using BTS.Service;
using BTS.Web.Infrastructure.Extensions;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BTS.Web.Areas.Intranet.Controllers
{
    [AuthorizeRoles(CommonConstants.System_CanView_Role)]
    public class ApplicationGroupController : BaseController
    {
        private IApplicationGroupService _appGroupService;
        //private ApplicationUserManager _userManager;

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
            List<ApplicationGroup> model = _appGroupService.GetAll().ToList();
            return Mapper.Map<IEnumerable<ApplicationGroupViewModel>>(model);
        }


        public ActionResult Add()
        {
            ApplicationGroupViewModel Item = new ApplicationGroupViewModel();
            List<ApplicationRole> allRole = RoleManager.Roles.ToList();
            foreach (ApplicationRole roleItem in allRole)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = roleItem.Description,
                    Value = roleItem.Id,
                    Selected = false
                };
                Item.RoleList.Add(listItem);
            }
            return View(Item);
        }

        [AuthorizeRoles(CommonConstants.System_CanViewDetail_Role)]
        public ActionResult Detail(string id = "")
        {
            ApplicationGroupViewModel Item = new ApplicationGroupViewModel();
            ApplicationGroup DbItem = _appGroupService.GetByID(id);
            if (DbItem == null)
            {
                return HttpNotFound();
            }
            else
            {
                List<ApplicationRole> allRole = RoleManager.Roles.OrderByDescending(x => x.Name).ToList();
                IEnumerable<ApplicationRole> listRole = _appGroupService.GetRolesByGroupId(id);

                Item = Mapper.Map<ApplicationGroupViewModel>(DbItem);
                foreach (ApplicationRole roleItem in allRole)
                {
                    SelectListItem listItem = new SelectListItem()
                    {
                        Text = roleItem.Description,
                        Value = roleItem.Id,
                        Selected = listRole.Any(g => g.Id == roleItem.Id)
                    };
                    Item.RoleList.Add(listItem);
                }

                List<ApplicationUser> allUser = UserManager.Users.ToList();
                List<ApplicationUser> listUser = _appGroupService.GetUsersByGroupId(id).ToList();
                foreach (ApplicationUser userItem in allUser)
                {
                    SelectListItem listItem = new SelectListItem()
                    {
                        Text = userItem.FullName,
                        Value = userItem.Id,
                        Selected = listUser.Any(g => g.Id == userItem.Id)
                    };
                    Item.UserList.Add(listItem);
                }
                return View(Item);
            }
        }

        [AuthorizeRoles(CommonConstants.System_CanEdit_Role)]
        public ActionResult Edit(string id = "")
        {
            ApplicationGroupViewModel Item = new ApplicationGroupViewModel();
            ApplicationGroup DbItem = _appGroupService.GetByID(id);
            if (DbItem == null)
            {
                return HttpNotFound();
            }
            else
            {
                List<ApplicationRole> allRole = RoleManager.Roles.OrderByDescending(x => x.Name).ToList();
                IEnumerable<ApplicationRole> listRole = _appGroupService.GetRolesByGroupId(id);

                Item = Mapper.Map<ApplicationGroupViewModel>(DbItem);
                foreach (ApplicationRole roleItem in allRole)
                {
                    SelectListItem listItem = new SelectListItem()
                    {
                        Text = roleItem.Description,
                        Value = roleItem.Id,
                        Selected = listRole.Any(g => g.Id == roleItem.Id)
                    };
                    Item.RoleList.Add(listItem);
                }

                List<ApplicationUser> allUser = UserManager.Users.ToList();
                List<ApplicationUser> listUser = _appGroupService.GetUsersByGroupId(id).ToList();
                foreach (ApplicationUser userItem in allUser)
                {
                    SelectListItem listItem = new SelectListItem()
                    {
                        Text = userItem.FullName,
                        Value = userItem.Id,
                        Selected = listUser.Any(g => g.Id == userItem.Id)
                    };
                    Item.UserList.Add(listItem);
                }
                return View(Item);
            }
        }

        [AuthorizeRoles(CommonConstants.System_CanAdd_Role, CommonConstants.System_CanViewDetail_Role, CommonConstants.System_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, string id = "")
        {
            ApplicationGroupViewModel Item = new ApplicationGroupViewModel();
            if ((act == CommonConstants.Action_Detail || act == CommonConstants.Action_Edit) && !string.IsNullOrEmpty(id))
            {
                ApplicationGroup DbItem = _appGroupService.GetByID(id);
                if (DbItem == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    List<ApplicationRole> allRole = RoleManager.Roles.OrderByDescending(x => x.Name).ToList();
                    IEnumerable<ApplicationRole> listRole = _appGroupService.GetRolesByGroupId(id);

                    Item = Mapper.Map<ApplicationGroupViewModel>(DbItem);
                    foreach (ApplicationRole roleItem in allRole)
                    {
                        SelectListItem listItem = new SelectListItem()
                        {
                            Text = roleItem.Description,
                            Value = roleItem.Id,
                            Selected = listRole.Any(g => g.Id == roleItem.Id)
                        };
                        Item.RoleList.Add(listItem);
                    }

                    List<ApplicationUser> allUser = UserManager.Users.ToList();
                    List<ApplicationUser> listUser = _appGroupService.GetUsersByGroupId(id).ToList();
                    foreach (ApplicationUser userItem in allUser)
                    {
                        SelectListItem listItem = new SelectListItem()
                        {
                            Text = userItem.FullName,
                            Value = userItem.Id,
                            Selected = listUser.Any(g => g.Id == userItem.Id)
                        };
                        Item.UserList.Add(listItem);
                    }

                    if (act == CommonConstants.Action_Edit)
                    {
                        return View("Edit", Item);
                    }
                    else
                    {
                        return View("Detail", Item);
                    }
                }
            }
            else
            {
                List<ApplicationRole> allRole = RoleManager.Roles.ToList();
                foreach (ApplicationRole roleItem in allRole)
                {
                    SelectListItem listItem = new SelectListItem()
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
        [AuthorizeRoles(CommonConstants.System_CanAdd_Role)]
        public async Task<ActionResult> Add(ApplicationGroupViewModel Item, string[] selectedRoleItems, params string[] selectedUserItems)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationGroup appGroup;

                    // Create new Group
                    ApplicationGroup newItem = new ApplicationGroup();
                    newItem.UpdateApplicationGroup(Item);

                    newItem.CreatedBy = User.Identity.Name;
                    newItem.CreatedDate = DateTime.Now;

                    appGroup = _appGroupService.Add(newItem);
                    _appGroupService.Save();
                    //save group

                    UpdateFromGroupToRoleUser(appGroup, selectedRoleItems, selectedUserItems);
                   
                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)

            {
                return Json(new
                {
                    status = CommonConstants.Status_Error,
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.System_CanEdit_Role)]
        public async Task<ActionResult> Edit(ApplicationGroupViewModel Item, string[] selectedRoleItems, params string[] selectedUserItems)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationGroup appGroup;

                    // Edit new Group
                    appGroup = _appGroupService.GetDetail(Item.Id);

                    appGroup.UpdateApplicationGroup(Item);
                    appGroup.UpdatedBy = User.Identity.Name;
                    appGroup.UpdatedDate = DateTime.Now;

                    _appGroupService.Update(appGroup);
                    _appGroupService.Save();

                    UpdateFromGroupToRoleUser(appGroup, selectedRoleItems, selectedUserItems);

                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)

            {
                return Json(new
                {
                    status = CommonConstants.Status_Error,
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        private async void UpdateFromGroupToRoleUser(ApplicationGroup appGroup, string[] selectedRoleItems, string[] selectedUserItems)
        {
            //delete ApplicationRoleGroup
            _appGroupService.DeleteRolesFromGroup(appGroup.Id);
            _appGroupService.Save();

            //add ApplicationRoleGroup
            List<ApplicationRoleGroup> listRoleGroup = new List<ApplicationRoleGroup>();
            selectedRoleItems = selectedRoleItems ?? new string[] { };
            foreach (string role in selectedRoleItems)
            {
                listRoleGroup.Add(new ApplicationRoleGroup()
                {
                    GroupId = appGroup.Id,
                    RoleId = role,
                    CreatedBy = User.Identity.Name,
                    CreatedDate = DateTime.Now
                });
            }
            _appGroupService.AddRoleGroups(listRoleGroup);
            _appGroupService.Save();


            List<string> oldUserIds = _appGroupService.GetUsersByGroupId(appGroup.Id).Select(x => x.Id).ToList();

            //delete ApplicationUserGroup
            _appGroupService.DeleteUsersFromGroup(appGroup.Id);
            _appGroupService.Save();

            //add ApplicationUserGroup
            List<ApplicationUserGroup> listUserGroup = new List<ApplicationUserGroup>();
            selectedUserItems = selectedUserItems ?? new string[] { };
            foreach (string userId in selectedUserItems)
            {
                listUserGroup.Add(new ApplicationUserGroup()
                {
                    GroupId = appGroup.Id,
                    UserId = userId,
                    CreatedBy = User.Identity.Name,
                    CreatedDate = DateTime.Now
                });
            }
            _appGroupService.AddUserGroups(listUserGroup);
            _appGroupService.Save();

            foreach (string userIdItem in oldUserIds)
            {

                await removeUserRoles(userIdItem);

                List<ApplicationRole> newUserRoles = _appGroupService.GetLogicRolesByUserId(userIdItem).ToList();

                await addUserRoles(userIdItem, newUserRoles);
            }

            List<ApplicationUser> users = _appGroupService.GetUsersByGroupId(appGroup.Id).ToList();

            foreach (ApplicationUser userItem in users)
            {
                if (!oldUserIds.Contains(userItem.Id))
                {
                    await removeUserRoles(userItem.Id);

                    List<ApplicationRole> newUserRoles = _appGroupService.GetLogicRolesByUserId(userItem.Id).ToList();

                    await addUserRoles(userItem.Id, newUserRoles);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.System_CanAdd_Role, CommonConstants.System_CanEdit_Role)]
        public async Task<ActionResult> AddOrEdit(string act, ApplicationGroupViewModel Item, string[] selectedRoleItems, params string[] selectedUserItems)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationGroup appGroup;
                    // Create new Group
                    if (act == CommonConstants.Action_Add)
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

                    //delete ApplicationRoleGroup
                    _appGroupService.DeleteRolesFromGroup(appGroup.Id);
                    _appGroupService.Save();

                    //add ApplicationRoleGroup
                    List<ApplicationRoleGroup> listRoleGroup = new List<ApplicationRoleGroup>();
                    selectedRoleItems = selectedRoleItems ?? new string[] { };
                    foreach (string role in selectedRoleItems)
                    {
                        listRoleGroup.Add(new ApplicationRoleGroup()
                        {
                            GroupId = appGroup.Id,
                            RoleId = role,
                            CreatedBy = User.Identity.Name,
                            CreatedDate = DateTime.Now
                        });
                    }
                    _appGroupService.AddRoleGroups(listRoleGroup);
                    _appGroupService.Save();


                    List<string> oldUserIds = _appGroupService.GetUsersByGroupId(appGroup.Id).Select(x => x.Id).ToList();

                    //delete ApplicationUserGroup
                    _appGroupService.DeleteUsersFromGroup(appGroup.Id);
                    _appGroupService.Save();

                    //add ApplicationUserGroup
                    List<ApplicationUserGroup> listUserGroup = new List<ApplicationUserGroup>();
                    selectedUserItems = selectedUserItems ?? new string[] { };
                    foreach (string userId in selectedUserItems)
                    {
                        listUserGroup.Add(new ApplicationUserGroup()
                        {
                            GroupId = appGroup.Id,
                            UserId = userId,
                            CreatedBy = User.Identity.Name,
                            CreatedDate = DateTime.Now
                        });
                    }
                    _appGroupService.AddUserGroups(listUserGroup);
                    _appGroupService.Save();

                    foreach (string userIdItem in oldUserIds)
                    {

                        await removeUserRoles(userIdItem);

                        List<ApplicationRole> newUserRoles = _appGroupService.GetLogicRolesByUserId(userIdItem).ToList();

                        await addUserRoles(userIdItem, newUserRoles);
                    }

                    List<ApplicationUser> users = _appGroupService.GetUsersByGroupId(appGroup.Id).ToList();

                    foreach (ApplicationUser userItem in users)
                    {
                        if (!oldUserIds.Contains(userItem.Id))
                        {
                            await removeUserRoles(userItem.Id);

                            List<ApplicationRole> newUserRoles = _appGroupService.GetLogicRolesByUserId(userItem.Id).ToList();

                            await addUserRoles(userItem.Id, newUserRoles);
                        }
                    }
                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)

            {
                return Json(new
                {
                    status = CommonConstants.Status_Error,
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRoles(CommonConstants.System_CanDelete_Role)]
        public ActionResult Delete(string id)
        {
            try
            {
                ApplicationGroup group = _appGroupService.GetByID(id);
                if (group == null)
                {
                    return HttpNotFound();
                }
                var dbUser = _appGroupService.GetUsersByGroupId(id).LastOrDefault();
                if (dbUser != null)
                {
                    return Json(new { status = CommonConstants.Status_Error, message = "Không thể xóa Nhóm còn người dùng" }, JsonRequestBehavior.AllowGet);
                }

                _appGroupService.DeleteRolesFromGroup(id);
                _appGroupService.Save();

                _appGroupService.Delete(id);
                _appGroupService.Save();

                return Json(new { data_restUrl = "/ApplicationGroup/Add", status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}