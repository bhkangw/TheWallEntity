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
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterUser newUser) // takes in a RegisterUser object from register form
        {
            if (_context.users.Where(u => u.Email == newUser.Email).SingleOrDefault() != null)
                ModelState.AddModelError("Email", "Email already in use"); // checking if email already exists in the db

            PasswordHasher<RegisterUser> hasher = new PasswordHasher<RegisterUser>(); // necessary for password hashing
            
            if (ModelState.IsValid) // if model passes validation, create new User instance from User class
            {
                User User = new User 
                {
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Email = newUser.Email,
                    Password = hasher.HashPassword(newUser, newUser.Password),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Add(User); // add the new user object to the db
                _context.SaveChanges();

                HttpContext.Session.SetInt32("id", User.UserId); // save the user id in session
                return RedirectToAction("Index", "Wall"); // redirect to the Wall controller
            }
            return View("Index");
        }

        [HttpPost]
        [Route("/login")]
        public IActionResult Login(LoginUser logUser) // takes in the LoginUser from the login form
        {
            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();

            User userToLog = _context.users.Where(u => u.Email == logUser.LogEmail).SingleOrDefault();
            // User userToLog = _context.users.SingleOrDefault(u => u.Email == logUser.LogEmail); // can use this or above?
            
            if(userToLog == null) // if email is not in db..
                ModelState.AddModelError("LogEmail", "Cannot find Email");

            else if( hasher.VerifyHashedPassword(logUser, userToLog.Password, logUser.LogPassword) == 0) // if password does not match 
            {
                ModelState.AddModelError("LogPassword", "Wrong Password");
            }

            if(!ModelState.IsValid) // if form is empty or any other basic validation is failed
                return View("Index");

            HttpContext.Session.SetInt32("id", userToLog.UserId); // if pass all of the above, save the user id in session
            return RedirectToAction("Index", "Wall"); // and redirect to the Wall controller
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // clear all user data in session upon logout
            return RedirectToAction("Index");
        }
    }
}
