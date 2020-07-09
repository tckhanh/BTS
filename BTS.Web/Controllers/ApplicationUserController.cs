using AutoMapper;
using BTS.Common;
using BTS.Data;
using BTS.Data.ApplicationModels;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Infrastructure.Extensions;
using BTS.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static BTS.Web.Models.AccountViewModel;

namespace BTS.Web.Areas.Intranet.Controllers
{
    [AuthorizeRoles(CommonConstants.System_CanView_Role)]
    public class ApplicationUserController : BaseController
    {
        private IApplicationGroupService _appGroupService;
        private ICityService _cityService;

        public ApplicationUserController(IApplicationGroupService appGroupService, ICityService cityService, IErrorService errorService)
            : base(errorService)
        {
            _appGroupService = appGroupService;
            _cityService = cityService;
        }

        public ICollection<SelectListItem> ListRolesOfUser(string id = "")
        {
            ICollection<SelectListItem> allRolesOfUser = new List<SelectListItem>();
            IEnumerable<ApplicationRole> listRoles = RoleManager.Roles.ToList();
            foreach (ApplicationRole roleItem in listRoles)
            {
                allRolesOfUser.Add(new SelectListItem()
                {
                    Value = roleItem.Id,
                    Text = roleItem.Name,
                    Selected = false
                });
            }

            IEnumerable<ApplicationGroup> listGroupOfUser = _appGroupService.GetGroupsByUserId(id).ToList();
            foreach (ApplicationGroup groupItem in listGroupOfUser)
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
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            UserPermissionsViewModel model = new UserPermissionsViewModel(user);
            return View(model);
        }

        private IEnumerable<ApplicationUserViewModel> GetAll()
        {
            List<ApplicationUser> model = UserManager.Users.ToList();
            return Mapper.Map<IEnumerable<ApplicationUserViewModel>>(model);
        }


        public ActionResult Add()
        {
            ApplicationUserViewModel applicationUserVM = new ApplicationUserViewModel();

            List<ApplicationGroup> allGroup = _appGroupService.GetAll().ToList();

            // load the roles/Roles for selection in the form:
            foreach (ApplicationGroup groupItem in allGroup)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = groupItem.Description,
                    Value = groupItem.Id,
                    Selected = false
                };
                applicationUserVM.GroupList.Add(listItem);
            }
            return View(applicationUserVM);
        }

        [AuthorizeRoles(CommonConstants.System_CanViewDetail_Role)]
        public async Task<ActionResult> Detail(string id = "")
        {
            ApplicationUserViewModel applicationUserVM = new ApplicationUserViewModel();

            ApplicationUser existedApplicationUser = await UserManager.FindByIdAsync(id);
            if (existedApplicationUser == null)
            {
                return HttpNotFound();
            }
            else
            {
                applicationUserVM = await FillInApplicationUser(existedApplicationUser);
                return View(applicationUserVM);
            }
        }

        [AuthorizeRoles(CommonConstants.System_CanEdit_Role)]
        public async Task<ActionResult> Edit(string id = "")
        {
            ApplicationUserViewModel applicationUserVM = new ApplicationUserViewModel();

            ApplicationUser existedApplicationUser = await UserManager.FindByIdAsync(id);
            if (existedApplicationUser == null)
            {
                return HttpNotFound();
            }
            else
            {
                applicationUserVM = await FillInApplicationUser(existedApplicationUser);
                return View(applicationUserVM);
            }
        }

        [AuthorizeRoles(CommonConstants.System_CanReset_Role)]
        public async Task<ActionResult> Reset(string id = "")
        {
            ApplicationUser existedApplicationUser = await UserManager.FindByIdAsync(id);
            if (existedApplicationUser == null)
            {
                return HttpNotFound();
            }
            else
            {
                ManageUserViewModel user = new ManageUserViewModel
                {
                    Id = existedApplicationUser.Id,
                    FullName = existedApplicationUser.FullName,
                    UserName = existedApplicationUser.UserName
                };
                return View(user);
            }
        }

        private async Task<ApplicationUserViewModel> FillInApplicationUser(ApplicationUser existedApplicationUser)
        {
            ApplicationUserViewModel applicationUserVM = Mapper.Map<ApplicationUserViewModel>(existedApplicationUser);

            List<ApplicationGroup> allGroup = _appGroupService.GetAll().ToList();
            List<ApplicationGroup> listGroup = _appGroupService.GetGroupsByUserId(existedApplicationUser.Id).ToList();

            foreach (ApplicationGroup groupItem in allGroup)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = groupItem.Description,
                    Value = groupItem.Id,
                    Selected = listGroup.Any(g => g.Id == groupItem.Id)
                };
                applicationUserVM.GroupList.Add(listItem);
            }

            List<ApplicationRole> allRole = RoleManager.Roles.OrderByDescending(x => x.Name).ToList();
            IList<string> listRole = await UserManager.GetRolesAsync(existedApplicationUser.Id);
            foreach (ApplicationRole roleItem in allRole)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = roleItem.Description,
                    Value = roleItem.Id,
                    Selected = listRole.Any(g => g == roleItem.Name)
                };
                applicationUserVM.RoleList.Add(listItem);
            }

            List<City> allCity = _cityService.getAll().ToList();
            string[] listCity = existedApplicationUser.CityIDsScope?.Split(new char[] { ';' });
            List<string> allArea = allCity?.Select(x => x.Area).Distinct().ToList();
            string[] listArea = existedApplicationUser.AreasScope?.Split(new char[] { ';' });

            foreach (string areaItem in allArea)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = areaItem,
                    Value = areaItem,
                    Selected = (listArea == null) ? false : listArea.Contains(areaItem)
                };
                applicationUserVM.AreaList.Add(listItem);
            }

            foreach (City cityItem in allCity)
            {
                SelectListGroup cityGroup = new SelectListGroup()
                {
                    Name = cityItem.Area.ToUnsignString()
                };

                SelectListItem listItem = new SelectListItem()
                {
                    Text = cityItem.Name,
                    Value = cityItem.Id,
                    Group = cityGroup,
                    Selected = (listCity == null) ? false : listCity.Contains(cityItem.Id)
                };
                applicationUserVM.CityList.Add(listItem);
            }
            return applicationUserVM;
        }

        [AuthorizeRoles(CommonConstants.System_CanAdd_Role, CommonConstants.System_CanViewDetail_Role, CommonConstants.System_CanEdit_Role)]
        public async Task<ActionResult> AddOrEdit(string act, string id = "")
        {
            ApplicationUserViewModel applicationUserVM = new ApplicationUserViewModel();
            if ((act == CommonConstants.Action_Detail || act == CommonConstants.Action_Edit || act == CommonConstants.Action_Reset) && !string.IsNullOrEmpty(id))
            {
                ApplicationUser existedApplicationUser = await UserManager.FindByIdAsync(id);
                if (existedApplicationUser == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    applicationUserVM = Mapper.Map<ApplicationUserViewModel>(existedApplicationUser);

                    List<ApplicationGroup> allGroup = _appGroupService.GetAll().ToList();
                    List<ApplicationGroup> listGroup = _appGroupService.GetGroupsByUserId(id).ToList();
                    foreach (ApplicationGroup groupItem in allGroup)
                    {
                        SelectListItem listItem = new SelectListItem()
                        {
                            Text = groupItem.Description,
                            Value = groupItem.Id,
                            Selected = listGroup.Any(g => g.Id == groupItem.Id)
                        };
                        applicationUserVM.GroupList.Add(listItem);
                    }

                    List<ApplicationRole> allRole = RoleManager.Roles.OrderByDescending(x => x.Name).ToList();
                    IList<string> listRole = await UserManager.GetRolesAsync(id);
                    foreach (ApplicationRole roleItem in allRole)
                    {
                        SelectListItem listItem = new SelectListItem()
                        {
                            Text = roleItem.Description,
                            Value = roleItem.Id,
                            Selected = listRole.Any(g => g == roleItem.Name)
                        };
                        applicationUserVM.RoleList.Add(listItem);
                    }

                    List<City> allCity = _cityService.getAll().ToList();
                    string[] listCity = existedApplicationUser.CityIDsScope?.Split(new char[] { ';' });
                    List<string> allArea = allCity?.Select(x => x.Area).Distinct().ToList();
                    string[] listArea = existedApplicationUser.AreasScope?.Split(new char[] { ';' });

                    foreach (string areaItem in allArea)
                    {
                        SelectListItem listItem = new SelectListItem()
                        {
                            Text = areaItem,
                            Value = areaItem,
                            Selected = (listArea == null) ? false : listArea.Contains(areaItem)
                        };
                        applicationUserVM.AreaList.Add(listItem);
                    }

                    foreach (City cityItem in allCity)
                    {
                        SelectListGroup cityGroup = new SelectListGroup()
                        {
                            Name = cityItem.Area.ToUnsignString()
                        };

                        SelectListItem listItem = new SelectListItem()
                        {
                            Text = cityItem.Name,
                            Value = cityItem.Id,
                            Group = cityGroup,
                            Selected = (listCity == null) ? false : listCity.Contains(cityItem.Id)
                        };
                        applicationUserVM.CityList.Add(listItem);
                    }

                    if (act == CommonConstants.Action_Edit)
                    {
                        return View("Edit", applicationUserVM);
                    }
                    else if (act == CommonConstants.Action_Reset)
                    {
                        ManageUserViewModel user = new ManageUserViewModel
                        {
                            Id = applicationUserVM.Id,
                            FullName = applicationUserVM.FullName,
                            UserName = applicationUserVM.UserName
                        };
                        return View("Reset", user);
                    }
                    else
                    {
                        return View("Detail", applicationUserVM);
                    }
                }
            }
            else
            {
                List<ApplicationGroup> allGroup = _appGroupService.GetAll().ToList();

                // load the roles/Roles for selection in the form:
                foreach (ApplicationGroup groupItem in allGroup)
                {
                    SelectListItem listItem = new SelectListItem()
                    {
                        Text = groupItem.Description,
                        Value = groupItem.Id,
                        Selected = false
                    };
                    applicationUserVM.GroupList.Add(listItem);
                }
                return View("Add", applicationUserVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.System_CanAdd_Role)]
        public async Task<ActionResult> Add(ApplicationUserViewModel Item, string[] selectedGroupItems, string[] selectedAreaItems, params string[] selectedCityItems)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result;
                    ApplicationUser newAppUser = new ApplicationUser();

                    Item.CityIDsScope = string.Join(";", selectedCityItems ?? new string[0]);
                    Item.AreasScope = string.Join(";", selectedAreaItems ?? new string[0]);

                    if (Item.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(Item.ImageUpload.FileName);
                        string extension = Path.GetExtension(Item.ImageUpload.FileName);
                        //fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        fileName = "UserImage" + DateTime.Now.ToString("yyyymmssfff") + extension;
                        Item.ImagePath = "/AppFiles/Images/" + fileName;
                        Item.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                    }

                    newAppUser = new ApplicationUser();
                    newAppUser.UpdateUser(Item);
                    newAppUser.CreatedBy = User.Identity.Name;
                    newAppUser.CreatedDate = DateTime.Now;

                    result = await UserManager.CreateAsync(newAppUser, Item.Password);

                    if (result.Succeeded)
                    {

                        UpdateFromUserToGroupRole(newAppUser, selectedGroupItems);

                        return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = CommonConstants.Status_Error, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = string.Join(",", result.Errors) }, JsonRequestBehavior.AllowGet);
                    }
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
        public async Task<ActionResult> Edit(ApplicationUserViewModel Item, string[] selectedGroupItems, string[] selectedAreaItems, params string[] selectedCityItems)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result;
                    ApplicationUser newAppUser = new ApplicationUser();

                    Item.CityIDsScope = string.Join(";", selectedCityItems ?? new string[0]);
                    Item.AreasScope = string.Join(";", selectedAreaItems ?? new string[0]);

                    if (Item.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(Item.ImageUpload.FileName);
                        string extension = Path.GetExtension(Item.ImageUpload.FileName);
                        //fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        fileName = "UserImage" + DateTime.Now.ToString("yyyymmssfff") + extension;
                        Item.ImagePath = "/AppFiles/Images/" + fileName;
                        Item.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                    }

                    newAppUser = await UserManager.FindByIdAsync(Item.Id);
                    newAppUser.UpdateUser(Item);
                    newAppUser.UpdatedBy = User.Identity.Name;
                    newAppUser.UpdatedDate = DateTime.Now;

                    result = await UserManager.UpdateAsync(newAppUser);

                    if (result.Succeeded)
                    {

                        UpdateFromUserToGroupRole(newAppUser, selectedGroupItems);

                        return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = CommonConstants.Status_Error, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = string.Join(",", result.Errors) }, JsonRequestBehavior.AllowGet);
                    }
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

        private async void UpdateFromUserToGroupRole(ApplicationUser newAppUser, string[] selectedGroupItems)
        {
            _appGroupService.DeleteUserFromGroups(newAppUser.Id);
            _appGroupService.Save();

            List<ApplicationUserGroup> listAppUserGroup = new List<ApplicationUserGroup>();
            selectedGroupItems = selectedGroupItems ?? new string[] { };

            foreach (string group in selectedGroupItems)
            {
                listAppUserGroup.Add(new ApplicationUserGroup()
                {
                    GroupId = group,
                    UserId = newAppUser.Id,
                    CreatedBy = User.Identity.Name,
                    CreatedDate = DateTime.Now
                });
            }
            _appGroupService.AddUserGroups(listAppUserGroup);
            _appGroupService.Save();


            await removeUserRoles(newAppUser.Id);

            IEnumerable<ApplicationRole> newUserRoles = _appGroupService.GetLogicRolesByUserId(newAppUser.Id);

            await addUserRoles(newAppUser.Id, newUserRoles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.System_CanAdd_Role, CommonConstants.System_CanEdit_Role)]
        public async Task<ActionResult> AddOrEdit(string act, ApplicationUserViewModel Item, string[] selectedGroupItems, string[] selectedAreaItems, params string[] selectedCityItems)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result;
                    ApplicationUser newAppUser = new ApplicationUser();

                    Item.CityIDsScope = string.Join(";", selectedCityItems ?? new string[0]);
                    Item.AreasScope = string.Join(";", selectedAreaItems ?? new string[0]);

                    if (Item.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(Item.ImageUpload.FileName);
                        string extension = Path.GetExtension(Item.ImageUpload.FileName);
                        //fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        fileName = "UserImage" + DateTime.Now.ToString("yyyymmssfff") + extension;
                        Item.ImagePath = "/AppFiles/Images/" + fileName;
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

                        _appGroupService.DeleteUserFromGroups(newAppUser.Id);
                        _appGroupService.Save();

                        List<ApplicationUserGroup> listAppUserGroup = new List<ApplicationUserGroup>();
                        selectedGroupItems = selectedGroupItems ?? new string[] { };

                        foreach (string group in selectedGroupItems)
                        {
                            listAppUserGroup.Add(new ApplicationUserGroup()
                            {
                                GroupId = group,
                                UserId = newAppUser.Id,
                                CreatedBy = User.Identity.Name,
                                CreatedDate = DateTime.Now
                            });
                        }
                        _appGroupService.AddUserGroups(listAppUserGroup);
                        _appGroupService.Save();


                        await removeUserRoles(newAppUser.Id);

                        IEnumerable<ApplicationRole> newUserRoles = _appGroupService.GetLogicRolesByUserId(newAppUser.Id);

                        await addUserRoles(newAppUser.Id, newUserRoles);

                        return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = CommonConstants.Status_Error, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = string.Join(",", result.Errors) }, JsonRequestBehavior.AllowGet);
                    }
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
        [AuthorizeRoles(CommonConstants.System_CanReset_Role)]
        public async Task<ActionResult> Reset(ManageUserViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BTSDbContext context = new BTSDbContext();
                    UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim> store = new UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(context);
                    //UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(store);

                    string hashedNewPassword = UserManager.PasswordHasher.HashPassword(Item.Password);
                    ApplicationUser cUser = await store.FindByIdAsync(Item.Id);
                    cUser.UpdatedBy = User.Identity.Name;
                    cUser.UpdatedDate = DateTime.Now;

                    await store.SetPasswordHashAsync(cUser, hashedNewPassword);
                    await store.UpdateAsync(cUser);
                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
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

        [AuthorizeRoles(CommonConstants.System_CanLock_Role)]
        public async Task<ActionResult> Lock(string id)
        {
            try
            {
                ApplicationUser appUser = await UserManager.FindByIdAsync(id);
                appUser.Locked = !appUser.Locked;
                appUser.UpdatedBy = User.Identity.Name;
                appUser.UpdatedDate = DateTime.Now;

                IdentityResult result = await UserManager.UpdateAsync(appUser);
                if (result.Succeeded)
                {
                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Locked/UnLocked Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = CommonConstants.Status_Error, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = string.Join(",", result.Errors) }, JsonRequestBehavior.AllowGet);
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
                ApplicationUser appUser = await UserManager.FindByIdAsync(id);
                if (appUser == null)
                {
                    return HttpNotFound();
                }

                if (_appGroupService.GetGroupsByUserId(id) != null)
                {
                    return Json(new { status = CommonConstants.Status_Error, message = "Không thể xóa Người dùng còn thuộc nhóm" }, JsonRequestBehavior.AllowGet);
                }

                IdentityResult result = await UserManager.DeleteAsync(appUser);
                if (result.Succeeded)
                {
                    return Json(new { data_restUrl = "/ApplicationUser/Add", status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = CommonConstants.Status_Error, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = string.Join(",", result.Errors) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}