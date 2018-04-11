namespace BsuirHealthProjectServer.Models.DatabaseModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Time
    {
        public Time()
        {
            FavoriteList = new HashSet<FavoriteList>();
            Dish = new HashSet<Dish>();
        }

        [Key]
        public int Id { get; set; }

        public TimeSpan DateBegin { get; set; }

        public TimeSpan DateEnd { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<FavoriteList> FavoriteList { get; set; }

        public virtual ICollection<Dish> Dish { get; set; }
    }
}
