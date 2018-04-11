
using BsuirHealthProjectServer.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using BsuirHealthProjectServer.Models;
using System.Data.Entity.Validation;
using BsuirHealthProjectServer.Shared;

namespace BsuirHealthProjectServer.Services
{
    public class UserService : Service<User>
    {
        public UserService(ApplicationDbContext context) : base(context)
        {
        }

        public override ValidationResult Add(User item)
        {
            try
            {
                ValidationResult valResult = IsCorrectItem(item);
                if (valResult.IsSuccess == false)
                    return valResult;
                if (context.User.Add(item) == null)
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

        public override IQueryable<User> Get()
        {
            try
            {
                return context.User;
            }
            catch (Exception ex)
            {
                //logging
                throw new BHPException("Internal server error", ex);
            }
        }

        public override User Get(int id)
        {
            try
            {
                return context.User.Find(id);
            }
            catch (Exception ex)
            {
                //logging
                throw new BHPException("Internal server error", ex);
            }
        }

        public override ValidationResult Remove(User item)
        {
            try
            {
                if (context.User.Remove(item) == null)
                    return new ValidationResult(false, "User was not found");
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
            User user = context.User.Find(id);
            return Remove(user);
        }

        public override ValidationResult Update(int id, User item)
        {
            try
            {
                ValidationResult valResult = IsCorrectItem(item);
                if (valResult.IsSuccess == false)
                    return valResult;
                User user = context.User.Find(id);
                if (user == null)
                    return new ValidationResult(false, "FoodCategory was not found");
                user.FirstName = item.FirstName;
                user.LastName = item.LastName;
                user.DateOfBirth = item.DateOfBirth;
                user.Sex = item.Sex;
                //user.IdUserCredential = item.IdUserCredential;
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

        public override ValidationResult IsCorrectItem(User item)
        {
            ValidationResult valResult = new ValidationResult(true);
            if (string.IsNullOrWhiteSpace(item.FirstName) == true || item.FirstName.Length > 50)
                valResult.AddErrorMessage("FirstName is incorrect");
            if (string.IsNullOrWhiteSpace(item.LastName) == true || item.LastName.Length > 50)
                valResult.AddErrorMessage("LastName is incorrect");
            if (string.IsNullOrWhiteSpace(item.IdUserCredential) == true)
                valResult.AddErrorMessage("IdUserCredential is incorrect");
            return valResult;
        }

        public int GetUserIdByUserCredential(string userCredentialId)
        {
            /*try
            {*/
                return context.User.First(user => user.IdUserCredential == userCredentialId).Id;
           /* }
            catch (Exception ex)
            {
                //logging
                throw new BHPException("Internal server error.", ex);
            }*/
        }
    }
}