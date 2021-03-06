﻿using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BsuirHealthProjectServer.Models;
using BsuirHealthProjectServer.Models.DatabaseModels;
using BsuirHealthProjectServer.Services;
using Microsoft.AspNet.Identity.Owin;

namespace BsuirHealthProjectServer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DishCategoriesController : Controller
    {
        public ApplicationDbContext Context
        {
            get
            {
                return Request.GetOwinContext().Get<ApplicationDbContext>();
            }
        }
        private DishCategoryService dishCategoryService;
        public DishCategoryService DishCategoryService
        {
            get { return dishCategoryService ?? (dishCategoryService = new DishCategoryService(Context)); }
            set { dishCategoryService = value; }
        }

        // GET: DishCategories
        public ActionResult Index()
        {
            return View(DishCategoryService.Get().ToList());
        }

        // GET: DishCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishCategory dishCategory = DishCategoryService.Get((int) id);
            if (dishCategory == null)
            {
                return HttpNotFound();
            }
            return View(dishCategory);
        }

        // GET: DishCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DishCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Image,IsDeleted,DateModification")] DishCategory dishCategory)
        {
            if (ModelState.IsValid)
            {
                DishCategoryService.Add(dishCategory);
                return RedirectToAction("Index");
            }

            return View(dishCategory);
        }

        // GET: DishCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishCategory dishCategory = DishCategoryService.Get((int) id);
            if (dishCategory == null)
            {
                return HttpNotFound();
            }
            return View(dishCategory);
        }

        // POST: DishCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Image,IsDeleted,DateModification")] DishCategory dishCategory)
        {
            if (ModelState.IsValid)
            {
                DishCategoryService.Update(dishCategory.Id, dishCategory);
                return RedirectToAction("Index");
            }
            return View(dishCategory);
        }

        // GET: DishCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishCategory dishCategory = DishCategoryService.Get((int) id);
            if (dishCategory == null)
            {
                return HttpNotFound();
            }
            return View(dishCategory);
        }

        // POST: DishCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DishCategoryService.Remove(id);
            return RedirectToAction("Index");
        }
    }
}
