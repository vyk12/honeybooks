using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoneyBooks.Controllers
{
    public class AuthorsController : Controller
    {
        public ActionResult Add()
        {
            return View();
        }

        public void Delete(int id)
        {
            
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Update(int id)
        {
            ViewBag.id = id;
            return View();
        }
    }
}
