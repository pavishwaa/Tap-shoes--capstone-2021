using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TapShoesCanada.Data;
using TapShoesCanada.Models;

namespace TapShoesCanada.Controllers
{
    public class AddressController : Controller
    {
        private ShoeContext context;

        public AddressController(ShoeContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Address address)
        {
            if (ModelState.IsValid)
            {
               String UserID = Request.HttpContext.Session.GetString(SessionData.loggedUserID);
               address.UserId = Convert.ToInt32(UserID);
               context.Addresses.Add(address);
               if (context.SaveChanges() > 0)
               {
                   return RedirectToAction("List");
               }
               else
               {
                   ViewBag.message = "Failed to add address";
                   return View();
               }
            }
            else
            {
                return View(address);
            }
        }

        [HttpDelete]
        public IActionResult DeleteAddress(int id)
        {
            Address addressObj = context.Addresses.Find(id);
            if (addressObj != null)
            {
                context.Addresses.Remove(addressObj);
                context.SaveChanges();
            }

            return RedirectToAction("List");

        }
        public IActionResult Details(int id)
        {
            Address addressObj = context.Addresses.Find(id);
            if (addressObj != null)
            {
                return View(addressObj);
            }
            else
            {
                return RedirectToAction("List");
            }

        }

        public IActionResult Next(int id)
        {
            Request.HttpContext.Session.SetInt32(SessionData.selectedAddressID,id);
            return RedirectToAction("Payment","Cart");
        }

        public IActionResult List()
        {
            String UserID = Request.HttpContext.Session.GetString(SessionData.loggedUserID);
            List<Address> addresses = context.Addresses.Where(item => item.UserId.ToString().Equals(UserID)).ToList();
            return View(addresses);
        }

       
    }
}
