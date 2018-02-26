using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Models
{
    public class Client
    {

        public int Id { get; set; }
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Legal Number")]
        [Required]
        public string LegalNumber { get; set; }
        [Display(Name = "Web Page")]
        [Required]
        public string WebPage { get; set; }
        [Required]
        public string Address { get; set; }
        [Display(Name = "Phone Number")]
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public Sectors Sector { get; set; }

    }
    public enum Sectors
    {
        Education, Industry, Agriculture, Manufacturing, Services, Others
    };
}
