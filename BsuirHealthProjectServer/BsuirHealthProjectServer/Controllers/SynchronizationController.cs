using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BsuirHealthProjectServer.Services;
using BsuirHealthProjectServer.Models;
using BsuirHealthProjectServer.Models.DatabaseModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BsuirHealthProjectServer.Controllers
{
    /// <summary>
    /// SynchronizationController
    /// </summary>
    // [Authorize]
    [RoutePrefix("/api/Synchronization")]
    [Authorize]
    [HostAuthentication(DefaultAuthenticationTypes.ApplicationCookie)]
    public class SynchronizationController : ApiController
    {
        private ApplicationDbContext context;
        private FoodConsistencyTypeService foodConsistencyTypeService;
        private FoodCategoryService foodCategoryService;
        private DishCategoryService dishCategoryService;
        private FoodService foodService;
        private DishService dishService;
        private PortionFoodService portionService;

        /// <summary>
        /// Post example 
        /// </summary>
        /// <param name="startDateDecorator"></param>
        public SynchronizationData Post(DateTimeDecorator startDateDecorator)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Input data format is invalid.");
                return null;
            }
            DateTime startDate = startDateDecorator.DateTime;
            var data = new SynchronizationData();
            context = Request.GetOwinContext().Get<ApplicationDbContext>();

            foodConsistencyTypeService = new FoodConsistencyTypeService(context);
            data.FoodConsistencyTypes = foodConsistencyTypeService.Get(startDate).ToList();

            foodCategoryService = new FoodCategoryService(context);
            data.FoodCategories = foodCategoryService.Get(startDate).ToList();

            dishCategoryService = new DishCategoryService(context);
            data.DishCategories = dishCategoryService.Get(startDate).ToList();

            foodService = new FoodService(context);
            data.Food = foodService.Get(startDate).ToList();

            dishService = new DishService(context);
            data.Dishes = dishService.Get(startDate).ToList();

            portionService = new PortionFoodService(context);
            data.Portions = portionService.Get(startDate).ToList();

            return data;
        }

    }

    public class DateTimeDecorator
    {
        [Required]
        public DateTime DateTime { get; set; }
    }

    public class SynchronizationData
    {
        public List<DishCategory> DishCategories { get; set; }

        public List<FoodCategory> FoodCategories { get; set; }

        public List<FoodConsistencyType> FoodConsistencyTypes { get; set; }

        public List<Food> Food { get; set; } 

        public List<Dish> Dishes { get; set; }

        public List<PortionFood> Portions { get; set; }
    }
}
