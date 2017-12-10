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
    public class WallController : Controller
    {
        private UserContext _context;

        public WallController(UserContext context)
        {
            _context = context;
        }

        private User ActiveUser
        {
            get{ return _context.users.Where(u => u.UserId == HttpContext.Session.GetInt32("id")).FirstOrDefault();}
        }

        [HttpGet]
        [Route("thewall")]
        public IActionResult Index()
        {
            if(ActiveUser == null)
                return RedirectToAction("Index", "Home");
            // User user = _context.users.Where(u => u.UserId == HttpContext.Session.GetInt32("id")).FirstOrDefault();
            // ViewBag.UserInfo = user;
            ViewBag.UserInfo = ActiveUser;

            ViewBag.messages = _context.messages.Include(m => m.User).ThenInclude(m => m.Comment).ToList();
            ViewBag.comments = _context.comments.Include(m => m.User).ThenInclude(m => m.Message).ToList();
            return View();
        }

        [HttpPost]
        [Route("postmessage")]
        public IActionResult PostMessage(ViewMessage messagepost)
        {
            if (ActiveUser == null)
                return RedirectToAction("Index", "Home");
            if(ModelState.IsValid)
            {
                Message message = new Message
                {
                    UserId = ActiveUser.UserId,
                    MessageContent = messagepost.MContent,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.messages.Add(message);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserInfo = ActiveUser;
            return View("Index");
        }

        [HttpPost]
        [Route("postcomment/{MessageId}")]
        public IActionResult PostComment (ViewComment commentpost, int MessageId)
        {
            if (ActiveUser == null)
                return RedirectToAction("Index", "Home");
            if(ModelState.IsValid)
            {
                Comment comment = new Comment
                {
                    UserId = ActiveUser.UserId,
                    MessageId = MessageId,
                    CommentContent = commentpost.CContent,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.comments.Add(comment);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserInfo = ActiveUser;
            return View("Index");
        }
    }
}