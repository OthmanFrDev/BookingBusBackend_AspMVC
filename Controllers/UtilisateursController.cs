﻿using BookingBus.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BookingBus.Controllers
{
    public class UtilisateursController : Controller
    {
        private BookingBusEntities db = new BookingBusEntities();

        // GET: Utilisateurs
        public ActionResult Index()
        {
            //ViewBag.role = role;
            //if (role == 1) {  var utilisateurs = db.Utilisateurs.Where(u => u.role == "client").Include(u => u.Client); return View(utilisateurs.ToList());}
            //else if(role == 2) { var utilisateurs = db.Utilisateurs.Where(u=>u.role=="societe").Include(u => u.Societe); return View(utilisateurs.ToList()); }
            //return View("Index","Admins");
            return View();

        }
        public ActionResult lister(string role)
        {
            ViewBag.role = role;
            if (role == "client") { var utilisateurs = db.Utilisateurs.Where(u => u.role == role).Include(u => u.Client); return View(utilisateurs.ToList()); }
            else if (role == "societe") { var utilisateurs = db.Utilisateurs.Where(u => u.role == role).Include(u => u.Societe); return View(utilisateurs.ToList()); }
            return View("Index", "Admins");


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
        public ActionResult Create(string role)
        {
            ViewBag.id_utilisateur = new SelectList(db.Admins, "id_utilisateur", "id_utilisateur");
            ViewBag.id_utilisateur = new SelectList(db.Clients, "id_utilisateur", "id_utilisateur");
            ViewBag.id_utilisateur = new SelectList(db.Societes, "id_utilisateur", "lieu");
            ViewBag.role = role;
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
                return RedirectToAction("index","Home");
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
                return RedirectToAction("lister",new { role=utilisateur.role});
            }
            ViewBag.id_utilisateur = new SelectList(db.Admins, "id_utilisateur", "id_utilisateur", utilisateur.id_utilisateur);
            ViewBag.id_utilisateur = new SelectList(db.Clients, "id_utilisateur", "id_utilisateur", utilisateur.id_utilisateur);
            ViewBag.id_utilisateur = new SelectList(db.Societes, "id_utilisateur", "lieu", utilisateur.id_utilisateur);
            return View("Index","Admins");
        }

        // GET: Utilisateurs/Delete/5
        public ActionResult Delete(int? id)
        {
            return RedirectToAction("DeleteConfirmed", new { id = id });
        }

        // POST: Utilisateurs/Delete/5
        [HttpGet, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            db.Utilisateurs.Remove(utilisateur);
            db.SaveChanges();
            return RedirectToAction("lister",new { role=utilisateur.role});
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
