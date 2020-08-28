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
        public DbSet<GitRepo> GitRepos { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<GitRepoTag> GitRepoTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GitRepoTag>().HasKey(gt => new {gt.GitRepoId, gt.TagId});

            modelBuilder.Entity<GitRepo>()
                .HasOne<User>(u => u.User)
                .WithMany(g => g.GitRepos)
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