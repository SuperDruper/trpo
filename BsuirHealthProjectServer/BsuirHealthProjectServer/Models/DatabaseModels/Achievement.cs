namespace BsuirHealthProjectServer.Models.DatabaseModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Achievement")]
    public partial class Achievement
    {
        public Achievement()
        {
            UserAchievement = new HashSet<UserAchievement>();
        }

        [Key]
        public int Id { get; set; }

        public string Description { get; set; }

        [ForeignKey("AchievementType")]
        public int IdAchievementType { get; set; }

        public virtual AchievementType AchievementType { get; set; }

        public virtual ICollection<UserAchievement> UserAchievement { get; set; }
    }
}
