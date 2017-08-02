using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataBase;

namespace WebApplication.Controllers
{
    public class error_typeController : Controller
    {
        private bsadashiDataBaseEntities db = new bsadashiDataBaseEntities();

        // GET: error_type
        [HandleError]
        [Authorize]
        public ActionResult Index()
        {
            return View(db.error_type.ToList());
        }

        // GET: error_type/Details/5
        [HandleError]
        [Authorize]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            error_type error_type = db.error_type.Find(id);
            if (error_type == null)
            {
                return HttpNotFound();
            }
            return View(error_type);
        }

        [HandleError]
        [Authorize]
        // GET: error_type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: error_type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [HandleError]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "error_cat_id,error_cat_name,error_cat_desc")] error_type error_type)
        {
            if (ModelState.IsValid)
            {
                db.error_type.Add(error_type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(error_type);
        }
        [HandleError]
        [Authorize]
        // GET: error_type/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            error_type error_type = db.error_type.Find(id);
            if (error_type == null)
            {
                return HttpNotFound();
            }
            return View(error_type);
        }

        // POST: error_type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [HandleError]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "error_cat_id,error_cat_name,error_cat_desc")] error_type error_type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(error_type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(error_type);
        }

        // GET: error_type/Delete/5
        [HandleError]
        [Authorize]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            error_type error_type = db.error_type.Find(id);
            if (error_type == null)
            {
                return HttpNotFound();
            }
            return View(error_type);
        }

        // POST: error_type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HandleError]
        [Authorize]
        public ActionResult DeleteConfirmed(string id)
        {
            error_type error_type = db.error_type.Find(id);
            db.error_type.Remove(error_type);
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
