using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IntranetMessenger.Models;

namespace IntranetMessenger.Controllers
{
    public class MessagesController : Controller
    {
        private TestEntities db = new TestEntities();

        // GET: Messages
        public ActionResult Index()
        {
            var MessageList = db.spShowMessages(ActiveUser.Name).ToList();
            var MessageList2 = from m in MessageList select new Message { ID = m.ID, Reciever = m.Reciever, Sender = m.Sender, MessageText = m.MessageText, SendTime = m.SendTime };

            return View(MessageList2);
        }

       

        // GET: Messages/Create
        public ActionResult Create(string reciever)
        {
            return View(new Message {Reciever=reciever });
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageText")] Message message, string reciever)
        {

            message.Sender = ActiveUser.Name;
            message.SendTime = DateTime.Now;
            message.Reciever = reciever;
            if (ModelState.IsValid)
            {
                db.spSendMessage(message.Sender, reciever, message.MessageText, message.SendTime);
                //db.Messages.Add(message);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(message);
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
