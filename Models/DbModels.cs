using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheWallEntity.Models
{
    public class User : BaseEntity
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<Message> Message { get; set; }
        public List<Comment> Comment { get; set; }

        public User()
        {
            Message = new List<Message>();
            Comment = new List<Comment>();
        }
    }

    public class Message : BaseEntity
    {
        [Key]
        public int MessageId {get; set;}
        public int UserId {get; set;}
        public User User { get; set; }
        public string MessageContent {get; set;}

        public List<Comment> Comment { get; set; }
        
        public Message()
        {
            Comment = new List<Comment>();
        }
    }

    public class Comment : BaseEntity
    {
        [Key]
        public int CommentId { get; set; }
        public int MessageId { get; set; }
        public Message Message { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string CommentContent { get; set; }
    }
}