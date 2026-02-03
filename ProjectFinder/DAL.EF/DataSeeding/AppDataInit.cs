using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.DataSeeding;

public static class AppDataInit
{
    public static void SeedAppData(AppDbContext context)
    {
        SeedFolders(context);
        SeedProjectTypes(context);
        SeedProjectStatuses(context);
        SeedStepStatuses(context);
        
        context.SaveChanges();
    }

    public static void MigrateDatabase(AppDbContext context)
    {
        context.Database.Migrate();
    }

    public static void DeleteDatabase(AppDbContext context)
    {
        context.Database.EnsureDeleted();
    }

    public static void SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        foreach (var (roleName, id) in InitialData.Roles)
        {
            var role = roleManager.FindByNameAsync(roleName).Result;

            if (role != null) continue;

            role = new AppRole()
            {
                Id = id ?? Guid.NewGuid(),
                Name = roleName,
            };

            var result = roleManager.CreateAsync(role).Result;
            if (!result.Succeeded)
            {
                throw new ApplicationException("Role creation failed!");
            }
        }


        foreach (var userInfo in InitialData.Users)
        {
            var user = userManager.FindByEmailAsync(userInfo.name).Result;
            if (user == null)
            {
                user = new AppUser()
                {
                    Id = userInfo.id ?? Guid.NewGuid(),
                    Email = userInfo.name,
                    UserName = userInfo.name,
                    EmailConfirmed = true,
                    FirstName = userInfo.firstName,
                    LastName = userInfo.lastName,
                };
                var result = userManager.CreateAsync(user, userInfo.password).Result;
                if (!result.Succeeded)
                {
                    throw new ApplicationException("User creation failed!");
                }
            }

            foreach (var role in userInfo.roles)
            {
                if (userManager.IsInRoleAsync(user, role).Result)
                {
                    Console.WriteLine($"User {user.UserName} already in role {role}");
                    continue;
                }
                
                var roleResult = userManager.AddToRoleAsync(user, role).Result;
                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }
                else
                {
                    Console.WriteLine($"User {user.UserName} added to role {role}");
                }
            }
        }
    }
    
    private static void SeedFolders(AppDbContext context)
    {
        foreach (var (name, isVisible) in InitialData.Folders)
        {
            var folder = new Domain.Folder()
            {
                Name = name,
                IsVisible = isVisible,
                CreatedBy = "system",
                CreatedAt = DateTime.UtcNow,
            };
            
            if (context.Folders.Any(f => f.Name == name)) continue;
            
            var result = context.Folders.AddAsync(folder).Result;
            
            if (result.State == EntityState.Added)
            {
                Console.WriteLine($"Folder {name} added to database.");
            }
            else
            {
                Console.WriteLine($"Failed to add Folder {name} into database.");
            }
        }
    }
    
    private static void SeedStepStatuses(AppDbContext context)
    {
        foreach (var name in InitialData.StepStatuses)
        {
            var stepStatus = new Domain.StepStatus()
            {
                Name = name,
                CreatedBy = "system",
                CreatedAt = DateTime.UtcNow,
            };
            
            if (context.StepStatuses.Any(f => f.Name == name)) continue;
            
            var result = context.StepStatuses.AddAsync(stepStatus).Result;
            
            if (result.State == EntityState.Added)
            {
                Console.WriteLine($"Step status {name} added to database.");
            }
            else
            {
                Console.WriteLine($"Failed to add Step status {name} into database.");
            }
        }
    }
    
    private static void SeedProjectTypes(AppDbContext context)
    {
        foreach (var name in InitialData.ProjectTypes)
        {
            var projectType = new Domain.ProjectType()
            {
                Name = name,
                CreatedBy = "system",
                CreatedAt = DateTime.UtcNow,
            };
            
            if (context.ProjectTypes.Any(f => f.Name == name)) continue;
            
            var result = context.ProjectTypes.AddAsync(projectType).Result;
            
            if (result.State == EntityState.Added)
            {
                Console.WriteLine($"Project type {name} added to database.");
            }
            else
            {
                Console.WriteLine($"Failed to add Project type {name} into database.");
            }
        }
    }
    
    private static void SeedProjectStatuses(AppDbContext context)
    {
        foreach (var name in InitialData.ProjectStatuses)
        {
            var projectStatus = new Domain.ProjectStatus()
            {
                Name = name,
                CreatedBy = "system",
                CreatedAt = DateTime.UtcNow,
            };
            
            if (context.ProjectStatuses.Any(f => f.Name == name)) continue;
            
            var result = context.ProjectStatuses.AddAsync(projectStatus).Result;
            
            if (result.State == EntityState.Added)
            {
                Console.WriteLine($"Project status {name} added to database.");
            }
            else
            {
                Console.WriteLine($"Failed to add Project status {name} into database.");
            }
        }
    }
}
