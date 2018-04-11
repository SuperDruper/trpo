using BsuirHealthProjectServer.Shared;
using BsuirHealthProjectServer.Models;
using BsuirHealthProjectServer.Models.DatabaseModels;
using System;
using System.Data.Entity.Validation;
using System.Linq;

namespace BsuirHealthProjectServer.Services
{
    public class DishCategoryService : Service<DishCategory>
    {
        public DishCategoryService(ApplicationDbContext context)
            : base(context)
        {
        }

        public override ValidationResult Add(DishCategory item)
        {
            try
            {
                ValidationResult valResult = IsCorrectItem(item);
                if (valResult.IsSuccess == false)
                    return valResult;

                item.DateModification = DateTime.Now;

                if (context.DishCategory.Add(item) == null)
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

        public override IQueryable<DishCategory> Get()
        {
            return Get(null);
        }

        public IQueryable<DishCategory> Get(DateTime? date)
        {
            try
            {
                if (date == null)
                {
                    return context.DishCategory;
                }
                return context.DishCategory.Where(type => type.DateModification >= date && type.IsDeleted == false);
            }
            catch (Exception ex)
            {
                //logging
                throw new BHPException("Internal server error", ex);
            }
        }

        public override DishCategory Get(int id)
        {
            try
            {
                var item = context.DishCategory.Find(id);
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

        public override ValidationResult Remove(DishCategory item)
        {
            try
            {
                if (item == null || item.IsDeleted)
                    return new ValidationResult(false, "DishCategory was not found");
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
            DishCategory category = context.DishCategory.Find(id);
            return Remove(category);
        }

        public override ValidationResult Update(int id, DishCategory item)
        {
            try
            {
                ValidationResult valResult = IsCorrectItem(item);
                if (valResult.IsSuccess == false)
                    return valResult;
                DishCategory category = context.DishCategory.Find(id);
                if (category == null)
                    return new ValidationResult(false, "DishCategory was not found");
                category.Name = item.Name;
                category.Image = item.Image;
                category.DateModification = DateTime.Now;
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

        public override ValidationResult IsCorrectItem(DishCategory item)
        {
            ValidationResult valResult = new ValidationResult(true);
            if (string.IsNullOrWhiteSpace(item.Name))
                valResult.AddErrorMessage("Name is incorrect");
            if (item.Image == null || item.Image.Length == 0)
                valResult.AddErrorMessage("Image is incorrect");
            return valResult;
        }
    }
}