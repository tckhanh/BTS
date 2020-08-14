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
using System.Web.SessionState;

namespace BTS.Web.Areas.Controllers
{
    [AuthorizeRoles(CommonConstants.Data_CanView_Role)]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CertificateController : BaseController
    {
        private ICertificateService _certificateService;
        private IOperatorService _operatorService;
        private IProfileService _profileService;
        private ICityService _cityService;
        private IInCaseOfService _inCaseOfService;
        private ILabService _labService;


        public CertificateController(ICertificateService certificateService, IOperatorService operatorService, IProfileService profileService, ICityService cityService, IInCaseOfService inCaseOfService, ILabService labService, IErrorService errorService) : base(errorService)
        {
            _certificateService = certificateService;
            _operatorService = operatorService;
            _profileService = profileService;
            _cityService = cityService;
            _inCaseOfService = inCaseOfService;
            _labService = labService;
        }

        // GET: Certificate
        public ActionResult Index()
        {
            IEnumerable<OperatorViewModel> operators = Mapper.Map<List<OperatorViewModel>>(_operatorService.getAll()).ToList();
            IEnumerable<ProfileViewModel> profiles = Mapper.Map<List<ProfileViewModel>>(_profileService.getAll().OrderByDescending(x => x.ApplyDate)).ToList();
            IEnumerable<CityViewModel> cities = Mapper.Map<List<CityViewModel>>(_cityService.getAll()).ToList();

            cities = cities.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.Id));

            ViewBag.operators = operators;
            ViewBag.profiles = profiles;
            ViewBag.cities = cities;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult loadCertificate()
        {
            int countItem;

            string CityID = Request.Form.GetValues("CityID")?.FirstOrDefault();
            string OperatorID = Request.Form.GetValues("OperatorID")?.FirstOrDefault();
            string ProfileID = Request.Form.GetValues("ProfileID")?.FirstOrDefault();
            string CertificateNum = Request.Form.GetValues("CertificateNum")?.FirstOrDefault().ToUpper();
            string BtsCodeOrAddress = Request.Form.GetValues("BtsCodeOrAddress")?.FirstOrDefault().ToLower();
            string IsExpired = Request.Form.GetValues("IsExpired")?.FirstOrDefault().ToLower();
            DateTime StartDate, EndDate;

            if (!DateTime.TryParse(Request.Form.GetValues("StartDate")?.FirstOrDefault(), out StartDate))
            {
                Console.Write("Loi chuyen doi kieu");
            }

            if (!DateTime.TryParse(Request.Form.GetValues("EndDate")?.FirstOrDefault(), out EndDate))
            {
                Console.Write("Loi chuyen doi kieu");
            }

            // searching ...
            IEnumerable<Certificate> Items;


            if (!(string.IsNullOrEmpty(CertificateNum)))
            {
                Items = _certificateService.getCertificateByCertificateNum(CertificateNum).ToList();
            }
            else if (!(string.IsNullOrEmpty(CityID)))
            {
                Items = _certificateService.getCertificateByCity(CityID).ToList();
            }
            else if (!(string.IsNullOrEmpty(OperatorID)))
            {
                Items = _certificateService.getCertificateByOperator(OperatorID).ToList();
            }
            else if (!(string.IsNullOrEmpty(ProfileID)))
            {
                Items = _certificateService.getCertificateByProfile(ProfileID).ToList();
            }
            else if (!(string.IsNullOrEmpty(BtsCodeOrAddress)))
            {
                Items = _certificateService.getCertificateByBtsCodeOrAddress(BtsCodeOrAddress).ToList();
            }
            else if (StartDate != null && EndDate != null)
            {
                Items = _certificateService.getAll(out countItem, false, StartDate, EndDate).ToList();
            }
            else
            {
                Items = _certificateService.getAll(out countItem, false).ToList();
            }


            if (StartDate != null && EndDate != null)
            {
                Items = Items.Where(x => x.IssuedDate >= StartDate && x.IssuedDate <= EndDate).ToList();
            }


            if (!(string.IsNullOrEmpty(CertificateNum)))
            {
                Items = Items.Where(x => x.Id.Contains(CertificateNum)).ToList();
            }

            if (!(string.IsNullOrEmpty(BtsCodeOrAddress)))
            {
                Items = Items.Where(x => x.BtsCode.ToLower().Contains(BtsCodeOrAddress) || x.Address.ToLower().Contains(BtsCodeOrAddress)).ToList();
            }


            if (IsExpired == "yes")
            {
                Items = Items.Where(x => x.ExpiredDate < DateTime.Today).ToList();
            }
            else
            {
                Items = Items.Where(x => x.ExpiredDate >= DateTime.Today).ToList();
            }

            if (!(string.IsNullOrEmpty(CityID)))
            {
                Items = Items.Where(x => x.CityID == CityID).ToList();
            }

            if (!(string.IsNullOrEmpty(OperatorID)))
            {
                Items = Items.Where(x => x.OperatorID.Contains(OperatorID)).ToList();
            }

            if (!(string.IsNullOrEmpty(ProfileID)))
            {
                Items = Items.Where(x => x.ProfileID?.ToString() == ProfileID).ToList();
            }

            Items = Items.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID)).ToList();

            //Items = Items.OrderByDescending(x => x.IssuedDate.Year.ToString() + x.Id);

            IEnumerable<CertificateViewModel> dataViewModel = Mapper.Map<List<CertificateViewModel>>(Items);

            int recordsFiltered = Items.Count();
            //for (int i = 0; i < recordsFiltered; i++)
            //{
            //    IEnumerable<SubBtsInCert> SubItems = _certificateService.getDetailByID(dataViewModel.ElementAt(i).Id);
            //    IEnumerable<SubBtsInCertViewModel> SubItemsVM = Mapper.Map<List<SubBtsInCertViewModel>>(SubItems);
            //    foreach (var subItemVM in SubItemsVM)
            //    {
            //        dataViewModel.ElementAt(i).SubBtsList.Add(subItemVM);
            //    }
            //}

            if (recordsFiltered > 0)
            {
                //var tbcat = from c in dataViewModel select new { c.Id, c.title, c.descriptions, action = "<a href='" + Url.Action("edit", "Category", new { id = c.Id }) + "'>Edit</a> | <a href='javascript:;' onclick='MyStore.Delete(" + c.Id + ")'>Delete</a>" };
                JsonResult result = Json(new { data = dataViewModel }, JsonRequestBehavior.AllowGet);
                result.MaxJsonLength = int.MaxValue;
                return result;
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRoles(CommonConstants.Data_CanViewDetail_Role)]
        public JsonResult SubDetail(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                IEnumerable<SubBtsInCert> Items = _certificateService.getDetailByID(Id);

                int countItem = Items.Count();

                IEnumerable<SubBtsInCertViewModel> dataViewModel = Mapper.Map<List<SubBtsInCertViewModel>>(Items);
                if (countItem > 0)
                {
                    return Json(new { status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "SubDetail", dataViewModel), message = "Lấy dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = CommonConstants.Status_Error, message = "Không có dữ liệu" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { status = CommonConstants.Status_Error, message = "Lỗi lấy dữ liệu" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PivotTable()
        {
            return View();
        }

        public ActionResult GetCertificateByCity(string cityID)
        {
            IEnumerable<Certificate> Items = _certificateService.getCertificateByCity(cityID);
            Items = Items.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID)).ToList();

            IEnumerable<CertificateViewModel> model = Mapper.Map<IEnumerable<Certificate>, IEnumerable<CertificateViewModel>>(Items);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCertificate()
        {
            int countBTS;
            List<Certificate> dataSumary = _certificateService.getAll(out countBTS, true).ToList();

            return Json(dataSumary, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ViewAll()
        {
            return View(GetAll());
        }

        private IEnumerable<CertificateViewModel> GetAll()
        {
            int rows;
            List<Certificate> model = _certificateService.getAll(out rows, false).ToList();
            return Mapper.Map<IEnumerable<CertificateViewModel>>(model);
        }

        private CertificateViewModel FillInCertificateVM(CertificateViewModel ItemVm)
        {
            IEnumerable<Operator> operatorList = _operatorService.getAll().ToList();
            foreach (Operator operatorItem in operatorList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = operatorItem.Name,
                    Value = operatorItem.Id,
                    Selected = false
                };
                ItemVm.OperatorList.Add(listItem);
            }

            IEnumerable<Model.Models.Profile> profileList = _profileService.getAll().ToList();
            foreach (Model.Models.Profile profileItem in profileList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = profileItem.BtsQuantity + "-" + profileItem.ApplicantID + "-" + profileItem.ProfileNum,
                    Value = profileItem.Id?.ToString(),
                    Selected = false
                };
                ItemVm.ProfileList.Add(listItem);
            }

            IEnumerable<City> cityList = _cityService.getAll().ToList();
            foreach (City cityItem in cityList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = cityItem.Name,
                    Value = cityItem.Id,
                    Selected = false
                };
                ItemVm.CityList.Add(listItem);
            }

            IEnumerable<InCaseOf> inCaseOfList = _inCaseOfService.getAll().ToList();
            foreach (InCaseOf inCaseOfItem in inCaseOfList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = inCaseOfItem.Name,
                    Value = inCaseOfItem.Id.ToString(),
                    Selected = false
                };
                ItemVm.InCaseOfList.Add(listItem);
            }

            IEnumerable<Lab> labList = _labService.getAll().ToList();
            foreach (Lab labItem in labList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = labItem.Name,
                    Value = labItem.Id.ToString(),
                    Selected = false
                };
                ItemVm.LabList.Add(listItem);
            }
            return ItemVm;
        }

        public ActionResult Add()
        {
            CertificateViewModel ItemVm = new CertificateViewModel();
            ItemVm = FillInCertificateVM(ItemVm);
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanViewDetail_Role)]
        public ActionResult Detail(string id = "0")
        {
            CertificateViewModel ItemVm = new CertificateViewModel();
            Certificate DbItem = _certificateService.getByID(id);

            if (DbItem != null)
            {
                ItemVm = Mapper.Map<CertificateViewModel>(DbItem);
            }
            ItemVm = FillInCertificateVM(ItemVm);
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(string id = "0")
        {
            CertificateViewModel ItemVm = new CertificateViewModel();
            Certificate DbItem = _certificateService.getByID(id);

            if (DbItem != null)
            {
                ItemVm = Mapper.Map<CertificateViewModel>(DbItem);
            }
            ItemVm = FillInCertificateVM(ItemVm);
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, string id = "0")
        {
            CertificateViewModel ItemVm = new CertificateViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                Certificate DbItem = _certificateService.getByID(id);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<CertificateViewModel>(DbItem);
                }
            }

            IEnumerable<Operator> operatorList = _operatorService.getAll().ToList();
            foreach (Operator operatorItem in operatorList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = operatorItem.Name,
                    Value = operatorItem.Id,
                    Selected = false
                };
                ItemVm.OperatorList.Add(listItem);
            }

            IEnumerable<Model.Models.Profile> profileList = _profileService.getAll().ToList();
            foreach (Model.Models.Profile profileItem in profileList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = profileItem.BtsQuantity + "-" + profileItem.ApplicantID + "-" + profileItem.ProfileNum,
                    Value = profileItem.Id?.ToString(),
                    Selected = false
                };
                ItemVm.ProfileList.Add(listItem);
            }

            IEnumerable<City> cityList = _cityService.getAll().ToList();
            foreach (City cityItem in cityList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = cityItem.Name,
                    Value = cityItem.Id,
                    Selected = false
                };
                ItemVm.CityList.Add(listItem);
            }

            IEnumerable<InCaseOf> inCaseOfList = _inCaseOfService.getAll().ToList();
            foreach (InCaseOf inCaseOfItem in inCaseOfList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = inCaseOfItem.Name,
                    Value = inCaseOfItem.Id.ToString(),
                    Selected = false
                };
                ItemVm.InCaseOfList.Add(listItem);
            }

            IEnumerable<Lab> labList = _labService.getAll().ToList();
            foreach (Lab labItem in labList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Text = labItem.Name,
                    Value = labItem.Id.ToString(),
                    Selected = false
                };
                ItemVm.LabList.Add(listItem);
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
        public ActionResult Add(CertificateViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    string[] SubBtsAntenHeights, SubBtsAntenNums, SubBtsBands, SubBtsCodes, SubBtsConfigurations, SubBtsEquipments, SubBtsOperatorIDs, SubBtsPowerSums;

                    SubBtsCodes = Item.SubBtsCodes.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsCodes.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Mã trạm của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsOperatorIDs = Item.SubBtsOperatorIDs.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsOperatorIDs.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Mã Doanh nghiệp CCDV của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsEquipments = Item.SubBtsEquipments.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsEquipments.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Thiết bị phát của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsAntenNums = Item.SubBtsAntenNums.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsAntenNums.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Số các Anten của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsConfigurations = Item.SubBtsConfigurations.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsConfigurations.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Số máy phát, thu phát của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsBands = Item.SubBtsBands.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsBands.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Băng tần của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsPowerSums = Item.SubBtsPowerSums.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsPowerSums.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Tổng công suất phát các Anten của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsAntenHeights = Item.SubBtsAntenHeights.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsAntenHeights.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Độ cao các Anten của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    Certificate newItem = new Certificate();

                    if (_certificateService.getByID(Item.Id) != null)
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Giấy Chứng nhận kiểm định số " + Item.Id + " đã tôn tại rồi" }, JsonRequestBehavior.AllowGet);
                    }

                    if (_certificateService.findCertificate(Item.BtsCode, Item.ProfileID) != null)
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Trạm gốc " + Item.BtsCode + " trong hồ sơ đã được cấp Giấy CNKĐ rồi (số: " + Item.Id + ")" }, JsonRequestBehavior.AllowGet);
                    }

                    newItem.UpdateCertificate(Item);

                    newItem.Id = Item.Id;
                    newItem.CreatedBy = User.Identity.Name;
                    newItem.CreatedDate = DateTime.Now;

                    _certificateService.Add(newItem);
                    _certificateService.SaveChanges();

                    for (int j = 0; j < Item.SubBtsQuantity; j++)
                    {
                        SubBtsInCert subBtsItem = new SubBtsInCert
                        {
                            CertificateID = Item.Id,
                            BtsSerialNo = j + 1,
                            BtsCode = SubBtsCodes[j],
                            OperatorID = SubBtsOperatorIDs[j],
                            AntenHeight = SubBtsAntenHeights[j],
                            AntenNum = int.Parse(SubBtsAntenNums[j]),
                            Band = SubBtsBands[j],
                            Configuration = SubBtsConfigurations[j],
                            Equipment = SubBtsEquipments[j]
                        };
                        if (subBtsItem.Equipment.IndexOf(' ') >= 0)
                        {
                            subBtsItem.Manufactory = subBtsItem.Equipment.Substring(0, subBtsItem.Equipment.IndexOf(' '));
                        }
                        else
                        {
                            subBtsItem.Manufactory = SubBtsEquipments[j];
                        }
                        subBtsItem.PowerSum = SubBtsPowerSums[j];

                        subBtsItem.CreatedBy = User.Identity.Name;
                        subBtsItem.CreatedDate = DateTime.Now;


                        _certificateService.Add(subBtsItem);
                        _certificateService.SaveChanges();
                    }

                    return Json(new { status = CommonConstants.Status_Success, message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    // html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<BtsViewModel>>(GetAll()))
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
        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(CertificateViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string[] SubBtsAntenHeights, SubBtsAntenNums, SubBtsBands, SubBtsCodes, SubBtsConfigurations, SubBtsEquipments, SubBtsOperatorIDs, SubBtsPowerSums;

                    SubBtsCodes = Item.SubBtsCodes.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsCodes.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Mã trạm của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsOperatorIDs = Item.SubBtsOperatorIDs.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsOperatorIDs.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Mã Doanh nghiệp CCDV của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsEquipments = Item.SubBtsEquipments.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsEquipments.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Thiết bị phát của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsAntenNums = Item.SubBtsAntenNums.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsAntenNums.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Số các Anten của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsConfigurations = Item.SubBtsConfigurations.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsConfigurations.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Số máy phát, thu phát của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsBands = Item.SubBtsBands.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsBands.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Băng tần của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsPowerSums = Item.SubBtsPowerSums.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsPowerSums.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Tổng công suất phát các Anten của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsAntenHeights = Item.SubBtsAntenHeights.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsAntenHeights.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Độ cao các Anten của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }


                    Certificate editItem = _certificateService.getByID(Item.Id);
                    editItem.UpdateCertificate(Item);
                    editItem.UpdatedBy = User.Identity.Name;
                    editItem.UpdatedDate = DateTime.Now;

                    _certificateService.Update(editItem);
                    _certificateService.SaveChanges();

                    _certificateService.DeleteSubBTSinCert(Item.Id);
                    _certificateService.SaveChanges();

                    for (int j = 0; j < Item.SubBtsQuantity; j++)
                    {
                        SubBtsInCert subBtsItem = new SubBtsInCert
                        {
                            CertificateID = Item.Id,
                            BtsSerialNo = j + 1,
                            BtsCode = SubBtsCodes[j],
                            OperatorID = SubBtsOperatorIDs[j],
                            AntenHeight = SubBtsAntenHeights[j],
                            AntenNum = int.Parse(SubBtsAntenNums[j]),
                            Band = SubBtsBands[j],
                            Configuration = SubBtsConfigurations[j],
                            Equipment = SubBtsEquipments[j]
                        };
                        if (subBtsItem.Equipment.IndexOf(' ') >= 0)
                        {
                            subBtsItem.Manufactory = subBtsItem.Equipment.Substring(0, subBtsItem.Equipment.IndexOf(' '));
                        }
                        else
                        {
                            subBtsItem.Manufactory = SubBtsEquipments[j];
                        }
                        subBtsItem.PowerSum = SubBtsPowerSums[j];

                        subBtsItem.CreatedBy = User.Identity.Name;
                        subBtsItem.CreatedDate = DateTime.Now;


                        _certificateService.Add(subBtsItem);
                        _certificateService.SaveChanges();
                    }
                    return Json(new { status = CommonConstants.Status_Success, message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    // html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<BtsViewModel>>(GetAll()))
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
        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, CertificateViewModel Item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string[] SubBtsAntenHeights, SubBtsAntenNums, SubBtsBands, SubBtsCodes, SubBtsConfigurations, SubBtsEquipments, SubBtsOperatorIDs, SubBtsPowerSums;

                    SubBtsCodes = Item.SubBtsCodes.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsCodes.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Mã trạm của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsOperatorIDs = Item.SubBtsOperatorIDs.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsOperatorIDs.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Mã Doanh nghiệp CCDV của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsEquipments = Item.SubBtsEquipments.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsEquipments.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Thiết bị phát của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsAntenNums = Item.SubBtsAntenNums.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsAntenNums.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Số các Anten của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsConfigurations = Item.SubBtsConfigurations.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsConfigurations.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Số máy phát, thu phát của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsBands = Item.SubBtsBands.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsBands.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Băng tần của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsPowerSums = Item.SubBtsPowerSums.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsPowerSums.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Tổng công suất phát các Anten của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsAntenHeights = Item.SubBtsAntenHeights.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsAntenHeights.Count())
                    {
                        return Json(new { status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Độ cao các Anten của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    if (act == CommonConstants.Action_Add)
                    {
                        Certificate newItem = new Certificate();

                        if (_certificateService.getByID(Item.Id) != null)
                        {
                            return Json(new { status = CommonConstants.Status_Error, message = "Giấy Chứng nhận kiểm định số " + Item.Id + " đã tôn tại rồi" }, JsonRequestBehavior.AllowGet);
                        }

                        if (_certificateService.findCertificate(Item.BtsCode, Item.ProfileID) != null)
                        {
                            return Json(new { status = CommonConstants.Status_Error, message = "Trạm gốc " + Item.BtsCode + " trong hồ sơ đã được cấp Giấy CNKĐ rồi (số: " + Item.Id + ")" }, JsonRequestBehavior.AllowGet);
                        }

                        newItem.UpdateCertificate(Item);

                        newItem.Id = Item.Id;
                        newItem.CreatedBy = User.Identity.Name;
                        newItem.CreatedDate = DateTime.Now;

                        _certificateService.Add(newItem);
                        _certificateService.SaveChanges();

                        for (int j = 0; j < Item.SubBtsQuantity; j++)
                        {
                            SubBtsInCert subBtsItem = new SubBtsInCert
                            {
                                CertificateID = Item.Id,
                                BtsSerialNo = j + 1,
                                BtsCode = SubBtsCodes[j],
                                OperatorID = SubBtsOperatorIDs[j],
                                AntenHeight = SubBtsAntenHeights[j],
                                AntenNum = int.Parse(SubBtsAntenNums[j]),
                                Band = SubBtsBands[j],
                                Configuration = SubBtsConfigurations[j],
                                Equipment = SubBtsEquipments[j]
                            };
                            if (subBtsItem.Equipment.IndexOf(' ') >= 0)
                            {
                                subBtsItem.Manufactory = subBtsItem.Equipment.Substring(0, subBtsItem.Equipment.IndexOf(' '));
                            }
                            else
                            {
                                subBtsItem.Manufactory = SubBtsEquipments[j];
                            }
                            subBtsItem.PowerSum = SubBtsPowerSums[j];

                            subBtsItem.CreatedBy = User.Identity.Name;
                            subBtsItem.CreatedDate = DateTime.Now;


                            _certificateService.Add(subBtsItem);
                            _certificateService.SaveChanges();
                        }

                        return Json(new { status = CommonConstants.Status_Success, message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                        // html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<BtsViewModel>>(GetAll()))
                    }
                    else
                    {
                        Certificate editItem = _certificateService.getByID(Item.Id);
                        editItem.UpdateCertificate(Item);
                        editItem.UpdatedBy = User.Identity.Name;
                        editItem.UpdatedDate = DateTime.Now;

                        _certificateService.Update(editItem);
                        _certificateService.SaveChanges();

                        _certificateService.DeleteSubBTSinCert(Item.Id);
                        _certificateService.SaveChanges();

                        for (int j = 0; j < Item.SubBtsQuantity; j++)
                        {
                            SubBtsInCert subBtsItem = new SubBtsInCert
                            {
                                CertificateID = Item.Id,
                                BtsSerialNo = j + 1,
                                BtsCode = SubBtsCodes[j],
                                OperatorID = SubBtsOperatorIDs[j],
                                AntenHeight = SubBtsAntenHeights[j],
                                AntenNum = int.Parse(SubBtsAntenNums[j]),
                                Band = SubBtsBands[j],
                                Configuration = SubBtsConfigurations[j],
                                Equipment = SubBtsEquipments[j]
                            };
                            if (subBtsItem.Equipment.IndexOf(' ') >= 0)
                            {
                                subBtsItem.Manufactory = subBtsItem.Equipment.Substring(0, subBtsItem.Equipment.IndexOf(' '));
                            }
                            else
                            {
                                subBtsItem.Manufactory = SubBtsEquipments[j];
                            }
                            subBtsItem.PowerSum = SubBtsPowerSums[j];

                            subBtsItem.CreatedBy = User.Identity.Name;
                            subBtsItem.CreatedDate = DateTime.Now;


                            _certificateService.Add(subBtsItem);
                            _certificateService.SaveChanges();
                        }

                        return Json(new { status = CommonConstants.Status_Success, message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                        // html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<BtsViewModel>>(GetAll()))
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

        [AuthorizeRoles(CommonConstants.Data_CanDelete_Role)]
        public ActionResult Delete(string id = "0")
        {
            try
            {
                Certificate dbItem = _certificateService.getByID(id);
                if (dbItem == null)
                {
                    return HttpNotFound();
                }

                //if (_btsService.IsUsed(id))
                //{
                //    return Json(new { status = CommonConstants.Status_Error, message = "Không thể xóa Trường hợp kiểm định này do đã được sử dụnd" }, JsonRequestBehavior.AllowGet);
                //}

                _certificateService.Delete(id);
                _certificateService.SaveChanges();

                return Json(new { data_restUrl = "/Certificate/Add", status = CommonConstants.Status_Success, message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                // html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()),
            }
            catch (Exception ex)
            {
                return Json(new { status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}