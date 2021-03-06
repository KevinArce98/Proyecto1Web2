﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Models
{
    public class Meeting
    {
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 3)]
        [Required]

        public string Title { get; set; }
        [Required]
        [Display(Name = "Date and Time")]
        [DataType(DataType.DateTime)]
        public DateTime DateAndTime { get; set; }
        [Required]
        [Display(Name = "Is Virtual")]
        public bool IsVirtual { get; set; }
        public int? ClientId { get; set; }
        public virtual Client Client { get; set; }
    }
}
