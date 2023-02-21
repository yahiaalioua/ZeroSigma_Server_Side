using AngularAuthApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace AngularAuthApi.Data_Access
{
    public class UserDbContext:DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options):base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Auth> Auth { get; set; } 



    }
}
