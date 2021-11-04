using Microsoft.AspNetCore.Mvc;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using TapShoesCanada.Data;
using TapShoesCanada.Models;

namespace TapShoesCanada.Controllers
{
    public class CartController : Controller
    {
        private ShoeContext shoeContext;

        public CartController(ShoeContext shoeContext)
        {
            this.shoeContext = shoeContext;
        }
        public IActionResult Index()
        {
            List<Cart> cartItems = null;
            String strUserID = HttpContext.Session.GetString(SessionData.loggedUserID);
            if (strUserID == null)
            {
                ViewBag.userNotFound = "UserNotFound";
            }
            else
            {
                int UserID = Convert.ToInt32(strUserID);
                cartItems = shoeContext.Carts.Where(item => item.CartUserId == UserID)
                    .Include(item => item.Shoe).ToList();
            }
            return View(cartItems);
        
        }

        [HttpPost]
        public IActionResult Add(int? id,int qty, Custom_shoe? shoe)
        {
            Shoe shoeObj = shoeContext.Shoes.SingleOrDefault(item => item.Id == id);
            Cart cartItemObj = new Cart();
            if (shoeObj != null)
            {
                int UserID = Convert.ToInt32(HttpContext.Session.GetString(SessionData.loggedUserID));

                cartItemObj.CartUserId = UserID;
                cartItemObj.Price = shoeObj.Price;
                cartItemObj.Qty = qty;
                cartItemObj.ShoeId = (int)id;
                cartItemObj.TotalPrice = qty * shoeObj.Price;
                cartItemObj.PaymentStatus = PaymentStatus.Done.ToString();
                shoeContext.Carts.Add(cartItemObj);

                if (shoeContext.SaveChanges() > 0)
                    return View("Details", shoeObj);
                else
                    return View("Index");
            }
            else
            {
                ViewBag.errormessage = "No Object Found";
                return View("Index");
            }
        }

        [HttpGet]
        public IActionResult View(int id)
        {
            Shoe shoe = shoeContext.Shoes.Find(id);
            return View("Details", shoe);
        }

        [HttpPost]
        public IActionResult Remove(int cartId)
        {
            Cart cartObj = shoeContext.Carts.Find(cartId);
            if (cartObj != null)
            {
                
                shoeContext.Carts.Remove(cartObj);
                shoeContext.SaveChanges();
            }

            return RedirectToActionPermanent("Index");
        }

        [HttpPost]
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        [ActionName("CheckoutData")]
        public IActionResult getCheckoutData()
        {
            List<Cart> cartItems = null;
            String strUserID = HttpContext.Session.GetString(SessionData.loggedUserID);
            if (strUserID == null)
            {
                ViewBag.userNotFound = "UserNotFound";
            }
            else
            {
                int UserID = Convert.ToInt32(strUserID);
                cartItems = shoeContext.Carts.Where(item => item.CartUserId == UserID)
                    .Include(item => item.Shoe).ToList();
            }

            return new JsonResult(cartItems);
        }
        [ActionName("CheckoutDataRead")]
        public IActionResult ReadingCartData()
        {
            List<Cart> cartItems = null;
            String strUserID = HttpContext.Session.GetString(SessionData.loggedUserID);
            if (strUserID == null)
            {
                ViewBag.userNotFound = "UserNotFound";
            }
            else
            {
                int UserID = Convert.ToInt32(strUserID);
                cartItems = shoeContext.Carts.Where(item => item.CartUserId == UserID)
                    .Include(item => item.Shoe).ToList();
            }

            return new JsonResult(cartItems);
        }


        public IActionResult ClearCart()
        {
            List<Cart> cartItems = null;

            String strUserID = HttpContext.Session.GetString(SessionData.loggedUserID);
            if (strUserID == null)
            {
                ViewBag.userNotFound = "UserNotFound";
            }
            else
            {
                int UserID = Convert.ToInt32(strUserID);
                cartItems = shoeContext.Carts.Where(item => item.CartUserId == UserID)
                    .Include(item => item.Shoe).ToList();

                shoeContext.Carts.RemoveRange(cartItems);
                shoeContext.SaveChanges();

            }

            return View();
        }


        
    }
}
