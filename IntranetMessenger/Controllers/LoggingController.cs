using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IntranetMessenger.Models;
using System.Security.Cryptography;

namespace IntranetMessenger.Controllers
{
    public class LoggingController : Controller
    {
        private TestEntities db = new TestEntities();
        

        // GET: Logging
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Logging/Log
        public ActionResult Log()
        {
            return View();
        }

        // POST: Logging/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Log([Bind(Include = "Name,Password")] UserLog userLog)
        {
            if (ModelState.IsValid)
            {
                byte[] bHash = SHA256.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(userLog.Password));
                string sHash = BitConverter.ToString(bHash).Replace("-", String.Empty);
                userLog.Password = String.Empty;
                User FindUser = db.Users.Single(u => u.Name == userLog.Name);
                
                if (FindUser != null && FindUser.Hash==sHash)
                {
                    ActiveUser.ID = FindUser.ID;
                    ActiveUser.Name = FindUser.Name;
                    ActiveUser.Hash = FindUser.Hash;
                    return Redirect("~/Users/Index");
                }
                return RedirectToAction("Index");
            }

            return View(userLog);
        }

     

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
