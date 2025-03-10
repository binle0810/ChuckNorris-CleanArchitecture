using Domain.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure;

public sealed class ApplicationDbContext : IdentityDbContext<AppUser>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<LikeJoke> LikeJokes { get; set; }
    public DbSet<UserLikeJoke> UserLikeJokes { get; set; }
   // public DbSet<Webinar> webinars { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
       modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.Entity<UserLikeJoke>()
        .HasKey(uj => new { uj.UserId, uj.JokeId });
        modelBuilder.Entity<UserLikeJoke>()
               .HasOne(u => u.User)
               .WithMany(u => u.LikedJokes)
               .HasForeignKey(p => p.UserId);

        modelBuilder.Entity<UserLikeJoke>()
            .HasOne(u => u.Joke)
            .WithMany(u => u.LikedByUsers)
            .HasForeignKey(p => p.JokeId);


        List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            };
        modelBuilder.Entity<IdentityRole>().HasData(roles);
    }
}