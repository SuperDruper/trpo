using Newtonsoft.Json;

namespace BsuirHealthProjectServer.Models.DatabaseModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PortionFood
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("Food")]
        public int IdFood { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("Dish")]
        public int IdDish { get; set; }

        public float Amount { get; set; }

		public bool IsDeleted { get; set; }

        [Required]
        public DateTime DateModification { get; set; }
		
        [JsonIgnore]
        public virtual Dish Dish { get; set; }

        [JsonIgnore]
        public virtual Food Food { get; set; }
    }
}
