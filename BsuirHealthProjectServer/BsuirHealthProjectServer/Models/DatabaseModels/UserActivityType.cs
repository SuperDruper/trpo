using Newtonsoft.Json;

namespace BsuirHealthProjectServer.Models.DatabaseModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserActivityType
    {
        public UserActivityType()
        {
            User = new HashSet<User>();
        }
        
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> User { get; set; }
    }
}
