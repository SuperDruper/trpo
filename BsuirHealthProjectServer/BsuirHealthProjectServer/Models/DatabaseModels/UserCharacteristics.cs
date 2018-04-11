namespace BsuirHealthProjectServer.Models.DatabaseModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserCharacteristics
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateUpdate { get; set; }

        public float Value { get; set; }

        [ForeignKey("Characteristic")]
        public int? IdCharacteristic { get; set; }

        [ForeignKey("User")]
        public int? IdUser { get; set; }

        public virtual User User { get; set; }

        public virtual Characteristic Characteristic { get; set; }
    }
}
