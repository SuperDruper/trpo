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
    public class FoodConsistencyTypeController : Controller
    {
        public FoodConsistencyTypeService FoodConsistencyTypeService { get; private set; }
        public ApplicationDbContext Context { get; private set; }

        public FoodConsistencyTypeController()
        {
            if (Request != null)
                Context = Request.GetOwinContext().Get<ApplicationDbContext>();
            else
                Context = ApplicationDbContext.Create();
            FoodConsistencyTypeService = new FoodConsistencyTypeService(Context);
        }

        public ActionResult Index()
        {
            IEnumerable<FoodConsistencyTypeViewModel> model = FoodConsistencyTypeService.Get()
                .Select(fct => new FoodConsistencyTypeViewModel {
                    Id = fct.Id,
                    Name = fct.Name,
                    DateModification = fct.DateModification,
                }).ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FoodConsistencyTypeViewModel model)
        {            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ValidationResult valRes = FoodConsistencyTypeService.Add(new Models.DatabaseModels.FoodConsistencyType
            {
                Name = model.Name,
                DateModification = DateTime.Now,
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
            ValidationResult valRes = FoodConsistencyTypeService.Remove(id);
            if (!valRes.IsSuccess)
            {
                return View("Error");
            }
            return RedirectToAction("Index");
        }
    }
}