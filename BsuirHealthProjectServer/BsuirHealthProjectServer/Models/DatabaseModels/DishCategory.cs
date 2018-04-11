using Newtonsoft.Json;

namespace BsuirHealthProjectServer.Models.DatabaseModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DishCategory
    {
        public DishCategory()
        {
            Dish = new HashSet<Dish>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "image")]
        public byte[] Image { get; set; }
		
		public bool IsDeleted { get; set; }

        [Required]
        public DateTime DateModification { get; set; }

        [JsonIgnore]
        public virtual ICollection<Dish> Dish { get; set; }
    }
}
