using Microsoft.EntityFrameworkCore;
using TeamTasksApi.Models;

namespace TeamTasksApi.Data;

public class TeamTasksDbContext : DbContext
{
    public TeamTasksDbContext(DbContextOptions<TeamTasksDbContext> options) : base(options)
    {
    }

    public DbSet<Developer> Developers { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<Models.Task> Tasks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Developer configuration
        modelBuilder.Entity<Developer>(entity =>
        {
            entity.ToTable("Developers");
            entity.HasKey(e => e.DeveloperId);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        // Project configuration
        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("Projects");
            entity.HasKey(e => e.ProjectId);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ClientName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.StartDate).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        // Task configuration
        modelBuilder.Entity<Models.Task>(entity =>
        {
            entity.ToTable("Tasks");
            entity.HasKey(e => e.TaskId);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Priority).IsRequired().HasMaxLength(50);
            entity.Property(e => e.EstimatedComplexity).IsRequired();
            entity.Property(e => e.DueDate).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            // Relationships
            entity.HasOne(e => e.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Assignee)
                .WithMany(d => d.Tasks)
                .HasForeignKey(e => e.AssigneeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
