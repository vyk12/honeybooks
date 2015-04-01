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
            return View(BL.Author.getAll());
        }

        //
        // GET: /Authors/Create

        public ActionResult Create()
        {

            return View();
        }

        //
        // POST: /Authors/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
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

        //
        // GET: /Authors/Details/5

        public ActionResult Details(int id)
        {
            return View(BL.Author.getByAid(id));
        }

        //
        // GET: /Authors/Edit/5

        public ActionResult Edit(int id)
        {
            return View(BL.Author.getByAid(id));
        }

        //
        // POST: /Authors/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                BL.Author author = BL.Author.getByAid(id);
                author.About = collection["About"];
                author.BirthYear = Convert.ToInt32(collection["BirthYear"]);
                author.FirstName = collection["FirstName"];
                author.LastName = collection["LastName"];
                author.save();
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Authors/Delete/5

        public ActionResult Delete(int id)
        {
            BL.Author author = BL.Author.getByAid(id);

            List<BL.Book> books = author.getBooks();

            author.delete();

            foreach (var book in books)
            {
                if (book.onlyBelongsTo(author.Aid))
                {
                    book.delete();
                }
            }

            return RedirectToAction("Index");
        }

    }
}
