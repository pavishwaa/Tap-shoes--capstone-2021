 using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TapShoesCanada.Data;
using TapShoesCanada.Models;


namespace TapShoesCanada.Controllers
{
    public class UsersController : Controller
    {    

		private readonly UserContext _context;



			public UsersController(UserContext context)
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


		public IActionResult Login()
		{
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

	
		////GET: Register

		//public ActionResult Register()
		//{
		//	return View();
		//}


		//// GET: Users
		//public async Task<IActionResult> Index()
		//{
		//    return View(await _context.Users.ToListAsync());
		//}

		//// GET: Users/Details/5
		//public async Task<IActionResult> Details(int? id)
		//{
		//    if (id == null)
		//    {
		//        return NotFound();
		//    }

		//    var user = await _context.Users
		//        .FirstOrDefaultAsync(m => m.UserId == id);
		//    if (user == null)
		//    {
		//        return NotFound();
		//    }

		//    return View(user);
		//}

		//// GET: Users/Create
		//public IActionResult Create()
		//{
		//    return View();
		//}

		//// POST: Users/Create
		//// To protect from overposting attacks, enable the specific properties you want to bind to.
		//// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Create([Bind("UserId,FirstName,LastName,Email,Password")] User user)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        _context.Add(user);
		//        await _context.SaveChangesAsync();
		//        return RedirectToAction(nameof(Index));
		//    }
		//    return View(user);
		//}

		//// GET: Users/Edit/5
		//public async Task<IActionResult> Edit(int? id)
		//{
		//    if (id == null)
		//    {
		//        return NotFound();
		//    }

		//    var user = await _context.Users.FindAsync(id);
		//    if (user == null)
		//    {
		//        return NotFound();
		//    }
		//    return View(user);
		//}

		//// POST: Users/Edit/5
		//// To protect from overposting attacks, enable the specific properties you want to bind to.
		//// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Edit(int id, [Bind("UserId,FirstName,LastName,Email,Password")] User user)
		//{
		//    if (id != user.UserId)
		//    {
		//        return NotFound();
		//    }

		//    if (ModelState.IsValid)
		//    {
		//        try
		//        {
		//            _context.Update(user);
		//            await _context.SaveChangesAsync();
		//        }
		//        catch (DbUpdateConcurrencyException)
		//        {
		//            if (!UserExists(user.UserId))
		//            {
		//                return NotFound();
		//            }
		//            else
		//            {
		//                throw;
		//            }
		//        }
		//        return RedirectToAction(nameof(Index));
		//    }
		//    return View(user);
		//}

		//// GET: Users/Delete/5
		//public async Task<IActionResult> Delete(int? id)
		//{
		//    if (id == null)
		//    {
		//        return NotFound();
		//    }

		//    var user = await _context.Users
		//        .FirstOrDefaultAsync(m => m.UserId == id);
		//    if (user == null)
		//    {
		//        return NotFound();
		//    }

		//    return View(user);
		//}

		//// POST: Users/Delete/5
		//[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> DeleteConfirmed(int id)
		//{
		//    var user = await _context.Users.FindAsync(id);
		//    _context.Users.Remove(user);
		//    await _context.SaveChangesAsync();
		//    return RedirectToAction(nameof(Index));
		//}

		//private bool UserExists(int id)
		//{
		//    return _context.Users.Any(e => e.UserId == id);
		//}
	}
}
