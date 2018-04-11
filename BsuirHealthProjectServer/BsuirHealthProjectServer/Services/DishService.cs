using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using BsuirHealthProjectServer.Models;
using BsuirHealthProjectServer.Models.DatabaseModels;
using BsuirHealthProjectServer.Shared;

namespace BsuirHealthProjectServer.Services
{
    public class DishService : Service<Dish>
    {
        protected DishCategoryService CategoryService { get; private set; }

        public DishService(ApplicationDbContext context)
            : base(context)
        {
            CategoryService = new DishCategoryService(context);
        }

        public override Dish Get(int id)
        {
            try
            {
                var item = context.Dish.Find(id);
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

        public override IQueryable<Dish> Get()
        {
            return Get(null);
        }

        public IQueryable<Dish> Get(DateTime? date)
        {
            try
            {
                if (date == null)
                {
                    return context.Dish;
                }
                return context.Dish.Where(type => type.DateModification >= date && type.IsDeleted == false);
            }
            catch (Exception ex)
            {
                //logging
                throw new BHPException("Internal server error", ex);
            }
        }

        public override ValidationResult Add(Dish item)
        {
            try
            {
                ValidationResult valResult = IsCorrectItem(item);
                if (valResult.IsSuccess == false)
                    return valResult;

                item.DateModification = DateTime.Now;

                if (context.Dish.Add(item) == null)
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

        public override ValidationResult Update(int id, Dish item)
        {
            try
            {
                ValidationResult valResult = IsCorrectItem(item);
                if (valResult.IsSuccess == false)
                    return valResult;
                Dish dish = context.Dish.Find(id);
                if (dish == null)
                    return new ValidationResult(false, "Dish was not found");
                dish.Name = item.Name;
                dish.Description = item.Description;

                if (item.IdDishCategory > 0)
                {
                    dish.IdDishCategory = item.IdDishCategory;
                }
                else
                {
                    dish.DishCategory = item.DishCategory;
                }

                dish.IdUser = item.IdUser;
                dish.TotalAmountWater = item.TotalAmountWater;
                dish.TotalCarbs = item.TotalCarbs;
                dish.TotalCcal = item.TotalCcal;
                dish.TotalFat = item.TotalFat;
                dish.TotalProteins = item.TotalProteins;
                dish.TotalSugar = item.TotalSugar;

                if (!dish.IsDeleted || !item.IsDeleted)
                {
                    dish.DateModification = DateTime.Now;
                }

                dish.IsDeleted = item.IsDeleted;
                
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
            Dish dish = context.Dish.Find(id);
            return Remove(dish);
        }

        public override ValidationResult Remove(Dish item)
        {
            if (item == null || item.IsDeleted)
                return new ValidationResult(false, "Dish was not found");

            using (DbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    item.IsDeleted = true;
                    item.DateModification = DateTime.Now;

                    context.SaveChanges();

                    PortionFoodService portionFoodService = new PortionFoodService(context);
                    foreach (var portion in item.PortionFood)
                    {
                        portionFoodService.Remove(portion);
                    }

                    transaction.Commit();

                    return new ValidationResult(true);
                }
                catch (DbEntityValidationException ex)
                {
                    transaction.Rollback();
                    //logging
                    throw new BHPException("Some data was damaged, and changes was not saved", ex);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    //logging
                    throw new BHPException("Internal server error", ex);
                }
            }
        }

        public override ValidationResult IsCorrectItem(Dish item)
        {
            ValidationResult valResult = new ValidationResult(true);
            if (string.IsNullOrWhiteSpace(item.Name) || item.Name.Length > 50)
                valResult.AddErrorMessage("Name is incorrect");
            
            if (item.DishCategory == null && CategoryService.Get(item.IdDishCategory) == null)
                valResult.AddErrorMessage("IdDishCategory is incorrect");

            return valResult;
        }
    }
}