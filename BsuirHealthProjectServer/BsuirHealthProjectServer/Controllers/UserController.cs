using BsuirHealthProjectServer.Models;
using BsuirHealthProjectServer.Models.DatabaseModels;
using BsuirHealthProjectServer.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BsuirHealthProjectServer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        public ApplicationUserManager UserManager
        {
            get
            {
                return Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        public ApplicationDbContext Context
        {
            get
            {
                return Request.GetOwinContext().Get<ApplicationDbContext>();
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
        }

        public ActionResult Index()
        {
            var model = Context.User.Select(u => new UserViewModel()
            {
                Id = u.Id,
                Email = u.UserCredential.Email,
                Phone = u.UserCredential.PhoneNumber,
                Username = u.UserCredential.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                DateOfBirth = u.DateOfBirth,
                Sex = u.Sex,
            });
            return View(model);
        }

        public ActionResult Search(string name)
        {
            var model = Context.User
                .Where(u => u.FirstName.Contains(name)
                    || u.LastName.Contains(name)
                    || u.UserCredential.Email.Contains(name)
                    || u.UserCredential.UserName.Contains(name)
                    || u.UserCredential.PhoneNumber.Contains(name))
                .Select(u => new UserViewModel()
                {
                    Id = u.Id,
                    Email = u.UserCredential.Email,
                    Phone = u.UserCredential.PhoneNumber,
                    Username = u.UserCredential.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    DateOfBirth = u.DateOfBirth,
                    Sex = u.Sex,
                }).ToList();
            return View("Index", model);
        }

        public ActionResult Admins()
        {
            List<AdminViewModel> model = new List<AdminViewModel>();
            IdentityRole role = RoleManager.FindByName("Admin");
            if (role == null)
            {
                //show error not found
                return RedirectToAction("Index");
            }
            foreach (var userRole in role.Users)
            {
                ApplicationUser user = UserManager.FindById(userRole.UserId);
                if (user != null)
                    model.Add(new AdminViewModel
                    {
                        Email = user.Email,
                        Username = user.UserName,
                        Id = user.Id,
                    });
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAdmin(CreateAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ApplicationUser newUser = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Email,
            };
            try
            {
                if (UserManager.FindByName(model.Email) != null)
                {
                    ModelState.AddModelError("", "User with such email is already exist.");
                    return View(model);
                }
                IdentityResult result = UserManager.Create(newUser, model.Password);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Error.");
                    return View(model);
                }
                UserManager.AddToRole(newUser.Id, "Admin");
            }
            catch
            {
                //logging
                ModelState.AddModelError("", "Internal server error.");
                return View(model);
            }
            return RedirectToAction("Admins");
        }

        [HttpPost]
        public ActionResult DeleteAdmin(string id)
        {
            ApplicationUser user = UserManager.FindById(id);
            if (user == null)
            {
                //show error not found
                return RedirectToAction("Admins");
            }
            try
            {
                if (UserManager.IsInRole(user.Id, "Admin"))
                {
                    UserManager.RemoveFromRole(user.Id, "Admin");
                    UserManager.Delete(user);
                }
            }
            catch
            {
                //show error not found
                return RedirectToAction("Admins");
            }
            return RedirectToAction("Admins");
        }
    }
}