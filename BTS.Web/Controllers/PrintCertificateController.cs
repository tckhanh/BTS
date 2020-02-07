using AutoMapper;
using BTS.Common;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Infrastructure.Extensions;
using BTS.Web.Models;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Controllers
{
    [AuthorizeRoles(CommonConstants.Info_CanPrintCertificate_Role)]
    public class PrintCertificateController : BaseController
    {
        private ICertificateService _certificateService;
        private IOperatorService _operatorService;
        private IProfileService _profileService;

        public PrintCertificateController(ICertificateService certificateService, IOperatorService operatorService, IProfileService profileService, IErrorService errorService) : base(errorService)
        {
            _certificateService = certificateService;
            _operatorService = operatorService;
            _profileService = profileService;
        }

        // GET: PrintCertificate        

        public ActionResult Index(string id = "", string Template = "GCNKD", string BtsNum = "")
        {
            PrintCertificateViewModel ItemVm = new PrintCertificateViewModel();
            string[] SubBtsAntenHeights, SubBtsAntenNums, SubBtsBands, SubBtsCodes, SubBtsConfigurations, SubBtsEquipments, SubBtsOperatorIDs, SubBtsPowerSums;
            int SubBtsQuantityIndex;
            IEnumerable<PrintCertificateViewModel> printCertificates = new List<PrintCertificateViewModel>();

            if (!string.IsNullOrEmpty(id) && id !="undefined")
            {
                Certificate DbItem = _certificateService.getByID(id, new string[] { "Operator" });

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<PrintCertificateViewModel>(DbItem);

                    ItemVm.OperatorName = DbItem.Operator.Name?.ToUpper();
                    ItemVm.ApplicantName = _profileService.getByID(DbItem.ProfileID, new string[] { "Applicant" }).Applicant.Name?.ToUpper();


                    SubBtsAntenHeights = ItemVm.SubBtsAntenHeights.Split(new char[] { ';' });
                    SubBtsAntenNums = ItemVm.SubBtsAntenNums.Split(new char[] { ';' });
                    SubBtsBands = ItemVm.SubBtsBands.Split(new char[] { ';' });
                    SubBtsCodes = ItemVm.SubBtsCodes.Split(new char[] { ';' });
                    SubBtsConfigurations = ItemVm.SubBtsConfigurations.Split(new char[] { ';' });
                    SubBtsEquipments = ItemVm.SubBtsEquipments.Split(new char[] { ';' });
                    SubBtsOperatorIDs = ItemVm.SubBtsOperatorIDs.Split(new char[] { ';' });
                    SubBtsPowerSums = ItemVm.SubBtsPowerSums.Split(new char[] { ';' });

                    SubBtsQuantityIndex = 0;
                    if (ItemVm.SubBtsQuantity > SubBtsQuantityIndex)
                    {
                        ItemVm.SubBtsAntenHeight1 = SubBtsAntenHeights[SubBtsQuantityIndex];
                        ItemVm.SubBtsAntenNum1 = SubBtsAntenNums[SubBtsQuantityIndex];
                        ItemVm.SubBtsBand1 = SubBtsBands[SubBtsQuantityIndex];
                        ItemVm.SubBtsCode1 = SubBtsCodes[SubBtsQuantityIndex];
                        ItemVm.SubBtsConfiguration1 = SubBtsConfigurations[SubBtsQuantityIndex];
                        ItemVm.SubBtsEquipment1 = SubBtsEquipments[SubBtsQuantityIndex];
                        ItemVm.SubBtsOperatorID1 = SubBtsOperatorIDs[SubBtsQuantityIndex];
                        ItemVm.SubBtsPowerSum1 = SubBtsPowerSums[SubBtsQuantityIndex];

                        SubBtsQuantityIndex++;
                        if (ItemVm.SubBtsQuantity > SubBtsQuantityIndex)
                        {
                            ItemVm.SubBtsAntenHeight2 = SubBtsAntenHeights[SubBtsQuantityIndex];
                            ItemVm.SubBtsAntenNum2 = SubBtsAntenNums[SubBtsQuantityIndex];
                            ItemVm.SubBtsBand2 = SubBtsBands[SubBtsQuantityIndex];
                            ItemVm.SubBtsCode2 = SubBtsCodes[SubBtsQuantityIndex];
                            ItemVm.SubBtsConfiguration2 = SubBtsConfigurations[SubBtsQuantityIndex];
                            ItemVm.SubBtsEquipment2 = SubBtsEquipments[SubBtsQuantityIndex];
                            ItemVm.SubBtsOperatorID2 = SubBtsOperatorIDs[SubBtsQuantityIndex];
                            ItemVm.SubBtsPowerSum2 = SubBtsPowerSums[SubBtsQuantityIndex];

                            SubBtsQuantityIndex++;
                            if (ItemVm.SubBtsQuantity > SubBtsQuantityIndex)
                            {
                                ItemVm.SubBtsAntenHeight3 = SubBtsAntenHeights[SubBtsQuantityIndex];
                                ItemVm.SubBtsAntenNum3 = SubBtsAntenNums[SubBtsQuantityIndex];
                                ItemVm.SubBtsBand3 = SubBtsBands[SubBtsQuantityIndex];
                                ItemVm.SubBtsCode3 = SubBtsCodes[SubBtsQuantityIndex];
                                ItemVm.SubBtsConfiguration3 = SubBtsConfigurations[SubBtsQuantityIndex];
                                ItemVm.SubBtsEquipment3 = SubBtsEquipments[SubBtsQuantityIndex];
                                ItemVm.SubBtsOperatorID3 = SubBtsOperatorIDs[SubBtsQuantityIndex];
                                ItemVm.SubBtsPowerSum3 = SubBtsPowerSums[SubBtsQuantityIndex];

                                SubBtsQuantityIndex++;
                                if (ItemVm.SubBtsQuantity > SubBtsQuantityIndex)
                                {
                                    ItemVm.SubBtsAntenHeight4 = SubBtsAntenHeights[SubBtsQuantityIndex];
                                    ItemVm.SubBtsAntenNum4 = SubBtsAntenNums[SubBtsQuantityIndex];
                                    ItemVm.SubBtsBand4 = SubBtsBands[SubBtsQuantityIndex];
                                    ItemVm.SubBtsCode4 = SubBtsCodes[SubBtsQuantityIndex];
                                    ItemVm.SubBtsConfiguration4 = SubBtsConfigurations[SubBtsQuantityIndex];
                                    ItemVm.SubBtsEquipment4 = SubBtsEquipments[SubBtsQuantityIndex];
                                    ItemVm.SubBtsOperatorID4 = SubBtsOperatorIDs[SubBtsQuantityIndex];
                                    ItemVm.SubBtsPowerSum4 = SubBtsPowerSums[SubBtsQuantityIndex];

                                    SubBtsQuantityIndex++;
                                    if (ItemVm.SubBtsQuantity > SubBtsQuantityIndex)
                                    {
                                        ItemVm.SubBtsAntenHeight5 = SubBtsAntenHeights[SubBtsQuantityIndex];
                                        ItemVm.SubBtsAntenNum5 = SubBtsAntenNums[SubBtsQuantityIndex];
                                        ItemVm.SubBtsBand5 = SubBtsBands[SubBtsQuantityIndex];
                                        ItemVm.SubBtsCode5 = SubBtsCodes[SubBtsQuantityIndex];
                                        ItemVm.SubBtsConfiguration5 = SubBtsConfigurations[SubBtsQuantityIndex];
                                        ItemVm.SubBtsEquipment5 = SubBtsEquipments[SubBtsQuantityIndex];
                                        ItemVm.SubBtsOperatorID5 = SubBtsOperatorIDs[SubBtsQuantityIndex];
                                        ItemVm.SubBtsPowerSum5 = SubBtsPowerSums[SubBtsQuantityIndex];
                                    }
                                }
                            }
                        }
                    }
                    printCertificates = printCertificates.Add(ItemVm);
                }
                Session["printCertificates"] = printCertificates;
            }            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index()
        {
            int countItem;

            string CityID = Request.Form.GetValues("CityID")?.FirstOrDefault();
            string OperatorID = Request.Form.GetValues("OperatorID")?.FirstOrDefault();
            string ProfileID = Request.Form.GetValues("ProfileID")?.FirstOrDefault();
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
            IEnumerable<Certificate> DbItems;

            if (StartDate != null && EndDate != null)
            {
                DbItems = _certificateService.getAll(out countItem, false, StartDate, EndDate, new string[] { "Operator" }).ToList();
            }
            else
            {
                DbItems = _certificateService.getAll(out countItem, false, new string[] { "Operator" }).ToList();
            }

            if (IsExpired == "yes")
            {
                DbItems = DbItems.Where(x => x.ExpiredDate < DateTime.Today).ToList();
            }
            else
            {
                DbItems = DbItems.Where(x => x.ExpiredDate >= DateTime.Today).ToList();
            }

            if (!(string.IsNullOrEmpty(CityID)))
            {
                DbItems = DbItems.Where(x => x.CityID == CityID).ToList();
            }

            DbItems = DbItems.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID)).ToList();


            if (!(string.IsNullOrEmpty(OperatorID)))
            {
                DbItems = DbItems.Where(x => x.OperatorID.Contains(OperatorID)).ToList();
            }

            if (!(string.IsNullOrEmpty(ProfileID)))
            {
                DbItems = DbItems.Where(x => x.ProfileID?.ToString() == ProfileID).ToList();
            }

            if (!(string.IsNullOrEmpty(BtsCodeOrAddress)))
            {
                DbItems = DbItems.Where(x => x.BtsCode.ToLower().Contains(BtsCodeOrAddress) || x.Address.ToLower().Contains(BtsCodeOrAddress)).ToList();
            }

            //DbItems = DbItems.OrderByDescending(x => x.IssuedDate.Year.ToString() + x.Id);

            IEnumerable<PrintCertificateViewModel> printCertificates = Mapper.Map<List<PrintCertificateViewModel>>(DbItems);

            foreach (var ItemVm in printCertificates)
            {
                string[] SubBtsAntenHeights, SubBtsAntenNums, SubBtsBands, SubBtsCodes, SubBtsConfigurations, SubBtsEquipments, SubBtsOperatorIDs, SubBtsPowerSums;
                int SubBtsQuantityIndex;

                ItemVm.OperatorName = ItemVm.Operator.Name?.ToUpper();
                ItemVm.ApplicantName = _profileService.getByID(ItemVm.ProfileID, new string[] { "Applicant" }).Applicant.Name?.ToUpper();

                SubBtsAntenHeights = ItemVm.SubBtsAntenHeights.Split(new char[] { ';' });
                SubBtsAntenNums = ItemVm.SubBtsAntenNums.Split(new char[] { ';' });
                SubBtsBands = ItemVm.SubBtsBands.Split(new char[] { ';' });
                SubBtsCodes = ItemVm.SubBtsCodes.Split(new char[] { ';' });
                SubBtsConfigurations = ItemVm.SubBtsConfigurations.Split(new char[] { ';' });
                SubBtsEquipments = ItemVm.SubBtsEquipments.Split(new char[] { ';' });
                SubBtsOperatorIDs = ItemVm.SubBtsOperatorIDs.Split(new char[] { ';' });
                SubBtsPowerSums = ItemVm.SubBtsPowerSums.Split(new char[] { ';' });

                SubBtsQuantityIndex = 0;
                if (ItemVm.SubBtsQuantity > SubBtsQuantityIndex)
                {
                    ItemVm.SubBtsAntenHeight1 = SubBtsAntenHeights[SubBtsQuantityIndex];
                    ItemVm.SubBtsAntenNum1 = SubBtsAntenNums[SubBtsQuantityIndex];
                    ItemVm.SubBtsBand1 = SubBtsBands[SubBtsQuantityIndex];
                    ItemVm.SubBtsCode1 = SubBtsCodes[SubBtsQuantityIndex];
                    ItemVm.SubBtsConfiguration1 = SubBtsConfigurations[SubBtsQuantityIndex];
                    ItemVm.SubBtsEquipment1 = SubBtsEquipments[SubBtsQuantityIndex];
                    ItemVm.SubBtsOperatorID1 = SubBtsOperatorIDs[SubBtsQuantityIndex];
                    ItemVm.SubBtsPowerSum1 = SubBtsPowerSums[SubBtsQuantityIndex];

                    SubBtsQuantityIndex++;
                    if (ItemVm.SubBtsQuantity > SubBtsQuantityIndex)
                    {
                        ItemVm.SubBtsAntenHeight2 = SubBtsAntenHeights[SubBtsQuantityIndex];
                        ItemVm.SubBtsAntenNum2 = SubBtsAntenNums[SubBtsQuantityIndex];
                        ItemVm.SubBtsBand2 = SubBtsBands[SubBtsQuantityIndex];
                        ItemVm.SubBtsCode2 = SubBtsCodes[SubBtsQuantityIndex];
                        ItemVm.SubBtsConfiguration2 = SubBtsConfigurations[SubBtsQuantityIndex];
                        ItemVm.SubBtsEquipment2 = SubBtsEquipments[SubBtsQuantityIndex];
                        ItemVm.SubBtsOperatorID2 = SubBtsOperatorIDs[SubBtsQuantityIndex];
                        ItemVm.SubBtsPowerSum2 = SubBtsPowerSums[SubBtsQuantityIndex];

                        SubBtsQuantityIndex++;
                        if (ItemVm.SubBtsQuantity > SubBtsQuantityIndex)
                        {
                            ItemVm.SubBtsAntenHeight3 = SubBtsAntenHeights[SubBtsQuantityIndex];
                            ItemVm.SubBtsAntenNum3 = SubBtsAntenNums[SubBtsQuantityIndex];
                            ItemVm.SubBtsBand3 = SubBtsBands[SubBtsQuantityIndex];
                            ItemVm.SubBtsCode3 = SubBtsCodes[SubBtsQuantityIndex];
                            ItemVm.SubBtsConfiguration3 = SubBtsConfigurations[SubBtsQuantityIndex];
                            ItemVm.SubBtsEquipment3 = SubBtsEquipments[SubBtsQuantityIndex];
                            ItemVm.SubBtsOperatorID3 = SubBtsOperatorIDs[SubBtsQuantityIndex];
                            ItemVm.SubBtsPowerSum3 = SubBtsPowerSums[SubBtsQuantityIndex];

                            SubBtsQuantityIndex++;
                            if (ItemVm.SubBtsQuantity > SubBtsQuantityIndex)
                            {
                                ItemVm.SubBtsAntenHeight4 = SubBtsAntenHeights[SubBtsQuantityIndex];
                                ItemVm.SubBtsAntenNum4 = SubBtsAntenNums[SubBtsQuantityIndex];
                                ItemVm.SubBtsBand4 = SubBtsBands[SubBtsQuantityIndex];
                                ItemVm.SubBtsCode4 = SubBtsCodes[SubBtsQuantityIndex];
                                ItemVm.SubBtsConfiguration4 = SubBtsConfigurations[SubBtsQuantityIndex];
                                ItemVm.SubBtsEquipment4 = SubBtsEquipments[SubBtsQuantityIndex];
                                ItemVm.SubBtsOperatorID4 = SubBtsOperatorIDs[SubBtsQuantityIndex];
                                ItemVm.SubBtsPowerSum4 = SubBtsPowerSums[SubBtsQuantityIndex];

                                SubBtsQuantityIndex++;
                                if (ItemVm.SubBtsQuantity > SubBtsQuantityIndex)
                                {
                                    ItemVm.SubBtsAntenHeight5 = SubBtsAntenHeights[SubBtsQuantityIndex];
                                    ItemVm.SubBtsAntenNum5 = SubBtsAntenNums[SubBtsQuantityIndex];
                                    ItemVm.SubBtsBand5 = SubBtsBands[SubBtsQuantityIndex];
                                    ItemVm.SubBtsCode5 = SubBtsCodes[SubBtsQuantityIndex];
                                    ItemVm.SubBtsConfiguration5 = SubBtsConfigurations[SubBtsQuantityIndex];
                                    ItemVm.SubBtsEquipment5 = SubBtsEquipments[SubBtsQuantityIndex];
                                    ItemVm.SubBtsOperatorID5 = SubBtsOperatorIDs[SubBtsQuantityIndex];
                                    ItemVm.SubBtsPowerSum5 = SubBtsPowerSums[SubBtsQuantityIndex];
                                }
                            }
                        }
                    }
                }
            }
            Session["printCertificates"] = printCertificates;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrintFromInspect()
        {
            int countItem;
            string StrCertNunStart = Request.Form.GetValues("CertNunStart")?.FirstOrDefault();
            string StrCertNunEnd = Request.Form.GetValues("CertNunEnd")?.FirstOrDefault();
            int CertNumStart, CertNumEnd;
            Int32.TryParse(StrCertNunStart, out CertNumStart);
            Int32.TryParse(StrCertNunEnd, out CertNumEnd);
            DateTime StartDate, EndDate;

            if (!DateTime.TryParse(Request.Form.GetValues("StartDate").FirstOrDefault(), out StartDate))
            {
                Console.Write("Loi chuyen doi kieu");
            }

            if (!DateTime.TryParse(Request.Form.GetValues("EndDate").FirstOrDefault(), out EndDate))
            {
                Console.Write("Loi chuyen doi kieu");
            }

            // searching ...
            IEnumerable<Certificate> DbItems;

            if (StartDate != null && EndDate != null)
            {
                DbItems = _certificateService.getAll(out countItem, false, StartDate, EndDate, new string[] { "Operator" }).ToList();
            }
            else
            {
                DbItems = _certificateService.getAll(out countItem, false, new string[] { "Operator" }).ToList();
            }

            if (CertNumStart > 0)
            {
                DbItems = DbItems.Where(x => Int32.Parse(x.Id.Substring(1, 5)) >= CertNumStart);
            }

            if (CertNumEnd > 0)
            {
                DbItems = DbItems.Where(x => Int32.Parse(x.Id.Substring(1, 5)) <= CertNumEnd);
            }

            DbItems = DbItems.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID)).ToList();


            //DbItems = DbItems.OrderByDescending(x => x.IssuedDate.Year.ToString() + x.Id);

            IEnumerable<PrintCertificateViewModel> printCertificates = Mapper.Map<List<PrintCertificateViewModel>>(DbItems);

            foreach (var ItemVm in printCertificates)
            {
                string[] SubBtsAntenHeights, SubBtsAntenNums, SubBtsBands, SubBtsCodes, SubBtsConfigurations, SubBtsEquipments, SubBtsOperatorIDs, SubBtsPowerSums;
                int SubBtsQuantityIndex;

                ItemVm.OperatorName = ItemVm.Operator.Name?.ToUpper();
                ItemVm.ApplicantName = _profileService.getByID(ItemVm.ProfileID, new string[] { "Applicant" }).Applicant.Name?.ToUpper();

                SubBtsAntenHeights = ItemVm.SubBtsAntenHeights.Split(new char[] { ';' });
                SubBtsAntenNums = ItemVm.SubBtsAntenNums.Split(new char[] { ';' });
                SubBtsBands = ItemVm.SubBtsBands.Split(new char[] { ';' });
                SubBtsCodes = ItemVm.SubBtsCodes.Split(new char[] { ';' });
                SubBtsConfigurations = ItemVm.SubBtsConfigurations.Split(new char[] { ';' });
                SubBtsEquipments = ItemVm.SubBtsEquipments.Split(new char[] { ';' });
                SubBtsOperatorIDs = ItemVm.SubBtsOperatorIDs.Split(new char[] { ';' });
                SubBtsPowerSums = ItemVm.SubBtsPowerSums.Split(new char[] { ';' });

                SubBtsQuantityIndex = 0;
                if (ItemVm.SubBtsQuantity > SubBtsQuantityIndex)
                {
                    ItemVm.SubBtsAntenHeight1 = SubBtsAntenHeights[SubBtsQuantityIndex];
                    ItemVm.SubBtsAntenNum1 = SubBtsAntenNums[SubBtsQuantityIndex];
                    ItemVm.SubBtsBand1 = SubBtsBands[SubBtsQuantityIndex];
                    ItemVm.SubBtsCode1 = SubBtsCodes[SubBtsQuantityIndex];
                    ItemVm.SubBtsConfiguration1 = SubBtsConfigurations[SubBtsQuantityIndex];
                    ItemVm.SubBtsEquipment1 = SubBtsEquipments[SubBtsQuantityIndex];
                    ItemVm.SubBtsOperatorID1 = SubBtsOperatorIDs[SubBtsQuantityIndex];
                    ItemVm.SubBtsPowerSum1 = SubBtsPowerSums[SubBtsQuantityIndex];

                    SubBtsQuantityIndex++;
                    if (ItemVm.SubBtsQuantity > SubBtsQuantityIndex)
                    {
                        ItemVm.SubBtsAntenHeight2 = SubBtsAntenHeights[SubBtsQuantityIndex];
                        ItemVm.SubBtsAntenNum2 = SubBtsAntenNums[SubBtsQuantityIndex];
                        ItemVm.SubBtsBand2 = SubBtsBands[SubBtsQuantityIndex];
                        ItemVm.SubBtsCode2 = SubBtsCodes[SubBtsQuantityIndex];
                        ItemVm.SubBtsConfiguration2 = SubBtsConfigurations[SubBtsQuantityIndex];
                        ItemVm.SubBtsEquipment2 = SubBtsEquipments[SubBtsQuantityIndex];
                        ItemVm.SubBtsOperatorID2 = SubBtsOperatorIDs[SubBtsQuantityIndex];
                        ItemVm.SubBtsPowerSum2 = SubBtsPowerSums[SubBtsQuantityIndex];

                        SubBtsQuantityIndex++;
                        if (ItemVm.SubBtsQuantity > SubBtsQuantityIndex)
                        {
                            ItemVm.SubBtsAntenHeight3 = SubBtsAntenHeights[SubBtsQuantityIndex];
                            ItemVm.SubBtsAntenNum3 = SubBtsAntenNums[SubBtsQuantityIndex];
                            ItemVm.SubBtsBand3 = SubBtsBands[SubBtsQuantityIndex];
                            ItemVm.SubBtsCode3 = SubBtsCodes[SubBtsQuantityIndex];
                            ItemVm.SubBtsConfiguration3 = SubBtsConfigurations[SubBtsQuantityIndex];
                            ItemVm.SubBtsEquipment3 = SubBtsEquipments[SubBtsQuantityIndex];
                            ItemVm.SubBtsOperatorID3 = SubBtsOperatorIDs[SubBtsQuantityIndex];
                            ItemVm.SubBtsPowerSum3 = SubBtsPowerSums[SubBtsQuantityIndex];

                            SubBtsQuantityIndex++;
                            if (ItemVm.SubBtsQuantity > SubBtsQuantityIndex)
                            {
                                ItemVm.SubBtsAntenHeight4 = SubBtsAntenHeights[SubBtsQuantityIndex];
                                ItemVm.SubBtsAntenNum4 = SubBtsAntenNums[SubBtsQuantityIndex];
                                ItemVm.SubBtsBand4 = SubBtsBands[SubBtsQuantityIndex];
                                ItemVm.SubBtsCode4 = SubBtsCodes[SubBtsQuantityIndex];
                                ItemVm.SubBtsConfiguration4 = SubBtsConfigurations[SubBtsQuantityIndex];
                                ItemVm.SubBtsEquipment4 = SubBtsEquipments[SubBtsQuantityIndex];
                                ItemVm.SubBtsOperatorID4 = SubBtsOperatorIDs[SubBtsQuantityIndex];
                                ItemVm.SubBtsPowerSum4 = SubBtsPowerSums[SubBtsQuantityIndex];

                                SubBtsQuantityIndex++;
                                if (ItemVm.SubBtsQuantity > SubBtsQuantityIndex)
                                {
                                    ItemVm.SubBtsAntenHeight5 = SubBtsAntenHeights[SubBtsQuantityIndex];
                                    ItemVm.SubBtsAntenNum5 = SubBtsAntenNums[SubBtsQuantityIndex];
                                    ItemVm.SubBtsBand5 = SubBtsBands[SubBtsQuantityIndex];
                                    ItemVm.SubBtsCode5 = SubBtsCodes[SubBtsQuantityIndex];
                                    ItemVm.SubBtsConfiguration5 = SubBtsConfigurations[SubBtsQuantityIndex];
                                    ItemVm.SubBtsEquipment5 = SubBtsEquipments[SubBtsQuantityIndex];
                                    ItemVm.SubBtsOperatorID5 = SubBtsOperatorIDs[SubBtsQuantityIndex];
                                    ItemVm.SubBtsPowerSum5 = SubBtsPowerSums[SubBtsQuantityIndex];
                                }
                            }
                        }
                    }
                }
            }
            Session["printCertificates"] = printCertificates;
            return View();
        }

        public ActionResult GetReport(string id = "", string Template = "GCNKD", string BtsNum = "")
        {
            IEnumerable<PrintCertificateViewModel> printCertificates = (IEnumerable<PrintCertificateViewModel>)Session["printCertificates"];
            if (string.IsNullOrEmpty(BtsNum) && printCertificates != null)
            {
                int[] BtsNums = new int[5];
                foreach (var item in printCertificates)
                {
                    BtsNums[item.SubBtsQuantity - 1]++;
                }

                for (int i = 0; i < BtsNums.Length; i++)
                {
                    if (BtsNums[i] > 0)
                    {
                        BtsNum = (i + 1).ToString();
                        break;
                    }
                }
            }
            // Create the report object and load data from xml file
            var report = new StiReport();
            report.Load(Server.MapPath("~/Content/ReportTemplates/" + Template + "_" + BtsNum + "BTS.mrt"));
            report.RegData(printCertificates?.ToDataSet("DataSet", "Certificates"));

            return StiMvcViewer.GetReportResult(report);
        }

        public ActionResult ViewerEvent()
        {
            //var TempData1 = StiMvcViewer.GetRequestParams();
            //var TempData2 = StiMvcViewer.GetFormValues();
            //var TempData3 = StiMvcViewer.GetRouteValues();
            //var TempData4 = StiMvcViewer.ViewerEventResult();
            return StiMvcViewer.ViewerEventResult();
        }
    }
}