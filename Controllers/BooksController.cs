using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoneyBooks.Controllers
{
    public class BooksController : Controller
    {
        public ActionResult Browse()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

        public ActionResult SearchResults()
        {
            return View("Search");
        }
    }
}
