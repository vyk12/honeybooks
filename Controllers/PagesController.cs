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

                User user = BL.User.getByLoginAndPasswd(login, password);

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
    }
}
