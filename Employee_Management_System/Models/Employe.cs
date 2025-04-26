namespace Employee_Management_System.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    public class Employe
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Salary { get; set; }
        public int AnnualLeaveDays { get; set; }
        public string PostePosition { get; set; }
        public DateTime HireDate { get; set; }
        public int SalaryRaised { get; set; }

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        public int? SalaryBonusId { get; set; }
        public SalaryBonus? SalaryBonuses { get; set; }
        public ICollection<LeaveApplication>? LeaveApplications { get; set; }

    }

}
