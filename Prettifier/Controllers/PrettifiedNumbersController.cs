using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Prettifier.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Rotativa;

namespace Prettifier.Controllers
{
    [Authorize]
    public class PrettifiedNumbersController : Controller
    {
        // GET: PrettifiedNumbers
        public ActionResult Index()
        {
            List<PrettifiedNumbers> prettifiedNumbers;
            using (var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>())
            {
                prettifiedNumbers = db.PrettifiedNumbers.Include(p => p.User).ToList();
                prettifiedNumbers = prettifiedNumbers.OrderByDescending(p => p.CreatedDate).ToList();
            }
            return View(prettifiedNumbers);

        }

        // GET: PrettifiedNumbers/Create
        public ActionResult Create()
        {
            ViewBag.UserId = User.Identity.GetUserId();
            return View();
        }

        // POST: PrettifiedNumbers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrginalNumber,PrettifiedNumber,PrettifiedCategory,UserId")] PrettifiedNumbers prettifiedNumbers)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            prettifiedNumbers.CreatedDate = DateTime.Now;
            prettifiedNumbers.User = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(prettifiedNumbers.UserId);

            if (ModelState.IsValid)
            {
                using (var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>())
                {
                    db.PrettifiedNumbers.Add(prettifiedNumbers);
                    db.SaveChanges();
                }
            }

            ViewBag.UserId = User.Identity.GetUserId();
            return View(prettifiedNumbers);
        }

        // GET: PrettifiedNumbers/Report
        public ActionResult Report()
        {
            ReportViewModel ReportModel = new ReportViewModel();
            using (var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>())
            {
                var prettifiedNumbers = db.PrettifiedNumbers.ToList();

                //Get Top Most used Prettified Numbers
                ReportModel.TopMostUsedPrettifiedNumbers = (from num in prettifiedNumbers
                                                            group num by num.PrettifiedCategory into g
                                                            orderby g.Count() descending
                                                            select new { Category = g.Key }).Select(t => t.Category).ToList();
                //Get Prettified Numbers Counts Per User
                ReportModel.PrettifiedNumbersByUsers = prettifiedNumbers.GroupBy(p => p.User).Select(result => new UserStats { Email = result.Key.Email, PrettifiedNumbersCount = result.Count() }).ToList();

            }
            return View(ReportModel);

        }

        [AllowAnonymous]
        // GET: PrettifiedNumbers/ExportReport
        public ActionResult Export()
        {
            ReportViewModel ReportModel = new ReportViewModel();
            using (var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>())
            {
                var prettifiedNumbers = db.PrettifiedNumbers.ToList();

                //Get Top Most used Prettified Numbers
                ReportModel.TopMostUsedPrettifiedNumbers = (from num in prettifiedNumbers
                                                            group num by num.PrettifiedCategory into g
                                                            orderby g.Count() descending
                                                            select new { Category = g.Key }).Select(t => t.Category).ToList();
                //Get Prettified Numbers Counts Per User
                ReportModel.PrettifiedNumbersByUsers = prettifiedNumbers.GroupBy(p => p.User).Select(result => new UserStats { Email = result.Key.Email, PrettifiedNumbersCount = result.Count() }).ToList();

            }
            return View(ReportModel);

        }

        [AllowAnonymous]
        // GET: PrettifiedNumbers/ExportPdf
        public ActionResult ExportPdf()
        {
            return new Rotativa.ActionAsPdf("Export");
        }

        [AllowAnonymous]
        // GET: PrettifiedNumbers/ExportExcel
        public ActionResult ExportExcel()
        {
            ReportViewModel ReportModel = new ReportViewModel();
            using (var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>())
            {
                var prettifiedNumbers = db.PrettifiedNumbers.ToList();

                //Get Top Most used Prettified Numbers
                ReportModel.TopMostUsedPrettifiedNumbers = (from num in prettifiedNumbers
                                                            group num by num.PrettifiedCategory into g
                                                            orderby g.Count() descending
                                                            select new { Category = g.Key }).Select(t => t.Category).ToList();
                //Get Prettified Numbers Counts Per User
                ReportModel.PrettifiedNumbersByUsers = prettifiedNumbers.GroupBy(p => p.User).Select(result => new UserStats { Email = result.Key.Email, PrettifiedNumbersCount = result.Count() }).ToList();

            }
            Response.AddHeader("content-disposition", "attachment; filename=PrettifierReport.xls");
            Response.ContentType = "application/ms-excel";
            return View("exportexcel", ReportModel);
        }

    }
}
