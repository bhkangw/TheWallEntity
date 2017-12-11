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

        private User ActiveUser // creates a new User instance using the id of the logged in user
        {
            get{ return _context.users.Where(u => u.UserId == HttpContext.Session.GetInt32("id")).FirstOrDefault();} // returns one user object where UserId matches session's
        }

        [HttpGet]
        [Route("thewall")]
        public IActionResult Index() // once the user successfully logs in
        {
            if(ActiveUser == null)
                return RedirectToAction("Index", "Home");
            
            ViewBag.UserInfo = ActiveUser; // uses the ActiveUser instance as the current user for id, name, etc..
            // seems that the ViewBags below are necessary to include in every method?
            ViewBag.messages = _context.messages.Include(m => m.User).ThenInclude(m => m.Comment).ToList(); // Grabs messages, includes User and Comment tables
            ViewBag.comments = _context.comments.Include(m => m.User).ThenInclude(m => m.Message).ToList(); // Grabs comments, includes User and Message tables
            return View();
        }

        [HttpPost]
        [Route("postmessage")]
        public IActionResult PostMessage(ViewMessage messagepost) // posts from a form containing ViewMessage contents
        {
            if (ActiveUser == null) // checks to see if a valid user is logged in
                return RedirectToAction("Index", "Home");

            if(ModelState.IsValid) // if ViewMessage validation is clear, create new Message object
            {
                Message message = new Message
                {
                    UserId = ActiveUser.UserId, 
                    MessageContent = messagepost.MContent,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.messages.Add(message); // add new Message object into the db
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserInfo = ActiveUser; // must redefine ViewBag for each new page refresh?
            ViewBag.messages = _context.messages.Include(m => m.User).ThenInclude(m => m.Comment).ToList();
            ViewBag.comments = _context.comments.Include(m => m.User).ThenInclude(m => m.Message).ToList();
            return View("Index");
        }

        [HttpPost]
        [Route("postcomment/{MessageId}")] // must specify message id to be able to pass in as a parameter 
        public IActionResult PostComment (ViewComment commentpost, int MessageId) // takes in ViewComment from form and MessageId
        {
            if (ActiveUser == null) // checks again if valid user
                return RedirectToAction("Index", "Home");

            if(ModelState.IsValid) // if ViewComment validation are clear, create new comment instance
            {
                Comment comment = new Comment
                {
                    UserId = ActiveUser.UserId, // id of the current user logged in, creating the comment
                    MessageId = MessageId, // id of the message the comment is being left on
                    CommentContent = commentpost.CContent,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.comments.Add(comment); // add the comment instance into the db
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserInfo = ActiveUser; 
            ViewBag.messages = _context.messages.Include(m => m.User).ThenInclude(m => m.Comment).ToList();
            ViewBag.comments = _context.comments.Include(m => m.User).ThenInclude(m => m.Message).ToList();
            return View("Index");
        }

        [HttpGet]
        [Route("deletemessage/{MessageId}")]
        public IActionResult DeleteMessage (int MessageId) // delete message (if not a parent of comments)
        {
            Message ToDelete = _context.messages.Where(message => message.MessageId == MessageId).SingleOrDefault(); // locate the message object where id = id
            _context.messages.Remove(ToDelete); // delete the message object
            _context.SaveChanges();

            ViewBag.UserInfo = ActiveUser;
            ViewBag.messages = _context.messages.Include(m => m.User).ThenInclude(m => m.Comment).ToList();
            ViewBag.comments = _context.comments.Include(m => m.User).ThenInclude(m => m.Message).ToList();
            return View("Index");
        }

        [HttpGet]
        [Route("deletecomment/{CommentId}")]
        public IActionResult DeleteComment (int CommentId)
        {
            Comment ToDelete = _context.comments.Where(comment => comment.CommentId == CommentId).SingleOrDefault(); // locate the comment object where id = id
            _context.comments.Remove(ToDelete); // delete the comment object
            _context.SaveChanges();

            ViewBag.UserInfo = ActiveUser;
            ViewBag.messages = _context.messages.Include(m => m.User).ThenInclude(m => m.Comment).ToList();
            ViewBag.comments = _context.comments.Include(m => m.User).ThenInclude(m => m.Message).ToList();
            return View("Index");
        }
    }
}