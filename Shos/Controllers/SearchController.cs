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
            Engine searchEngine = new Engine();

            List<Item> searchObject = searchEngine.Search3(searchString);

            return View(searchObject);
        }
    }
}