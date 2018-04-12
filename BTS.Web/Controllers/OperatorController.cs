using AutoMapper;
using BTS.Service;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTS.Web.Controllers
{
    public class OperatorController : BaseController
    {
        private IOperatorService _operatorService;

        public OperatorController(IOperatorService operatorService, IErrorService errorService) : base(errorService)
        {
            _operatorService = operatorService;
        }

        // GET: Operator
        public ActionResult Index()
        {
            var model = _operatorService.getAll().ToList();
            var data = Mapper.Map<List<OperatorViewModel>>(model);
            return View(data);
        }
    }
}