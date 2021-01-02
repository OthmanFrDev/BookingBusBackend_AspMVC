using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookingBus.Models;

namespace BookingBus.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Utilisateur objUser)
        {
            if (ModelState.IsValid)
            {
                using (BookingBusEntities  db = new BookingBusEntities ())
                {
                    var obj = db.Utilisateurs.Where(a => a.mail.Equals(objUser.mail) && a.mdp.Equals(objUser.mdp)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserID"] = obj.id_utilisateur.ToString();
                        Session["UserName"] = obj.nom_complet.ToString();
                        Session["mail"] = obj.mail.ToString();
                        Session["telephone"] = obj.telephone.ToString();
                        Session["role"] = obj.role.ToString();
                       
                        //Session["user"] = objUser;
                        return RedirectToAction("UserDashBoard");
                    }
                }
            }
            return View(objUser);
        }

        public ActionResult UserDashBoard()
        {
            if (Session["UserID"] != null && Session["role"].ToString() == "societe") { return RedirectToAction("Index", "Societes"); }
            else if (Session["UserID"] != null && Session["role"].ToString() == "admin") { return RedirectToAction("Index", "Admins"); }
            else if (Session["UserID"] != null && Session["role"].ToString() == "client") { return RedirectToAction("Index", "Clients"); }
            else
            {
                return RedirectToAction("Login");
            }
        }

    }
}