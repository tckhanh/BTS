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
    public class ApplicantController : BaseController
    {
        private IApplicantService _applicantService;

        public ApplicantController(IErrorService errorService, IApplicantService applicantService) : base(errorService)
        {
            _applicantService = applicantService;
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

        private IEnumerable<ApplicantViewModel> GetAll()
        {
            var model = _applicantService.getAll();
            return Mapper.Map<IEnumerable<ApplicantViewModel>>(model);
        }

        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanEdit_Role)]
        public async Task<ActionResult> AddOrEdit(string act, string id = "0")
        {
            ApplicantViewModel ItemVm = new ApplicantViewModel();

            if ((act == CommonConstants.Action_Detail || act == CommonConstants.Action_Edit) && !string.IsNullOrEmpty(id))
            {
                var DbItem = _applicantService.getByID(id);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<ApplicantViewModel>(DbItem);
                }
                IEnumerable<Operator> operatorList = _applicantService.GetAllOperator();
                foreach (var operatorItem in operatorList)
                {
                    var listItem = new SelectListItem()
                    {
                        Text = operatorItem.Name,
                        Value = operatorItem.Id,
                        Selected = false
                    };
                    ItemVm.OperatorList.Add(listItem);
                }
                if (act == CommonConstants.Action_Edit)
                    return View("Edit", ItemVm);
                else
                    return View("Detail", ItemVm);
            }
            else
            {
                IEnumerable<Operator> operatorList = _applicantService.GetAllOperator();
                foreach (var operatorItem in operatorList)
                {
                    var listItem = new SelectListItem()
                    {
                        Text = operatorItem.Name,
                        Value = operatorItem.Id,
                        Selected = false
                    };
                    ItemVm.OperatorList.Add(listItem);
                }
                return View("Add", ItemVm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanEdit_Role)]
        public async Task<ActionResult> AddOrEdit(string act, ApplicantViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (act == CommonConstants.Action_Add)
                    {
                        var newItem = new Applicant();
                        newItem.UpdateApplicant(Item);
                        newItem.Id = Item.Id;

                        newItem.CreatedBy = User.Identity.Name;
                        newItem.CreatedDate = DateTime.Now;

                        _applicantService.Add(newItem);
                        _applicantService.SaveChanges();
                        return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ApplicantViewModel>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var editItem = _applicantService.getByID(Item.Id);
                        editItem.UpdateApplicant(Item);
                        editItem.UpdatedBy = User.Identity.Name;
                        editItem.UpdatedDate = DateTime.Now;

                        _applicantService.Update(editItem);
                        _applicantService.SaveChanges();
                        return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<ApplicantViewModel>>(GetAll())), message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> Delete(string id = "0")
        {
            try
            {
                var dbItem = _applicantService.getByID(id);
                if (dbItem == null)
                {
                    return HttpNotFound();
                }

                if (_applicantService.IsUsed(id))
                {
                    return Json(new { status = CommonConstants.Status_Error, message = "Không thể xóa Trường hợp kiểm định này do đã được sử dụnd" }, JsonRequestBehavior.AllowGet);
                }

                _applicantService.Delete(id);
                _applicantService.SaveChanges();

                return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()), message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}