using BookingBus.Models;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;

namespace BookingBus.Controllers
{
   
    public class AdminsController : Controller
    {
        private BookingBusEntities db = new BookingBusEntities();
        string role = "admin";
        // GET: Admins
        public ActionResult Index()
        {

            if (Session["UserID"] != null && Session["role"].ToString() == role) { return View(); }

            else { return RedirectToAction("Login", "Home"); }
        }
        public ActionResult lister(string role) 
        {
            if (Session["UserID"] != null && Session["role"].ToString() == role) {  return RedirectToAction("lister", "Utilisateurs",new { role = role });}
            else { return RedirectToAction("Login", "Home"); }
        }
        // GET: Admins/Details/5        
        public ActionResult Details(int? id)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == role) {        if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);}
            else { return RedirectToAction("Login", "Home"); }
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            if (Session["UserID"] != null && Session["role"].ToString() == role) {ViewBag.id_utilisateur = new SelectList(db.Utilisateurs, "id_utilisateur", "nom_complet");
            return View(); }
            else { return RedirectToAction("Login", "Home"); }
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_utilisateur")] Admin admin)
        {

            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_utilisateur = new SelectList(db.Utilisateurs, "id_utilisateur", "nom_complet", admin.id_utilisateur);
            return View(admin);
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == role) {       if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_utilisateur = new SelectList(db.Utilisateurs, "id_utilisateur", "nom_complet", admin.id_utilisateur);
            return View(admin);}
            else { return RedirectToAction("Login", "Home"); }
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_utilisateur")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_utilisateur = new SelectList(db.Utilisateurs, "id_utilisateur", "nom_complet", admin.id_utilisateur);
            return View(admin);
        }

        // GET: Admins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == role) { 
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur user = db.Utilisateurs.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);}
            else { return RedirectToAction("Login", "Home"); }
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Utilisateur user = db.Utilisateurs.Find(id);
            db.Utilisateurs.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult detail()
        {
            if (Session["UserID"] != null && Session["role"].ToString() == role) { return RedirectToAction("Index", "Utilisateurs");}
            else { return RedirectToAction("Login", "Home"); }
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
