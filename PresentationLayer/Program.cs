using BLL.Service;
using BLL.Service.impl;
using DAL.DataContext;
using DAL.Model;
using DAL.Repository;
using DAL.Repository.impl;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IEntityRepository<Image>, ImageRepository>();
builder.Services.AddScoped<IEntityRepository<Pet>, PetRepository>();
builder.Services.AddScoped<IEntityRepository<Post>, PostRepository>();
builder.Services.AddScoped<IEntityRepository<User>, UserRepository>();

builder.Services.AddDbContext<FindYourPetContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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