
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
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



            string[] smodel = new string[] { "Jazz Tap Master", "Triple Threat", "Tap Boot", "Rhythm HeelsPro", "Broadway Diva", "Classic Jane", "New Show Girl", "T Step", "La Coquette", "Sportap" };
            SelectList list1 = new SelectList(smodel);
            ViewBag.Smodel = list1;

            string[] sole = new string[] { "Black", "Natural", "Wood" };
            SelectList list2 = new SelectList(sole);
            ViewBag.sole = list2;


            string[] Style = new string[] { "Regular", "Royal", "Eyelets" }; // for different syles of a shoe
           
            SelectList list = new SelectList(Style);
            ViewBag.Style = list;
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Size,Style,Model,Colour1,Colour2,Sole,Lace")] Custom_shoe cshoe)
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
