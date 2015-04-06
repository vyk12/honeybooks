using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using System.Web.Script.Serialization;

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

            try
            {
                ViewBag.authors = Author.getAll();
            }
            catch
            {
                return View("Error500");
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
                ViewBag.authors = Author.getAll();
            }
            catch
            {
                return View("Error500");
            }

            try
            {
                // Let's create a new book and fill it with form's data
                Book book = new Book();

                book.Title = collection["Title"] as string;
                book.ISBN = collection["ISBN"] as string;
                book.Pages = Convert.ToInt32(collection["Pages"]);
                book.SignId = Convert.ToInt32(collection["SignId"]);
                book.PublicationInfo = collection["PublicationInfo"] as string;
                book.PublicationYear = collection["PublicationYear"] as string;

                book.create();

                // The link between the book and its author mus be created
                BL.BookAuthor.create(book.ISBN, Convert.ToInt32(collection["AuthorId"]));

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Browse()
        {
            string sortBy = Request.QueryString.Get("sortby");

            try
            {
                // Let's fetch all books that match the query
                ViewBag.books = Book.getAll(sortBy == "author" ? 0 : 1);
            }
            catch
            {
                return View("Error500");
            }

            return View();
        }

        
        public ActionResult Delete(string isbn)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Pages");
            }

            try
            {
                // Let's get the book first
                Book book = Book.getByISBN(isbn);

                // Let's delete the link between the book and its author(s)
                BookAuthor.delete(book.ISBN);
                
                // And finally, let's delete the book
                book.delete();
            }
            catch
            {
                return View("Error500");
            }

            return RedirectToAction("List");
        }

        
        public ActionResult Details(string isbn)
        {
            try
            {
                ViewBag.book = Book.getByISBN(isbn);
            }
            catch
            {
                return View("Error500");
            }

            return View();
        }


        public ActionResult Edit(string isbn)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Pages");
            }

            Book book;

            try
            {
                book = BL.Book.getByISBN(isbn);

                ViewBag.authors = Author.getAll();
                ViewBag.AuthorId = BookAuthor.getAuthorId(isbn);
            }
            catch
            {
                return View("Error500");
            }

            return View(book);
        }


        [HttpPost]
        public ActionResult Edit(string isbn, FormCollection collection)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Pages");
            }

            try
            {
                ViewBag.authors = Author.getAll();
                ViewBag.AuthorId = BookAuthor.getAuthorId(isbn);
            }
            catch
            {
                return View("Error500");
            }

            try
            {
                // Let's create a new book and fill it with the form's data
                BL.Book book = BL.Book.getByISBN(isbn);

                book.Title = collection["Title"] as string;
                book.Pages = Convert.ToInt32(collection["Pages"]);
                book.SignId = Convert.ToInt32(collection["SignId"]);
                book.PublicationInfo = collection["PublicationInfo"] as string;
                book.PublicationYear = collection["PublicationYear"] as string;

                book.edit();

                // Let's also update the link between the book and its author
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
            List<Book> books;

            try
            {
                books = BL.Book.getAll();
            }
            catch
            {
                return View("Error500");
            }

            return View(books);
        }

        
        public ActionResult Search()
        {
            string query = Request.QueryString.Get("query");

            if (query != null)
            {
                int JSON = Convert.ToInt32(Request.QueryString.Get("json"));
                int searchBy = Convert.ToInt32(Request.QueryString.Get("searchBy"));

                // If the searchBy parameter is invalid, let's assign 0 to it
                if (searchBy != 0 && searchBy != 1) searchBy = 0;

                List<Book> books;

                try
                {
                    books = Book.search(query, searchBy);
                }
                catch
                {
                    return View("Error500");
                }

                ViewBag.books = books;

                // If the client wants a JSON formated output
                if (JSON == 1)
                {
                    ViewBag.padding = Convert.ToInt32(Request.QueryString.Get("padding"));
                    ViewBag.JSON = new JavaScriptSerializer().Serialize(books);
                    return View("JSONSearch");
                }
            }

            return View();
        }
    }
}
