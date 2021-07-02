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
    public class tourLCPhiController : Controller
    {
        private tour_dulichEntities db = new tour_dulichEntities();

        // GET: tourLCPhi
        public ActionResult Index()
        {
            return View(db.tour_loaichiphi.ToList());
        }

        // GET: tourLCPhi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_loaichiphi tour_loaichiphi = db.tour_loaichiphi.Find(id);
            if (tour_loaichiphi == null)
            {
                return HttpNotFound();
            }
            return View(tour_loaichiphi);
        }

        // GET: tourLCPhi/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tourLCPhi/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cp_id,cp_ten,cp_mota")] tour_loaichiphi tour_loaichiphi)
        {
            if (ModelState.IsValid)
            {
                db.tour_loaichiphi.Add(tour_loaichiphi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tour_loaichiphi);
        }

        // GET: tourLCPhi/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_loaichiphi tour_loaichiphi = db.tour_loaichiphi.Find(id);
            if (tour_loaichiphi == null)
            {
                return HttpNotFound();
            }
            return View(tour_loaichiphi);
        }

        // POST: tourLCPhi/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cp_id,cp_ten,cp_mota")] tour_loaichiphi tour_loaichiphi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tour_loaichiphi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tour_loaichiphi);
        }

        // GET: tourLCPhi/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_loaichiphi tour_loaichiphi = db.tour_loaichiphi.Find(id);
            if (tour_loaichiphi == null)
            {
                return HttpNotFound();
            }
            return View(tour_loaichiphi);
        }

        // POST: tourLCPhi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tour_loaichiphi tour_loaichiphi = db.tour_loaichiphi.Find(id);
            db.tour_loaichiphi.Remove(tour_loaichiphi);
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
