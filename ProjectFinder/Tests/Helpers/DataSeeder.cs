using DAL.EF;
using Domain;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Helpers;

public static class DataSeeder
{
    public static void SeedData(IServiceProvider rootServices)
    {
        SeedAsync(rootServices).GetAwaiter().GetResult();
    }

    public static async Task SeedAsync(IServiceProvider rootServices)
    {
        using var scope = rootServices.CreateScope();
        var sp = scope.ServiceProvider;

        var userManager = sp.GetRequiredService<UserManager<AppUser>>();
        var roleManager = sp.GetRequiredService<RoleManager<AppRole>>();
        var ctx = sp.GetRequiredService<AppDbContext>();

        await EnsureRoleAsync(roleManager, "user");
        await EnsureRoleAsync(roleManager, "student");
        await EnsureRoleAsync(roleManager, "teacher");
        await EnsureRoleAsync(roleManager, "admin");
        
        await EnsureUserAsync(userManager, TestUsers.UserAId, TestUsers.UserAEmail, TestUsers.Password);
        await EnsureUserAsync(userManager, TestUsers.UserBId, TestUsers.UserBEmail, TestUsers.Password);
        await EnsureUserAsync(userManager, TestUsers.AdminUserId, TestUsers.AdminUserEmail, TestUsers.Password);
        await EnsureUserAsync(userManager, TestUsers.StudentAId, TestUsers.StudentAEmail, TestUsers.Password);
        
        await userManager.AddToRoleAsync(await userManager.FindByIdAsync(TestUsers.AdminUserId.ToString()), "admin");
        await userManager.AddToRoleAsync(await userManager.FindByIdAsync(TestUsers.StudentAId.ToString()), "student");

        await SeedProjectTypesAsync(ctx);
        await SeedProjectStatusesAsync(ctx);
        await SeedUserProjectRolesAsync(ctx);
    }

    private static async Task SeedProjectTypesAsync(AppDbContext ctx)
    {
        if (!ctx.ProjectTypes.Any(i => i.Id == TestUsers.ProjectTypeAId))
        {
            ctx.ProjectTypes.Add(new ProjectType()
            {
                Id = TestUsers.ProjectTypeAId,
                Name = TestUsers.ProjectTypeAName,
            });
        }
    
        await ctx.SaveChangesAsync();
    }

    private static async Task SeedProjectStatusesAsync(AppDbContext ctx)
    {
        if (!ctx.ProjectStatuses.Any(i => i.Id == TestUsers.ProjectStatusAId))
        {
            ctx.ProjectStatuses.Add(new ProjectStatus()
            {
                Id = TestUsers.ProjectStatusAId,
                Name = TestUsers.ProjectStatusAName,
            });
        }
        
        await ctx.SaveChangesAsync();
    }

    private static async Task SeedUserProjectRolesAsync(AppDbContext ctx)
    {
        if (!ctx.UserProjectRoles.Any(i => i.Id == TestUsers.AuthorRoleId))
        {
            ctx.UserProjectRoles.Add(new UserProjectRole()
            {
                Id = TestUsers.AuthorRoleId,
                Name = TestUsers.AuthorRoleName,
            });
        }
        
        if (!ctx.UserProjectRoles.Any(i => i.Id == TestUsers.SupervisorRoleId))
        {
            ctx.UserProjectRoles.Add(new UserProjectRole()
            {
                Id = TestUsers.SupervisorRoleId,
                Name = TestUsers.SupervisorRoleName,
            });
        }
        
        if  (!ctx.UserProjectRoles.Any(i => i.Id == TestUsers.ExternalSupervisorRoleId))
        {
            ctx.UserProjectRoles.Add(new UserProjectRole()
            {
                Id = TestUsers.ExternalSupervisorRoleId,
                Name = TestUsers.ExternalSupervisorRoleName,
            });
        }
        
        if (!ctx.UserProjectRoles.Any(i => i.Id == TestUsers.ExecutorRoleId))
        {
            ctx.UserProjectRoles.Add(new UserProjectRole()
            {
                Id = TestUsers.ExecutorRoleId,
                Name = TestUsers.ExecutorRoleName,
            });
        }
        
        await ctx.SaveChangesAsync();
    }

    private static async Task EnsureUserAsync(UserManager<AppUser> userManager, Guid id, string email, string password)
    {
        if (await userManager.FindByEmailAsync(email) != null) return;

        var user = new AppUser
        {
            Id = id,
            UserName = email,
            FirstName = "Test",
            LastName = "User",
            Email = email,
            EmailConfirmed = true,
        };

        var res = await userManager.CreateAsync(user, password);
        if (!res.Succeeded)
        {
            throw new InvalidOperationException(
                $"Failed to create test user {email}: " +
                string.Join(", ", res.Errors.Select(e => e.Description)));
        }
    }

    private static async Task EnsureRoleAsync(RoleManager<AppRole> roleManager, string roleName)
    {
        if (await roleManager.FindByNameAsync(roleName) != null) return;

        var result = await roleManager.CreateAsync(new AppRole
        {
            Id = Guid.NewGuid(),
            Name = roleName,
        });

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                $"Failed to create role {roleName}: " +
                string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
