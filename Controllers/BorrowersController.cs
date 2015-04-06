using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;

namespace HoneyBooks.Controllers
{
    public class BorrowersController : Controller
    {
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
                // Let's create a new borrower and fill it with form's data
                BL.Borrower borrower = new BL.Borrower();

                borrower.FirstName = collection["FirstName"];
                borrower.LastName = collection["LastName"];
                borrower.PersonId = collection["PersonId"];
                borrower.Address = collection["Address"];
                borrower.Telno = collection["Telno"];
                borrower.CategoryId = Convert.ToInt32(collection["CategoryId"]);
                borrower.Username = collection["Username"];
                borrower.Password = Settings.SecureString(collection["Password"]);
                
                borrower.create();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // Action to delete a borrower
        public ActionResult Delete(string PersonId)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Pages");
            }

            try
            {
                // Let's delete everything the borrower has borrowed
                Borrow.delete(PersonId);

                // Let's delete the borrower
                Borrower.delete(PersonId);
            }
            catch
            {
                return View("Error500");
            }

            return RedirectToAction("List");
        }

        public ActionResult Details(string PersonId)
        {
            Borrower borrower;

            try
            {
                borrower = BL.Borrower.getByPersonId(PersonId);
            }
            catch
            {
                return View("Error500");
            }

            return View(borrower);
        }


        public ActionResult Edit(string PersonId)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Pages");
            }

            Borrower borrower;

            try
            {
                borrower = BL.Borrower.getByPersonId(PersonId);
                borrower.Password = "";
            }
            catch
            {
                return View("Error500");
            }

            return View(borrower);
        }


        [HttpPost]
        public ActionResult Edit(string PersonId, FormCollection collection)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Pages");
            }

            try
            {
                // Let's create a new borrower and fill it with form's data
                BL.Borrower borrower = BL.Borrower.getByPersonId(PersonId);

                borrower.FirstName = collection["FirstName"];
                borrower.LastName = collection["LastName"];
                borrower.Address = collection["Address"];
                borrower.Telno = collection["Telno"];
                borrower.CategoryId = Convert.ToInt32(collection["CategoryId"]);
                borrower.Username = collection["Username"];

                // If the password has been changed, let's update it
                if (collection["Password"] != "")
                {
                    borrower.Password = Settings.SecureString(collection["Password"]);
                }

                // Let's edit the borrower
                borrower.edit();

                borrower.Password = "";

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Index(string PersonId = "")
        {
            if (Session["Borrower"] == null && Session["User"] == null)
            {
                return RedirectToAction("Login");
            }

            Borrower borrower = null;

            // If the borrower is in session, let's retrieve it
            if (Session["Borrower"] != null)
            {
                borrower = (Borrower)Session["Borrower"];
            }
            // If not, let's try to fetch it from database
            else
            {
                try
                {
                    borrower = Borrower.getByPersonId(PersonId);
                }
                catch
                {
                    return View("Error500");
                }
            }

            ViewBag.borrower = borrower;

            // Let's try to get all borrowed books by the borrower
            try
            {
                ViewBag.borrows = Borrow.getByPersonId(borrower.PersonId);
            }
            catch
            {
                return View("Error500");
            }

            return View();
        }

        public ActionResult List()
        {
            List<Borrower> borrowers;

            try
            {
                borrowers = BL.Borrower.getAll();
            }
            catch
            {
                return View("Error500");
            }

            return View();
        }


        public ActionResult Login()
        {
            if (Session["Borrower"] != null)
            {
                return RedirectToAction("Index");
            }

            if (Request.HttpMethod == "POST")
            {
                string login = Request.Form["username"];
                string password = Settings.SecureString(Request.Form["password"]);

                Borrower borrower = null;

                // Let's try to obtain the requested borrower by its login and password
                try
                {
                    borrower = Borrower.getByLoginAndPasswd(login, password);
                }
                catch
                {
                    return View("Error500");
                }

                if (borrower == null)
                {
                    ViewBag.error = "Please verify your login information.";
                }
                else
                {
                    Session["Borrower"] = borrower;
                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            // Let's remove the borrower from the session to log him/her out
            Session.Remove("Borrower");

            return RedirectToAction("Index", "Books");
        }


        public ActionResult RenewLoan(string barcode, string personId = "")
        {
            if (Session["Borrower"] == null && Session["User"] == null)
            {
                return RedirectToAction("Login");
            }

            Borrower borrower = null;

            // Let's retrieve the borrower from the session
            if (Session["Borrower"] != null)
            {
                borrower = (Borrower)Session["Borrower"];
            }
            // Otherwise, let's retrieve him/her from the database
            else
            {
                try
                {
                    borrower = Borrower.getByPersonId(personId);
                }
                catch
                {
                    return View("Error500");
                }
            }

            Category category;

            // Let's try to get the borrower's category to get the period of a loan
            try
            {
                category = Category.getById(borrower.CategoryId);
            }
            catch
            {
                return View("Error500");
            }

            // Let's try to renew the loan
            try
            {
                Borrow.renewLoan(barcode, category.Period);
                Session["RenewLoanSuccess"] = true;
            }
            catch
            {
                return View("Error500");
            }

            if (Session["Borrower"] != null)
                return RedirectToAction("Index");
            else
                return RedirectToAction("Index", new { PersonId = borrower.PersonId });
        }
    }
}
