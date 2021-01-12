using BookingBus.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BookingBus.Controllers
{
    public class BusesController : Controller
    {

        private BookingBusEntities db = new BookingBusEntities();

        // GET: Buses
        public ActionResult Index(int id)
        {
            if (Session["UserID"] != null)
            {
                int ids = int.Parse((Session["UserID"].ToString()));
                ViewBag.id = ids;
                var buses = db.Buses.Include(b => b.Navette).Include(b => b.Societe).Where(b => b.id_societe == id);

                return View(buses.ToList());
            }
            else if (Session["UserID"] == null) { return RedirectToAction("login", "Home"); }
            return RedirectToAction("Index", "Home");

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
        public ActionResult Create([Bind(Include = "id_bus,nom,nbr_place,climatiseur,tv,description,id_societe,id_navette,image")] Bus bus, HttpPostedFileBase imagefile)
        {
            if (Session["UserID"] != null)
            {
                int ids = int.Parse((Session["UserID"].ToString()));
                if (ModelState.IsValid)
                {
                    if (imagefile != null)
                    {
                        string namePic = Path.GetFileNameWithoutExtension(imagefile.FileName);
                        string ext = Path.GetExtension(imagefile.FileName);
                        namePic += System.DateTime.Now.ToString("yymmssfff") + ext;
                        string path = Path.Combine(Server.MapPath("~/Content/"), namePic);
                        bus.image = namePic;
                        imagefile.SaveAs(path);
                    }
                    else { bus.image = "defaultbus.png"; }
                    bus.nbr_place = (int)bus.nbr_place;
                    db.Buses.Add(bus);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = ids });
                }
                ViewBag.id_navette = new SelectList(db.Navettes, "id_navette", "lieu_depart", bus.id_navette);
                ViewBag.id_societe = new SelectList(db.Societes, "id_utilisateur", "lieu", bus.id_societe);

                return View(bus);
            }
            else if (Session["UserID"] == null) { return RedirectToAction("login", "Home"); }
            return RedirectToAction("Index", "Home");
        }

        // GET: Buses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "societe")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Bus bus = db.Buses.Find(id);
                ViewBag.id = Session["user_id"];
                ViewBag.navr = db.Navettes.ToList();
                //ViewBag.id_navette = new SelectList(db.Navettes, "id_navette", "lieu_depart", bus.id_navette);
                ViewBag.id_societe = new SelectList(db.Societes, "id_utilisateur", "lieu", bus.id_societe);
                List<SelectListItem> Navette = new List<SelectListItem>();
                string navetta = "";
                foreach (var n in db.Navettes)
                {
                    navetta = n.lieu_depart + " - " + n.lieu_arriver;
                    Navette.Add(new SelectListItem { Text = navetta, Value = n.id_navette.ToString() });
                }
                var req = (from n in db.Navettes where n.id_navette == bus.id_navette select n).FirstOrDefault();
                Navette.Find(n => n.Text == req.lieu_depart + " - " + req.lieu_arriver).Selected = true;
                ViewBag.id_navette = Navette;
                ViewBag.id = Session["user_id"];

                if (bus == null)
                {
                    return HttpNotFound();
                }

                return View(bus);
            }
            else if (Session["UserID"] == null) { return RedirectToAction("login", "Home"); }
            return RedirectToAction("Index", "Home");
        }

        // POST: Buses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_bus,nom,nbr_place,climatiseur,tv,description,id_societe,id_navette,image")] Bus bus, HttpPostedFileBase imagefile)
        {

            if (Session["UserID"] != null)
            {
                int ids = int.Parse((Session["UserID"].ToString()));

                string namePic = Path.GetFileNameWithoutExtension(imagefile.FileName);
                string ext = Path.GetExtension(imagefile.FileName);
                namePic += System.DateTime.Now.ToString("yymmssfff") + ext;
                string path = Path.Combine(Server.MapPath("~/Content/"), namePic);
                bus.image = namePic;
                imagefile.SaveAs(path);



                if (ModelState.IsValid)
                {

                    bus.nbr_place = (int)bus.nbr_place;
                    db.Entry(bus).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = ids });
                }
                ViewBag.id_navette = new SelectList(db.Navettes, "id_navette", "lieu_depart", bus.id_navette);
                ViewBag.id_societe = new SelectList(db.Societes, "id_utilisateur", "lieu", bus.id_societe);
                return View(bus);
            }
            else if (Session["UserID"] == null) { return RedirectToAction("login", "Home"); }
            return RedirectToAction("Index", "Home");
        }

        // GET: Buses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "societe")
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

                ViewBag.navr = db.Navettes.ToList();
                return View(bus);
            }
            else if (Session["UserID"] == null) { return RedirectToAction("login", "Home"); }
            return RedirectToAction("Index", "Home");
        }

        // POST: Buses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "societe")
            {
                int ids = int.Parse((Session["UserID"].ToString()));
                Bus bus = db.Buses.Find(id);
                db.Buses.Remove(bus);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = ids });
            }
            else if (Session["UserID"] == null) { return RedirectToAction("login", "Home"); }
            return RedirectToAction("Index", "Home");
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
