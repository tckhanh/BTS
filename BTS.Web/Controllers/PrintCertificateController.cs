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

namespace BTS.Web.Areas.Controllers
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
        public ActionResult Index(string id = "", bool Template = false, string BtsNum = "")
        {
            PrintCertificateViewModel ItemVm = new PrintCertificateViewModel();
            string[] SubBtsAntenHeights, SubBtsAntenNums, SubBtsBands, SubBtsCodes, SubBtsConfigurations, SubBtsEquipments, SubBtsOperatorIDs, SubBtsPowerSums;
            int FrontSubBtsQuantity, BackSubBtsQuantity;
            IEnumerable<PrintCertificateViewModel> printCertificates = new List<PrintCertificateViewModel>();

            if (!string.IsNullOrEmpty(id) && id != "undefined")
            {
                Certificate DbItem = _certificateService.getByID(id, new string[] { "Operator" });

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<PrintCertificateViewModel>(DbItem);

                    ItemVm.OperatorName = DbItem.Operator.Name?.ToUpper();
                    ItemVm.ApplicantName = _profileService.getByID(DbItem.ProfileID, new string[] { "Applicant" }).Applicant.Name?.ToUpper();
                    ItemVm.SignatureFileName = Server.MapPath("~/Content/ReportTemplates/" + ItemVm.Signer.ToUnsignString() + ".jpg");
                    ItemVm.SubSignFileName = Server.MapPath("~/Content/ReportTemplates/" + ItemVm.Verifier1.ToUnsignString() + ".jpg");
                    ItemVm.StrIssuedDate = "Ngày " + ItemVm.IssuedDate.Day + " tháng " + ItemVm.IssuedDate.Month + " năm " + ItemVm.IssuedDate.Year;

                    SubBtsAntenHeights = ItemVm.SubBtsAntenHeights.Split(new char[] { ';' });
                    SubBtsAntenNums = ItemVm.SubBtsAntenNums.Split(new char[] { ';' });
                    SubBtsBands = ItemVm.SubBtsBands.Split(new char[] { ';' });
                    SubBtsCodes = ItemVm.SubBtsCodes.Split(new char[] { ';' });
                    SubBtsConfigurations = ItemVm.SubBtsConfigurations.Split(new char[] { ';' });
                    SubBtsEquipments = ItemVm.SubBtsEquipments.Split(new char[] { ';' });
                    SubBtsOperatorIDs = ItemVm.SubBtsOperatorIDs.Split(new char[] { ';' });
                    SubBtsPowerSums = ItemVm.SubBtsPowerSums.Split(new char[] { ';' });

                    FrontSubBtsQuantity = 0;
                    BackSubBtsQuantity = 0;
                    for (int i = 0; i < ItemVm.SubBtsQuantity; i++)
                    {
                        if (ItemVm.OperatorID == SubBtsOperatorIDs[i])
                        {
                            FrontSubBtsQuantity++;
                            switch (FrontSubBtsQuantity)
                            {
                                case 1:
                                    ItemVm.SubBtsAntenHeight1 = SubBtsAntenHeights[i];
                                    ItemVm.SubBtsAntenNum1 = SubBtsAntenNums[i];
                                    ItemVm.SubBtsBand1 = SubBtsBands[i];
                                    ItemVm.SubBtsCode1 = SubBtsCodes[i];
                                    ItemVm.SubBtsConfiguration1 = SubBtsConfigurations[i];
                                    ItemVm.SubBtsEquipment1 = SubBtsEquipments[i];
                                    ItemVm.SubBtsOperatorID1 = SubBtsOperatorIDs[i];
                                    ItemVm.SubBtsPowerSum1 = SubBtsPowerSums[i];
                                    break;
                                case 2:
                                    ItemVm.SubBtsAntenHeight2 = SubBtsAntenHeights[i];
                                    ItemVm.SubBtsAntenNum2 = SubBtsAntenNums[i];
                                    ItemVm.SubBtsBand2 = SubBtsBands[i];
                                    ItemVm.SubBtsCode2 = SubBtsCodes[i];
                                    ItemVm.SubBtsConfiguration2 = SubBtsConfigurations[i];
                                    ItemVm.SubBtsEquipment2 = SubBtsEquipments[i];
                                    ItemVm.SubBtsOperatorID2 = SubBtsOperatorIDs[i];
                                    ItemVm.SubBtsPowerSum2 = SubBtsPowerSums[i];
                                    break;
                                case 3:
                                    ItemVm.SubBtsAntenHeight3 = SubBtsAntenHeights[i];
                                    ItemVm.SubBtsAntenNum3 = SubBtsAntenNums[i];
                                    ItemVm.SubBtsBand3 = SubBtsBands[i];
                                    ItemVm.SubBtsCode3 = SubBtsCodes[i];
                                    ItemVm.SubBtsConfiguration3 = SubBtsConfigurations[i];
                                    ItemVm.SubBtsEquipment3 = SubBtsEquipments[i];
                                    ItemVm.SubBtsOperatorID3 = SubBtsOperatorIDs[i];
                                    ItemVm.SubBtsPowerSum3 = SubBtsPowerSums[i];
                                    break;
                                case 4:
                                    ItemVm.SubBtsAntenHeight4 = SubBtsAntenHeights[i];
                                    ItemVm.SubBtsAntenNum4 = SubBtsAntenNums[i];
                                    ItemVm.SubBtsBand4 = SubBtsBands[i];
                                    ItemVm.SubBtsCode4 = SubBtsCodes[i];
                                    ItemVm.SubBtsConfiguration4 = SubBtsConfigurations[i];
                                    ItemVm.SubBtsEquipment4 = SubBtsEquipments[i];
                                    ItemVm.SubBtsOperatorID4 = SubBtsOperatorIDs[i];
                                    ItemVm.SubBtsPowerSum4 = SubBtsPowerSums[i];
                                    break;
                                case 5:
                                    ItemVm.SubBtsAntenHeight5 = SubBtsAntenHeights[i];
                                    ItemVm.SubBtsAntenNum5 = SubBtsAntenNums[i];
                                    ItemVm.SubBtsBand5 = SubBtsBands[i];
                                    ItemVm.SubBtsCode5 = SubBtsCodes[i];
                                    ItemVm.SubBtsConfiguration5 = SubBtsConfigurations[i];
                                    ItemVm.SubBtsEquipment5 = SubBtsEquipments[i];
                                    ItemVm.SubBtsOperatorID5 = SubBtsOperatorIDs[i];
                                    ItemVm.SubBtsPowerSum5 = SubBtsPowerSums[i];
                                    break;
                                case 6:
                                    ItemVm.SubBtsAntenHeight6 = SubBtsAntenHeights[i];
                                    ItemVm.SubBtsAntenNum6 = SubBtsAntenNums[i];
                                    ItemVm.SubBtsBand6 = SubBtsBands[i];
                                    ItemVm.SubBtsCode6 = SubBtsCodes[i];
                                    ItemVm.SubBtsConfiguration6 = SubBtsConfigurations[i];
                                    ItemVm.SubBtsEquipment6 = SubBtsEquipments[i];
                                    ItemVm.SubBtsOperatorID6 = SubBtsOperatorIDs[i];
                                    ItemVm.SubBtsPowerSum6 = SubBtsPowerSums[i];
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            BackSubBtsQuantity++;
                            string SubBtsOperatorName = ((Operator)_operatorService.getByID(SubBtsOperatorIDs[i])).Name;
                            switch (BackSubBtsQuantity)
                            {
                                case 1:
                                    ItemVm.BackSubBtsAntenHeight1 = SubBtsAntenHeights[i];
                                    ItemVm.BackSubBtsAntenNum1 = SubBtsAntenNums[i];
                                    ItemVm.BackSubBtsBand1 = SubBtsBands[i];
                                    ItemVm.BackSubBtsCode1 = SubBtsCodes[i];
                                    ItemVm.BackSubBtsConfiguration1 = SubBtsConfigurations[i];
                                    ItemVm.BackSubBtsEquipment1 = SubBtsEquipments[i];
                                    ItemVm.BackSubBtsOperatorID1 = SubBtsOperatorIDs[i];
                                    ItemVm.BackSubBtsOperatorName1 = SubBtsOperatorName;
                                    ItemVm.BackSubBtsPowerSum1 = SubBtsPowerSums[i];
                                    break;
                                case 2:
                                    ItemVm.BackSubBtsAntenHeight2 = SubBtsAntenHeights[i];
                                    ItemVm.BackSubBtsAntenNum2 = SubBtsAntenNums[i];
                                    ItemVm.BackSubBtsBand2 = SubBtsBands[i];
                                    ItemVm.BackSubBtsCode2 = SubBtsCodes[i];
                                    ItemVm.BackSubBtsConfiguration2 = SubBtsConfigurations[i];
                                    ItemVm.BackSubBtsEquipment2 = SubBtsEquipments[i];
                                    ItemVm.BackSubBtsOperatorID2 = SubBtsOperatorIDs[i];
                                    ItemVm.BackSubBtsOperatorName2 = SubBtsOperatorName;
                                    ItemVm.BackSubBtsPowerSum2 = SubBtsPowerSums[i];
                                    break;
                                case 3:
                                    ItemVm.BackSubBtsAntenHeight3 = SubBtsAntenHeights[i];
                                    ItemVm.BackSubBtsAntenNum3 = SubBtsAntenNums[i];
                                    ItemVm.BackSubBtsBand3 = SubBtsBands[i];
                                    ItemVm.BackSubBtsCode3 = SubBtsCodes[i];
                                    ItemVm.BackSubBtsConfiguration3 = SubBtsConfigurations[i];
                                    ItemVm.BackSubBtsEquipment3 = SubBtsEquipments[i];
                                    ItemVm.BackSubBtsOperatorID3 = SubBtsOperatorIDs[i];
                                    ItemVm.BackSubBtsOperatorName3 = SubBtsOperatorName;
                                    ItemVm.BackSubBtsPowerSum3 = SubBtsPowerSums[i];
                                    break;
                                case 4:
                                    ItemVm.BackSubBtsAntenHeight4 = SubBtsAntenHeights[i];
                                    ItemVm.BackSubBtsAntenNum4 = SubBtsAntenNums[i];
                                    ItemVm.BackSubBtsBand4 = SubBtsBands[i];
                                    ItemVm.BackSubBtsCode4 = SubBtsCodes[i];
                                    ItemVm.BackSubBtsConfiguration4 = SubBtsConfigurations[i];
                                    ItemVm.BackSubBtsEquipment4 = SubBtsEquipments[i];
                                    ItemVm.BackSubBtsOperatorID4 = SubBtsOperatorIDs[i];
                                    ItemVm.BackSubBtsOperatorName4 = SubBtsOperatorName;
                                    ItemVm.BackSubBtsPowerSum4 = SubBtsPowerSums[i];
                                    break;
                                case 5:
                                    ItemVm.BackSubBtsAntenHeight5 = SubBtsAntenHeights[i];
                                    ItemVm.BackSubBtsAntenNum5 = SubBtsAntenNums[i];
                                    ItemVm.BackSubBtsBand5 = SubBtsBands[i];
                                    ItemVm.BackSubBtsCode5 = SubBtsCodes[i];
                                    ItemVm.BackSubBtsConfiguration5 = SubBtsConfigurations[i];
                                    ItemVm.BackSubBtsEquipment5 = SubBtsEquipments[i];
                                    ItemVm.BackSubBtsOperatorID5 = SubBtsOperatorIDs[i];
                                    ItemVm.BackSubBtsOperatorName5 = SubBtsOperatorName;
                                    ItemVm.BackSubBtsPowerSum5 = SubBtsPowerSums[i];
                                    break;
                                case 6:
                                    ItemVm.BackSubBtsAntenHeight6 = SubBtsAntenHeights[i];
                                    ItemVm.BackSubBtsAntenNum6 = SubBtsAntenNums[i];
                                    ItemVm.BackSubBtsBand6 = SubBtsBands[i];
                                    ItemVm.BackSubBtsCode6 = SubBtsCodes[i];
                                    ItemVm.BackSubBtsConfiguration6 = SubBtsConfigurations[i];
                                    ItemVm.BackSubBtsEquipment6 = SubBtsEquipments[i];
                                    ItemVm.BackSubBtsOperatorID6 = SubBtsOperatorIDs[i];
                                    ItemVm.BackSubBtsOperatorName6 = SubBtsOperatorName;
                                    ItemVm.BackSubBtsPowerSum6 = SubBtsPowerSums[i];
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    ItemVm.FrontSubBtsQuantity = FrontSubBtsQuantity;
                    ItemVm.BackSubBtsQuantity = BackSubBtsQuantity;

                    printCertificates = printCertificates.Add(ItemVm);
                }
                Session["printCertificates"] = printCertificates;
            }
            return View();
        }

        [Audit(AuditingLevel = 1)]
        public ActionResult Print(string id)
        {
            PrintCertificateViewModel ItemVm = new PrintCertificateViewModel();
            string[] SubBtsAntenHeights, SubBtsAntenNums, SubBtsBands, SubBtsCodes, SubBtsConfigurations, SubBtsEquipments, SubBtsOperatorIDs, SubBtsPowerSums;
            int FrontSubBtsQuantity, BackSubBtsQuantity;
            IEnumerable<PrintCertificateViewModel> printCertificates = new List<PrintCertificateViewModel>();

            if (!string.IsNullOrEmpty(id) && id != "undefined")
            {
                Certificate DbItem = _certificateService.getByID(id, new string[] { "Operator" });

                if (DbItem != null)
                {
                    ItemVm = Mapper.Map<PrintCertificateViewModel>(DbItem);

                    ItemVm.OperatorName = DbItem.Operator.Name?.ToUpper();
                    ItemVm.ApplicantName = _profileService.getByID(DbItem.ProfileID, new string[] { "Applicant" }).Applicant.Name?.ToUpper();
                    ItemVm.SignatureFileName = Server.MapPath("~/Content/ReportTemplates/" + ItemVm.Signer.ToUnsignString() + ".jpg");
                    ItemVm.SubSignFileName = Server.MapPath("~/Content/ReportTemplates/" + ItemVm.Verifier1.ToUnsignString() + ".jpg");
                    ItemVm.StrIssuedDate = "Ngày " + ItemVm.IssuedDate.Day + " tháng " + ItemVm.IssuedDate.Month + " năm " + ItemVm.IssuedDate.Year;

                    SubBtsAntenHeights = ItemVm.SubBtsAntenHeights.Split(new char[] { ';' });
                    SubBtsAntenNums = ItemVm.SubBtsAntenNums.Split(new char[] { ';' });
                    SubBtsBands = ItemVm.SubBtsBands.Split(new char[] { ';' });
                    SubBtsCodes = ItemVm.SubBtsCodes.Split(new char[] { ';' });
                    SubBtsConfigurations = ItemVm.SubBtsConfigurations.Split(new char[] { ';' });
                    SubBtsEquipments = ItemVm.SubBtsEquipments.Split(new char[] { ';' });
                    SubBtsOperatorIDs = ItemVm.SubBtsOperatorIDs.Split(new char[] { ';' });
                    SubBtsPowerSums = ItemVm.SubBtsPowerSums.Split(new char[] { ';' });

                    FrontSubBtsQuantity = 0;
                    BackSubBtsQuantity = 0;
                    for (int i = 0; i < ItemVm.SubBtsQuantity; i++)
                    {
                        if (ItemVm.OperatorID == SubBtsOperatorIDs[i])
                        {
                            FrontSubBtsQuantity++;
                            switch (FrontSubBtsQuantity)
                            {
                                case 1:
                                    ItemVm.SubBtsAntenHeight1 = SubBtsAntenHeights[i];
                                    ItemVm.SubBtsAntenNum1 = SubBtsAntenNums[i];
                                    ItemVm.SubBtsBand1 = SubBtsBands[i];
                                    ItemVm.SubBtsCode1 = SubBtsCodes[i];
                                    ItemVm.SubBtsConfiguration1 = SubBtsConfigurations[i];
                                    ItemVm.SubBtsEquipment1 = SubBtsEquipments[i];
                                    ItemVm.SubBtsOperatorID1 = SubBtsOperatorIDs[i];
                                    ItemVm.SubBtsPowerSum1 = SubBtsPowerSums[i];
                                    break;
                                case 2:
                                    ItemVm.SubBtsAntenHeight2 = SubBtsAntenHeights[i];
                                    ItemVm.SubBtsAntenNum2 = SubBtsAntenNums[i];
                                    ItemVm.SubBtsBand2 = SubBtsBands[i];
                                    ItemVm.SubBtsCode2 = SubBtsCodes[i];
                                    ItemVm.SubBtsConfiguration2 = SubBtsConfigurations[i];
                                    ItemVm.SubBtsEquipment2 = SubBtsEquipments[i];
                                    ItemVm.SubBtsOperatorID2 = SubBtsOperatorIDs[i];
                                    ItemVm.SubBtsPowerSum2 = SubBtsPowerSums[i];
                                    break;
                                case 3:
                                    ItemVm.SubBtsAntenHeight3 = SubBtsAntenHeights[i];
                                    ItemVm.SubBtsAntenNum3 = SubBtsAntenNums[i];
                                    ItemVm.SubBtsBand3 = SubBtsBands[i];
                                    ItemVm.SubBtsCode3 = SubBtsCodes[i];
                                    ItemVm.SubBtsConfiguration3 = SubBtsConfigurations[i];
                                    ItemVm.SubBtsEquipment3 = SubBtsEquipments[i];
                                    ItemVm.SubBtsOperatorID3 = SubBtsOperatorIDs[i];
                                    ItemVm.SubBtsPowerSum3 = SubBtsPowerSums[i];
                                    break;
                                case 4:
                                    ItemVm.SubBtsAntenHeight4 = SubBtsAntenHeights[i];
                                    ItemVm.SubBtsAntenNum4 = SubBtsAntenNums[i];
                                    ItemVm.SubBtsBand4 = SubBtsBands[i];
                                    ItemVm.SubBtsCode4 = SubBtsCodes[i];
                                    ItemVm.SubBtsConfiguration4 = SubBtsConfigurations[i];
                                    ItemVm.SubBtsEquipment4 = SubBtsEquipments[i];
                                    ItemVm.SubBtsOperatorID4 = SubBtsOperatorIDs[i];
                                    ItemVm.SubBtsPowerSum4 = SubBtsPowerSums[i];
                                    break;
                                case 5:
                                    ItemVm.SubBtsAntenHeight5 = SubBtsAntenHeights[i];
                                    ItemVm.SubBtsAntenNum5 = SubBtsAntenNums[i];
                                    ItemVm.SubBtsBand5 = SubBtsBands[i];
                                    ItemVm.SubBtsCode5 = SubBtsCodes[i];
                                    ItemVm.SubBtsConfiguration5 = SubBtsConfigurations[i];
                                    ItemVm.SubBtsEquipment5 = SubBtsEquipments[i];
                                    ItemVm.SubBtsOperatorID5 = SubBtsOperatorIDs[i];
                                    ItemVm.SubBtsPowerSum5 = SubBtsPowerSums[i];
                                    break;
                                case 6:
                                    ItemVm.SubBtsAntenHeight6 = SubBtsAntenHeights[i];
                                    ItemVm.SubBtsAntenNum6 = SubBtsAntenNums[i];
                                    ItemVm.SubBtsBand6 = SubBtsBands[i];
                                    ItemVm.SubBtsCode6 = SubBtsCodes[i];
                                    ItemVm.SubBtsConfiguration6 = SubBtsConfigurations[i];
                                    ItemVm.SubBtsEquipment6 = SubBtsEquipments[i];
                                    ItemVm.SubBtsOperatorID6 = SubBtsOperatorIDs[i];
                                    ItemVm.SubBtsPowerSum6 = SubBtsPowerSums[i];
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            BackSubBtsQuantity++;
                            string SubBtsOperatorName = ((Operator)_operatorService.getByID(SubBtsOperatorIDs[i])).Name;
                            switch (BackSubBtsQuantity)
                            {
                                case 1:
                                    ItemVm.BackSubBtsAntenHeight1 = SubBtsAntenHeights[i];
                                    ItemVm.BackSubBtsAntenNum1 = SubBtsAntenNums[i];
                                    ItemVm.BackSubBtsBand1 = SubBtsBands[i];
                                    ItemVm.BackSubBtsCode1 = SubBtsCodes[i];
                                    ItemVm.BackSubBtsConfiguration1 = SubBtsConfigurations[i];
                                    ItemVm.BackSubBtsEquipment1 = SubBtsEquipments[i];
                                    ItemVm.BackSubBtsOperatorID1 = SubBtsOperatorIDs[i];
                                    ItemVm.BackSubBtsOperatorName1 = SubBtsOperatorName;
                                    ItemVm.BackSubBtsPowerSum1 = SubBtsPowerSums[i];
                                    break;
                                case 2:
                                    ItemVm.BackSubBtsAntenHeight2 = SubBtsAntenHeights[i];
                                    ItemVm.BackSubBtsAntenNum2 = SubBtsAntenNums[i];
                                    ItemVm.BackSubBtsBand2 = SubBtsBands[i];
                                    ItemVm.BackSubBtsCode2 = SubBtsCodes[i];
                                    ItemVm.BackSubBtsConfiguration2 = SubBtsConfigurations[i];
                                    ItemVm.BackSubBtsEquipment2 = SubBtsEquipments[i];
                                    ItemVm.BackSubBtsOperatorID2 = SubBtsOperatorIDs[i];
                                    ItemVm.BackSubBtsOperatorName2 = SubBtsOperatorName;
                                    ItemVm.BackSubBtsPowerSum2 = SubBtsPowerSums[i];
                                    break;
                                case 3:
                                    ItemVm.BackSubBtsAntenHeight3 = SubBtsAntenHeights[i];
                                    ItemVm.BackSubBtsAntenNum3 = SubBtsAntenNums[i];
                                    ItemVm.BackSubBtsBand3 = SubBtsBands[i];
                                    ItemVm.BackSubBtsCode3 = SubBtsCodes[i];
                                    ItemVm.BackSubBtsConfiguration3 = SubBtsConfigurations[i];
                                    ItemVm.BackSubBtsEquipment3 = SubBtsEquipments[i];
                                    ItemVm.BackSubBtsOperatorID3 = SubBtsOperatorIDs[i];
                                    ItemVm.BackSubBtsOperatorName3 = SubBtsOperatorName;
                                    ItemVm.BackSubBtsPowerSum3 = SubBtsPowerSums[i];
                                    break;
                                case 4:
                                    ItemVm.BackSubBtsAntenHeight4 = SubBtsAntenHeights[i];
                                    ItemVm.BackSubBtsAntenNum4 = SubBtsAntenNums[i];
                                    ItemVm.BackSubBtsBand4 = SubBtsBands[i];
                                    ItemVm.BackSubBtsCode4 = SubBtsCodes[i];
                                    ItemVm.BackSubBtsConfiguration4 = SubBtsConfigurations[i];
                                    ItemVm.BackSubBtsEquipment4 = SubBtsEquipments[i];
                                    ItemVm.BackSubBtsOperatorID4 = SubBtsOperatorIDs[i];
                                    ItemVm.BackSubBtsOperatorName4 = SubBtsOperatorName;
                                    ItemVm.BackSubBtsPowerSum4 = SubBtsPowerSums[i];
                                    break;
                                case 5:
                                    ItemVm.BackSubBtsAntenHeight5 = SubBtsAntenHeights[i];
                                    ItemVm.BackSubBtsAntenNum5 = SubBtsAntenNums[i];
                                    ItemVm.BackSubBtsBand5 = SubBtsBands[i];
                                    ItemVm.BackSubBtsCode5 = SubBtsCodes[i];
                                    ItemVm.BackSubBtsConfiguration5 = SubBtsConfigurations[i];
                                    ItemVm.BackSubBtsEquipment5 = SubBtsEquipments[i];
                                    ItemVm.BackSubBtsOperatorID5 = SubBtsOperatorIDs[i];
                                    ItemVm.BackSubBtsOperatorName5 = SubBtsOperatorName;
                                    ItemVm.BackSubBtsPowerSum5 = SubBtsPowerSums[i];
                                    break;
                                case 6:
                                    ItemVm.BackSubBtsAntenHeight6 = SubBtsAntenHeights[i];
                                    ItemVm.BackSubBtsAntenNum6 = SubBtsAntenNums[i];
                                    ItemVm.BackSubBtsBand6 = SubBtsBands[i];
                                    ItemVm.BackSubBtsCode6 = SubBtsCodes[i];
                                    ItemVm.BackSubBtsConfiguration6 = SubBtsConfigurations[i];
                                    ItemVm.BackSubBtsEquipment6 = SubBtsEquipments[i];
                                    ItemVm.BackSubBtsOperatorID6 = SubBtsOperatorIDs[i];
                                    ItemVm.BackSubBtsOperatorName6 = SubBtsOperatorName;
                                    ItemVm.BackSubBtsPowerSum6 = SubBtsPowerSums[i];
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    ItemVm.FrontSubBtsQuantity = FrontSubBtsQuantity;
                    ItemVm.BackSubBtsQuantity = BackSubBtsQuantity;

                    printCertificates = printCertificates.Add(ItemVm);
                }
                Session["printCertificates"] = printCertificates;
            }
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Audit(AuditingLevel = 1)]
        public ActionResult Index()
        {
            int countItem;

            string CityID = Request.Form.GetValues("CityID")?.FirstOrDefault();
            string OperatorID = Request.Form.GetValues("OperatorID")?.FirstOrDefault();
            string ProfileID = Request.Form.GetValues("ProfileID")?.FirstOrDefault();
            string CertificateNum = Request.Form.GetValues("CertificateNum")?.FirstOrDefault().ToUpper();
            string BtsCodeOrAddress = Request.Form.GetValues("BtsCodeOrAddress")?.FirstOrDefault().ToLower();
            string IsExpired = Request.Form.GetValues("IsExpired")?.FirstOrDefault().ToLower();
            DateTime StartDate, EndDate;

            string[] SubBtsAntenHeights, SubBtsAntenNums, SubBtsBands, SubBtsCodes, SubBtsConfigurations, SubBtsEquipments, SubBtsOperatorIDs, SubBtsPowerSums;
            int FrontSubBtsQuantity, BackSubBtsQuantity;

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
                Items = _certificateService.getCertificateByCertificateNum(CertificateNum, new string[] { "Operator" }).ToList();
            }
            else if (!(string.IsNullOrEmpty(CityID)))
            {
                Items = _certificateService.getCertificateByCity(CityID, new string[] { "Operator" }).ToList();
            }
            else if (!(string.IsNullOrEmpty(OperatorID)))
            {
                Items = _certificateService.getCertificateByOperator(OperatorID, new string[] { "Operator" }).ToList();
            }
            else if (!(string.IsNullOrEmpty(ProfileID)))
            {
                Items = _certificateService.getCertificateByProfile(ProfileID, new string[] { "Operator" }).ToList();
            }
            else if (!(string.IsNullOrEmpty(BtsCodeOrAddress)))
            {
                Items = _certificateService.getCertificateByBtsCodeOrAddress(BtsCodeOrAddress, new string[] { "Operator" }).ToList();
            }
            else if (StartDate != null && EndDate != null)
            {
                Items = _certificateService.getAll(out countItem, false, StartDate, EndDate, new string[] { "Operator" }).ToList();
            }
            else
            {
                Items = _certificateService.getAll(out countItem, false, new string[] { "Operator" }).ToList();
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

            if (getEnableCityIDsScope() == "True")
            {
                Items = Items.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID)).ToList();
            }
            //DbItems = DbItems.OrderByDescending(x => x.IssuedDate.Year.ToString() + x.Id);

            IEnumerable<PrintCertificateViewModel> printCertificates = Mapper.Map<List<PrintCertificateViewModel>>(Items);

            foreach (var ItemVm in printCertificates)
            {
                ItemVm.OperatorName = ItemVm.Operator.Name?.ToUpper();
                ItemVm.ApplicantName = _profileService.getByID(ItemVm.ProfileID, new string[] { "Applicant" }).Applicant.Name?.ToUpper();
                ItemVm.SignatureFileName = Server.MapPath("~/Content/ReportTemplates/" + ItemVm.Signer.ToUnsignString() + ".jpg");
                ItemVm.SubSignFileName = Server.MapPath("~/Content/ReportTemplates/" + ItemVm.Verifier1.ToUnsignString() + ".jpg");
                ItemVm.StrIssuedDate = "Ngày " + ItemVm.IssuedDate.Day + " tháng " + ItemVm.IssuedDate.Month + " năm " + ItemVm.IssuedDate.Year;

                SubBtsAntenHeights = ItemVm.SubBtsAntenHeights.Split(new char[] { ';' });
                SubBtsAntenNums = ItemVm.SubBtsAntenNums.Split(new char[] { ';' });
                SubBtsBands = ItemVm.SubBtsBands.Split(new char[] { ';' });
                SubBtsCodes = ItemVm.SubBtsCodes.Split(new char[] { ';' });
                SubBtsConfigurations = ItemVm.SubBtsConfigurations.Split(new char[] { ';' });
                SubBtsEquipments = ItemVm.SubBtsEquipments.Split(new char[] { ';' });
                SubBtsOperatorIDs = ItemVm.SubBtsOperatorIDs.Split(new char[] { ';' });
                SubBtsPowerSums = ItemVm.SubBtsPowerSums.Split(new char[] { ';' });

                FrontSubBtsQuantity = 0;
                BackSubBtsQuantity = 0;
                for (int i = 0; i < ItemVm.SubBtsQuantity; i++)
                {
                    if (ItemVm.OperatorID == SubBtsOperatorIDs[i])
                    {
                        FrontSubBtsQuantity++;
                        switch (FrontSubBtsQuantity)
                        {
                            case 1:
                                ItemVm.SubBtsAntenHeight1 = SubBtsAntenHeights[i];
                                ItemVm.SubBtsAntenNum1 = SubBtsAntenNums[i];
                                ItemVm.SubBtsBand1 = SubBtsBands[i];
                                ItemVm.SubBtsCode1 = SubBtsCodes[i];
                                ItemVm.SubBtsConfiguration1 = SubBtsConfigurations[i];
                                ItemVm.SubBtsEquipment1 = SubBtsEquipments[i];
                                ItemVm.SubBtsOperatorID1 = SubBtsOperatorIDs[i];
                                ItemVm.SubBtsPowerSum1 = SubBtsPowerSums[i];
                                break;
                            case 2:
                                ItemVm.SubBtsAntenHeight2 = SubBtsAntenHeights[i];
                                ItemVm.SubBtsAntenNum2 = SubBtsAntenNums[i];
                                ItemVm.SubBtsBand2 = SubBtsBands[i];
                                ItemVm.SubBtsCode2 = SubBtsCodes[i];
                                ItemVm.SubBtsConfiguration2 = SubBtsConfigurations[i];
                                ItemVm.SubBtsEquipment2 = SubBtsEquipments[i];
                                ItemVm.SubBtsOperatorID2 = SubBtsOperatorIDs[i];
                                ItemVm.SubBtsPowerSum2 = SubBtsPowerSums[i];
                                break;
                            case 3:
                                ItemVm.SubBtsAntenHeight3 = SubBtsAntenHeights[i];
                                ItemVm.SubBtsAntenNum3 = SubBtsAntenNums[i];
                                ItemVm.SubBtsBand3 = SubBtsBands[i];
                                ItemVm.SubBtsCode3 = SubBtsCodes[i];
                                ItemVm.SubBtsConfiguration3 = SubBtsConfigurations[i];
                                ItemVm.SubBtsEquipment3 = SubBtsEquipments[i];
                                ItemVm.SubBtsOperatorID3 = SubBtsOperatorIDs[i];
                                ItemVm.SubBtsPowerSum3 = SubBtsPowerSums[i];
                                break;
                            case 4:
                                ItemVm.SubBtsAntenHeight4 = SubBtsAntenHeights[i];
                                ItemVm.SubBtsAntenNum4 = SubBtsAntenNums[i];
                                ItemVm.SubBtsBand4 = SubBtsBands[i];
                                ItemVm.SubBtsCode4 = SubBtsCodes[i];
                                ItemVm.SubBtsConfiguration4 = SubBtsConfigurations[i];
                                ItemVm.SubBtsEquipment4 = SubBtsEquipments[i];
                                ItemVm.SubBtsOperatorID4 = SubBtsOperatorIDs[i];
                                ItemVm.SubBtsPowerSum4 = SubBtsPowerSums[i];
                                break;
                            case 5:
                                ItemVm.SubBtsAntenHeight5 = SubBtsAntenHeights[i];
                                ItemVm.SubBtsAntenNum5 = SubBtsAntenNums[i];
                                ItemVm.SubBtsBand5 = SubBtsBands[i];
                                ItemVm.SubBtsCode5 = SubBtsCodes[i];
                                ItemVm.SubBtsConfiguration5 = SubBtsConfigurations[i];
                                ItemVm.SubBtsEquipment5 = SubBtsEquipments[i];
                                ItemVm.SubBtsOperatorID5 = SubBtsOperatorIDs[i];
                                ItemVm.SubBtsPowerSum5 = SubBtsPowerSums[i];
                                break;
                            case 6:
                                ItemVm.SubBtsAntenHeight6 = SubBtsAntenHeights[i];
                                ItemVm.SubBtsAntenNum6 = SubBtsAntenNums[i];
                                ItemVm.SubBtsBand6 = SubBtsBands[i];
                                ItemVm.SubBtsCode6 = SubBtsCodes[i];
                                ItemVm.SubBtsConfiguration6 = SubBtsConfigurations[i];
                                ItemVm.SubBtsEquipment6 = SubBtsEquipments[i];
                                ItemVm.SubBtsOperatorID6 = SubBtsOperatorIDs[i];
                                ItemVm.SubBtsPowerSum6 = SubBtsPowerSums[i];
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        BackSubBtsQuantity++;
                        string SubBtsOperatorName = ((Operator)_operatorService.getByID(SubBtsOperatorIDs[i])).Name;
                        switch (BackSubBtsQuantity)
                        {
                            case 1:
                                ItemVm.BackSubBtsAntenHeight1 = SubBtsAntenHeights[i];
                                ItemVm.BackSubBtsAntenNum1 = SubBtsAntenNums[i];
                                ItemVm.BackSubBtsBand1 = SubBtsBands[i];
                                ItemVm.BackSubBtsCode1 = SubBtsCodes[i];
                                ItemVm.BackSubBtsConfiguration1 = SubBtsConfigurations[i];
                                ItemVm.BackSubBtsEquipment1 = SubBtsEquipments[i];
                                ItemVm.BackSubBtsOperatorID1 = SubBtsOperatorIDs[i];
                                ItemVm.BackSubBtsOperatorName1 = SubBtsOperatorName;
                                ItemVm.BackSubBtsPowerSum1 = SubBtsPowerSums[i];
                                break;
                            case 2:
                                ItemVm.BackSubBtsAntenHeight2 = SubBtsAntenHeights[i];
                                ItemVm.BackSubBtsAntenNum2 = SubBtsAntenNums[i];
                                ItemVm.BackSubBtsBand2 = SubBtsBands[i];
                                ItemVm.BackSubBtsCode2 = SubBtsCodes[i];
                                ItemVm.BackSubBtsConfiguration2 = SubBtsConfigurations[i];
                                ItemVm.BackSubBtsEquipment2 = SubBtsEquipments[i];
                                ItemVm.BackSubBtsOperatorID2 = SubBtsOperatorIDs[i];
                                ItemVm.BackSubBtsOperatorName2 = SubBtsOperatorName;
                                ItemVm.BackSubBtsPowerSum2 = SubBtsPowerSums[i];
                                break;
                            case 3:
                                ItemVm.BackSubBtsAntenHeight3 = SubBtsAntenHeights[i];
                                ItemVm.BackSubBtsAntenNum3 = SubBtsAntenNums[i];
                                ItemVm.BackSubBtsBand3 = SubBtsBands[i];
                                ItemVm.BackSubBtsCode3 = SubBtsCodes[i];
                                ItemVm.BackSubBtsConfiguration3 = SubBtsConfigurations[i];
                                ItemVm.BackSubBtsEquipment3 = SubBtsEquipments[i];
                                ItemVm.BackSubBtsOperatorID3 = SubBtsOperatorIDs[i];
                                ItemVm.BackSubBtsOperatorName3 = SubBtsOperatorName;
                                ItemVm.BackSubBtsPowerSum3 = SubBtsPowerSums[i];
                                break;
                            case 4:
                                ItemVm.BackSubBtsAntenHeight4 = SubBtsAntenHeights[i];
                                ItemVm.BackSubBtsAntenNum4 = SubBtsAntenNums[i];
                                ItemVm.BackSubBtsBand4 = SubBtsBands[i];
                                ItemVm.BackSubBtsCode4 = SubBtsCodes[i];
                                ItemVm.BackSubBtsConfiguration4 = SubBtsConfigurations[i];
                                ItemVm.BackSubBtsEquipment4 = SubBtsEquipments[i];
                                ItemVm.BackSubBtsOperatorID4 = SubBtsOperatorIDs[i];
                                ItemVm.BackSubBtsOperatorName4 = SubBtsOperatorName;
                                ItemVm.BackSubBtsPowerSum4 = SubBtsPowerSums[i];
                                break;
                            case 5:
                                ItemVm.BackSubBtsAntenHeight5 = SubBtsAntenHeights[i];
                                ItemVm.BackSubBtsAntenNum5 = SubBtsAntenNums[i];
                                ItemVm.BackSubBtsBand5 = SubBtsBands[i];
                                ItemVm.BackSubBtsCode5 = SubBtsCodes[i];
                                ItemVm.BackSubBtsConfiguration5 = SubBtsConfigurations[i];
                                ItemVm.BackSubBtsEquipment5 = SubBtsEquipments[i];
                                ItemVm.BackSubBtsOperatorID5 = SubBtsOperatorIDs[i];
                                ItemVm.BackSubBtsOperatorName5 = SubBtsOperatorName;
                                ItemVm.BackSubBtsPowerSum5 = SubBtsPowerSums[i];
                                break;
                            case 6:
                                ItemVm.BackSubBtsAntenHeight6 = SubBtsAntenHeights[i];
                                ItemVm.BackSubBtsAntenNum6 = SubBtsAntenNums[i];
                                ItemVm.BackSubBtsBand6 = SubBtsBands[i];
                                ItemVm.BackSubBtsCode6 = SubBtsCodes[i];
                                ItemVm.BackSubBtsConfiguration6 = SubBtsConfigurations[i];
                                ItemVm.BackSubBtsEquipment6 = SubBtsEquipments[i];
                                ItemVm.BackSubBtsOperatorID6 = SubBtsOperatorIDs[i];
                                ItemVm.BackSubBtsOperatorName6 = SubBtsOperatorName;
                                ItemVm.BackSubBtsPowerSum6 = SubBtsPowerSums[i];
                                break;
                            default:
                                break;
                        }
                    }
                }
                ItemVm.FrontSubBtsQuantity = FrontSubBtsQuantity;
                ItemVm.BackSubBtsQuantity = BackSubBtsQuantity;
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

            string[] SubBtsAntenHeights, SubBtsAntenNums, SubBtsBands, SubBtsCodes, SubBtsConfigurations, SubBtsEquipments, SubBtsOperatorIDs, SubBtsPowerSums;
            int FrontSubBtsQuantity, BackSubBtsQuantity;

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

            if (getEnableCityIDsScope() == "True")
            {
                DbItems = DbItems.Where(x => getCityIDsScope().Split(new char[] { ';' }).Contains(x.CityID)).ToList();
            }

            //DbItems = DbItems.OrderByDescending(x => x.IssuedDate.Year.ToString() + x.Id);

            IEnumerable<PrintCertificateViewModel> printCertificates = Mapper.Map<List<PrintCertificateViewModel>>(DbItems);

            foreach (var ItemVm in printCertificates)
            {
                ItemVm.OperatorName = ItemVm.Operator.Name?.ToUpper();
                ItemVm.ApplicantName = _profileService.getByID(ItemVm.ProfileID, new string[] { "Applicant" }).Applicant.Name?.ToUpper();
                ItemVm.SignatureFileName = Server.MapPath("~/Content/ReportTemplates/" + ItemVm.Signer.ToUnsignString() + ".jpg");
                ItemVm.SubSignFileName = Server.MapPath("~/Content/ReportTemplates/" + ItemVm.Verifier1.ToUnsignString() + ".jpg");
                ItemVm.StrIssuedDate = "Ngày " + ItemVm.IssuedDate.Day + " tháng " + ItemVm.IssuedDate.Month + " năm " + ItemVm.IssuedDate.Year;


                SubBtsAntenHeights = ItemVm.SubBtsAntenHeights.Split(new char[] { ';' });
                SubBtsAntenNums = ItemVm.SubBtsAntenNums.Split(new char[] { ';' });
                SubBtsBands = ItemVm.SubBtsBands.Split(new char[] { ';' });
                SubBtsCodes = ItemVm.SubBtsCodes.Split(new char[] { ';' });
                SubBtsConfigurations = ItemVm.SubBtsConfigurations.Split(new char[] { ';' });
                SubBtsEquipments = ItemVm.SubBtsEquipments.Split(new char[] { ';' });
                SubBtsOperatorIDs = ItemVm.SubBtsOperatorIDs.Split(new char[] { ';' });
                SubBtsPowerSums = ItemVm.SubBtsPowerSums.Split(new char[] { ';' });


                FrontSubBtsQuantity = 0;
                BackSubBtsQuantity = 0;
                for (int i = 0; i < ItemVm.SubBtsQuantity; i++)
                {
                    if (ItemVm.OperatorID == SubBtsOperatorIDs[i])
                    {
                        FrontSubBtsQuantity++;
                        switch (FrontSubBtsQuantity)
                        {
                            case 1:
                                ItemVm.SubBtsAntenHeight1 = SubBtsAntenHeights[i];
                                ItemVm.SubBtsAntenNum1 = SubBtsAntenNums[i];
                                ItemVm.SubBtsBand1 = SubBtsBands[i];
                                ItemVm.SubBtsCode1 = SubBtsCodes[i];
                                ItemVm.SubBtsConfiguration1 = SubBtsConfigurations[i];
                                ItemVm.SubBtsEquipment1 = SubBtsEquipments[i];
                                ItemVm.SubBtsOperatorID1 = SubBtsOperatorIDs[i];
                                ItemVm.SubBtsPowerSum1 = SubBtsPowerSums[i];
                                break;
                            case 2:
                                ItemVm.SubBtsAntenHeight2 = SubBtsAntenHeights[i];
                                ItemVm.SubBtsAntenNum2 = SubBtsAntenNums[i];
                                ItemVm.SubBtsBand2 = SubBtsBands[i];
                                ItemVm.SubBtsCode2 = SubBtsCodes[i];
                                ItemVm.SubBtsConfiguration2 = SubBtsConfigurations[i];
                                ItemVm.SubBtsEquipment2 = SubBtsEquipments[i];
                                ItemVm.SubBtsOperatorID2 = SubBtsOperatorIDs[i];
                                ItemVm.SubBtsPowerSum2 = SubBtsPowerSums[i];
                                break;
                            case 3:
                                ItemVm.SubBtsAntenHeight3 = SubBtsAntenHeights[i];
                                ItemVm.SubBtsAntenNum3 = SubBtsAntenNums[i];
                                ItemVm.SubBtsBand3 = SubBtsBands[i];
                                ItemVm.SubBtsCode3 = SubBtsCodes[i];
                                ItemVm.SubBtsConfiguration3 = SubBtsConfigurations[i];
                                ItemVm.SubBtsEquipment3 = SubBtsEquipments[i];
                                ItemVm.SubBtsOperatorID3 = SubBtsOperatorIDs[i];
                                ItemVm.SubBtsPowerSum3 = SubBtsPowerSums[i];
                                break;
                            case 4:
                                ItemVm.SubBtsAntenHeight4 = SubBtsAntenHeights[i];
                                ItemVm.SubBtsAntenNum4 = SubBtsAntenNums[i];
                                ItemVm.SubBtsBand4 = SubBtsBands[i];
                                ItemVm.SubBtsCode4 = SubBtsCodes[i];
                                ItemVm.SubBtsConfiguration4 = SubBtsConfigurations[i];
                                ItemVm.SubBtsEquipment4 = SubBtsEquipments[i];
                                ItemVm.SubBtsOperatorID4 = SubBtsOperatorIDs[i];
                                ItemVm.SubBtsPowerSum4 = SubBtsPowerSums[i];
                                break;
                            case 5:
                                ItemVm.SubBtsAntenHeight5 = SubBtsAntenHeights[i];
                                ItemVm.SubBtsAntenNum5 = SubBtsAntenNums[i];
                                ItemVm.SubBtsBand5 = SubBtsBands[i];
                                ItemVm.SubBtsCode5 = SubBtsCodes[i];
                                ItemVm.SubBtsConfiguration5 = SubBtsConfigurations[i];
                                ItemVm.SubBtsEquipment5 = SubBtsEquipments[i];
                                ItemVm.SubBtsOperatorID5 = SubBtsOperatorIDs[i];
                                ItemVm.SubBtsPowerSum5 = SubBtsPowerSums[i];
                                break;
                            case 6:
                                ItemVm.SubBtsAntenHeight6 = SubBtsAntenHeights[i];
                                ItemVm.SubBtsAntenNum6 = SubBtsAntenNums[i];
                                ItemVm.SubBtsBand6 = SubBtsBands[i];
                                ItemVm.SubBtsCode6 = SubBtsCodes[i];
                                ItemVm.SubBtsConfiguration6 = SubBtsConfigurations[i];
                                ItemVm.SubBtsEquipment6 = SubBtsEquipments[i];
                                ItemVm.SubBtsOperatorID6 = SubBtsOperatorIDs[i];
                                ItemVm.SubBtsPowerSum6 = SubBtsPowerSums[i];
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        BackSubBtsQuantity++;
                        string SubBtsOperatorName = ((Operator)_operatorService.getByID(SubBtsOperatorIDs[i])).Name;
                        switch (BackSubBtsQuantity)
                        {
                            case 1:
                                ItemVm.BackSubBtsAntenHeight1 = SubBtsAntenHeights[i];
                                ItemVm.BackSubBtsAntenNum1 = SubBtsAntenNums[i];
                                ItemVm.BackSubBtsBand1 = SubBtsBands[i];
                                ItemVm.BackSubBtsCode1 = SubBtsCodes[i];
                                ItemVm.BackSubBtsConfiguration1 = SubBtsConfigurations[i];
                                ItemVm.BackSubBtsEquipment1 = SubBtsEquipments[i];
                                ItemVm.BackSubBtsOperatorID1 = SubBtsOperatorIDs[i];
                                ItemVm.BackSubBtsOperatorName1 = SubBtsOperatorName;
                                ItemVm.BackSubBtsPowerSum1 = SubBtsPowerSums[i];
                                break;
                            case 2:
                                ItemVm.BackSubBtsAntenHeight2 = SubBtsAntenHeights[i];
                                ItemVm.BackSubBtsAntenNum2 = SubBtsAntenNums[i];
                                ItemVm.BackSubBtsBand2 = SubBtsBands[i];
                                ItemVm.BackSubBtsCode2 = SubBtsCodes[i];
                                ItemVm.BackSubBtsConfiguration2 = SubBtsConfigurations[i];
                                ItemVm.BackSubBtsEquipment2 = SubBtsEquipments[i];
                                ItemVm.BackSubBtsOperatorID2 = SubBtsOperatorIDs[i];
                                ItemVm.BackSubBtsOperatorName2 = SubBtsOperatorName;
                                ItemVm.BackSubBtsPowerSum2 = SubBtsPowerSums[i];
                                break;
                            case 3:
                                ItemVm.BackSubBtsAntenHeight3 = SubBtsAntenHeights[i];
                                ItemVm.BackSubBtsAntenNum3 = SubBtsAntenNums[i];
                                ItemVm.BackSubBtsBand3 = SubBtsBands[i];
                                ItemVm.BackSubBtsCode3 = SubBtsCodes[i];
                                ItemVm.BackSubBtsConfiguration3 = SubBtsConfigurations[i];
                                ItemVm.BackSubBtsEquipment3 = SubBtsEquipments[i];
                                ItemVm.BackSubBtsOperatorID3 = SubBtsOperatorIDs[i];
                                ItemVm.BackSubBtsOperatorName3 = SubBtsOperatorName;
                                ItemVm.BackSubBtsPowerSum3 = SubBtsPowerSums[i];
                                break;
                            case 4:
                                ItemVm.BackSubBtsAntenHeight4 = SubBtsAntenHeights[i];
                                ItemVm.BackSubBtsAntenNum4 = SubBtsAntenNums[i];
                                ItemVm.BackSubBtsBand4 = SubBtsBands[i];
                                ItemVm.BackSubBtsCode4 = SubBtsCodes[i];
                                ItemVm.BackSubBtsConfiguration4 = SubBtsConfigurations[i];
                                ItemVm.BackSubBtsEquipment4 = SubBtsEquipments[i];
                                ItemVm.BackSubBtsOperatorID4 = SubBtsOperatorIDs[i];
                                ItemVm.BackSubBtsOperatorName4 = SubBtsOperatorName;
                                ItemVm.BackSubBtsPowerSum4 = SubBtsPowerSums[i];
                                break;
                            case 5:
                                ItemVm.BackSubBtsAntenHeight5 = SubBtsAntenHeights[i];
                                ItemVm.BackSubBtsAntenNum5 = SubBtsAntenNums[i];
                                ItemVm.BackSubBtsBand5 = SubBtsBands[i];
                                ItemVm.BackSubBtsCode5 = SubBtsCodes[i];
                                ItemVm.BackSubBtsConfiguration5 = SubBtsConfigurations[i];
                                ItemVm.BackSubBtsEquipment5 = SubBtsEquipments[i];
                                ItemVm.BackSubBtsOperatorID5 = SubBtsOperatorIDs[i];
                                ItemVm.BackSubBtsOperatorName5 = SubBtsOperatorName;
                                ItemVm.BackSubBtsPowerSum5 = SubBtsPowerSums[i];
                                break;
                            case 6:
                                ItemVm.BackSubBtsAntenHeight6 = SubBtsAntenHeights[i];
                                ItemVm.BackSubBtsAntenNum6 = SubBtsAntenNums[i];
                                ItemVm.BackSubBtsBand6 = SubBtsBands[i];
                                ItemVm.BackSubBtsCode6 = SubBtsCodes[i];
                                ItemVm.BackSubBtsConfiguration6 = SubBtsConfigurations[i];
                                ItemVm.BackSubBtsEquipment6 = SubBtsEquipments[i];
                                ItemVm.BackSubBtsOperatorID6 = SubBtsOperatorIDs[i];
                                ItemVm.BackSubBtsOperatorName6 = SubBtsOperatorName;
                                ItemVm.BackSubBtsPowerSum6 = SubBtsPowerSums[i];
                                break;
                            default:
                                break;
                        }
                    }
                }
                ItemVm.FrontSubBtsQuantity = FrontSubBtsQuantity;
                ItemVm.BackSubBtsQuantity = BackSubBtsQuantity;
            }
            Session["printCertificates"] = printCertificates;
            return View();
        }

        public ActionResult GetReport(bool Template = false, string BtsNum = "")
        {
            IEnumerable<PrintCertificateViewModel> printCertificates = (IEnumerable<PrintCertificateViewModel>)Session["printCertificates"];
            if (string.IsNullOrEmpty(BtsNum) && printCertificates != null)
            {
                BtsNum = "1";
                int[] BtsNums = new int[6];
                foreach (var item in printCertificates)
                {
                    BtsNums[item.FrontSubBtsQuantity - 1]++;
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
            report.Load(Server.MapPath("~/Content/ReportTemplates/GCNKD_" + BtsNum + "BTS.mrt"));

            report.Dictionary.Variables["IsTemplate"].ValueObject = Template;
            if (Template)
            {
                var page = report.Pages[0];

                page.Watermark.Image = System.Drawing.Image.FromFile(Server.MapPath("~/Content/ReportTemplates/Watermark.jpg"));
                page.Watermark.AspectRatio = true;
                page.Watermark.ImageMultipleFactor = 0.5;
                page.Watermark.ImageTransparency = 0;
                page.Watermark.ImageStretch = true;
                page.Watermark.ShowImageBehind = true;
            }
            report.RegData(printCertificates?.ToDataSet("DataSet", "Certificates"));
            
            return StiMvcViewer.GetReportResult(report);
        }

        public ActionResult ViewerEvent()
        {
            var TempData1 = StiMvcViewer.GetRequestParams();
            var TempData2 = StiMvcViewer.GetFormValues();
            var TempData3 = StiMvcViewer.GetRouteValues();
            var TempData4 = StiMvcViewer.ViewerEventResult();
            return StiMvcViewer.ViewerEventResult();
        }
    }
}