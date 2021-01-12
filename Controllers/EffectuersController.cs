using BookingBus.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BookingBus.Controllers
{
    public class EffectuersController : Controller
    {
        private BookingBusEntities db = new BookingBusEntities();

        // GET: Effectuers
        public ActionResult Index(int id, string? message)
        {

            if (Session["UserID"] != null && Session["role"].ToString() == "client")
            {
                var effectuers = db.Effectuers.Include(e => e.Abonnement).Include(e => e.Client).Where(e => e.id_client == id);
                var query = (from e in effectuers join b in db.Buses on e.Abonnement.id_navette equals b.id_navette select b).ToList();
                ViewBag.bus = query;
                ViewBag.exist = message;
                int i = effectuers.Select(a => a.id_abonnement).FirstOrDefault();
                if (i == 0) { ViewBag.msg = "no abonnement reserved"; }
                return View(effectuers.ToList());
            }
            else if (Session["UserID"] == null) { return RedirectToAction("login", "home"); }
            return RedirectToAction("login", "home");
        }

        // GET: Effectuers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Effectuer effectuer = db.Effectuers.Find(id);
            if (effectuer == null)
            {
                return HttpNotFound();
            }
            return View(effectuer);
        }

        // GET: Effectuers/Create
        public ActionResult Create()
        {
            ViewBag.id_abonnement = new SelectList(db.Abonnements, "id_abonnement", "id_abonnement");
            ViewBag.id_client = new SelectList(db.Clients, "id_utilisateur", "id_utilisateur");
            return View();
        }

        // POST: Effectuers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_client,id_abonnement,duree")] Effectuer effectuer)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "client")
            {
                if (ModelState.IsValid)
                {
                    db.Effectuers.Add(effectuer);
                    db.SaveChanges();
                    return RedirectToAction("Index",new { id=effectuer.id_client});
                }

                ViewBag.id_abonnement = new SelectList(db.Abonnements, "id_abonnement", "id_abonnement", effectuer.id_abonnement);
                ViewBag.id_client = new SelectList(db.Clients, "id_utilisateur", "id_utilisateur", effectuer.id_client);
                return View(effectuer);
            }
            return RedirectToAction("login", "home");
        }

        // GET: Effectuers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "client")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Effectuer effectuer = db.Effectuers.Find(id);
                if (effectuer == null)
                {
                    return HttpNotFound();
                }
                ViewBag.id_abonnement = new SelectList(db.Abonnements, "id_abonnement", "id_abonnement", effectuer.id_abonnement);
                ViewBag.id_client = new SelectList(db.Clients, "id_utilisateur", "id_utilisateur", effectuer.id_client);
                return View(effectuer);
            }
            return RedirectToAction("login", "home");
        }

        // POST: Effectuers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_client,id_abonnement,duree")] Effectuer effectuer)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "client")
            {
                if (ModelState.IsValid)
                {
                    db.Entry(effectuer).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.id_abonnement = new SelectList(db.Abonnements, "id_abonnement", "id_abonnement", effectuer.id_abonnement);
                ViewBag.id_client = new SelectList(db.Clients, "id_utilisateur", "id_utilisateur", effectuer.id_client);
                return View(effectuer);
            }
            return RedirectToAction("login", "home");
        }

        // GET: Effectuers/Delete/5
        public ActionResult Delete(int? id1, int? id2)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "client")
            {
                if (id1 == null || id2 == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Effectuer effectuer = db.Effectuers.Find(id1, id2);
                if (effectuer == null)
                {
                    return HttpNotFound();
                }
                return View(effectuer);
            }
            return RedirectToAction("login", "home");
        }

        // POST: Effectuers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id1, int id2)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "client")
            {
                Effectuer effectuer = db.Effectuers.Find(id1, id2);
                db.Effectuers.Remove(effectuer);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id1 });
            }
            return RedirectToAction("login", "home");
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
