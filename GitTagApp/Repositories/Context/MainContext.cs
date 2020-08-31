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

            modelBuilder.Entity<GitRepoTag>()
                .HasOne<GitRepo>(gt => gt.GitRepo)
                .WithMany(t => t.GitRepoTags)
                .HasForeignKey(gt => gt.GitRepoId);

            modelBuilder.Entity<GitRepoTag>()
                .HasOne<Tag>(gt => gt.Tag)
                .WithMany(g => g.GitRepoTags)
                .HasForeignKey(gt => gt.TagId);

            modelBuilder.Entity<GitRepo>()
                .HasKey(x => x.Id);
            
            modelBuilder.Entity<GitRepo>()
                .Property(x => x.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<GitRepo>()
                .HasOne(u => u.User)
                .WithMany(g => g.GitRepos)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);
            
            modelBuilder.Entity<User>()
                .Property(x => x.Id)
                .ValueGeneratedNever();
            
            base.OnModelCreating(modelBuilder);
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration["GithubOauth:ConnectionString"]);
            }
        }
    }
}