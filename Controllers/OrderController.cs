using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TapShoesCanada.Data;
using TapShoesCanada.Models;

namespace TapShoesCanada.Controllers
{
    public class OrderController : Controller
    {
        private ShoeContext _context;

        public OrderController(ShoeContext _shoeContext)
        {
            this._context = _shoeContext;
        }
        public IActionResult Index()
        {
            String UserID = Request.HttpContext.Session.GetString(SessionData.loggedUserID);
            if (UserID != null)
            {
                List<Order> orders = _context.Orders.Include(item=>item.Address).Where(item => item.Address.User.UserId.ToString().Equals(UserID)).ToList();
                return View(orders);
            }
            return RedirectToAction("Index","Home");
        }

        public IActionResult Details(int id)
        {
            Order order = _context.Orders.Find(id);
            if(order != null) {
                CustomCart cartObj = new CustomCart();

                String[] cartStrings = order.CartItems.Split(',');
                List<Cart> carts = this._context.Carts
                    .Include(item=>item.Shoe)
                    .Where(item => cartStrings.Contains(item.CartItemId.ToString()))
                    .ToList();
                cartStrings = order.CustomCartItems.Split(',');
                List<CartCustomShoe> customCarts = this._context.CartCustomShoes
                    .Include(item=>item.Shoe)
                    .Where(item => cartStrings.Contains(item.CartItemId.ToString()))
                    .ToList();
                cartObj.Carts = carts;
                cartObj.CustomCarts = customCarts;
                return View(cartObj);
            }

            return RedirectToAction("Index");
        }
    }
}
