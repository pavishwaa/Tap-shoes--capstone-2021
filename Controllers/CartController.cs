using Microsoft.AspNetCore.Mvc;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
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
            CustomCart customCart = new CustomCart();

            String strUserID = HttpContext.Session.GetString(SessionData.loggedUserID);
            if (strUserID == null)
            {
                //Getting Custom Cart Data
                String CartItems = CookieOperations.Read(Request.Cookies, ConfigParams.CUSTOM_CART_ITEM_COOKIE_NAME);
                String[] items;

                if (CartItems != null && CartItems.Trim() != String.Empty)
                {
                    items = CartItems.Split(',');
                    customCart.CustomCarts = shoeContext.CartCustomShoes
                        .Where(item => items.Contains(item.CartItemId.ToString()))
                        .Include(item => item.Shoe).ToList();
                }

                //Getting Main Cart Data
                CartItems = CookieOperations.Read(Request.Cookies, ConfigParams.CART_ITEM_COOKIE_NAME);
                if(CartItems != null && CartItems.Trim() != String.Empty) { 
                    items = CartItems.Split(',');
                    customCart.Carts = shoeContext.Carts.Where(item => items.Contains(item.CartItemId.ToString()))
                        .Include(item => item.Shoe).ToList();
                }

            }
            else
            {
                String CartItems = CookieOperations.Read(Request.Cookies, ConfigParams.CUSTOM_CART_ITEM_COOKIE_NAME);
                String[] items;

                if (CartItems != null && CartItems.Trim() != String.Empty)
                {
                    items = CartItems.Split(',');
                    customCart.CustomCarts = shoeContext.CartCustomShoes
                        .Where(item => items.Contains(item.CartItemId.ToString()))
                        .Include(item => item.Shoe).ToList();
                }

                //Getting Main Cart Data
                CartItems = CookieOperations.Read(Request.Cookies, ConfigParams.CART_ITEM_COOKIE_NAME);
                if (CartItems != null && CartItems.Trim() != String.Empty)
                {
                    items = CartItems.Split(',');
                    customCart.Carts = shoeContext.Carts.Where(item => items.Contains(item.CartItemId.ToString()))
                        .Include(item => item.Shoe).ToList();
                }

                if (customCart.Carts != null)
                {
                    foreach (var obj in customCart.Carts)
                    {
                        obj.CartUserId = Convert.ToInt32(strUserID);
                    }
                    shoeContext.Carts.UpdateRange(customCart.Carts);
                    CookieOperations.Remove(Response.Cookies, ConfigParams.CART_ITEM_COOKIE_NAME);
                    shoeContext.SaveChanges();
                }

                if (customCart.CustomCarts != null) { 
                    foreach (var obj in customCart.CustomCarts)
                    {
                        obj.CartUserId = Convert.ToInt32(strUserID);
                    }
                    shoeContext.CartCustomShoes.UpdateRange(customCart.CustomCarts);
                    CookieOperations.Remove(Response.Cookies, ConfigParams.CUSTOM_CART_ITEM_COOKIE_NAME);
                    shoeContext.SaveChanges();
                }

                int UserID = Convert.ToInt32(strUserID);
                customCart.Carts = shoeContext.Carts.Where(item => item.CartUserId == UserID && item.PaymentStatus == PaymentStatus.Pending.ToString())
                 
                    .Include(item => item.Shoe).ToList();
                customCart.CustomCarts = shoeContext.CartCustomShoes.Where(item => item.CartUserId == UserID && item.PaymentStatus == PaymentStatus.Pending.ToString())
                    .Include(item => item.Shoe).ToList();
            }

            
            return View(customCart);
        
        }

        [HttpPost]
        public IActionResult Add(int? id,int qty, Custom_shoe? shoe)
        {
            Shoe shoeObj = shoeContext.Shoes.SingleOrDefault(item => item.Id == id);
            Cart cartItemObj = new Cart();
            if (shoeObj != null)
            {
                int UserID = Convert.ToInt32(HttpContext.Session.GetString(SessionData.loggedUserID));
                if (UserID == 0) UserID = -1;
                cartItemObj.CartUserId = UserID;
                cartItemObj.Price = shoeObj.Price;
                cartItemObj.Qty = qty;
                cartItemObj.ShoeId = (int)id;
                cartItemObj.TotalPrice = qty * shoeObj.Price;
                cartItemObj.PaymentStatus = PaymentStatus.Pending.ToString();
                shoeContext.Carts.Add(cartItemObj);

                shoeContext.SaveChanges();

                if(UserID == -1) { 
                    if (!Request.Cookies.ContainsKey(ConfigParams.CART_ITEM_COOKIE_NAME))
                    {
                        CookieOperations.Set(Response.Cookies, ConfigParams.CART_ITEM_COOKIE_NAME,cartItemObj.CartItemId.ToString());
                    }
                    else
                    {
                        String customCartItems = CookieOperations.Read(Request.Cookies, ConfigParams.CART_ITEM_COOKIE_NAME);
                        if (customCartItems != "") customCartItems += "," + cartItemObj.CartItemId.ToString();
                        else customCartItems = cartItemObj.CartItemId.ToString();
                        CookieOperations.Set(Response.Cookies, ConfigParams.CART_ITEM_COOKIE_NAME, customCartItems);
                    }
                }

                return RedirectToActionPermanent("Index");
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

                if (Request.Cookies.ContainsKey(ConfigParams.CART_ITEM_COOKIE_NAME))
                {
                    String customCartItems = CookieOperations.Read(Request.Cookies, ConfigParams.CART_ITEM_COOKIE_NAME);
                    List<String> Items = customCartItems.Split(',').ToList();
                    int deletedIndex = Items.IndexOf(cartId.ToString());
                    if (deletedIndex != -1) Items.RemoveAt(deletedIndex);
                    customCartItems = string.Join(',', Items);
                    CookieOperations.Set(Response.Cookies, ConfigParams.CART_ITEM_COOKIE_NAME, customCartItems);
                }
                shoeContext.Carts.Remove(cartObj);
                shoeContext.SaveChanges();
            }

            return RedirectToActionPermanent("Index");
        }
        [HttpPost]
        public IActionResult RemoveCustom(int cartId)
        {
            CartCustomShoe cartObj = shoeContext.CartCustomShoes.Find(cartId);
            if (cartObj != null)
            {
                if (Request.Cookies.ContainsKey(ConfigParams.CUSTOM_CART_ITEM_COOKIE_NAME))
                {
                    String customCartItems = CookieOperations.Read(Request.Cookies, ConfigParams.CUSTOM_CART_ITEM_COOKIE_NAME);
                    List<String> Items = customCartItems.Split(',').ToList();
                    int deletedIndex = Items.IndexOf(cartId.ToString());
                    if(deletedIndex != -1) Items.RemoveAt(deletedIndex);
                    customCartItems = string.Join(',', Items);
                    CookieOperations.Set(Response.Cookies, ConfigParams.CUSTOM_CART_ITEM_COOKIE_NAME, customCartItems);
                }

                shoeContext.CartCustomShoes.Remove(cartObj);
                shoeContext.SaveChanges();
            }

            return RedirectToActionPermanent("Index");
        }

        [HttpPost]
        public IActionResult Checkout()
        {
            String strUserID = HttpContext.Session.GetString(SessionData.loggedUserID);
            if (strUserID == null)
            {
                String strMessage = "Please login to checkout";
                return RedirectToAction("Login", "Users",new { checkoutLogin = true });
            }
            return RedirectToAction("List","Address");
        }

        public IActionResult Payment()
        {
            
            return View("Checkout");
        }

        [HttpPost]
        [ActionName("CheckoutData")]
        public IActionResult getCheckoutData()
        {
            List<Cart> cartItems = null;
            List<CartCustomShoe> customCartItems = null;
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
                customCartItems = shoeContext.CartCustomShoes.Where(item => item.CartUserId == UserID)
                    .Include(item => item.Shoe).ToList();
              
            }
            CustomCart customCartObj = new CustomCart();
            customCartObj.Carts = cartItems;
            customCartObj.CustomCarts = customCartItems;
            return new JsonResult(customCartObj);
        }
        [ActionName("CheckoutDataRead")]
        public IActionResult ReadingCartData()
        {
            return getCheckoutData();
        }

        [HttpPost]
        public IActionResult ProcessOrder(String transactionId)
        {
            String strUserID = HttpContext.Session.GetString(SessionData.loggedUserID);
            int? addressId = HttpContext.Session.GetInt32(SessionData.selectedAddressID);
            if (strUserID != null)
            {
                int UserID = Convert.ToInt32(strUserID);
                var 
                    cartItems = shoeContext.Carts.Where(item => item.CartUserId == UserID && item.PaymentStatus == PaymentStatus.Pending.ToString())
                    .Include(item => item.Shoe).ToList();

                var
                    customCartItems = shoeContext.CartCustomShoes.Where(item => item.CartUserId == UserID && item.PaymentStatus == PaymentStatus.Pending.ToString())
                        .Include(item => item.Shoe).ToList();
                
                String cartItemsStr = "";
                foreach (var cartItem in cartItems)
                {
                    cartItemsStr += cartItem.CartItemId + ",";
                    cartItem.PaymentStatus = PaymentStatus.Done.ToString();
                }
                cartItemsStr = cartItemsStr.Trim(',');

                String customCartItemsStr = "";
                foreach (var customCartItem in customCartItems)
                {
                    customCartItemsStr += customCartItem.CartItemId + ",";
                    customCartItem.PaymentStatus = PaymentStatus.Done.ToString();
                }
                customCartItemsStr = customCartItemsStr.Trim(',');
                shoeContext.Carts.UpdateRange(cartItems);
                shoeContext.CartCustomShoes.UpdateRange(customCartItems);
                
                Order orderObj = new Order();
                orderObj.TransactionId = transactionId;
                orderObj.CartItems = cartItemsStr;
                orderObj.CustomCartItems = customCartItemsStr;
                orderObj.AddressId = addressId??-1;
                orderObj.paymentStatus = PaymentStatus.Done.ToString();
                shoeContext.Orders.Add(orderObj);

                shoeContext.SaveChanges();
                return new JsonResult(new {
                    status = true
                });
            }

            return new JsonResult(new {
                status = false
            });
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
