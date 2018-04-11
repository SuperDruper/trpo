namespace BsuirHealthProjectServer.Models.DatabaseModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AchievementType
    {
        public AchievementType()
        {
            Achievement = new HashSet<Achievement>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "image")]
        public byte[] Image { get; set; }

        public DateTime DateUpdate { get; set; }

        public virtual ICollection<Achievement> Achievement { get; set; }
    }
}
