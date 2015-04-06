using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;

namespace HoneyBooks.Controllers
{
    public class UsersController : Controller
    {
        public ActionResult Index()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Pages");
            }

            List<BL.User> users;

            try
            {
                users = BL.User.getAll();
            }
            catch
            {
                return View("Error500");
            }

            return View(users);
        }

        public ActionResult Create()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Pages");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Pages");
            }

            try
            {
                // Let's create a new user and fill it with form's data
                BL.User user = new BL.User();
                user.Username = collection["Username"];
                user.Password = Settings.SecureString(collection["Password"]);

                user.save();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Pages");
            }

            User user;

            // Let's try to fetch the user from the database
            try
            {
                user = BL.User.getById(id);

                // Let's remove its password so it won't put the md5 hash into the password text field
                user.Password = "";
            }
            catch
            {
                return View("Error500");
            }

            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Pages");
            }

            try
            {
                BL.User user = BL.User.getById(id);
                user.Username = collection["Username"];

                // If a password has been specified, let's update the user's old password
                if (collection["Password"] != "")
                {
                    user.Password = Settings.SecureString(collection["Password"]);
                }

                user.save();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Pages");
            }

            // Let's try to delete the user
            try
            {
                BL.User.deleteById(id);
            }
            catch
            {
                return View("Error500");
            }

            return RedirectToAction("Index");
        }

    }
}
