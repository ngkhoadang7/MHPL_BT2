using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TourDuLich.Models;
using System.Globalization;
using System.Web.Script.Serialization;


namespace TourDuLich.Controllers
{
    public class tourGiaController : Controller
    {
        private tour_dulichEntities db = new tour_dulichEntities();

        public class tourGiaModelIndex {
            public int gia_id { get; set; }
            public double gia_sotien { get; set; }
            public DateTime gia_tungay { get; set; }
            public DateTime gia_denngay { get; set; }
            public string tour_ten { get; set; }
        }

        // GET: tourGia
        public ActionResult Index()
        {
            var tour = from t in db.tours
                       orderby t.tour_id
                       select t;

            ViewBag.tours = new SelectList(tour, "tour_id", "tour_ten"); 

            return View(db.tour_gia.ToList());
        }

        [HttpPost]
        public JsonResult GetAllPricesByID(int id)
        {
            try
            {
                var query = from gg in db.tour_gia
                            where gg.tour_id == id
                            select gg;

                return Json(new JavaScriptSerializer().Serialize(query));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: tourGia/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_gia tour_gia = db.tour_gia.Find(id);
            if (tour_gia == null)
            {
                return HttpNotFound();
            }
            return View(tour_gia);
        }

        // GET: tourGia/Create
        public ActionResult Create(int? Error)
        {
            if (Error == 1)
            {
                ModelState.AddModelError(string.Empty, "Ngày áp dụng từ ngày phải trước đến ngày");
            }

            var loaiTour = from lt in db.tours
                           select lt;
            ViewBag.tours = new SelectList(loaiTour, "tour_id", "tour_ten");

            return View();
        }

        // POST: tourGia/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "gia_id,gia_sotien,gia_tungay,gia_denngay,tour_id")] tour_gia tour_gia)
        {
            if (ModelState.IsValid)
            {
                if (DateTime.Compare(tour_gia.gia_tungay, tour_gia.gia_denngay) > 0)
                {
                    return RedirectToAction("Create", new { Error = 1 });
                }

                db.tour_gia.Add(tour_gia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tour_gia);
        }

        // GET: tourGia/Edit/5
        public ActionResult Edit(int? id, int? Error)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_gia tour_gia = db.tour_gia.Find(id);
            if (tour_gia == null)
            {
                return HttpNotFound();
            }

            if (Error == 1)
            {
                ModelState.AddModelError(string.Empty, "Ngày áp dụng từ ngày phải trước đến ngày");
            }

            /*tour_gia.gia_tungay = DateTime.ParseExact(tour_gia.gia_tungay.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            tour_gia.gia_denngay = DateTime.ParseExact(tour_gia.gia_denngay.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture);*/


            var loaiTour = from lt in db.tours
                           select lt;
            ViewBag.tours = new SelectList(loaiTour, "tour_id", "tour_ten");

            return View(tour_gia);
        }

        // POST: tourGia/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "gia_id,gia_sotien,gia_tungay,gia_denngay,tour_id")] tour_gia tour_gia)
        {
            if (ModelState.IsValid)
            {
                if (DateTime.Compare(tour_gia.gia_tungay, tour_gia.gia_denngay) > 0)
                {
                    return RedirectToAction("Edit", new { id = tour_gia.gia_id, Error = 1 });
                }

                db.Entry(tour_gia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tour_gia);
        }

        // GET: tourGia/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_gia tour_gia = db.tour_gia.Find(id);
            if (tour_gia == null)
            {
                return HttpNotFound();
            }

            var tourTen = (from t in db.tours
                          where t.tour_id == tour_gia.tour_id
                          select t.tour_ten).First();

            ViewBag.gia = tour_gia.gia_sotien.ToString("#,#", new CultureInfo("es-ES"));
            ViewBag.tourTen = tourTen;
            return View(tour_gia);
        }

        // POST: tourGia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tour_gia tour_gia = db.tour_gia.Find(id);
            db.tour_gia.Remove(tour_gia);
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
