using AutoMapper;
using BTS.Common;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Infrastructure.Extensions;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Controllers
{
    [AuthorizeRoles(CommonConstants.Data_CanView_Role)]
    public class ProfileController : BaseController
    {
        private IProfileService _profileService;

        public ProfileController(IErrorService errorService, IProfileService profileService) : base(errorService)
        {
            _profileService = profileService;
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

        private IEnumerable<ProfileViewModel> GetAll()
        {
            var model = _profileService.getAll();
            return Mapper.Map<IEnumerable<ProfileViewModel>>(model);
        }

        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanEdit_Role)]
        public async Task<ActionResult> AddOrEdit(string act, string id = "")
        {
            ProfileViewModel ItemVm = new ProfileViewModel();

            if (!string.IsNullOrEmpty(id))
            {
                var DbItem = _profileService.getByID(id);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<ProfileViewModel>(DbItem);
                }
            }

            IEnumerable<Applicant> applicantList = _profileService.getAllApplicant();
            foreach (var applicantItem in applicantList)
            {
                var listItem = new SelectListItem()
                {
                    Text = applicantItem.Name,
                    Value = applicantItem.Id,
                    Selected = false
                };
                ItemVm.ApplicantList.Add(listItem);
            }

            if (act == CommonConstants.Action_Edit)
                return View("Edit", ItemVm);
            else if (act == CommonConstants.Action_Detail)
            {
                return View("Detail", ItemVm);
            }
            else
            {
                return View("Add", ItemVm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanEdit_Role)]
        public async Task<ActionResult> AddOrEdit(string act, ProfileViewModel ItemVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (act == CommonConstants.Action_Add)
                    {
                        var newItem = new Model.Models.Profile();
                        newItem.UpdateProfile(ItemVm);
                        newItem.Id = ItemVm.Id;

                        newItem.CreatedBy = User.Identity.Name;
                        newItem.CreatedDate = DateTime.Now;

                        _profileService.Add(newItem);
                        _profileService.Save();
                        return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ProfileViewModel>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var editItem = _profileService.getByID(ItemVm.Id);
                        editItem.UpdateProfile(ItemVm);
                        editItem.UpdatedBy = User.Identity.Name;
                        editItem.UpdatedDate = DateTime.Now;

                        _profileService.Update(editItem);
                        _profileService.Save();
                        return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ProfileViewModel>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
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

        [AuthorizeRoles(CommonConstants.Data_CanDelete_Role)]
        public async Task<ActionResult> Delete(string id = "")
        {
            try
            {
                var dbItem = _profileService.getByID(id);
                if (dbItem == null)
                {
                    return HttpNotFound();
                }

                if (_profileService.IsUsed(id))
                {
                    return Json(new { status = CommonConstants.Status_Error, message = "Không thể xóa Trường hợp kiểm định này do đã được sử dụnd" }, JsonRequestBehavior.AllowGet);
                }

                _profileService.Delete(id);
                _profileService.Save();

                return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}