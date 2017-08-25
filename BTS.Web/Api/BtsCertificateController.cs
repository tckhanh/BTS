using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Infastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BTS.Web.Api
{
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
            int totalRow = 0;

            return CreateHttpResponse(request, () =>
            {
                var listbtsCertificate = _btsCertificateService.getAll(out totalRow);

                //var listPostCategoryVm = Mapper.Map<List<PostCategoryViewModel>>(listCategory);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, listbtsCertificate);

                return response;
            });
        }

        [Route("add")]
        public HttpResponseMessage Post(HttpRequestMessage request, BTSCertificate btsCertificate)
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
                    var newBTSCertificate = _btsCertificateService.Add(btsCertificate);
                    _btsCertificateService.Save();

                    response = request.CreateResponse(HttpStatusCode.Created, newBTSCertificate);

                }
                return response;
            });
        }

        [Route("update")]
        public HttpResponseMessage Put(HttpRequestMessage request, BTSCertificate btsCertificate)
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