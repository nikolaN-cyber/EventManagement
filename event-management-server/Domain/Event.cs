using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Location { get; set; }
        public byte[]? Image { get; set; }
        public string Description { get; set; }


        public int OrganizerId { get; set; }
        public User Organizer { get; set; } = null;
        public List<EventAttendance> Attendees { get; set; } = new List<EventAttendance>();
    }
}
