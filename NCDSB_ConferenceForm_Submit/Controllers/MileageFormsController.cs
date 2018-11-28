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
    public class MileageFormsController : Controller
    {
        private ConferenceFormEntities db = new ConferenceFormEntities();

        // GET: MileageForms
        public ActionResult Index(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSort = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.ForStatusSort = sortOrder == "Status" ? "status_desc" : "Status";

            var mileageForms = db.MileageForms.AsNoTracking()
                .Where(m => m.StaffMember.Email == User.Identity.Name)
                .Where(m => m.IsRemoved == false)
                .Include(m => m.FormStatus);

            switch (sortOrder)
            {
                case "date_desc":
                    mileageForms = mileageForms.OrderByDescending(s => s.Date);
                    break;
                case "Status":
                    mileageForms = mileageForms.OrderBy(s => s.FormStatus.StatusType);
                    break;
                case "status_desc":
                    mileageForms = mileageForms.OrderByDescending(s => s.FormStatus.StatusType);
                    break;                
                default:
                    mileageForms = mileageForms.OrderBy(s => s.Date);
                    break;
            }

            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(mileageForms.ToPagedList(pageNumber, pageSize));
        }

        // GET: MileageForms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MileageForm mileageForm = db.MileageForms.Find(id);

            ViewBag.Trips = db.Trips.AsNoTracking()
                .Where(a => a.MileageFormID == id)
                .OrderByDescending(a => a.Date).ToList();

            var model = new MileageFormViewModel();
            model.MileageForm = mileageForm;

            if (mileageForm == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: MileageForms/Create
        public ActionResult Create()
        {
            MileageForm mileageForm = new MileageForm {
                Date = DateTime.Now,
                StaffMemberID = db.Staff.Where(s => s.Email == User.Identity.Name).SingleOrDefault().ID,
                FormStatusID = db.FormStatus.Where(f => f.StatusType == "Pending").SingleOrDefault().ID
            };
            db.MileageForms.Add(mileageForm);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = mileageForm.ID });
        }

        // POST: MileageForms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,Date,FormStatusID,StaffMemberID,ConferenceFormID,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] MileageForm mileageForm)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            mileageForm.StaffMemberID = db.Staff.Where(s => s.Email == User.Identity.Name).SingleOrDefault().ID;
        //            mileageForm.FormStatusID = db.FormStatus.Where(f => f.StatusType == "Pending").SingleOrDefault().ID;
        //            db.MileageForms.Add(mileageForm);
        //            db.SaveChanges();
        //            return RedirectToAction("Details", new { id = mileageForm.ID }); 
        //        }
        //    }
        //    catch (RetryLimitExceededException)
        //    {
        //        ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
        //    }
        //    catch (DataException)
        //    {
        //        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
        //    }

        //    return View(mileageForm);
        //}

        // GET: MileageForms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MileageForm mileageForm = db.MileageForms.Find(id);
            if (mileageForm == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole("Admin"))
            {
                FormStatusDropDown(mileageForm);
            }
            return View(mileageForm);
        }

        // POST: MileageForms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, Byte[] rowVersion)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var mileageFormToUpdate = db.MileageForms.Find(id);

            if (TryUpdateModel(mileageFormToUpdate, "",
                new string[] { "ID", "Date", "FormStatusID" }))
            {
                try
                {
                    db.Entry(mileageFormToUpdate).State = EntityState.Modified;
                    db.Entry(mileageFormToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)// Added for concurrency
                {
                    var entry = ex.Entries.Single();
                    var theValues = (MileageForm)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "Unable to save changes. The Conference was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (MileageForm)databaseEntry.ToObject();
                        if (databaseValues.Date != theValues.Date)
                            ModelState.AddModelError("Date", "Current value: "
                                + databaseValues.Date);
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you received your values. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to save your version of this record, click "
                                + "the Save button again.");
                        mileageFormToUpdate.RowVersion = databaseValues.RowVersion;
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
            if (User.IsInRole("Admin"))
            {
                FormStatusDropDown(mileageFormToUpdate);
            }
            return View(mileageFormToUpdate);
        }       

        // GET: MileageForms/Delete/5
        public ActionResult Remove(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MileageForm mileageForm = db.MileageForms.Find(id);
            if (mileageForm == null)
            {
                return HttpNotFound();
            }
            return View(mileageForm);
        }

        // POST: MileageForms/Delete/5
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveConfirmed(int id)
        {
            MileageForm mileageForm = db.MileageForms.Find(id);
            db.Entry(mileageForm).State = EntityState.Modified;
            mileageForm.IsRemoved = true;
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

        private void FormStatusDropDown(MileageForm mileageForm = null)
        {
            ViewBag.FormStatusID = FormStatusSelectList(mileageForm?.FormStatusID);
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
