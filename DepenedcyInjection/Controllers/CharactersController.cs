using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DependencyInjection.Models;
using DepenedcyInjection.Repositories;
using Domain;

namespace DepenedcyInjection.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CharactersController : Controller
    {
        private IRepository<Character> db;

        public CharactersController(IRepository<Character> db)
        {
            this.db = db;
        }

        // GET: Characters
        public ActionResult Index()
        {
            return View(db.Items.ToList());
        }

        // GET: Characters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Character character = db.Items.FirstOrDefault(x => x.Id == id);
            if (character == null)
            {
                return HttpNotFound();
            }
            return View(character);
        }

        // GET: Characters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Characters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Gender,Born,Died,Cost")] Character character)
        {
            if (ModelState.IsValid)
            {
                db.Add(character);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(character);
        }

        // GET: Characters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Character character = db.Items.FirstOrDefault(x => x.Id == id);
            if (character == null)
            {
                return HttpNotFound();
            }
            return View(character);
        }

        
        public ActionResult Upload(string name)
        {
            return View((object)name);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase fileData, string name)
        {
            if (fileData != null && fileData.ContentLength > 0)
            {
                string extension = Path.GetExtension(fileData.FileName);

                if (extension != ".jpg")
                    return new HttpStatusCodeResult(HttpStatusCode.NotImplemented);

                var renamedImage = Server.MapPath($"~/Content/Images/{name.Replace(" ", "")}.jpg");

                fileData.SaveAs(renamedImage);
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Characters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Gender,Born,Died,Cost")] Character character)
        {
            if (ModelState.IsValid)
            {
                var oldPath = Server.MapPath($"~/Content/Images/{db.Items.First(x => x.Id == character.Id).Name.Replace(" ", "")}.jpg");
                db.Update(character);
                db.SaveChanges();
                
                var imgPath = Server.MapPath($"~/Content/Images/{character.Name.Replace(" ", "")}.jpg");
                System.IO.File.Move(oldPath, imgPath);

                return RedirectToAction("Index");
            }
            return View(character);
        }

        // GET: Characters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Character character = db.Items.FirstOrDefault(x => x.Id == id);
            if (character == null)
            {
                return HttpNotFound();
            }
            return View(character);
        }

        // POST: Characters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Character character = db.Items.FirstOrDefault(x => x.Id == id);
            db.Remove(character);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
