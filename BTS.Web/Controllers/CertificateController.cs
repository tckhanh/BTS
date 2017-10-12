using AutoMapper;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Controllers
{
    public class CertificateController : Controller
    {
        IBTSCertificateService _btsCertificateService;

        public CertificateController(IBTSCertificateService btsCertificateService)
        {
            _btsCertificateService = btsCertificateService;
        }

        // GET: Certificate
        public ActionResult Index()
        {            
            return View();
        }

        public ActionResult GetCertificateByCity(string cityID)
        {
            var CertificateData = _btsCertificateService.getCertificateByCity(cityID);
            var model = Mapper.Map<IEnumerable<BTSCertificate>, IEnumerable<CertificateViewModel>>(CertificateData);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }
}