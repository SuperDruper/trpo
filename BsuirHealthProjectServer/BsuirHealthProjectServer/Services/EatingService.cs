using System;
using System.Data.Entity.Validation;
using System.Linq;
using BsuirHealthProjectServer.Models;
using BsuirHealthProjectServer.Models.DatabaseModels;
using BsuirHealthProjectServer.Shared;

namespace BsuirHealthProjectServer.Services
{
    public class EatingService : Service<Eating>
    {
        protected UserService UserService { get; private set; }


        public EatingService(ApplicationDbContext context) : base(context)
        {
            UserService = new UserService(context);
        }

        

        public override ValidationResult Add(Eating item)
        {
            try
            {
                ValidationResult valResult = IsCorrectItem(item);
                if (valResult.IsSuccess == false)
                    return valResult;
                if (context.Eating.Add(item) == null)
                    return new ValidationResult(false, "Item was not added");
                context.SaveChanges();
                return new ValidationResult(true);
            }
            catch (DbEntityValidationException ex)
            {
                //logging
                throw new BHPException("Added value is incorrect, and changes was not saved", ex);
            }
            catch (Exception ex)
            {
                //logging
                throw new BHPException("Internal server error", ex);
            }
        }

        public override IQueryable<Eating> Get()
        {
            try
            {
                return context.Eating;
            }
            catch (Exception ex)
            {
                //logging
                throw new BHPException("Internal server error", ex);
            }
        }

        public override Eating Get(int id)
        {
            try
            {
                return context.Eating.Find(id);
            }
            catch (Exception ex)
            {
                //logging
                throw new BHPException("Internal server error", ex);
            }
        }

        public override ValidationResult Remove(Eating item)
        {
            try
            {
                if (context.Eating.Remove(item) == null)
                    return new ValidationResult(false, "Eating was not found");
                context.SaveChanges();
                return new ValidationResult(true);
            }
            catch (DbEntityValidationException ex)
            {
                //logging
                throw new BHPException("Some data was damaged, and changes was not saved", ex);
            }
            catch (Exception ex)
            {
                //logging
                throw new BHPException("Internal server error", ex);
            }
        }

        public override ValidationResult Remove(int id)
        {
            Eating eating = context.Eating.Find(id);
            return Remove(eating);
        }

        public override ValidationResult Update(int id, Eating item)
        {
            try
            {
                ValidationResult valResult = IsCorrectItem(item);
                if (valResult.IsSuccess == false)
                    return valResult;
                Eating eating = context.Eating.Find(id);
                if (eating == null)
                    return new ValidationResult(false, "Eating was not found");
                eating.AmountOfWater = item.AmountOfWater;
                eating.Carbs = item.Carbs;
                eating.Ccal = item.Ccal;
                eating.Fat = item.Fat;
                eating.Date = item.Date;
                eating.IdUser = item.IdUser;
                eating.Proteins = item.Proteins;
                eating.Sugar = item.Sugar;
                context.SaveChanges();
                return new ValidationResult(true);
            }
            catch (DbEntityValidationException ex)
            {
                //logging
                throw new BHPException("Some data was damaged, and changes was not saved", ex);
            }
            catch (Exception ex)
            {
                //logging
                throw new BHPException("Internal server error", ex);
            }
        }

        public override ValidationResult IsCorrectItem(Eating item)
        {
            ValidationResult valResult = new ValidationResult(true);
            if (item.User == null && UserService.Get(item.IdUser) == null)
                valResult.AddErrorMessage("IdUser is incorrect");
            return valResult;
        }
    }
}