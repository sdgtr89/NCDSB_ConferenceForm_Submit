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
using NCDSB_ConferenceForm_Submit.ViewModels;
using PagedList;

namespace NCDSB_ConferenceForm_Submit.Controllers
{
    [Authorize]
    public class ConferenceFormsController : Controller
    {
        private ConferenceFormEntities db = new ConferenceFormEntities();

        [HttpPost]
        public ActionResult GetConferences(string prefix)
        {
            var customers = (from conferences in db.Conferences
                             where conferences.Name.StartsWith(prefix)
                             select new
                             {
                                 label = conferences.Name + " at " + conferences.Address.SiteName,
                                 val = conferences.ID
                             }).ToList();

            return Json(customers);
        }

        // GET: ConferenceForms
        public ActionResult Index(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSort = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";

            var conferenceForms = db.ConferenceForms.AsNoTracking()
                .Where(c => c.StaffMember.Email == User.Identity.Name)
                .Where(c => c.IsRemoved == false)
                .Include(c => c.Conference)
                .Include(c => c.FormStatus)
                .Include(c => c.Mileage)
                .Include(c => c.Expenses);

            switch (sortOrder)
            {
                case "date_desc":
                    conferenceForms = conferenceForms.OrderByDescending(s => s.Date);
                    break;
                
                default:
                    conferenceForms = conferenceForms.OrderBy(s => s.Date);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(conferenceForms.ToPagedList(pageNumber, pageSize));
        }

        // GET: ConferenceForms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ConferenceForm conferenceForm = db.ConferenceForms.Find(id);
            Expense expense = new Expense();
            ViewBag.Expenses = db.Expenses.AsNoTracking()
                .Where(a => a.ConferenceFormID == id)
                .Where(c => c.IsRemoved == false)
                .OrderByDescending(a => a.ExpenseEstAmount).ToList();

            ViewBag.Trips = db.Trips.AsNoTracking()
               .Where(a => a.ConferenceFormID == id)
               .OrderByDescending(a => a.Date).ToList();

            var model = new ConferenceFormVM
            {
                ConferenceForm = conferenceForm                
            };


            if (id == null)
            {
                return HttpNotFound();
            }           
            
            return View(model);
        }

        // GET: ConferenceForms/Create
        public ActionResult Create()
        {
            PopulateDropDownLists();
            return View();
        }

        // POST: ConferenceForms/Create
        // To protect from over-posting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ConferenceID,BenefitOfAttending,ReqReplacementStaff," +
           "FormStatusID,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] ConferenceForm conferenceForm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    conferenceForm.FormStatusID = db.FormStatus
                        .Where(f => f.StatusType == "Pending")
                        .SingleOrDefault().ID;

                    conferenceForm.StaffMemberID = db.Staff
                        .Where(s => s.Email == User.Identity.Name)
                        .SingleOrDefault().ID;

                    db.ConferenceForms.Add(conferenceForm);
                    db.SaveChanges();

                    return RedirectToAction("Details", new { id = conferenceForm.ID });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. " +
                    "Try again, and if the problem persists, see your system administrator.");
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the " +
                    "problem persists see your system administrator.");
            }

            PopulateDropDownLists(conferenceForm);
            return View(conferenceForm);
        }

        // GET: ConferenceForms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ConferenceForm conferenceForm = db.ConferenceForms.Find(id);

            if (conferenceForm == null)
            {
                return HttpNotFound();
            }

            PopulateDropDownLists(conferenceForm);
            if (User.IsInRole("Admin"))
            {
                FormStatusDropDown(conferenceForm);
            }
            return View(conferenceForm);
        }

        // POST: ConferenceForms/Edit/5
        // To protect from over-posting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, Byte[] rowVersion)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ConferenceForm conferenceFormToUpdate = db.ConferenceForms.Find(id);

            if (TryUpdateModel(conferenceFormToUpdate, "",
                new string[] { "ID","ConferenceID","BenefitOfAttending","ReqReplacementStaff",
                    "MileageFormID","FormStatusID"}))
            {
                try
                {
                    db.Entry(conferenceFormToUpdate).State = EntityState.Modified;
                    db.Entry(conferenceFormToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    db.SaveChanges();

                    return RedirectToAction("Details", new { id = conferenceFormToUpdate.ID });
                }
                catch (DbUpdateConcurrencyException ex)// Added for concurrency
                {
                    var entry = ex.Entries.Single();
                    var theValues = (ConferenceForm)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "Unable to save changes. The Conference Form was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (ConferenceForm)databaseEntry.ToObject();
                        if (databaseValues.ConferenceID != theValues.ConferenceID)
                            ModelState.AddModelError("ConferenceID", "Current value: "
                                + db.Conferences.Find(databaseValues.ConferenceID).Name);
                        if (databaseValues.BenefitOfAttending != theValues.BenefitOfAttending)
                            ModelState.AddModelError("BenefitOfAttending", "Current value: "
                                + databaseValues.BenefitOfAttending);
                        if (databaseValues.ReqReplacementStaff != theValues.ReqReplacementStaff)
                            ModelState.AddModelError("ReqReplacementStaff", "Current value: "
                                + databaseValues.ReqReplacementStaff);                       
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you received your values. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to save your version of this record, click "
                                + "the Save button again.");
                        conferenceFormToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            
            PopulateDropDownLists(conferenceFormToUpdate);
            if (User.IsInRole("Admin"))
            {
                FormStatusDropDown(conferenceFormToUpdate);
            }
            return View(conferenceFormToUpdate);
        }

        // GET: ConferenceForms/Delete/5
        public ActionResult Remove(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConferenceForm conferenceForm = db.ConferenceForms.Find(id);
            if (conferenceForm == null)
            {
                return HttpNotFound();
            }
            return View(conferenceForm);
        }

        // POST: ConferenceForms/Delete/5
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveConfirmed(int id)
        {
            ConferenceForm conferenceForm = db.ConferenceForms.Find(id);
            db.Entry(conferenceForm).State = EntityState.Modified;
            conferenceForm.IsRemoved = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private SelectList FormStatusSelectList(int? selectedID)
        {
            var formStatus = from f in db.FormStatus   
                              orderby f.StatusType
                              select f;

            return new SelectList(formStatus, "ID", "StatusType", selectedID);
        }

        private void FormStatusDropDown(ConferenceForm conferenceForm = null)
        {
            ViewBag.FormStatusID = FormStatusSelectList(conferenceForm?.FormStatusID);
        }

        private SelectList conferenceSelectList(int? selectedID)
        {
            var conferences = from c in db.Conferences
                              join a in db.Addresses
                              on c.AddressID equals a.ID
                              orderby c.Name
                              select c;
            return new SelectList(conferences, "ID", "ConferenceSummary", selectedID);
        }
        private void PopulateDropDownLists(ConferenceForm conferenceForm = null)
        {
            ViewBag.ConferenceID = conferenceSelectList(conferenceForm?.ConferenceID);

        }

        [HttpGet]
        public ActionResult GetConferences(int? id)
        {
            SelectList conferences  = conferenceSelectList(id);
            return Json(conferences, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public string GetAConferenceString(int? ID)
        {
            if (ID.HasValue)
            {
                Conference conference = db.Conferences
                    .Include(d => d.Address)
                    .Include(d => d.ConferenceDate)
                    .Include(d => d.Cost)
                    .Include(d => d.EndDate)
                    .Include(d => d.Description)
                    .Where(d => d.ID == ID)
                    .SingleOrDefault();
                return conference.Address + " - " +
                     conference.ConferenceDate + ", " +
                     conference.Cost + ", " +
                     conference.EndDate + ", " +
                     conference.Description;
            }
            return "No Doctor Found";
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
