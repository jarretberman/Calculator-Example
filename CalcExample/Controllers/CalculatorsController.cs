using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CalcExample.Data;
using CalcExample.Models;
using Microsoft.AspNetCore.Authorization;

namespace CalcExample.Controllers
{
    public class CalculatorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalculatorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Calculators
        public async Task<IActionResult> Index()
        {
            return View(await _context.Calculator.ToListAsync());
        }

        // GET: Calculators/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calculator = await _context.Calculator
                .FirstOrDefaultAsync(m => m.Id == id);
            if (calculator == null)
            {
                return NotFound();
            }

            return View(calculator);
        }

        
        // GET: Calculators/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Calculators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create([Bind("Id,State,Industry,Employees")] Calculator calculator)
        {
            if (ModelState.IsValid)
            {
                _context.Add(calculator);
                await _context.SaveChangesAsync();
                return RedirectPreserveMethod(nameof(CalculateResults));
            }
            return Redirect("~");
        }

        // GET: Calculators/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calculator = await _context.Calculator.FindAsync(id);
            if (calculator == null)
            {
                return NotFound();
            }
            return View(calculator);
        }

        // POST: Calculators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,State,Industry,Employees")] Calculator calculator)
        {
            if (id != calculator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(calculator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CalculatorExists(calculator.Id))
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
            return View(calculator);
        }

        // GET: Calculators/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calculator = await _context.Calculator
                .FirstOrDefaultAsync(m => m.Id == id);
            if (calculator == null)
            {
                return NotFound();
            }

            return View(calculator);
        }

        // POST: Calculators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var calculator = await _context.Calculator.FindAsync(id);
            _context.Calculator.Remove(calculator);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //POST: Calculators/CalculateResults
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalculateResults([Bind("Id,State,Industry,Employees")] Calculator calculator)
        {
            ViewData["State"] = calculator.State;
            ViewData["Employees"] = calculator.Employees;
            ViewData["Industry"] = calculator.Industry;


            int dataAlgorithm(int st, int ind, int emp)
                {
                return st * ind * emp;
            };

            ViewData["Results"] = dataAlgorithm(int.Parse(calculator.State), int.Parse(calculator.Industry), calculator.Employees);

            return View(await _context.Calculator.ToListAsync());
        }

        private bool CalculatorExists(int id)
        {
            return _context.Calculator.Any(e => e.Id == id);
        }
    }
}
