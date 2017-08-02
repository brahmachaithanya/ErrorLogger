using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DataBase;

namespace Rest.Controllers
{
    using System.Web.Http;
    public class restController : ApiController
    {
        private bsadashiDataBaseEntities db = new bsadashiDataBaseEntities();

                // POST: api/rest
        [HttpPost]
        [ResponseType(typeof(error_log))]
        public async Task<IHttpActionResult> Posterror_log(error_log error_log)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.error_log.Add(error_log);

            try
            {
                await db.SaveChangesAsync();
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

            return CreatedAtRoute("DefaultApi", new { id = error_log.error_id }, error_log);
        }

                protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool error_logExists(string id)
        {
            return db.error_log.Count(e => e.error_id == id) > 0;
        }
    }
}