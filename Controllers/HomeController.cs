using BookingBus.Models;
using System.Linq;
using System.Web.Mvc;

namespace BookingBus.Controllers
{
    public class HomeController : Controller
    {

        private BookingBusEntities db = new BookingBusEntities();
        public ActionResult Index()
        {
            var ville = db.Villes.OrderBy(v => v.nom);
            //            if (Session["UserID"] != null) {
            //                int id = int.Parse((Session["UserID"].ToString()));

            //                var abo = db.Abonnements.ToList();
            //            foreach(var a in abo)
            //{
            //                var duree = a.date_fin.Subtract(System.DateTime.Now).Days;
            //                if (duree <= 0) {
            //                    Abonnement abonnement = db.Abonnements.Find(a.id_abonnement);
            //                    Effectuer effectuer = db.Effectuers.Find(a.id_abonnement,id);
            //                    db.Effectuers.Remove(effectuer);
            //                    db.Abonnements.Remove(abonnement);
            //                    db.SaveChanges();
            //                }
            //            }}
            return View(ville.ToList());

        }
        public ActionResult Apropos()
        {
            return View();
        }
        public ActionResult rechercher(string ld, string la)
        {
            var abonnement = db.Abonnements;
            var query = from a in db.Abonnements join n in db.Navettes on a.id_navette equals n.id_navette where n.lieu_depart == ld && n.lieu_arriver == la select a;
            return View(query.ToList());

        }
        public ActionResult Login()
        {
            if (Session["UserID"] != null) { return RedirectToAction("UserDashBoard"); }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Utilisateur objUser)
        {
            if (ModelState.IsValid)
            {
                using (BookingBusEntities db = new BookingBusEntities())
                {
                    var obj = db.Utilisateurs.Where(a => a.mail.Equals(objUser.mail) && a.mdp.Equals(objUser.mdp)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserID"] = obj.id_utilisateur.ToString();
                        Session["UserName"] = obj.nom_complet.ToString();
                        Session["mail"] = obj.mail.ToString();
                        Session["telephone"] = obj.telephone.ToString();
                        Session["role"] = obj.role.ToString();
                        if (obj.image != null) { Session["img"] = obj.image.ToString(); }
                        else { Session["img"] = "default.jpg"; }

                        //Session["user"] = objUser;
                        return RedirectToAction("UserDashBoard");
                    }
                }
            }
            return View(objUser);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        public ActionResult UserDashBoard()
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "societe") { return RedirectToAction("Index", "home"); }
            else if (Session["UserID"] != null && Session["role"].ToString() == "admin") {; return RedirectToAction("Index", "home"); }
            else if (Session["UserID"] != null && Session["role"].ToString() == "client") { return RedirectToAction("Index", "home"); }
            else
            {
                return RedirectToAction("Login");
            }
        }

    }
}