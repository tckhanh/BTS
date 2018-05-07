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

namespace BTS.Web.Controllers
{
    public class ApplicationGroupController : BaseController
    {
        private IApplicationGroupService _appGroupService;
        private IApplicationRoleService _appRoleService;
        private ApplicationUserManager _userManager;

        public ApplicationGroupController(IErrorService errorService,
            IApplicationRoleService appRoleService,
            ApplicationUserManager userManager,
            IApplicationGroupService appGroupService) : base(errorService)
        {
            _appGroupService = appGroupService;
            _appRoleService = appRoleService;
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

        IEnumerable<ApplicationGroupViewModel> GetAll()
        {
            var model = _appGroupService.GetAll();
            return Mapper.Map<IEnumerable<ApplicationGroupViewModel>>(model);
        }

        public ActionResult AddOrEdit(string id = "")
        {
            ApplicationGroupViewModel Item = new ApplicationGroupViewModel();
            if (string.IsNullOrEmpty(id))
            {
                var DbItem = _appGroupService.GetDetail(id);


                if (DbItem != null)
                {
                    Item = Mapper.Map<ApplicationGroupViewModel>(DbItem);
                    var listRole = _appRoleService.GetListRoleByGroupId(Item.ID);
                    Item.Roles = Mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<ApplicationRoleViewModel>>(listRole);
                }

                ViewBag.Roles = Mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<ApplicationRoleViewModel>>(_appRoleService.GetAll());

            }
            return View(Item);
        }


        [HttpPost]
        public async Task<ActionResult> AddOrEdit(ApplicationGroupViewModel Item)
        {
            try
            {
                if (string.IsNullOrEmpty(Item.ID))
                {
                    ApplicationGroup newItem = new ApplicationGroup();
                    newItem.UpdateGroup(Item);
                    newItem.Name = Item.Name;
                    newItem.Description = Item.Description;

                    var appGroup = _appGroupService.Add(newItem);
                    _appGroupService.Save();
                    //save group

                    //add ApplicationRoleGroup
                    var listRoleGroup = new List<ApplicationRoleGroup>();
                    foreach (var role in Item.Roles)
                    {
                        listRoleGroup.Add(new ApplicationRoleGroup()
                        {
                            GroupId = appGroup.ID,
                            RoleId = role.ID
                        });
                    }
                    _appRoleService.AddRolesToGroup(listRoleGroup, appGroup.ID);
                    _appRoleService.Save();

                    return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    var appGroup = _appGroupService.GetDetail(Item.ID);

                    appGroup.UpdateApplicationGroup(Item);
                    _appGroupService.Update(appGroup);
                    //_appGroupService.Save();

                    //save group
                    var listRoleGroup = new List<ApplicationRoleGroup>();
                    foreach (var role in Item.Roles)
                    {
                        listRoleGroup.Add(new ApplicationRoleGroup()
                        {
                            GroupId = appGroup.ID,
                            RoleId = role.ID
                        });
                    }
                    _appRoleService.AddRolesToGroup(listRoleGroup, appGroup.ID);
                    _appRoleService.Save();

                    //add role to user
                    var listRole = _appRoleService.GetListRoleByGroupId(appGroup.ID);
                    var listUserInGroup = _appGroupService.GetListUserByGroupId(appGroup.ID);
                    foreach (var user in listUserInGroup)
                    {
                        var listRoleName = listRole.Select(x => x.Name).ToArray();
                        foreach (var roleName in listRoleName)
                        {
                            await _userManager.RemoveFromRoleAsync(user.Id, roleName);
                            await _userManager.AddToRoleAsync(user.Id, roleName);
                        }
                    }
                    
                    return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);

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