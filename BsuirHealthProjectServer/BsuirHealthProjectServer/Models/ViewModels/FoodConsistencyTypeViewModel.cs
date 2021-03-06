﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsuirHealthProjectServer.Models.ViewModels
{
     public class FoodConsistencyTypeViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public DateTime DateModification { get; set; }
    }
}
