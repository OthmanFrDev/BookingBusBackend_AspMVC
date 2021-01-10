using BookingBus.Models;
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
        public ActionResult Index(int ids)
        {
            int id = int.Parse((Session["UserID"].ToString()));
            ViewBag.id = id;
            var abonnements = db.Abonnements.Include(a => a.Navette).Include(a => a.Societe).Where(a=>a.id_societe==ids).ToList();
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
            int id = int.Parse((Session["UserID"].ToString()));
            if (ModelState.IsValid)
            {
                db.Abonnements.Add(abonnement);
                db.SaveChanges();
                return RedirectToAction("Index",new { ids=id});
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
            if (abonnement == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_navette = new SelectList(db.Navettes, "id_navette", "lieu_depart", abonnement.id_navette);
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
            int id = int.Parse((Session["UserID"].ToString()));
            if (ModelState.IsValid)
            {
                db.Entry(abonnement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { ids = id });
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
            int ids = int.Parse((Session["UserID"].ToString()));
            Abonnement abonnement = db.Abonnements.Find(id);
            db.Abonnements.Remove(abonnement);
            db.SaveChanges();
            return RedirectToAction("Index" , new { ids = ids });
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
