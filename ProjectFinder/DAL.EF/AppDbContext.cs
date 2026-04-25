using Base.Contracts;
using Domain;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;

namespace DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>, AppUserRole, 
    IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public DbSet<Application> Applications { get; set; } = default!;
    public DbSet<Comment> Comments { get; set; } = default!;
    public DbSet<Folder> Folders { get; set; } = default!;
    public DbSet<FolderTag> FolderTags { get; set; } = default!;
    public DbSet<Group> Groups { get; set; } = default!;
    public DbSet<Invitation> Invitations { get; set; } = default!;
    public DbSet<Notification> Notifications { get; set; } = default!;
    public DbSet<Project> Projects { get; set; } = default!;
    public DbSet<ProjectFolder> ProjectFolders { get; set; } = default!;
    public DbSet<ProjectStatus> ProjectStatuses { get; set; } = default!;
    public DbSet<ProjectStep> ProjectSteps { get; set; } = default!;
    public DbSet<ProjectTag> ProjectTags { get; set; } = default!;
    public DbSet<ProjectType> ProjectTypes { get; set; } = default!;
    public DbSet<Step> Steps { get; set; } = default!;
    public DbSet<StepStatus> StepStatuses { get; set; } = default!;
    public DbSet<Tag> Tags { get; set; } = default!;
    public DbSet<UserGroup> UserGroups { get; set; } = default!;
    public DbSet<UserProject> UserProjects { get; set; } = default!;
    public DbSet<UserProjectRole> UserProjectRoles { get; set; } = default!;
    public DbSet<UserTag> UserTags { get; set; } = default!;
    
    public DbSet<AppRefreshToken> RefreshTokens { get; set; } = default!;
    
    private readonly IUserNameResolver _userNameResolver;
    private readonly ILogger<AppDbContext> _logger;
    
    public AppDbContext(DbContextOptions<AppDbContext> options, IUserNameResolver userNameResolver,
        ILogger<AppDbContext> logger)
        : base(options)
    {
        _userNameResolver = userNameResolver;
        _logger = logger;
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(), // Convert to UTC before saving
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc) // Ensure UTC when reading
        );

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(dateTimeConverter);
                }
            }
        }

        base.OnModelCreating(builder);
        
        // remove cascade delete
        foreach (var relationship in builder.Model
                     .GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        builder.Entity<AppUserRole>()
            .HasOne(a => a.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(a => a.UserId);

        builder.Entity<AppUserRole>()
            .HasOne(a => a.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(a => a.RoleId);
        
        
        builder.Entity<Invitation>()
            .HasOne(i => i.ToUser)
            .WithMany(u => u.OutgoingInvitations)
            .HasForeignKey(i => i.ToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Invitation>()
            .HasOne(i => i.User)
            .WithMany(u => u.IncomingInvitations)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict);

    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var addedEntries = ChangeTracker.Entries();
        foreach (var entry in addedEntries)
        {
            if (entry is { Entity: IDomainMeta })
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        (entry.Entity as IDomainMeta)!.CreatedAt = DateTime.UtcNow;
                        (entry.Entity as IDomainMeta)!.CreatedBy = _userNameResolver.CurrentUserName;
                        break;
                    case EntityState.Modified:
                        entry.Property("ChangedAt").IsModified = true;
                        entry.Property("ChangedBy").IsModified = true;
                        (entry.Entity as IDomainMeta)!.ChangedAt = DateTime.UtcNow;
                        (entry.Entity as IDomainMeta)!.ChangedBy = _userNameResolver.CurrentUserName;

                        // Prevent overwriting CreatedBy/CreatedAt on update
                        entry.Property("CreatedAt").IsModified = false;
                        entry.Property("CreatedBy").IsModified = false;
                        break;
                }
            }

            if (entry is { Entity: IDomainUserId, State: EntityState.Modified })
            {
                // do not allow userid modification
                entry.Property("UserId").IsModified = false;
                _logger.LogWarning("UserId modification attempt. Denied!");
            }
        }


        return base.SaveChangesAsync(cancellationToken);
    }
}
