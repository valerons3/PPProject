using Microsoft.EntityFrameworkCore;
using SearchWork.Models;

namespace SearchWork.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<JobCategory> JobCategories { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<Resume> Resumes { get; set; }
    public DbSet<Interview> Interviews { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().
            HasIndex(u => u.Email).
            IsUnique();

        modelBuilder.Entity<Company>().
            HasOne(c => c.Owner).
            WithOne().
            HasForeignKey<Company>(c => c.OwnerId);
        modelBuilder.Entity<Company>()
            .HasMany(c => c.Jobs)
            .WithOne(j => j.Company)
            .HasForeignKey(j => j.CompanyId);

        modelBuilder.Entity<Job>().
            HasOne(j => j.Category).
            WithMany(c => c.Jobs).
            HasForeignKey(j => j.CategoryId);
        modelBuilder.Entity<Job>().
            HasMany(j => j.Applications).
            WithOne(a => a.Job).
            HasForeignKey(a => a.JobId);

        modelBuilder.Entity<Application>().
            HasOne(a => a.Seeker).
            WithMany().
            HasForeignKey(a => a.SeekerId);

        modelBuilder.Entity<Resume>().
            HasOne(r => r.Seeker)
            .WithMany().
            HasForeignKey(r => r.SeekerId);

        modelBuilder.Entity<Interview>()
            .HasOne(i => i.Application)
            .WithMany()
            .HasForeignKey(i => i.ApplicationId);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Seeker)
            .WithMany()
            .HasForeignKey(r => r.SeekerId);
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Job)
            .WithMany()
            .HasForeignKey(r => r.JobId);

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.UserId);

        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.Seeker)
            .WithMany()
            .HasForeignKey(s => s.SeekerId);

        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.Category)
            .WithMany()
            .HasForeignKey(s => s.CategoryId);
    }
}