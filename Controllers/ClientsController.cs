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
                var clients = db.Clients.Include(c => c.Utilisateur);
                return View(clients.ToList());
            }

            else { return RedirectToAction("Login", "Home"); }

        }
        public ActionResult Demander()
        {
            return RedirectToAction("Create", "Demandes");
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
        public ActionResult Reserver()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Reserver(Abonnement abonnement)
        {

            return View();
        }
        // GET: Clients/Details/5
        public ActionResult Details(int? id)
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

        // GET: Clients/Create
        public ActionResult Create()
        {
            ViewBag.id_utilisateur = new SelectList(db.Utilisateurs, "id_utilisateur", "nom_complet");
            return View();
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
