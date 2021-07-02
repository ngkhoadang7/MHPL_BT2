using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TourDuLich.Models;

namespace TourDuLich.Controllers
{
    public class tourLoaiController : Controller
    {
        private tour_dulichEntities db = new tour_dulichEntities();

        // GET: tourLoai
        public ActionResult Index()
        {
            return View(db.tour_loai.ToList());
        }

        // GET: tourLoai/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_loai tour_loai = db.tour_loai.Find(id);
            if (tour_loai == null)
            {
                return HttpNotFound();
            }
            return View(tour_loai);
        }

        // GET: tourLoai/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tourLoai/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "loai_id,loai_ten,loai_mota")] tour_loai tour_loai)
        {
            if (ModelState.IsValid)
            {
                db.tour_loai.Add(tour_loai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tour_loai);
        }

        // GET: tourLoai/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_loai tour_loai = db.tour_loai.Find(id);
            if (tour_loai == null)
            {
                return HttpNotFound();
            }
            return View(tour_loai);
        }

        // POST: tourLoai/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "loai_id,loai_ten,loai_mota")] tour_loai tour_loai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tour_loai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tour_loai);
        }

        // GET: tourLoai/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_loai tour_loai = db.tour_loai.Find(id);
            if (tour_loai == null)
            {
                return HttpNotFound();
            }
            return View(tour_loai);
        }

        // POST: tourLoai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tour_loai tour_loai = db.tour_loai.Find(id);
            db.tour_loai.Remove(tour_loai);
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
