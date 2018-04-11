using Newtonsoft.Json;

namespace BsuirHealthProjectServer.Models.DatabaseModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FoodConsistencyType
    {
        public FoodConsistencyType()
        {
            Food = new HashSet<Food>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

		[Required]
        public DateTime DateModification { get; set; }

        [JsonIgnore]
        public virtual ICollection<Food> Food { get; set; }
    }
}
