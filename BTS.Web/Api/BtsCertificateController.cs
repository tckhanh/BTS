using AutoMapper;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Infrastructure.Core;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BTS.Web.Infrastructure.Extensions;

namespace BTS.Web.Api
{
    [RoutePrefix("api/BtsCertificate")]
    [Authorize]
    public class BtsCertificateController : ApiControllerBase
    {
        private ICertificateService _certificateService;

        public BtsCertificateController(IErrorService errorService, ICertificateService certificateService) : base(errorService)
        {
            this._certificateService = certificateService;
        }

        [Route("getall")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            int totalRow = 10;

            return CreateHttpResponse(request, () =>
            {
                var listBtsCertificate = _certificateService.getAll(out totalRow);

                var listBtsCertificateVm = Mapper.Map<List<CertificateViewModel>>(listBtsCertificate);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, listBtsCertificateVm);

                return response;
            });
        }

        [Route("add")]
        public HttpResponseMessage Post(HttpRequestMessage request, CertificateViewModel btsCertificateVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    Certificate newBTSCertificate = new Certificate();
                    newBTSCertificate.UpdateCertificate(btsCertificateVm);
                    newBTSCertificate = _certificateService.Add(newBTSCertificate);
                    _certificateService.Save();

                    response = request.CreateResponse(HttpStatusCode.Created, newBTSCertificate);
                }
                return response;
            });
        }

        [Route("update")]
        public HttpResponseMessage Put(HttpRequestMessage request, CertificateViewModel certificateVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var certificate = _certificateService.getByID(certificateVm.ID);
                    certificate.UpdateCertificate(certificateVm);

                    _certificateService.Update(certificate);
                    _certificateService.Save();
                    response = request.CreateResponse(HttpStatusCode.OK);
                }
                return response;
            });
        }

        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    _certificateService.Delete(id);
                    _certificateService.Save();
                    response = request.CreateResponse(HttpStatusCode.OK);
                }
                return response;
            });
        }
    }
}