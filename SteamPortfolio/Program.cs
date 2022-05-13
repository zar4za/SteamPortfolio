using Microsoft.AspNetCore.Authentication.Cookies;
using SteamPortfolio.Models;
using SteamPortfolio.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/signout";
})
.AddSteam(options =>
{
    options.ApplicationKey = "7B27A6889F463A40D55C992FE5045C7B";
});
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MongoDbRepositorySettings>(builder.Configuration.GetSection("MongoDbRepository"));
builder.Services.AddSingleton<IInventoryRepository, MongoDbRepository>();
builder.Services.AddSingleton<IMarketHashNameProvider, MockMarketHashNameProvider>();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseAuthentication();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapFallbackToFile("index.html"); ;
app.UseSwagger();
app.UseSwaggerUI();
app.Run();