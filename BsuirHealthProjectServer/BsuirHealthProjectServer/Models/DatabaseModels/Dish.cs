using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BsuirHealthProjectServer.Models.DatabaseModels
{
    public class Dish
    {
        public Dish()
        {
            FavoriteList = new HashSet<FavoriteList>();
            DishEating = new HashSet<DishEating>();
            PortionFood = new HashSet<PortionFood>();
            Time = new HashSet<Time>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        public float TotalProteins { get; set; }

        public float TotalFat { get; set; }

        public float TotalCarbs { get; set; }

        public float TotalCcal { get; set; }

        public float? TotalSugar { get; set; }

        public float? TotalAmountWater { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public DateTime DateModification { get; set; }

        [JsonIgnore]        
		public virtual DishCategory DishCategory { get; set; }
        [ForeignKey("DishCategory")]
        public int IdDishCategory { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
        [ForeignKey("User")]
        public int? IdUser { get; set; }

        [JsonIgnore]
        public virtual ICollection<FavoriteList> FavoriteList { get; set; }

        [JsonIgnore]
        public virtual ICollection<DishEating> DishEating { get; set; }

        [JsonIgnore]
        public virtual ICollection<PortionFood> PortionFood { get; set; }

        [JsonIgnore]
        public virtual ICollection<Time> Time { get; set; }
    }
}
