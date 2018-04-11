using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BsuirHealthProjectServer.Models;
using BsuirHealthProjectServer.Models.DatabaseModels;
using BsuirHealthProjectServer.Services;
using BsuirHealthProjectServer.Shared;
using Microsoft.AspNet.Identity.Owin;

namespace BsuirHealthProjectServer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DishController : Controller
    {
        private DishService dishService;
        private FoodService foodService;
        private PortionFoodService portionFoodService;
        private DishCategoryService dishCategoryService;

        public DishService DishService
        {
            get { return dishService ?? (dishService = new DishService(Context)); }
            set { dishService = value; }
        }
        public FoodService FoodService
        {
            get { return foodService ?? (foodService = new FoodService(Context)); }
            set { foodService = value; }
        }
        public PortionFoodService PortionFoodService
        {
            get { return portionFoodService ?? (portionFoodService = new PortionFoodService(Context)); }
            set { portionFoodService = value; }
        }
        public DishCategoryService DishCategoryService
        {
            get { return dishCategoryService ?? (dishCategoryService = new DishCategoryService(Context)); }
            set { dishCategoryService = value; }
        }

        public IEnumerable<Dish> Model { get; set; }

        public ApplicationDbContext Context
        {
            get
            {
                return Request.GetOwinContext().Get<ApplicationDbContext>();
            }
        }
        
        public ActionResult Index()
        {
            Model = DishService.Get().ToList();
            return View(Model);
        }
        public ActionResult Search(string name)
        {
            var model = DishService.Get()
                .Where(e => e.User.FirstName.Contains(name)
                            || e.User.LastName.Contains(name)
                            || e.User.UserCredential.UserName.Contains(name)
                            || e.Id.ToString() == name
                            || e.DishCategory.Name.Contains(name)
                            || e.Name.Contains(name)).ToList();
            return View("Index", model);
        }

        public ActionResult Create()
        {
            Dish newDishToAdd = new Dish();
            ViewBag.IdDishCategory = new SelectList(Context.DishCategory, "Id", "Name");
            return View(newDishToAdd);
        }

        [HttpPost]
        public ActionResult Create(Dish dish)
        {
            string errorDetails = "";

            if (ModelState.IsValid)
            {
                ValidationResult result = DishService.Add(dish);
                if (result.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                
                errorDetails = result.GetAllErrors();
            }

            throw new HttpException(404, "Model you add is incorrect." + errorDetails);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dishToDel = DishService.Get((int) id);
            return View(dishToDel);
        }

        public ActionResult RemoveDish(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishService.Remove((int) id);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dish = DishService.Get(id.Value);
            //List<String> strings = new List<String>();
            SelectList list = new SelectList(DishCategoryService.Get(), "Id", "Name");
            list.First(i =>
            {
                // strings.Add(i.Text);
                return i.Value == dish.IdDishCategory.ToString(); 
            }).Selected = true;
            ViewBag.IdDishCategory = list;
            ViewBag.DishToView = dish;

            return View(dish.PortionFood);
        }

        [HttpPost]
        public ActionResult Edit(int Id, string Name, string Description, float? TotalAmountWater, float TotalCarbs,
                                 float TotalCcal, float TotalFat, float TotalProteins, float? TotalSugar, int IdDishCategory)
        {
            Dish item = new Dish
            {
                Id = Id,
                Name = Name,
                Description = Description,
                TotalAmountWater = TotalAmountWater,
                TotalCarbs = TotalCarbs,
                TotalCcal = TotalCcal,
                TotalFat = TotalFat,
                TotalProteins = TotalProteins,
                TotalSugar = TotalSugar,
                IdDishCategory = IdDishCategory
            };

            DishService.Update(Id, item);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult AddPortionFood(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<int> usedInDishFoodIds = DishService.Get(id.Value).PortionFood.
                Where(p => !p.IsDeleted).Select(pf => pf.Food.Id).ToList();
            ViewBag.IdFood = new SelectList(
                FoodService.Get().Where(x => !usedInDishFoodIds.Contains(x.Id)),
                "Id", "Name");
            ViewBag.IdDish = id;

            PortionFood portion = new PortionFood();
            return View(portion);
        }

        [HttpPost]
        public ActionResult AddPortionFood([Bind(Include = "IdFood, Amount")]PortionFood portion)
        {
            int idDish = int.Parse(RouteData.Values["Id"].ToString());

            if (ModelState.IsValid)
            {
                portion.IdDish = idDish;

                ValidationResult result = PortionFoodService.Add(portion);
                if (!result.IsSuccess)
                {
                    var errorDetails = result.GetAllErrors();
                    throw new HttpException(404, "Model you add is incorrect. " + errorDetails);
                }
            }

            List<int> usedInDishFoodIds = DishService.Get(idDish).PortionFood.
                Where(p => !p.IsDeleted).Select(pf => pf.Food.Id).ToList();
            ViewBag.IdFood = new SelectList(
                FoodService.Get().Where(x => !usedInDishFoodIds.Contains(x.Id)),
                "Id", "Name");
            ViewBag.IdDish = idDish;

            return View();
        }

        [HttpGet]
        public ActionResult EditPortionFood(int? foodId, int? dishId)
        {
            if (foodId == null || dishId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortionFood portionFood = PortionFoodService.Get(foodId.Value, dishId.Value);
            if (portionFood == null)
            {
                return HttpNotFound();
            }

            List<int> usedInDishFoodIds = DishService.Get(dishId.Value).PortionFood.
                Where(p => !p.IsDeleted).Select(pf => pf.Food.Id).ToList();
            ViewBag.IdFood = new SelectList(
                FoodService.Get().Where(f => !usedInDishFoodIds.Contains(f.Id) || f.Id == foodId.Value),
                "Id", "Name");
            ViewBag.IdDish = dishId.Value;
            return View(portionFood);
        }

        // POST: PortionFoods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPortionFood([Bind(Include = "IdFood,IdDish,Amount")] PortionFood portionFood)
        {
            if (ModelState.IsValid)
            {
                PortionFoodService.Update(portionFood.IdFood, portionFood.IdDish, portionFood);
                return RedirectToAction("Edit", new RouteValueDictionary { { "id", portionFood .IdDish} });
            }

            List<int> usedInDishFoodIds = DishService.Get(portionFood.IdDish).PortionFood.
                Where(p => !p.IsDeleted).Select(pf => pf.Food.Id).ToList();
            ViewBag.IdFood = new SelectList(
                FoodService.Get().Where(x => !usedInDishFoodIds.Contains(x.Id)),
                "Id", "Name");
            ViewBag.IdDish = portionFood.IdDish;

            return View(portionFood);
        }

        // GET: PortionFoods/Delete/5
        [HttpGet]
        public ActionResult DeletePortionFood(int? foodId, int? dishId)
        {
            if (foodId == null || dishId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortionFood portionFood = PortionFoodService.Get(foodId.Value, dishId.Value);
            if (portionFood == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdDish = dishId.Value;

            return View(portionFood);
        }

        // POST: PortionFoods/Delete/5
        [HttpPost, ActionName("DeletePortionFood")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedPortionFood(int foodId, int dishId)
        {
            PortionFoodService.Remove(foodId, dishId);
            return RedirectToAction("Edit", new RouteValueDictionary {{"id", dishId}});
        }

        public ActionResult DetailsPortionFood(int? foodId, int? dishId)
        {
            if (foodId == null || dishId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortionFood portionFood = PortionFoodService.Get(foodId.Value, dishId.Value);
            if (portionFood == null)
            {
                return HttpNotFound();
            }
            return View(portionFood);
        }
    }

    /*
    class FoodEqualityComparer : IEqualityComparer<Food>
    {
        public bool Equals(Food x, Food y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Food obj)
        {
            return obj.Id;
        }
    }*/

}