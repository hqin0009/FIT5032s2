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
    public class CoursesController : Controller
    {
        private Databaseversion3Entities db = new Databaseversion3Entities();

        // GET: Courses
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var course = db.Course.Include(c => c.Teacher);
            return View(course.ToList());
        }
        
        [Authorize(Roles = "Teacher,Student,Admin")]
        public ActionResult Display(string option, string search, int? pageNumber, string sort)
        {
            ViewBag.SortByCourseName = string.IsNullOrEmpty(sort) ? "descending name" : "";
            ViewBag.SortByCredit = sort == "Credit" ? "descending Credit" : "Credit";
            var records = db.Course.AsQueryable();
            if (option == "CourseName")
            {

                records=db.Course.Where(x => x.CourseName.StartsWith(search) || search == null);
            }
            else if(option =="Credit")
            {
                records=db.Course.Where(x => x.Credits.ToString().StartsWith(search) || search == null);
            }
            else if(option == "LastName") {
                records=db.Course.Where(x => x.Teacher.LastName.StartsWith(search) || search == null);
            }
            switch (sort)
            {
                case "descending name":
                    records = records.OrderByDescending(s => s.CourseName);
                    break;
                case "Credit":
                    records = records.OrderBy(s => s.Credits);
                    break;
                case "descending Credit":
                    records = records.OrderByDescending(s => s.Credits);
                    break;
                default:
                    records = records.OrderBy(s => s.CourseName);
                    break;
            }
            return View(records.ToPagedList(pageNumber ?? 1, 10));

        }
        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Course.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.TeacherId = new SelectList(db.Teacher, "TeacherId", "FirstName");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "CourseId,CourseName,Credits,TeacherId")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Course.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TeacherId = new SelectList(db.Teacher, "TeacherId", "FirstName", course.TeacherId);
            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Course.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeacherId = new SelectList(db.Teacher, "TeacherId", "FirstName", course.TeacherId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "CourseId,CourseName,Credits,TeacherId")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeacherId = new SelectList(db.Teacher, "TeacherId", "FirstName", course.TeacherId);
            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Course.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Course.Find(id);
            db.Course.Remove(course);
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
