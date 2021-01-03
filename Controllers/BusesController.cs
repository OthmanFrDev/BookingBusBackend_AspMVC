using BookingBus.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BookingBus.Controllers
{
    public class BusesController : Controller
    {
        private BookingBusEntities db = new BookingBusEntities();

        // GET: Buses
        public ActionResult Index()
        {
            var buses = db.Buses.Include(b => b.Navette).Include(b => b.Societe);
            return View(buses.ToList());

        }

        // GET: Buses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bus bus = db.Buses.Find(id);
            if (bus == null)
            {
                return HttpNotFound();
            }
            return View(bus);
        }

        // GET: Buses/Create
        public ActionResult Create(int id)
        {
            ViewBag.id = id;
            ViewBag.id_navette = new SelectList(db.Navettes, "id_navette", "lieu_depart");
            ViewBag.navr = db.Navettes.ToList();
            ViewBag.id_societe = new SelectList(db.Societes, "id_utilisateur", "lieu");
            
            return View();
        }

        // POST: Buses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_bus,nom,nbr_place,climatiseur,tv,description,id_societe,id_navette")] Bus bus)
        {
            if (ModelState.IsValid)
            {
                db.Buses.Add(bus);
                db.SaveChanges();
                return RedirectToAction("Index","Societes");
            }

            ViewBag.id_navette = new SelectList(db.Navettes, "id_navette", "lieu_depart", bus.id_navette);
            ViewBag.id_societe = new SelectList(db.Societes, "id_utilisateur", "lieu", bus.id_societe);
            
            return View(bus);
        }

        // GET: Buses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bus bus = db.Buses.Find(id);
            if (bus == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_navette = new SelectList(db.Navettes, "id_navette", "lieu_depart", bus.id_navette);
            ViewBag.id_societe = new SelectList(db.Societes, "id_utilisateur", "lieu", bus.id_societe);
            return View(bus);
        }

        // POST: Buses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_bus,nom,nbr_place,climatiseur,tv,description,id_societe,id_navette")] Bus bus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_navette = new SelectList(db.Navettes, "id_navette", "lieu_depart", bus.id_navette);
            ViewBag.id_societe = new SelectList(db.Societes, "id_utilisateur", "lieu", bus.id_societe);
            return View(bus);
        }

        // GET: Buses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bus bus = db.Buses.Find(id);
            if (bus == null)
            {
                return HttpNotFound();
            }
            return View(bus);
        }

        // POST: Buses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bus bus = db.Buses.Find(id);
            db.Buses.Remove(bus);
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
