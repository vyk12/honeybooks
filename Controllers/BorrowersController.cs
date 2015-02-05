using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoneyBooks.Controllers
{
    public class BorrowersController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
