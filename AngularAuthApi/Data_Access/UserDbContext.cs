using AngularAuthApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace AngularAuthApi.Data_Access
{
    public class UserDbContext:DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options):base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Auth> Auth { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u=>u.Salt).HasColumnType("varbinary(128)");
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(u => u.Auth).WithOne(u=>u.User).HasForeignKey<Auth>(u=>u.Id);
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(u => u.UserInfo).WithOne(u => u.User).HasForeignKey<UserInfo>(u=>u.Id);
            });
                
        }



    }
}
