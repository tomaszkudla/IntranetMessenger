using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntranetMessenger.Models;

namespace IntranetMessenger.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["Name"] == null)
            {
                Session["Name"] = "";
            }
            Session["alert"] = "";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}