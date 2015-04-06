using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoneyBooks.Controllers
{
    public class AuthorsController : Controller
    {
        public ActionResult Index()
        {
            List<BL.Author> authors;

            try
            {
                authors = BL.Author.getAll();
            }
            catch
            {
                return View("Error500");
            }

            return View(authors);
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
                // Let's create a new author and fill it with the form's data
                BL.Author author = new BL.Author();
                author.About = collection["About"];
                author.BirthYear = Convert.ToInt32(collection["BirthYear"]);
                author.FirstName = collection["FirstName"];
                author.LastName = collection["LastName"];

                author.save();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Details(int id)
        {
            BL.Author author;

            // Let's try to get the corresponding author
            try
            {
                author = BL.Author.getByAid(id);
            }
            catch
            {
                return View("Error500");
            }

            return View(author);
        }


        public ActionResult Edit(int id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Pages");
            }

            BL.Author author;

            // Let's try to get the corresponding author
            try
            {
                author = BL.Author.getByAid(id);
            }
            catch
            {
                return View("Error500");
            }

            return View(author);
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
                // Let's create a new author and fill it with the form's data
                BL.Author author = BL.Author.getByAid(id);
                author.About = collection["About"];
                author.BirthYear = Convert.ToInt32(collection["BirthYear"]);
                author.FirstName = collection["FirstName"];
                author.LastName = collection["LastName"];
                
                author.save();

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

            try
            {
                // First, let's get the author
                BL.Author author = BL.Author.getByAid(id);

                // Then, let's get all his/her books
                List<BL.Book> books = author.getBooks();

                // Let's delete every links between the author and his/her books
                BL.BookAuthor.delete(author.Aid);
                
                // Let's delete the author
                author.delete();

                foreach (var book in books)
                {
                    // If there isn't any other other attached to the book, it must be deleted
                    if (book.onlyBelongsTo(author.Aid))
                    {
                        book.delete();
                    }
                }
            }
            catch
            {
                return View("Error500");
            }

            return RedirectToAction("Index");
        }

    }
}
