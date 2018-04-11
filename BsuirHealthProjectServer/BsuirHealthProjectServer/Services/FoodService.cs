using System;
using System.Data.Entity.Validation;
using System.Linq;
using BsuirHealthProjectServer.Models;
using BsuirHealthProjectServer.Models.DatabaseModels;
using BsuirHealthProjectServer.Shared;

namespace BsuirHealthProjectServer.Services
{
    public class FoodService : Service<Food>
    {
        protected FoodCategoryService FoodCategoryService { get; private set; }

        protected FoodConsistencyTypeService FoodConsistencyTypeService { get; private set; }

        public FoodService(ApplicationDbContext context)
            : base(context)
        {
            FoodCategoryService = new FoodCategoryService(context);
            FoodConsistencyTypeService = new FoodConsistencyTypeService(context);
        }

        public override ValidationResult Add(Food item)
        {
            try
            {
                ValidationResult valResult = IsCorrectItem(item);
                if (valResult.IsSuccess == false)
                    return valResult;

                item.DateModification = DateTime.Now;

                if (context.Food.Add(item) == null)
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

        public override IQueryable<Food> Get()
        {
            return Get(null);
        }

        public IQueryable<Food> Get(DateTime? date)
        {
            try
            {
                if (date == null)
                {
                    return context.Food;
                }
                return context.Food.Where(type => type.DateModification >= date && type.IsDeleted == false);
            }
            catch (Exception ex)
            {
                //logging
                throw new BHPException("Internal server error", ex);
            }
        }

        public override Food Get(int id)
        {
            try
            {
                var item = context.Food.Find(id);
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

        public override ValidationResult Remove(Food item)
        {
            try
            {
                if (item == null || item.IsDeleted)
                    return new ValidationResult(false, "Food was not found");
                if (item.PortionFood != null && item.PortionFood.Count > 0)
                    return new ValidationResult(false,
                        String.Format(
                            "Can't delete food from database, because it is contained in some dishes ({0} times).",
                            item.PortionFood.Count));

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

        public override ValidationResult Remove(int id)
        {
            Food food = context.Food.Find(id);
            return Remove(food);
        }

        public override ValidationResult Update(int id, Food item)
        {
            try
            {
                ValidationResult valResult = IsCorrectItem(item);
                if (valResult.IsSuccess == false)
                    return valResult;
                Food food = context.Food.Find(id);
                if (food == null)
                    return new ValidationResult(false, "Food was not found");
                food.AmountOfWater = item.AmountOfWater;
                food.Carbs = item.Carbs;
                food.Ccal = item.Ccal;
                food.Fat = item.Fat;
                food.IdFoodCategory = item.IdFoodCategory;
                food.IdFoodConsistencyType = item.IdFoodConsistencyType;
                food.Image = item.Image;
                food.Name = item.Name;
                food.Proteins = item.Proteins;
                food.Sugar = item.Sugar;
                food.DateModification = DateTime.Now;
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

        public override ValidationResult IsCorrectItem(Food item)
        {
            ValidationResult valResult = new ValidationResult(true);
            if (string.IsNullOrWhiteSpace(item.Name) || item.Name.Length > 50)
                valResult.AddErrorMessage("Name is incorrect");
            if (item.Image == null || item.Image.Length == 0)
                valResult.AddErrorMessage("Image is incorrect");
            if (item.FoodCategory == null && FoodCategoryService.Get(item.IdFoodCategory) == null)
                valResult.AddErrorMessage("IdFoodCategory is incorrect");
            if (item.FoodConsistencyType == null && FoodConsistencyTypeService.Get(item.IdFoodConsistencyType) == null)
                valResult.AddErrorMessage("IdFoodConsistencyType is incorrect");
            return valResult;
        }
    }
}