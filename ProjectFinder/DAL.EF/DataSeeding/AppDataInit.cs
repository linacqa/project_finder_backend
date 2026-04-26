using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL.EF.DataSeeding;

public static class AppDataInit
{
    public static void SeedAppData(AppDbContext context)
    {
        SeedFolders(context);
        SeedProjectTypes(context);
        SeedProjectStatuses(context);
        SeedStepStatuses(context);
        SeedUserProjectRoles(context);
        
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

    public static void SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IConfiguration configuration)
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

        var adminEmail = configuration["ADMIN_EMAIL"];
        var adminPassword = configuration["ADMIN_PASSWORD"];
        var adminFirstName = configuration["ADMIN_FIRSTNAME"] ?? "Admin";
        var adminLastName = configuration["ADMIN_LASTNAME"] ?? "User";

        if (!string.IsNullOrEmpty(adminEmail) && !string.IsNullOrEmpty(adminPassword))
        {
            var admin = userManager.FindByEmailAsync(adminEmail).Result;
            if (admin == null)
            {
                admin = new AppUser()
                {
                    Id = Guid.NewGuid(),
                    Email = adminEmail,
                    EmailConfirmed = true,
                    UserName = adminEmail,
                    FirstName = adminFirstName,
                    LastName = adminLastName,
                };
                
                var result = userManager.CreateAsync(admin, adminPassword).Result;
                if (!result.Succeeded)
                {
                    Console.WriteLine("Failed to create admin user: " + string.Join(",", result.Errors.Select(e => e.Description)));
                    return;
                }
                var roleName = "admin";

                if (!userManager.IsInRoleAsync(admin, roleName).Result)
                {
                    userManager.AddToRoleAsync(admin, roleName).Wait();
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
        foreach (var (name, id) in InitialData.StepStatuses)
        {
            var stepStatus = new Domain.StepStatus()
            {
                Id = id ?? Guid.NewGuid(),
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
        foreach (var (name, id) in InitialData.ProjectTypes)
        {
            var projectType = new Domain.ProjectType()
            {
                Id = id ?? Guid.NewGuid(),
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
        foreach (var (name, id) in InitialData.ProjectStatuses)
        {
            var projectStatus = new Domain.ProjectStatus()
            {
                Id = id ?? Guid.NewGuid(),
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

    private static void SeedUserProjectRoles(AppDbContext context)
    {
        foreach (var (name, id) in InitialData.UserProjectRoles)
        {
            var userProjectRole = new Domain.UserProjectRole()
            {
                Id = id ?? Guid.NewGuid(),
                Name = name,
                CreatedBy = "system",
                CreatedAt = DateTime.UtcNow,
            };
            
            if (context.UserProjectRoles.Any(f => f.Name == name)) continue;
            
            var result = context.UserProjectRoles.AddAsync(userProjectRole).Result;
            
            if (result.State == EntityState.Added)
            {
                Console.WriteLine($"User project role {name} added to database.");
            }
            else
            {
                Console.WriteLine($"Failed to add User project role {name} into database.");
            }
        }
    }
}
