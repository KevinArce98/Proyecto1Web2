using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Models
{
    public class MeetingsForUser
    {
        public MeetingsForUser()
        {

        }
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string MeetingId { get; set; }
        public virtual Meeting Meeting { get; set; }
    }
}
