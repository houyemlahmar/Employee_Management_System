using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Employee_Management_System.Data;
using Employee_Management_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Employee_Management_System.Controllers
{
    public class LeaveApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // HRManager: Voir toutes les demandes
        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "HRManager") return RedirectToAction("Index", "Compte");
            var leaves = await _context.LeaveApplications.Include(l => l.Employe).ToListAsync();
            return View(leaves);
        }

        // Employé : Voir ses propres demandes
        public async Task<IActionResult> MyLeaves()
        {
            var role = HttpContext.Session.GetString("UserRole");
            var userId = HttpContext.Session.GetString("UserId");
            if (role != "Employee") return RedirectToAction("LoginEmployee", "Compte");

            var empId = int.Parse(userId);
            var leaves = await _context.LeaveApplications
                .Include(l => l.Employe)
                .Where(l => l.EmployeeId == empId)
                .ToListAsync();
            return View(leaves);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StartDate,EndDate,Type,Description,Priority")] LeaveApplication leaveApplication)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null) return RedirectToAction("LoginEmployee", "Compte");
            leaveApplication.EmployeeId = int.Parse(userId);
            leaveApplication.Status = LeaveStatus.Pending;

            if (ModelState.IsValid)
            {
                _context.Add(leaveApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyLeaves));
            }
            return View(leaveApplication);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication == null) return NotFound();

            var userId = HttpContext.Session.GetString("UserId");
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Employee" || leaveApplication.EmployeeId.ToString() != userId || leaveApplication.Status != LeaveStatus.Pending)
                return Unauthorized();

            return View(leaveApplication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartDate,EndDate,Type,Description,Priority")] LeaveApplication leaveApplication)
        {
            var original = await _context.LeaveApplications.FindAsync(id);
            if (original == null || original.Status != LeaveStatus.Pending) return NotFound();

            if (ModelState.IsValid)
            {
                original.StartDate = leaveApplication.StartDate;
                original.EndDate = leaveApplication.EndDate;
                original.Type = leaveApplication.Type;
                original.Description = leaveApplication.Description;
                original.Priority = leaveApplication.Priority;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyLeaves));
            }
            return View(leaveApplication);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication == null) return NotFound();

            return View(leaveApplication);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication != null && leaveApplication.Status == LeaveStatus.Pending)
            {
                _context.LeaveApplications.Remove(leaveApplication);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(MyLeaves));
        }

        // HRManager : Modifier le status (Accepted, Refused)
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, LeaveStatus newStatus)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "HRManager") return Unauthorized();

            var leave = await _context.LeaveApplications
                .Include(l => l.Employe)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (leave == null) return NotFound();

            var previousStatus = leave.Status;
            var leaveDays = (int)(leave.EndDate - leave.StartDate).TotalDays + 1;

            if (newStatus == LeaveStatus.Accepted && previousStatus != LeaveStatus.Accepted)
            {
                

                leave.Employe.AnnualLeaveDays -= leaveDays;
            }
            else if (newStatus == LeaveStatus.Refused && previousStatus == LeaveStatus.Accepted)
            {
                leave.Employe.AnnualLeaveDays += leaveDays;
            }

            leave.Status = newStatus;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



    }
}
