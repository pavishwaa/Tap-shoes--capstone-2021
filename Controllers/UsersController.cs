 using System;
using System.Collections.Generic;
using System.Linq;
 using System.Security.Claims;
 using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
 using Microsoft.AspNetCore.Authentication;
 using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.Http;
 using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TapShoesCanada.Data;
using TapShoesCanada.Models;

 namespace TapShoesCanada.Controllers
{
    public class UsersController : Controller
    {    

		private readonly ShoeContext _context;
		


			public UsersController(ShoeContext context)
			{
				_context = context;
			}

		public IActionResult Index()
		{
			
				return View();
			
			
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Register(User user)
		{
			_context.Add(user);
			_context.SaveChanges();
			ViewBag.message = "The user " + user.FirstName + " " + user.LastName + " is added succesfully!";
			return RedirectToAction("Login");
		}


		public IActionResult Login(Boolean checkoutLogin,String message)
        {
            if (checkoutLogin) ViewBag.message = "Login to checkout!";
            if (message != "")
            {
                ViewBag.message = message;
            } 
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public IActionResult Login(User user)
		{
			
			var _user = _context.Users.Where(s => s.Email == user.Email);
			if (_user.Any())
			{

				if (_user.Where(s => s.Password == user.Password).Any())
                {
                    User userObj = _user.Where(s => s.Password == user.Password).FirstOrDefault();
                    HttpContext.Session.SetString(SessionData.loggedUserID,userObj.UserId.ToString());
                    ViewBag.message = "Login Successfull!";
                    return RedirectToAction("Index", "Home");
					//return Json(new { status = true, message = "Login Successfull!" });
				}
				else
				{
					ViewBag.message = "Incorrect password!";

					//return Json(new { status = false, message = "Invalid Password!" });
					return View();
				}
			}
			else
			{
				//return Json(new { status = false, message = "Invalid Email!" });

				ViewBag.message = "Incorrect E-mail address!";

				return View();

			}
		}

			

        public ActionResult Logout()

		{


			return RedirectToAction("Login");
		}



		public static string GetMD5(string str)
		{
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] fromData = Encoding.UTF8.GetBytes(str);
			byte[] targetData = md5.ComputeHash(fromData);
			string byte2String = null;

			for (int i = 0; i < targetData.Length; i++)
			{
				byte2String += targetData[i].ToString("x2");

			}
			return byte2String;
		}

	
		
	}
}
