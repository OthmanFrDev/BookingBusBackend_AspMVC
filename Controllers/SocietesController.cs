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
    public class SocietesController : Controller
    {
        private BookingBusEntities db = new BookingBusEntities();

        // GET: Societes
        public ActionResult Index()
        {
            var societes = db.Societes.Include(s => s.Abonnement).Include(s => s.Utilisateur);
            return View(societes.ToList());
        }

        // GET: Societes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Societe societe = db.Societes.Find(id);
            if (societe == null)
            {
                return HttpNotFound();
            }
            return View(societe);
        }

        // GET: Societes/Create
        public ActionResult Create()
        {
            ViewBag.id_utilisateur = new SelectList(db.Abonnements, "id_abonnement", "id_abonnement");
            ViewBag.id_utilisateur = new SelectList(db.Utilisateurs, "id_utilisateur", "nom_complet");
            return View();
        }

        // POST: Societes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_utilisateur,lieu")] Societe societe)
        {
            if (ModelState.IsValid)
            {
                db.Societes.Add(societe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_utilisateur = new SelectList(db.Abonnements, "id_abonnement", "id_abonnement", societe.id_utilisateur);
            ViewBag.id_utilisateur = new SelectList(db.Utilisateurs, "id_utilisateur", "nom_complet", societe.id_utilisateur);
            return View(societe);
        }

        // GET: Societes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Societe societe = db.Societes.Find(id);
            if (societe == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_utilisateur = new SelectList(db.Abonnements, "id_abonnement", "id_abonnement", societe.id_utilisateur);
            ViewBag.id_utilisateur = new SelectList(db.Utilisateurs, "id_utilisateur", "nom_complet", societe.id_utilisateur);
            return View(societe);
        }

        // POST: Societes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_utilisateur,lieu")] Societe societe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(societe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_utilisateur = new SelectList(db.Abonnements, "id_abonnement", "id_abonnement", societe.id_utilisateur);
            ViewBag.id_utilisateur = new SelectList(db.Utilisateurs, "id_utilisateur", "nom_complet", societe.id_utilisateur);
            return View(societe);
        }

        // GET: Societes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Societe societe = db.Societes.Find(id);
            if (societe == null)
            {
                return HttpNotFound();
            }
            return View(societe);
        }

        // POST: Societes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Societe societe = db.Societes.Find(id);
            db.Societes.Remove(societe);
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
