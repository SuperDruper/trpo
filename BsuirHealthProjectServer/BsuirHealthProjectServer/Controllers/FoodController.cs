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
    public class FoodController : Controller
    {
        private FoodService foodService;
        public FoodService FoodService
        {
            get { return foodService ?? (foodService = new FoodService(Context)); }
            set { foodService = value; }
        }
        public ApplicationDbContext Context
        {
            get
            {
                return Request.GetOwinContext().Get<ApplicationDbContext>();
            }
        }
        // GET: Food
        public ActionResult Index()
        {
            var food = FoodService.Get().ToList();
            return View(food);
        }

        public ActionResult Search(string name)
        {
            var model = FoodService.Get()
                .Where(e => e.Name.Contains(name)
                            || e.FoodCategory.Name.Contains(name)
                            || e.Id.ToString() == name)
                .OrderByDescending(e => e.DateModification).ToList();
            return View("Index", model);
        }

        // GET: Food/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = FoodService.Get((int) id);
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // GET: Food/Create
        public ActionResult Create()
        {
            ViewBag.IdFoodCategory = new SelectList(Context.FoodCategory, "Id", "Name");
            ViewBag.IdFoodConsistencyType = new SelectList(Context.FoodConsistencyType, "Id", "Name");
            return View();
        }

        // POST: Food/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string Name,int Proteins, int Fat, int Carbs, int Ccal,int Sugar, int AmountOfWater,int IdFoodCategory,int IdFoodConsistencyType)
        {
            var toAdd = new Food
            {
                Name = Name,
                Proteins = Proteins,
                Fat = Fat,
                Carbs = Carbs,
                Ccal = Ccal,
                Sugar = Sugar,
                AmountOfWater = AmountOfWater,
                IdFoodCategory = IdFoodCategory,
                IdFoodConsistencyType = IdFoodConsistencyType,
                Image = new byte[] { 3 }
            };

            if (ModelState.IsValid)
            {
                FoodService.Add(toAdd);
                return RedirectToAction("Index");
            }

            ViewBag.IdFoodCategory = new SelectList(Context.FoodCategory, "Id", "Name", toAdd.IdFoodCategory);
            ViewBag.IdFoodConsistencyType = new SelectList(Context.FoodConsistencyType, "Id", "Name", toAdd.IdFoodConsistencyType);
            return View(toAdd);
        }

        // GET: Food/Edit/5
        public ActionResult Edit(int id)
        {
            Food food = FoodService.Get(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdFoodCategory = new SelectList(Context.FoodCategory, "Id", "Name", food.IdFoodCategory);
            ViewBag.IdFoodConsistencyType = new SelectList(Context.FoodConsistencyType, "Id", "Name", food.IdFoodConsistencyType);
            return View(food);
        }

        // POST: Food/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id, string Name, int Proteins, int Fat, int Carbs, int Ccal, int Sugar,
                        int AmountOfWater, int IdFoodCategory, int IdFoodConsistencyType)
        {
            Food food = FoodService.Get().First(x => x.Id == Id);
            //TODO Exception if there is no matching Id 
            if (ModelState.IsValid)
            {
                food.Name = Name;
                food.Proteins = Proteins;
                food.Fat = Fat;
                food.Ccal = Ccal;
                food.Carbs = Carbs;
                food.Sugar = Sugar;
                food.AmountOfWater = AmountOfWater;
                food.IdFoodCategory = IdFoodCategory;
                food.IdFoodConsistencyType = IdFoodConsistencyType;
                food.Image = new byte[] { 1 };

                FoodService.Update(Id, food);
                return RedirectToAction("Index");
            }
            ViewBag.IdFoodCategory = new SelectList(Context.FoodCategory, "Id", "Name", food.IdFoodCategory);
            ViewBag.IdFoodConsistencyType = new SelectList(Context.FoodConsistencyType, "Id", "Name", food.IdFoodConsistencyType);
            return View(food);
        }

        // GET: Food/Delete/5
        public ActionResult Delete(int id)
        {
            Food food = FoodService.Get(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // POST: Food/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Food food = FoodService.Get(id);
            food.IsDeleted = true;
            FoodService.Remove(food);
            return RedirectToAction("Index");
        }
    }
}
