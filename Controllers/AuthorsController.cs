using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoneyBooks.Controllers
{
    public class AuthorsController : Controller
    {
        // Action to add an author
        public ActionResult Add()
        {
            return View();
        }

        // Action to delete an author
        public void Delete(int id)
        {
            
        }

        // Action to list authors
        public ActionResult List()
        {
            return View();
        }

        // Action to update an author
        public ActionResult Update(int id)
        {
            ViewBag.id = id;
            return View();
        }
    }
}
