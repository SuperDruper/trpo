using BsuirHealthProjectServer.Shared;
using BsuirHealthProjectServer.Models;
using BsuirHealthProjectServer.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace BsuirHealthProjectServer.Services
{
    public class FoodCategoryService : Service<FoodCategory>
    {
        public FoodCategoryService(ApplicationDbContext context)
            : base(context)
        {
        }

        public override ValidationResult Add(FoodCategory item)
        {
            try
            {
                ValidationResult valResult = IsCorrectItem(item);
                if (valResult.IsSuccess == false)
                    return valResult;

                item.DateModification = DateTime.Now;
                item.IsDeleted = false;

                if (context.FoodCategory.Add(item) == null)
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

        public override IQueryable<FoodCategory> Get()
        {
            return Get(null);
        }

        public IQueryable<FoodCategory> Get(DateTime? date)
        {
            try
            {
                if (date == null)
                {
                    return context.FoodCategory;
                }
                return context.FoodCategory.Where(type => type.DateModification >= date && type.IsDeleted == false);
            }
            catch (Exception ex)
            {
                //logging
                throw new BHPException("Internal server error", ex);
            }
        }

        public override FoodCategory Get(int id)
        {
            try
            {
                var item = context.FoodCategory.Find(id);
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

        public override ValidationResult Remove(FoodCategory item)
        {
            try
            {
                if (item == null || item.IsDeleted)
                    return new ValidationResult(false, "FoodCategory was not found");
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
            FoodCategory category = context.FoodCategory.Find(id);
            return Remove(category);
        }

        public override ValidationResult Update(int id, FoodCategory item)
        {
            try
            {
                ValidationResult valResult = IsCorrectItem(item);
                if (valResult.IsSuccess == false)
                    return valResult;
                FoodCategory category = context.FoodCategory.Find(id);
                if (category == null)
                    return new ValidationResult(false, "FoodCategory was not found");
                category.Name = item.Name;
                category.Image = item.Image;
                category.DateModification = DateTime.Now;
                category.IsDeleted = item.IsDeleted;
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

        public override ValidationResult IsCorrectItem(FoodCategory item)
        {
            ValidationResult valResult = new ValidationResult(true);
            if (string.IsNullOrWhiteSpace(item.Name) || item.Name.Length > 50)
                valResult.AddErrorMessage("Name is incorrect");
            if (item.Image == null || item.Image.Length == 0)
                valResult.AddErrorMessage("Image is incorrect");
            return valResult;
        }
    }
}