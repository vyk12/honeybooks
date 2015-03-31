using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;

namespace HoneyBooks.Controllers
{
    public class BooksController : Controller
    {
        // Action to add a book
        public ActionResult Add()
        {
            return View();
        }

        // Action to browse books (either by title or author)
        public ActionResult Browse()
        {
            return View();
        }

        // Action to delete a book
        public void Delete(int id)
        {

        }

        // Action to get details about a book
        public ActionResult Details(int id)
        {
            ViewBag.id = id;
            return View();
        }


        public ActionResult Index()
        {
            ViewBag.books = Book.getAll();

            return View();
        }

        // Action to look for a book (either by title or author)
        public ActionResult Search()
        {
            int searchBy = Convert.ToInt32(Request.QueryString.Get("searchBy"));

            if (searchBy != 0 && searchBy != 1) searchBy = 0;

            ViewBag.books = Book.search(Request.QueryString.Get("query"), searchBy);

            return View();
        }

        // Action to fetch books according to the search query
        public ActionResult SearchResults()
        {
            return View("Search");
        }

        // Action to update a book
        public ActionResult Update()
        {
            return View();
        }
    }
}
