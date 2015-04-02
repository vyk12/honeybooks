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

        //
        // POST: /Authors/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Pages");
            }

            try
            {
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

            Borrow.delete(PersonId);
            Borrower.delete(PersonId);

            return RedirectToAction("List");
        }

        public ActionResult Details(string PersonId)
        {
            return View(BL.Borrower.getByPersonId(PersonId));
        }

        //
        // GET: /Authors/Edit/5

        public ActionResult Edit(string PersonId)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Pages");
            }

            Borrower borrower = BL.Borrower.getByPersonId(PersonId);
            borrower.Password = "";
            return View(borrower);
        }

        //
        // POST: /Authors/Edit/5

        [HttpPost]
        public ActionResult Edit(string PersonId, FormCollection collection)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Pages");
            }

            try
            {
                BL.Borrower borrower = BL.Borrower.getByPersonId(PersonId);

                borrower.FirstName = collection["FirstName"];
                borrower.LastName = collection["LastName"];
                borrower.Address = collection["Address"];
                borrower.Telno = collection["Telno"];
                borrower.CategoryId = Convert.ToInt32(collection["CategoryId"]);
                borrower.Username = collection["Username"];

                if (collection["Password"] != "")
                {
                    borrower.Password = Settings.SecureString(collection["Password"]);
                }

                borrower.edit();

                borrower.Password = "";

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // Action to log in as a borrower
        public ActionResult Index(string PersonId = "")
        {
            if (Session["Borrower"] == null && Session["User"] == null)
            {
                return RedirectToAction("Login");
            }

            Borrower borrower = null;

            if (Session["Borrower"] != null)
                borrower = (Borrower)Session["Borrower"];
            else
                borrower = Borrower.getByPersonId(PersonId);

            ViewBag.borrower = borrower;

            ViewBag.borrows = Borrow.getByPersonId(borrower.PersonId);

            return View();
        }

        public ActionResult List()
        {
            return View(BL.Borrower.getAll());
        }

        // Action to log in a borrower
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

                Borrower borrower = Borrower.getByLoginAndPasswd(login, password);

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
            Session.Remove("Borrower");

            return RedirectToAction("Index", "Books");
        }

        // Action to renew a loan
        public ActionResult RenewLoan(string barcode, string personId = "")
        {
            if (Session["Borrower"] == null && Session["User"] == null)
            {
                return RedirectToAction("Login");
            }

            Borrower borrower = null;

            if (Session["Borrower"] != null)
                borrower = (Borrower)Session["Borrower"];
            else
                borrower = Borrower.getByPersonId(personId);

            Category category = Category.getById(borrower.CategoryId);

            Borrow.renewLoan(barcode, category.Period);
            Session["RenewLoanSuccess"] = true;

            if (Session["Borrower"] != null)
                return RedirectToAction("Index");
            else
                return RedirectToAction("Index", new { PersonId = borrower.PersonId });
        }
    }
}
