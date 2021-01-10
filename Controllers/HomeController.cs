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
            var ville = db.Villes.OrderBy(v=>v.nom);
            
            return View(ville.ToList());
            
        }
        public ActionResult rechercher(string ld,string la)
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
                        Session["img"] = obj.image.ToString();

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
            if (Session["UserID"] != null && Session["role"].ToString() == "societe") { return RedirectToAction("Index", "Societes"); }
            else if (Session["UserID"] != null && Session["role"].ToString() == "admin") {   ;return RedirectToAction("Index", "Admins"); }
            else if (Session["UserID"] != null && Session["role"].ToString() == "client") { return RedirectToAction("Index", "Clients"); }
            else
            {
                return RedirectToAction("Login");
            }
        }

    }
}