using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Employee_Management_System.Data;
using Employee_Management_System.Models;

namespace Employee_Management_System.Controllers
{
    public class SalaryBonusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalaryBonusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SalaryBonus
        public async Task<IActionResult> Index()
        {
            return View(await _context.SalaryBonuses.ToListAsync());
        }

        // GET: SalaryBonus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryBonus = await _context.SalaryBonuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salaryBonus == null)
            {
                return NotFound();
            }

            return View(salaryBonus);
        }

        // GET: SalaryBonus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SalaryBonus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,Description,Amount")] SalaryBonus salaryBonus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salaryBonus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(salaryBonus);
        }

        // GET: SalaryBonus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryBonus = await _context.SalaryBonuses.FindAsync(id);
            if (salaryBonus == null)
            {
                return NotFound();
            }
            return View(salaryBonus);
        }

        // POST: SalaryBonus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Description,Amount")] SalaryBonus salaryBonus)
        {
            if (id != salaryBonus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salaryBonus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryBonusExists(salaryBonus.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(salaryBonus);
        }

        // GET: SalaryBonus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryBonus = await _context.SalaryBonuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salaryBonus == null)
            {
                return NotFound();
            }

            return View(salaryBonus);
        }

        // POST: SalaryBonus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salaryBonus = await _context.SalaryBonuses.FindAsync(id);
            if (salaryBonus != null)
            {
                _context.SalaryBonuses.Remove(salaryBonus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaryBonusExists(int id)
        {
            return _context.SalaryBonuses.Any(e => e.Id == id);
        }
    }
}
