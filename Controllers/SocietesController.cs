using BookingBus.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BookingBus.Controllers
{
    public class SocietesController : Controller
    {
        string role = "societe";
        private BookingBusEntities db = new BookingBusEntities();

        // GET: Societes
        
        public ActionResult Index()
        {

            // new url().Urlsup(role);
            if (Session["UserID"].ToString() != null) 
            {
                if (Session["UserID"] != null && Session["role"].ToString() == "societe") { 
                int id = int.Parse((Session["UserID"].ToString()));
            ViewBag.id = id;
             var societes = db.Abonnements.Where(s=>s.id_societe==id);
            return View(societes.ToList());  }
            else { return RedirectToAction("Index", "Home"); }
            }
            else { return RedirectToAction("login", "Home"); }
        }
        public ActionResult lister(string role)
        {
            if (Session["UserID"].ToString() != null) { 
            int id = int.Parse((Session["UserID"].ToString()));
            if (Session["UserID"] != null && Session["role"].ToString() == "societe") {    if (role == "abonnement") { return RedirectToAction("Index", "Abonnements",new { ids=id}); }
            else if (role == "bus") { return RedirectToAction("Index", "Buses", new { id = id }); }
            return View();}
            else { return RedirectToAction("Index", "Home"); }
            }
            else { return RedirectToAction("login", "Home"); }
        }
        public ActionResult consulter() 
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "societe") {  return RedirectToAction("Index", "Demandes");}
            else { return RedirectToAction("Index", "Home"); }
        }
        public ActionResult createbus(int id)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "societe")
            { return RedirectToAction("Create", "Buses", new { id = id }); }
            else { return RedirectToAction("Index", "Home"); }
        }

        public ActionResult creatabonnement(int id)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "societe")
            { return RedirectToAction("create", "Abonnements", new { id = id }); }
            else { return RedirectToAction("Index", "Home"); }
        }
        [HttpPost]
        //public ActionResult Creatabonnement(Abonnement abonnement)
        //{
        //    new AbonnementsController().Create(abonnement);
        //    return RedirectToAction("Index", "Abonnements");
        //}
        // GET: Societes/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "societe") {
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Societe societe = db.Societes.Find(id);
            if (societe == null)
            {
                return HttpNotFound();
            }
            return View(societe); }
            else { return RedirectToAction("Index", "Home"); }
        }

        // GET: Societes/Create
        //public ActionResult Create()
        //{
        //    ViewBag.id_utilisateur = new SelectList(db.Abonnements, "id_abonnement", "id_abonnement");
        //    ViewBag.id_utilisateur = new SelectList(db.Utilisateurs, "id_utilisateur", "nom_complet");
        //    return View();
        //}

        // POST: Societes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "id_utilisateur,lieu")] Societe societe)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Societes.Add(societe);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.id_utilisateur = new SelectList(db.Abonnements, "id_abonnement", "id_abonnement", societe.id_utilisateur);
        //    ViewBag.id_utilisateur = new SelectList(db.Utilisateurs, "id_utilisateur", "nom_complet", societe.id_utilisateur);
        //    return View(societe);
        //}

        // GET: Societes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "societe") { 
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
            else { return RedirectToAction("Index", "Home"); }
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
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Societe societe = db.Societes.Find(id);
        //    if (societe == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(societe);
        //}

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
