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
    public class FoodConsistencyTypesController : Controller
    {
        

        public ApplicationDbContext Context
        {
            get
            {
                return Request.GetOwinContext().Get<ApplicationDbContext>();
            }
        }
        private FoodConsistencyTypeService foodConsistencyTypeService;
        public FoodConsistencyTypeService FoodConsTypeService
        {
            get { return foodConsistencyTypeService ?? (foodConsistencyTypeService = new FoodConsistencyTypeService(Context)); }
            set { foodConsistencyTypeService = value; }
        }
        // GET: FoodConsistencyTypes
        public ActionResult Index()
        {
            return View(FoodConsTypeService.Get().ToList());
        }

        // GET: FoodConsistencyTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodConsistencyType foodConsistencyType = FoodConsTypeService.Get((int) id);
            if (foodConsistencyType == null)
            {
                return HttpNotFound();
            }
            return View(foodConsistencyType);
        }

        // GET: FoodConsistencyTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FoodConsistencyTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DateModification")] FoodConsistencyType foodConsistencyType)
        {
            if (ModelState.IsValid)
            {
                FoodConsTypeService.Add(foodConsistencyType);
                return RedirectToAction("Index");
            }

            return View(foodConsistencyType);
        }

        // GET: FoodConsistencyTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodConsistencyType foodConsistencyType = FoodConsTypeService.Get((int) id);
            if (foodConsistencyType == null)
            {
                return HttpNotFound();
            }
            return View(foodConsistencyType);
        }

        // POST: FoodConsistencyTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DateModification")] FoodConsistencyType foodConsistencyType)
        {
            if (ModelState.IsValid)
            {
                FoodConsTypeService.Update(foodConsistencyType.Id, foodConsistencyType);
                return RedirectToAction("Index");
            }
            return View(foodConsistencyType);
        }

        // GET: FoodConsistencyTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodConsistencyType foodConsistencyType = FoodConsTypeService.Get((int) id);
            if (foodConsistencyType == null)
            {
                return HttpNotFound();
            }
            return View(foodConsistencyType);
        }

        // POST: FoodConsistencyTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FoodConsistencyType foodConsistencyType = FoodConsTypeService.Get(id);
            FoodConsTypeService.Remove(foodConsistencyType);
            return RedirectToAction("Index");
        }
    }
}
