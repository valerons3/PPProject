using Microsoft.EntityFrameworkCore;
using SearchWork.Models.Entity;

namespace SearchWork.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


    public DbSet<Application> Applications { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Interview> Interviews { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Resume> Resumes { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Vacancy> Vacancies { get; set; }
    public DbSet<CategoryRequest> CategoryRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User - Role (Many-to-One)
        modelBuilder.Entity<User>()
            .HasOne(u => u.UserRole)
            .WithMany(r => r.RoleUsers)
            .HasForeignKey(u => u.RoleId);

        // User - Company (One-to-One)
        modelBuilder.Entity<User>()
            .HasOne(u => u.Company)
            .WithOne(c => c.User)
            .HasForeignKey<Company>(c => c.UserId);

        // User - Resume (One-to-One)
        modelBuilder.Entity<User>()
            .HasOne(u => u.UserResume)
            .WithOne(r => r.User)
            .HasForeignKey<Resume>(r => r.UserId);

        // User - Applications (One-to-Many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.UserApplications)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId);

        // User - Notifications (One-to-Many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.UserNotifications)
            .WithOne(n => n.Users)
            .HasForeignKey(n => n.UserId);

        // User - Reviews (One-to-Many) (Reviewer & Reviewed)
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Reviewer)
            .WithMany(u => u.GivenReviews)
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Reviewed)
            .WithMany(u => u.ReceivedReviews)
            .HasForeignKey(r => r.ReviewedId)
            .OnDelete(DeleteBehavior.Restrict);

        // Company - Vacancies (One-to-Many)
        modelBuilder.Entity<Company>()
            .HasMany(c => c.CompanyVacancies)
            .WithOne(v => v.Company)
            .HasForeignKey(v => v.CompanyId);

        // Vacancy - Category (Many-to-One)
        modelBuilder.Entity<Vacancy>()
            .HasOne(v => v.Company)
            .WithMany(c => c.CompanyVacancies)
            .HasForeignKey(v => v.CompanyId);

        modelBuilder.Entity<Vacancy>()
            .HasOne<Category>()
            .WithMany(c => c.CategoryVacancies)
            .HasForeignKey(v => v.CategoryId);

        // Application - Vacancy (Many-to-One)
        modelBuilder.Entity<Application>()
            .HasOne<Vacancy>()
            .WithMany()
            .HasForeignKey(a => a.VacansyId);

        // Application - Interview (One-to-One)
        modelBuilder.Entity<Application>()
            .HasOne(a => a.interview)
            .WithOne(i => i.Application)
            .HasForeignKey<Interview>(i => i.ApplicationId);

        modelBuilder.Entity<CategoryRequest>()
            .HasKey(c => c.RequestId);
    }
}