using BookingBus.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BookingBus.Controllers
{
    public class ClientsController : Controller
    {
        private BookingBusEntities db = new BookingBusEntities();

        // GET: Clients
        public ActionResult Index()
        {


            if (Session["UserID"] != null && Session["role"].ToString() == "client")
            {
                int id = int.Parse((Session["UserID"].ToString()));
                ViewBag.id = id;
                var clients = db.Utilisateurs.Where(u => u.role == "client");
                /* return View(clients.ToList());*/
                return View();
            }

            else if (Session["UserID"] == null) { return RedirectToAction("Login", "Home"); }

            return RedirectToAction("Login", "Home");

        }
        public ActionResult Demander(int id)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "client") { return RedirectToAction("Create", "Demandes", new { id = id }); }
            else { return RedirectToAction("Login", "Home"); }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Demander(Demande dm)
        {
            if (ModelState.IsValid)
            {
                db.Demandes.Add(dm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Reserver(int id)
        {

            if (Session["UserID"] != null && Session["role"].ToString() == "client")
            {
                int id_user = int.Parse((Session["UserID"].ToString()));
                var exist = db.Effectuers.Where(a => a.id_abonnement == id && a.id_client == id_user).FirstOrDefault();
                if (exist != null) { string msg = "Abonnement already exist !"; return RedirectToAction("index", "effectuers", new { id = id_user, message = msg }); }
                Abonnement ab = db.Abonnements.Find(id);
                var annee = ab.date_fin.Year - ab.date_debut.Year;

                var date = ab.date_fin.Subtract(System.DateTime.Now).Days;

                Effectuer res = new Effectuer { id_client = id_user, id_abonnement = id, duree = date };
                db.Effectuers.Add(res);
                db.SaveChanges();
                return RedirectToAction("Index", "Effectuers", new { id = Session["UserID"] });
            }
            else { return RedirectToAction("Login", "Home"); }
        }
        [HttpPost]
        public ActionResult Reserver(Abonnement abonnement)
        {

            return View();
        }
        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "client")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Client client = db.Clients.Find(id);
                if (client == null)
                {
                    return HttpNotFound();
                }
                return View(client);
            }
            else { return RedirectToAction("Login", "Home"); }
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "client")
            {
                ViewBag.id_utilisateur = new SelectList(db.Utilisateurs, "id_utilisateur", "nom_complet");
                return View();
            }
            else { return RedirectToAction("Login", "Home"); }
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_utilisateur")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_utilisateur = new SelectList(db.Utilisateurs, "id_utilisateur", "nom_complet", client.id_utilisateur);
            return View(client);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "client")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Client client = db.Clients.Find(id);
                if (client == null)
                {
                    return HttpNotFound();
                }
                ViewBag.id_utilisateur = new SelectList(db.Utilisateurs, "id_utilisateur", "nom_complet", client.id_utilisateur);
                return View(client);
            }
            else { return RedirectToAction("Login", "Home"); }
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_utilisateur")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_utilisateur = new SelectList(db.Utilisateurs, "id_utilisateur", "nom_complet", client.id_utilisateur);
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "client")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Client client = db.Clients.Find(id);
                if (client == null)
                {
                    return HttpNotFound();
                }
                return View(client);
            }
            else { return RedirectToAction("Login", "Home"); }
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
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
