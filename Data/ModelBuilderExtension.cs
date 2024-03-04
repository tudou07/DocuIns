using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DocuIns.Models;

public static class ModelBuilderExtensions {
    public static void Seed(this ModelBuilder builder) {
        var pwd = "P@$$w0rd";
        var passwordHasher = new PasswordHasher<CustomUser>();

        // Seed Roles
        var adminRole = new CustomRole("Admin");
        adminRole.NormalizedName = adminRole.Name?.ToUpper();
        adminRole.CreatedDate = DateTime.Now;
        adminRole.Description = "Administrator role with full rights";

        var memberRole = new CustomRole("Member");
        memberRole.NormalizedName = memberRole.Name?.ToUpper();
        memberRole.CreatedDate = DateTime.Now;
        memberRole.Description = "Member role with limited rights";

        List<CustomRole> roles = new List<CustomRole>() {
           adminRole,
           memberRole
        };

        // roles are added to the database here
        builder.Entity<CustomRole>().HasData(roles);

         
        // Seed Users
        var adminUser = new CustomUser {
            UserName = "aa@aa.aa",
            Email = "aa@aa.aa",
            EmailConfirmed = true,
            FirstName = "Sahil",
            LastName = "Rai",
        };
        adminUser.NormalizedUserName = adminUser.UserName.ToUpper();
        adminUser.NormalizedEmail = adminUser.Email.ToUpper();
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, pwd);

        var memberUser = new CustomUser {
            UserName = "mm@mm.mm",
            Email = "mm@mm.mm",
            EmailConfirmed = true,
            FirstName = "John",
            LastName = "Smith",
        };
        memberUser.NormalizedUserName = memberUser.UserName.ToUpper();
        memberUser.NormalizedEmail = memberUser.Email.ToUpper();
        memberUser.PasswordHash = passwordHasher.HashPassword(memberUser, pwd);

        List<CustomUser> users = new List<CustomUser>() {
           adminUser,
           memberUser,
        };

        builder.Entity<CustomUser>().HasData(users);

        // Seed UserRoles
        List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();

        userRoles.Add(new IdentityUserRole<string> {
            UserId = users[0].Id,
            RoleId = roles.First(q => q.Name == "Admin").Id
        });

        userRoles.Add(new IdentityUserRole<string> {
            UserId = users[1].Id,
            RoleId = roles.First(q => q.Name == "Member").Id
        });


        builder.Entity<IdentityUserRole<string>>().HasData(userRoles);
    }
}