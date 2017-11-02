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
        ICertificateService _btsCertificateService;

        public CertificateController(ICertificateService btsCertificateService, IErrorService errorService) : base(errorService)
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
            var model = Mapper.Map<IEnumerable<Certificate>, IEnumerable<CertificateViewModel>>(CertificateData);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCertificate()
        {
            int countBTS = 0;
            List<Certificate> dataSumary = _btsCertificateService.getAll(out countBTS).ToList();

            return Json(dataSumary, JsonRequestBehavior.AllowGet);
        }
    }
}