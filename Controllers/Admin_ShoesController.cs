using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TapShoesCanada.Data;
using TapShoesCanada.Models;

namespace TapShoesCanada.Controllers
{
    public class Admin_ShoesController : Controller
    {
        private readonly ShoeContext _context;

        public Admin_ShoesController(ShoeContext context)
        {
            _context = context;
        }

        // GET: Admin_Shoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Shoes.ToListAsync());
        }

        // GET: Admin_Shoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin_Shoes = await _context.Shoes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admin_Shoes == null)
            {
                return NotFound();
            }

            return View(admin_Shoes);
        }

        // GET: Admin_Shoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin_Shoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Size,Style,Model,Colour1,Colour2,Sole,Lace,Img_Lk,Price,Model_Type")] Shoe admin_Shoes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(admin_Shoes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(admin_Shoes);
        }

        // GET: Admin_Shoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin_Shoes = await _context.Shoes.FindAsync(id);
            if (admin_Shoes == null)
            {
                return NotFound();
            }
            return View(admin_Shoes);
        }

        // POST: Admin_Shoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Size,Style,Model,Colour1,Colour2,Sole,Lace,Img_Lk,Price,Model_Type")] Shoe admin_Shoes)
        {
            if (id != admin_Shoes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admin_Shoes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Admin_ShoesExists(admin_Shoes.Id))
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
            return View(admin_Shoes);
        }

        // GET: Admin_Shoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin_Shoes = await _context.Shoes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admin_Shoes == null)
            {
                return NotFound();
            }

            return View(admin_Shoes);
        }

        // POST: Admin_Shoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var admin_Shoes = await _context.Shoes.FindAsync(id);
            _context.Shoes.Remove(admin_Shoes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Admin_ShoesExists(int id)
        {
            return _context.Shoes.Any(e => e.Id == id);
        }
    }
}
