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
            if (Session["Name"]!=null)
            {
                var MessageList = db.spShowMessages(Session["Name"].ToString()).ToList();
                var MessageList2 = from m in MessageList select new Message { ID = m.ID, Reciever = m.Reciever, Sender = m.Sender, MessageText = m.MessageText, SendTime = m.SendTime };
                return View(MessageList2);
            }
            else
            {
                return Redirect("~/Home/Index");
            }

        }

        // GET: Messages/Create
        public ActionResult Create(string reciever)
        {
            if (Session["Name"]!=null)
            {
                return View(new MessageTextInput { MessageText = "", Reciever = reciever });
            }
            else
            {
                return Redirect("~/Home/Index");
            }
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Reciever,MessageText")] MessageTextInput messageTextInput)
        {
            if (ModelState.IsValid)
            {
                db.spSendMessage(Session["Name"].ToString(), messageTextInput.Reciever, messageTextInput.MessageText, DateTime.Now);
                return RedirectToAction("Index");
            }
            return View(messageTextInput);
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
