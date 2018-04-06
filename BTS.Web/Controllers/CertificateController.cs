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
    public class CertificateController : BaseController
    {
        private ICertificateService _btsCertificateService;

        public CertificateController(ICertificateService btsCertificateService, IErrorService errorService) : base(errorService)
        {
            _btsCertificateService = btsCertificateService;
        }

        // GET: Certificate
        public ActionResult Index()
        {
            int countBTS = 0;
            List<Certificate> data = _btsCertificateService.getAll(out countBTS, true).ToList();
            ViewBag.Certificates = data;
            return View();
        }

        public ActionResult PivotTable()
        {
            return View();
        }

        public ActionResult GetCertificateByCity(string cityID)
        {
            var CertificateData = _btsCertificateService.getCertificateByCity(cityID);
            var model = Mapper.Map<IEnumerable<Certificate>, IEnumerable<CertificateViewModel>>(CertificateData);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCertificate()
        {
            int countBTS = 0;
            List<Certificate> dataSumary = _btsCertificateService.getAll(out countBTS, true).ToList();

            return Json(dataSumary, JsonRequestBehavior.AllowGet);
        }
    }
}