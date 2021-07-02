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
using System.Globalization;

namespace TourDuLich.Controllers
{
    public class tourCPhiController : Controller
    {
        private tour_dulichEntities db = new tour_dulichEntities();

        public class ViewModelIndex
        {
            public string doan_name { get; set; }
            public tour_chiphi tour_chiphi { get; set; }
        }

        public class chiphi_chitiet
        {
            public string hoaDon { get; set; }
            public int loaiChiPhi { get; set; }
            public string noiDung { get; set; }
            public string ngay { get; set; }
            public float soTien { get; set; }
        }

        public class chiphi_chitietView
        {
            public chiphi_chitiet chiphi_chitiet { get; set; }
            public string loaiChiPhiText { get; set; }
        }

        public static string formatMoneyToPrint(double num)
        {
            return num.ToString("#,#", new CultureInfo("es-ES"));
        }


        public static string formatDateToPrint(string date)
        {
            DateTime oDate = DateTime.ParseExact(date, "yyyy-MM-dd", null);
            return oDate.ToString("dd'/'MM'/'yyyy");
        }

        // GET: tourCPhi
        public ActionResult Index()
        {
            var query = (from tcp in db.tour_chiphi
                        join d in db.tour_doan 
                            on tcp.doan_id equals d.doan_id
                        select new {tcp=tcp, d=d}).ToList();

            var list = from q in query
                       select new ViewModelIndex()
                       {
                           doan_name = q.d.doan_name,
                           tour_chiphi = q.tcp
                       };

            return View(list.ToList());
        }

        // GET: tourCPhi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_chiphi tour_chiphi = db.tour_chiphi.Find(id);
            if (tour_chiphi == null)
            {
                return HttpNotFound();
            }

            var doan = (from d in db.tour_doan
                        where d.doan_id == tour_chiphi.doan_id
                        select d.doan_name).Single();
            ViewBag.doan = doan;

            var chiphi_chitiet = JsonConvert.DeserializeObject<List<chiphi_chitiet>>(tour_chiphi.chiphi_chitiet);

            var loaiChiPhi = db.tour_loaichiphi.Select(s => new { s.cp_id, s.cp_ten }).ToList();

            var query = (from ct in chiphi_chitiet
                         join lcp in loaiChiPhi
                             on ct.loaiChiPhi equals lcp.cp_id
                         select new { ct = ct, lcp = lcp }).ToList();

            var list = from q in query
                       select new chiphi_chitietView()
                       {
                           chiphi_chitiet = q.ct,
                           loaiChiPhiText = q.lcp.cp_ten
                       };

            ViewBag.chiPhiChiTiet = list.ToList();

            return View(tour_chiphi);
        }

        // GET: tourCPhi/Create
        public ActionResult Create(int? Error)
        {
            if (Error == 1)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng thêm chi tiết chi phí");
            }

            var loaiChiPhi = from lcp in db.tour_loaichiphi
                             select lcp;

            ViewBag.loaiChiPhi = new SelectList(loaiChiPhi, "cp_id", "cp_ten");

            var doan = from d in db.tour_doan
                       where !((from tcp in db.tour_chiphi
                               select tcp.doan_id).Contains(d.doan_id))
                       select d;

            ViewBag.doan = new SelectList(doan, "doan_id", "doan_name");

            return View();
        }

        // POST: tourCPhi/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "chiphi_id,doan_id,chiphi_total,chiphi_chitiet")] tour_chiphi tour_chiphi)
        {
            String[] hoaDon = Request.Form.GetValues("hoaDon");
            String[] loaiChiPhi = Request.Form.GetValues("loaiChiPhi");
            String[] noiDung = Request.Form.GetValues("noiDung");
            String[] ngay = Request.Form.GetValues("ngay");
            String[] soTien = Request.Form.GetValues("soTien");

            if (hoaDon == null)
            {
                return RedirectToAction("Create", new { Error = 1 });
            }

            List<chiphi_chitiet> chitietArray = new List<chiphi_chitiet>();
            for (int i = 0; i < hoaDon.Length; i++)
            {
                chiphi_chitiet ct = new chiphi_chitiet();
                ct.hoaDon = hoaDon[i];
                ct.loaiChiPhi = int.Parse(loaiChiPhi[i]);
                ct.noiDung = noiDung[i];
                ct.ngay = ngay[i];
                ct.soTien = float.Parse(soTien[i]);
                chitietArray.Add(ct);
            }

            var json = JsonConvert.SerializeObject(chitietArray);

            tour_chiphi.chiphi_chitiet = json;

            if (ModelState.IsValid)
            {
                db.tour_chiphi.Add(tour_chiphi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tour_chiphi);
        }

        // GET: tourCPhi/Edit/5
        public ActionResult Edit(int? id, int? Error)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_chiphi tour_chiphi = db.tour_chiphi.Find(id);
            if (tour_chiphi == null)
            {
                return HttpNotFound();
            }
            if (Error == 1)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng thêm chi tiết chi phí");
            }

            var loaiChiPhi = from lcp in db.tour_loaichiphi
                             select lcp;

            ViewBag.loaiChiPhi = new SelectList(loaiChiPhi, "cp_id", "cp_ten");

            var doan = (from d in db.tour_doan
                       where !((from tcp in db.tour_chiphi
                                select tcp.doan_id).Contains(d.doan_id))
                       select d).Union(from dd in db.tour_doan
                                       where dd.doan_id == tour_chiphi.doan_id
                                       select dd);

            ViewBag.doan = new SelectList(doan, "doan_id", "doan_name");

            var chiphi_chitiet = JsonConvert.DeserializeObject<List<chiphi_chitiet>>(tour_chiphi.chiphi_chitiet);

            var loaiChiPhiNew = db.tour_loaichiphi.Select(s => new { s.cp_id, s.cp_ten }).ToList();

            var query = (from ct in chiphi_chitiet
                         join lcp in loaiChiPhiNew
                             on ct.loaiChiPhi equals lcp.cp_id
                         select new { ct = ct, lcp = lcp }).ToList();

            var list = from q in query
                       select new chiphi_chitietView()
                       {
                           chiphi_chitiet = q.ct,
                           loaiChiPhiText = q.lcp.cp_ten
                       };

            ViewBag.chiPhiChiTiet = list.ToList();

            return View(tour_chiphi);
        }

        // POST: tourCPhi/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "chiphi_id,doan_id,chiphi_total,chiphi_chitiet")] tour_chiphi tour_chiphi)
        {
            String[] hoaDon = Request.Form.GetValues("hoaDon");
            String[] loaiChiPhi = Request.Form.GetValues("loaiChiPhi");
            String[] noiDung = Request.Form.GetValues("noiDung");
            String[] ngay = Request.Form.GetValues("ngay");
            String[] soTien = Request.Form.GetValues("soTien");

            if (hoaDon == null)
            {
                return RedirectToAction("Edit", new { id = tour_chiphi.chiphi_id, Error = 1 });
            }

            List<chiphi_chitiet> chitietArray = new List<chiphi_chitiet>();
            for (int i = 0; i < hoaDon.Length; i++)
            {
                chiphi_chitiet ct = new chiphi_chitiet();
                ct.hoaDon = hoaDon[i];
                ct.loaiChiPhi = int.Parse(loaiChiPhi[i]);
                ct.noiDung = noiDung[i];
                ct.ngay = ngay[i];
                ct.soTien = float.Parse(soTien[i]);
                chitietArray.Add(ct);
            }

            var json = JsonConvert.SerializeObject(chitietArray);

            tour_chiphi.chiphi_chitiet = json;

            if (ModelState.IsValid)
            {
                db.Entry(tour_chiphi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tour_chiphi);
        }

        // GET: tourCPhi/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_chiphi tour_chiphi = db.tour_chiphi.Find(id);
            if (tour_chiphi == null)
            {
                return HttpNotFound();
            }
            var doan = (from d in db.tour_doan
                        where d.doan_id == tour_chiphi.doan_id
                        select d.doan_name).Single();
            ViewBag.doan = doan;

            return View(tour_chiphi);
        }

        // POST: tourCPhi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tour_chiphi tour_chiphi = db.tour_chiphi.Find(id);
            db.tour_chiphi.Remove(tour_chiphi);
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
