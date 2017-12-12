using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheWallEntity.Models
{
    // the following classes are built specifically for insertion into the db. Models for validations are in ViewModels.cs
    public class User : BaseEntity
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        // a user can have many messages and comments therefore:
        public List<Message> Message { get; set; } // list to expect multiple message objects
        public List<Comment> Comment { get; set; } // list to expect multiple comment objects
        // must create an empty list in the single side of a one to many
        public User()
        {
            Message = new List<Message>(); // new empty list of messages
            Comment = new List<Comment>(); // new empty list of comments
        }
    }

    public class Message : BaseEntity
    {
        [Key]
        public int MessageId {get; set;}
        public int UserId {get; set;} // foreign key goes in the multiple side of a one to many
        public User User { get; set; } // User objects created along with the foreign key
        public string MessageContent {get; set;}
        // a message can have many messages therefore:
        public List<Comment> Comment { get; set; } // list to expect multiple comment objects
        // must create an empty list in the single side of a one to many
        public Message()
        {
            Comment = new List<Comment>();
        }
    }

    public class Comment : BaseEntity
    {
        [Key]
        public int CommentId { get; set; }
        public int MessageId { get; set; } // foreign key goes in the multiple side of a one to many
        public Message Message { get; set; } // Message objects created along with the foreign key
        public int UserId { get; set; } // foreign key goes in the multiple side of a one to many
        public User User { get; set; } // User objects created along with the foreign key
        public string CommentContent { get; set; }
    }
}