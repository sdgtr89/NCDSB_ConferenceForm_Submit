using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NCDSB_ConferenceForm_Submit.DAL;
using NCDSB_ConferenceForm_Submit.Models;

namespace NCDSB_ConferenceForm_Submit.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private ConferenceFormEntities db = new ConferenceFormEntities();

        // GET: Expense
        public ActionResult Index()
        {
            var expenses = db.Expenses
                .Include(e => e.ConferenceForm)
                .Include(e => e.ExpenseType)
                .Include(e => e.BudgetCode)
                .Include(e => e.Receipt);
            return View(expenses.ToList());
        }

        // GET: Expense/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            return View(expense);
        }

        // GET: Expense/Create
        public ActionResult Add(int conferenceFormID, string conferenceSummary)
        {
            Expense expense = new Expense()
            {
                ConferenceFormID = conferenceFormID
            };

            ViewBag.ConferenceForm = conferenceSummary;
            ViewBag.ConferenceFormID = conferenceFormID;

            PopulateDropDownList();           

            return View(expense);
        }

        // POST: Expense/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "ID,ConferenceFormID,ExpenseTypeID,ExpenseEstAmount,Reason,ExpenseActAmount," +
            "BudgetCodeID,ReceiptID,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                db.Expenses.Add(expense);
                db.SaveChanges();
                return RedirectToAction("Details", "ConferenceForms", new { id = expense.ConferenceFormID });
            }

            PopulateDropDownList();
            return View(expense);
        }

        // GET: Expense/Edit/5
        public ActionResult Update(int? id, int conferenceFormID, string conferenceSummary)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Expense expense = db.Expenses.Find(id);

            if (expense == null)
            {
                return HttpNotFound();
            }

            ViewBag.ConferenceForm = conferenceSummary;
            ViewBag.ConferenceFormID = conferenceFormID;
            PopulateDropDownList(expense);
            return View(expense);
        }

        // POST: Expense/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Update")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePost(int? id, int conferenceFormID, string conferenceSummary, Byte[] rowVersion)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Expense expenseToUpdate = db.Expenses.Find(id);

            if (TryUpdateModel(expenseToUpdate, "",
                new string[] { "ID", "ExpneseTypeID", "ExpenseEstAmount", "Reason", "BudgetCodeID" }))
            {
                try
                {
                    db.Entry(expenseToUpdate).State = EntityState.Modified;
                    db.Entry(expenseToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    db.SaveChanges();
                    return RedirectToAction("Details", "ConferenceForms", new { id = conferenceFormID });
                }
                catch (DbUpdateConcurrencyException ex)// Added for concurrency
                {
                    var entry = ex.Entries.Single();
                    var theValues = (Expense)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "Unable to save changes. The expense was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Expense)databaseEntry.ToObject();                        
                        if (databaseValues.ExpenseTypeID != theValues.ExpenseTypeID)
                            ModelState.AddModelError("ExpenseTypeID", "Current value: "
                                + db.ExpenseTypes.Find(databaseValues.ExpenseTypeID).TypeOfExpense);
                        if (databaseValues.ExpenseEstAmount != theValues.ExpenseEstAmount)
                            ModelState.AddModelError("ExpenseEstAmount", "Current value: "
                                + databaseValues.ExpenseEstAmount);
                        if (databaseValues.Reason != theValues.Reason)
                            ModelState.AddModelError("Reason", "Current value: "
                                + databaseValues.Reason);
                        if (databaseValues.BudgetCodeID != theValues.BudgetCodeID)
                            ModelState.AddModelError("BudgetCodeID", "Current value: "
                                + db.BudgetCodes.Find(databaseValues.BudgetCodeID).CodeType);
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you received your values. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to save your version of this record, click "
                                + "the Save button again.");

                        expenseToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
            }

            PopulateDropDownList(expenseToUpdate);
            return View(expenseToUpdate);
        }

        // GET: Expense/Confirm/5
        /// <summary>
        /// Method for retrieving the expense for confirmation and receipt upload
        /// </summary>
        /// <param name="id">Expense ID</param>
        /// <param name="conferenceFormID">Conference Form ID</param>
        /// <param name="conferenceSummary">Summary of Conference</param>
        /// <returns></returns>
        public ActionResult Confirm(int? id, int conferenceFormID, string conferenceSummary)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Expense expense = db.Expenses.Find(id);

            if (expense == null)
            {
                return HttpNotFound();
            }

            ViewBag.ConferenceForm = conferenceSummary;
            ViewBag.ConferenceFormID = conferenceFormID;
            PopulateDropDownList(expense);
            return View(expense);
        }

        // POST: Expense/Confirm/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Method for posting the actual expense amount along with the receipt confirmation
        /// </summary>
        /// <param name="id">Expense ID</param>
        /// <param name="conferenceFormID">Conference Form ID</param>
        /// <param name="conferenceSummary">Summary of Conference</param>
        /// <param name="rowVersion">Rowversion</param>
        /// <returns></returns>
        [HttpPost, ActionName("Confirm")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmPost(int? id, int? conferenceFormID, string conferenceSummary, Byte[] rowVersion)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Expense expenseToUpdate = db.Expenses.Find(id);

            if (TryUpdateModel(expenseToUpdate, "",
                new string[] { "ID", "ExpneseTypeID", "ExpenseEstAmount", "ExpenseActAmount", "Reason", "BudgetCodeID" }))
            {
                try
                {
                    AddPicture(ref expenseToUpdate, Request.Files["receiptUpload"]);
                    db.Entry(expenseToUpdate).State = EntityState.Modified;
                    db.Entry(expenseToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    db.SaveChanges();
                    return RedirectToAction("Details", "ConferenceForms", new { id = conferenceFormID });
                }
                catch (DbUpdateConcurrencyException ex)// Added for concurrency
                {
                    var entry = ex.Entries.Single();
                    var theValues = (Expense)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "Unable to save changes. The expense was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Expense)databaseEntry.ToObject();
                        if (databaseValues.ExpenseTypeID != theValues.ExpenseTypeID)
                            ModelState.AddModelError("ExpenseTypeID", "Current value: "
                                + db.ExpenseTypes.Find(databaseValues.ExpenseTypeID).TypeOfExpense);
                        if (databaseValues.ExpenseEstAmount != theValues.ExpenseEstAmount)
                            ModelState.AddModelError("ExpenseEstAmount", "Current value: "
                                + databaseValues.ExpenseEstAmount);
                        if (databaseValues.ExpenseActAmount != theValues.ExpenseActAmount)
                            ModelState.AddModelError("ExpenseActAmount", "Current value: "
                                + databaseValues.ExpenseActAmount);
                        if (databaseValues.Reason != theValues.Reason)
                            ModelState.AddModelError("Reason", "Current value: "
                                + databaseValues.Reason);
                        if (databaseValues.BudgetCodeID != theValues.BudgetCodeID)
                            ModelState.AddModelError("BudgetCodeID", "Current value: "
                                + db.BudgetCodes.Find(databaseValues.BudgetCodeID).CodeType);
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you received your values. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to save your version of this record, click "
                                + "the Save button again.");

                        expenseToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
            }

            PopulateDropDownList(expenseToUpdate);
            return View(expenseToUpdate);
        }

        // GET: Expense/Delete/5
        public ActionResult Remove(int? id, int conferenceFormID, string conferenceSummary)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Expense expense = db.Expenses.Find(id);

            if (expense == null)
            {
                return HttpNotFound();
            }

            ViewBag.ConferenceForm = conferenceSummary;
            ViewBag.ConferenceFormID = conferenceFormID;
            return View(expense);
        }

        // POST: Expense/Delete/5
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveConfirmed(int? id, int conferenceFormID, string conferenceSummary)
        {
            Expense expense = db.Expenses.Find(id);
            db.Entry(expense).State = EntityState.Modified;
            expense.IsRemoved = true;
            db.SaveChanges();
            ViewBag.ConferenceForm = conferenceSummary;
            ViewBag.ConferenceFormID = conferenceFormID;
            return RedirectToAction("Details", "ConferenceForms", new { id = conferenceFormID });
        }

        private void AddPicture(ref Expense expense, HttpPostedFileBase r)
        {
            if (r != null)
            {
                string mimeType = r.ContentType;
                int fileLength = r.ContentLength;
                if ((mimeType.Contains("image") && fileLength > 0))//Looks like we have a file!!!
                {
                    //Save the full size image
                    Stream fileStream = r.InputStream;
                    byte[] fullData = new byte[fileLength];
                    fileStream.Read(fullData, 0, fileLength);
                    //This is used for both Create and Edit so must decide
                    if (expense.Receipt == null)//Create New 
                    {
                        Receipt receipt = new Receipt
                        {
                            Content = fullData,
                            MimeType = mimeType
                        };
                        expense.Receipt = receipt;
                    }
                    else //Update the current image
                    {
                        expense.Receipt.Content = fullData;
                        expense.Receipt.MimeType = mimeType;
                    }
                }
            }

        }

        private void PopulateDropDownList(Expense expense = null)
        {
            var expenses = from e in db.ExpenseTypes
                            orderby e.TypeOfExpense
                            select e;

            var budgetCode = from a in db.BudgetCodes
                             orderby a.CodeType
                             select a;

            ViewBag.ExpenseTypeID = new SelectList(expenses, "ID", "TypeOfExpense", expense?.ExpenseTypeID);
            ViewBag.BudgetCodeID = new SelectList(budgetCode, "ID", "CodeType", expense?.BudgetCodeID);
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
