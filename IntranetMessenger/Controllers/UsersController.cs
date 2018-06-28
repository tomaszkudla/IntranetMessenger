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
    public class UsersController : Controller
    {
        private TestEntities db = new TestEntities();

        // GET: Users
        public ActionResult Index()
        {
            var UsersShow = from u in db.Users select new UserShow { ID = u.ID, Name = u.Name };
            IEnumerable<UserShow> UsersList = UsersShow.ToList();
            return View(UsersList);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Password,ConfirmPassword")] User user)
        {
            if (ModelState.IsValid)
            {
                //db.Users.Add(user);

                byte[] bHash = SHA256.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(user.Password));
                string sHash = BitConverter.ToString(bHash).Replace("-", String.Empty);
                user.Password = String.Empty;
                db.spAddUser(user.Name, sHash);
                db.SaveChanges();

                ActiveUser.ID = user.ID;
                ActiveUser.Name = user.Name;
                ActiveUser.Hash = user.Hash;
                return Redirect("~/Users/Index");

              
            }

            return View(user);
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

                if (FindUser != null && FindUser.Hash == sHash)
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

        // GET: Logging/Log
        public ActionResult LogOut()
        {
            ActiveUser.Name = "";
            ActiveUser.ID = 0;
            ActiveUser.Hash = "";

            return Redirect("~/Home/Index");
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
