using GitTagApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GitTagApp.Repositories.Context
{
    public class MainContext : DbContext
    {
        private IConfiguration Configuration { get; }
        
        public MainContext(DbContextOptions options) : base(options)
        {
        }
        
        public MainContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<GitRepository> GitRepositories { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GitRepositoryTag>().HasKey(gt => new {gt.GitRepositoryId, gt.TagId});

            modelBuilder.Entity<GitRepository>()
                .HasOne<User>(u => u.User)
                .WithMany(g => g.GitRepositories)
                .HasForeignKey(u => u.UserId);
            
            base.OnModelCreating(modelBuilder);
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("Server=127.0.0.1,1433;Database=GitTagApp;User Id=SA;Password=Ed=ME15432309");
            }
        }
    }
}