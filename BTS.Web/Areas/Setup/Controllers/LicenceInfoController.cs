using AutoMapper;
using BTS.Common;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Areas.Controllers;
using BTS.Web.Controllers;
using BTS.Web.Infrastructure.Core;
using BTS.Web.Infrastructure.Extensions;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BTS.Web.Areas.Setup.Controllers
{    
    [AllowAnonymous]
    public class LicenceInfoController : BaseController
    {
        private ILicenceService _licenceService;


        public LicenceInfoController(IErrorService errorService, ILicenceService licenceService) : base(errorService)
        {
            _licenceService = licenceService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAll());
        }

        private IEnumerable<LicenceViewModel> GetAll()
        {
            List<Licence> model = _licenceService.getAll().ToList();
            List<LicenceViewModel> viewModel = new List<LicenceViewModel>();
            foreach (var item in model)
            {
                viewModel.Add(checkLicence.GetLicenceInfo(item));
            }
            return viewModel;
        }

        public ActionResult Add()
        {
            LicenceViewModel ItemVm = new LicenceViewModel();
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.System_CanViewDetail_Role)]
        public ActionResult Detail(string id = "0")
        {
            LicenceViewModel ItemVm = new LicenceViewModel();
            Licence DbItem = _licenceService.getByID(id);
            if (DbItem != null)
            {
                ItemVm = checkLicence.GetLicenceInfo(DbItem);
            }
            return View(ItemVm);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.System_CanAdd_Role)]
        public ActionResult Add(LicenceViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Licence newItem = new Licence();
                    newItem.UpdateLicence(Item);

                    newItem.CreatedBy = User.Identity.Name;
                    newItem.CreatedDate = DateTime.Now;

                    _licenceService.Add(newItem);
                    _licenceService.Save();
                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<LicenceViewModel>>(GetAll())), message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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
    }
}