using System;
using System.Data.Entity.Validation;
using System.Linq;
using BsuirHealthProjectServer.Models;
using BsuirHealthProjectServer.Models.DatabaseModels;
using BsuirHealthProjectServer.Shared;

namespace BsuirHealthProjectServer.Services
{
    public class PortionFoodService
    {
        protected FoodService FoodService { get; private set; }
        protected DishService DishService { get; private set; }

        protected ApplicationDbContext context;


        public PortionFoodService(ApplicationDbContext context)
        {
            FoodService = new FoodService(context);
            DishService = new DishService(context);
            this.context = context;
        }

        public ValidationResult Add(PortionFood item)
        {
            try
            {
                ValidationResult valResult = IsCorrectItem(item);
                if (valResult.IsSuccess == false)
                    return valResult;

                item.DateModification = DateTime.Now;

                PortionFood existingPortionFood = context.PortionFood.Find(item.IdFood, item.IdDish);
                if (existingPortionFood != null && existingPortionFood.IsDeleted)
                {
                    existingPortionFood.Amount = item.Amount;
                    existingPortionFood.DateModification = item.DateModification;
                    existingPortionFood.IsDeleted = false;
                }
                else
                {
                    if (context.PortionFood.Add(item) == null)
                        return new ValidationResult(false, "Item was not added");
                }

                
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

        public IQueryable<PortionFood> Get()
        {
            return Get(null);
        }

        public IQueryable<PortionFood> Get(DateTime? date)
        {
            try
            {
                if (date == null)
                {
                    return context.PortionFood;
                }
                return context.PortionFood.Where(type => type.DateModification >= date && type.IsDeleted == false);
            }
            catch (Exception ex)
            {
                //logging
                throw new BHPException("Internal server error", ex);
            }
        }

        public PortionFood Get(int foodId, int dishId)
        {
            try
            {
                var item = context.PortionFood.Find(foodId, dishId);
                if (item != null && !item.IsDeleted)
                {
                    return item;
                }
                return null;
            }
            catch (Exception ex)
            {
                //logging
                throw new BHPException("Internal server error", ex);
            }
        }

        public ValidationResult Remove(PortionFood item)
        {
            try
            {
                if (item == null || item.IsDeleted)
                    return new ValidationResult(false, "PortionFood was not found");
                item.IsDeleted = true;
                item.DateModification = DateTime.Now;
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

        public ValidationResult Remove(int foodId, int dishId)
        {
            PortionFood portion = context.PortionFood.Find(foodId, dishId);
            return Remove(portion);
        }

        public ValidationResult Update(int foodId, int dishId, PortionFood item)
        {
            try
            {
                ValidationResult valResult = IsCorrectItem(item);
                if (valResult.IsSuccess == false)
                    return valResult;
                PortionFood portion = context.PortionFood.Find(foodId, dishId);
                if (portion == null)
                    return new ValidationResult(false, "PortionFood was not found");
                portion.Amount = item.Amount;
                portion.IdDish = item.IdDish;
                portion.IdFood = item.IdFood;
                portion.DateModification = DateTime.Now;
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

        public ValidationResult IsCorrectItem(PortionFood item)
        {
            ValidationResult valResult = new ValidationResult(true);
            if (item.Food == null && FoodService.Get(item.IdFood) == null)
                valResult.AddErrorMessage("IdFood is incorrect");
            if (item.Dish == null && DishService.Get(item.IdDish) == null)
                valResult.AddErrorMessage("IdDish is incorrect");
            return valResult;
        }
    }
}