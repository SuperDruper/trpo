using BsuirHealthProjectServer.Models;
using BsuirHealthProjectServer.Models.ViewModels;
using BsuirHealthProjectServer.Services;
using BsuirHealthProjectServer.Shared;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BsuirHealthProjectServer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FoodCategoryController : Controller
    {
        private const int IMAGE_WIDTH = 64;
        private const int IMAGE_HEIGHT = 64;

        public FoodCategoryService FoodCategoryService { get; private set; }
        public ApplicationDbContext Context { get; private set; }

        public FoodCategoryController()
        {
            Context = Request != null ? Request.GetOwinContext().Get<ApplicationDbContext>() : ApplicationDbContext.Create();
            FoodCategoryService = new FoodCategoryService(Context);
        }

        // GET: FoodCategory
        public ActionResult Index()
        {
            IEnumerable<FoodCategoryViewModel> model = FoodCategoryService.Get()
                .Select(fc => new FoodCategoryViewModel {
                    Id = fc.Id,
                    DateModification = fc.DateModification,
                    Image = fc.Image,
                    IsDeleted = fc.IsDeleted,
                    Name = fc.Name,
                }).ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Exclude = "image")]FoodCategoryViewModel model, HttpPostedFileBase image)
        {
            if (image == null)
                ModelState.AddModelError("Image", "Please, choose image.");
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ValidationResult valRes = FoodCategoryService.Add(new Models.DatabaseModels.FoodCategory
            {
                Name = model.Name,
                DateModification = DateTime.Now,
                IsDeleted = false,
                Image = ImageEditor.GetResizedImage(image, IMAGE_WIDTH, IMAGE_HEIGHT),
            });
            if (!valRes.IsSuccess)
            {
                ModelState.AddModelError("", valRes.GetAllErrors());
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            ValidationResult valRes = FoodCategoryService.Remove(id);
            if (!valRes.IsSuccess)
            {
                return View("Error");
            }
            return RedirectToAction("Index");
        }
    }
}