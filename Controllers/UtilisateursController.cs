using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookingBus.Models;

namespace BookingBus.Controllers
{
    public class UtilisateursController : Controller
    {
        private BookingBusEntities db = new BookingBusEntities();

        // GET: Utilisateurs
        public ActionResult Index()
        {
            var utilisateurs = db.Utilisateurs.Include(u => u.Admin).Include(u => u.Client).Include(u => u.Societe);
            return View(utilisateurs.ToList());
        }

        // GET: Utilisateurs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // GET: Utilisateurs/Create
        public ActionResult Create()
        {
            ViewBag.id_utilisateur = new SelectList(db.Admins, "id_utilisateur", "id_utilisateur");
            ViewBag.id_utilisateur = new SelectList(db.Clients, "id_utilisateur", "id_utilisateur");
            ViewBag.id_utilisateur = new SelectList(db.Societes, "id_utilisateur", "lieu");

            return View();
        }

        // POST: Utilisateurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_utilisateur,nom_complet,mail,mdp,telephone,role")] Utilisateur utilisateur,string lieu)
        {
            
            if (ModelState.IsValid)
            {
                
                db.Utilisateurs.Add(utilisateur);
                
                if (utilisateur.role == "client") { db.Clients.Add(new Client { id_utilisateur=utilisateur.id_utilisateur}); }
                else if (utilisateur.role == "admin") { db.Admins.Add(new Admin { id_utilisateur = utilisateur.id_utilisateur }); }
                else if (utilisateur.role == "societe") { db.Societes.Add(new Societe { id_utilisateur = utilisateur.id_utilisateur ,lieu=lieu}); }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_utilisateur = new SelectList(db.Admins, "id_utilisateur", "id_utilisateur", utilisateur.id_utilisateur);
            ViewBag.id_utilisateur = new SelectList(db.Clients, "id_utilisateur", "id_utilisateur", utilisateur.id_utilisateur);
            ViewBag.id_utilisateur = new SelectList(db.Societes, "id_utilisateur", "lieu", utilisateur.id_utilisateur);
            return View(utilisateur);
        }

        // GET: Utilisateurs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_utilisateur = new SelectList(db.Admins, "id_utilisateur", "id_utilisateur", utilisateur.id_utilisateur);
            ViewBag.id_utilisateur = new SelectList(db.Clients, "id_utilisateur", "id_utilisateur", utilisateur.id_utilisateur);
            ViewBag.id_utilisateur = new SelectList(db.Societes, "id_utilisateur", "lieu", utilisateur.id_utilisateur);
            return View(utilisateur);
        }

        // POST: Utilisateurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_utilisateur,nom_complet,mail,mdp,telephone,role")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                db.Entry(utilisateur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_utilisateur = new SelectList(db.Admins, "id_utilisateur", "id_utilisateur", utilisateur.id_utilisateur);
            ViewBag.id_utilisateur = new SelectList(db.Clients, "id_utilisateur", "id_utilisateur", utilisateur.id_utilisateur);
            ViewBag.id_utilisateur = new SelectList(db.Societes, "id_utilisateur", "lieu", utilisateur.id_utilisateur);
            return View(utilisateur);
        }

        // GET: Utilisateurs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // POST: Utilisateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            db.Utilisateurs.Remove(utilisateur);
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
