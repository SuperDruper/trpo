using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using BsuirHealthProjectServer.Models;
using BsuirHealthProjectServer.Models.DatabaseModels;
using BsuirHealthProjectServer.Shared;

namespace BsuirHealthProjectServer.Services
{
    public class UserActivityTypeService : Service<UserActivityType>
    {
        public UserActivityTypeService(ApplicationDbContext context)
            : base(context)
        {
        }

        public override ValidationResult Add(UserActivityType item)
        {
            try
            {
                ValidationResult valResult = IsCorrectItem(item);
                if (valResult.IsSuccess == false)
                    return valResult;
                if (context.UserActivityType.Add(item) == null)
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

        public override IQueryable<UserActivityType> Get()
        {
            try
            {
                return context.UserActivityType;
            }
            catch (Exception ex)
            {
                //logging
                throw new BHPException("Internal server error", ex);
            }
        }

        public override UserActivityType Get(int id)
        {
            try
            {
                return context.UserActivityType.Find(id);
            }
            catch (Exception ex)
            {
                //logging
                throw new BHPException("Internal server error", ex);
            }
        }

        public override ValidationResult Remove(UserActivityType item)
        {
            try
            {
                if (context.UserActivityType.Remove(item) == null)
                    return new ValidationResult(false, "UserActivityType was not found");
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
            UserActivityType userActivityType = context.UserActivityType.Find(id);
            return Remove(userActivityType);
        }

        public override ValidationResult Update(int id, UserActivityType item)
        {
            try
            {
                ValidationResult valResult = IsCorrectItem(item);
                if (valResult.IsSuccess == false)
                    return valResult;
                UserActivityType userActivityType = context.UserActivityType.Find(id);
                if (userActivityType == null)
                    return new ValidationResult(false, "FoodCategory was not found");
                userActivityType.Description = item.Description;
                userActivityType.Name = item.Name;
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

        public override ValidationResult IsCorrectItem(UserActivityType item)
        {
            ValidationResult valResult = new ValidationResult(true);
            if (string.IsNullOrWhiteSpace(item.Name) == true || item.Name.Length > 50)
                valResult.AddErrorMessage("Name is incorrect");
            return valResult;
        }

    }
}