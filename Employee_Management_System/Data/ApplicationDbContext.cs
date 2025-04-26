using Employee_Management_System.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employe> Employes { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<SalaryBonus> SalaryBonuses { get; set; }
        public virtual DbSet<LeaveApplication> LeaveApplications { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Utilisateur> Utilisateurs { get; set; }

    }

}
