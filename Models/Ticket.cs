using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Models
{
    public class Ticket
    {

        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Detials { get; set; }
        [Required]
        public string Reporter { get; set; }
        [Required]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        [Required]
        public Status State { get; set; }

    }
    public enum Status {
        Open,
        Process,
        Waiting,
        Finish
    }
}
