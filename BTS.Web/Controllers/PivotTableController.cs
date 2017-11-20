using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BTS.Web.Controllers
{
    public class PivotTableController : Controller
    {
        // GET: PivotTable
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult CharterColumn()
        //{
        //    var _context = new MVCChartsEntities();
        //    ArrayList xValue = new ArrayList();
        //    ArrayList yValue = new ArrayList();

        //    var Results = (from c in _context.tblMVCCharts select c);
        //    Results.ToList().ForEach(rs => xValue.Add(rs.Growth_Year));
        //    Results.ToList().ForEach(rs => yValue.Add(rs.Growth_Value));

        //    new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
        //    .AddTitle("Chart for Growth.[Column Chart]")
        //    .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue)
        //    .Write("bmp");
        //    return null;
        //}

        //public ActionResult ChartBar()
        //{
        //    var _context = new MVCChartsEntities();
        //    ArrayList xValue = new ArrayList();
        //    ArrayList yValue = new ArrayList();
        //    var resutls = (from d in _context.tblMVCCharts select d);
        //    resutls.ToList().ForEach(x => xValue.Add(x.Growth_Year));
        //    resutls.ToList().ForEach(x => yValue.Add(x.Growth_Value));
        //    new Chart(width: 600, height: 400, theme: ChartTheme.Vanilla)
        //        .AddTitle("Chart for Growth.[Bar Chart]")
        //        .AddSeries("Default", chartType: "Bar", xValue: xValue, yValues: yValue)
        //        .Write("bmp");
        //    return null;
        //}

        //public ActionResult ChartPie()
        //{
        //    var _context = new MVCChartsEntities();
        //    ArrayList xValue = new ArrayList();
        //    ArrayList yValue = new ArrayList();
        //    var resutls = (from d in _context.tblMVCCharts select d);
        //    resutls.ToList().ForEach(x => xValue.Add(x.Growth_Year));
        //    resutls.ToList().ForEach(x => yValue.Add(x.Growth_Value));
        //    new Chart(width: 600, height: 400, theme: ChartTheme.Vanilla)
        //        .AddTitle("Chart for Growth.[Pie Chart]")
        //        .AddLegend("Summary")
        //        .AddSeries("Default", chartType: "Pie", xValue: xValue, yValues: yValue)
        //        .Write("bmp");
        //    return null;
        //}

        //public ActionResult DoughnutChart()
        //{
        //    var _context = new MVCChartsEntities();
        //    ArrayList xValue = new ArrayList();
        //    ArrayList yValue = new ArrayList();
        //    var resutls = (from d in _context.tblMVCCharts select d);
        //    resutls.ToList().ForEach(x => xValue.Add(x.Growth_Year));
        //    resutls.ToList().ForEach(x => yValue.Add(x.Growth_Value));
        //    new Chart(width: 600, height: 400, theme: ChartTheme.Vanilla)
        //        .AddTitle("Chart for Growth.[Doughnut Chart]")
        //        .AddLegend("Summary")
        //        .AddSeries("Default", chartType: "Doughnut", xValue: xValue, yValues: yValue)
        //        .Write("bmp");
        //    return null;
        //}

        //public ActionResult PyramidChart()
        //{
        //    var _context = new MVCChartsEntities();
        //    ArrayList xValue = new ArrayList();
        //    ArrayList yValue = new ArrayList();
        //    var resutls = (from d in _context.tblMVCCharts select d);
        //    resutls.ToList().ForEach(x => xValue.Add(x.Growth_Year));
        //    resutls.ToList().ForEach(x => yValue.Add(x.Growth_Value));
        //    new Chart(width: 600, height: 400, theme: ChartTheme.Vanilla)
        //        .AddTitle("Chart for Growth.[Pyramid Chart]")
        //        .AddLegend("Summary")
        //        .AddSeries("Default", chartType: "Pyramid", xValue: xValue, yValues: yValue)
        //        .Write("bmp");
        //    return null;
        //}
    }
}