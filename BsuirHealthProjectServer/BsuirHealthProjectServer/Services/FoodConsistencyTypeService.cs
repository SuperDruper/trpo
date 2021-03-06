﻿using BsuirHealthProjectServer.Shared;
using BsuirHealthProjectServer.Models;
using BsuirHealthProjectServer.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace BsuirHealthProjectServer.Services
{
    public class FoodConsistencyTypeService : Service<FoodConsistencyType>
    {
        public FoodConsistencyTypeService(ApplicationDbContext context)
            : base(context)
        {
        }

        public override ValidationResult Add(FoodConsistencyType item)
        {
            try
            {
                ValidationResult valResult = IsCorrectItem(item);
                if (valResult.IsSuccess == false)
                    return valResult;

                item.DateModification = DateTime.Now;

                if (context.FoodConsistencyType.Add(item) == null)
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

        public override IQueryable<FoodConsistencyType> Get()
        {
            return Get(null);
        }

        public IQueryable<FoodConsistencyType> Get(DateTime? date)
        {
            try
            {
                if (date == null)
                {
                    return context.FoodConsistencyType;
                }
                return context.FoodConsistencyType.Where(type => type.DateModification >= date);
            }
            catch (Exception ex)
            {
                //logging
                throw new BHPException("Internal server error", ex);
            }
        }

        public override FoodConsistencyType Get(int id)
        {
            try
            {
                return context.FoodConsistencyType.Find(id);
            }
            catch (Exception ex)
            {
                //logging
                throw new BHPException("Internal server error", ex);
            }
        }

        public override ValidationResult Remove(FoodConsistencyType item)
        {
            try
            {
                if (context.FoodConsistencyType.Remove(item) == null)
                    return new ValidationResult(false, "FoodConsistencyType was not found");
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
            FoodConsistencyType category = context.FoodConsistencyType.Find(id);
            return Remove(category);
        }

        public override ValidationResult Update(int id, FoodConsistencyType item)
        {
            try
            {
                ValidationResult valResult = IsCorrectItem(item);
                if (valResult.IsSuccess == false)
                    return valResult;
                FoodConsistencyType category = context.FoodConsistencyType.Find(id);
                if (category == null)
                    return new ValidationResult(false, "FoodConsistencyType was not found");
                category.Name = item.Name;
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

        public override ValidationResult IsCorrectItem(FoodConsistencyType item)
        {
            ValidationResult valResult = new ValidationResult(true);
            if (string.IsNullOrWhiteSpace(item.Name) == true || item.Name.Length > 50)
                valResult.AddErrorMessage("Name is incorrect");
            return valResult;
        }
    }
}