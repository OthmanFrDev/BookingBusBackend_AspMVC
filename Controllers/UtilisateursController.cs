using BookingBus.Models;
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
            if (Session["UserID"] != null && Session["role"].ToString() == "admin") { 
                ViewBag.role = role;
            if (role == "client") { var utilisateurs = db.Utilisateurs.Where(u => u.role == role).Include(u => u.Client); return View(utilisateurs.ToList()); }
            else if (role == "societe") { var utilisateurs = db.Utilisateurs.Where(u => u.role == role).Include(u => u.Societe); return View(utilisateurs.ToList()); }
            return View("Index", "Admins");
                          }
            else { return RedirectToAction("Index", "Home"); }
                

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
            ViewBag.role = role;if (ViewBag.role != null) {
            return View(); }
            else { return RedirectToAction("Index","Home"); }
        }

        // POST: Utilisateurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_utilisateur,nom_complet,mail,mdp,telephone,role")] Utilisateur utilisateur,string lieu)
        {
            var exist = db.Utilisateurs.Where(u => u.mail == utilisateur.mail).FirstOrDefault();
            if (utilisateur.role == null) { return RedirectToAction("Index", "Home"); }
            if (exist != null) { ViewBag.exist = "mail already exsit please select a new one"; return View(utilisateur); }
            else { 
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
            return View(utilisateur);}
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
            ViewBag.role = utilisateur.role;
            ViewBag.lieu = utilisateur.Societe.lieu;
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
        public ActionResult Edit([Bind(Include = "id_utilisateur,nom_complet,mail,mdp,telephone,role")] Utilisateur utilisateur,string lieu)
        {
            ViewBag.role = utilisateur.role;
           

            if (ModelState.IsValid)
            {
                var societe = db.Societes.Find(utilisateur.id_utilisateur);
                societe.lieu = lieu;
                db.Entry(utilisateur).State = EntityState.Modified;
                db.Entry(societe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("details",new { id=utilisateur.id_utilisateur});
            }
            ViewBag.id_utilisateur = new SelectList(db.Admins, "id_utilisateur", "id_utilisateur", utilisateur.id_utilisateur);
            ViewBag.id_utilisateur = new SelectList(db.Clients, "id_utilisateur", "id_utilisateur", utilisateur.id_utilisateur);
            ViewBag.id_utilisateur = new SelectList(db.Societes, "id_utilisateur", "lieu", utilisateur.id_utilisateur);
            return View();
        }

        // GET: Utilisateurs/Delete/5
    
        // POST: Utilisateurs/Delete/5
        [ActionName("Delete")]
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
