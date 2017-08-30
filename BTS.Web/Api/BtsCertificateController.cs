using AutoMapper;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Infastructure.Core;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BTS.Web.Infastructure.Extensions;

namespace BTS.Web.Api
{
    [RoutePrefix("api/BtsCertificate")]
    public class BtsCertificateController : ApiControllerBase
    {
        IBTSCertificateService _btsCertificateService;

        public BtsCertificateController(IErrorService errorService, IBTSCertificateService btsCertificateService) : base(errorService)
        {
            this._btsCertificateService = btsCertificateService;
        }
        [Route("getall")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {            
            int totalRow = 10;

            return CreateHttpResponse(request, () =>
            {
                var listBtsCertificate = _btsCertificateService.getAll(out totalRow);

                var listBtsCertificateVm = Mapper.Map<List<BTSCertificateViewModel>>(listBtsCertificate);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, listBtsCertificateVm);

                return response;
            });
        }

        [Route("add")]
        public HttpResponseMessage Post(HttpRequestMessage request, BTSCertificateViewModel btsCertificateVm)
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
                    BTSCertificate  newBTSCertificate = new BTSCertificate();
                    newBTSCertificate.UpdateBTSCertificate(btsCertificateVm);
                    newBTSCertificate = _btsCertificateService.Add(newBTSCertificate);
                    _btsCertificateService.Save();

                    response = request.CreateResponse(HttpStatusCode.Created, newBTSCertificate);

                }
                return response;
            });
        }

        [Route("update")]
        public HttpResponseMessage Put(HttpRequestMessage request, BTSCertificateViewModel btsCertificateVm)
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
                    var btsCertificate = _btsCertificateService.getByID(btsCertificateVm.ID);
                    btsCertificate.UpdateBTSCertificate(btsCertificateVm);

                    _btsCertificateService.Update(btsCertificate);
                    _btsCertificateService.Save();                    
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
                    _btsCertificateService.Delete(id);
                    _btsCertificateService.Save();                    
                    response = request.CreateResponse(HttpStatusCode.OK);

                }
                return response;
            });
        }
    }
}