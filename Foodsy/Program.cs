using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Foodsy.Data;
using Foodsy.Models;
using System;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IMenuService, MenuService>();


// Add DB context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add identity
builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// Register session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add authorization
builder.Services.AddAuthorization();

var app = builder.Build();

static async Task InitializeAdminRoleAndUser(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // Check if "Admin" role exists, and create if it doesn't
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // Check if the admin user already exists
    var adminUser = await userManager.FindByEmailAsync("admin@admin.com");
    if (adminUser == null)
    {
        // If the user does not exist, create a new one
        adminUser = new ApplicationUser
        {
            UserName = "admin@admin.com",
            Email = "admin@admin.com",
            FullName = "Admin User"
        };

        var userResult = await userManager.CreateAsync(adminUser, "AdminPassword123!"); // Set the password here
        if (userResult.Succeeded)
        {
            // After creating the user, assign the "Admin" role
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
        else
        {
            // Handle errors (you can log or throw exceptions based on your requirements)
            foreach (var error in userResult.Errors)
            {
                Console.WriteLine(error.Description);
            }
        }
    }
}

// Initialize admin role and user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        InitializeAdminRoleAndUser(services).Wait();
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred creating roles or users: " + ex.Message);
    }
}


app.UseRouting();


// Configure middleware to use authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

// Enable session middleware
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

