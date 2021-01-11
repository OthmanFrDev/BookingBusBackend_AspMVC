using BookingBus.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BookingBus.Controllers
{
    public class NavettesController : Controller
    {
        private BookingBusEntities db = new BookingBusEntities();

        // GET: Navettes
        public ActionResult Index()
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "admin")
            {
                return View(db.Navettes.ToList());
            }
            return RedirectToAction("index", "home");
        }

        // GET: Navettes/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "admin")
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
            return RedirectToAction("index", "home");
        }

        // GET: Navettes/Create
        public ActionResult Create()
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "admin")
            {
                ViewBag.ville = db.Villes.OrderBy(v => v.nom).Select(v => v.nom).ToList();
                return View();
            }
            return RedirectToAction("index", "home");
            
        }

        // POST: Navettes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_navette,lieu_depart,lieu_arriver,date_depart,date_arriver")] Navette navette)
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "admin")
            {
                if (ModelState.IsValid)
                {
                    db.Navettes.Add(navette);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(navette);
            }return RedirectToAction("index", "home");
        }

        // GET: Navettes/Edit/5
        public ActionResult Edit(int? id)
        {
                if (Session["UserID"] != null && Session["role"].ToString() == "admin")
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
            else
            {
                ViewBag.nav = navette;
                return View(navette);
            }
            }
            return RedirectToAction("index", "home");
        }

        // POST: Navettes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_navette,lieu_depart,lieu_arriver,date_depart,date_arriver")] Navette navette)
        {

                    if (Session["UserID"] != null && Session["role"].ToString() == "admin")
                    {
                        if (ModelState.IsValid)
            {
                db.Entry(navette).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(navette);
            }
            return RedirectToAction("index", "home");
        }

        // GET: Navettes/Delete/5
        public ActionResult Delete(int? id)
        {
                        if (Session["UserID"] != null && Session["role"].ToString() == "admin")
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
            return RedirectToAction("index", "home");
        }

        // POST: Navettes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
                            if (Session["UserID"] != null && Session["role"].ToString() == "admin")
                            {
                                Navette navette = db.Navettes.Find(id);
            db.Navettes.Remove(navette);
            db.SaveChanges();
            return RedirectToAction("Index");
            }
            return RedirectToAction("index", "home");
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
