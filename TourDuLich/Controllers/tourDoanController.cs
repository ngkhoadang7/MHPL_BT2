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
using System.Globalization;

namespace TourDuLich.Controllers
{
    public class tourDoanController : Controller
    {
        private tour_dulichEntities db = new tour_dulichEntities();

        public class ViewModelIndex
        {
            public tour_doan tour_doan { get; set; }
            public string tour_ten { get; set; }
            public double gia_sotien { get; set; }
        }

        public class JSONGroup
        {
            public tour_doan tour_doan { get; set; }
            public tour_gia tour_gia { get; set; }
        }

        public static string formatMoneyToPrint(double num)
        {
            return num.ToString("#,#", new CultureInfo("es-ES"));
        }

        // GET: tourDoan
        public ActionResult Index()
        {
            var tour = from t in db.tours
                       orderby t.tour_id
                       select t;

            ViewBag.tours = new SelectList(tour, "tour_id", "tour_ten");

            var query = (from d in db.tour_doan
                            join t in db.tours 
                                on d.tour_id equals t.tour_id
                            join g in db.tour_gia 
                                on d.gia_id equals g.gia_id
                        select new { d = d, t = t, g = g}).ToList();

            var list = from q in query
                       select new ViewModelIndex()
                       {
                           tour_doan = q.d,
                           tour_ten = q.t.tour_ten,
                           gia_sotien = q.g.gia_sotien
                       };

            return View(list.ToList());
        }

        // GET: tourDoan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_doan tour_doan = db.tour_doan.Find(id);
            if (tour_doan == null)
            {
                return HttpNotFound();
            }

            var query = (from d in db.tour_doan
                         join t in db.tours
                             on d.tour_id equals t.tour_id
                         join g in db.tour_gia
                             on d.gia_id equals g.gia_id
                         where d.doan_id == id
                         select new ViewModelIndex()
                         { tour_doan = d, tour_ten = t.tour_ten, gia_sotien = g.gia_sotien }).First();

            return View(query);
        }

        // GET: tourDoan/Create
        public ActionResult Create(int? Error)
        {
            if (Error == 1)
            {
                ModelState.AddModelError(string.Empty, "Ngày đi phải trước hoặc bằng ngày về");
            }

            if (Error == 2)
            {
                ModelState.AddModelError(string.Empty, "Ngày đi hoặc ngày về của đoàn không phù hợp với ngày áp dụng giá tour");
            }

            var tours = from t in db.tours
                        select t;
            ViewBag.tours = new SelectList(tours, "tour_id", "tour_ten");

            ViewBag.prices = new SelectList("");

            return View();
        }

        // POST: tourDoan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "doan_id,tour_id,doan_name,doan_ngaydi,doan_ngayve,doan_chitietchuongtrinh,gia_id")] tour_doan tour_doan)
        {
            if (ModelState.IsValid)
            {
                var cost = (from c in db.tour_gia
                           where c.gia_id == tour_doan.gia_id
                           select c).Single();

                if (DateTime.Compare(tour_doan.doan_ngaydi, tour_doan.doan_ngayve) > 0) 
                {
                    return RedirectToAction("Create", new { Error = 1 });
                }

                if(DateTime.Compare(tour_doan.doan_ngaydi,cost.gia_tungay) < 0 || DateTime.Compare(tour_doan.doan_ngayve,cost.gia_denngay) > 0 )
                {
                    return RedirectToAction("Create", new { Error = 2 });
                }


                db.tour_doan.Add(tour_doan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tour_doan);
        }

        // GET: tourDoan/Edit/5
        public ActionResult Edit(int? id, int? Error)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_doan tour_doan = db.tour_doan.Find(id);
            if (tour_doan == null)
            {
                return HttpNotFound();
            }

            if (Error == 1)
            {
                ModelState.AddModelError(string.Empty, "Ngày đi phải trước hoặc bằng ngày về");
            }

            if (Error == 2)
            {
                ModelState.AddModelError(string.Empty, "Ngày đi hoặc ngày về của đoàn không phù hợp với ngày áp dụng giá tour");
            }

            var tours = from t in db.tours
                        select t;
            ViewBag.tours = new SelectList(tours, "tour_id", "tour_ten");

            var prices = from p in db.tour_gia
                         where p.tour_id == tour_doan.tour_id
                         select p;
            
            ViewBag.prices = new SelectList(prices,"gia_id","gia_sotien");

            return View(tour_doan);
        }

        // POST: tourDoan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "doan_id,tour_id,doan_name,doan_ngaydi,doan_ngayve,doan_chitietchuongtrinh,gia_id")] tour_doan tour_doan)
        {
            if (ModelState.IsValid)
            {
                var cost = (from c in db.tour_gia
                            where c.gia_id == tour_doan.gia_id
                            select c).Single();

                if (DateTime.Compare(tour_doan.doan_ngaydi, tour_doan.doan_ngayve) > 0)
                {
                    return RedirectToAction("Edit", new { id = tour_doan.doan_id , Error = 1 });
                }


                if (DateTime.Compare(tour_doan.doan_ngaydi, cost.gia_tungay) < 0 || DateTime.Compare(tour_doan.doan_ngayve, cost.gia_denngay) > 0)
                {
                    return RedirectToAction("Edit", new { id = tour_doan.doan_id, Error = 2 });
                }

                db.Entry(tour_doan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tour_doan);
        }

        // GET: tourDoan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_doan tour_doan = db.tour_doan.Find(id);
            if (tour_doan == null)
            {
                return HttpNotFound();
            }

            var query = (from d in db.tour_doan
                         join t in db.tours
                             on d.tour_id equals t.tour_id
                         join g in db.tour_gia
                             on d.gia_id equals g.gia_id
                         where d.doan_id == id
                         select new ViewModelIndex()
                         { tour_doan = d, tour_ten = t.tour_ten, gia_sotien = g.gia_sotien }).First();

            return View(query);
        }

        // POST: tourDoan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tour_doan tour_doan = db.tour_doan.Find(id);
            db.tour_doan.Remove(tour_doan);
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

        [HttpPost]
        public JsonResult GetAllGroupByTourID(int id)
        {
            try
            {
                var query = (from gr in db.tour_doan
                            join tg in db.tour_gia
                                on gr.gia_id equals tg.gia_id
                            where gr.tour_id == id
                            select new { gr = gr, tg = tg}).ToList();

                var list = from q in query
                           select new JSONGroup
                           {
                               tour_doan = q.gr,
                               tour_gia = q.tg
                           };



                return Json(new JavaScriptSerializer().Serialize(list));
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPost]
        public JsonResult GetAllPricesByTourID(int id)
        {
            try
            {
                var prices = from g in db.tour_gia
                             where g.tour_id == id
                             select g;

                return Json(new JavaScriptSerializer().Serialize(prices));
            }
            catch (Exception ex) { throw ex; }
        }

    }
}
