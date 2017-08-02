using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataBase;
using Microsoft.AspNet.Identity;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class applicationsController : Controller
    {
        private bsadashiDataBaseEntities db = new bsadashiDataBaseEntities();

        // GET: applications
        [HandleError]
        [Authorize]
        public ActionResult Index()
        {
            var context = new MyDbContext();
            if (User.IsInRole("Admin"))
            {
                return View(db.applications.ToList());
            }
            else
            {

                string currentUserId = User.Identity.GetUserId();
                string currentUser = context.Users.FirstOrDefault(x => x.Id == currentUserId).UserName;

                return View(db.applications.Where(i => i.owner == currentUser && i.is_active=="yes"));
            }
        }
        [HandleError]
        [Authorize]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var context = new MyDbContext();
            string currentUserId = User.Identity.GetUserId();
            string currentUser = context.Users.FirstOrDefault(x => x.Id == currentUserId).UserName;

            application application =db.applications.FirstOrDefault(x => x.app_id == id);
            
            if (application == null)
            {
                return View(Index());
            }
            return View(application);
        }
        [HandleError]
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.owner = new SelectList(db.users, "username", "username");
            return View();
        }

        // POST: applications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "app_id,app_name,app_description,owner,reg_date,is_active")] application application)
        {
            if (ModelState.IsValid)
            {
                application.is_active = "yes";
                 var date1= DateTime.Now;
                
                application.reg_date= date1.ToShortDateString();
                db.applications.Add(application);
                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Diagnostics.Trace.TraceInformation("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                }
                return RedirectToAction("Index");
            }

            return View(application);
        }
        [HandleError]
        [Authorize(Roles = "Admin")]
        // GET: applications/Edit/5
        public ActionResult Disable(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            application application = db.applications.FirstOrDefault(x => x.app_id == id);




            if (application == null)
            {
                return HttpNotFound();
            }

            if (application.is_active.Trim() == "no")
            {
                application.is_active = "yes";
            }
            else
            {
                application.is_active = "no";

            }
            db.SaveChanges();


            return RedirectToAction("Index", "applications");
        }

        // POST: applications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "app_id,app_name,app_description,owner,reg_date,is_active")] application application)
        {
            if (ModelState.IsValid)
            {
                db.Entry(application).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(application);
        }

        // GET: applications/Delete/5
        [HandleError]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            application application = db.applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }
        [HandleError]
        // POST: applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            application application = db.applications.Find(id);
            db.applications.Remove(application);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
