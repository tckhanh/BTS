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
using System.Web.Mvc;

namespace BTS.Web.Areas.Controllers
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
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAll());
        }

        private IEnumerable<ProfileViewModel> GetAll()
        {
            List<Model.Models.Profile> model = _profileService.getAll().ToList();
            return Mapper.Map<IEnumerable<ProfileViewModel>>(model);
        }

        public ActionResult Add()
        {
            ProfileViewModel ItemVm = new ProfileViewModel();

            IEnumerable<Applicant> applicantList = _profileService.getAllApplicant().ToList();
            foreach (Applicant applicantItem in applicantList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = applicantItem.Name,
                    Value = applicantItem.Id,
                    Selected = false
                };
                ItemVm.ApplicantList.Add(listItem);
            }
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanViewDetail_Role)]
        public ActionResult Detail(string id = "")
        {
            ProfileViewModel ItemVm = new ProfileViewModel();

            Model.Models.Profile DbItem = _profileService.getByID(id);

            if (DbItem != null)
            {
                ItemVm = Mapper.Map<ProfileViewModel>(DbItem);
            }

            IEnumerable<Applicant> applicantList = _profileService.getAllApplicant().ToList();
            foreach (Applicant applicantItem in applicantList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = applicantItem.Name,
                    Value = applicantItem.Id,
                    Selected = false
                };
                ItemVm.ApplicantList.Add(listItem);
            }
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(string id = "")
        {
            ProfileViewModel ItemVm = new ProfileViewModel();

            Model.Models.Profile DbItem = _profileService.getByID(id);

            if (DbItem != null)
            {
                ItemVm = Mapper.Map<ProfileViewModel>(DbItem);
            }

            IEnumerable<Applicant> applicantList = _profileService.getAllApplicant().ToList();
            foreach (Applicant applicantItem in applicantList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = applicantItem.Name,
                    Value = applicantItem.Id,
                    Selected = false
                };
                ItemVm.ApplicantList.Add(listItem);
            }
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, string id = "")
        {
            ProfileViewModel ItemVm = new ProfileViewModel();

            if (!string.IsNullOrEmpty(id))
            {
                Model.Models.Profile DbItem = _profileService.getByID(id);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<ProfileViewModel>(DbItem);
                }
            }

            IEnumerable<Applicant> applicantList = _profileService.getAllApplicant().ToList();
            foreach (Applicant applicantItem in applicantList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = applicantItem.Name,
                    Value = applicantItem.Id,
                    Selected = false
                };
                ItemVm.ApplicantList.Add(listItem);
            }

            if (act == CommonConstants.Action_Edit)
            {
                return View("Edit", ItemVm);
            }
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
        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role)]
        public ActionResult Add(ProfileViewModel ItemVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Model.Models.Profile newItem = new Model.Models.Profile();
                    newItem.UpdateProfile(ItemVm);
                    newItem.Id = ItemVm.Id;

                    newItem.CreatedBy = User.Identity.Name;
                    newItem.CreatedDate = DateTime.Now;

                    _profileService.Add(newItem);
                    _profileService.Save();
                    return Json(new { resetUrl = Url.Action("Add", "Profile"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ProfileViewModel>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "Profile"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Profile"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(ProfileViewModel ItemVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Model.Models.Profile editItem = _profileService.getByID(ItemVm.Id);
                    editItem.UpdateProfile(ItemVm);
                    editItem.UpdatedBy = User.Identity.Name;
                    editItem.UpdatedDate = DateTime.Now;

                    _profileService.Update(editItem);
                    _profileService.Save();
                    return Json(new { resetUrl = Url.Action("Add", "Profile"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ProfileViewModel>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "Profile"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Profile"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, ProfileViewModel ItemVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (act == CommonConstants.Action_Add)
                    {
                        Model.Models.Profile newItem = new Model.Models.Profile();
                        newItem.UpdateProfile(ItemVm);
                        newItem.Id = ItemVm.Id;

                        newItem.CreatedBy = User.Identity.Name;
                        newItem.CreatedDate = DateTime.Now;

                        _profileService.Add(newItem);
                        _profileService.Save();
                        return Json(new { resetUrl = Url.Action("Add", "Profile"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ProfileViewModel>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Model.Models.Profile editItem = _profileService.getByID(ItemVm.Id);
                        editItem.UpdateProfile(ItemVm);
                        editItem.UpdatedBy = User.Identity.Name;
                        editItem.UpdatedDate = DateTime.Now;

                        _profileService.Update(editItem);
                        _profileService.Save();
                        return Json(new { resetUrl = Url.Action("Add", "Profile"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ProfileViewModel>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "Profile"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Profile"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRoles(CommonConstants.Data_CanDelete_Role)]
        public ActionResult Delete(string id = "")
        {
            try
            {
                Model.Models.Profile dbItem = _profileService.getByID(id);
                if (dbItem == null)
                {
                    return HttpNotFound();
                }

                if (_profileService.IsUsed(id))
                {
                    return Json(new { resetUrl = Url.Action("Add", "Profile"), status = CommonConstants.Status_Error, message = "Không thể xóa Trường hợp kiểm định này do đã được sử dụnd" }, JsonRequestBehavior.AllowGet);
                }

                _profileService.Delete(id);
                _profileService.Save();

                return Json(new { resetUrl = Url.Action("Add", "Profile"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "Profile"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}