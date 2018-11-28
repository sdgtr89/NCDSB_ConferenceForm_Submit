using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NCDSB_ConferenceForm_Submit.DAL;
using NCDSB_ConferenceForm_Submit.Models;

namespace NCDSB_ConferenceForm_Submit.Controllers
{
    [Authorize]
    public class FormStatusController : Controller
    {
        private ConferenceFormEntities db = new ConferenceFormEntities();

        // GET: FormStatus
        public ActionResult Index()
        {
            return View(db.FormStatus.ToList());
        }

        // GET: FormStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            FormStatus formStatus = db.FormStatus.Find(id);

            if (formStatus == null)
            {
                return HttpNotFound();
            }
            return View(formStatus);
        }

        // GET: FormStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FormStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,StatusType,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] FormStatus formStatus)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.FormStatus.Add(formStatus);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch (DataException dex)
            {
                if (dex.InnerException.InnerException.Message.Contains("IX_"))
                {
                    ModelState.AddModelError("", "Unable to save changes. The form status must be unique.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
                }
            }
            return View(formStatus);
        }

        // GET: FormStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            FormStatus formStatus = db.FormStatus.Find(id);

            if (formStatus == null)
            {
                return HttpNotFound();
            }
            return View(formStatus);
        }

        // POST: FormStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, Byte[] rowVersion)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var formStatusToUpdate = db.FormStatus.Find(id);

            if (TryUpdateModel(formStatusToUpdate, "", new string[] { "ID", "StatusType" }))
            {
                try
                {
                    db.Entry(formStatusToUpdate).State = EntityState.Modified;
                    db.Entry(formStatusToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var theValues = (FormStatus)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "Unable to save changes. The form status was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (FormStatus)databaseEntry.ToObject();

                        if (databaseValues.StatusType != theValues.StatusType)
                            ModelState.AddModelError("StatusType", "Current value: "
                                + databaseValues.StatusType);

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you received your values. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to save your version of this "
                                + "record, click the Save button again.");

                        formStatusToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (DataException dex)
                {
                    if (dex.InnerException.InnerException.Message.Contains("IX_"))
                    {
                        ModelState.AddModelError("", "Unable to save changes. The form status must be unique.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
                    }
                }
            }
            return View(formStatusToUpdate);
        }

        // GET: FormStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormStatus formStatus = db.FormStatus.Find(id);
            if (formStatus == null)
            {
                return HttpNotFound();
            }
            return View(formStatus);
        }

        // POST: FormStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FormStatus formStatus = db.FormStatus.Find(id);
            db.FormStatus.Remove(formStatus);
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
