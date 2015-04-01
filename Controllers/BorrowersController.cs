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
        // Action to add a borrower
        public ActionResult Add()
        {
            return View();
        }

        // Action to delete a borrower
        public void Delete(int id)
        {

        }

        // Action to log in as a borrower
        public ActionResult Index()
        {
            if (Session["Borrower"] == null)
            {
                return RedirectToAction("Login");
            }

            Borrower borrower = (Borrower)Session["borrower"];

            ViewBag.borrower = borrower;

            ViewBag.borrows = Borrow.getByPersonId(borrower.PersonId);

            return View();
        }

        // Action to log in a borrower
        public ActionResult Login()
        {
            if (Session["borrower"] != null)
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
        public ActionResult RenewLoan(string barcode)
        {
            Borrower borrower = (Borrower)Session["borrower"];
            Category category = Category.getById(borrower.CategoryId);

            Borrow.renewLoan(barcode, category.Period);
            Session["RenewLoanSuccess"] = true;

            return RedirectToAction("Index");
        }

        // Action to update a borrower
        public ActionResult Update(int id)
        {
            return View();
        }
    }
}
