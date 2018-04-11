using System;
using System.Data.Entity;
using BsuirHealthProjectServer.Models.DatabaseModels;
using BsuirHealthProjectServer.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BsuirHealthProjectServer.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual DbSet<Achievement> Achievement { get; set; }
        public virtual DbSet<UserAchievement> UserAchievement { get; set; }
        public virtual DbSet<AchievementType> AchievementType { get; set; }
        public virtual DbSet<Dish> Dish { get; set; }
        public virtual DbSet<DishCategory> DishCategory { get; set; }
        public virtual DbSet<Eating> Eating { get; set; }
        public virtual DbSet<FavoriteList> FavoriteList { get; set; }
        public virtual DbSet<Food> Food { get; set; }
        public virtual DbSet<FoodCategory> FoodCategory { get; set; }
        public virtual DbSet<FoodConsistencyType> FoodConsistencyType { get; set; }
        public virtual DbSet<DishEating> DishEating { get; set; }
        public virtual DbSet<PortionFood> PortionFood { get; set; }
        public virtual DbSet<Time> Time { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserCharacteristics> UserCharacteristics { get; set; }
        public virtual DbSet<Characteristic> Characteristic { get; set; }
        public virtual DbSet<UserActivityType> UserActivityType { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>().ToTable("UserCredential");
            modelBuilder.Entity<ApplicationUser>().ToTable("UserCredential");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<IdentityRole>().ToTable("Role");

            modelBuilder.Entity<Achievement>()
                .ToTable("Achievement")
                .HasMany(e => e.UserAchievement)
                .WithRequired(e => e.Achievement)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AchievementType>()
                .ToTable("AchievementType")
                .HasMany(e => e.Achievement)
                .WithRequired(e => e.AchievementType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dish>()
                .ToTable("Dish")
                .HasMany(e => e.FavoriteList)
                .WithRequired(e => e.Dish)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dish>()
                .HasMany(e => e.DishEating)
                .WithRequired(e => e.Dish)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dish>()
                .HasMany(e => e.PortionFood)
                .WithRequired(e => e.Dish)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dish>()
                .HasMany(e => e.Time)
                .WithMany(e => e.Dish);

            modelBuilder.Entity<DishCategory>()
                .ToTable("DishCategory")
                .HasMany(e => e.Dish)
                .WithRequired(e => e.DishCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Eating>()
                .ToTable("Eating")
                .HasMany(e => e.DishEating)
                .WithRequired(e => e.Eating)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Food>()
                .ToTable("Food")
                .HasMany(e => e.PortionFood)
                .WithRequired(e => e.Food)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<FoodCategory>()
                .ToTable("FoodCategory")
                .HasMany(e => e.Food)
                .WithRequired(e => e.FoodCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FoodConsistencyType>()
                .ToTable("FoodConsistencyType")
                .HasMany(e => e.Food)
                .WithRequired(e => e.FoodConsistencyType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .ToTable("User")
                .HasMany(e => e.AchievementList)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Eating)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.FavoriteList)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Characteristic>()
                .ToTable("Characteristic");

            modelBuilder.Entity<DishEating>()
                .ToTable("DishEating");

            modelBuilder.Entity<FavoriteList>()
                .ToTable("FavoriteList");

            modelBuilder.Entity<PortionFood>()
                .ToTable("PortionFood");

            modelBuilder.Entity<Time>()
                .ToTable("Time");

            modelBuilder.Entity<UserAchievement>()
                .ToTable("UserAchievement");

            modelBuilder.Entity<UserActivityType>()
                .ToTable("UserActivityType");

            modelBuilder.Entity<UserCharacteristics>()
                .ToTable("UserCharacteristics");
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext> {
        protected override void Seed(ApplicationDbContext context)
        {
            var type1 = new FoodConsistencyType { Name = "Твердый" };
            var type2 = new FoodConsistencyType { Name = "Жидкий" };
            var foodConsistencyTypeService = new FoodConsistencyTypeService(context);
            foodConsistencyTypeService.Add(type1);
            foodConsistencyTypeService.Add(type2);

            var fakeFoodCategory = new FoodCategory
            {
                DateModification = DateTime.Now,
                Name = "fake",
                Image = new byte[] { 3, 3 }
            };
            var foodCategoryService = new FoodCategoryService(context);
            foodCategoryService.Add(fakeFoodCategory);

            var fakeDishCategory = new DishCategory
            {
                DateModification = DateTime.Now,
                Name = "fake",
                Image = new byte[] { 4 }
            };
            var dishCategoryService = new DishCategoryService(context);
            dishCategoryService.Add(fakeDishCategory);

            var fakeDish1 = new Dish
            {
                Name = "fake1",
                DishCategory = fakeDishCategory,
                TotalAmountWater = 1,
                TotalCarbs = 1,
                TotalCcal = 1,
                TotalFat = 1,
                TotalProteins = 1,
                TotalSugar = 1
            };
            var fakeDish2 = new Dish
            {
                Name = "fake2",
                DishCategory = fakeDishCategory,
                TotalAmountWater = 2,
                TotalCarbs = 2,
                TotalCcal = 2,
                TotalFat = 2,
                TotalProteins = 2,
                TotalSugar = 2,
                IsDeleted = true
            };
            var fakeDish3 = new Dish
            {
                Name = "fake3",
                DishCategory = fakeDishCategory,
                TotalAmountWater = 3,
                TotalCarbs = 3,
                TotalCcal = 3,
                TotalFat = 3,
                TotalProteins = 3,
                TotalSugar = 3
            };
            var fakeDish4 = new Dish
            {
                Name = "fake4",
                DishCategory = fakeDishCategory,
                TotalAmountWater = 4,
                TotalCarbs = 4,
                TotalCcal = 4,
                TotalFat = 4,
                TotalProteins = 4,
                TotalSugar = 4
            };
            var fakeDish5 = new Dish
            {
                Name = "fake5",
                DishCategory = fakeDishCategory,
                TotalAmountWater = 5,
                TotalCarbs = 5,
                TotalCcal = 5,
                TotalFat = 5,
                TotalProteins = 5,
                TotalSugar = 5
            };
            var dishService = new DishService(context);
            dishService.Add(fakeDish1);
            dishService.Add(fakeDish2);
            dishService.Add(fakeDish3);
            dishService.Add(fakeDish4);
            dishService.Add(fakeDish5);

            var fakeFood1 = new Food
            {
                Name = "fake1",
                FoodCategory = fakeFoodCategory,
                FoodConsistencyType = type1,
                Image = new byte[] { 1 },
                AmountOfWater = 1,
                Carbs = 1,
                Ccal = 1,
                Fat = 1,
                Proteins = 1,
                Sugar = 1
            };
            var fakeFood2 = new Food
            {
                Name = "fake2",
                FoodCategory = fakeFoodCategory,
                FoodConsistencyType = type1,
                Image = new byte[] { 3 },
                AmountOfWater = 2,
                Carbs = 2,
                Ccal = 2,
                Fat = 2,
                Proteins = 2,
                Sugar = 2
            };
            var foodService = new FoodService(context);
            foodService.Add(fakeFood1);
            foodService.Add(fakeFood2);

            var fakePortion1 = new PortionFood
            {
                Dish = fakeDish1,
                Food = fakeFood1,
                Amount = 3
            };
            var fakePortion2 = new PortionFood
            {
                Dish = fakeDish1,
                Food = fakeFood2,
                Amount = 5
            };
            var portionFoodService = new PortionFoodService(context);
            portionFoodService.Add(fakePortion1);
            portionFoodService.Add(fakePortion2);

            var fakeActivityType = new UserActivityType()
            {
                Description = "fake",
                Name = "fake"
            };
            var fakeActivityTypeService = new UserActivityTypeService(context);
            fakeActivityTypeService.Add(fakeActivityType);

            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            roleManager.Create(new IdentityRole("Admin"));
            roleManager.Create(new IdentityRole("User"));
            string adminEmail = "Admin@gmail.com";
            if (userManager.Create(new ApplicationUser() { Email = adminEmail, UserName = "Admin" }, "bsuirhealthproject").Succeeded == true)
            {
                ApplicationUser user = userManager.FindByEmail(adminEmail);
                userManager.AddToRole(user.Id, "Admin");
                userManager.AddToRole(user.Id, "User");
                var userService = new UserService(context);
                userService.Add(new User
                {
                    DateOfBirth = DateTime.Now,
                    FirstName = "fake",
                    IdActivityType = fakeActivityType.Id,
                    IdUserCredential = user.Id,
                    LastName = "fake",
                    Sex = false
                });
            }
        }
    }
}