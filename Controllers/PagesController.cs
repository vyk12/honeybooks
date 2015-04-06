using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;

namespace HoneyBooks.Controllers
{
    public class PagesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (Session["User"] != null)
            {
                return RedirectToAction("Index", "Books");
            }

            if (Request.HttpMethod == "POST")
            {
                string login = Request.Form["username"];
                string password = Settings.SecureString(Request.Form["password"]);

                User user = null;

                // Let's try to retrieve user from database
                try
                {
                    user = BL.User.getByLoginAndPasswd(login, password);
                }
                catch
                {
                    return View("Error500");
                }

                // If no user has been found, it means the client specified wrong information
                if (user == null)
                {
                    ViewBag.error = "Please verify your login information.";
                }
                else
                {
                    Session["User"] = user;
                    return RedirectToAction("Index", "Books");
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            // Let's remove the user from the session to log him/her out
            Session.Remove("User");

            return RedirectToAction("Index", "Pages");
        }
    }
}
