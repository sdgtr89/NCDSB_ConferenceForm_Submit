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
    public class ExpenseTypesController : Controller
    {
        private ConferenceFormEntities db = new ConferenceFormEntities();

        // GET: ExpenseTypes
        public ActionResult Index()
        {
            return View(db.ExpenseTypes.ToList());
        }

        // GET: ExpenseTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ExpenseType expenseType = db.ExpenseTypes.Find(id);

            if (expenseType == null)
            {
                return HttpNotFound();
            }

            return View(expenseType);
        }

        // GET: ExpenseTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExpenseTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TypeOfExpense,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] ExpenseType expenseType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.ExpenseTypes.Add(expenseType);
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
                    ModelState.AddModelError("", "Unable to save changes. The expense name must be unique.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
                }
            }

            return View(expenseType);
        }

        // GET: ExpenseTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ExpenseType expenseType = db.ExpenseTypes.Find(id);

            if (expenseType == null)
            {
                return HttpNotFound();
            }

            return View(expenseType);
        }

        // POST: ExpenseTypes/Edit/5
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

            var expenseTypeToUpdate = db.ExpenseTypes.Find(id);

            if (TryUpdateModel(expenseTypeToUpdate, "", 
                new string[] { "ID", "TypeOfExpense" }))
            {
                try
                {
                    db.Entry(expenseTypeToUpdate).State = EntityState.Modified;
                    db.Entry(expenseTypeToUpdate).OriginalValues["RowVersion"] = rowVersion;
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
                    var theValues = (ExpenseType)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "Unable to save changes. The expense type was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (ExpenseType)databaseEntry.ToObject();

                        if (databaseValues.TypeOfExpense != theValues.TypeOfExpense)
                            ModelState.AddModelError("TypeOfExpense", "Current value: "
                                + databaseValues.TypeOfExpense);

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you received your values. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to save your version of this "
                                + "record, click the Save button again.");

                        expenseTypeToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (DataException dex)
                {
                    if (dex.InnerException.InnerException.Message.Contains("IX_"))
                    {
                        ModelState.AddModelError("", "Unable to save changes. The expense type name must be unique.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
                    }
                }
            }
            
            return View(expenseTypeToUpdate);
        }

        // GET: ExpenseTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ExpenseType expenseType = db.ExpenseTypes.Find(id);

            if (expenseType == null)
            {
                return HttpNotFound();
            }

            return View(expenseType);
        }

        // POST: ExpenseTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExpenseType expenseType = db.ExpenseTypes.Find(id);
            db.ExpenseTypes.Remove(expenseType);
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
