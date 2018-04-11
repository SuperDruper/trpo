using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;    


namespace BsuirHealthProjectServer.Models.DatabaseModels
{
    public partial class User
    {
        public User()
        {
            AchievementList = new HashSet<UserAchievement>();
            Dish = new HashSet<Dish>();
            Eating = new HashSet<Eating>();
            FavoriteList = new HashSet<FavoriteList>();
            UserCharacteristics = new HashSet<UserCharacteristics>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool Sex { get; set; }

        [ForeignKey("UserActivityType")]
        public int? IdActivityType { get; set; }

        [Required]
        [StringLength(128)]
        [ForeignKey("UserCredential")]
        public string IdUserCredential { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserAchievement> AchievementList { get; set; }

        [JsonIgnore]
        public virtual ICollection<Dish> Dish { get; set; }

        [JsonIgnore]
        public virtual ICollection<Eating> Eating { get; set; }

        [JsonIgnore]
        public virtual ICollection<FavoriteList> FavoriteList { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserCharacteristics> UserCharacteristics { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser UserCredential { get; set; }

        [JsonIgnore]
        public virtual UserActivityType UserActivityType { get; set; }
    }
}
