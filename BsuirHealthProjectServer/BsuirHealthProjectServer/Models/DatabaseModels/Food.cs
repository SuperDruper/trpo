using Newtonsoft.Json;

namespace BsuirHealthProjectServer.Models.DatabaseModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Food
    {
        public Food()
        {
            PortionFood = new HashSet<PortionFood>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public float Proteins { get; set; }

        public float Fat { get; set; }

        public float Carbs { get; set; }

        public float Ccal { get; set; }

        public float? Sugar { get; set; }

        public float? AmountOfWater { get; set; }

		public bool IsDeleted { get; set; }

        [Required]
        public DateTime DateModification { get; set; }
		
        [ForeignKey("FoodCategory")]
        public int IdFoodCategory { get; set; }

        [ForeignKey("FoodConsistencyType")]
        public int IdFoodConsistencyType { get; set; }

        [Column(TypeName = "image")]
        public byte[] Image { get; set; }

        [JsonIgnore]
        public virtual FoodCategory FoodCategory { get; set; }

        [JsonIgnore]
        public virtual FoodConsistencyType FoodConsistencyType { get; set; }

        [JsonIgnore]
        public virtual ICollection<PortionFood> PortionFood { get; set; }
    }
}
