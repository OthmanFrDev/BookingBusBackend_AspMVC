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
                        return RedirectToAction("UserDashBoard");
                    }
                }
            }
            return View(objUser);
        }

        public ActionResult UserDashBoard()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}