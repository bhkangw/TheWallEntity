using System;
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
    }

    // public class Message : BaseEntity
    // {
    //     [Key]
    //     public int MessageId {get; set;}

    // }

    // public class Comment : BaseEntity
    // {
    //     [Key]
    //     public int CommentId { get; set; }

    // }
}