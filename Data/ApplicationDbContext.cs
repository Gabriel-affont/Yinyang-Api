using Microsoft.EntityFrameworkCore;
using Yinyang_Api.Models;
namespace Yinyang_Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Request> Requests { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Skill)
                .WithMany(s => s.Requests)
                .HasForeignKey(r => r.SkillId);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Requester)
                .WithMany(u => u.RequestMade)
                .HasForeignKey(r => r.RequesterId);
            modelBuilder.Entity<Skill>()
                .HasOne(s => s.Owner)
                .WithMany(u => u.Skills)
                .HasForeignKey(s => s.OwnerId);
        }
    }
}
//"ConnectionStrings": {
//    "DefaultConnection": "Server=localhost;Database=YinYang;Trusted_Connection=True;TrustServerCertificate=True;"
//}

