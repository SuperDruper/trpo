
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Web.Http;
using BsuirHealthProjectServer.Models;
using BsuirHealthProjectServer.Models.DatabaseModels;
using BsuirHealthProjectServer.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace BsuirHealthProjectServer.Controllers
{
    [RoutePrefix("/api/EatingJournal")]
    [Authorize]
    [HostAuthentication(DefaultAuthenticationTypes.ApplicationCookie)]
    public class EatingJournalController : ApiController
    {
        public EatingData Get()
        {
            ApplicationDbContext context = Request.GetOwinContext().Get<ApplicationDbContext>();
            EatingService eatingService = new EatingService(context);
            UserService userService = new UserService(context);

            ApplicationUserManager manager = new ApplicationUserManager(
                new UserStore<ApplicationUser>(context));
            ApplicationUser userCredential = manager.FindByName(RequestContext.Principal.Identity.Name);
            int userId = userService.GetUserIdByUserCredential(userCredential.Id);

            return new EatingData()
            {
                Eatings = new List<Eating>()
                {
                    new Eating()
                    {
                        AmountOfWater = null,
                        Carbs = 1,
                        DishEating = new List<DishEating>
                        {
                            new DishEating
                            {
                                IdDish = 1,
                                IdEatinng = 1
                            }
                        },
                        IdUser = 1
                    }
                }
            };
        }


        /// <summary>
        /// Post example 
        /// </summary>
        /// <param name="eatingData"></param>
        public IHttpActionResult Post(EatingData eatingData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Input data format is invalid.");
            }

            if (eatingData.Eatings.Count == 0)
            {
                return BadRequest("Eating list can't be empty.");
            }

            ApplicationDbContext context = Request.GetOwinContext().Get<ApplicationDbContext>();
            EatingService eatingService = new EatingService(context);
            UserService userService = new UserService(context);

            ApplicationUserManager manager = new ApplicationUserManager(
                new UserStore<ApplicationUser>(context));
            ApplicationUser userCredential = manager.FindByName(RequestContext.Principal.Identity.Name);
            int userId = userService.GetUserIdByUserCredential(userCredential.Id);

            foreach (var eating in eatingData.Eatings)
            {
                eating.IdUser = userId;
                foreach (var dishEating in eating.DishEating)
                {
                    dishEating.IdEatinng = 0;
                }
                eatingService.Add(eating);
            }

            return Ok();
        }


        public class EatingData
        {
            [Required]
            public List<Eating> Eatings { get; set; }
        }

    }
}
