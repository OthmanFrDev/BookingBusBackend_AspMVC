using BookingBus.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BookingBus.Controllers
{
    public class AbonnementsController : Controller
    {
        private BookingBusEntities db = new BookingBusEntities();

        // GET: Abonnements
        public ActionResult Index()
        {
            int id = int.Parse((Session["UserID"].ToString()));
            ViewBag.id = id;
            ViewBag.img = db.Societes.Find(id).Utilisateur.image;
            var abonnements = db.Abonnements.Include(a => a.Navette).Include(a => a.Societe1).ToList();
            return View(abonnements.ToList());
        }

        // GET: Abonnements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonnement abonnement = db.Abonnements.Find(id);
            if (abonnement == null)
            {
                return HttpNotFound();
            }
            return View(abonnement);
        }

        // GET: Abonnements/Create
        public ActionResult Create(int id)
        {
            ViewBag.id = id;
            ViewBag.id_navette = new SelectList(db.Navettes, "id_navette", "lieu_depart");
            ViewBag.id_societe = new SelectList(db.Societes, "id_utilisateur", "lieu");
            ViewBag.navr = db.Navettes.ToList();
            return View();
        }

        // POST: Abonnements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_abonnement,date_debut,date_fin,id_navette,id_societe,prix")] Abonnement abonnement)
        {
            if (ModelState.IsValid)
            {
                db.Abonnements.Add(abonnement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_navette = new SelectList(db.Navettes, "id_navette", "lieu_depart", abonnement.id_navette);
            ViewBag.id_societe = new SelectList(db.Societes, "id_utilisateur", "lieu", abonnement.id_societe);
            return View(abonnement);
        }

        // GET: Abonnements/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonnement abonnement = db.Abonnements.Find(id);
            List<SelectListItem> Navette = new List<SelectListItem>();
            string navetta = "";
            foreach (var n in db.Navettes)
            {
                navetta = n.lieu_depart + " - " + n.lieu_arriver;
                Navette.Add(new SelectListItem { Text = navetta, Value = n.id_navette.ToString() });
            }
            var req = (from n in db.Navettes where n.id_navette == abonnement.id_navette select n).FirstOrDefault();
            Navette.Find(n => n.Text == req.lieu_depart + " - " + req.lieu_arriver).Selected = true;
            ViewBag.id_navette = Navette;
            if (abonnement == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_societe = new SelectList(db.Societes, "id_utilisateur", "lieu", abonnement.id_societe);
            return View(abonnement);
        }

        // POST: Abonnements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_abonnement,date_debut,date_fin,id_navette,id_societe,prix")] Abonnement abonnement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(abonnement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_navette = new SelectList(db.Navettes, "id_navette", "lieu_depart", abonnement.id_navette);
            ViewBag.id_societe = new SelectList(db.Societes, "id_utilisateur", "lieu", abonnement.id_societe);
            return View(abonnement);
        }

        // GET: Abonnements/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonnement abonnement = db.Abonnements.Find(id);
            if (abonnement == null)
            {
                return HttpNotFound();
            }
            return View(abonnement);
        }

        // POST: Abonnements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Abonnement abonnement = db.Abonnements.Find(id);
            db.Abonnements.Remove(abonnement);
            db.SaveChanges();
            return RedirectToAction("Index"         );
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
