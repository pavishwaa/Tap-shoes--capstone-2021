
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
                int UserID = Convert.ToInt32(HttpContext.Session.GetString(SessionData.loggedUserID));
                if (UserID == 0) UserID = -1;

                CartCustomShoe cartCustomShoe = new CartCustomShoe();
                cartCustomShoe.CartUserId = UserID;
                cartCustomShoe.PaymentStatus = PaymentStatus.Pending.ToString();
                cartCustomShoe.Price = ConfigParams.CUSTOM_SHOE_FIXED_PRICE;
                cartCustomShoe.Qty = 1;
                cartCustomShoe.TotalPrice = ConfigParams.CUSTOM_SHOE_FIXED_PRICE;
                cartCustomShoe.Shoe = cshoe;
                _context.CartCustomShoes.Add(cartCustomShoe);
                await _context.SaveChangesAsync();
                int cartItemId = cartCustomShoe.CartItemId;
                if (!Request.Cookies.ContainsKey(ConfigParams.CUSTOM_CART_ITEM_COOKIE_NAME))
                {
                    CookieOperations.Set(Response.Cookies,ConfigParams.CUSTOM_CART_ITEM_COOKIE_NAME,cartItemId.ToString());
                }
                else
                {
                    String customCartItems = CookieOperations.Read(Request.Cookies, ConfigParams.CUSTOM_CART_ITEM_COOKIE_NAME);
                    customCartItems += "," + cartItemId.ToString();
                    CookieOperations.Set(Response.Cookies,ConfigParams.CUSTOM_CART_ITEM_COOKIE_NAME,customCartItems);
                }
            }
            return RedirectToAction("Index");

        }

        [ActionName("CookieCart")]
        public String GetCartItemsFromCookies()
        {
            return CookieOperations.Read(Request.Cookies, ConfigParams.CUSTOM_CART_ITEM_COOKIE_NAME);
        }

        [ActionName("ClearCookie")]
        public String ClearCookie()
        {
            CookieOperations.Remove(Response.Cookies,ConfigParams.CUSTOM_CART_ITEM_COOKIE_NAME);
            CookieOperations.Remove(Response.Cookies,ConfigParams.CART_ITEM_COOKIE_NAME);
            return "True";
        }       
    }
}
