using Newtonsoft.Json;

namespace BsuirHealthProjectServer.Models.DatabaseModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DishEating
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("Dish")]
        public int IdDish { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("Eating")]
        public int IdEatinng { get; set; }

        public float Amount { get; set; }

        [JsonIgnore]
        public virtual Dish Dish { get; set; }

        [JsonIgnore]
        public virtual Eating Eating { get; set; }
    }
}
