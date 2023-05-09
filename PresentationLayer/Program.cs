using System.Configuration;
using BLL.Service;
using BLL.Service.impl;
using DAL.DataContext;
using DAL.Init;
using DAL.Model;
using DAL.Repository;
using DAL.Repository.impl;
using EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.CustomTokenProviders;
using PresentationLayer.Mappings;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IPetPostImageService, PetPostImageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

builder.Services.AddAutoMapper(typeof(AppMappingProfile));
builder.Services.AddHttpContextAccessor();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//connectionString = connectionString!.Replace("1", builder.Configuration["1"]);

builder.Services.AddDefaultIdentity<User>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 3;
        options.Password.RequiredUniqueChars = 0;

        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = true;
        options.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
    })
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<FindYourPetContext>()
    .AddDefaultTokenProviders()
    .AddTokenProvider<EmailConfirmationTokenProvider<User>>("emailconfirmation");

var emailConfig = builder.Configuration.GetSection("EmailConfiguration")
  .Get<EmailConfiguration>();

builder.Services.AddSingleton(emailConfig);

builder.Services.Configure<PasswordHasherOptions>(options =>
    options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2);
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
   options.TokenLifespan = TimeSpan.FromHours(2));
builder.Services.Configure<EmailConfirmationTokenProviderOptions>(options =>
    options.TokenLifespan = TimeSpan.FromDays(3));

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/Login";
});

builder.Services.AddDbContext<FindYourPetContext>(options =>
    options.UseNpgsql(connectionString));

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
    await RoleInitializer.InitializeAsync(userManager, rolesManager, app.Configuration);
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();