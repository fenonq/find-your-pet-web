using BLL.Service;
using BLL.Service.impl;
using DAL.DataContext;
using DAL.Repository;
using DAL.Repository.impl;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Mappings;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPetPostImageService, PetPostImageService>();

builder.Services.AddAutoMapper(typeof(AppMappingProfile));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
connectionString = connectionString!.Replace("DbPassword", builder.Configuration["DbPassword"]);

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();