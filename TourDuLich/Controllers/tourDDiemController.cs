using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TourDuLich.Models;
using System.Web.Script.Serialization;


namespace TourDuLich.Controllers
{
    public class tourDDiemController : Controller
    {
        private tour_dulichEntities db = new tour_dulichEntities();

        // GET: tourDDiem
        public ActionResult Index()
        {
            var thanhPho = (from tp in db.tour_diadiem
                            select tp.dd_thanhpho).Distinct();
            ViewBag.thanhPho = new SelectList(thanhPho, "dd_thanhpho");


            return View(db.tour_diadiem.ToList());
        }

        [HttpPost]
        public JsonResult GetAllLocationsByCity(string city)
        {
            try 
            {
                var diaDiem = from dd in db.tour_diadiem
                              where dd.dd_thanhpho == city
                              select dd;
                return Json(new JavaScriptSerializer().Serialize(diaDiem));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: tourDDiem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_diadiem tour_diadiem = db.tour_diadiem.Find(id);
            if (tour_diadiem == null)
            {
                return HttpNotFound();
            }
            return View(tour_diadiem);
        }

        // GET: tourDDiem/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tourDDiem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "dd_id,dd_thanhpho,dd_ten,dd_mota")] tour_diadiem tour_diadiem)
        {
            if (ModelState.IsValid)
            {
                db.tour_diadiem.Add(tour_diadiem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tour_diadiem);
        }

        // GET: tourDDiem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_diadiem tour_diadiem = db.tour_diadiem.Find(id);
            if (tour_diadiem == null)
            {
                return HttpNotFound();
            }
            return View(tour_diadiem);
        }

        // POST: tourDDiem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "dd_id,dd_thanhpho,dd_ten,dd_mota")] tour_diadiem tour_diadiem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tour_diadiem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tour_diadiem);
        }

        // GET: tourDDiem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_diadiem tour_diadiem = db.tour_diadiem.Find(id);
            if (tour_diadiem == null)
            {
                return HttpNotFound();
            }
            return View(tour_diadiem);
        }

        // POST: tourDDiem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tour_diadiem tour_diadiem = db.tour_diadiem.Find(id);
            db.tour_diadiem.Remove(tour_diadiem);
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
