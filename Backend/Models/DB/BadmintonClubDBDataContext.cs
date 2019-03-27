using System;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models.DB
{
    public class BadmintonClubDBDataContext : DbContext
    {
        // Public Properties
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Statistics> Statistics { get; set; }
        public DbSet<User> Users { get; set; }

        // Constructors
        public BadmintonClubDBDataContext(DbContextOptions<BadmintonClubDBDataContext> options)
            : base(options) => Database.EnsureCreated();

        // Overridden Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Player)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Opponent)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Matches)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .Property<Guid>("SeasonId");

            // Default Values
            modelBuilder.Entity<Statistics>()
                .Property(s => s.GamesDrawn)
                .HasDefaultValue(0);

            modelBuilder.Entity<Statistics>()
                .Property(s => s.GamesLost)
                .HasDefaultValue(0);

            modelBuilder.Entity<Statistics>()
                .Property(s => s.GamesWon)
                .HasDefaultValue(0);

            modelBuilder.Entity<Statistics>()
                .Property(s => s.PointsAgainst)
                .HasDefaultValue(0);

            modelBuilder.Entity<Statistics>()
                .Property(s => s.PointsFor)
                .HasDefaultValue(0);

            modelBuilder.Entity<User>()
                .Property(u => u.Title)
                .HasDefaultValue("Member");

            // Auto generate GUIDs
            modelBuilder.Entity<BlogPost>()
                .Property(bp => bp.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Match>()
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Season>()
                .Property(s => s.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Statistics>()
                .Property(st => st.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .HasDefaultValueSql("NEWID()");
        }
    }
}