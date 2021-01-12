using BookingBus.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BookingBus.Controllers
{
    public class DemandesController : Controller
    {
        private BookingBusEntities db = new BookingBusEntities();

        // GET: Demandes
  
        public ActionResult Index(int? id ,string? msg)
        {
            if (id != null) { ViewBag.id = id;
            var demandes = db.Demandes.Include(d => d.Client).Where(d => d.id_client == id);
                int i = demandes.Select(a => a.id_demande).FirstOrDefault();
                if (i == 0) { ViewBag.messaged = "no demande to be found!"; }
                return View(demandes.ToList()); }
            else {
                if (msg != null) { ViewBag.msg = msg; }
                var demandes = db.Demandes.Include(d => d.Client);
                int i = demandes.Select(a => a.id_demande).FirstOrDefault();
                if (i == 0) { ViewBag.messaged = "no demande to be found!"; }
                return View(demandes.ToList());
            }
        }
        [HttpPost]
     public ActionResult Ajouterdemande(int id) 
        {
            if(Session["UserID"]!=null&& Session["role"].ToString() == "societe") 
            {
                var query = (from d in db.Demandes where d.id_demande == id select d).FirstOrDefault();
                var check = (from t in db.Navettes where t.lieu_depart == query.depart && t.lieu_arriver == query.arriver select t).FirstOrDefault();
                if (check != null) 
                {
                    return RedirectToAction("create", "abonnements",new { id=Session["UserID"].ToString(),id_navette=check.id_navette});
                }
                else { string msg = "no route was found !"; return RedirectToAction("index",new { msg=msg}); }
            }
            else if (Session["UserID"] == null) { return RedirectToAction("login", "home"); }
            return RedirectToAction("index", "home");
        }

        // GET: Demandes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Demande demande = db.Demandes.Find(id);
            if (demande == null)
            {
                return HttpNotFound();
            }
            return View(demande);
        }

        // GET: Demandes/Create
        public ActionResult Create(int id)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "client") { 
            ViewBag.id_client = new SelectList(db.Clients, "id_utilisateur", "id_utilisateur");
            ViewBag.id = id;
            return View();
            }
            else if (Session["UserID"] == null) { return RedirectToAction("login", "Home"); }
            return RedirectToAction("Index", "Home");
        }

        // POST: Demandes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_demande,depart,arriver,date_depart,date_arriver,id_client")] Demande demande)
        {
            var olddemande = db.Demandes.Where(d=>d.depart==demande.depart && d.arriver==demande.arriver && d.date_arriver==demande.date_arriver && d.date_depart==demande.date_depart).FirstOrDefault();
            if (olddemande != null) {
                olddemande.number += 1;
                db.SaveChanges();
                db.Entry(olddemande).State = EntityState.Modified; 
                return RedirectToAction("Index"); }
            else
            {
                if (ModelState.IsValid)
                {
                    demande.number = 1;
                    db.Demandes.Add(demande);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.id_client = new SelectList(db.Clients, "id_utilisateur", "id_utilisateur", demande.id_client);
                return View(demande);
            }
        }

        // GET: Demandes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "client")
            {
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Demande demande = db.Demandes.Find(id);
            if (demande == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_client = new SelectList(db.Clients, "id_utilisateur", "id_utilisateur", demande.id_client);
            ViewBag.number = demande.number;
            return View(demande);
            }
            else if (Session["UserID"] == null) { return RedirectToAction("login", "Home"); }
            return RedirectToAction("Index", "Home");
        }

        // POST: Demandes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_demande,depart,arriver,date_depart,date_arriver,id_client,number")] Demande demande)
        {
            if (ModelState.IsValid)
            {
                db.Entry(demande).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_client = new SelectList(db.Clients, "id_utilisateur", "id_utilisateur", demande.id_client);
            return View(demande);
        }

        // GET: Demandes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Demande demande = db.Demandes.Find(id);
            if (demande == null)
            {
                return HttpNotFound();
            }
            return View(demande);
        }

        // POST: Demandes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Demande demande = db.Demandes.Find(id);
            db.Demandes.Remove(demande);
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
