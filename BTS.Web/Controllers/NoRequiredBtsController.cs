using AutoMapper;
using BTS.Common;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Infrastructure.Extensions;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace BTS.Web.Areas.Controllers
{
    [AuthorizeRoles(CommonConstants.Data_CanView_Role)]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class NoRequiredBtsController : BaseController
    {
        private INoRequiredBtsService _noRequiredBtsService;
        private IOperatorService _operatorService;
        private ICityService _cityService;
        private ISubBtsInNoRequiredBtsService _subBtsInNoRequiredService;


        public NoRequiredBtsController(INoRequiredBtsService noRequiredBtsService, ISubBtsInNoRequiredBtsService subBtsInNoRequiredBtsService, IOperatorService operatorService, IProfileService profileService, ICityService cityService, IInCaseOfService inCaseOfService, ILabService labService, IErrorService errorService) : base(errorService)
        {
            _noRequiredBtsService = noRequiredBtsService;
            _subBtsInNoRequiredService = subBtsInNoRequiredBtsService;
            _operatorService = operatorService;
            _cityService = cityService;
        }

        // GET: NoRequiredBts
        public ActionResult Index()
        {
            IEnumerable<OperatorViewModel> operators = Mapper.Map<List<OperatorViewModel>>(_operatorService.getAll()).ToList();
            IEnumerable<CityViewModel> cities = Mapper.Map<List<CityViewModel>>(_cityService.getAll()).ToList();

            if (User.Identity.IsAuthenticated)
            {
                if (getEnableCityIDsScope() == "True")
                {
                    cities = cities.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.Id));
                }
            }
            else
            {
                cities = new List<CityViewModel>();
            }

            ViewBag.operators = operators;
            ViewBag.cities = cities;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult loadNoRequiredBts()
        {
            int countItem;

            string CityID = Request.Form.GetValues("CityID")?.FirstOrDefault();
            string OperatorID = Request.Form.GetValues("OperatorID")?.FirstOrDefault();
            string ProfileID = Request.Form.GetValues("ProfileID")?.FirstOrDefault();
            string NoRequiredBtsNum = Request.Form.GetValues("NoRequiredBtsNum")?.FirstOrDefault().ToUpper();
            string BtsCodeOrAddress = Request.Form.GetValues("BtsCodeOrAddress")?.FirstOrDefault().ToLower();
            string NoRequiredBtsStatus = Request.Form.GetValues("NoRequiredBtsStatus")?.FirstOrDefault();
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
            IEnumerable<NoRequiredBts> Items = new List<NoRequiredBts>();

            if (NoRequiredBtsStatus == CommonConstants.CertStatus_WaitToSign)
            {
                //Items = _noRequiredBtsService.getNoRequiredBtsWaitToSign().ToList();
            }
            else if (NoRequiredBtsStatus == CommonConstants.CertStatus_Expired)
            {
                Items = _noRequiredBtsService.getNoRequiredBtsExpired().ToList();
            }
            else if (NoRequiredBtsStatus == CommonConstants.CertStatus_Valid)
            {
                if (!(string.IsNullOrEmpty(CityID)) && CityID != CommonConstants.SelectAll)
                {
                    Items = _noRequiredBtsService.getNoRequiredBtsByCity(CityID).ToList();
                }
                else if (!(string.IsNullOrEmpty(OperatorID)) && OperatorID != CommonConstants.SelectAll)
                {
                    Items = _noRequiredBtsService.getNoRequiredBtsByOperator(OperatorID).ToList();
                }
                else if (!(string.IsNullOrEmpty(ProfileID)) && ProfileID != CommonConstants.SelectAll)
                {
                    Items = _noRequiredBtsService.getNoRequiredBtsByProfile(ProfileID).ToList();
                }
                else if (!(string.IsNullOrEmpty(BtsCodeOrAddress)))
                {
                    Items = _noRequiredBtsService.getNoRequiredBtsByBtsCodeOrAddress(BtsCodeOrAddress).ToList();
                }
                else if (NoRequiredBtsStatus == CommonConstants.CertStatus_Valid && StartDate != null && EndDate != null)
                {
                    Items = _noRequiredBtsService.getAll(out countItem, false, StartDate, EndDate).ToList();
                }
                else
                {
                    Items = _noRequiredBtsService.getAll(out countItem, false).ToList();
                }
            }


            if (NoRequiredBtsStatus == CommonConstants.CertStatus_WaitToSign)
            {
                Items = Items.Where(x=> x.IsCanceled == false);
            }
            if (NoRequiredBtsStatus == CommonConstants.CertStatus_Expired)
            {
                Items = Items.Where(x => x.IsCanceled == true);
            }

            if (NoRequiredBtsStatus == CommonConstants.CertStatus_Valid)
            {
                Items = Items.Where(x => x.IsCanceled == false);

                if (StartDate != null && EndDate != null)
                {
                    Items = Items.Where(x => x.AnnouncedDate >= StartDate && x.AnnouncedDate <= EndDate).ToList();
                }
            }

            if (!(string.IsNullOrEmpty(BtsCodeOrAddress)))
            {
                Items = Items.Where(x => x.BtsCode.ToLower().Contains(BtsCodeOrAddress) || x.Address.ToLower().Contains(BtsCodeOrAddress)).ToList();
            }

            if (!(string.IsNullOrEmpty(CityID)) && CityID != CommonConstants.SelectAll)
            {
                Items = Items.Where(x => x.CityID == CityID).ToList();
            }

            if (!(string.IsNullOrEmpty(OperatorID)) && OperatorID != CommonConstants.SelectAll)
            {
                Items = Items.Where(x => x.OperatorID.Contains(OperatorID)).ToList();
            }

            if (!(string.IsNullOrEmpty(ProfileID)) && ProfileID != CommonConstants.SelectAll)
            {
                Items = Items.Where(x => x.AnnouncedDoc == ProfileID).ToList();
            }

            if (getEnableCityIDsScope() == "True")
            {
                Items = Items.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID)).ToList();
            }
            

            //Items = Items.OrderByDescending(x => x.IssuedDate.Year.ToString() + x.Id);

            IEnumerable<NoRequiredBtsVM> dataViewModel = Mapper.Map<List<NoRequiredBtsVM>>(Items);

            int recordsFiltered = Items.Count();

            //for (int i = 0; i < recordsFiltered; i++)
            //{
            //    IEnumerable<SubBtsInNoRequiredBts> SubItems = _certificateService.getDetailByID(dataViewModel.ElementAt(i).Id);
            //    IEnumerable<SubBtsInNoRequiredBtsViewModel> SubItemsVM = Mapper.Map<List<SubBtsInNoRequiredBtsViewModel>>(SubItems);
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
                IEnumerable<SubBtsInNoRequiredBts> Items = _noRequiredBtsService.getDetailByID(Id);

                int countItem = Items.Count();

                IEnumerable<SubBtsInNoRequiredBtsVM> dataViewModel = Mapper.Map<List<SubBtsInNoRequiredBtsVM>>(Items);
                if (countItem > 0)
                {
                    return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Success, html = GlobalClass.RenderRazorViewToString(this, "SubDetail", dataViewModel), message = "Lấy dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Không có dữ liệu" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi lấy dữ liệu" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PivotTable()
        {
            return View();
        }

        public ActionResult GetNoRequiredBtsByCity(string cityID)
        {
            IEnumerable<NoRequiredBts> Items = _noRequiredBtsService.getNoRequiredBtsByCity(cityID);

            if (getEnableCityIDsScope() == "True")
            {
                Items = Items.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID)).ToList();
            }
            IEnumerable<NoRequiredBtsVM> model = Mapper.Map<IEnumerable<NoRequiredBts>, IEnumerable<NoRequiredBtsVM>>(Items);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNoRequiredBts()
        {
            int countBTS;
            List<NoRequiredBts> dataSumary = _noRequiredBtsService.getAll(out countBTS, true).ToList();

            return Json(dataSumary, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ViewAll()
        {
            return View(GetAll());
        }

        private IEnumerable<NoRequiredBtsVM> GetAll()
        {
            int rows;
            List<NoRequiredBts> model = _noRequiredBtsService.getAll(out rows, false).ToList();
            return Mapper.Map<IEnumerable<NoRequiredBtsVM>>(model);
        }

        private NoRequiredBtsVM FillInNoRequiredBtsVM(NoRequiredBtsVM ItemVm)
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
            return ItemVm;
        }

        public ActionResult Add()
        {
            NoRequiredBtsVM ItemVm = new NoRequiredBtsVM();
            ItemVm = FillInNoRequiredBtsVM(ItemVm);
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanViewDetail_Role)]
        public ActionResult Detail(string id = "0")
        {
            NoRequiredBtsVM ItemVm = new NoRequiredBtsVM();
            NoRequiredBts DbItem = _noRequiredBtsService.getByID(id);

            if (DbItem != null)
            {
                ItemVm = Mapper.Map<NoRequiredBtsVM>(DbItem);
            }
            ItemVm = FillInNoRequiredBtsVM(ItemVm);
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(string id = "0")
        {
            NoRequiredBtsVM ItemVm = new NoRequiredBtsVM();
            NoRequiredBts DbItem = _noRequiredBtsService.getByID(id);

            if (DbItem != null)
            {
                ItemVm = Mapper.Map<NoRequiredBtsVM>(DbItem);
            }
            ItemVm = FillInNoRequiredBtsVM(ItemVm);
            return View(ItemVm);
        }

        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, string id = "0")
        {
            NoRequiredBtsVM ItemVm = new NoRequiredBtsVM();
            if (!string.IsNullOrEmpty(id))
            {
                NoRequiredBts DbItem = _noRequiredBtsService.getByID(id);

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<NoRequiredBtsVM>(DbItem);
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
        public ActionResult Add(NoRequiredBtsVM Item)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    string[] SubBtsAntenHeights, SubBtsAntenNums, SubBtsBands, SubBtsCodes, SubBtsConfigurations, SubBtsEquipments, SubBtsOperatorIDs, SubBtsPowerSums;

                    SubBtsCodes = Item.SubBtsCodes.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsCodes.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Mã trạm của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsOperatorIDs = Item.SubBtsOperatorIDs.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsOperatorIDs.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Mã Doanh nghiệp CCDV của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsEquipments = Item.SubBtsEquipments.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsEquipments.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Thiết bị phát của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsAntenNums = Item.SubBtsAntenNums.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsAntenNums.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Số các Anten của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsConfigurations = Item.SubBtsConfigurations.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsConfigurations.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Số máy phát, thu phát của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsBands = Item.SubBtsBands.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsBands.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Băng tần của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsPowerSums = Item.SubBtsPowerSums.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsPowerSums.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Tổng công suất phát các Anten của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsAntenHeights = Item.SubBtsAntenHeights.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsAntenHeights.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Độ cao các Anten của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    NoRequiredBts newItem = new NoRequiredBts();

                    if (_noRequiredBtsService.getByID(Item.Id) != null)
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Giấy Chứng nhận kiểm định số " + Item.Id + " đã tôn tại rồi" }, JsonRequestBehavior.AllowGet);
                    }

                    if (_noRequiredBtsService.findNoRequiredBts(Item.BtsCode, Item.AnnouncedDoc) != null)
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Trạm gốc " + Item.BtsCode + " trong hồ sơ đã được cấp Giấy CNKĐ rồi (số: " + Item.Id + ")" }, JsonRequestBehavior.AllowGet);
                    }

                    newItem.UpdateNoRequiredBts(Item);

                    newItem.Id = Item.Id;
                    newItem.CreatedBy = User.Identity.Name;
                    newItem.CreatedDate = DateTime.Now;

                    _noRequiredBtsService.Add(newItem);
                    _noRequiredBtsService.SaveChanges();

                    for (int j = 0; j < Item.SubBtsQuantity; j++)
                    {
                        SubBtsInNoRequiredBts subBtsItem = new SubBtsInNoRequiredBts
                        {
                            Id = Item.Id,
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


                        _noRequiredBtsService.Add(subBtsItem);
                        _noRequiredBtsService.SaveChanges();
                    }

                    return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Success, message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    // html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<BtsViewModel>>(GetAll()))
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanEdit_Role)]
        public ActionResult Edit(NoRequiredBtsVM Item)
        {
            int StartIndex, StopIndex;
            string Technology;
            try
            {
                if (ModelState.IsValid)
                {
                    string[] SubBtsAntenHeights, SubBtsAntenNums, SubBtsBands, SubBtsCodes, SubBtsConfigurations, SubBtsEquipments, SubBtsOperatorIDs, SubBtsPowerSums;

                    SubBtsCodes = Item.SubBtsCodes.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsCodes.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Mã trạm của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsOperatorIDs = Item.SubBtsOperatorIDs.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsOperatorIDs.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Mã Doanh nghiệp CCDV của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsEquipments = Item.SubBtsEquipments.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsEquipments.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Thiết bị phát của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsAntenNums = Item.SubBtsAntenNums.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsAntenNums.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Số các Anten của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsConfigurations = Item.SubBtsConfigurations.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsConfigurations.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Số máy phát, thu phát của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsBands = Item.SubBtsBands.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsBands.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Băng tần của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsPowerSums = Item.SubBtsPowerSums.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsPowerSums.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Tổng công suất phát các Anten của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsAntenHeights = Item.SubBtsAntenHeights.Split(new char[] { ';' });
                    if (Item.SubBtsQuantity != SubBtsAntenHeights.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Độ cao các Anten của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }


                    NoRequiredBts editItem = _noRequiredBtsService.getByID(Item.Id);
                    editItem.UpdateNoRequiredBts(Item);
                    editItem.UpdatedBy = User.Identity.Name;
                    editItem.UpdatedDate = DateTime.Now;

                    _noRequiredBtsService.Update(editItem);
                    _noRequiredBtsService.SaveChanges();

                    _noRequiredBtsService.DeleteSubBTSinCert(Item.Id);
                    _noRequiredBtsService.SaveChanges();

                    for (int j = 0; j < Item.SubBtsQuantity; j++)
                    {
                        StartIndex = SubBtsBands[j].IndexOf("(");
                        StopIndex = SubBtsBands[j].IndexOf(")");
                        if (StartIndex > -1 && StopIndex > -1 && StopIndex > StartIndex)
                        {
                            Technology = SubBtsBands[j].Substring(StartIndex + 1, StopIndex - StartIndex - 1);
                        }
                        else
                        {
                            Technology = "";
                        }
                        int AntenNum;
                        int.TryParse(SubBtsAntenNums[j], out AntenNum);
                        SubBtsInNoRequiredBts subBtsItem = new SubBtsInNoRequiredBts
                        {
                            Id = Item.Id,
                            BtsSerialNo = j + 1,
                            BtsCode = SubBtsCodes[j],
                            OperatorID = SubBtsOperatorIDs[j],
                            AntenHeight = SubBtsAntenHeights[j],
                            AntenNum = AntenNum,
                            Band = SubBtsBands[j],
                            Technology = Technology,
                            Configuration = SubBtsConfigurations[j],
                            Equipment = SubBtsEquipments[j]
                        };
                        if (subBtsItem.Equipment.IndexOf(' ') >= 0)
                        {
                            subBtsItem.Manufactory= subBtsItem.Equipment.Substring(0, subBtsItem.Equipment.IndexOf(' '));
                        }
                        else
                        {
                            subBtsItem.Manufactory = SubBtsEquipments[j];
                        }
                        subBtsItem.PowerSum = SubBtsPowerSums[j];

                        subBtsItem.CreatedBy = User.Identity.Name;
                        subBtsItem.CreatedDate = DateTime.Now;


                        _noRequiredBtsService.Add(subBtsItem);
                        _noRequiredBtsService.SaveChanges();
                    }
                    return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Success, message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                    // html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<BtsViewModel>>(GetAll()))
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanCancel_Role)]
        public ActionResult Cancel()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CertificateNum = Request.Form.GetValues("noRequiredBtsId")?.FirstOrDefault();
                    string CanceledReason = Request.Form.GetValues("canceledReason")?.FirstOrDefault();
                    DateTime CanceledDate;

                    if (!DateTime.TryParse(Request.Form.GetValues("CanceledDate")?.FirstOrDefault(), out CanceledDate))
                    {
                        Console.Write("Loi chuyen doi kieu");
                    }

                    NoRequiredBts editItem = _noRequiredBtsService.getByID(CertificateNum);
                    editItem.IsCanceled = true;
                    editItem.CanceledReason = CanceledReason;
                    editItem.CanceledDate = CanceledDate;

                    editItem.UpdatedBy = User.Identity.Name;
                    editItem.UpdatedDate = DateTime.Now;

                    _noRequiredBtsService.Update(editItem);
                    _noRequiredBtsService.SaveChanges();

                    return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Success, message = "Thu hồi/ Hủy bỏ Giấy CNKĐ thành công" }, JsonRequestBehavior.AllowGet);
                    // html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<BtsViewModel>>(GetAll()))
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanEdit_Role)]
        public ActionResult AddOrEdit(string act, NoRequiredBtsVM ItemVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string[] SubBtsAntenHeights, SubBtsAntenNums, SubBtsBands, SubBtsCodes, SubBtsConfigurations, SubBtsEquipments, SubBtsOperatorIDs, SubBtsPowerSums;

                    SubBtsCodes = ItemVm.SubBtsCodes.Split(new char[] { ';' });
                    if (ItemVm.SubBtsQuantity != SubBtsCodes.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Mã trạm của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsOperatorIDs = ItemVm.SubBtsOperatorIDs.Split(new char[] { ';' });
                    if (ItemVm.SubBtsQuantity != SubBtsOperatorIDs.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Mã Doanh nghiệp CCDV của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsEquipments = ItemVm.SubBtsEquipments.Split(new char[] { ';' });
                    if (ItemVm.SubBtsQuantity != SubBtsEquipments.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Thiết bị phát của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsAntenNums = ItemVm.SubBtsAntenNums.Split(new char[] { ';' });
                    if (ItemVm.SubBtsQuantity != SubBtsAntenNums.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Số các Anten của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsConfigurations = ItemVm.SubBtsConfigurations.Split(new char[] { ';' });
                    if (ItemVm.SubBtsQuantity != SubBtsConfigurations.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Số máy phát, thu phát của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsBands = ItemVm.SubBtsBands.Split(new char[] { ';' });
                    if (ItemVm.SubBtsQuantity != SubBtsBands.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Băng tần của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsPowerSums = ItemVm.SubBtsPowerSums.Split(new char[] { ';' });
                    if (ItemVm.SubBtsQuantity != SubBtsPowerSums.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Tổng công suất phát các Anten của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    SubBtsAntenHeights = ItemVm.SubBtsAntenHeights.Split(new char[] { ';' });
                    if (ItemVm.SubBtsQuantity != SubBtsAntenHeights.Count())
                    {
                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Lỗi nhập Danh sách Độ cao các Anten của các trạm BTS" }, JsonRequestBehavior.AllowGet);
                    }

                    if (act == CommonConstants.Action_Add)
                    {
                        NoRequiredBts newItem = new NoRequiredBts();

                        if (_noRequiredBtsService.getByID(ItemVm.Id) != null)
                        {
                            return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Giấy Chứng nhận kiểm định số " + ItemVm.Id + " đã tôn tại rồi" }, JsonRequestBehavior.AllowGet);
                        }

                        if (_noRequiredBtsService.findNoRequiredBts(ItemVm.BtsCode, ItemVm.AnnouncedDoc) != null)
                        {
                            return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Trạm gốc " + ItemVm.BtsCode + " trong hồ sơ đã được cấp Giấy CNKĐ rồi (số: " + ItemVm.Id + ")" }, JsonRequestBehavior.AllowGet);
                        }

                        newItem.UpdateNoRequiredBts(ItemVm);

                        newItem.Id = ItemVm.Id;
                        newItem.CreatedBy = User.Identity.Name;
                        newItem.CreatedDate = DateTime.Now;

                        _noRequiredBtsService.Add(newItem);
                        _noRequiredBtsService.SaveChanges();

                        for (int j = 0; j < ItemVm.SubBtsQuantity; j++)
                        {
                            SubBtsInNoRequiredBts subBtsItem = new SubBtsInNoRequiredBts
                            {
                                Id = ItemVm.Id,
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


                            _noRequiredBtsService.Add(subBtsItem);
                            _noRequiredBtsService.SaveChanges();
                        }

                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Success, message = "Thêm dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                        // html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<BtsViewModel>>(GetAll()))
                    }
                    else
                    {
                        NoRequiredBts editItem = _noRequiredBtsService.getByID(ItemVm.Id);
                        editItem.UpdateNoRequiredBts(ItemVm);
                        editItem.UpdatedBy = User.Identity.Name;
                        editItem.UpdatedDate = DateTime.Now;

                        _noRequiredBtsService.Update(editItem);
                        _noRequiredBtsService.SaveChanges();

                        _noRequiredBtsService.DeleteSubBTSinCert(ItemVm.Id);
                        _noRequiredBtsService.SaveChanges();

                        for (int j = 0; j < ItemVm.SubBtsQuantity; j++)
                        {
                            SubBtsInNoRequiredBts subBtsItem = new SubBtsInNoRequiredBts
                            {
                                Id = ItemVm.Id,
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


                            _noRequiredBtsService.Add(subBtsItem);
                            _noRequiredBtsService.SaveChanges();
                        }

                        return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Success, message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                        // html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<BtsViewModel>>(GetAll()))
                    }
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(CommonConstants.Data_CanSign_Role)]
        public ActionResult Sign()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CertificateNum = Request.Form.GetValues("CertificateNum")?.FirstOrDefault();                    

                    NoRequiredBts editItem = _noRequiredBtsService.getByID(CertificateNum);

                    editItem.UpdatedBy = User.Identity.Name;
                    editItem.UpdatedDate = DateTime.Now;

                    _noRequiredBtsService.Update(editItem);
                    _noRequiredBtsService.SaveChanges();

                    return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Success, message = "Ký phê duyệt ban hành thành công" }, JsonRequestBehavior.AllowGet);
                    // html = GlobalClass.RenderRazorViewToString(this, "ViewAll", Mapper.Map<IEnumerable<BtsViewModel>>(GetAll()))
                }
                else
                {
                    return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = ModelState.Values.SelectMany(v => v.Errors).Take(1).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRoles(CommonConstants.Data_CanDelete_Role)]
        public ActionResult Delete(string id = "0")
        {
            try
            {
                NoRequiredBts dbItem = _noRequiredBtsService.getByID(id);
                if (dbItem == null)
                {
                    return HttpNotFound();
                }

                //if (_btsService.IsUsed(id))
                //{
                //    return Json(new {resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = "Không thể xóa Trường hợp kiểm định này do đã được sử dụnd" }, JsonRequestBehavior.AllowGet);
                //}

                _noRequiredBtsService.Delete(id);
                _noRequiredBtsService.SaveChanges();

                return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Success, message = "Xóa dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                // html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAll()),
            }
            catch (Exception ex)
            {
                return Json(new { resetUrl = Url.Action("Add", "NoRequiredBts"), status = CommonConstants.Status_Error, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}