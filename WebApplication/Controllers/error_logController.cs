using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataBase;
using PagedList;
using System.Collections;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class error_logController : Controller
    {
        private bsadashiDataBaseEntities db = new bsadashiDataBaseEntities();

        //// GET: error_log
        //public ActionResult Index()
        //{
        //    var error_log = db.error_log.Include(e => e.error_type).Include(e => e.application);
        //    return View(error_log.ToList());
        //}
        [HandleError]
        [Authorize]
        public ActionResult Index(string sortOrder,string currentFilter, string searchString,int? page)
        {
            var context = new MyDbContext();
            ViewBag.CurrentSort = sortOrder;

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "error_message" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.CatSortParm = sortOrder == "Cat" ? "cat_dec" : "Cat";
            ViewBag.AppSortParm = sortOrder == "app" ? "app_dec" : "app";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            

            ViewBag.CurrentFilter = searchString;
            var error_log = db.error_log.Include(e => e.error_type).Include(e => e.application);
            if (User.IsInRole("Admin"))
            {

            }
            else
            {
                try
                {
                    string currentUserId = User.Identity.GetUserId();
                    string currentUser = context.Users.FirstOrDefault(x => x.Id == currentUserId).UserName;
                    string app_id_new = db.applications.FirstOrDefault(e => e.owner == currentUser && e.is_active == "yes").app_id;
                    error_log = error_log.Where(s => s.app_id.Contains(app_id_new.Trim()));
                }
                catch (Exception ex)
                {
                    View("Home","Index");
                }
            }
            //app_id_new.Trim());

            if (!String.IsNullOrEmpty(searchString))
            {
                error_log = error_log.Where(s => s.error_message.Contains(searchString)|| s.app_id.Contains(searchString)|| s.error_cat_id.Contains(searchString));

             }
            switch (sortOrder)
            {
                case "error_message":
                    error_log = error_log.OrderByDescending(s => s.error_message);
            break;
      case "Date":
                    error_log = error_log.OrderByDescending(s => s.datetime);
                    break;
      case "date_desc":
                    error_log = error_log.OrderBy(s => s.datetime);
            break;
                case "Cat":
                    error_log = error_log.OrderByDescending(s => s.error_cat_id);
                    break;
                case "cat_dec":
                    error_log = error_log.OrderBy(s => s.error_cat_id);
                    break;

                case "app":
                    error_log = error_log.OrderByDescending(s => s.app_id);
                    break;
                case "app_dec":
                    error_log = error_log.OrderBy(s => s.app_id);
                    break;
                default:
                    error_log = error_log.OrderBy(s => s.error_message);
            break;
        }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(error_log.ToPagedList(pageNumber, pageSize));
        }
        [HandleError]
        [Authorize]
        // GET: error_log/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            error_log error_log = db.error_log.Find(id);
            if (error_log == null)
            {
                return HttpNotFound();
            }
            return View(error_log);
        }
        [HandleError]
        [Authorize]
        public ActionResult Charts()
        {
            try
            {
                ArrayList xValue = new ArrayList();
                ArrayList yValue = new ArrayList();
                List<string> error_cat = db.error_log.Select(p => p.error_cat_id).Distinct().ToList();
                var grouped = db.error_log.GroupBy(i => i.error_cat_id).Select(i => new { Word = i, Count = i.Count() });
                for (int i = 0; i < error_cat.Count();i++)
                {
                    xValue.Add(error_cat[i]);
                }
             
                grouped.ToList().ForEach(rs => yValue.Add(rs.Count));

                new Chart(width: 600, height: 400, theme: ChartTheme.Green).AddTitle("Error Counts").
                    AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue).Write("bmp");
            }
            catch (Exception ex)
            {
            }

            return null;

        }



        [HandleError]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
