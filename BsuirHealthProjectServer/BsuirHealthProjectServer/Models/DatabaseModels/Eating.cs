using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BsuirHealthProjectServer.Models.DatabaseModels
{
    public partial class Eating
    {
        public Eating()
        {
            DishEating = new HashSet<DishEating>();
        }

        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int IdUser { get; set; }

        public DateTime Date { get; set; }

        public float Proteins { get; set; }

        public float Fat { get; set; }

        public float Carbs { get; set; }

        public float Ccal { get; set; }

        public float? Sugar { get; set; }

        public float? AmountOfWater { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        public virtual ICollection<DishEating> DishEating { get; set; }


        // This method is here just to make json serializer to not serialize property DishEating
        // Ya-ya, it's awful
        public bool ShouldSerializeDishEating()
        {
            return false;
        }
    }
}
