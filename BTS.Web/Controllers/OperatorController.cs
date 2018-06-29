using AutoMapper;
using BTS.Model.Models;
using BTS.Service;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTS.Web.Infrastructure.Extensions;
using System.Web.Script.Serialization;

namespace BTS.Web.Controllers
{
    public class OperatorController : BaseController
    {
        private IOperatorService _operatorService;

        public OperatorController(IOperatorService operatorService, IErrorService errorService) : base(errorService)
        {
            _operatorService = operatorService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            var model = _operatorService.getAll().ToList();
            var data = Mapper.Map<List<OperatorViewModel>>(model);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View(new OperatorViewModel());
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            return View(Mapper.Map<OperatorViewModel>(_operatorService.getByID(id)));
        }

        [HttpGet]
        public ActionResult AddOrEdit(string id = "")
        {
            if (string.IsNullOrEmpty(id))
                return View(new OperatorViewModel());
            else
            {
                return View(Mapper.Map<OperatorViewModel>(_operatorService.getByID(id)));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveData(OperatorViewModel itemVm)
        {
            if (string.IsNullOrEmpty(itemVm.Id))
            {
                Operator newItem = new Operator();
                newItem.UpdateOperator(itemVm);
                newItem.CreatedBy = User.Identity.Name;
                newItem.CreatedDate = DateTime.Now;

                _operatorService.Add(newItem);
                _operatorService.SaveChanges();
                return Json(new { success = true, message = "Added Successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var dbItem = _operatorService.getByID(itemVm.Id);
                if (dbItem == null)
                {
                    Operator newItem = new Operator();
                    newItem.UpdateOperator(itemVm);
                    newItem.CreatedBy = User.Identity.Name;
                    newItem.CreatedDate = DateTime.Now;

                    _operatorService.Add(newItem);

                    _operatorService.SaveChanges();
                    return Json(new { success = true, message = "Added Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    dbItem.UpdateOperator(itemVm);
                    dbItem.UpdatedBy = User.Identity.Name;
                    dbItem.UpdatedDate = DateTime.Now;

                    _operatorService.Update(dbItem);
                    _operatorService.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveData_Tedu(string strOperator)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            OperatorViewModel itemVm = serializer.Deserialize<OperatorViewModel>(strOperator);

            if (string.IsNullOrEmpty(itemVm.Id))
            {
                Operator newItem = new Operator();
                newItem.UpdateOperator(itemVm);
                newItem.CreatedBy = User.Identity.Name;
                newItem.CreatedDate = DateTime.Now;

                _operatorService.Add(newItem);
                _operatorService.SaveChanges();
                return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var dbItem = _operatorService.getByID(itemVm.Id);
                dbItem.UpdateOperator(itemVm);
                dbItem.UpdatedBy = User.Identity.Name;
                dbItem.UpdatedDate = DateTime.Now;

                _operatorService.Update(dbItem);
                _operatorService.SaveChanges();

                return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            _operatorService.Delete(id);
            _operatorService.SaveChanges();
            return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
        }
    }
}