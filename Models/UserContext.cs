using Microsoft.EntityFrameworkCore;

namespace TheWallEntity.Models
{
    public class UserContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public DbSet<User> users {get;set;}
        public DbSet<Message> messages {get;set;}
        public DbSet<Comment> comments {get;set;}
    }
}