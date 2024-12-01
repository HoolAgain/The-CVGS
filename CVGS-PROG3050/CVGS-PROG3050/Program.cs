using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Entities;
using CVGS_PROG3050.Services;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
string connStr = builder.Configuration.GetConnectionString("VaporMarketplace");

builder.Services.AddDbContext<VaporDbContext>(options => options.UseSqlServer(connStr));
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Edit this later for strong password ***
    /**
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;*/
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
})
        .AddEntityFrameworkStores<VaporDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddDNTCaptcha(options =>
{
    options.UseCookieStorageProvider(SameSiteMode.Strict)
           .ShowThousandsSeparators(false)
           .WithEncryptionKey("VaporIsBest");
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;
});

builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<WishlistService>();
builder.Services.AddScoped<ReviewService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");





var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{

    await VaporDbContext.CreateAdminUser(scope.ServiceProvider);
}

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var dbContext = services.GetRequiredService<VaporDbContext>();
//    await VaporDbContext.CreateAdminUser(services);
//}

app.Run();
