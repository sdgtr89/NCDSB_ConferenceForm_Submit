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
    public class BudgetCodesController : Controller
    {
        private ConferenceFormEntities db = new ConferenceFormEntities();

        // GET: BudgetCodes
        public ActionResult Index()
        {
            return View(db.BudgetCodes.ToList());
        }

        // GET: BudgetCodes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BudgetCode budgetCode = db.BudgetCodes.Find(id);

            if (budgetCode == null)
            {
                return HttpNotFound();
            }
            return View(budgetCode);
        }

        // GET: BudgetCodes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BudgetCodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CodeType")] BudgetCode budgetCode)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.BudgetCodes.Add(budgetCode);
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
                    ModelState.AddModelError("", "Unable to save changes. The budget code must be unique.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
                }
            }
            return View(budgetCode);
        }

        // GET: BudgetCodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BudgetCode budgetCode = db.BudgetCodes.Find(id);

            if (budgetCode == null)
            {
                return HttpNotFound();
            }
            return View(budgetCode);
        }

        // POST: BudgetCodes/Edit/5
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

            var budgetCodeToUpdate = db.BudgetCodes.Find(id);

            if (TryUpdateModel(budgetCodeToUpdate, "", new string[] { "ID", "CodeType" }))
            {
                try
                {
                    db.Entry(budgetCodeToUpdate).State = EntityState.Modified;
                    db.Entry(budgetCodeToUpdate).OriginalValues["RowVersion"] = rowVersion;
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
                    var theValues = (BudgetCode)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "Unable to save changes. The budget code was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (BudgetCode)databaseEntry.ToObject();

                        if (databaseValues.CodeType != theValues.CodeType)
                            ModelState.AddModelError("CodeType", "Current value: "
                                + databaseValues.CodeType);

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you received your values. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to save your version of this "
                                + "record, click the Save button again.");

                        budgetCodeToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (DataException dex)
                {
                    if (dex.InnerException.InnerException.Message.Contains("IX_"))
                    {
                        ModelState.AddModelError("", "Unable to save changes. The budget code must be unique.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
                    }
                }
            }
            return View(budgetCodeToUpdate);
        }

        // GET: BudgetCodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetCode budgetCode = db.BudgetCodes.Find(id);
            if (budgetCode == null)
            {
                return HttpNotFound();
            }
            return View(budgetCode);
        }

        // POST: BudgetCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BudgetCode budgetCode = db.BudgetCodes.Find(id);
            db.BudgetCodes.Remove(budgetCode);
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
