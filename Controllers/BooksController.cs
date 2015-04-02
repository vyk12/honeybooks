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
        public ActionResult Create()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Pages");
            }

            ViewBag.authors = Author.getAll();

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

            ViewBag.authors = Author.getAll();

            try
            {
                Book book = new Book();

                book.Title = collection["Title"] as string;
                book.ISBN = collection["ISBN"] as string;
                book.Pages = Convert.ToInt32(collection["Pages"]);
                book.SignId = Convert.ToInt32(collection["SignId"]);
                book.PublicationInfo = collection["PublicationInfo"] as string;
                book.PublicationYear = collection["PublicationYear"] as string;

                book.create();

                BL.BookAuthor.create(book.ISBN, Convert.ToInt32(collection["AuthorId"]));

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // Action to browse books (either by title or author)
        public ActionResult Browse()
        {
            string sortBy = Request.QueryString.Get("sortby");

            ViewBag.books = Book.getAll(sortBy == "author" ? 0 : 1);

            return View();
        }

        // Action to delete a book
        public ActionResult Delete(int id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Pages");
            }

            return RedirectToAction("List");
        }

        // Action to get details about a book
        public ActionResult Details(string isbn)
        {
            ViewBag.book = Book.getByISBN(isbn);
            return View();
        }

        //
        // GET: /Authors/Edit/5

        public ActionResult Edit(string isbn)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Pages");
            }

            ViewBag.authors = Author.getAll();
            ViewBag.AuthorId = BookAuthor.getAuthorId(isbn);

            return View(BL.Book.getByISBN(isbn));
        }

        //
        // POST: /Authors/Edit/5

        [HttpPost]
        public ActionResult Edit(string isbn, FormCollection collection)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Pages");
            }

            ViewBag.authors = Author.getAll();
            ViewBag.AuthorId = BookAuthor.getAuthorId(isbn);

            try
            {
                BL.Book book = BL.Book.getByISBN(isbn);

                book.Title = collection["Title"] as string;
                book.Pages = Convert.ToInt32(collection["Pages"]);
                book.SignId = Convert.ToInt32(collection["SignId"]);
                book.PublicationInfo = collection["PublicationInfo"] as string;
                book.PublicationYear = collection["PublicationYear"] as string;

                book.edit();

                BL.BookAuthor.edit(book.ISBN, Convert.ToInt32(collection["AuthorId"]));

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View(BL.Book.getAll());
        }

        // Action to look for a book (either by title or author)
        public ActionResult Search()
        {
            string query = Request.QueryString.Get("query");

            if (query != null)
            {
                int searchBy = Convert.ToInt32(Request.QueryString.Get("searchBy"));

                if (searchBy != 0 && searchBy != 1) searchBy = 0;

                ViewBag.books = Book.search(query, searchBy);
            }

            return View();
        }

        // Action to fetch books according to the search query
        public ActionResult SearchResults()
        {
            return View("Search");
        }
    }
}
