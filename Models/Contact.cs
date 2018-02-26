using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        [Display(Name = "Email"), DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Position { get; set; }
        [Required]
        [Display(Name = "Client")]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
    }
}
