using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Assignment_Final_version3.Models;
using PagedList;
namespace Assignment_Final_version3.Controllers
{
    public class TeachersController : Controller
    {
        private Databaseversion3Entities db = new Databaseversion3Entities();

        // GET: Teachers
        public ActionResult Index()
        {
            return View(db.Teacher.ToList());
        }
        
        [Authorize(Roles = "Teacher,Student,Admin")]
        public ActionResult Display(string option, string search, int? pageNumber, string sort)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sort) ? "name_desc" : "";
            ViewBag.DateSortParm = sort == "Date" ? "date_desc" : "Date";
            var teachers = db.Teacher.AsQueryable();
            if (option == "FirstName")
            {

                teachers=db.Teacher.Where(x => x.FirstName.StartsWith(search) || search == null);
            }
            else
            {
                teachers = db.Teacher.Where(x => x.LastName.StartsWith(search) || search == null);
            }

            switch (sort)
            {
                case "name_desc":
                    teachers = teachers.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    teachers = teachers.OrderBy(s => s.HireDate);
                    break;
                case "date_desc":
                    teachers = teachers.OrderByDescending(s => s.HireDate);
                    break;
                default:
                    teachers = teachers.OrderBy(s => s.LastName);
                    break;
            }
            return View(teachers.ToPagedList(pageNumber ?? 1, 10));
        }

        // GET: Teachers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teacher.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // GET: Teachers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TeacherId,FirstName,LastName,HireDate")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Teacher.Add(teacher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teacher.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeacherId,FirstName,LastName,HireDate")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teacher.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Teacher teacher = db.Teacher.Find(id);
            db.Teacher.Remove(teacher);
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
