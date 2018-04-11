using System;
using System.Data.Entity.Validation;
using System.Linq;
using BsuirHealthProjectServer.Models;
using BsuirHealthProjectServer.Models.DatabaseModels;
using BsuirHealthProjectServer.Shared;

namespace BsuirHealthProjectServer.Services
{
    public class DishEatingService : Service<DishEating>
    {
        protected DishService DishService { get; private set; }

        protected EatingService EatingService { get; private set; }


        public DishEatingService(ApplicationDbContext context) : base(context)
        {
            DishService = new DishService(context);
            EatingService = new EatingService(context);
        }

        

        public override ValidationResult Add(DishEating item)
        {
            try
            {
                ValidationResult valResult = IsCorrectItem(item);
                if (valResult.IsSuccess == false)
                    return valResult;
                if (context.DishEating.Add(item) == null)
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

        public override IQueryable<DishEating> Get()
        {
            try
            {
                return context.DishEating;
            }
            catch (Exception ex)
            {
                //logging
                throw new BHPException("Internal server error", ex);
            }
        }

        public override DishEating Get(int id)
        {
            try
            {
                return context.DishEating.Find(id);
            }
            catch (Exception ex)
            {
                //logging
                throw new BHPException("Internal server error", ex);
            }
        }

        public override ValidationResult Remove(DishEating item)
        {
            try
            {
                if (context.DishEating.Remove(item) == null)
                    return new ValidationResult(false, "DishEating was not found");
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
            DishEating dishEating = context.DishEating.Find(id);
            return Remove(dishEating);
        }

        public override ValidationResult Update(int id, DishEating item)
        {
            try
            {
                ValidationResult valResult = IsCorrectItem(item);
                if (valResult.IsSuccess == false)
                    return valResult;
                DishEating dishEating = context.DishEating.Find(id);
                if (dishEating == null)
                    return new ValidationResult(false, "DishEating was not found");
                dishEating.Amount = item.Amount;
                dishEating.IdDish = item.IdDish;
                dishEating.IdEatinng = item.IdEatinng;
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

        public override ValidationResult IsCorrectItem(DishEating item)
        {
            ValidationResult valResult = new ValidationResult(true);
            if (item.Dish == null && DishService.Get(item.IdDish) == null)
                valResult.AddErrorMessage("IdDish is incorrect");
            if (item.Eating == null && DishService.Get(item.IdEatinng) == null)
                valResult.AddErrorMessage("IdEatinng is incorrect");
            return valResult;
        }
    }
}