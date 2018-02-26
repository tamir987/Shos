using Models;
using SearchEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shos.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetItems(String searchString)
        {
            List<Item> searchObject = Engine.Search(searchString);
            // return View(searchString as object);
            return View(searchObject);
        }
    }
}