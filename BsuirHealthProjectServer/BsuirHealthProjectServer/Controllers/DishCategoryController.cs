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
    public class DishCategoryController : Controller
    {
        private const int IMAGE_WIDTH = 64;
        private const int IMAGE_HEIGHT = 64;

        public DishCategoryService DishCategoryService { get; private set; }
        public ApplicationDbContext Context { get; private set; }

        public DishCategoryController()
        {
            Context = Request != null ? Request.GetOwinContext().Get<ApplicationDbContext>() : ApplicationDbContext.Create();
            DishCategoryService = new DishCategoryService(Context);
        }

        public ActionResult Index()
        {
            IEnumerable<DishCategoryViewModel> model = DishCategoryService.Get()
                .Select(dc => new DishCategoryViewModel
                {
                    Id = dc.Id,
                    DateModification = dc.DateModification,
                    Image = dc.Image,
                    IsDeleted = dc.IsDeleted,
                    Name = dc.Name,
                }).ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Exclude = "image")]DishCategoryViewModel model, HttpPostedFileBase image)
        {
            if (image == null)
                ModelState.AddModelError("Image", "Please, choose image.");
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ValidationResult valRes = DishCategoryService.Add(new Models.DatabaseModels.DishCategory
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
            ValidationResult valRes = DishCategoryService.Remove(id);
            if (!valRes.IsSuccess)
            {
                return View("Error");
            }
            return RedirectToAction("Index");
        }
    }
}