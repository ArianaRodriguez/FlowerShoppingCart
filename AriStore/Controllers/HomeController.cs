using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AriStore.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Index view
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Index()
        {
            ViewBag.Message = "Xochimilco";         

            return View();
        }
        /// <summary>
        /// About view
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult About()
        {
            ViewBag.Message = "We send your flowers wherever you want!";

            return View();
        }
        /// <summary>
        /// Contact view
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Contact()
        {
            return View();
        }
    }
}
