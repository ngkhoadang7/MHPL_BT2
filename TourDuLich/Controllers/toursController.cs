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
using Newtonsoft.Json;



namespace TourDuLich.Controllers
{
    public class toursController : Controller
    {
        private tour_dulichEntities db = new tour_dulichEntities();

        public class tour_chitiet
        {
            public int dd_id { get; set; }
            public int ct_thutu { get; set; }

        }

        public class tourViewModelDetail
        {
            public int dd_id { get; set; }
            public int ct_thutu { get; set; }
            public string dd_ten { get; set; }
            public string dd_mota { get; set; }
        }

        public class ViewModelIndex
        {
            public tour tour { get; set; }
            public string loai_ten { get; set; }
        }


        // GET: tours
        public ActionResult Index()
        {
            var query = (from t in db.tours
                         join tl in db.tour_loai
                             on t.loai_id equals tl.loai_id
                         select new { t = t, tl = tl }).ToList();

            var list = from q in query
                       select new ViewModelIndex()
                       {
                           tour = q.t,
                           loai_ten = q.tl.loai_ten
                       };

            return View(list.ToList());
        }

        // GET: tours/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour tour = db.tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }

            var loaiTour = (from lt in db.tour_loai
                           where lt.loai_id == tour.loai_id
                           select lt.loai_ten).Single();
            ViewBag.loaiTour = loaiTour;

            var tour_Chitiet = JsonConvert.DeserializeObject<List<tour_chitiet>>(tour.tour_chitiet);

            // Convert to Anonymous Type to use two Anonymous Types in Linq below
            var temp = tour_Chitiet.Select(s => new { s.dd_id, s.ct_thutu }).ToList();
            var diaDiem = db.tour_diadiem.Select(s => new { s.dd_id, s.dd_ten, s.dd_mota }).ToList();

            var query = (from dd in diaDiem
                         join ct in temp
                             on dd.dd_id equals ct.dd_id
                         orderby ct.ct_thutu
                         select new { dd = dd, ct = ct }).ToList();

            var list = from q in query
                       select new tourViewModelDetail()
                       {
                           dd_id = q.ct.dd_id,
                           ct_thutu = q.ct.ct_thutu,
                           dd_ten = q.dd.dd_ten,
                           dd_mota = q.dd.dd_mota
                       };

            ViewBag.tour_chitiet = list.ToList();

            return View(tour);
        }

        // GET: tours/Create
        public ActionResult Create(int? Error)
        {
            if(Error == 1)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng thêm địa điểm đến!");
            }
            
            var loaiTour = from lt in db.tour_loai
                        select lt;
            ViewBag.loaiTour = new SelectList(loaiTour, "loai_id", "loai_ten");

            var thanhPho = (from tp in db.tour_diadiem
                            select tp.dd_thanhpho).Distinct();
            ViewBag.thanhPho = new SelectList(thanhPho, "dd_thanhpho");

            ViewBag.diaDiem = new SelectList("");

            return View();
        }

        // POST: tours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "tour_id,tour_ten,tour_mota,loai_id")] tour tour)
        {
            String[] dd_id = Request.Form.GetValues("dd_id");
            String[] ct_thutu = Request.Form.GetValues("ct_thutu");

            if(dd_id == null) 
            {
                return RedirectToAction("Create", new { Error = 1 });
            }

            if (ModelState.IsValid)
            {
                List<tour_chitiet> chitietArray = new List<tour_chitiet>();
                for (int i = 0; i < dd_id.Length; i++)
                {
                    tour_chitiet ct = new tour_chitiet();
                    ct.dd_id = Int32.Parse(dd_id[i]);
                    ct.ct_thutu = Int32.Parse(ct_thutu[i]);
                    chitietArray.Add(ct);
                }
                var json = JsonConvert.SerializeObject(chitietArray);

                tour.tour_chitiet = json;
                db.tours.Add(tour);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Create");
        }

        // GET: tours/Edit/5
        public ActionResult Edit(int? id, int? Error)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour tour = db.tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }

            if (Error == 1)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng thêm địa điểm đến");
            }

            var loaiTour = from lt in db.tour_loai
                           select lt;
            ViewBag.loaiTour = new SelectList(loaiTour, "loai_id", "loai_ten");

            var thanhPho = (from tp in db.tour_diadiem
                            select tp.dd_thanhpho).Distinct();
            ViewBag.thanhPho = new SelectList(thanhPho, "dd_thanhpho");

            ViewBag.diaDiem = new SelectList("");

            var tour_Chitiet = JsonConvert.DeserializeObject<List<tour_chitiet>>(tour.tour_chitiet);

            // Convert to Anonymous Type to use two Anonymous Types in Linq below
            var temp = tour_Chitiet.Select(s => new { s.dd_id, s.ct_thutu }).ToList(); 
            var diaDiem = db.tour_diadiem.Select(s => new { s.dd_id, s.dd_ten, s.dd_mota }).ToList();

            var query = (from dd in diaDiem
                         join ct in temp
                             on dd.dd_id equals ct.dd_id
                         orderby ct.ct_thutu
                         select new { dd = dd, ct = ct }).ToList();

            var list = from q in query
                       select new tourViewModelDetail()
                       {
                           dd_id = q.ct.dd_id,
                           ct_thutu = q.ct.ct_thutu,
                           dd_ten = q.dd.dd_ten,
                           dd_mota = q.dd.dd_mota
                       };

            ViewBag.tour_chitiet = list.ToList();

            return View(tour);
        }

        // POST: tours/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "tour_id,tour_ten,tour_mota,loai_id")] tour tour)
        {
            String[] dd_id = Request.Form.GetValues("dd_id");
            String[] ct_thutu = Request.Form.GetValues("ct_thutu");

            if (dd_id == null)
            {
                return RedirectToAction("Create", new { Error = 1 });
            }

            if (ModelState.IsValid)
            {
                List<tour_chitiet> chitietArray = new List<tour_chitiet>();
                for (int i = 0; i < dd_id.Length; i++)
                {
                    tour_chitiet ct = new tour_chitiet();
                    ct.dd_id = Int32.Parse(dd_id[i]);
                    ct.ct_thutu = Int32.Parse(ct_thutu[i]);
                    chitietArray.Add(ct);
                }
                var json = JsonConvert.SerializeObject(chitietArray);

                tour.tour_chitiet = json;
                db.Entry(tour).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tour);
        }

        // GET: tours/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour tour = db.tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }

            var tenLoaiTour = (from tl in db.tour_loai
                              where tl.loai_id == tour.loai_id
                              select tl.loai_ten).First();

            ViewBag.tenLoaiTour = tenLoaiTour;

            return View(tour);
        }

        // POST: tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tour tour = db.tours.Find(id);
            db.tours.Remove(tour);

            var gia = db.tour_gia.Where(g => g.tour_id == id);
            db.tour_gia.RemoveRange(gia);

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
        public JsonResult GetAllLocationByCity(string city)
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

        [HttpPost]
        public JsonResult GetLocationById(string id)
        {
            try
            {
                var diaDiem = from dd in db.tour_diadiem
                              where dd.dd_id.ToString() == id
                              select dd;

                return Json(new JavaScriptSerializer().Serialize(diaDiem));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
