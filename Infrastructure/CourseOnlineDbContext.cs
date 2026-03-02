using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public sealed class CourseOnlineDbContext(DbContextOptions<CourseOnlineDbContext> options)
    : DbContext(options)
{
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<CourseOccasion> CourseOccasions => Set<CourseOccasion>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Description)
                .HasMaxLength(2000);

            entity.HasMany(x => x.Occasions)
                .WithOne(x => x.Course)
                .HasForeignKey(x => x.CourseId);
        });

        modelBuilder.Entity<CourseOccasion>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.StartDate).HasConversion(
                v => v.ToDateTime(TimeOnly.MinValue),
                v => DateOnly.FromDateTime(v));

            entity.Property(x => x.EndDate).HasConversion(
                v => v.ToDateTime(TimeOnly.MinValue),
                v => DateOnly.FromDateTime(v));

            entity.HasOne(x => x.Teacher)
                .WithMany(x => x.CourseOccasions)
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.Property(x => x.Capacity).IsRequired();
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(200);

            entity.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(200);

            entity.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(x => new { x.StudentId, x.CourseOccasionId });

            entity.HasOne(x => x.Student)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.StudentId);

            entity.HasOne(x => x.CourseOccasion)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.CourseOccasionId);

            entity.Property(x => x.CreatedAtUtc).IsRequired();
        });
    }
}