using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BsuirHealthProjectServer.Models;
using BsuirHealthProjectServer.Models.DatabaseModels;
using BsuirHealthProjectServer.Services;
using Microsoft.AspNet.Identity.Owin;

namespace BsuirHealthProjectServer.Controllers
{
    public class FoodCategoriesController : Controller
    {
        public ApplicationDbContext Context
        {
            get
            {
                return Request.GetOwinContext().Get<ApplicationDbContext>();
            }
        }
        private FoodCategoryService foodCategoryService;
        public FoodCategoryService FoodCategoryService
        {
            get { return foodCategoryService ?? (foodCategoryService = new FoodCategoryService(Context)); }
            set { foodCategoryService = value; }
        }


        // GET: FoodCategories
        public ActionResult Index()
        {
            return View(FoodCategoryService.Get().ToList());
        }

        // GET: FoodCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodCategory foodCategory = FoodCategoryService.Get((int) id);
            if (foodCategory == null)
            {
                return HttpNotFound();
            }
            return View(foodCategory);
        }

        // GET: FoodCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FoodCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Image,IsDeleted,DateModification")] FoodCategory foodCategory)
        {
            if (ModelState.IsValid)
            {
                FoodCategoryService.Add(foodCategory);
                return RedirectToAction("Index");
            }

            return View(foodCategory);
        }

        // GET: FoodCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodCategory foodCategory = FoodCategoryService.Get((int) id);
            if (foodCategory == null)
            {
                return HttpNotFound();
            }
            return View(foodCategory);
        }

        // POST: FoodCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Image,IsDeleted,DateModification")] FoodCategory foodCategory)
        {
            if (ModelState.IsValid)
            {
                FoodCategoryService.Update(foodCategory.Id, foodCategory);
                return RedirectToAction("Index");
            }
            return View(foodCategory);
        }

        // GET: FoodCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodCategory foodCategory = FoodCategoryService.Get((int) id);
            if (foodCategory == null)
            {
                return HttpNotFound();
            }
            return View(foodCategory);
        }

        // POST: FoodCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FoodCategory foodCategory = FoodCategoryService.Get(id);
            FoodCategoryService.Remove(foodCategory);
            return RedirectToAction("Index");
        }
    }
}
