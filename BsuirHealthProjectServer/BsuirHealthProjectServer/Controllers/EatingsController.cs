using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BsuirHealthProjectServer.Models;
using BsuirHealthProjectServer.Models.DatabaseModels;
using BsuirHealthProjectServer.Models.ViewModels;
using BsuirHealthProjectServer.Services;
using Microsoft.AspNet.Identity.Owin;

namespace BsuirHealthProjectServer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EatingsController : Controller
    {
        private EatingService eatingService;
        public EatingService EatingService
        {
            get { return eatingService ?? (eatingService = new EatingService(Context)); }
            set { eatingService = value; }
        }
        public ApplicationDbContext Context
        {
            get
            {
                return Request.GetOwinContext().Get<ApplicationDbContext>();
            }
        }
       

        // GET: Eatings
        public ActionResult Index()
        {
            var eating = EatingService.Get().Include(e => e.User).ToList();
            return View(eating.ToList());
        }

        public ActionResult Search(string name)
        {
            var model = EatingService.Get()
                .Where(e => e.User.FirstName.Contains(name)
                            || e.User.LastName.Contains(name)
                            || e.User.UserCredential.UserName.Contains(name)
                            || e.Id.ToString() == name)
                .OrderByDescending(e => e.Date).ToList();
            return View("Index", model);
        }

        // GET: Eatings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eating eating = EatingService.Get(id.Value);
            if (eating == null)
            {
                return HttpNotFound();
            }
            return View(eating);
        }

        // GET: Eatings/Create
        public ActionResult Create()
        {
            ViewBag.IdUser = GetUserSelectList(null);
            return View();
        }

        // POST: Eatings/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdUser,Date,Proteins,Fat,Carbs,Ccal,Sugar,AmountOfWater")] Eating eating)
        {
            if (ModelState.IsValid)
            {
                EatingService.Add(eating);
                return RedirectToAction("Index");
            }

            ViewBag.IdUser = GetUserSelectList(eating.IdUser);
            return View(eating);
        }

        // GET: Eatings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eating eating = EatingService.Get(id.Value);
            if (eating == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdUser = GetUserSelectList(eating.IdUser);
           
            return View(eating);
        }

        private SelectList GetUserSelectList(int? eatingId)
        {
            return eatingId != null 
                ? new SelectList(
                Context.User.Select(user => new UserIdFullNameProjection
                {
                    Id = user.Id,
                    FullName = "#" + user.Id + " " + user.FirstName + " " + user.LastName
                }), 
                "Id", 
                "FullName",
                eatingId)
                : new SelectList(
                Context.User.Select(user => new UserIdFullNameProjection
                {
                    Id = user.Id,
                    FullName = "#" + user.Id + " " + user.FirstName + " " + user.LastName
                }),
                "Id",
                "FullName");
        }

        // POST: Eatings/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdUser,Date,Proteins,Fat,Carbs,Ccal,Sugar,AmountOfWater")] Eating eating)
        {
            if (ModelState.IsValid)
            {
                EatingService.Update(eating.Id, eating);
                return RedirectToAction("Index");
            }
            ViewBag.IdUser = GetUserSelectList(eating.IdUser);
            return View(eating);
        }

        // GET: Eatings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eating eating = EatingService.Get(id.Value);
            if (eating == null)
            {
                return HttpNotFound();
            }
            return View(eating);
        }

        // POST: Eatings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EatingService.Remove(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}
