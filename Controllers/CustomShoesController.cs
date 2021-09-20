
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TapShoesCanada.Data;
using TapShoesCanada.Models;

namespace TapShoesCanada.Controllers
{
    public class CustomShoesController : Controller
    {
        private readonly CustomShoeContext _context;

        public CustomShoesController(CustomShoeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            
            return View(await _context.Custom_Shoes.ToListAsync());
        }

        public IActionResult Form()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Size,Style,Model,Colour1,Colour2,Sole,Lace")] Custom_shoe cshoe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cshoe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cshoe);
        }
    }
}
