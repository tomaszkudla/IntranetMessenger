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
            if (Session["Name"]!=null)
            {
                var UsersShow = from u in db.Users select new UserShow { ID = u.ID, Name = u.Name };
                IEnumerable<UserShow> UsersList = UsersShow.ToList();
                return View(UsersList);
            }
            else
            {
                return RedirectToAction("~/Home/Index");
            }

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
        public ActionResult Create([Bind(Include = "Name,Password,ConfirmPassword")] UserRegister userRegister)
        {
            if (ModelState.IsValid)
            {
                //db.Users.Add(user);
                Session["alert"] = "";
                if (db.Users.Any(n => n.Name == userRegister.Name))
                {
                    Session["alert"] = "Name already exists";
                    return View("Create", userRegister);
                }
                byte[] bHash = SHA256.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(userRegister.Password));
                string sHash = BitConverter.ToString(bHash).Replace("-", String.Empty);
                userRegister.Password = String.Empty;
                db.spAddUser(userRegister.Name, sHash);
                db.SaveChanges();

                Session["Name"] = userRegister.Name;
                
                return Redirect("~/Users/Index");

              
            }

            return View(userRegister);
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
                Session["alert"] = "";
                byte[] bHash = SHA256.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(userLog.Password));
                string sHash = BitConverter.ToString(bHash).Replace("-", String.Empty);
                userLog.Password = String.Empty;
                User FindUser = db.Users.SingleOrDefault(u => u.Name == userLog.Name);

                if (FindUser != null && FindUser.Hash == sHash)
                {
                    
                    Session["Name"] = FindUser.Name;
                    
                    return Redirect("~/Messages/Index");
                }
                else
                {
                    Session["alert"] = "Name or password is incorrect";
                    return RedirectToAction("Log");
                }
                
            }
            return View(userLog);
        }

        // GET: Logging/Log
        public ActionResult LogOut()
        {
            Session["Name"] = "";
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
