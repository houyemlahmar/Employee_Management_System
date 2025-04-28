using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Employee_Management_System.Data;
using Employee_Management_System.Models;

namespace Employee_Management_System.Controllers
{
    public class EmployesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<Employe> _passwordHasher;

        public EmployesController(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Employe>();
        }
        

        public IActionResult DashboardEmployee()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Compte");
            }

            var employee = _context.Employes
                .Include(e => e.LeaveApplications)
                .FirstOrDefault(e => e.Id.ToString() == userId);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // GET: Employes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Employes.Include(e => e.Department).Include(e => e.SalaryBonuses);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Employes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var employe = await _context.Employes
                .Include(e => e.Department)
                .Include(e => e.SalaryBonuses)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (employe == null) return NotFound();

            return View(employe);
        }

        // GET: Employes/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Nom");
            ViewData["SalaryBonusId"] = new SelectList(_context.SalaryBonuses.Select(sb => new {
                sb.Id,
                Display = sb.Type + " (" + sb.Amount + ")"
            }), "Id", "Display");
            return View();
        }

        // POST: Employes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Email,Password,Salary,AnnualLeaveDays,PostePosition,HireDate,SalaryRaised,DepartmentId,SalaryBonusId")] Employe employe)
        {
            if (ModelState.IsValid)
            {

                _context.Add(employe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Nom", employe.DepartmentId);
            ViewData["SalaryBonusId"] = new SelectList(_context.SalaryBonuses.Select(sb => new {
                sb.Id,
                Display = sb.Type + " (" + sb.Amount + ")"
            }), "Id", "Display");

            return View(employe);
        }

        // GET: Employes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var employe = await _context.Employes.FindAsync(id);
            if (employe == null) return NotFound();

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Nom", employe.DepartmentId);
            ViewData["SalaryBonusId"] = new SelectList(_context.SalaryBonuses.Select(sb => new {
                sb.Id,
                Display = sb.Type + " (" + sb.Amount + ")"
            }), "Id", "Display");

            return View(employe);
        }

        // POST: Employes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Password,Salary,AnnualLeaveDays,PostePosition,HireDate,SalaryRaised,DepartmentId,SalaryBonusId")] Employe employe)
        {
            if (id != employe.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingEmploye = await _context.Employes.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

                    if (existingEmploye == null) return NotFound();

                    

                    _context.Update(employe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeExists(employe.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Nom", employe.DepartmentId);
            ViewData["SalaryBonusId"] = new SelectList(_context.SalaryBonuses.Select(sb => new {
                sb.Id,
                Display = sb.Type + " (" + sb.Amount + ")"
            }), "Id", "Display");

            return View(employe);
        }

        // GET: Employes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var employe = await _context.Employes
                .Include(e => e.Department)
                .Include(e => e.SalaryBonuses)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (employe == null) return NotFound();

            return View(employe);
        }

        // POST: Employes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employe = await _context.Employes.FindAsync(id);
            if (employe != null)
            {
                _context.Employes.Remove(employe);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        private bool EmployeExists(int id)
        {
            return _context.Employes.Any(e => e.Id == id);
        }
    }
}
