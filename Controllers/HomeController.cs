using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TheWallEntity.Models;
using Microsoft.AspNetCore.Identity;

namespace TheWallEntity.Controllers
{
    public class HomeController : Controller
    {
        private UserContext _context;

        public HomeController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            // List<Person> AllUsers = _context.Users.ToList();
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterUser newUser)
        {
            if (_context.users.Where(u => u.Email == newUser.Email).SingleOrDefault() != null)
                ModelState.AddModelError("Username", "Username in use");

            if (ModelState.IsValid)
            {
                PasswordHasher<RegisterUser> hasher = new PasswordHasher<RegisterUser>();
                // insert user into DB
                User User = new User
                {
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Email = newUser.Email,
                    Password = hasher.HashPassword(newUser, newUser.Password),
                    CreatedAt = NOW(),
                };

                User theUser = _context.Add(User).Entity;
                _context.SaveChanges();

                HttpContext.Session.SetInt32("id", theUser.UserId);
                return RedirectToAction("thewall");
            }
            return View("Index");
        }

        [HttpPost]
        [Route("/login")]
        public IActionResult Login(LoginUser user)
        {
            if (ModelState.IsValid)
            {
                // insert user into DB
            }
            return View("Index");
        }

    }
}
