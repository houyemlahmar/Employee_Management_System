namespace Employee_Management_System.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public Utilisateur ? User { get; set; }
        public bool IsRead { get; set; } = false;
    }

}
