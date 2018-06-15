using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity;
using BTS.Data;
using BTS.Model.Models;

namespace Asp.NETMVCCRUD.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            using (BTSDbContext db = new BTSDbContext())
            {
                List<Operator> empList = db.Operators.ToList<Operator>();
                return Json(new { data = empList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(string id = "")
        {
            if (id == "")
                return View(new Operator());
            else
            {
                using (BTSDbContext db = new BTSDbContext())
                {
                    return View(db.Operators.Where(x => x.ID == id).FirstOrDefault<Operator>());
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEdit(Operator emp)
        {
            using (BTSDbContext db = new BTSDbContext())
            {
                if (emp.ID == "")
                {
                    db.Operators.Add(emp);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(emp).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            using (BTSDbContext db = new BTSDbContext())
            {
                Operator emp = db.Operators.Where(x => x.ID == id).FirstOrDefault<Operator>();
                db.Operators.Remove(emp);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}