
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
        private readonly ShoeContext _context;

        public CustomShoesController(ShoeContext context)
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
            SelectList list1 = new SelectList(smodel); // ten available model of shoes
            ViewBag.Smodel = list1;

            string[] sole = new string[] { "Black", "Natural", "Wood" }; // options for sole
            SelectList list2 = new SelectList(sole);
            ViewBag.sole = list2;


            string[] Style = new string[] { "Regular", "Royal", "Eyelets" }; // for different syles of a shoe
           
            SelectList list = new SelectList(Style);
            ViewBag.Style = list;
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Custom_shoe cshoe)
        {

            

            if (ModelState.IsValid)
            {
                

                // set the selected parameters in custome shoes object
                cshoe.Size = Request.Form["Size"].ToString();
                cshoe.Style = Request.Form["style"].ToString();
                cshoe.Model = Request.Form["model"].ToString();
                cshoe.Colour1 = Request.Form["Colour1"].ToString();
                cshoe.Colour2 = Request.Form["Colour2"].ToString();
                cshoe.Sole = Request.Form["sole"].ToString();
                cshoe.Lace = Request.Form["Lace"].ToString();

                _context.Add(cshoe);
               await _context.SaveChangesAsync();
                
                return RedirectToAction("Index"); // Redirect to orders page of custom orders.
            }
            return View(RedirectToAction("Index"));


        }
       
    }
}
