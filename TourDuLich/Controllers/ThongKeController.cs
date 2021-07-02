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
    public class ThongKeController : Controller
    {
        private tour_dulichEntities db = new tour_dulichEntities();

        public class ViewModelIndex 
        {
            public int tourID { get; set; }
            public string tourName { get; set; }
            public int numOfGroups { get; set; }
            public float revenue { get; set; }
            public float expenses { get; set; }
            public float profit { get; set; }
        }

        public class ViewModelDetail
        {
            public string groupName { get; set; }
            public int numOfCustomers { get; set; }
            public float cost { get; set; }
            public float revenue { get; set; }
            public float expenses { get; set; }
            public int expensesID { get; set; }
            public float profit { get; set; }
        }

        public static string formatMoneyToPrint(decimal num)
        {
            return num.ToString("#,#", new CultureInfo("es-ES"));
        }

        // GET: ThongKe
        public ActionResult Index()
        {
            var tours = (from t in db.tours
                       select t).ToList();

            List<ViewModelIndex> listViewModel = new List<ViewModelIndex>();

            foreach(var tour in tours) 
            {
                ViewModelIndex model = new ViewModelIndex();
                model.tourID = tour.tour_id;
                model.tourName = tour.tour_ten;
                var groups = (from g in db.tour_doan
                             where g.tour_id == tour.tour_id
                             select g).ToList();

                model.numOfGroups = groups.Count;
                model.revenue = 0;
                model.expenses = 0;
                foreach(var grouP in groups)
                {
                    var participants = (from p in db.tour_nguoidi
                                        where p.doan_id == grouP.doan_id
                                        select p.nguoidi_dskhach).SingleOrDefault();
                    List<int> listParticipants;
                    if (participants == null)
                    {
                        listParticipants = new List<int>();
                    }
                    else
                    {
                        listParticipants = JsonConvert.DeserializeObject<List<int>>(participants);
                    }
                    

                    var cost = (from c in db.tour_gia
                                where c.gia_id == grouP.gia_id
                                select c.gia_sotien).SingleOrDefault();

                    model.revenue += (float) cost * listParticipants.Count;

                    var temp = (from e in db.tour_chiphi
                                where e.doan_id == grouP.doan_id
                                select e.chiphi_total).SingleOrDefault();

                    model.expenses += (float) temp; 
                }
                model.profit = model.revenue - model.expenses;
                listViewModel.Add(model);
            }

            return View(listViewModel);
        }

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
            ViewBag.tour = tour.tour_ten;

            var groups = (from g in db.tour_doan
                                       where g.tour_id == tour.tour_id
                                       select g).ToList();
            float totalRevenue = 0;
            float totalExpenses = 0;
            List<ViewModelDetail> listViewModel = new List<ViewModelDetail>();

            foreach (var grouP in groups)
            {
                ViewModelDetail model = new ViewModelDetail();

                model.groupName = grouP.doan_name;

                var participants = (from p in db.tour_nguoidi
                                    where p.doan_id == grouP.doan_id
                                    select p.nguoidi_dskhach).SingleOrDefault();
                List<int> listParticipants;
                if (participants == null)
                {
                    listParticipants = new List<int>();
                }
                else
                {
                    listParticipants = JsonConvert.DeserializeObject<List<int>>(participants);
                }

                model.numOfCustomers = listParticipants.Count;

                model.cost = (float) (from c in db.tour_gia
                            where c.gia_id == grouP.gia_id
                            select c.gia_sotien).SingleOrDefault();

                model.revenue = model.cost * listParticipants.Count;
                totalRevenue += model.revenue;

                var temp = (from e in db.tour_chiphi
                            where e.doan_id == grouP.doan_id
                            select e).SingleOrDefault();
                if(temp == null)
                {
                    model.expenses = 0;
                    model.expensesID = 0;
                } 
                else
                {
                    model.expenses = (float)temp.chiphi_total;
                    model.expensesID = temp.chiphi_id;
                }
                totalExpenses += model.expenses;

                model.profit = model.revenue - model.expenses;

                listViewModel.Add(model);
            }
            ViewBag.totalExpenses = totalExpenses;
            ViewBag.totalRevenue = totalRevenue;
            ViewBag.totalProfit = totalRevenue - totalExpenses;


            return View(listViewModel);
        }
    }
}