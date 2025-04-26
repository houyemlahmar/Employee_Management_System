using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Management_System.Models
{
    public enum LeavePriority { High, Medium, Low }
    public enum LeaveStatus { Pending, Accepted, Refused }

    public class LeaveApplication
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public LeavePriority Priority { get; set; }
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employe? Employe { get; set; }
    }

}
