using BeFit.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BeFit.Models.ExerciseType> ExerciseType { get; set; } = default!;
        public DbSet<BeFit.Models.Exercise> Exercise { get; set; } = default!;
        public DbSet<BeFit.Models.ExerciseSession> ExerciseSession { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "8895f9cd-6508-4cf7-8948-0edb4e6fd3f1",
                Name = "Adult",
                NormalizedName = "ADULT",
                ConcurrencyStamp = "8895f9cd-6508-4cf7-8948-0edb4e6fd3f1"
            });

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "8895f9cd-6508-4cf7-8948-0edb4e6fd3f2",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
                ConcurrencyStamp = "8895f9cd-6508-4cf7-8948-0edb4e6fd3f2"
            });


        }
    }
}