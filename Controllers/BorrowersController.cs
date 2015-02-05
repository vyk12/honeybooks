using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            return View();
        }

        // Action to list borrowed books
        public ActionResult List()
        {
            return View();
        }

        // Action to renew a loan
        public ActionResult RenewLoan()
        {
            return View();
        }

        // Action to update a borrower
        public ActionResult Update(int id)
        {
            return View();
        }
    }
}
