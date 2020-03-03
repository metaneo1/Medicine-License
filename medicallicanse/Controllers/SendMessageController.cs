using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace medicallicanse.Controllers
{
    public class SendMessageController : Controller
    {
        // GET: SendMessage
        public ActionResult Index()
        {
            return View();
        }

        // GET: SendMessage/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SendMessage/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SendMessage/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SendMessage/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SendMessage/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SendMessage/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SendMessage/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
