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
    public class tourNVienController : Controller
    {
        private tour_dulichEntities db = new tour_dulichEntities();

        public static string formatDateToPrint(string date)
        {
            DateTime oDate = DateTime.ParseExact(date, "yyyy-MM-dd", null);
            return oDate.ToString("dd'/'MM'/'yyyy");
        }

        // GET: tourNVien
        public ActionResult Index()
        {
            return View(db.tour_nhanvien.ToList());
        }

        // GET: tourNVien/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_nhanvien tour_nhanvien = db.tour_nhanvien.Find(id);
            if (tour_nhanvien == null)
            {
                return HttpNotFound();
            }
            return View(tour_nhanvien);
        }

        // GET: tourNVien/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tourNVien/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "nv_id,nv_ten,nv_sdt,nv_ngaysinh,nv_email,nv_nhiemvu")] tour_nhanvien tour_nhanvien)
        {
            if (ModelState.IsValid)
            {
                db.tour_nhanvien.Add(tour_nhanvien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tour_nhanvien);
        }

        // GET: tourNVien/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_nhanvien tour_nhanvien = db.tour_nhanvien.Find(id);
            if (tour_nhanvien == null)
            {
                return HttpNotFound();
            }
            return View(tour_nhanvien);
        }

        // POST: tourNVien/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "nv_id,nv_ten,nv_sdt,nv_ngaysinh,nv_email,nv_nhiemvu")] tour_nhanvien tour_nhanvien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tour_nhanvien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tour_nhanvien);
        }

        // GET: tourNVien/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_nhanvien tour_nhanvien = db.tour_nhanvien.Find(id);
            if (tour_nhanvien == null)
            {
                return HttpNotFound();
            }
            return View(tour_nhanvien);
        }

        // POST: tourNVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tour_nhanvien tour_nhanvien = db.tour_nhanvien.Find(id);
            db.tour_nhanvien.Remove(tour_nhanvien);
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
