using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TourDuLich.Models;
using Newtonsoft.Json;

namespace TourDuLich.Controllers
{
    public class tourNDiController : Controller
    {
        private tour_dulichEntities db = new tour_dulichEntities();

        public class ViewModelIndex
        {
            public tour_nguoidi tour_nguoidi { get; set; }
            public string doan_name { get; set; }
            public int soNV { get; set; }
            public int soKH { get; set; }
        }

        // GET: tourNDi
        public ActionResult Index()
        {
            var query = (from nd in db.tour_nguoidi
                         join td in db.tour_doan
                             on nd.doan_id equals td.doan_id
                         select new { nd = nd, td = td }).ToList();

            var list = from q in query
                       select new ViewModelIndex()
                       {
                           tour_nguoidi = q.nd,
                           doan_name = q.td.doan_name,
                           soNV = JsonConvert.DeserializeObject<List<int>>(q.nd.nguoidi_dsnhanvien).Count,
                           soKH = JsonConvert.DeserializeObject<List<int>>(q.nd.nguoidi_dskhach).Count
                       };

            return View(list.ToList());
        }

        // GET: tourNDi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_nguoidi tour_nguoidi = db.tour_nguoidi.Find(id);
            if (tour_nguoidi == null)
            {
                return HttpNotFound();
            }

            List<int> ds_kh = JsonConvert.DeserializeObject<List<int>>(tour_nguoidi.nguoidi_dskhach);
            List<int> ds_nv = JsonConvert.DeserializeObject<List<int>>(tour_nguoidi.nguoidi_dsnhanvien);
            

            var tenDoan = (from td in db.tour_doan
                           where td.doan_id == tour_nguoidi.doan_id
                         select td.doan_name).Single();

            var model = new ViewModelIndex()
                       {
                           tour_nguoidi = tour_nguoidi,
                           doan_name = tenDoan,
                           soNV = ds_nv.Count,
                           soKH = ds_kh.Count
                       };

            var customers = db.tour_khachhang.Where(cs => ds_kh.Contains(cs.kh_id));
            var staffs = db.tour_nhanvien.Where(sa => ds_nv.Contains(sa.nv_id));

            ViewBag.customers = customers;
            ViewBag.staffs = staffs;

            return View(model);
        }

        // GET: tourNDi/Create
        public ActionResult Create(int? Error)
        {
            if (Error == 1)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng thêm nhân viên!");
            }

            if (Error == 2)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng thêm khách hàng!");
            }

            var doan = from dd in db.tour_doan
                       where !(from nd in db.tour_nguoidi
                              select nd.doan_id).Contains(dd.doan_id)
                       select dd;

            ViewBag.doan = new SelectList(doan, "doan_id", "doan_name");

            return View();
        }

        // POST: tourNDi/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "nguoidi_id,doan_id,nguoidi_dsnhanvien,nguoidi_dskhach")] tour_nguoidi tour_nguoidi)
        {
            String[] nv_id = Request.Form.GetValues("nv_id");
            String[] kh_id = Request.Form.GetValues("kh_id");

            if (nv_id == null)
            {
                return RedirectToAction("Create", new { Error = 1 });
            }

            if (kh_id == null)
            {
                return RedirectToAction("Create", new { Error = 2 });
            }

            List<int> nv_ids = new List<int>();
            for (int i = 0; i < nv_id.Length; i++)
            {
                nv_ids.Add(Int32.Parse(nv_id[i]));
            }
            var json = JsonConvert.SerializeObject(nv_ids);

            tour_nguoidi.nguoidi_dsnhanvien = json;

            List<int> kh_ids = new List<int>();
            for (int i = 0; i < kh_id.Length; i++)
            {
                kh_ids.Add(Int32.Parse(kh_id[i]));
            }
            json = JsonConvert.SerializeObject(kh_ids);

            tour_nguoidi.nguoidi_dskhach = json;

            if (ModelState.IsValid)
            {
                db.tour_nguoidi.Add(tour_nguoidi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Create");
        }

        // GET: tourNDi/Edit/5
        public ActionResult Edit(int? id, int? Error)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_nguoidi tour_nguoidi = db.tour_nguoidi.Find(id);
            if (tour_nguoidi == null)
            {
                return HttpNotFound();
            }

            if (Error == 1)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng thêm nhân viên!");
            }

            if (Error == 2)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng thêm khách hàng!");
            }

            var doan = (from dd in db.tour_doan
                       where !(from nd in db.tour_nguoidi
                               select nd.doan_id).Contains(dd.doan_id)
                       select dd).Union(from dd in db.tour_doan
                                        where dd.doan_id == tour_nguoidi.doan_id
                                        select dd);

            ViewBag.doan = new SelectList(doan, "doan_id", "doan_name");

            List<int> ds_kh = JsonConvert.DeserializeObject<List<int>>(tour_nguoidi.nguoidi_dskhach);
            List<int> ds_nv = JsonConvert.DeserializeObject<List<int>>(tour_nguoidi.nguoidi_dsnhanvien);

            var customers = db.tour_khachhang.Where(cs => ds_kh.Contains(cs.kh_id));
            var staffs = db.tour_nhanvien.Where(sa => ds_nv.Contains(sa.nv_id));

            ViewBag.customers = customers;
            ViewBag.staffs = staffs;
            ViewBag.id_customers = string.Join(",", ds_kh);
            ViewBag.id_staffs = string.Join(",", ds_nv);

            return View(tour_nguoidi);
        }

        // POST: tourNDi/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "nguoidi_id,doan_id,nguoidi_dsnhanvien,nguoidi_dskhach")] tour_nguoidi tour_nguoidi)
        {
            String[] nv_id = Request.Form.GetValues("nv_id");
            String[] kh_id = Request.Form.GetValues("kh_id");

            if (nv_id == null)
            {
                return RedirectToAction("Edit", new { Error = 1 });
            }

            if (kh_id == null)
            {
                return RedirectToAction("Edit", new { Error = 2 });
            }

            List<int> nv_ids = new List<int>();
            for (int i = 0; i < nv_id.Length; i++)
            {
                nv_ids.Add(Int32.Parse(nv_id[i]));
            }
            var json = JsonConvert.SerializeObject(nv_ids);

            tour_nguoidi.nguoidi_dsnhanvien = json;

            List<int> kh_ids = new List<int>();
            for (int i = 0; i < kh_id.Length; i++)
            {
                kh_ids.Add(Int32.Parse(kh_id[i]));
            }
            json = JsonConvert.SerializeObject(kh_ids);

            tour_nguoidi.nguoidi_dskhach = json;

            if (ModelState.IsValid)
            {
                db.Entry(tour_nguoidi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tour_nguoidi);
        }

        // GET: tourNDi/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tour_nguoidi tour_nguoidi = db.tour_nguoidi.Find(id);
            if (tour_nguoidi == null)
            {
                return HttpNotFound();
            }

            List<int> ds_kh = JsonConvert.DeserializeObject<List<int>>(tour_nguoidi.nguoidi_dskhach);
            List<int> ds_nv = JsonConvert.DeserializeObject<List<int>>(tour_nguoidi.nguoidi_dsnhanvien);

            var tenDoan = (from td in db.tour_doan
                           where td.doan_id == tour_nguoidi.doan_id
                           select td.doan_name).Single();

            ViewBag.doan = tenDoan;
            ViewBag.kh = ds_kh.Count;
            ViewBag.nv = ds_nv.Count;

            return View(tour_nguoidi);
        }

        // POST: tourNDi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tour_nguoidi tour_nguoidi = db.tour_nguoidi.Find(id);
            db.tour_nguoidi.Remove(tour_nguoidi);
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
        /* =========================== Staff  AJAX Function ========================================*/
        [HttpPost]
        public JsonResult getStaffPagination(int currentPage, int itemsInPage)
        {
            try
            {
                var nhanVien = (from st in db.tour_nhanvien
                                orderby st.nv_id
                              select st).Skip((currentPage - 1) * itemsInPage).Take(itemsInPage);

                return Json(new JavaScriptSerializer().Serialize(nhanVien));
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult countAllStaff()
        {
            try
            {
                var nhanVien = (from nv in db.tour_nhanvien
                                select nv).Count();

                return Json(new JavaScriptSerializer().Serialize(nhanVien));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult getStaffsByIds(List<int> rootStaffs)
        {
            try
            {
                var nhanVien = db.tour_nhanvien
                               .Where(t => rootStaffs.Contains(t.nv_id));


                return Json(new JavaScriptSerializer().Serialize(nhanVien));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /* ===========================================================================================*/
        /* =========================== Customer AJAX Function ========================================*/
        [HttpPost]
        public JsonResult getCustomerPagination(int currentPage, int itemsInPage)
        {
            try
            {
                var khachHang = (from cs in db.tour_khachhang
                                    orderby cs.kh_id
                                 select cs).Skip((currentPage-1)* itemsInPage).Take(itemsInPage);

                return Json(new JavaScriptSerializer().Serialize(khachHang));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult countAllCustomer()
        {
            try
            {
                var khachHang = (from cs in db.tour_khachhang
                                 select cs).Count();

                return Json(new JavaScriptSerializer().Serialize(khachHang));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult getCustomersByIds(List<int> rootCustomers)
        {
            try
            {
                var khachHang = db.tour_khachhang
                               .Where(t => rootCustomers.Contains(t.kh_id));


                return Json(new JavaScriptSerializer().Serialize(khachHang));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /* ===========================================================================================*/
    }
}
