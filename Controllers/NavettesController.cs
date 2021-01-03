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
    public class NavettesController : Controller
    {
        private BookingBusEntities db = new BookingBusEntities();

        // GET: Navettes
        public ActionResult Index()
        {
            return View(db.Navettes.ToList());
        }

        // GET: Navettes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Navette navette = db.Navettes.Find(id);
            if (navette == null)
            {
                return HttpNotFound();
            }
            return View(navette);
        }

        // GET: Navettes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Navettes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_navette,lieu_depart,lieu_arriver,date_depart,date_arriver")] Navette navette)
        {
            if (ModelState.IsValid)
            {
                db.Navettes.Add(navette);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(navette);
        }

        // GET: Navettes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Navette navette = db.Navettes.Find(id);

            if (navette == null)
            {
                return HttpNotFound();
            }
            else { ViewBag.nav = navette;
            return View(navette);}
            
        }

        // POST: Navettes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_navette,lieu_depart,lieu_arriver,date_depart,date_arriver")] Navette navette)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(navette).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(navette);
        }

        // GET: Navettes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Navette navette = db.Navettes.Find(id);
            if (navette == null)
            {
                return HttpNotFound();
            }
            return View(navette);
        }

        // POST: Navettes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Navette navette = db.Navettes.Find(id);
            db.Navettes.Remove(navette);
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
