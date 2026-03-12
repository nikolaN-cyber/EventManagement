namespace Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public byte[]? ProfileImage { get; set; }
        public List<Event> OrganizedEvents { get; set; } = new List<Event>();
        public List<EventAttendance> AttendingEvents { get; set; } = new List<EventAttendance>();
    }
}
